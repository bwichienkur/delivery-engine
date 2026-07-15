# 19. Security Review

Severity uses: 🔴 High, 🟠 Medium, 🟡 Low. All findings reference source. This is a static review; a live pentest is recommended before modernization sign-off.

## Summary table

| # | Finding | Severity | Location |
|---|---------|----------|----------|
| 1 | TLS certificate validation globally disabled | 🔴 | `Helper/HTTPPost.cs:19-28,80-81` |
| 2 | Secrets/PII/internal hostnames committed to source | 🔴 | `Web.config`, `app.config`, transforms |
| 3 | WCF services have no authN/authZ; metadata published | 🔴 | `Web.config:126-138`, `app.config:125-152` |
| 4 | HubSpot API key committed in a test fixture | 🟠 | `UnitTest/.../DeliverInstantPostFixture.cs:39` |
| 5 | Dynamic assembly load + reflection (plugin) | 🟠 | `ConditionValidator/RemoteData.cs:56-77` |
| 6 | Potential SQL injection via dynamic SP names | 🟠 | `ConditionDAO.GetComparisonTable`, `DataTransformationDAO.GetStoredProcResult` |
| 7 | PII in leads/logs; free-text logging | 🟠 | `WriteToDELog`, EntLib listeners |
| 8 | Windows Service runs as LocalSystem | 🟠 | `ProjectInstaller.Designer.cs:36` |
| 9 | No rate limiting / no inbound throttling | 🟡 | services + poll loop |
| 10 | XSLT processing of DB-authored stylesheets | 🟡 | `ApplyXsltForXml.cs` |
| 11 | Publish encryption key in `.csproj` | 🟡 | `*.csproj` `DeployEncryptKey` |
| 12 | ClickOnce temporary signing key committed | 🟡 | `WindowsService_TemporaryKey.pfx` |

---

## 1 🔴 TLS validation disabled globally
`TrustAllCert.OnValidationCallback` returns `true` and is assigned to `ServicePointManager.ServerCertificateValidationCallback` on every POST (`Helper/HTTPPost.cs:25-28,80-81`). This disables certificate validation **for the entire AppDomain**, enabling MITM on all HTTPS (including partner posts and any other outbound TLS in the process).
**Fix:** remove the callback; validate certificates; if a specific partner needs a custom trust, scope it per-request (not global) and pin explicitly.

## 2 🔴 Committed secrets, PII, and infrastructure
- SMTP servers/IPs: `gwsmtp.usa.net`, `165.212.65.102` (`Web.config:88,95,13`).
- Alert/PII emails: `rkamenetskiy@EducationDynamics.com`, `sgupte@educationDynamics.com`, `CheetahAlert@EducationDynamics.com` (`Web.config:13,99,101`).
- Internal hostnames: `cycle-sql1`, `cycle-issvc1`, `*.eddycorp.local`, `isdb.eddyprod.local` (connection strings + `Master.config`).
- `MachineKey` GUID `87133C9B-...` (`Web.config:93`).
**Fix:** move secrets/hosts to a secret store or environment-injected config; scrub history; rotate anything still live. (No SQL passwords — Integrated Security is used, which is a plus.)

## 3 🔴 No service auth; metadata exposed
WCF services use `basicHttpBinding` security `None` with no credentials/authorization, and `serviceMetadata httpGetEnabled="true"` publishes WSDL (`Web.config:131`). Anyone with network access to IIS can invoke `ProcessLeads`/`processBatch`/etc.
**Fix:** enable transport security (HTTPS + Windows/cert auth) or front with an authenticated gateway; set `httpGetEnabled="false"` in production; restrict network access.

## 4 🟠 Third-party API key in test fixture
`DeliverInstantPostFixture.cs:39` embeds a HubSpot `hapikey=25fe0d45-...` in test XML.
**Fix:** remove and rotate the key; use fake values in tests.

## 5 🟠 Dynamic assembly load + reflection
`RemoteData` loads `EDDY.Nexus.BusinessComponent.dll` from `ReferenceLibraryPath` and instantiates types by name (`:56-77`). If the path or DLL is writable by a lower-privileged process, this is a code-execution vector.
**Fix:** load from a fixed, ACL'd path; verify assembly strong-name/signature; constrain resolvable types.

## 6 🟠 Dynamic stored-procedure names
`ConditionDAO.GetComparisonTable` and `DataTransformationDAO.GetStoredProcResult` execute SP names sourced from DB-stored XML/config. If that configuration is attacker-influenced, arbitrary procedures could be executed. Parameters are passed as SP params (parameterized), so classic value injection is mitigated, but the **procedure name itself** is dynamic.
**Fix:** whitelist allowed SP names; validate against a known set.

## 7 🟠 PII handling & logging
Leads contain PII (name, email, phone, program interest). Logging is free-text (`WriteToDELog` truncates messages; EntLib DB/flat-file/email listeners). Payloads and emails may capture PII.
**Fix:** classify PII, mask in logs, set retention on `EddyLogging`, ensure encrypted transport for all deliveries.

## 8 🟠 LocalSystem service account
The Windows Service installs as **LocalSystem** (`ProjectInstaller.Designer.cs:36`) — excessive privilege.
**Fix:** run under a least-privilege domain service account with only the needed DB and file-share rights.

## 9 🟡 No rate limiting
No inbound throttling on WCF services; the poll loop batch size is config-driven. A flood of leads or a hostile caller could overload partners or the DB.
**Fix:** add concurrency/rate controls; consider circuit breakers on partner endpoints.

## 10 🟡 XSLT execution
`ApplyXsltForXml` runs DB-authored XSLT via temp files. Malicious XSLT (`document()`, scripting) could read files or execute code depending on settings.
**Fix:** disable XSLT scripting/`document()`resolution; validate stylesheet source; use `XsltSettings.Default`.

## 11 🟡 Publish encryption key in csproj
`DeployEncryptKey` = URL-encoded `Ch$$TAH!` in multiple `.csproj` files. Low impact (publish-time), but a committed shared secret.

## 12 🟡 Committed temporary signing key
`EDDY.IS.DeliveryEngine.WindowsService_TemporaryKey.pfx` (ClickOnce) is in source control.

## Not found (good)
No SQL passwords in connection strings (Integrated Security). WCF fault exception detail is disabled (`includeExceptionDetailInFaults="false"`). No obvious open-redirect/CSRF/XSS surface (no web UI in this repo). No SSRF beyond the intended outbound POST to configured partner URLs (but note: URLs come from DB config — validate them).

## Prioritized recommendations
1. Remove global TLS trust (#1) — immediate.
2. Rotate + externalize secrets/keys (#2,#4,#11,#12); scrub history.
3. Add transport security + auth to WCF; disable metadata in prod (#3).
4. Whitelist dynamic SP/plugin/XSLT sources (#5,#6,#10).
5. Least-privilege service account (#8); PII masking + retention (#7).

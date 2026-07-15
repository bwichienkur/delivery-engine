# 30. Refactoring Recommendations (Ranked)

Ranked by **impact** (risk reduction + enabling modernization), balanced against effort. Effort is expressed in technical scope, not calendar time. Each item lists Problem, Risk, Effort, Business impact, Technical impact, Suggested implementation, Priority.

Priority key: **P0** critical/urgent, **P1** high, **P2** medium, **P3** opportunistic.

---

## R1 — Remove global TLS trust-all (P0)
- **Problem:** `HTTPPost` disables certificate validation AppDomain-wide (`Helper/HTTPPost.cs:19-28,80-81`).
- **Risk of leaving:** MITM on all outbound HTTPS, including lead PII to partners.
- **Effort:** Small (delete callback; targeted fix).
- **Business impact:** Data-protection/compliance risk removed.
- **Technical impact:** Restores TLS integrity for the whole process.
- **Implementation:** Remove the `ServicePointManager` assignment; rely on OS trust; if a specific partner truly needs custom trust, scope validation per-request and pin.
- **Priority:** **P0**.

## R2 — Externalize & rotate secrets; scrub artifacts (P0)
- **Problem:** SMTP hosts/IPs, PII emails, internal hostnames, a HubSpot API key, publish key, and a `.pfx` are committed; `bin/obj` are in source control.
- **Risk:** Credential/infra disclosure; key abuse.
- **Effort:** Medium (config refactor + history scrub + `.gitignore`).
- **Business impact:** Reduces breach surface.
- **Technical impact:** Cleaner repo; env-injected config.
- **Implementation:** Move secrets to a secret store / env vars; add `.gitignore` for `bin/obj/*.pfx`; rotate keys; purge from history.
- **Priority:** **P0**.

## R3 — Secure the WCF services (P1)
- **Problem:** `basicHttpBinding` security `None`, `httpGetEnabled="true"`, no auth (`Web.config:126-138`).
- **Risk:** Unauthenticated invocation of delivery/batch operations.
- **Effort:** Medium.
- **Implementation:** HTTPS + Windows/cert auth (or authenticated gateway); `httpGetEnabled="false"` in prod; network ACLs.
- **Priority:** **P1**.

## R4 — Version the database schema & SP logic (P1)
- **Problem:** ~110+ SPs and all tables live only in SQL Server; no schema in repo. The real business logic is invisible/untestable here.
- **Risk:** Cannot safely change or migrate; knowledge loss.
- **Effort:** Medium-Large.
- **Implementation:** Extract schema + `sys.sql_modules` into an SSDT `.sqlproj` (or DbUp scripts) committed alongside code; add golden-master tests for key SPs (see [../Testing.md](../Testing.md)).
- **Priority:** **P1**.

## R5 — Remove dead/forked code & config drift (P1)
- **Problem:** ~51 uncompiled activity files; duplicate `FieldToCompare`; unused preview client/self-host; `Test` harness not in sln with broken paths; conflicting `Web.Release.config`; installer name mismatch.
- **Risk:** Confusion, wrong-file edits, deploy surprises.
- **Effort:** Small-Medium.
- **Implementation:** Delete dead files; reconcile transforms; align service names; either fix or archive the `Test` project.
- **Priority:** **P1**.

## R6 — Resolve stubbed features (capping/scoring/RDQ) (P1)
- **Problem:** Capping and scoring are silently disabled (`SeLeadProcessingConfigurationValues.cs:23-24`, `GetCap.cs`, `LeadScoringBusinessComponent`); `GetRDQsForProcessing` returns empty.
- **Risk:** Business believes caps/scoring apply when they don't; over-delivery / contract breaches.
- **Effort:** Medium (decide + implement or formally remove).
- **Implementation:** Confirm intended behavior with product; either implement (honoring `Web.config` flags) or delete and document.
- **Priority:** **P1** (business-correctness).

## R7 — Introduce DI + fix concurrency model (P2)
- **Problem:** Static locators/`new`; `Parallel.ForEach` negated by `lock`; fully synchronous I/O; fixed 5s poll.
- **Risk:** Poor testability & throughput.
- **Effort:** Medium-Large.
- **Implementation:** `Microsoft.Extensions.DependencyInjection`; inject `I*DAO`/transports; convert I/O to async; bounded concurrency (`SemaphoreSlim`); configurable poll/backoff; typed `HttpClient` + Polly.
- **Priority:** **P2**.

## R8 — Decompose god classes (P2)
- **Problem:** `DeliveryEngineDAO`, `ConditionBC`, `DataTransformationBC`, `DeliveryEngineBC` are oversized.
- **Effort:** Medium (mechanical but risky without tests → do after R4/Testing).
- **Implementation:** Split DAO by aggregate; turn condition operators into a Strategy set; separate DT orchestration from task creation; collapse BC boilerplate.
- **Priority:** **P2**.

## R9 — Add resilience & caching (P2)
- **Problem:** No retry policy abstraction, no circuit breakers, no caching of definitions/endpoints/plugins.
- **Effort:** Medium.
- **Implementation:** Polly policies on transports; cache config with invalidation; cache reflected plugin types.
- **Priority:** **P2**.

## R10 — Modernize off WF4/WCF/EntLib (P2, strategic)
- **Problem:** WF4, WCF Workflow Services, and Enterprise Library are unsupported on modern .NET; blocks .NET 8+ migration and containerization.
- **Risk:** Growing platform risk; hiring/support difficulty.
- **Effort:** Large.
- **Implementation (incremental):**
  1. Re-express workflows as explicit C# orchestrators / a small state machine (testable).
  2. Replace WCF with a modern transport (HTTP/gRPC/queue) behind the existing `WindowsService↔Service` seam (the cleanest boundary).
  3. Swap EntLib Logging/Exception for `Microsoft.Extensions.Logging` (+ Serilog) and standard try/catch/middleware.
  4. Swap EntLib Data for `Microsoft.Data.SqlClient` (+ Dapper) or EF Core over the (now-versioned) schema.
  5. Containerize once off WF4/WCF.
- **Priority:** **P2** strategic (sequence after R1–R6 stabilization).

## R11 — Observability & structured logging (P3)
- **Problem:** Free-text logs, no correlation IDs, no metrics/telemetry.
- **Effort:** Medium.
- **Implementation:** Structured logging with correlation (LeadId/BatchId), metrics on delivery success/latency/retries, tracing.
- **Priority:** **P3**.

## R12 — Test coverage & CI (P3, enabler)
- **Problem:** Minimal, brittle, DB-coupled tests; no CI.
- **Effort:** Medium (ongoing).
- **Implementation:** See [../Testing.md](../Testing.md); add CI to run unit tests + build on every push.
- **Priority:** **P3** (do alongside R4/R8 as an enabler).

---

## Suggested sequence

```mermaid
flowchart LR
    R1[R1 TLS] --> R2[R2 Secrets]
    R2 --> R3[R3 WCF auth]
    R3 --> R5[R5 Dead code]
    R5 --> R6[R6 Stub decisions]
    R6 --> R4[R4 Schema in repo]
    R4 --> R12[R12 Tests+CI]
    R12 --> R8[R8 Decompose]
    R8 --> R7[R7 DI+async]
    R7 --> R9[R9 Resilience]
    R9 --> R10[R10 Re-platform]
    R10 --> R11[R11 Observability]
```

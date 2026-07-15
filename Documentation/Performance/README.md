# 20. Performance Review

Static analysis; no profiling data available. Confidence noted per item.

## Hot paths
1. **Poll → dispatch loop** (`WorkflowLauncherService.ProcessDeliveryWorkflow`) — runs every ~5s continuously.
2. **Per-lead delivery** (`InstantDeliveryWF` → `RealtimeEndpointWF` → `InstantPostWF`) — one per lead per endpoint; includes DB reads, transformation, network I/O.
3. **Transformation pipeline** (`DataTransformationBC.Transform`) — per endpoint per lead.
4. **Batch file build** (`CreateBatchLeadFile`) — O(leads) with string building + file I/O.

## Concurrency & async
| Issue | Detail | Confidence |
|-------|--------|-----------|
| `Parallel.ForEach` negated by `lock(Locker)` | WCF calls serialized despite parallel loop (`:302-306,343-347`) | Very high |
| Fully synchronous I/O | DB, HTTP, SMTP, FTP all blocking; no `async/await` | High |
| Fixed 5s sleep | Adds latency floor; no adaptive backpressure | Very high |
| Single worker thread | One poll thread; parallelism only within a batch (then serialized) | High |

**Impact:** effective delivery throughput is roughly serial per host; horizontal scale relies on multiple machines partitioned by `DeliveryEngineMachineKey`.

## Database
| Issue | Detail |
|-------|--------|
| SP-centric | Query plans/indexes live in SQL Server (not in repo) — can't assess N+1 from C# alone |
| Potential N+1 | Per-endpoint DAO calls inside per-lead loops (e.g., `GetDeliveryEndpoint`, transformation task loads, `CreateLog`) | 
| `new DAO()` per call | Minor; ADO.NET connection pooling mitigates |
| `SQLCommandTimeOut=120` | Long timeout can mask slow procs |
| Command reuse | Each activity opens its own command per SP |

**Recommendation:** capture actual execution stats from SQL Server (`sys.dm_exec_query_stats`, missing-index DMVs) — the real perf story is in the procedures.

## Allocations / memory
| Issue | Detail |
|-------|--------|
| String building in loops | `CreateBatchLeadFile` / `CreatePostData` build large strings; prefer `StringBuilder` (partly used) |
| XSLT temp files | `ApplyXsltForXml` writes to `C:\DeliveryTransformationTemp\DT_Temp` per transform with 3 retries + `Thread.Sleep(1000)` — disk I/O + latency on contention |
| Dictionaries copied | Transformation copies name/value dictionaries between tasks |
| Reflected plugin loads | `Assembly.LoadFile` per remote condition (no caching) |

## Blocking calls
- `Thread.Sleep(5000)` poll; `Thread.Sleep(1000)` XSLT retry; `_worker.Join()` on stop; synchronous WCF/HTTP/SMTP/FTP.

## Caching opportunities
- **Delivery definitions / endpoints / transformation tasks** are read repeatedly and change infrequently → cache with invalidation.
- **Reflected condition plugin types** → cache `Type`/instances.
- **Status/lookup tables** → cache.
- Currently **no caching layer exists**.

## Parallelization opportunities
- Replace `Parallel.ForEach` + `lock` with bounded async concurrency (`SemaphoreSlim` + `Task.WhenAll`) once WCF/HTTP go async.
- Batch DB reads (fetch endpoints/tasks for a definition once) to cut per-endpoint round trips.

## Quick wins (low risk)
1. Make poll cadence configurable; reduce idle latency.
2. Cache definition/endpoint/transformation config.
3. Remove the `lock` and use bounded concurrency.
4. Cache reflected plugin types.
5. Profile the top SPs and add missing indexes.

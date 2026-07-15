# 8. API Documentation

This system has **no REST/HTTP JSON API and no MVC/WebAPI controllers**. Its "API" surface is a set of **WCF Workflow Services** (`basicHttpBinding`) exposed by the IIS-hosted `EDDY.IS.DeliveryEngine.Workflow.Service` project, plus the outbound HTTP/SMTP/FTP it performs to partners (documented in [../Services/Integrations.md](../Services/Integrations.md)).

- WCF operations: [WorkflowServices.md](./WorkflowServices.md)
- Outbound partner integrations: [../Services/Integrations.md](../Services/Integrations.md)

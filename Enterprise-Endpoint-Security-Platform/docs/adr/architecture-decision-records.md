# DOC-016 â€” Architecture Decision Records



**Document Version:** v0.1  

**Phase:** G8 â€” Architecture Decision Records  

**Status:** Draft â€” Baseline Pending Review  

**Date:** 08-10-2026  

**Product:** Enterprise Endpoint Security \& Zero Trust Platform



---



## 1. Purpose



This document records the major architectural decisions for the Enterprise Endpoint Security \& Zero Trust Platform.



The purpose of these Architecture Decision Records (ADRs) is to:



- capture important architectural choices;

- document the reasoning behind those choices;

- record alternatives considered;

- maintain consistency between requirements, architecture and implementation;

- prevent accidental architectural drift during development;

- provide a clear technical rationale for future maintenance and review.



These decisions complement:



- DOC-002 â€” Product Scope;

- DOC-005 â€” Functional Requirements;

- DOC-006 â€” Non-Functional Requirements;

- DOC-007 â€” Security Requirements;

- DOC-009 â€” Requirements Traceability;

- DOC-013 â€” Threat Model;

- DOC-014 â€” System Architecture;

- DOC-015 â€” Security Architecture.



---



## 2. ADR Format



Each decision contains:



- **Status** â€” current state of the decision;

- **Context** â€” why the decision is required;

- **Decision** â€” selected architectural approach;

- **Alternatives Considered** â€” significant alternatives evaluated;

- **Rationale** â€” reasons for the selected approach;

- **Consequences** â€” expected benefits, trade-offs and constraints.



Unless explicitly superseded, these decisions are considered the architectural baseline for implementation.



---



# ADR-001 â€” Use a Modular Monolith for the Initial Backend



**Status:** Accepted



## Context



The platform requires multiple backend capabilities including:



- identity and access management;

- device management;

- security policy management;

- Zero Trust evaluation;

- endpoint controls;

- telemetry;

- audit;

- health and operational monitoring.



The initial product requires clear separation of responsibilities while avoiding unnecessary distributed-system complexity.



## Decision



The backend will initially be implemented as a **modular monolith** using separate .NET projects and clear application/domain boundaries.



The backend will contain:



- `SecurityPlatform.Api`

- `SecurityPlatform.Application`

- `SecurityPlatform.Domain`

- `SecurityPlatform.Infrastructure`

- `SecurityPlatform.Contracts`



Modules will be separated logically within the application and through explicit interfaces and responsibilities.



## Alternatives Considered



### Microservices



Rejected for the initial implementation because it would introduce additional operational complexity including:



- service-to-service communication;

- distributed authentication;

- deployment complexity;

- distributed tracing requirements;

- additional failure modes;

- increased infrastructure requirements.



### Single-layer monolithic application



Rejected because it would provide insufficient separation of domain, application, infrastructure and API responsibilities.



## Rationale



A modular monolith provides:



- strong separation of concerns;

- simpler deployment;

- simpler local development;

- easier testing;

- lower operational overhead;

- a future path toward service extraction if justified by actual scale.



## Consequences



### Positive



- Faster initial development.

- Lower infrastructure complexity.

- Clear project boundaries.

- Easier debugging and testing.



### Negative



- Requires discipline to prevent unwanted coupling.

- Independent scaling of modules is not initially available.



---



# ADR-002 â€” Use ASP.NET Core and .NET for the Platform Backend



**Status:** Accepted



## Context



The platform requires a secure backend API capable of supporting authentication, authorization, device management, policy management, telemetry and administrative operations.



The project also requires strong integration with the Windows endpoint agent.



## Decision



The platform backend will use:



- C#;

- ASP.NET Core;

- the current supported .NET platform selected during implementation;

- RESTful HTTP APIs.



The backend will follow the layered project structure defined in DOC-014.



## Alternatives Considered



### Node.js / TypeScript backend



Not selected because the platform has strong .NET and Windows integration requirements.



### Java / Spring Boot



Not selected because it would introduce an additional primary backend technology without providing a sufficient benefit for this project.



## Rationale



ASP.NET Core provides:



- strong C# ecosystem support;

- mature authentication and authorization capabilities;

- strong Windows integration;

- high-performance HTTP APIs;

- dependency injection;

- middleware;

- structured configuration;

- strong automated testing support.



## Consequences



The backend team must maintain secure .NET dependency versions and apply security updates throughout development.



---



# ADR-003 â€” Separate Operational Data and Telemetry Data



**Status:** Accepted



## Context



The platform contains two fundamentally different categories of data.



Operational data includes:



- users;

- roles;

- permissions;

- devices;

- policies;

- policy assignments;

- administrative configuration.



Telemetry data includes:



- endpoint events;

- security events;

- health information;

- endpoint activity;

- investigation-related events.



These workloads have different storage and query characteristics.



## Decision



The platform will use:



- **MySQL** for structured operational data;

- **MongoDB** for telemetry and event-oriented data.



The dashboard and agent will not directly access either database.



All access will occur through the backend platform.



## Alternatives Considered



### MySQL only



Rejected because high-volume and flexible telemetry/event data would create unnecessary pressure on the operational relational model.



### MongoDB only



Rejected because identity, authorization, policy and configuration data benefit from relational structure and transactional consistency.



### Additional event-streaming platform



Deferred until actual throughput requirements justify the operational complexity.



## Rationale



The separation allows each data store to serve its appropriate workload while preserving a single controlled backend boundary.



## Consequences



### Positive



- Clear data ownership.

- Appropriate storage models.

- Reduced coupling between telemetry and operational data.

- Easier future optimization.



### Negative



- Two database technologies must be operated and secured.

- Cross-store reporting requires deliberate application-level handling.



---



# ADR-004 â€” Use a Windows Background Agent for Endpoint Security



**Status:** Accepted



## Context



The initial product targets Windows endpoints.



Endpoint capabilities require background execution for:



- device identity;

- posture collection;

- policy evaluation;

- endpoint controls;

- telemetry collection;

- health and heartbeat;

- secure communication with the platform.



The agent must operate without requiring an interactive user interface.



## Decision



The endpoint component will be implemented as a **Windows Worker/Service-style background agent** using .NET.



The agent will contain separate responsibilities for:



- device identity;

- posture collection;

- policy management;

- control management;

- telemetry collection;

- health and heartbeat;

- secure platform communication.



## Alternatives Considered



### Desktop application



Rejected because endpoint security functions should not depend on an interactive user session.



### Browser-based agent



Rejected because browser execution cannot provide the required endpoint-level control capabilities.



### Kernel driver



Not selected for the initial implementation because the defined platform scope does not require kernel-level functionality.



## Rationale



A Windows background service provides persistent execution while remaining aligned with the initial Windows-only product scope.



## Consequences



The agent must be designed carefully around:



- Windows service lifecycle;

- least privilege;

- secure local configuration;

- credential protection;

- failure recovery;

- update and deployment mechanisms.



---



# ADR-005 â€” Use React and TypeScript for the Security Dashboard



**Status:** Accepted



## Context



Security administrators and analysts require a web-based interface for:



- authentication;

- device inventory;

- policy management;

- Zero Trust decisions;

- endpoint controls;

- telemetry investigation;

- audit review;

- health monitoring.



## Decision



The dashboard will use:



- React;

- TypeScript;

- a component-based UI architecture;

- a centralized API client;

- protected routes;

- centralized session/authentication handling.



## Alternatives Considered



### Server-rendered MVC UI



Rejected because the platform requires a highly interactive administrative dashboard.



### Angular



Technically viable but not selected because React/TypeScript provides a suitable component ecosystem and aligns with the project's existing frontend direction.



## Rationale



React and TypeScript provide:



- reusable components;

- strong typing;

- maintainable UI structure;

- suitable support for interactive security dashboards;

- clean separation between frontend and backend APIs.



## Consequences



The dashboard must enforce client-side usability controls while treating the backend as the authoritative security boundary.



---



# ADR-006 â€” Use Token-Based Authentication with Short-Lived Access Tokens



**Status:** Accepted



## Context



The platform requires authenticated access for:



- dashboard users;

- protected APIs;

- administrative operations;

- endpoint communication.



The architecture also requires explicit authorization and secure session handling.



## Decision



The platform will use token-based authentication with:



- short-lived access tokens;

- refresh-token support where required;

- secure token validation;

- server-side authorization;

- explicit authentication failure handling.



Authentication and authorization will remain separate concerns.



## Alternatives Considered



### Long-lived access tokens



Rejected because compromise of a long-lived credential increases the security impact of token theft.



### Session-only server authentication



Not selected as the primary architecture because the platform includes independently communicating endpoint agents.



## Rationale



Short-lived access tokens reduce the useful lifetime of compromised access credentials while supporting stateless API authentication.



## Consequences



The implementation must address:



- token expiry;

- refresh-token protection;

- revocation;

- secure storage;

- signing-key protection;

- clock and validation considerations.



---



# ADR-007 â€” Use RBAC with Claims and Policy-Based Authorization



**Status:** Accepted



## Context



The platform has multiple administrative roles:



- Platform Admin;

- Security Admin;

- Security Analyst;

- Auditor / Read-Only.



Different users must receive different permissions, and authorization must be enforced by the platform rather than trusted to the dashboard.



## Decision



Authorization will use:



- role-based access control;

- claims where appropriate;

- policy-based authorization;

- server-side enforcement;

- least privilege;

- deny-by-default behavior.



Resource-level authorization will be applied where role membership alone is insufficient.



## Alternatives Considered



### Role checks only



Rejected because simple role checks are insufficient for resource-level and contextual authorization.



### Client-side authorization



Rejected because client-side checks cannot provide a trustworthy security boundary.



### Attribute-based authorization as the primary model



Deferred because the initial platform requirements can be satisfied with RBAC combined with claims and policies.



## Rationale



RBAC provides the baseline access model while claims and policies allow more precise authorization decisions.



## Consequences



Authorization policies must be centralized, tested and audited.



---



# ADR-008 â€” Use HTTPS REST APIs for Platform Communication



**Status:** Accepted



## Context



The dashboard and endpoint agent require controlled communication with the central platform.



Communication includes:



- authentication;

- device registration;

- heartbeat;

- policy retrieval;

- control commands;

- telemetry submission;

- administrative operations.



## Decision



The platform will use authenticated HTTPS-based REST APIs for primary communication.



The architecture will enforce:



- TLS-protected communication;

- authentication;

- authorization;

- request validation;

- secure error handling;

- replay-aware protections where applicable;

- rate limiting where required by the endpoint.



The agent will communicate through defined backend API boundaries rather than directly accessing platform databases.



## Alternatives Considered



### Direct database communication



Rejected because it would bypass the application security boundary.



### Unencrypted HTTP



Rejected because sensitive security and identity data must not traverse the network without transport protection.



### Message broker as the primary communication mechanism



Deferred because the initial product does not require the operational complexity of a broker for all communication.



## Rationale



HTTPS REST APIs provide a clear and testable communication boundary suitable for the initial platform.



## Consequences



Future high-volume or asynchronous workloads may introduce specialized messaging components if justified by measurable requirements.



---



# ADR-009 â€” Keep Deployment Container-Compatible



**Status:** Accepted



## Context



The central platform should support reproducible development and deployment environments.



The architecture includes:



- ASP.NET Core backend;

- MySQL;

- MongoDB;

- React dashboard.



The endpoint agent remains Windows-specific.



## Decision



The central platform components will be designed to support containerized deployment.



Docker-compatible deployment artifacts will be maintained under:



```text

deploy/docker/

deploy/compose/

deploy/kubernetes/

```



The Windows endpoint agent will remain a native Windows service/worker and will not be containerized as part of the initial platform.



## Alternatives Considered



### VM-only deployment



Rejected because it provides less consistency between development, testing and deployment environments.



### Kubernetes-first implementation



Deferred because the initial platform does not require Kubernetes operational complexity.



## Rationale



Container compatibility provides deployment portability while allowing the project to remain operationally simple during initial development.



## Consequences



The application must avoid assumptions that depend on a specific host filesystem or environment.



Environment-specific configuration must be externalized.



---



# ADR-010 â€” Use OpenTelemetry-Compatible Observability



**Status:** Accepted



## Context



The platform requires operational visibility across:



- backend APIs;

- endpoint agents;

- database interactions;

- authentication failures;

- policy operations;

- telemetry processing;

- system health.



Security-relevant and operational events must be distinguishable and traceable.



## Decision



The platform will follow an OpenTelemetry-compatible observability model.



The implementation will support structured:



- logs;

- metrics;

- traces where appropriate;

- health information.



Observability data must not expose unnecessary sensitive information.



## Alternatives Considered



### Application-specific logging only



Rejected because it limits interoperability and makes future observability integration more difficult.



### Full external SIEM integration from the beginning



Deferred because the initial product scope does not require building a complete SIEM integration platform.



## Rationale



An OpenTelemetry-compatible approach provides a standard foundation without forcing a specific observability vendor or infrastructure during initial development.



## Consequences



The implementation must define:



- consistent event fields;

- correlation identifiers;

- log severity;

- sensitive-data handling;

- health endpoints;

- telemetry retention considerations.



---



# 3. Decision Summary



| ADR | Decision | Status |

|---|---|---|

| ADR-001 | Modular monolith backend | Accepted |

| ADR-002 | ASP.NET Core / .NET backend | Accepted |

| ADR-003 | MySQL + MongoDB data separation | Accepted |

| ADR-004 | Windows background endpoint agent | Accepted |

| ADR-005 | React + TypeScript dashboard | Accepted |

| ADR-006 | Short-lived token-based authentication | Accepted |

| ADR-007 | RBAC + claims/policy authorization | Accepted |

| ADR-008 | HTTPS REST API communication | Accepted |

| ADR-009 | Container-compatible central platform | Accepted |

| ADR-010 | OpenTelemetry-compatible observability | Accepted |



---



# 4. Architecture Alignment



These decisions support the architecture defined in DOC-014 and DOC-015.



The decisions preserve the following architectural principles:



- Zero Trust;

- least privilege;

- explicit authentication and authorization;

- secure-by-default behavior;

- defense in depth;

- separation of responsibilities;

- controlled communication boundaries;

- protection of operational and telemetry data;

- testability;

- deployment portability;

- avoidance of unnecessary distributed-system complexity.



---



# 5. Decision Review and Change Control



An ADR may be changed when:



- a requirement changes;

- an architectural constraint changes;

- implementation evidence invalidates the decision;

- measurable scale or operational requirements justify a different approach;

- a security review identifies a material weakness.



Existing accepted decisions must not be silently changed.



When an accepted decision is replaced, the replacement decision must:



1\. identify the superseded ADR;

2\. explain why the previous decision is no longer appropriate;

3\. document the new alternatives and rationale;

4\. update affected architecture and implementation documentation.



---



# 6. Document Status



**Status:** Reviewed â€” Baseline v0.1



**Phase:** G8 â€” Architecture Decision Records



**Decision State:** Architecture baseline approved for implementation



**Next Phase:** G9 â€” Repository Engineering Baseline



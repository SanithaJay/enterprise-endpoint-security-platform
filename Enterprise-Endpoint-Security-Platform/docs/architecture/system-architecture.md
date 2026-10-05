# DOC-014 — System Architecture

**Document Version:** v0.1  
**Phase:** G5 — System Architecture  
**Status:** Reviewed — Baseline v0.1  
**Date:** 05-10-2026  
**Product:** Enterprise Endpoint Security & Zero Trust Platform  

---

## 1. Purpose

This document defines the system architecture for the Enterprise Endpoint Security & Zero Trust Platform.

It translates the approved product scope, requirements, backlog and threat model into an implementation-oriented architecture.

The architecture establishes:

- system boundaries
- major components
- component responsibilities
- communication paths
- API boundaries
- authentication and authorization flows
- endpoint agent architecture
- dashboard architecture
- data architecture
- policy and telemetry flows
- Zero Trust decision flow
- deployment topology
- technology choices
- architecture constraints
- security considerations derived from the threat model

This document is the architectural baseline for subsequent implementation work.

---

## 2. Architecture Drivers

The architecture is primarily driven by the following requirements:

1. Windows endpoint security and management
2. Centralized device identity and posture
3. Strong authentication and authorization
4. Role-based access control
5. Security policy management
6. Zero Trust evaluation
7. Endpoint security controls
8. Security telemetry collection
9. Auditability and accountability
10. Secure agent-to-platform communication
11. Separation of structured operational data from telemetry/event data
12. Maintainability and testability
13. Secure-by-default behavior
14. Production-oriented deployment and observability

The architecture must remain aligned with DOC-002 Product Scope, DOC-005 Functional Requirements, DOC-006 Non-Functional Requirements, DOC-007 Security Requirements, DOC-008 Acceptance Criteria, DOC-009 Requirements Traceability and DOC-013 Threat Model.

---

## 3. System Context

The platform consists of three primary application surfaces:

1. Security Administration Dashboard
2. Central Security Platform
3. Windows Endpoint Agent

The high-level interaction is:

```text
+------------------------+
| Security Administrator |
| Security Analyst       |
| Auditor                |
+-----------+------------+
            |
            | HTTPS
            v
+------------------------+
| React / TypeScript     |
| Security Dashboard     |
+-----------+------------+
            |
            | HTTPS / REST
            v
+------------------------------------------------+
|              Security Platform                 |
|                                                |
|  ASP.NET Core API                              |
|  Application Layer                             |
|  Domain Layer                                  |
|  Infrastructure Layer                          |
|  Contracts                                     |
+------------+-------------------+---------------+
             |                   |
             |                   |
             v                   v
       +-----------+       +------------+
       |   MySQL   |       |  MongoDB   |
       | Operational|       | Telemetry  |
       |   Data    |       | / Events   |
       +-----------+       +------------+
             ^
             |
             | Secure agent communication
             |
+------------+-------------------------------+
|             Windows Endpoint                |
|                                             |
|  SecurityPlatform.Agent                     |
|  - Device identity                          |
|  - Posture collection                       |
|  - Policy retrieval                         |
|  - Endpoint controls                        |
|  - Telemetry collection                     |
|  - Heartbeat / health                       |
+---------------------------------------------+

## 4. Architecture Principles
	### 4.1 Security by Design
		Security requirements are architectural constraints rather than optional features.
	### 4.2 Least Privilege
		Users, services and agents receive only the permissions required for their responsibilities.
	### 4.3 Zero Trust
		The platform does not automatically trust a user, device or request based solely on network location.
	### 4.4 Separation of Responsibilities
		Domain logic, application orchestration, infrastructure access and API concerns remain separated.
	### 4.5 Explicit Trust Boundaries
		Communication crossing security boundaries must be authenticated, authorized and validated.
	### 4.6 Defense in Depth
		Security controls are applied across identity, API, data, endpoint, communication and audit layers.
	### 4.7 Centralized Policy, Local Enforcement
		Security policies are centrally managed while endpoint-specific controls are enforced by the Windows agent.
	### 4.8 Secure Failure
		Security-sensitive operations should fail closed or into a restricted state where practical.
	### 4.9 Observability
		Security-relevant operations must provide sufficient telemetry, health information and audit evidence.
	### 4.10 Testability
		Components must remain independently testable wherever practical.
## 5. Logical Architecture
	The platform follows a layered backend architecture with a separate Windows endpoint agent and React dashboard.

+------------------------------------------------------+
|              React / TypeScript Dashboard            |
|                                                      |
| Authentication | Devices | Policies | Zero Trust     |
| Controls | Telemetry | Audit | Health                |
+----------------------------+-------------------------+
                             |
                             | HTTPS / REST
                             v
+------------------------------------------------------+
|                SecurityPlatform.Api                  |
|                                                      |
| Controllers | Middleware | Authentication            |
| Authorization | Validation | API Responses           |
+----------------------------+-------------------------+
                             |
                             v
+------------------------------------------------------+
|             SecurityPlatform.Application             |
|                                                      |
| Use Cases | Commands | Queries | Services            |
| Validation | Authorization | Orchestration           |
+----------------------------+-------------------------+
                             |
                             v
+------------------------------------------------------+
|                SecurityPlatform.Domain               |
|                                                      |
| Entities | Value Objects | Domain Rules              |
| Security Concepts | Domain Events                    |
+----------------------------+-------------------------+
                             |
                             v
+------------------------------------------------------+
|            SecurityPlatform.Infrastructure           |
|                                                      |
| MySQL | MongoDB | LDAP/AD | External Services        |
| Persistence | Telemetry | Integrations               |
+------------------------------------------------------+

                         ^
                         |
                  Secure HTTPS
                         |
                         v

+------------------------------------------------------+
|              SecurityPlatform.Agent                  |
|                                                      |
| Device Identity | Posture | Policy | Controls        |
| Telemetry | Heartbeat | Secure Communication         |
+------------------------------------------------------+

## 6. Backend Project Responsibilities

	### 6.1 SecurityPlatform.Api
		Responsibilities:
			- expose HTTP API endpoints
			- authenticate incoming requests
			- enforce API-level authorization
			- validate request boundaries
			- map API requests to application operations
			- return API responses
			- expose health endpoints
			- provide centralized middleware
			- handle security-related HTTP concerns
	The API project must not contain core business rules.

	### 6.2 SecurityPlatform.Application
		Responsibilities:
			- implement application use cases
			- coordinate domain operations
			- execute commands and queries
			- perform application-level validation
			- enforce application authorization rules
			- coordinate persistence and external services
			- publish application events where required
	The Application layer should depend on abstractions rather than concrete infrastructure implementations.

	### 6.3 SecurityPlatform.Domain
		Responsibilities:
			- define domain entities
			- define value objects
			- define domain rules
			- define security concepts
			- define domain events where required
			- protect domain invariants
	The Domain layer must remain independent of database, HTTP and infrastructure implementation details.

	### 6.4 SecurityPlatform.Infrastructure
		Responsibilities:
			- MySQL persistence
			- MongoDB telemetry persistence
			- LDAP/AD integration
			- external service integrations
			- authentication infrastructure
			- telemetry infrastructure
			- repository implementations
			- infrastructure-level communication

	### 6.5 SecurityPlatform.Contracts
		Responsibilities:
			- API request contracts
			- API response contracts
			- shared DTOs
			- agent communication contracts where appropriate
			- versioned integration contracts
		Contracts must not expose internal domain implementation details unnecessarily.

## 7. Windows Agent Architecture

	The Windows agent is a background service designed to operate without requiring an interactive user interface.

Logical components:
+------------------------------------------------+
|          SecurityPlatform.Agent                |
+------------------------------------------------+
|                                                |
| Agent Host / Worker                            |
|                                                |
| +------------------+  +----------------------+ |
| | Device Identity  |  | Posture Collector    | |
| +------------------+  +----------------------+ |
|                                                |
| +------------------+  +----------------------+ |
| | Policy Manager   |  | Control Manager      | |
| +------------------+  +----------------------+ |
|                                                |
| +------------------+  +----------------------+ |
| | Telemetry        |  | Health / Heartbeat   | |
| | Collector        |  |                      | |
| +------------------+  +----------------------+ |
|                                                |
| +--------------------------------------------+ |
| | Secure Platform Communication              | |
| +--------------------------------------------+ |
+------------------------------------------------+

	### 7.1 Device Identity
		Responsible for:
			- establishing endpoint identity
			- registering the endpoint
			- maintaining device registration state
			- associating endpoint activity with the platform identity

	### 7.2 Posture Collector
		Collects approved endpoint security and health information required for device trust evaluation.

	### 7.3 Policy Manager
		Responsible for:
			- retrieving assigned policies
			- validating policy data
			- maintaining applicable policy state
			- detecting policy changes
			- applying policy configuration through appropriate controls

	### 7.4 Control Manager
		Provides endpoint enforcement capabilities for approved controls such as:
			- USB controls
			- website controls
			- file access controls
			- process-related controls
		Controls must be implemented with explicit authorization and safe failure behavior.

	### 7.5 Telemetry Collector
		Collects security-relevant endpoint events and sends them to the central platform.

	### 7.6 Health and Heartbeat
		Provides:
			- agent health
			- connectivity status
			- heartbeat
			- basic operational state
			- agent version information

## 8. Dashboard Architecture
	The dashboard is a React/TypeScript application.
	Primary responsibilities:
		- authentication
		- role-aware navigation
		- device inventory
		- device details
		- policy management
		- Zero Trust decisions
		- endpoint control management
		- telemetry investigation
		- audit review
		- platform health
Logical structure:
React Dashboard
|
+-- Authentication
|
+-- Authorization / Route Protection
|
+-- Device Management
|
+-- Policy Management
|
+-- Zero Trust
|
+-- Endpoint Controls
|
+-- Telemetry / Investigation
|
+-- Audit
|
+-- Health / Operations
|
+-- Shared API Client
|
+-- Shared Security / Session Handling

The dashboard must not directly access MySQL, MongoDB or endpoint systems.
All privileged operations must pass through the backend API.

## 9. Data Architecture
	The platform uses separate stores for operational data and high-volume telemetry.
	### 9.1 MySQL
		MySQL is the primary store for structured operational data.
		Expected data categories include:
			- users
			- roles
			- permissions
			- devices
			- device registration state
			- policies
			- policy assignments
			- Zero Trust configuration
			- endpoint control configuration
			- audit metadata
			- platform configuration
		Relational integrity is important for these entities.

	### 9.2 MongoDB
		MongoDB is used for telemetry and event-oriented data.
		Expected data categories include:
			- endpoint telemetry
			- security events
			- endpoint activity events
			- operational agent events
			- investigation-oriented event records
		MongoDB is selected because telemetry is expected to have high write volume and evolving event structures.

	### 9.3 Data Separation Principle
		Operational records must not be mixed unnecessarily with high-volume telemetry.

                  Security Platform
                         |
               +---------+---------+
               |                   |
               v                   v
            MySQL               MongoDB
        Operational Data     Telemetry / Events

## 10. Authentication and Authorization Flow
	The platform uses token-based authentication with role and permission enforcement.

High-level flow:
User
 |
 | Credentials
 v
API
 |
 | Authenticate
 v
Authentication Service
 |
 | Access Token
 v
User
 |
 | Access Token
 v
API
 |
 +--> Validate token
 |
 +--> Resolve claims / roles
 |
 +--> Authorize operation
 |
 v
Application Layer

	Authorization must be enforced server-side.
	The dashboard may hide unauthorized actions for usability, but UI restrictions are not considered a security boundary.

## 11. Agent Authentication Flow
	The agent must establish its platform identity before receiving privileged configuration or commands.

Windows Agent
     |
     | Registration / Authentication
     v
Security Platform API
     |
     | Validate registration
     | Authenticate agent
     v
Device Identity
     |
     | Authorized
     v
Agent receives permitted configuration

	The platform must distinguish:
		- unknown agent
		- registered agent
		- authenticated agent
		- authorized agent
		- restricted device
	Agent identity must not be based solely on an easily spoofed endpoint property.

## 12. Device Communication Flow

	Normal agent communication follows this pattern:
Agent
  |
  | HTTPS
  v
API
  |
  +--> Authenticate Agent
  |
  +--> Authorize Device
  |
  +--> Process Request
  |
  +--> Persist Operational Data
  |
  +--> Persist Telemetry
  |
  v
Response
  |
  v
Agent

	The server remains authoritative for platform policy and device authorization.

## 13. Policy Flow

	Policy management follows:

Security Administrator
        |
        v
React Dashboard
        |
        v
SecurityPlatform.Api
        |
        v
Application Layer
        |
        v
Policy Domain
        |
        v
      MySQL
        |
    Assigned Policy
        v
     Agent
        |
        v
Policy Validation
        |
        v
Local Enforcement
        |
        v
Telemetry / Status
        |
        v
Security Platform

	Policy changes must be auditable.
	The agent must validate received policy data before applying it.

## 14. Telemetry Flow
Endpoint telemetry follows:

Windows Endpoint
       |
       v
Agent Collectors
       |
       v
Local Validation / Buffering
       |
     HTTPS
       v
SecurityPlatform.Api
       |
       v
Application Layer
       |
       v
    MongoDB
       |
       +---------> Investigation
       |
       +---------> Detection / Analysis
       |
       +---------> Operational Views

	Telemetry must be validated and protected against unauthorized manipulation.
	The architecture must also prevent uncontrolled telemetry growth through appropriate retention, filtering and operational controls.

## 15. Zero Trust Decision Flow
	Zero Trust evaluation uses contextual information rather than network location alone.

Access Request
      |
      v
Identity Context
      |
      +--> User
      +--> Role / Claims
      |
      v
Device Context
      |
      +--> Device Identity
      +--> Device Posture
      +--> Agent Health
      |
      v
Policy Context
      |
      +--> Applicable Policy
      +--> Endpoint Restrictions
      |
      v
Risk / Trust Evaluation
      |
      v
Decision
  +---+---+---+
  |       |   |
Allow  Restrict Deny


	The decision must be explainable through appropriate security and audit information.

## 16. API Boundaries
	The API is the primary controlled entry point into the platform.
Logical API areas include:

/api/auth
/api/users
/api/roles
/api/devices
/api/policies
/api/zerotrust
/api/controls
/api/telemetry
/api/audit
/api/health
/api/agent

	These are logical boundaries rather than a commitment to exact final route names.
	API design must maintain:
		- authentication
		- authorization
		- input validation
		- consistent error handling
		- auditability
		- API versioning strategy where required
		- secure response handling

## 17. Deployment Topology
	The initial deployment architecture is container-friendly for backend services and supporting infrastructure.

+----------------------------------------------------------+
|                    Deployment Environment                 |
|                                                          |
|  +--------------------+                                  |
|  | React Dashboard    |                                  |
|  +---------+----------+                                  |
|            |                                             |
|            | HTTPS                                       |
|            v                                             |
|  +--------------------+                                  |
|  | ASP.NET Core API   |                                  |
|  +---------+----------+                                  |
|            |                                             |
|       +----+--------------------+                        |
|       |                         |                        |
|       v                         v                        |
|  +----------+              +-----------+                 |
|  |  MySQL   |              |  MongoDB  |                 |
|  +----------+              +-----------+                 |
|                                                          |
+----------------------------------------------------------+

                    ^
                    |
                  HTTPS
                    |
+-------------------+-------------------+
|       Windows Endpoint(s)             |
|                                       |
|    SecurityPlatform.Agent             |
+---------------------------------------+

	Docker and container orchestration artifacts are maintained separately under the deployment directories.
	The Windows agent remains an endpoint-side Windows service rather than a containerized component.

## 18. Technology Choices

|      Area            | Technology                         | Architectural Reason                                                    |
|----------------------|------------------------------------|-------------------------------------------------------------------------|
| Backend API          | ASP.NET Core / .NET                | Strong fit for security-oriented backend services and Windows ecosystem |
| Backend language     | C#                                 | Type safety and maintainability                                         |
| Agent                | .NET Windows Worker/Service        | Native Windows service model                                            |
| Dashboard            | React + TypeScript                 | Component-based administration UI                                       |
| Operational database | MySQL                              | Relational integrity for structured platform data                       |
| Telemetry database   | MongoDB                            | Flexible event-oriented telemetry storage                               |
| API communication    | HTTPS / REST                       | Explicit service boundary                                               |
| Agent communication  | HTTPS / REST                       | Secure centrally managed endpoint communication                         |
| Authentication       | Token-based authentication         | API authentication model                                                |
| Authorization        | RBAC + claims/policies             | Fine-grained access control                                             |
| Containerization     | Docker                             | Repeatable development and deployment                                   |
| Observability        | OpenTelemetry-compatible approach  | Standardized operational visibility                                     |
| Real-time updates    | SignalR where justified            | Dashboard notifications for selected events                             |


Technology selection remains subject to implementation validation and ADR review.


## 19. Architecture Constraints
	The architecture must respect the following constraints:
1. Windows endpoints are the initial supported endpoint platform.
2. The endpoint agent must operate as a background service.
3. The dashboard must not directly access platform databases.
4. Database credentials must not be exposed to clients.
5. Security-sensitive authorization must be enforced server-side.
6. Domain logic must not depend directly on infrastructure implementations.
7. Telemetry storage must remain separated from core operational data.
8. Endpoint controls must be implemented with explicit security boundaries.
9. Agent communication must be authenticated and authorized.
10. The architecture must remain testable.
11. The initial implementation must avoid unnecessary distributed-service complexity.
12. Redis is not required unless a concrete implementation need is demonstrated.
13. The architecture must remain compatible with Docker-based development and deployment.
14. Mobile, macOS and Linux endpoint agents are outside the initial scope.
15. Commercial EDR/XDR/SIEM/MDM replacement is outside the product scope.

## 20. Threat Model Alignment

	The architecture directly addresses the threat categories identified in DOC-013.

| Threat Area                          | Architectural Response                                   |
|--------------------------------------|----------------------------------------------------------|
| Credential theft                     | Central authentication and server-side authorization     |
| JWT/token theft                      | Token validation, expiry and authorization checks        |
| Privilege escalation                 | RBAC, claims and least privilege                         |
| Broken object authorization          | Application-level resource authorization                 |
| Unauthorized device registration     | Explicit device identity and registration flow 	  |
| Agent impersonation                  | Agent authentication and device identity                 |
| Replay                               | Request validation and authenticated communication       |
| Policy tampering                     | Central policy authority and agent validation            |
| Malicious policy assignment          | Authorization and audit controls                         |
| Telemetry tampering                  | Authenticated ingestion and validation                   |
| Telemetry disclosure                 | Controlled API access and data separation                |
| Audit manipulation                   | Dedicated audit architecture and restricted access       |
| API abuse                            | Authentication, authorization and validation             |
| Dashboard compromise                 | Secure authentication and server-side authorization      |
| Database compromise                  | Database isolation from clients                          |
| Endpoint compromise                  | Endpoint posture, agent health and Zero Trust evaluation |
| Agent binary/configuration tampering | Secure deployment and integrity controls                 |
| Endpoint control bypass              | Local enforcement plus centralized policy state          |
| Insider misuse                       | RBAC, auditability and separation of duties              |
| Denial of service                    | Health monitoring, bounded processing and resilience     |
| Communication interception           | HTTPS/TLS                                                |
| Unauthorized command/policy delivery | Authenticated and authorized agent communication         |
| Zero Trust manipulation              | Contextual evaluation and policy-controlled decisions    |
| Error exposure                       | Centralized secure error handling                        |
| Excessive telemetry                  | Filtering, retention and operational controls            |
| Audit repudiation                    | Security audit records                                   |


	The detailed threat-to-control mapping remains governed by DOC-013 and DOC-015.


## 21. Architecture Traceability
	The architecture is derived from the following baselines:

| Source                              | Relationship                                        |
|-------------------------------------|-----------------------------------------------------|
| DOC-001 Product Vision              | Defines product purpose and architectural direction |
| DOC-002 Product Scope               | Defines system boundaries and exclusions            |
| DOC-003 Personas                    | Defines primary human actors                        |
| DOC-004 Use Cases                   | Defines system behaviors                            |
| DOC-005 Functional Requirements     | Defines required capabilities                       |
| DOC-006 Non-Functional Requirements | Defines quality attributes                          |
| DOC-007 Security Requirements       | Defines security constraints                        |
| DOC-008 Acceptance Criteria         | Defines expected behavior                           |
| DOC-009 Requirements Traceability   | Provides requirement relationships                  |
| DOC-010 Epics                       | Organizes implementation scope                      |
| DOC-011 User Stories                | Defines implementation backlog                      |
| DOC-012 Sprint Plan                 | Defines implementation sequencing                   |
| DOC-013 Threat Model                | Defines threats, trust boundaries and residual risks|


## 22. Future Evolution
	The initial architecture is intentionally modular without introducing unnecessary distributed-service complexity.
	Future evolution may include:
		- additional endpoint platforms
		- dedicated policy evaluation services
		- event streaming
		- advanced detection pipelines
		- dedicated identity providers
		- certificate-based endpoint identity
		- additional real-time communication
		- horizontal scaling
		- centralized secrets management
		- advanced observability infrastructure
	These capabilities require explicit scope approval and architecture decisions before implementation.

## 23. Architecture Review Criteria

	DOC-014 is considered architecturally complete when:
		- system context is documented
		- major components are identified
		- component responsibilities are defined
		- agent architecture is defined
		- dashboard architecture is defined
		- backend boundaries are defined
		- data architecture is defined
		- authentication flow is defined
		- agent communication flow is defined
		- policy flow is defined
		- telemetry flow is defined
		- Zero Trust flow is defined
		- deployment topology is defined
		- technology choices are documented
		- architecture constraints are documented
		- DOC-013 threat alignment is documented
		- architecture remains consistent with approved product scope and requirements

## 24. Document Status
	**Status:** Reviewed — Baseline v0.1
	Next Review: Architecture review before DOC-015 Security Architecture.
	Related Documents:
		- DOC-001 Product Vision
		- DOC-002 Product Scope
		- DOC-003 Personas
		- DOC-004 Use Cases
		- DOC-005 Functional Requirements
		- DOC-006 Non-Functional Requirements
		- DOC-007 Security Requirements
		- DOC-008 Acceptance Criteria
		- DOC-009 Requirements Traceability
		- DOC-010 Backlog Epics
		- DOC-011 User Stories
		- DOC-012 Sprint Plan
		- DOC-013 Threat Model


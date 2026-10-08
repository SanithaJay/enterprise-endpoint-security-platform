# DOC-015 â€” Security Architecture



**Document Version:** v0.1  

**Phase:** G7 â€” Security Architecture  

**Status:** Reviewed — Baseline v0.1

**Date:** 08-10-2026  

**Related Documents:** DOC-001, DOC-002, DOC-005, DOC-006, DOC-007, DOC-008, DOC-009, DOC-013, DOC-014



---



## 1. Purpose



This document defines the security architecture for the Enterprise Endpoint Security \& Zero Trust Platform.



It translates the approved security requirements, threat model and system architecture into a coherent security design covering:



- User authentication

- Authorization and RBAC

- Agent identity and trust

- API security

- Endpoint communication security

- Data protection

- Secret and credential handling

- Policy protection

- Zero Trust security decisions

- Telemetry and audit protection

- Administrative security

- Security logging and monitoring

- Security failure handling

- Security architecture traceability



The purpose of this document is to establish the security baseline before implementation begins.



This document does not replace the detailed security requirements in DOC-007 or the threat analysis in DOC-013. It defines how those requirements and identified threats are addressed architecturally.



---



## 2. Security Architecture Objectives



The security architecture is designed to achieve the following objectives:



1\. Protect user and administrator identities.

2\. Enforce least-privilege access to platform capabilities.

3\. Prevent unauthorized access to devices, policies, telemetry and administrative functions.

4\. Establish a trusted identity for every registered endpoint agent.

5\. Protect communication between dashboard, API and endpoint agents.

6\. Prevent unauthorized policy creation, modification, assignment and delivery.

7\. Protect telemetry and audit information from unauthorized modification or disclosure.

8\. Apply server-side authorization to all security-sensitive operations.

9\. Support Zero Trust decisions using identity, device and security context.

10\. Maintain security-relevant auditability.

11\. Minimize exposure of sensitive information through logs, errors and APIs.

12\. Support secure failure and recovery behavior.

13\. Provide security controls that are testable and traceable to requirements.

14\. Keep security responsibilities separated between application layers and system components.



---



## 3. Security Principles



The platform follows these security principles.



### 3.1 Zero Trust by Default



No user, device or agent is trusted solely because it is inside a network boundary.



Access decisions must consider authenticated identity, authorization, device identity, device state and applicable security policy.



### 3.2 Least Privilege



Users, administrators, services and agents receive only the permissions required for their responsibilities.



### 3.3 Explicit Authorization



Authentication establishes identity.



Authorization determines what that identity is allowed to do.



Authentication must never be treated as implicit authorization.



### 3.4 Server-Side Enforcement



Security-sensitive authorization decisions are enforced by the backend.



The dashboard must not be treated as a security boundary.



UI controls may hide unauthorized actions, but the API must independently reject unauthorized requests.



### 3.5 Defense in Depth



Security controls are applied at multiple layers:



- Identity

- Authentication

- Authorization

- API

- Transport

- Application

- Data

- Endpoint agent

- Policy

- Audit

- Monitoring



### 3.6 Secure by Default



New users, devices, policies and security-sensitive resources must begin in a restrictive or explicitly configured state rather than receiving implicit trust.



### 3.7 Fail Securely



Security failures must not silently result in unrestricted access.



Where a security decision cannot be reliably established, the system should move toward a restrictive state appropriate to the operation.



### 3.8 Separation of Duties



Administrative capabilities should be separated according to defined roles.



High-impact operations should require the appropriate administrative permissions.



### 3.9 Complete Mediation



Security-sensitive requests must be authorized each time they cross a protected API boundary.



### 3.10 Minimize Sensitive Data



Only information required for security, operational and audit purposes should be collected and retained.



---



## 4. Security Zones and Trust Boundaries



The security architecture follows the trust boundaries identified in DOC-013 and the system boundaries defined in DOC-014.



### 4.1 Dashboard Zone



The React dashboard is an administrative client.



It is not a trusted security authority.



Responsibilities include:



- Presenting authenticated user interfaces

- Sending authorized API requests

- Displaying security state

- Displaying devices, policies, telemetry and audit information

- Enforcing user experience based on permissions



The dashboard must not:



- Connect directly to MySQL

- Connect directly to MongoDB

- Make authorization decisions that replace backend authorization

- Store long-lived privileged credentials unnecessarily



### 4.2 Platform API Zone



The ASP.NET Core API is the primary security enforcement boundary.



Responsibilities include:



- Authentication

- Authorization

- Input validation

- Resource authorization

- Policy enforcement

- Device authorization

- Security-sensitive business operations

- Audit generation

- Security event handling



### 4.3 Data Zone



MySQL and MongoDB are protected backend data stores.



Application components access these stores through controlled infrastructure and application boundaries.



Clients and endpoint agents must never connect directly to the databases.



### 4.4 Endpoint Agent Zone



The Windows SecurityPlatform.Agent operates on managed endpoints.



The agent is considered a security-sensitive component because compromise of an endpoint or agent may affect endpoint controls and telemetry.



The platform must therefore distinguish:



- Unknown agent

- Registered agent

- Authenticated agent

- Authorized agent

- Restricted agent



### 4.5 Administrative Boundary



Administrative operations represent a higher-risk security boundary.



Operations such as:



- User and role administration

- Policy changes

- Device trust changes

- Endpoint control changes

- Security configuration changes



must require appropriate authorization and generate security-relevant audit events.



---



## 5. Identity and Authentication Architecture



### 5.1 User Identity



Platform users authenticate through the platform's configured identity mechanism.



The architecture supports integration with enterprise identity sources where required, including directory-based identity such as LDAP/Active Directory.



The platform must establish a normalized application identity after successful authentication.



### 5.2 Authentication Result



Successful authentication produces an authenticated security principal containing the information required for authorization.



The principal may include:



- User identifier

- Username

- Roles

- Permission claims

- Authentication context

- Relevant security metadata



### 5.3 Token-Based Authentication



The API uses token-based authentication for authenticated application requests.



Access tokens are intended for short-lived authenticated access.



Refresh mechanisms, where implemented, must be protected and must not expose long-lived credentials to unnecessary components.



### 5.4 Token Validation



The API must validate security tokens before processing protected operations.



Validation must include appropriate checks for:



- Token integrity

- Issuer

- Audience

- Expiration

- Required claims

- Authentication context where applicable



Invalid or expired tokens must be rejected.



### 5.5 Authentication Failure



Authentication failures must not reveal unnecessary information about:



- User existence

- Credentials

- Internal authentication mechanisms

- Directory configuration

- Internal system details



Security-relevant authentication failures should be auditable.



---



## 6. Authorization and RBAC Architecture



### 6.1 Role-Based Access Control



The platform uses RBAC as the primary authorization model.



The baseline roles are:



- Platform Admin

- Security Admin

- Security Analyst

- Auditor / Read-Only



### 6.2 Permission Enforcement



Roles map to permissions or claims representing authorized capabilities.



Authorization must be evaluated server-side.



Example protected capabilities include:



- User administration

- Role administration

- Device registration approval

- Device trust management

- Policy creation

- Policy modification

- Policy assignment

- Endpoint control management

- Telemetry access

- Audit access

- Security investigation



### 6.3 Resource Authorization



Role checks alone are not sufficient for every operation.



The API must also validate whether the authenticated principal is authorized to access the specific resource.



Examples include:



- Device ownership or administrative scope

- Policy assignment scope

- Administrative resource scope

- Security investigation access



### 6.4 Privilege Escalation Protection



Users must not be able to modify their own authorization state unless explicitly authorized by an administrative security boundary.



Role and permission changes must be audited.



### 6.5 Deny by Default



If an authorization rule cannot establish that an operation is permitted, the operation must be denied.



---



## 7. Agent Identity and Authentication



### 7.1 Agent Identity



Each managed endpoint must have a unique platform device identity.



The identity is associated with the registered endpoint and its SecurityPlatform.Agent instance.



### 7.2 Device Registration



A new endpoint begins in an untrusted or pending state.



Registration must establish sufficient device identity information before the endpoint receives privileged platform instructions.



The registration process must prevent arbitrary endpoints from becoming trusted agents.



### 7.3 Agent Authentication



After registration, the agent must authenticate when communicating with the platform API.



Agent authentication must distinguish the device from an unauthenticated client.



### 7.4 Agent Authorization



Successful agent authentication does not automatically grant unrestricted authority.



The backend must determine whether the device is:



- Authorized

- Restricted

- Disabled

- Unknown

- Pending



### 7.5 Agent Credential Protection



Agent authentication credentials or tokens must not be stored in easily accessible plaintext configuration where avoidable.



Sensitive agent credentials must be protected using appropriate Windows security mechanisms and application security controls.



### 7.6 Agent Compromise



The architecture assumes an endpoint may eventually be compromised.



The platform must therefore support:



- Device restriction

- Device disablement

- Trust-state changes

- Credential/token invalidation where applicable

- Security event generation

- Auditability



---



## 8. API Security Architecture



### 8.1 API Security Boundary



The ASP.NET Core API is the authoritative security boundary between clients, agents and platform services.



Every protected endpoint must define its authentication and authorization requirements.



### 8.2 Input Validation



API input must be validated before business processing.



Validation applies to:



- Request bodies

- Query parameters

- Route parameters

- Headers where security-relevant

- Device identifiers

- Policy definitions

- User and role data



Invalid input must be rejected safely.



### 8.3 Resource Access Control



The API must verify that the authenticated caller is authorized to access the requested resource.



Client-supplied identifiers must never be treated as proof of authorization.



### 8.4 Injection Protection



Database and infrastructure interactions must use safe parameterization and appropriate framework abstractions.



The application must avoid constructing executable database commands directly from untrusted input.



### 8.5 Error Handling



API errors must provide useful client-facing information without exposing:



- Stack traces

- Database credentials

- Connection strings

- Internal filesystem paths

- Security tokens

- Sensitive configuration

- Internal infrastructure details



Detailed diagnostic information belongs in protected server-side logs.



### 8.6 Rate and Abuse Protection



Security-sensitive endpoints should support appropriate abuse controls.



Higher-risk operations include:



- Authentication

- Token issuance

- Registration

- Password-related operations

- Policy modification

- Administrative operations



---



## 9. Communication Security



### 9.1 Transport Protection



Communication between:



- Dashboard and API

- Agent and API

- Platform components where network communication exists



must use protected transport.



HTTPS/TLS is the baseline transport security mechanism.



### 9.2 Certificate Validation



Clients and agents must validate the server identity according to the deployment environment.



Certificate validation must not be disabled as a permanent security workaround.



### 9.3 Agent Communication



Agent communication must be authenticated and encrypted.



The platform must reject communication from unknown or unauthorized agents for protected operations.



### 9.4 Replay Protection



Security-sensitive requests must include sufficient protection against replay where required by the communication protocol and operation.



Short-lived authentication material, request freshness and server-side state validation may be used as appropriate.



### 9.5 Unauthorized Commands



The agent must not execute security-sensitive commands or policy instructions solely because a message appears to originate from a network endpoint.



Commands and policies must be associated with an authenticated and authorized device context.



---



## 10. Policy Security Architecture



### 10.1 Policy Ownership



Policies are platform-managed security resources.



Policy creation, modification and assignment require appropriate permissions.



### 10.2 Policy Validation



Policies must be validated before persistence and delivery.



Validation must prevent malformed or unsafe policy definitions from being accepted.



### 10.3 Policy Versioning



Policy changes should produce identifiable policy versions or revisions so that the platform can determine which policy version was assigned to an endpoint.



### 10.4 Policy Assignment



Policy assignment must verify:



- Caller authorization

- Target device authorization

- Policy validity

- Applicable scope

- Assignment state



### 10.5 Policy Delivery



The agent must validate received policy information before applying it.



The agent must not blindly trust arbitrary policy data received from the network.



### 10.6 Policy Audit



Security-relevant policy operations must generate audit events, including:



- Creation

- Modification

- Assignment

- Removal

- Activation/deactivation

- Failed operations



---



## 11. Zero Trust Security Architecture



### 11.1 Zero Trust Decision Inputs



Zero Trust evaluation may consider:



- User identity

- User authorization

- Device identity

- Device registration state

- Device posture

- Agent health

- Device trust state

- Applicable policy

- Security events

- Relevant contextual information



### 11.2 Trust States



The platform supports security states such as:



- Allow

- Restrict

- Deny



These decisions must be based on explicit security policy and available security context.



### 11.3 Trust Is Not Permanent



A device that was previously trusted may become restricted or denied if its security context changes.



Examples include:



- Agent becomes unhealthy

- Device becomes unregistered

- Security policy changes

- Device posture becomes non-compliant

- Administrative action changes device trust

- Security events indicate elevated risk



### 11.4 Decision Audit



Zero Trust decisions must be auditable.



The audit record should capture sufficient information to explain:



- Subject

- Device

- Decision

- Applicable policy

- Decision context

- Timestamp

- Decision source



Sensitive information should not be recorded unnecessarily.



---



## 12. Data Protection Architecture



### 12.1 Data Classification



The platform should distinguish at least the following categories:



- Identity data

- Authorization data

- Device information

- Security policy data

- Telemetry

- Security events

- Audit records

- Operational health data

- Security-sensitive configuration



### 12.2 MySQL Protection



MySQL stores structured operational information such as:



- Users

- Roles

- Permissions

- Devices

- Policies

- Assignments

- Configuration

- Operational state



Database access is restricted to trusted backend infrastructure.



### 12.3 MongoDB Protection



MongoDB stores telemetry and event-oriented information.



Telemetry access must be controlled through application authorization.



High-volume telemetry must not bypass platform security controls simply because it is operational event data.



### 12.4 Encryption in Transit



Sensitive data transmitted across network boundaries must use protected transport.



### 12.5 Encryption at Rest



Production deployments should use database and infrastructure encryption-at-rest capabilities appropriate to the deployment environment.



Application design must avoid assuming that database storage is inherently trusted.



### 12.6 Sensitive Fields



Credentials, authentication secrets, tokens and other sensitive values must not be stored in plaintext unless technically unavoidable and explicitly justified.



### 12.7 Data Retention



Telemetry and audit retention should follow defined operational and security requirements.



Retention must balance:



- Investigation requirements

- Audit requirements

- Storage cost

- Privacy

- Data minimization



---



## 13. Secret and Credential Management



### 13.1 No Hard-Coded Secrets



The source repository must not contain:



- Production passwords

- API secrets

- Database passwords

- Private keys

- Long-lived access tokens

- Connection strings containing credentials



### 13.2 Configuration Separation



Environment-specific secrets must be supplied through protected configuration mechanisms appropriate to the deployment environment.



### 13.3 Development Configuration



Local development may use development-only configuration, but development secrets must not be committed to Git.



### 13.4 Secret Rotation



The architecture should support replacement or rotation of sensitive credentials without requiring application source-code changes.



---



## 14. Endpoint Security Architecture



### 14.1 Agent Execution



The Windows SecurityPlatform.Agent operates as a background service.



It must minimize unnecessary privileges while retaining the permissions required for approved endpoint-security functions.



### 14.2 Local Protection



The agent must protect its configuration and security-sensitive local state from unauthorized modification where practical.



### 14.3 Endpoint Controls



Endpoint controls such as:



- USB control

- Website control

- File control

- Process control



must be governed by authorized platform policy.



### 14.4 Control Failure



If an endpoint control cannot be safely applied, the agent must report the failure rather than silently claiming successful enforcement.



### 14.5 Local Logging



Agent logs must avoid exposing sensitive credentials, tokens or unnecessary personal information.



Security-relevant failures must be recorded with sufficient diagnostic context.



---



## 15. Telemetry Security



### 15.1 Telemetry Integrity



Telemetry received by the platform must be associated with an authenticated device identity where applicable.



### 15.2 Telemetry Validation



The backend must validate telemetry structure, source identity and acceptable values before persistence.



### 15.3 Telemetry Confidentiality



Telemetry may contain security-sensitive endpoint information.



Access must therefore be controlled through authorization policies.



### 15.4 Telemetry Abuse



The platform must protect against excessive or malformed telemetry that could cause:



- Resource exhaustion

- Storage abuse

- Excessive logging

- Application instability



### 15.5 Telemetry and Audit Separation



Operational telemetry and security audit records must remain logically distinguishable.



Audit records must not be treated as ordinary endpoint telemetry.



---



## 16. Audit and Accountability Security



### 16.1 Security-Relevant Events



The platform should audit important security actions, including:



- Authentication success/failure

- Authorization failures

- User creation

- Role changes

- Permission changes

- Device registration

- Device trust changes

- Policy creation

- Policy modification

- Policy assignment

- Endpoint control changes

- Zero Trust decisions

- Administrative actions

- Security configuration changes



### 16.2 Audit Integrity



Audit records must be protected against unauthorized modification or deletion through normal application operations.



### 16.3 Audit Access



Audit information must be accessible only to appropriately authorized users.



### 16.4 Audit Context



Audit events should contain sufficient context for accountability without unnecessarily recording sensitive information.



---



## 17. Administrative Security



### 17.1 Administrative Roles



Administrative capabilities must be divided according to the approved platform roles.



### 17.2 High-Risk Operations



High-impact actions require explicit authorization.



Examples include:



- Changing roles

- Changing permissions

- Registering or disabling devices

- Changing device trust

- Modifying security policies

- Changing endpoint controls



### 17.3 Administrative Accountability



Administrative security operations must be auditable.



The system should make it possible to determine:



- Who performed an action

- What action was performed

- Which resource was affected

- When the action occurred

- Whether the operation succeeded or failed



---



## 18. Security Failure and Recovery



### 18.1 Authentication Failure



Unauthenticated requests must be rejected.



### 18.2 Authorization Failure



Unauthorized requests must be rejected without revealing protected resource information.



### 18.3 Agent Authentication Failure



An agent that cannot establish valid authentication must not receive protected policy or command information.



### 18.4 Policy Failure



An invalid policy must not be applied as if it were valid.



### 18.5 Data Store Failure



Database failures must not result in accidental authorization bypass.



### 18.6 Service Failure



Security-critical components should fail in a manner that avoids granting unintended access.



### 18.7 Recovery



Recovery operations must preserve security state and auditability.



---



## 19. Security Logging and Observability



Security logging must support detection, investigation and operational troubleshooting.



Security logs should distinguish:



- Authentication events

- Authorization events

- Device security events

- Policy events

- Endpoint control events

- Zero Trust decisions

- Application security errors

- Administrative actions



Logs must avoid storing:



- Passwords

- Authentication tokens

- Private keys

- Database credentials

- Unnecessary sensitive personal information



The observability architecture defined in DOC-014 remains applicable.



---



## 20. Security Testing Strategy



Security architecture must be testable.



Testing should cover:



### Authentication



- Valid authentication

- Invalid credentials

- Expired tokens

- Invalid tokens

- Missing authentication



### Authorization



- Authorized operation

- Unauthorized operation

- Role boundary enforcement

- Resource-level authorization

- Privilege escalation attempts



### API Security



- Invalid input

- Injection attempts

- Unauthorized resource access

- Malformed requests

- Security-sensitive endpoint abuse



### Agent Security



- Unknown agent

- Unregistered device

- Invalid agent credentials

- Restricted device

- Invalid policy delivery



### Policy Security



- Unauthorized policy modification

- Invalid policy

- Unauthorized policy assignment

- Policy integrity validation



### Zero Trust



- Allow decision

- Restrict decision

- Deny decision

- Device state changes

- Posture changes



### Data Protection



- Unauthorized database access through application paths

- Sensitive information exposure

- Credential exposure

- Logging of sensitive values



Security testing will be implemented as part of the G19 testing phase.



---



## 21. Threat Model Alignment



The security architecture directly addresses the threats identified in DOC-013.



| Threat Area | Security Architecture Response |

|---|---|

| Credential theft | Token validation, protected credentials, least privilege |

| JWT/token theft | Short-lived access model, validation, protected storage |

| Privilege escalation | RBAC, claims, server-side authorization, audit |

| Broken object authorization | Resource-level authorization |

| Unauthorized device registration | Registration state and agent authorization |

| Agent impersonation | Device identity and authenticated agent communication |

| Replay | Token lifetime and request freshness controls |

| Policy tampering | Policy validation, authorization and audit |

| Malicious policy assignment | Assignment authorization and audit |

| Telemetry tampering | Authenticated source association and validation |

| Telemetry disclosure | Authorization and protected transport |

| Audit manipulation | Controlled audit architecture and access restrictions |

| API injection/abuse | Validation, parameterization and abuse controls |

| Dashboard compromise | API treated as authoritative security boundary |

| Database compromise | Restricted database access and data protection |

| Endpoint compromise | Device trust states, posture and restrictive decisions |

| Agent tampering | Protected local configuration and device trust controls |

| Endpoint control bypass | Policy enforcement and failure reporting |

| Insider misuse | Least privilege, RBAC and auditability |

| Denial of service | Validation, resource controls and resilient failure handling |

| Communication interception | HTTPS/TLS |

| Unauthorized policy/command delivery | Authenticated and authorized agent communication |

| Zero Trust manipulation | Server-side decision logic and auditable context |

| Excessive telemetry | Validation and resource protection |

| Audit repudiation | Security audit records and accountability |



---



## 22. Security Architecture Traceability



The security architecture is derived from and remains traceable to the approved project baseline.



| Source | Security Architecture Relationship |

|---|---|

| DOC-001 Product Vision | Security and Zero Trust product objectives |

| DOC-002 Product Scope | Security boundaries and platform scope |

| DOC-005 Functional Requirements | Security-related functional behavior |

| DOC-006 Non-Functional Requirements | Security, privacy, resilience, auditability and observability constraints |

| DOC-007 Security Requirements | Primary security requirement baseline |

| DOC-008 Acceptance Criteria | Security acceptance expectations |

| DOC-009 Requirements Traceability | Requirement-to-use-case traceability |

| DOC-013 Threat Model | Threats, trust boundaries, attack surfaces and residual risks |

| DOC-014 System Architecture | System components, data flows, API boundaries and deployment architecture |



---



## 23. Security Architecture Constraints



The following constraints apply to the current implementation baseline:



1\. Windows endpoints are the initial supported endpoint platform.

2\. The endpoint agent operates as a Windows background service.

3\. The dashboard does not access databases directly.

4\. Security authorization is enforced server-side.

5\. MySQL remains the structured operational data store.

6\. MongoDB remains the telemetry/event-oriented data store.

7\. Agent communication is authenticated and protected.

8\. Commercial EDR/XDR/SIEM/MDM replacement is outside the current scope.

9\. Mobile, macOS and Linux endpoint agents are outside the current implementation scope.

10\. Advanced PKI-based endpoint identity is not required for the initial implementation unless introduced through an approved ADR.

11\. Additional distributed infrastructure must not be introduced without a concrete architectural requirement.



---



## 24. Security Architecture Review Criteria



DOC-015 is considered ready for baseline approval when the following conditions are satisfied:



- Authentication architecture is defined.

- Authorization and RBAC architecture is defined.

- Agent identity and authentication architecture is defined.

- API security architecture is defined.

- Communication security is defined.

- Data protection architecture is defined.

- Secret and credential handling is defined.

- Policy security is defined.

- Zero Trust security decisions are defined.

- Telemetry security is defined.

- Audit and accountability are defined.

- Administrative security is defined.

- Security failure behavior is defined.

- Security testing expectations are defined.

- Threat model alignment is documented.

- Security requirements remain traceable.

- No architecture decision contradicts DOC-014.

- No security control introduces unnecessary technology outside the approved scope.



---



## 25. Document Status



**Document:** DOC-015 â€” Security Architecture  

**Version:** v0.1  

**Phase:** G7 â€” Security Architecture  

**Status:** Reviewed — Baseline v0.1

**Review State:** Baseline Approved  

**Next Action:** Review security architecture against DOC-007 Security Requirements, DOC-013 Threat Model and DOC-014 System Architecture before baseline approval.








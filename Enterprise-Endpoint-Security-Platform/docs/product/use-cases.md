# DOC-004 — Product Use-Case Catalogue



**Document Version:** v0.1  

**Phase:** G1.3 — Product Discovery / Use-Case Catalogue  

**Product:** Enterprise Endpoint Security & Zero Trust Platform  

**Status:** Draft  

**Owner:** Project Team  

**Related Documents:**  

- DOC-001 — Product Vision

- DOC-002 — Product Scope

- DOC-003 — Product Personas



---



## 1. Purpose



This document defines the primary use cases for the Enterprise Endpoint Security & Zero Trust Platform.



The use cases describe the major interactions between human users, the Windows Endpoint Agent, identity and directory services, the security platform backend, and supporting data services.



The catalogue provides a functional baseline for the later Requirements, Architecture, Security Architecture, implementation, testing, and traceability phases.



This document intentionally focuses on **what the platform must allow actors to accomplish**, rather than defining detailed APIs, database schemas, implementation classes, or technology-specific designs.



---



## 2. Use-Case Model



The platform use cases are organized into the following functional areas:



1. Identity and Access

2. Endpoint Management

3. Policy Management

4. Zero Trust

5. Endpoint Security Controls

6. Security Telemetry and Investigation

7. Audit and Monitoring



---



## 3. Actors



### 3.1 Human Actors



- Platform Administrator

- Security Administrator

- Security Analyst

- Auditor / Read-only User



### 3.2 System and External Actors



- Windows Endpoint Agent

- Identity / Directory Provider

- Windows Operating System

- Security Platform Backend Services

- Data Storage Services



---



# 4. Identity and Access Use Cases



## UC-001 — Authenticate to Security Platform



**Primary Actor:** Platform User



**Supporting Actors:** Security Platform Backend Services



**Goal:**  

Allow an authorized platform user to authenticate and establish an authenticated session.



**Preconditions:**

- The user has a valid platform identity or supported directory identity.

- The security platform is available.

- The authentication request is received through a secure communication channel.



**Main Flow:**

1. The user submits authentication credentials or supported authentication information.

2. The platform validates the authentication information.

3. The platform determines whether authentication succeeds.

4. The platform establishes an authenticated session or issues the appropriate authentication result.

5. The authentication activity is recorded where required for security auditing.



**Alternative / Exception Flow:**

- Invalid authentication information results in authentication failure.

- Disabled or unavailable identity results in access being denied.

- Authentication service failure results in an appropriate failure response.



**Security Considerations:**

- Secure credential handling

- Authentication failure handling

- Session/token protection

- Transport security

- Auditability

- No unnecessary disclosure of authentication information



**Scope Traceability:** Identity and Access; Secure Communication; Audit



---



## UC-002 — Authorize Platform Access



**Primary Actor:** Security Platform Backend Services



**Supporting Actors:** Platform User



**Goal:**  

Ensure an authenticated user can access only the functionality permitted by their assigned role and permissions.



**Preconditions:**

- The user has been authenticated.

- The user has an applicable role or permission assignment.



**Main Flow:**

1. The user requests a protected platform operation.

2. The platform validates the authenticated identity.

3. The platform determines the user's applicable role and permissions.

4. The platform evaluates whether the requested operation is authorized.

5. The platform permits or denies the operation.

6. The authorization result is recorded where appropriate.



**Alternative / Exception Flow:**

- Missing authorization results in access denial.

- Invalid or expired authentication results in access denial.

- An operation outside the user's permissions is denied.



**Security Considerations:**

- Least privilege

- RBAC

- Explicit authorization

- Default deny for unauthorized operations

- Separation of administrative and investigative responsibilities



**Scope Traceability:** IAM; Administrative Roles; Security Boundary



---



## UC-003 — Synchronize Directory Identity



**Primary Actor:** Identity / Directory Provider



**Supporting Actors:** Security Administrator, Security Platform Backend Services



**Goal:**  

Synchronize supported users and groups from an external identity or directory provider with the security platform.



**Preconditions:**

- A supported directory integration exists.

- The platform has the required configuration and authorization.

- The directory provider is available.



**Main Flow:**

1. The platform initiates or receives a synchronization operation.

2. The platform communicates with the directory provider.

3. User and group information is retrieved.

4. The platform validates the received information.

5. Relevant identity and group information is synchronized.

6. Synchronization results are recorded.



**Alternative / Exception Flow:**

- Directory connectivity failure is recorded.

- Invalid directory data is rejected or handled according to defined validation rules.

- Authorization failure prevents synchronization.



**Security Considerations:**

- Secure directory communication

- Credential protection

- Input validation

- Least privilege

- Auditability



**Scope Traceability:** IAM; LDAP/AD Integration Boundary



---



# 5. Endpoint Management Use Cases



## UC-004 — Register Endpoint



**Primary Actor:** Windows Endpoint Agent



**Supporting Actors:** Security Platform Backend Services, Windows Operating System



**Goal:**  

Register a Windows endpoint with the security platform and establish its platform identity.



**Preconditions:**

- The endpoint agent is installed.

- The endpoint can communicate with the platform.

- The endpoint can provide the required machine identity information.



**Main Flow:**

1. The endpoint agent starts or initiates registration.

2. The agent collects required endpoint identity information.

3. The agent sends a registration request to the platform.

4. The platform validates the request.

5. The platform creates or updates the endpoint record.

6. The platform returns the registration result.

7. The endpoint stores the required registration information securely.



**Alternative / Exception Flow:**

- Invalid registration request is rejected.

- Duplicate or previously registered endpoint is handled according to lifecycle rules.

- Communication failure prevents registration.



**Security Considerations:**

- Endpoint identity validation

- Agent authentication

- Secure transport

- Replay protection where required

- Registration auditing



**Scope Traceability:** Device Management; Windows Endpoint Agent; Security Boundary



---



## UC-005 — Manage Endpoint Identity and Lifecycle



**Primary Actor:** Security Administrator



**Supporting Actors:** Windows Endpoint Agent, Security Platform Backend Services



**Goal:**  

Manage the platform lifecycle state of registered endpoints.



**Preconditions:**

- The endpoint is known to the platform.



**Main Flow:**

1. The administrator views an endpoint.

2. The platform displays its current identity and lifecycle information.

3. The administrator performs an authorized lifecycle operation.

4. The platform validates authorization.

5. The platform updates the endpoint lifecycle state.

6. The action is recorded for audit purposes.



**Alternative / Exception Flow:**

- Unauthorized lifecycle operation is denied.

- Unknown endpoint cannot be managed as a registered endpoint.



**Security Considerations:**

- RBAC

- Endpoint identity integrity

- Administrative authorization

- Auditability



**Scope Traceability:** Device Management; Administrative Roles



---



## UC-006 — Report Endpoint Health



**Primary Actor:** Windows Endpoint Agent



**Supporting Actors:** Security Platform Backend Services



**Goal:**  

Provide current endpoint health and connectivity information to the platform.



**Preconditions:**

- The endpoint is registered.

- The agent is running.



**Main Flow:**

1. The agent gathers required health information.

2. The agent sends a health or heartbeat update.

3. The platform validates the endpoint identity.

4. The platform records the health information.

5. The platform updates the endpoint health state.



**Alternative / Exception Flow:**

- Missing heartbeat causes the endpoint to become stale or unavailable according to defined rules.

- Invalid or unauthenticated health information is rejected.



**Security Considerations:**

- Agent authentication

- Endpoint identity validation

- Data integrity

- Availability monitoring



**Scope Traceability:** Device Management; Endpoint Health



---



## UC-007 — Report Endpoint Security Posture



**Primary Actor:** Windows Endpoint Agent



**Supporting Actors:** Windows Operating System, Security Platform Backend Services



**Goal:**  

Provide security-relevant endpoint posture information used by the platform for security decisions.



**Preconditions:**

- The endpoint is registered.

- The agent can collect the required posture information.



**Main Flow:**

1. The agent collects supported posture information.

2. The agent validates the collected information.

3. The agent submits posture information to the platform.

4. The platform validates the source and data.

5. The platform stores or processes the posture information.

6. The posture becomes available for policy and trust evaluation.



**Alternative / Exception Flow:**

- Missing posture information is recorded.

- Invalid posture information is rejected or marked unreliable.

- Agent communication failure prevents posture submission.



**Security Considerations:**

- Integrity of posture information

- Endpoint identity

- Secure communication

- Trustworthiness of collected data



**Scope Traceability:** Device Posture; Zero Trust Evaluation



---



# 6. Policy Management Use Cases



## UC-008 — Create and Manage Security Policy



**Primary Actor:** Security Administrator



**Supporting Actors:** Security Platform Backend Services



**Goal:**  

Allow an authorized administrator to create and manage endpoint security policies.



**Preconditions:**

- The administrator is authenticated and authorized.

- The platform is available.



**Main Flow:**

1. The administrator creates or selects a policy.

2. The administrator defines supported policy rules.

3. The platform validates the policy.

4. The policy is stored.

5. The policy becomes available for assignment and evaluation.

6. Administrative activity is recorded.



**Alternative / Exception Flow:**

- Invalid policy configuration is rejected.

- Unauthorized user cannot create or modify the policy.



**Security Considerations:**

- RBAC

- Input validation

- Policy integrity

- Change auditing

- Least privilege



**Scope Traceability:** Policy Management; Administrative Roles



---



## UC-009 — Assign Policy to Endpoint or Group



**Primary Actor:** Security Administrator



**Supporting Actors:** Security Platform Backend Services



**Goal:**  

Associate a security policy with one or more supported endpoints or endpoint groups.



**Preconditions:**

- The policy exists.

- The target endpoint or group exists.

- The administrator is authorized.



**Main Flow:**

1. The administrator selects a policy.

2. The administrator selects the target endpoint or group.

3. The platform validates the assignment.

4. The platform records the policy assignment.

5. The assignment becomes available to the policy evaluation process.

6. The administrative action is audited.



**Alternative / Exception Flow:**

- Invalid target is rejected.

- Unauthorized assignment is denied.

- Invalid policy state prevents assignment.



**Security Considerations:**

- Authorization

- Policy integrity

- Assignment integrity

- Auditability



**Scope Traceability:** Policy Management; Endpoint Management



---



## UC-010 — Evaluate Security Policy



**Primary Actor:** Security Platform Backend Services



**Supporting Actors:** Windows Endpoint Agent, Security Administrator



**Goal:**  

Determine whether current endpoint and contextual information satisfies applicable security policy rules.



**Preconditions:**

- Applicable policies exist.

- Required endpoint/context information is available.



**Main Flow:**

1. The platform identifies applicable policies.

2. The platform retrieves relevant endpoint and contextual information.

3. Policy rules are evaluated.

4. The platform determines the policy result.

5. The result is made available to the appropriate enforcement or trust-decision process.

6. The evaluation is recorded where required.



**Alternative / Exception Flow:**

- Missing required information is handled according to defined policy behavior.

- Invalid policy cannot be evaluated and is reported.

- Conflicting policies are handled according to defined precedence rules.



**Security Considerations:**

- Deterministic policy evaluation

- Policy integrity

- Default-deny considerations

- Auditability



**Scope Traceability:** Policy Management; Zero Trust Evaluation



---



# 7. Zero Trust Use Cases



## UC-011 — Evaluate Zero Trust Context



**Primary Actor:** Security Platform Backend Services



**Supporting Actors:** Platform User, Windows Endpoint Agent, Identity / Directory Provider



**Goal:**  

Evaluate relevant identity, endpoint, posture, context, and policy information before determining access or enforcement behavior.



**Preconditions:**

- An identity or endpoint request exists.

- Relevant trust information is available or can be evaluated.



**Main Flow:**

1. The platform identifies the subject and endpoint.

2. The platform evaluates identity information.

3. The platform evaluates endpoint identity and posture.

4. The platform evaluates applicable security policy.

5. The platform considers relevant contextual information.

6. The platform produces the information required for the trust decision.



**Alternative / Exception Flow:**

- Missing or invalid trust information causes the decision to follow defined secure handling rules.

- Failed validation prevents the request from being treated as trusted.



**Security Considerations:**

- Never trust solely based on network location

- Identity verification

- Device verification

- Posture evaluation

- Least privilege

- Continuous or repeated evaluation where required



**Scope Traceability:** Zero Trust Evaluation



---



## UC-012 — Produce Trust Decision



**Primary Actor:** Security Platform Backend Services



**Supporting Actors:** Windows Endpoint Agent, Security Administrator



**Goal:**  

Produce an explicit security decision based on the evaluated trust context.



**Preconditions:**

- Required trust information has been evaluated.



**Main Flow:**

1. The platform receives the evaluated trust context.

2. Applicable policy rules are considered.

3. The platform determines the applicable decision.

4. The decision is represented as an allowed outcome such as Allow, Deny, or Restrict.

5. The decision is communicated to the applicable enforcement or consuming component.

6. The decision is recorded where required.



**Alternative / Exception Flow:**

- Insufficient trust information results in the defined secure fallback behavior.

- Policy evaluation failure is recorded and handled according to security rules.



**Security Considerations:**

- Explicit decision

- Default-deny where appropriate

- Decision integrity

- Auditability

- Least privilege



**Scope Traceability:** Zero Trust Evaluation; Policy Management



---



# 8. Endpoint Security Control Use Cases



## UC-013 — Enforce Endpoint Security Policy



**Primary Actor:** Windows Endpoint Agent



**Supporting Actors:** Windows Operating System, Security Platform Backend Services



**Goal:**  

Apply security decisions and assigned endpoint policies on the Windows endpoint.



**Preconditions:**

- The endpoint is registered.

- Applicable policy or decision information is available.

- The agent has the required operating-system permissions.



**Main Flow:**

1. The agent receives or obtains the applicable policy/decision.

2. The agent validates the policy or decision.

3. The agent applies the supported endpoint control.

4. The agent records the enforcement result.

5. Relevant telemetry is sent to the platform.



**Alternative / Exception Flow:**

- Invalid policy is not applied.

- Enforcement failure is recorded.

- Required operating-system capability is unavailable.



**Security Considerations:**

- Minimum required privileges

- Policy integrity

- Fail-secure behavior where appropriate

- Enforcement auditing



**Scope Traceability:** Endpoint Controls; Windows Endpoint Agent



---



## UC-014 — Control USB Activity



**Primary Actor:** Windows Endpoint Agent



**Supporting Actors:** Windows Operating System, Security Administrator



**Goal:**  

Apply configured security policy to supported USB device activity.



**Preconditions:**

- USB control policy exists and applies to the endpoint.

- The agent is operating.



**Main Flow:**

1. USB activity is detected.

2. The agent identifies the applicable endpoint policy.

3. The policy decision is evaluated.

4. The agent allows or blocks the supported USB activity.

5. The enforcement event is recorded.



**Alternative / Exception Flow:**

- Policy unavailable results in defined fallback behavior.

- Enforcement failure is recorded.



**Security Considerations:**

- Least privilege

- Policy integrity

- Auditability

- Controlled enforcement



**Scope Traceability:** Endpoint Controls — USB



---



## UC-015 — Control Website / Network Access



**Primary Actor:** Windows Endpoint Agent



**Supporting Actors:** Windows Operating System, Security Administrator



**Goal:**  

Apply configured policy to supported website or network access activity.



**Preconditions:**

- A network or website control policy applies to the endpoint.

- The agent is operating.



**Main Flow:**

1. Supported network or website activity is detected.

2. The agent determines the relevant destination or request context.

3. Applicable policy is evaluated.

4. The activity is allowed or blocked according to the policy.

5. The result is recorded.



**Alternative / Exception Flow:**

- Policy evaluation failure follows defined secure handling.

- Required enforcement mechanism is unavailable and the failure is recorded.



**Security Considerations:**

- Policy integrity

- Secure enforcement

- Avoidance of unintended access

- Auditability



**Scope Traceability:** Endpoint Controls — Website / Network



---



## UC-016 — Control File Access



**Primary Actor:** Windows Endpoint Agent



**Supporting Actors:** Windows Operating System, Security Administrator



**Goal:**  

Apply configured security policy to supported file access activity.



**Preconditions:**

- File-control policy applies to the endpoint.

- The agent has the required operating-system permissions.



**Main Flow:**

1. Supported file activity is detected.

2. The agent identifies the applicable policy.

3. The policy decision is evaluated.

4. The requested activity is allowed or restricted according to policy.

5. The result is recorded.



**Alternative / Exception Flow:**

- Unauthorized activity is blocked where the applicable policy requires blocking.

- Enforcement failure is recorded.



**Security Considerations:**

- Minimum operating-system privileges

- File permission integrity

- Policy integrity

- Auditability



**Scope Traceability:** Endpoint Controls — File Access



---



## UC-017 — Control Process / Application Activity



**Primary Actor:** Windows Endpoint Agent



**Supporting Actors:** Windows Operating System, Security Administrator



**Goal:**  

Apply configured policy to supported process or application activity.



**Preconditions:**

- A process/application policy applies to the endpoint.

- The agent can observe the supported activity.



**Main Flow:**

1. Supported process activity is detected.

2. The agent identifies the relevant process/application.

3. Applicable policy is evaluated.

4. The activity is allowed or restricted according to policy.

5. The enforcement result is recorded.



**Alternative / Exception Flow:**

- Unknown or invalid process information is handled according to defined policy behavior.

- Enforcement failure is recorded.



**Security Considerations:**

- Process identity validation

- Least privilege

- Policy integrity

- Auditability



**Scope Traceability:** Endpoint Controls — Process / Application



---



# 9. Security Telemetry and Investigation Use Cases



## UC-018 — Collect Endpoint Telemetry



**Primary Actor:** Windows Endpoint Agent



**Supporting Actors:** Windows Operating System



**Goal:**  

Collect supported security and operational telemetry from the Windows endpoint.



**Preconditions:**

- The agent is operating.

- The required telemetry source is available.



**Main Flow:**

1. The agent observes supported endpoint activity.

2. Relevant telemetry is collected.

3. The telemetry is normalized into the platform's supported event representation.

4. The telemetry is prepared for secure submission.

5. Collection status is recorded.



**Alternative / Exception Flow:**

- Unavailable telemetry source is recorded.

- Invalid event data is discarded or marked according to validation rules.



**Security Considerations:**

- Data integrity

- Minimum required collection

- Privacy-aware collection

- Secure handling



**Scope Traceability:** Endpoint Telemetry



---



## UC-019 — Submit Endpoint Telemetry



**Primary Actor:** Windows Endpoint Agent



**Supporting Actors:** Security Platform Backend Services, Data Storage Services



**Goal:**  

Securely submit collected endpoint telemetry to the security platform.



**Preconditions:**

- Telemetry has been collected.

- The endpoint is registered or otherwise authorized to submit telemetry.



**Main Flow:**

1. The agent prepares telemetry for transmission.

2. The agent authenticates to the platform.

3. Telemetry is transmitted through secure communication.

4. The platform validates the source and payload.

5. Accepted telemetry is processed and stored.

6. Submission status is returned or recorded.



**Alternative / Exception Flow:**

- Authentication failure prevents acceptance.

- Invalid payload is rejected.

- Temporary communication failure is handled according to supported retry behavior.



**Security Considerations:**

- Authentication

- TLS/secure transport

- Input validation

- Data integrity

- Controlled retention



**Scope Traceability:** Endpoint Telemetry; Data Storage; Secure Communication



---



## UC-020 — Review Security Events



**Primary Actor:** Security Analyst



**Supporting Actors:** Security Platform Backend Services, Data Storage Services



**Goal:**  

Allow a security analyst to review relevant security events and telemetry.



**Preconditions:**

- The analyst is authenticated and authorized.

- Security event data exists.



**Main Flow:**

1. The analyst requests security events.

2. The platform verifies authorization.

3. Relevant events are retrieved.

4. Events are presented for analysis.

5. The analyst filters or examines available information.

6. Relevant review activity is recorded where required.



**Alternative / Exception Flow:**

- Unauthorized access is denied.

- No matching events results in an empty result.

- Data retrieval failure is reported.



**Security Considerations:**

- Read access control

- Data confidentiality

- Auditability

- Least privilege



**Scope Traceability:** Security Dashboard; Telemetry; Security Analyst Persona



---



## UC-021 — Investigate Endpoint Activity



**Primary Actor:** Security Analyst



**Supporting Actors:** Windows Endpoint Agent, Security Platform Backend Services, Data Storage Services



**Goal:**  

Allow an authorized analyst to investigate endpoint activity using available telemetry, security events, endpoint information, and audit data.



**Preconditions:**

- The analyst is authorized.

- Relevant endpoint and event data is available.



**Main Flow:**

1. The analyst selects an endpoint or security event.

2. The platform retrieves relevant information.

3. The analyst examines associated telemetry and events.

4. The analyst correlates available information.

5. The analyst records or follows up on the investigation as supported by the platform.



**Alternative / Exception Flow:**

- Insufficient data is identified.

- Access to restricted information is denied.



**Security Considerations:**

- Least-privilege investigation access

- Data confidentiality

- Auditability

- Separation of administrative and investigative responsibilities



**Scope Traceability:** Security Telemetry; Security Dashboard; Security Analyst Persona



---



# 10. Audit and Monitoring Use Cases



## UC-022 — Review Audit Events



**Primary Actor:** Auditor / Read-only User



**Supporting Actors:** Security Platform Backend Services, Data Storage Services



**Goal:**  

Allow an authorized read-only user to review recorded security and administrative audit events.



**Preconditions:**

- The user is authenticated and authorized.

- Audit records exist.



**Main Flow:**

1. The auditor requests audit information.

2. The platform validates authorization.

3. Relevant audit events are retrieved.

4. The auditor reviews the events.

5. Audit information is presented without allowing unauthorized modification.



**Alternative / Exception Flow:**

- Unauthorized access is denied.

- Audit retrieval failure is reported.



**Security Considerations:**

- Read-only access

- Audit integrity

- Confidentiality

- Separation of duties



**Scope Traceability:** Audit; Auditor Persona



---



## UC-023 — Record Security and Administrative Actions



**Primary Actor:** Security Platform Backend Services



**Supporting Actors:** Platform Administrator, Security Administrator, Security Analyst



**Goal:**  

Record important security and administrative actions for accountability and investigation.



**Preconditions:**

- A supported auditable action occurs.



**Main Flow:**

1. A supported administrative or security action occurs.

2. The platform identifies the relevant actor and action.

3. Relevant audit information is captured.

4. The audit record is stored.

5. Authorized users can later review the record.



**Alternative / Exception Flow:**

- Audit storage failure is detected and handled according to operational requirements.

- Invalid audit data is rejected or flagged.



**Security Considerations:**

- Audit integrity

- Timestamp accuracy

- Actor identification

- Tamper resistance

- Appropriate retention



**Scope Traceability:** Audit; Security Boundary



---



## UC-024 — Monitor Endpoint and Platform Health



**Primary Actor:** Security Administrator



**Supporting Actors:** Windows Endpoint Agent, Security Platform Backend Services



**Goal:**  

Allow authorized administrators to monitor the operational health of endpoints and platform services.



**Preconditions:**

- Endpoint and platform health information is available.



**Main Flow:**

1. The administrator requests health information.

2. The platform retrieves current health data.

3. Endpoint and platform status information is presented.

4. The administrator identifies unavailable, stale, or unhealthy components.

5. Relevant operational actions can be initiated according to the user's authorization.



**Alternative / Exception Flow:**

- Missing heartbeat or health information is shown as unavailable or stale.

- Platform monitoring data is temporarily unavailable.



**Security Considerations:**

- Access control

- Integrity of health information

- Avoiding unauthorized operational actions

- Auditability of administrative changes



**Scope Traceability:** Endpoint Health; Platform Administration and Monitoring



---



# 11. Cross-Cutting Security Principles



The following principles apply across the use cases:



- Authentication must be required for protected platform operations.

- Authorization must be explicit and based on applicable roles and permissions.

- Least privilege should be applied to human users, services, and endpoint agents.

- Endpoint identity should be validated before accepting protected endpoint operations.

- Secure communication should be used for protected platform communication.

- Input received from endpoints, users, and external systems must be validated.

- Security-sensitive administrative and system actions should be auditable.

- Security decisions should not rely solely on network location.

- Zero Trust decisions should consider applicable identity, endpoint, posture, context, and policy information.

- Endpoint enforcement should use the minimum operating-system privileges required.

- Security-sensitive failures should follow defined secure handling rules.

- Access to telemetry and audit information must be controlled according to role and responsibility.



---



# 12. Use-Case Traceability Summary



| Use Case Area | Related Product Scope |

|---|---|

| UC-001–UC-003 | Identity and Access Management |

| UC-004–UC-007 | Endpoint / Device Management |

| UC-008–UC-010 | Policy Management |

| UC-011–UC-012 | Zero Trust Evaluation |

| UC-013–UC-017 | Endpoint Security Controls |

| UC-018–UC-021 | Endpoint Telemetry and Security Investigation |

| UC-022–UC-023 | Audit |

| UC-024 | Platform and Endpoint Monitoring |



---



# 13. Out-of-Scope Confirmation



The following are not introduced as use cases by this catalogue:



- Full commercial EDR replacement

- Full SIEM replacement

- Full MDM/UEM platform

- Mobile endpoint management

- macOS endpoint agent

- Linux endpoint agent

- Enterprise-scale SaaS operation

- Full PKI / certificate authority platform

- Advanced threat-intelligence platform

- Automated offensive security platform



These boundaries remain governed by DOC-002 — Product Scope.



---



# 14. Assumptions



- The initial endpoint target is Windows.

- The endpoint agent operates as a background security component without requiring a visible user interface.

- Platform access is controlled through authentication and authorization.

- Directory integration is treated as a supported integration boundary rather than a replacement for an enterprise identity provider.

- Endpoint telemetry is collected only within the defined product scope.

- Detailed policy semantics, API contracts, database structures, and implementation mechanisms will be defined in later phases.



---



# 15. Traceability to Product Discovery



This document is derived from:



**DOC-001 — Product Vision**



Defines why the platform exists and the security outcomes it targets.



**DOC-002 — Product Scope**



Defines the capabilities and boundaries represented by the use cases.



**DOC-003 — Product Personas**



Defines the human and system actors participating in the use cases.



The use-case catalogue provides the bridge from Product Discovery to the Requirements phase.



---



# 16. Document Status



**Status:** Draft — G1.3 baseline



The use cases are considered the current product-discovery baseline and may be refined during Requirements, Architecture, Threat Modeling, Security Architecture, and implementation.



Changes discovered during later phases must be reviewed for their impact on scope, requirements, architecture, security, testing, and traceability.



---



# 17. Change History



| Version | Date        | Change                                                                                     | Status |

|---------|-------------|--------------------------------------------------------------------------------------------|--------|

| v0.1    | 25-09-2026 | Initial use-case catalogue created from Product Vision, Product Scope, and Product Personas | Draft  |




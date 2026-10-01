# DOC-008 — Acceptance Criteria

**Document Version:** v0.1  
**Phase:** G2.3 — Acceptance Criteria  
**Status:** Draft  
**Date:** 01-10-2026  

## 1. Purpose

This document defines testable acceptance criteria for the approved product use cases and requirements.

Acceptance criteria describe observable conditions that must be satisfied for a use case or requirement to be considered accepted.

They do not define implementation details, API contracts, database schemas, framework choices, deployment mechanisms, or specific technology choices.

## 2. Source Documents

| Document | Purpose |
|---|---|
| DOC-001 | Product Vision |
| DOC-002 | Product Scope |
| DOC-003 | Personas |
| DOC-004 | Use Cases |
| DOC-005 | Functional Requirements |
| DOC-006 | Non-Functional Requirements |
| DOC-007 | Security Requirements |

## 3. Acceptance Principles

Acceptance criteria shall:

1. Be observable and testable.
2. Describe expected system behavior or outcome.
3. Respect the approved product scope.
4. Avoid implementation-specific decisions.
5. Cover successful and relevant failure conditions.
6. Support functional, security and operational verification.
7. Provide a basis for later test cases.
8. Preserve traceability to approved requirements.

## 4. Use-Case Acceptance Criteria

### UC-001 — Identity and Authentication

- Valid authorized users can authenticate successfully.
- Invalid authentication attempts are rejected.
- Authentication failures do not result in unauthorized access.
- Authentication activity can be associated with the relevant identity.
- Authentication behavior follows the approved security requirements.

### UC-002 — Authorization and Role-Based Access

- An authorized user can access functions permitted by their assigned role.
- A user cannot access functions outside their authorization.
- Authorization is evaluated using the applicable security context.
- Unauthorized access attempts are rejected and auditable.

### UC-003 — User and Group Management

- Authorized administrators can view applicable users and groups.
- Authorized administrators can perform permitted user or group management actions.
- Unauthorized users cannot perform administrative identity actions.
- Changes to identity or group information are auditable.

### UC-004 — Device Registration

- A supported Windows endpoint can register with the platform.
- The platform associates the registered endpoint with a unique device identity.
- Invalid or unauthorized registration attempts are rejected.
- Registration activity is auditable.

### UC-005 — Device Management

- Authorized administrators can view registered endpoints.
- Device status and relevant posture information can be retrieved.
- Unauthorized users cannot perform restricted device-management actions.
- Device-management actions are auditable.

### UC-006 — Endpoint Heartbeat and Health

- A registered endpoint can provide heartbeat information.
- The platform can determine whether the endpoint is reporting within the defined operational expectations.
- Missing or stale heartbeat information does not result in false healthy status.
- Endpoint health information is observable.

### UC-007 — Endpoint Posture

- The endpoint can provide supported posture information.
- Posture information is associated with the correct endpoint.
- Posture information can be used as an input to trust evaluation.
- Invalid or incomplete posture data is handled according to security requirements.

### UC-008 — Policy Creation and Management

- Authorized administrators can create supported security policies.
- Policies contain the required information for their intended purpose.
- Unauthorized users cannot create or modify restricted policies.
- Policy changes are auditable.

### UC-009 — Policy Assignment

- Authorized administrators can assign applicable policies to supported targets.
- Policy assignments are associated with the intended target.
- Unauthorized policy assignments are rejected.
- Policy assignment changes are auditable.

### UC-010 — Policy Evaluation

- Applicable policy conditions are evaluated when required.
- Policy evaluation produces a defined result.
- Conflicting or invalid policy conditions are handled according to approved requirements.
- Policy evaluation activity can be audited.

### UC-011 — Zero Trust Trust Evaluation

- Trust evaluation considers the approved identity, device, posture and policy context.
- A trust decision produces an explicit result.
- Trust decisions are associated with the relevant identity and endpoint.
- Security-relevant trust decisions are auditable.

### UC-012 — Allow, Deny and Restrict Decisions

- The platform can produce Allow, Deny and Restrict outcomes where applicable.
- Denied activity does not receive unauthorized access.
- Restricted activity is limited according to the applicable policy.
- Decision outcomes are observable and auditable.

### UC-013 — USB Control

- Authorized administrators can configure supported USB-control policies.
- The endpoint enforces the applicable USB-control decision.
- Unauthorized USB activity is prevented or restricted according to policy.
- USB-control events are recorded.

### UC-014 — Website Control

- Authorized administrators can configure supported website-control policies.
- The endpoint evaluates applicable website access against policy.
- Blocked website access is prevented according to policy.
- Website-control events are recorded.

### UC-015 — File Control

- Authorized administrators can configure supported file-control policies.
- The endpoint evaluates applicable file activity against policy.
- Unauthorized file activity is prevented or restricted according to policy.
- File-control activity is auditable.

### UC-016 — Process Control

- Authorized administrators can configure supported process-control policies.
- The endpoint evaluates applicable process activity against policy.
- Unauthorized process activity is prevented or restricted according to policy.
- Process-control activity is auditable.

### UC-017 — Telemetry Collection

- Supported endpoint telemetry can be collected from registered endpoints.
- Telemetry is associated with the correct endpoint and relevant event context.
- Unauthorized or malformed telemetry is not trusted as valid platform data.
- Telemetry collection failures are observable.

### UC-018 — Security Event Investigation

- Authorized security analysts can view relevant security events.
- Events provide sufficient context for investigation within the approved scope.
- Event information can be associated with the relevant endpoint and activity.
- Access to investigation information is authorized and auditable.

### UC-019 — Alerting

- Security-relevant conditions can produce alerts where defined.
- Alerts contain sufficient information to identify the relevant event context.
- Unauthorized users cannot modify restricted alert information.
- Alert activity is auditable.

### UC-020 — Audit and Compliance Records

- Security-relevant administrative and control actions generate audit records where required.
- Audit records identify the relevant actor, action, target and outcome where applicable.
- Unauthorized users cannot modify or delete protected audit information.
- Audit failures do not silently result in loss of required security records.

### UC-021 — Monitoring and Health

- Authorized users can view applicable platform and endpoint health information.
- Health information reflects the available operational state.
- Monitoring failures are observable.
- Monitoring information is protected according to authorization requirements.

### UC-022 — Security Configuration Management

- Authorized administrators can manage supported security configuration.
- Configuration changes are subject to authorization.
- Invalid configuration is rejected or handled safely.
- Configuration changes are auditable.

### UC-023 — Security Administration

- Authorized security administrators can perform permitted security-management actions.
- Privileged actions require the applicable authorization context.
- Unauthorized privileged actions are rejected.
- Security administration activity is auditable.

### UC-024 — Auditor / Read-Only Access

- An auditor can access information permitted by the read-only role.
- Read-only access cannot modify protected platform state.
- Restricted information remains inaccessible.
- Auditor activity is auditable where required.

## 5. Cross-Cutting Acceptance Criteria

### 5.1 Authorization

- Protected operations require authorization.
- Authorization decisions use the applicable security context.
- Privilege escalation through unauthorized operations is prevented.

### 5.2 Endpoint Security

- Endpoint commands are authorized before execution.
- Revoked endpoint trust prevents newly unauthorized privileged activity.
- Endpoint failures do not silently create unauthorized access.

### 5.3 Data Protection

- Sensitive information is protected according to approved security requirements.
- Secrets are not exposed through ordinary operational output.
- Security-sensitive data handling follows approved data-protection requirements.

### 5.4 Auditability

- Required security events are recorded.
- Audit records remain protected from unauthorized modification.
- Audit persistence failures are detectable.

### 5.5 Resilience

- Backend or security-service unavailability does not grant new privileged access.
- Security decisions fail according to the approved secure-failure expectations.
- Relevant failures remain observable.

### 5.6 Input Validation

- Untrusted input is validated before security-sensitive processing.
- Invalid or malformed input is rejected or safely handled.
- Input validation failures do not result in unauthorized behavior.

## 6. Verification Methods

Acceptance criteria may be verified through:

- Unit testing
- Integration testing
- API testing
- Agent testing
- Security testing
- Negative testing
- Authorization testing
- Endpoint control testing
- Audit verification
- Observability and resilience testing
- Manual acceptance testing where appropriate

## 7. Acceptance Status

A requirement or use case is considered accepted when:

1. Its applicable acceptance criteria are satisfied.
2. Required verification evidence exists.
3. Security requirements are satisfied.
4. No known unresolved critical defect prevents acceptance.
5. The result remains within the approved product scope.

## 8. Traceability

Detailed requirement-to-use-case-to-acceptance-test mapping will be maintained in the G2.4 Requirements Traceability Matrix.

## 9. Out of Scope

Acceptance of this product does not imply replacement of commercial EDR, XDR, SIEM or MDM platforms.

The acceptance criteria apply only to functionality defined within the approved product scope.

## 10. Document Status

**Status:** Draft  
**Next Review:** G2.3 review  
**Next Phase:** G2.4 Requirements Traceability

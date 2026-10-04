# G3 — Epics

**Document Version:** v0.1  
**Phase:** G3 — Epics  
**Product:** Enterprise Endpoint Security & Zero Trust Platform  
**Status:** Draft  
**Date:** 04-10-2026

---

## 1. Purpose

This document defines the product-level epics for the Enterprise Endpoint Security & Zero Trust Platform.

The epics decompose the approved use-case catalogue into manageable product workstreams for subsequent backlog planning and user-story definition.

The epics do not introduce new product capabilities. They provide a planning structure derived from the approved G1 use cases and G2 requirements.

---

## 2. Epic Structure

| Epic ID | Epic Name | Primary Use Cases |
|---|---|---|
| EPIC-001 | Identity & Access Management | UC-001, UC-002, UC-003 |
| EPIC-002 | Endpoint & Device Management | UC-004, UC-005, UC-006, UC-007 |
| EPIC-003 | Security Policy Management | UC-008, UC-009, UC-010 |
| EPIC-004 | Zero Trust Evaluation & Decisions | UC-011, UC-012 |
| EPIC-005 | Endpoint Security Controls | UC-013, UC-014, UC-015, UC-016, UC-017 |
| EPIC-006 | Security Telemetry & Investigation | UC-018, UC-019, UC-020, UC-021 |
| EPIC-007 | Audit & Accountability | UC-022, UC-023 |
| EPIC-008 | Health & Operational Monitoring | UC-024 |

---

## 3. EPIC-001 — Identity & Access Management

### Objective

Provide controlled and auditable access to the security platform while supporting integration with an external identity or directory provider.

### Included Use Cases

- UC-001 — Authenticate to Security Platform
- UC-002 — Authorize Platform Access
- UC-003 — Synchronize Directory Identity

### Scope

This epic covers platform authentication, authorization, role-based access, and directory identity synchronization.

### Boundary

The platform consumes identity information from supported external identity or directory providers. It does not replace the organization's identity provider or directory service.

### Requirement Traceability

Primary requirement areas:

- Identity and access functional requirements
- Authentication and authorization security requirements
- Relevant security, privacy, auditability, and access-control NFRs

---

## 4. EPIC-002 — Endpoint & Device Management

### Objective

Establish and maintain the identity, lifecycle, health, and security posture of managed Windows endpoints.

### Included Use Cases

- UC-004 — Register Endpoint
- UC-005 — Manage Endpoint Identity and Lifecycle
- UC-006 — Report Endpoint Health
- UC-007 — Report Endpoint Security Posture

### Scope

This epic covers endpoint registration, endpoint identity, lifecycle state, health reporting, and security posture reporting.

### Boundary

The initial endpoint target is Windows. Mobile, macOS, and Linux endpoint agents are outside the current product scope.

### Requirement Traceability

Primary requirement areas:

- Endpoint management functional requirements
- Endpoint identity and posture security requirements
- Endpoint health, reliability, compatibility, and observability NFRs

---

## 5. EPIC-003 — Security Policy Management

### Objective

Provide a centralized mechanism for defining, assigning, and evaluating security policies for managed endpoints.

### Included Use Cases

- UC-008 — Create and Manage Security Policy
- UC-009 — Assign Policy to Endpoint or Group
- UC-010 — Evaluate Security Policy

### Scope

This epic covers security policy lifecycle, policy assignment, and policy evaluation.

### Boundary

Detailed policy semantics, API contracts, persistence schemas, and implementation mechanisms are defined in later architecture and implementation phases.

### Requirement Traceability

Primary requirement areas:

- Security policy functional requirements
- Policy management security requirements
- Policy integrity, consistency, availability, and auditability NFRs

---

## 6. EPIC-004 — Zero Trust Evaluation & Decisions

### Objective

Evaluate endpoint and contextual security information to produce explicit trust decisions.

### Included Use Cases

- UC-011 — Evaluate Zero Trust Context
- UC-012 — Produce Trust Decision

### Scope

This epic covers contextual trust evaluation and the resulting security decision.

Supported decision outcomes are defined at the product level as:

- Allow
- Deny
- Restrict

### Boundary

The epic defines the product capability and decision behavior. Detailed trust signals, evaluation algorithms, implementation architecture, and enforcement mechanisms are defined in later phases.

### Requirement Traceability

Primary requirement areas:

- Zero Trust functional requirements
- Trust evaluation and decision security requirements
- Security, resilience, performance, and auditability NFRs

---

## 7. EPIC-005 — Endpoint Security Controls

### Objective

Apply approved security policies to endpoint activities and enforce endpoint security controls.

### Included Use Cases

- UC-013 — Enforce Endpoint Security Policy
- UC-014 — Control USB Activity
- UC-015 — Control Website / Network Access
- UC-016 — Control File Access
- UC-017 — Control Process / Application Activity

### Scope

This epic covers policy enforcement and endpoint controls for:

- USB activity
- Website and network access
- File access
- Process and application activity

### Boundary

The epic does not represent a full commercial EDR/XDR, SIEM, MDM/UEM, or offensive security platform.

### Requirement Traceability

Primary requirement areas:

- Endpoint control functional requirements
- Endpoint enforcement security requirements
- Endpoint security, reliability, compatibility, and auditability NFRs

---

## 8. EPIC-006 — Security Telemetry & Investigation

### Objective

Collect, submit, review, and investigate endpoint security telemetry and events.

### Included Use Cases

- UC-018 — Collect Endpoint Telemetry
- UC-019 — Submit Endpoint Telemetry
- UC-020 — Review Security Events
- UC-021 — Investigate Endpoint Activity

### Scope

This epic covers endpoint telemetry collection, telemetry submission, security event review, and investigation workflows.

### Boundary

The platform provides security telemetry and investigation capabilities but is not defined as a full commercial SIEM replacement.

### Requirement Traceability

Primary requirement areas:

- Telemetry and security-event functional requirements
- Telemetry integrity and protection security requirements
- Performance, scalability, observability, retention, privacy, and auditability NFRs

---

## 9. EPIC-007 — Audit & Accountability

### Objective

Provide an auditable record of security-relevant and administrative actions performed within the platform.

### Included Use Cases

- UC-022 — Review Audit Events
- UC-023 — Record Security and Administrative Actions

### Scope

This epic covers audit-event generation, recording, and review for security and administrative activities.

### Boundary

Audit capability is limited to the platform's defined security and administrative activities. It does not constitute a general-purpose enterprise SIEM.

### Requirement Traceability

Primary requirement areas:

- Audit functional requirements
- Audit integrity and accountability security requirements
- Auditability, retention, privacy, and data-protection NFRs

---

## 10. EPIC-008 — Health & Operational Monitoring

### Objective

Provide visibility into the operational health of managed endpoints and platform components.

### Included Use Cases

- UC-024 — Monitor Endpoint and Platform Health

### Scope

This epic covers health monitoring for:

- Managed endpoints
- Endpoint agents
- Platform services
- Relevant operational components

### Boundary

Detailed logging, metrics, health-check implementation, and OpenTelemetry architecture are deferred to later architecture, observability, and implementation phases.

### Requirement Traceability

Primary requirement areas:

- Health and monitoring functional requirements
- Availability, reliability, resilience, performance, and observability NFRs
- Operational monitoring security requirements

---

## 11. Epic-to-Use-Case Coverage

All approved G1 use cases are assigned to exactly one primary epic.

| Use Case Range | Epic |
|---|---|
| UC-001–UC-003 | EPIC-001 |
| UC-004–UC-007 | EPIC-002 |
| UC-008–UC-010 | EPIC-003 |
| UC-011–UC-012 | EPIC-004 |
| UC-013–UC-017 | EPIC-005 |
| UC-018–UC-021 | EPIC-006 |
| UC-022–UC-023 | EPIC-007 |
| UC-024 | EPIC-008 |

**Coverage:** 24 of 24 approved use cases assigned.

---

## 12. Planning Boundaries

The following are intentionally not separate product epics at this stage:

- React dashboard
- Backend API
- Windows agent
- MySQL
- MongoDB
- SignalR
- Docker
- Kubernetes
- OpenTelemetry
- GitHub Actions

These are implementation, architecture, infrastructure, or technology concerns and will be addressed in their appropriate G4–G23 phases.

---

## 13. Relationship to Previous Baselines

This document is derived from:

- DOC-002 — Product Scope
- DOC-004 — Use-Case Catalogue
- DOC-005 — Functional Requirements
- DOC-006 — Non-Functional Requirements
- DOC-007 — Security Requirements
- DOC-008 — Acceptance Criteria
- DOC-009 — Requirements Traceability

The epic structure must not expand the approved product scope without a corresponding requirements and scope review.

---

## 14. G3.1 Preparation

The next backlog activity is **G3.1 — User Stories**.

User stories will be derived from these epics and their associated use cases and requirements.

No user stories are defined in this document.

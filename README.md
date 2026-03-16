# SparkFlow.Server

SparkFlow.Server is a backend orchestration server built with **ASP.NET Core** and structured using **Clean Architecture** principles.  
It is responsible for coordinating **workers**, managing **sessions**, executing **flows**, enforcing **policies**, and exposing operational **metrics**.

The project is designed to act as a central scheduling and execution server for distributed worker nodes.

---

## Overview

SparkFlow.Server manages the full lifecycle of execution across the system:

- Registering and monitoring worker nodes
- Assigning sessions to available workers
- Starting, completing, and failing sessions
- Managing accounts and flow definitions
- Applying execution and scheduling policies
- Recovering timed-out or interrupted sessions
- Exposing logs and metrics for observability

---

## Architecture

The project follows a layered architecture inspired by:

- Clean Architecture
- Domain-Driven Design (DDD)
- CQRS-style application features

### Layers

#### 1. API Layer
Responsible for:
- Exposing HTTP endpoints
- Middleware pipeline
- Request/response mapping
- Authentication and request tracing
- Swagger/OpenAPI configuration

Main folders:
- `src/Api/Endpoints`
- `src/Api/Middleware`
- `src/Api/OpenApi`
- `src/Api/BackgroundServices`

#### 2. Application Layer
Responsible for:
- Use cases and business workflows
- Commands / Queries / Handlers
- Validation behaviors
- Metrics and logging behaviors
- Scheduling and assignment services

Main folders:
- `src/Application/Features`
- `src/Application/Services`
- `src/Application/Pipelines`
- `src/Application/Scheduling`

#### 3. Domain Layer
Responsible for:
- Core business entities
- Domain events
- Rules and invariants
- Value objects
- Business enums and exceptions

Main folders:
- `src/Domain/Entities`
- `src/Domain/Events`
- `src/Domain/Rules`
- `src/Domain/ValueObjects`

#### 4. Infrastructure Layer
Responsible for:
- Persistence implementation
- JSON storage
- Metrics providers
- Cryptography
- Time and locking implementations
- Dependency injection setup

Main folders:
- `src/Infrastructure/Persistence`
- `src/Infrastructure/Metrics`
- `src/Infrastructure/Crypto`
- `src/Infrastructure/Configuration`

#### 5. Contracts Layer
Responsible for:
- DTOs
- API requests/responses
- Shared transport contracts between client/workers and server

Main folders:
- `src/Contracts`

---

## Architecture Diagram

```text
                 +----------------------+
                 |      Clients /       |
                 |      Worker Nodes    |
                 +----------+-----------+
                            |
                            v
                 +----------------------+
                 |      API Layer       |
                 |  Endpoints/Middleware|
                 +----------+-----------+
                            |
                            v
                 +----------------------+
                 |   Application Layer  |
                 | Commands / Queries   |
                 | Services / Handlers  |
                 +----------+-----------+
                            |
                            v
                 +----------------------+
                 |     Domain Layer     |
                 | Entities / Rules /   |
                 | Events / ValueObjects|
                 +----------+-----------+
                            |
                            v
                 +----------------------+
                 | Infrastructure Layer |
                 | JSON Store / Metrics |
                 | Crypto / Locking     |
                 +----------------------+

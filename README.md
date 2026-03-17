# SparkFlow.Server

SparkFlow.Server is the orchestration backend for SparkFlow workers.  
It manages accounts, flows, sessions, workers, policies, logs, scheduling, recovery, and health/metrics endpoints through a PostgreSQL-backed API. :contentReference[oaicite:1]{index=1}

## Features

- Account management
- Flow publishing and retrieval
- Session lifecycle management
- Worker registration, heartbeat, and session assignment
- Policy storage and evaluation
- Log ingestion
- Background scheduling and recovery services
- PostgreSQL persistence with EF Core migrations
- Swagger/OpenAPI support
- API key and worker authentication middleware
- Health and metrics endpoints :contentReference[oaicite:2]{index=2}

## Architecture

The project is organized into clear layers:

- **Api**  
  Minimal API endpoints, middleware, background services, OpenAPI configuration, and request/response mapping.

- **Application**  
  Use cases, handlers, validators, scheduling logic, services, abstractions, and transaction boundaries.

- **Domain**  
  Core entities, value objects, enums, rules, and domain events.

- **Infrastructure**  
  Persistence, repositories, EF Core DbContext, unit of work, crypto, metrics, locking, configuration, and serialization.

- **Contracts**  
  DTOs and request/response contracts shared with clients such as workers. :contentReference[oaicite:3]{index=3}

## Project Structure

```text
src/
  Api/
    BackgroundServices/
    DependencyInjection/
    Endpoints/
    Mapping/
    Middleware/
    OpenApi/

  Application/
    Abstractions/
    Features/
    Pipelines/
    Scheduling/
    Services/

  Contracts/
    Accounts/
    Bootstrap/
    Common/
    Flows/
    Logs/
    Sessions/
    Workers/

  Domain/
    Common/
    Entities/
    Enums/
    Events/
    Rules/
    ValueObjects/

  Infrastructure/
    Configuration/
    Crypto/
    DependencyInjection/
    Locking/
    Metrics/
    Persistence/
    Serialization/
    Time/

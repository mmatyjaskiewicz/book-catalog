# Book Catalog API

REST API for managing a book catalog, built with ASP.NET Core.

## Features

- Create, read, update and delete books
- Author management
- User management
- Book borrowing and returning
- Active loan and loan history endpoints
- Filtering and pagination
- Request validation with FluentValidation
- Global exception handling
- PostgreSQL persistence with Entity Framework Core
- Database migrations
- Concurrency protection for active book loans
- Swagger / OpenAPI
- Unit tests with xUnit and Moq
- Docker support with Docker Compose

## Structure

The project is divided into:

- **Domain** – entities and domain rules
- **Application** – business logic, DTOs and abstractions
- **Infrastructure** – database configuration, EF Core and repository implementations
- **WebApi** – controllers and API configuration
- **UnitTests** – unit tests for services and validators

## Documentation

More details can be found in:

- [`DESIGN_NOTE.md`](DESIGN_NOTE.md) – development notes and design decisions
- [`MERGE_REQUEST_HISTORY.md`](MERGE_REQUEST_HISTORY.md) – initial GitLab Merge Request history

## Project History

The project was initially developed on GitLab and later migrated to GitHub. Further development continues on GitHub.

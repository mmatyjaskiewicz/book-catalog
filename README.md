# Book Catalog API

REST API for managing a book catalog, built with ASP.NET Core.

## Features

- Create, read, update and delete books
- Request validation with FluentValidation
- Global exception handling
- Swagger / OpenAPI
- In-memory data storage

## Structure

The project is divided into:

- **Domain** – entities and domain rules
- **Application** – business logic, DTOs and abstractions
- **Infrastructure** – repository implementation
- **WebApi** – controllers and API configuration

## Documentation

More details can be found in:

- [`DESIGN_NOTE.md`](DESIGN_NOTE.md) – development notes and design decisions
- [`MERGE_REQUEST_HISTORY.md`](MERGE_REQUEST_HISTORY.md) – initial GitLab Merge Request history

## Project History

The project was initially developed on GitLab and later migrated to GitHub. Further development continues on GitHub.

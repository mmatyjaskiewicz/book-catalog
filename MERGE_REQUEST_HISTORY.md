# Merge Request History

This document is a readable archive of the Merge Requests created during the initial development of the project on GitLab.

> The project was initially developed on GitLab and was later migrated to GitHub for backup and continuity purposes. Further development continues on GitHub.

## !1 — chore: establish project foundation

**Status:** Merged  
**Branch:** `chore/project-foundation` → `main`  
**Author:** Maksym Matyjaskiewicz  
**Created:** 2026-08-19 09:16:19 UTC  
**Original MR:** https://gitlab.akvelon.net/m.matyjaskiewicz/book-catalog/-/merge_requests/1

### Description

No description was provided.

---

## !2 — feat: implement book domain

**Status:** Merged  
**Branch:** `feature/book-domain` → `main`  
**Author:** Maksym Matyjaskiewicz  
**Created:** 2026-08-19 10:51:46 UTC  
**Original MR:** https://gitlab.akvelon.net/m.matyjaskiewicz/book-catalog/-/merge_requests/2

### Description

No description was provided.

---

## !3 — feat: implement book application layer

**Status:** Merged  
**Branch:** `feature/book-application` → `main`  
**Author:** Maksym Matyjaskiewicz  
**Created:** 2026-08-19 18:09:49 UTC  
**Original MR:** https://gitlab.akvelon.net/m.matyjaskiewicz/book-catalog/-/merge_requests/3

### Description

## What was added:

- Added IBookRepository abstraction
- Added BookService
- Added create, get all, get by id and delete operations
- Added book update flow
- Added application exceptions
- Added request DTOs for create and update
- Added validation and not-found handling in the application layer

## Notes

- Domain validation remains inside the Book entity
- Repository is abstracted behind IBookRepository
- Update supports partial changes through PATCH-style request handling

---

## !4 — feat: implement fake book repository

**Status:** Merged  
**Branch:** `feature/infrastructure` → `main`  
**Author:** Maksym Matyjaskiewicz  
**Created:** 2026-08-20 10:40:56 UTC  
**Original MR:** https://gitlab.akvelon.net/m.matyjaskiewicz/book-catalog/-/merge_requests/4

### Description

## What was added

- Added `FakeBookRepository` implementing `IBookRepository`
- Added in-memory book storage using `List<Book>`
- Implemented create, get all, get by id and delete operations
- Implemented update operation for the in-memory repository

---

## !5 — feat: configure application dependency injection

**Status:** Merged  
**Branch:** `feature/extensions` → `main`  
**Author:** Maksym Matyjaskiewicz  
**Created:** 2026-08-20 10:54:01 UTC  
**Original MR:** https://gitlab.akvelon.net/m.matyjaskiewicz/book-catalog/-/merge_requests/5

### Description

## What was added

- Added `PersistenceExtensions` for repository registration
- Added `ServicesExtensions` for application service registration
- Added `AddApplicationModules` to aggregate application module registrations
- Registered `IBookRepository` with `FakeBookRepository`
- Registered `BookService` in the DI container
- Integrated application module registration into `Program.cs`

## Notes

- `Program.cs` only depends on the application module registration entry point

---

## !6 — feat: add book controller

**Status:** Merged  
**Branch:** `feature/book-controller` → `main`  
**Author:** Maksym Matyjaskiewicz  
**Created:** 2026-08-20 11:09:37 UTC  
**Original MR:** https://gitlab.akvelon.net/m.matyjaskiewicz/book-catalog/-/merge_requests/6

### Description

## What was added

- Added `BookController` with CRUD endpoints
- Added `GET /api/Book` for retrieving all books
- Added `GET /api/Book/{id}` for retrieving a book by ID
- Added `POST /api/Book` for creating books
- Added `PATCH /api/Book/{id}` for partial book updates
- Added `DELETE /api/Book/{id}` for deleting books

---

## !7 — feat: add global exception handler

**Status:** Merged  
**Branch:** `feature/exception-handler` → `feature/swagger`  
**Author:** Maksym Matyjaskiewicz  
**Created:** 2026-08-20 12:33:50 UTC  
**Original MR:** https://gitlab.akvelon.net/m.matyjaskiewicz/book-catalog/-/merge_requests/7

### Description

## Summary

- add global exception handler using `IExceptionHandler`
- handle `BadRequestException` with HTTP 400
- handle `NotFoundException` with HTTP 404
- handle unexpected exceptions with HTTP 500
- register the handler in dependency injection
- add exception handling middleware to the request pipeline

## Notes

The handler provides a consistent JSON error response for handled and unhandled exceptions.

---

## !8 — feat: complete swagger configuration

**Status:** Merged  
**Branch:** `feature/swagger` → `main`  
**Author:** Maksym Matyjaskiewicz  
**Created:** 2026-08-20 12:42:43 UTC  
**Original MR:** https://gitlab.akvelon.net/m.matyjaskiewicz/book-catalog/-/merge_requests/8

### Description

## Summary

- configure Swagger/OpenAPI
- document controller response status codes
- add global exception handling
- document successful and error responses for book endpoints

## Endpoints

- GET /api/Book
- GET /api/Book/{id}
- POST /api/Book
- PATCH /api/Book/{id}
- DELETE /api/Book/{id}

---

## !9 — Feature/input validation

**Status:** Merged  
**Branch:** `feature/input-validation` → `main`  
**Author:** Maksym Matyjaskiewicz  
**Created:** 2026-08-20 16:16:20 UTC  
**Original MR:** https://gitlab.akvelon.net/m.matyjaskiewicz/book-catalog/-/merge_requests/9

### Description

## Summary

- add FluentValidation validators for book create and update requests
- add global validation action filter
- register validators through dependency injection
- add validation rules for title, author and year
- ensure PATCH requests validate only provided fields
- return consistent 400 Bad Request responses for validation failures

## Testing

- verified validation responses through Swagger
- verified invalid POST requests return 400
- verified invalid PATCH requests return 400
- verified valid requests continue to the controller

---

## !10 — feat: add application logging

**Status:** Merged  
**Branch:** `feature/logging` → `main`  
**Author:** Maksym Matyjaskiewicz  
**Created:** 2026-08-20 18:07:42 UTC  
**Original MR:** https://gitlab.akvelon.net/m.matyjaskiewicz/book-catalog/-/merge_requests/10

### Description

## Summary

- added application logging using `ILogger<T>`
- added error logging for unhandled exceptions
- added warning logs for expected missing-book scenarios
- added information logs for successful book operations
- verified logging behavior through Swagger and console output

## Testing

- verified `Information` logs for successful book creation
- verified `Warning` logs for missing books
- verified `Error` logs and stack traces for unhandled exceptions
- verified existing API behavior remains unchanged

---

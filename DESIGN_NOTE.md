## Week 1

### What I built

Built the initial REST API for managing books with basic CRUD operations.

The project was structured into Domain, Application, Infrastructure and WebApi layers, with each layer having a clearly defined responsibility.

I also added request DTOs, input validation with FluentValidation, global exception handling, Swagger and application logging.

For data storage, I implemented an in-memory repository, which is sufficient for the current stage of the project.

### Key decisions

I chose a layered structure to keep responsibilities separated and make the application easier to extend later.

The application uses an `IBookRepository` abstraction instead of depending directly on the repository implementation.

For now, the repository stores books in memory. This keeps the implementation simple while allowing persistent storage to be introduced later without changing the application logic.

I also decided to keep validation and exception handling outside of the main business logic, so that `BookService` can focus mainly on application operations.

### What was difficult

Most of the work in the first week was quite intuitive for me. I was able to understand the structure and implement the features without any major blockers.

The main challenge was logging. It was a new area for me, so I needed to spend more time researching how logging works, where it should be used and which events are actually worth logging.

It was not something that blocked my progress for a long time, but compared to the rest of the work, it was the part that required the most additional learning.

### Additional Notes

The project was initially developed and maintained on GitLab during the first week.

After completing the initial development stage, the project was migrated to GitHub for backup and continuity purposes. Further development will continue on GitHub from this point forward.

The original GitLab repository will remain as a reference for the initial development history.

# Week 2

## What I built

Extended the REST API with pagination and filtering for the book list endpoint.

The endpoint now supports filtering books by title, author and year, as well as pagination with configurable page numbers and page sizes.

I also added validation for pagination parameters and handling for cases where the requested page is outside the available range.

I introduced unit testing with xUnit and Moq, covering the main `BookService` operations and FluentValidation validators, including successful operations and expected exceptions.

## Key decisions

I kept filtering and pagination inside the repository layer so that the `BookService` can focus on application-level logic and validation.

For title and author filtering, I used case-insensitive partial matching, while year filtering requires an exact match.

For pagination, the repository returns both the requested items and the total number of matching books, allowing the service to validate the requested page and provide pagination information to the client.

For unit tests, I used mocks to isolate `BookService` from its repository and logging dependencies. This allows the service logic to be tested without relying on the actual repository implementation.

I also separated validator tests from service tests, since validators can be tested independently without external dependencies.

## What was difficult

The most important learning area during this week was unit testing. I had limited experience with xUnit and Moq, so I needed to improve my understanding of how unit tests are structured and used.

Another useful discovery came from testing the PATCH functionality. The tests helped identify that the update validator was treating a partial update as if all fields were required.

I adjusted the validation rules so that fields are validated only when they are provided, while the service still prevents an empty PATCH request.

## Additional Notes

The changes from this week were developed through separate feature branches and merged into `development` before being promoted to `main`.

The project now has initial unit test coverage for the application service and validators, providing a foundation for adding more tests as the project grows.

# Week 3

## What I built

Reworked the persistence layer and extended the application with support for authors, users and book loans.

The previous in-memory storage was replaced with Entity Framework Core and PostgreSQL, allowing data to persist between application restarts.

I introduced EF Core entity configurations and migrations so that the database schema can be created and updated in a controlled way.

The domain model was extended with `Author`, `User` and `Loan` entities, together with the required relationships between books, authors, users and loans.

The lending functionality now supports borrowing books, returning them, retrieving active loans and retrieving loan history.

I separated active loans from returned loans by introducing `ArchivedLoan`. Active loans are stored in `loans`, while completed loans are moved to `archived_loans`.

The repository layer was refactored to use a generic `IRepository<T>` abstraction for common CRUD operations, while entity-specific repositories continue to contain custom query logic.

Database-side filtering and pagination use `IQueryable`, `CountAsync`, `Skip` and `Take`, so the database performs the filtering and pagination instead of loading unnecessary records into memory.

I also added protection against two users borrowing the same book at the same time using a unique database constraint.

PostgreSQL constraint violations are translated into an application-level concurrency exception and returned to the client as HTTP 409 Conflict.

Returning a book now removes the active loan and creates an archived loan as part of a single `SaveChangesAsync()` operation.

I added Docker support for both the WebApi and PostgreSQL, allowing the application and database to be started together using Docker Compose.

Configuration such as the database connection is provided through environment variables, keeping environment-specific settings outside the application code.

I also expanded Swagger response documentation for the new endpoints and their possible HTTP responses.

Finally, I extended the unit test coverage for the application services and validators to cover the new functionality and additional edge cases.

## Key decisions

I chose PostgreSQL with Entity Framework Core as the persistent storage solution because the application now requires relational data, foreign keys and database-level constraints.

I kept the application dependent on repository abstractions rather than directly depending on Entity Framework Core, allowing the application layer to remain independent from the persistence implementation.

I introduced a generic repository for operations shared between entities, while keeping filtering, pagination and other entity-specific queries in dedicated repositories.

For active loans, I used a unique index on `BookId` so that the database itself guarantees that a book cannot have multiple active loans.

I decided to keep returned loans in a separate `archived_loans` table instead of keeping both active and completed loans in the same table. This keeps the active loan table focused on the current state while preserving the complete lending history separately.

For returning a book, both database changes are saved through one `SaveChangesAsync()` call. This allows EF Core to handle the operation transactionally without introducing an additional Unit of Work abstraction.

For pagination and filtering, I kept the operations database-side to avoid loading records that are not required by the requested page.

Docker was added so that the application can be run together with a PostgreSQL instance using the same Compose configuration instead of requiring the database to be installed and configured manually.

I also kept the unit tests independent from PostgreSQL. The application services can therefore still be tested using mocks without requiring a running database.

## What was difficult

The most difficult part of this week was the refactoring caused by introducing the new domain entities and relationships.

At the beginning of the project, I did not yet have a complete understanding of all of the requirements and the full scope of the final data model. Because of that, the initial implementation was built around a much simpler `Book` model.

When the complete requirements became clear, I had to rethink part of the existing architecture instead of simply adding a few new endpoints.

Introducing `Author`, `User`, `Loan` and later `ArchivedLoan` affected several layers of the application.

I was initially surprised by how much of the existing code had to be refactored to support the new requirements. It showed me how changes to the domain model can have a significant impact on the rest of the application, even when the existing architecture is already separated into layers.

I also had to investigate how to handle concurrent borrowing correctly. A simple application-level check was not enough because two requests could pass the check at the same time. The final solution moved this guarantee to the database through a unique constraint and mapped the resulting PostgreSQL exception to a proper application-level conflict response.

Overall, this week required much more refactoring than the previous stages, but it also gave me a better understanding of how domain changes affect the overall architecture.

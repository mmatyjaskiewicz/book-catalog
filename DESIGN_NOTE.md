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

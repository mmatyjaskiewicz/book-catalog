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

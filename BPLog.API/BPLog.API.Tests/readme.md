# Blood Pressure API Tests
Here you can find tests that cover API methods and services.  
See test methods descriptions in code to find out why and what tests do.


# Fixtures

`InMemoryDbFixture` fixture provides in-memory database with prepared data, so it can be shared across different tests.

By default, 6 users and 10 blood pressure measurements per user are being populated.  

Default user password: `123`


# Libraries
- XUnit
- FluentAssertion
- Moq

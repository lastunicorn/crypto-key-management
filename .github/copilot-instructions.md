## Code Conventions

- Do not use `var` keyword. Whenever possible, use the actual type.

## Unit Tests

- When using `Assert.Throws` method, always use a block body for the lambda expression.

- For each method that is tested, create a different test file.
  - Ex: for a method called `Query()` create test file `QueryTests`
- All the test files for a single class should be placed in a directory with the name of the class.
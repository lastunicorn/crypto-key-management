## Code Conventions

- Do not use `var` keyword. Whenever possible, use the actual type.
- When using Linq, prefer using the name `x` for the parameter representing the item.

## Unit Tests

- When using `Assert.Throws` method, always use a block body for the lambda expression.

- For each public method that is tested (including the constructor), create a different test file.
  - Ex: For a method called `Query()` create test file `QueryTests`
- All the test files for a single class should be placed in a directory with the name of the class.
## Code Conventions

- Do not use `var` keyword. Whenever possible, use the actual type.
- When using Linq, prefer using the name `x` for the parameter representing the item.

## Code Documentation

- Do not create xml documentation for the types that are used only inside the current solution.
- Only create xml documentation for public types that are exposed as a NuGet package.

## Unit Tests

- When using `Assert.Throws` method, always use a block body for the lambda expression.

- For each public method that is tested (including the constructor), create a different test file.
  - Ex: For a method called `Query()` create test file `QueryTests`
- All the test files for a single class should be placed in a directory with the name of the class.
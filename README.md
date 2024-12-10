# Marten Infinite Tests Issue

This repository demonstrates an issue where creating many factories and properly disposing of them causes tests to never complete. The exact cause of this behavior is currently unknown.

## Description

The project is set up to create multiple factory instances and dispose of them correctly. Despite proper disposal, the tests hang indefinitely and do not finish execution.

## Steps to Reproduce

1. Clone the repository.
2. Setup postgres container and robbitmq
3. Build the project.
4. Run the tests.

## Expected Behavior

The tests should complete successfully after all factories are disposed of.

## Actual Behavior

The tests hang indefinitely and do not complete. 
There are two cases when it happens:
- SomeServicePart1IntegrationTests - factory per each test and each factory has hotcold async daemon.
- SomeServicePart2IntegrationTests - factory per each test but only base factory has async deamon.

## Environment

- **Language**: C#
- **Framework**: .NET
- **IDE**: JetBrains Rider 2024.3
- **Operating System**: macOS

## Additional Information

If you have any insights or solutions to this issue, please contribute by opening an issue or submitting a pull request.
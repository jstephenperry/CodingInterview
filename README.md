# Coding Interview Solutions

A collection of coding interview questions I've come across in my career, implemented in C# on .NET 8.0 with NUnit tests.

## What's included

| Area | File | Notes |
| --- | --- | --- |
| Strings | `StringSolutions.cs` | Longest palindrome (Manacher's algorithm), three anagram approaches (dictionary, Array.Sort, LINQ), max-occurring character/word, Roman numeral parsing, vowel/consonant counts |
| Stacks | `StackSolutions.cs` | Balanced-bracket validation, recursive stack inversion |
| Trees | `DataStructure/BinaryTree.cs` | Generic binary search tree with add/remove/contains |
| Math | `Mathematical/MathOperations.cs`, `MovingAverage.cs` | GCD/LCM, numeric derivative, sliding-window moving average |
| Solvers | `Solvers/SudokuSolver.cs` | 9x9 Sudoku solver via backtracking |
| JSON | `JsonUtils.cs` | JSON validity check, JSONPath value lookup |
| Random | `RandomUtilities/SecureRandomHelper.cs` | Cryptographically-strong RNG with rejection sampling for unbiased ranges |
| Dice | `DiceUtility.cs` | Tabletop-style dice rolls built on `SecureRandomHelper` |

## Build & test

```sh
dotnet build
dotnet test
```

## Layout

- `CodingInterviewImplementations/` — library code
- `CodingInterviewTests/` — NUnit tests, mirroring the implementation namespaces

# Leaderboard Calculator

## Implementation Notes

### Initial Approach: O(N) with Minimal Heap Allocations

The first implementation prioritized performance optimization. It achieved O(N) time complexity with minimal heap allocations by using Spans where possible to avoid stressing Gen0 garbage collection. This approach worked well and delivered good performance characteristics.

However, for a dataset constrained to under 100 items, the performance gains did not justify the code complexity. The resulting implementation was difficult to read and maintain.

### Current Approach: O(N log N) with Focus on Readability

The solution was refactored to prioritize code clarity and maintainability. The current implementation uses sorting (O(N log N) complexity) and makes more allocations to the heap, but remains performant for the problem constraints (up to 100 users).

The main method is decomposed into small, focused static methods that make the control flow self-documenting. Each method has a clear single responsibility, making the code easy to understand and test.

For 100 items, the performance difference between O(N) and O(N log N) is negligible in practice, while the readability improvement is significant.

### Changes from Initial Version

All implementation changes are visible in the 2nd commit. The full diff shows the full diff between boilerplate and solution.

### Technical Decisions

- **Target Framework**: .NET 8 was chosen over .NET 7 because that's the SDK installed locally. This should not cause compatibility issues.

- **LeaderboardMinScores**: Changed to a record type since the data structure is meant to be immutable. Records provide value-based equality semantics and cleaner syntax for immutable data.

## Running Tests

```bash
dotnet test
```

All test cases pass with the current implementation.


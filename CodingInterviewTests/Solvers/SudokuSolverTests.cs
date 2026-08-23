namespace CodingInterviewImplementations.Solvers.Tests
{
    public class SudokuSolverTests
    {
        private static int[,] SolvablePuzzle() => new[,]
        {
            { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
            { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
            { 0, 9, 8, 0, 0, 0, 0, 6, 0 },
            { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
            { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
            { 7, 0, 0, 0, 2, 0, 0, 0, 6 },
            { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
            { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
            { 0, 0, 0, 0, 8, 0, 0, 7, 9 }
        };

        private static int[,] ExpectedSolution() => new[,]
        {
            { 5, 3, 4, 6, 7, 8, 9, 1, 2 },
            { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
            { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
            { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
            { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
            { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
            { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
            { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
            { 3, 4, 5, 2, 8, 6, 1, 7, 9 }
        };

        /// <summary>A board whose givens repeat a 6 in the first row, so it cannot be completed.</summary>
        private static int[,] ContradictoryPuzzle() => new[,]
        {
            { 3, 6, 6, 5, 0, 8, 4, 0, 0 },
            { 5, 2, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 8, 7, 0, 0, 0, 0, 3, 1 },
            { 0, 0, 3, 0, 1, 0, 0, 8, 0 },
            { 9, 0, 0, 8, 6, 3, 0, 0, 5 },
            { 0, 5, 0, 0, 9, 0, 6, 0, 0 },
            { 1, 3, 0, 0, 0, 0, 2, 5, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 7, 4 },
            { 0, 0, 5, 2, 0, 6, 3, 0, 0 }
        };

        [Fact]
        public void Solve_ReturnsTheExpectedSolution()
        {
            Assert.Equal(ExpectedSolution(), SudokuSolver.Solve(SolvablePuzzle()));
        }

        [Fact]
        public void Solve_DoesNotModifyTheBoardItWasGiven()
        {
            int[,] puzzle = SolvablePuzzle();
            int[,] original = SolvablePuzzle();

            int[,] solution = SudokuSolver.Solve(puzzle);

            Assert.Multiple(
                () => Assert.Equal(original, puzzle),
                () => Assert.NotSame(puzzle, solution));
        }

        [Fact]
        public void IsSolvable_ReturnsTrueForACompletablePuzzle()
        {
            Assert.True(SudokuSolver.IsSolvable(SolvablePuzzle()));
        }

        [Fact]
        public void IsSolvable_DoesNotModifyTheBoardItWasGiven()
        {
            // The original solved in place, so asking whether a puzzle was solvable filled it in.
            // Every blank cell in the caller's array came back answered.
            int[,] puzzle = SolvablePuzzle();
            int[,] original = SolvablePuzzle();

            bool solvable = SudokuSolver.IsSolvable(puzzle);

            Assert.Multiple(
                () => Assert.True(solvable),
                () => Assert.Equal(original, puzzle),
                () => Assert.Equal(CountBlanks(original), CountBlanks(puzzle)));
        }

        [Fact]
        public void IsSolvable_ReturnsFalseWhenTheGivensContradictEachOther()
        {
            Assert.False(SudokuSolver.IsSolvable(ContradictoryPuzzle()));
        }

        [Fact]
        public void Solve_ThrowsInvalidOperationWhenThereIsNoSolution()
        {
            // Distinct from ArgumentException: the board is well formed, it simply has no solution.
            Assert.Throws<InvalidOperationException>(() => SudokuSolver.Solve(ContradictoryPuzzle()));
        }

        [Fact]
        public void Solve_CompletesAnAlreadySolvedBoard()
        {
            Assert.Equal(ExpectedSolution(), SudokuSolver.Solve(ExpectedSolution()));
        }

        [Fact]
        public void Solve_FillsAnEmptyBoard()
        {
            int[,] solution = SudokuSolver.Solve(new int[9, 9]);

            Assert.Multiple(
                () => Assert.Equal(0, CountBlanks(solution)),
                () => Assert.All(solution.Cast<int>(), value => Assert.InRange(value, 1, 9)));
        }

        [Fact]
        public void BothEntryPoints_RejectANullBoard()
        {
            Assert.Multiple(
                () => Assert.Throws<ArgumentNullException>(() => SudokuSolver.IsSolvable(null!)),
                () => Assert.Throws<ArgumentNullException>(() => SudokuSolver.Solve(null!)));
        }

        [Fact]
        public void BothEntryPoints_RejectAMisshapenBoard()
        {
            int[,] tooSmall = new int[4, 4];

            Assert.Multiple(
                () => Assert.Throws<ArgumentException>(() => SudokuSolver.IsSolvable(tooSmall)),
                () => Assert.Throws<ArgumentException>(() => SudokuSolver.Solve(tooSmall)));
        }

        [Fact]
        public void BothEntryPoints_RejectOutOfRangeCellValues()
        {
            int[,] board = new int[9, 9];
            board[0, 0] = 42;

            Assert.Multiple(
                () => Assert.Throws<ArgumentException>(() => SudokuSolver.IsSolvable(board)),
                () => Assert.Throws<ArgumentException>(() => SudokuSolver.Solve(board)));
        }

        private static int CountBlanks(int[,] board)
        {
            int blanks = 0;
            foreach (int value in board)
            {
                if (value == 0)
                {
                    blanks++;
                }
            }
            return blanks;
        }
    }
}

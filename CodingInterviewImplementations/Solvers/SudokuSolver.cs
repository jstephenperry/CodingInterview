namespace CodingInterviewImplementations.Solvers
{
    /// <summary>
    /// Backtracking solver for standard 9x9 Sudoku boards.
    /// </summary>
    /// <remarks>
    /// Neither public method mutates the board it is given. An earlier version solved in place, so
    /// calling <see cref="IsSolvable"/> silently overwrote the puzzle it was asked about.
    /// </remarks>
    public static class SudokuSolver
    {
        private const int Size = 9;
        private const int BoxSize = 3;
        private const int Empty = 0;

        /// <summary>
        /// Determines whether a board has at least one solution, leaving the board untouched.
        /// </summary>
        /// <param name="board">A 9x9 board where zero marks an empty cell.</param>
        /// <returns>True if the board can be completed.</returns>
        /// <exception cref="ArgumentNullException">The board is null.</exception>
        /// <exception cref="ArgumentException">The board is not 9x9 or holds values outside [0, 9].</exception>
        public static bool IsSolvable(int[,] board)
        {
            ValidateShape(board);

            // Work on a copy so that the board passed in survives the search.
            int[,] candidate = (int[,])board.Clone();
            return HasConsistentGivens(candidate) && SolveInternal(candidate, 0, 0);
        }

        /// <summary>
        /// Solves a board and returns the completed grid as a new array.
        /// </summary>
        /// <param name="board">A 9x9 board where zero marks an empty cell.</param>
        /// <returns>A new 9x9 array holding the solution. The board passed in is not modified.</returns>
        /// <exception cref="ArgumentNullException">The board is null.</exception>
        /// <exception cref="ArgumentException">The board is not 9x9 or holds values outside [0, 9].</exception>
        /// <exception cref="InvalidOperationException">The board is well formed but has no solution.</exception>
        public static int[,] Solve(int[,] board)
        {
            ValidateShape(board);

            int[,] solution = (int[,])board.Clone();
            if (!HasConsistentGivens(solution) || !SolveInternal(solution, 0, 0))
            {
                // Distinct from ArgumentException: the argument is a valid board, it just has no solution.
                throw new InvalidOperationException("The board has no solution.");
            }

            return solution;
        }

        private static void ValidateShape(int[,] board)
        {
            ArgumentNullException.ThrowIfNull(board);

            if (board.GetLength(0) != Size || board.GetLength(1) != Size)
            {
                throw new ArgumentException($"The board must be {Size}x{Size}.", nameof(board));
            }

            foreach (int value in board)
            {
                if (value < Empty || value > Size)
                {
                    throw new ArgumentException(
                        $"Board values must be in [0, {Size}], where 0 marks an empty cell.", nameof(board));
                }
            }
        }

        /// <summary>
        /// Checks that the pre-filled cells do not already conflict with each other.
        /// </summary>
        /// <remarks>
        /// <see cref="IsValidMove"/> only vets cells the solver fills, so a board whose givens already
        /// contain a duplicate would otherwise be explored exhaustively before failing.
        /// </remarks>
        private static bool HasConsistentGivens(int[,] board)
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    int value = board[row, col];
                    if (value == Empty)
                    {
                        continue;
                    }

                    board[row, col] = Empty;
                    bool valid = IsValidMove(board, row, col, value);
                    board[row, col] = value;

                    if (!valid)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool SolveInternal(int[,] board, int row, int col)
        {
            if (row == Size)
            {
                return true;
            }

            if (col == Size)
            {
                return SolveInternal(board, row + 1, 0);
            }

            if (board[row, col] != Empty)
            {
                return SolveInternal(board, row, col + 1);
            }

            for (int value = 1; value <= Size; value++)
            {
                if (IsValidMove(board, row, col, value))
                {
                    board[row, col] = value;
                    if (SolveInternal(board, row, col + 1))
                    {
                        return true;
                    }
                    board[row, col] = Empty;
                }
            }

            return false;
        }

        private static bool IsValidMove(int[,] board, int row, int col, int value)
        {
            for (int i = 0; i < Size; i++)
            {
                if (board[row, i] == value || board[i, col] == value)
                {
                    return false;
                }
            }

            int startRow = row - (row % BoxSize);
            int startCol = col - (col % BoxSize);

            for (int i = startRow; i < startRow + BoxSize; i++)
            {
                for (int j = startCol; j < startCol + BoxSize; j++)
                {
                    if (board[i, j] == value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

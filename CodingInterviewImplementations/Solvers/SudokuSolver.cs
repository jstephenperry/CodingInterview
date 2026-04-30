namespace CodingInterviewImplementations.Solvers
{
    public static class SudokuSolver
    {
        private const int Size = 9;
        private const int BoxSize = 3;

        /// <summary>
        /// Returns <see langword="true"/> if the given 9×9 Sudoku <paramref name="board"/> has at least one solution.
        /// The input board is not modified.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="board"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="board"/> is not 9×9 or contains values outside 0–9.</exception>
        public static bool IsSolvable(int[,] board)
        {
            int[,] copy = ValidateAndClone(board);
            return SolveInternal(copy, 0, 0);
        }

        /// <summary>
        /// Solves the given 9×9 Sudoku <paramref name="board"/> and returns a new array containing the solution.
        /// The input board is not modified.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="board"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="board"/> is not 9×9, contains values outside 0–9, or has no solution.</exception>
        public static int[,] Solve(int[,] board)
        {
            int[,] copy = ValidateAndClone(board);
            if (!SolveInternal(copy, 0, 0))
            {
                throw new ArgumentException("Board has no solution.", nameof(board));
            }
            return copy;
        }

        private static int[,] ValidateAndClone(int[,] board)
        {
            ArgumentNullException.ThrowIfNull(board);
            if (board.GetLength(0) != Size || board.GetLength(1) != Size)
            {
                throw new ArgumentException($"Board must be {Size}x{Size}.", nameof(board));
            }
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    int v = board[r, c];
                    if (v < 0 || v > Size)
                    {
                        throw new ArgumentException($"Cell values must be in 0..{Size}; got {v} at ({r},{c}).", nameof(board));
                    }
                }
            }
            return (int[,])board.Clone();
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
            if (board[row, col] != 0)
            {
                return SolveInternal(board, row, col + 1);
            }

            for (int v = 1; v <= Size; v++)
            {
                if (IsValidMove(board, row, col, v))
                {
                    board[row, col] = v;
                    if (SolveInternal(board, row, col + 1))
                    {
                        return true;
                    }
                    board[row, col] = 0;
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

            int startRow = row - row % BoxSize;
            int startCol = col - col % BoxSize;
            for (int r = startRow; r < startRow + BoxSize; r++)
            {
                for (int c = startCol; c < startCol + BoxSize; c++)
                {
                    if (board[r, c] == value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

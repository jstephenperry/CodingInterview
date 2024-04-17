namespace CodingInterviewImplementations.Solvers
{
    public class SudokuSolver
    {
        public static bool IsSolvable(int[,] board)
        {
            if (board == null || board.GetLength(0) != 9 || board.GetLength(1) != 9)
            {
                return false;
            }

            return SolveInternal(board, 0, 0);
        }

        public static int[,] Solve(int[,] board)
        {
            if (board == null || board.GetLength(0) != 9 || board.GetLength(1) != 9)
            {
                throw new ArgumentException("Invalid board");
            }

            if (SolveInternal(board, 0, 0))
            {
                return board;
            }
            else
            {
                throw new ArgumentException("Invalid board");
            }
        }

        private static bool SolveInternal(int[,] board, int row, int col)
        {
            if (row == 9)
            {
                return true;
            }

            if (col == 9)
            {
                return SolveInternal(board, row + 1, 0);
            }

            if (board[row, col] != 0)
            {
                return SolveInternal(board, row, col + 1);
            }

            for (int i = 1; i <= 9; i++)
            {
                if (IsValidMove(board, row, col, i))
                {
                    board[row, col] = i;
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
            for (int i = 0; i < 9; i++)
            {
                if (board[row, i] == value || board[i, col] == value)
                {
                    return false;
                }
            }

            int startRow = row - row % 3;
            int startCol = col - col % 3;

            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
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

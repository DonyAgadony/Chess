using System;
class Program
{
    static void Main()
    {
        char[,] board = {
    { '*', '*', '*', 'k', '*', '*', '*', 'R' },
    { '*', '*', '*', '*', '*', '*', '*', '*' },
    { '*', 'P', '*', '*', 'P', '*', '*', '*' },
    { '*', '*', '*', '*', '*', '*', 'B', '*' },
    { '*', '*', '*', '*', '*', '*', '*', '*' },
    { '*', '*', '*', '*', '*', '*', '*', '*' },
    { '*', 'K', '*', '*', '*', '*', '*', '*' },
    { '*', '*', '*', '*', '*', '*', '*', '*' }
};



        bool isCheckmate = ChessGame.IsCheckmate(board);
        Console.WriteLine($"Is Checkmate: {isCheckmate}");
    }
}

public class ChessGame
{
    private const char KingBlack = 'k';
    private const char KingWhite = 'K';

    public static bool IsCheckmate(char[,] board)
    {
        int blackKingRow = -1;
        int blackKingCol = -1;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j] == KingBlack)
                {
                    blackKingRow = i;
                    blackKingCol = j;
                    break;
                }
            }
            if (blackKingRow != -1)
                break;
        }

        if (blackKingRow == -1)
            return false; 

        
        if (!IsInCheck(board, blackKingRow, blackKingCol))
            return false; 

        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue; 

                int newRow = blackKingRow + dr;
                int newCol = blackKingCol + dc;

                if (IsInBounds(newRow, newCol) && IsSafe(board, newRow, newCol))
                {
                    return false; 
                }
            }
        }

        return true;
    }

    private static bool IsInCheck(char[,] board, int kingRow, int kingCol)
    {
        return IsThreatenedByPawns(board, kingRow, kingCol) ||
               IsThreatenedByRooksAndQueens(board, kingRow, kingCol) ||
               IsThreatenedByBishopsAndQueens(board, kingRow, kingCol) ||
               IsThreatenedByKnights(board, kingRow, kingCol) ||
               IsThreatenedByKings(board, kingRow, kingCol);
    }

    private static bool IsThreatenedByPawns(char[,] board, int kingRow, int kingCol)
    {
        return (kingRow < 7 && kingCol > 0 && board[kingRow + 1, kingCol - 1] == 'P') ||
               (kingRow < 7 && kingCol < 7 && board[kingRow + 1, kingCol + 1] == 'P');
    }

    private static bool IsThreatenedByRooksAndQueens(char[,] board, int kingRow, int kingCol)
    {
        for (int i = 0; i < 8; i++)
        {
            if (i != kingRow && board[i, kingCol] == 'R') return true;
            if (i != kingCol && board[kingRow, i] == 'R') return true;
            if (i != kingRow && board[i, kingCol] == 'Q') return true;
            if (i != kingCol && board[kingRow, i] == 'Q') return true;
        }

        return false;
    }

    private static bool IsThreatenedByBishopsAndQueens(char[,] board, int kingRow, int kingCol)
    {
        int[] directions = { -1, 1 };
        foreach (var dr in directions)
        {
            foreach (var dc in directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    int newRow = kingRow + dr * i;
                    int newCol = kingCol + dc * i;

                    if (!IsInBounds(newRow, newCol)) break;

                    if (board[newRow, newCol] == 'B' || board[newRow, newCol] == 'Q')
                        return true;

                    if (board[newRow, newCol] != '*') break; 
                }
            }
        }

        return false;
    }

    private static bool IsThreatenedByKnights(char[,] board, int kingRow, int kingCol)
    {
        int[] knightMovesRow = { 2, 2, -2, -2, 1, 1, -1, -1 };
        int[] knightMovesCol = { 1, -1, 1, -1, 2, -2, 2, -2 };

        for (int i = 0; i < 8; i++)
        {
            int newRow = kingRow + knightMovesRow[i];
            int newCol = kingCol + knightMovesCol[i];

            if (IsInBounds(newRow, newCol) && board[newRow, newCol] == 'N')
                return true;
        }

        return false;
    }

    private static bool IsThreatenedByKings(char[,] board, int kingRow, int kingCol)
    {
        int[] directions = { -1, 0, 1 };
        foreach (var dr in directions)
        {
            foreach (var dc in directions)
            {
                if (dr == 0 && dc == 0) continue; 

                int newRow = kingRow + dr;
                int newCol = kingCol + dc;

                if (IsInBounds(newRow, newCol) && board[newRow, newCol] == 'K')
                    return true;
            }
        }

        return false;
    }

    private static bool IsSafe(char[,] board, int row, int col)
    {
        return !IsInCheck(board, row, col);
    }

    private static bool IsInBounds(int row, int col)
    {
        return row >= 0 && row < 8 && col >= 0 && col < 8;
    }
}
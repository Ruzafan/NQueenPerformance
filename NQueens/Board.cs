using System.Drawing;
using System.Runtime.InteropServices;

namespace NQueens;

public class Board(int size)
{
    private int N { get; set; } = size;
    private int[,] board = new int[size, size];
    private List<int> ocuppiedDiagonals = new List<int>();
    private List<Point> points = new List<Point>();
    private List<int> alternatedRows = new List<int>();

    private void InitAlternatedRows()
    {
        var rows =  CollectionsMarshal.AsSpan(Enumerable.Range(0, N).ToList());
        var backwards = CollectionsMarshal.AsSpan(Enumerable.Range(0, N).Reverse().ToList());
        var turn = true;
        for (var i = 0; i < N; i++)
        {
            alternatedRows.Add(turn ? rows[i] : backwards[i]);
            turn = !turn;
        }
    }
    
    public void PrintBoard()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                //Console.Write((board[i, j] == 1 ? board[i, j] : 0) + " ");
                Console.Write(board[i, j] + " ");
            }

            Console.Write("\n");
        }
    }

    private bool IsSafe(int row, int col)
    {
        var _row = row + 1;
        var _col = col + 1;
        
        Span<Point> spanPoints = CollectionsMarshal.AsSpan(points);
        foreach (var point in spanPoints)
        {
            if(point.X == _row || point.Y == _col)
                return false;
        }

        Span<int> span = CollectionsMarshal.AsSpan(ocuppiedDiagonals);
        foreach (var point in span)
        {
            if (point == (_row + _col) || point == (_col - _row))
            {
                return false;
            }
        }

        return true;
    }

    public bool TheBoardSolver(int col)
    {
        if (col == 0) InitAlternatedRows();
        if (col >= N)
            return true;
        for (int i = 0; i < N; i++)
        {
            var currentRow = alternatedRows[i];
            if (IsSafe(currentRow, col))
            {
                board[currentRow, col] = 1;
                points.Add(new Point(currentRow+1, col+1));
                ocuppiedDiagonals.Add((currentRow+1)+(col+1));
                ocuppiedDiagonals.Add((col+1)- (currentRow+1));
                if (TheBoardSolver(col + 1))
                    return true;
                // Backtracking is important in this one.
                board[currentRow, col] = 0;
                points.RemoveAt(points.Count-1);
                ocuppiedDiagonals.RemoveRange(ocuppiedDiagonals.Count-2,2);
            }
        }

        return false;
    }

}
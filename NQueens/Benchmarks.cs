using System.Drawing;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace NQueens;

[MemoryDiagnoser(false)]
public class Benchmarks
{
    [Params(23, 24)] public int Size;

    private int[,] _board;
    private List<int> ocuppiedDiagonals = new List<int>();
    private List<Point> points = new List<Point>();

    [GlobalSetup]
    public void Setup()
    {
        _board = new int[Size, Size];
    }


    private bool IsSafe(int row, int col)
    {
        // Check the row on the left side
        var _row = row + 1;
        var _col = col + 1;
        if (points.Any(q => q.X == _row || q.Y == _col))
        {
            return false;
        }

        if (ocuppiedDiagonals.Any(q => q == (_row + _col) || q == (_col - _row)))
        {
            return false;
        }

        return true;
    }

    private bool IsSafeImp(int row, int col)
    {
        // Check the row on the left side
        var _row = row + 1;
        var _col = col + 1;

        Span<Point> spanPoints = CollectionsMarshal.AsSpan(points);
        foreach (var point in spanPoints)
        {
            if (point.X == _row || point.Y == _col)
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


    //[Benchmark]
    public bool SolveImproved() => TheBoardSolverImproved(0);


    [Benchmark]
    public bool SolveImproved2() => TheBoardSolverImproved2(0);

    private bool TheBoardSolverImproved(int col)
    {
        if (col >= Size)
            return true;
        for (int i = 0; i < Size; i++)
        {
            if (IsSafe(i, col))
            {
                _board[i, col] = 1;
                points.Add(new Point(i + 1, col + 1));
                ocuppiedDiagonals.Add((i + 1) + (col + 1));
                ocuppiedDiagonals.Add((col + 1) - (i + 1));
                if (TheBoardSolverImproved(col + 1))
                    return true;
                // Backtracking is important in this one.
                _board[i, col] = 0;
                points.RemoveAt(points.Count - 1);
                ocuppiedDiagonals.RemoveRange(ocuppiedDiagonals.Count - 2, 2);
            }
        }

        return false;
    }

    private bool TheBoardSolverImproved2(int col)
    {
        if (col >= Size)
            return true;
        for (int i = 0; i < Size; i++)
        {
            if (IsSafeImp(i, col))
            {
                _board[i, col] = 1;
                points.Add(new Point(i + 1, col + 1));
                ocuppiedDiagonals.Add((i + 1) + (col + 1));
                ocuppiedDiagonals.Add((col + 1) - (i + 1));
                if (TheBoardSolverImproved2(col + 1))
                    return true;
                // Backtracking is important in this one.
                _board[i, col] = 0;
                points.RemoveAt(points.Count - 1);
                ocuppiedDiagonals.RemoveRange(ocuppiedDiagonals.Count - 2, 2);
            }
        }

        return false;
    }
}
// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using BenchmarkDotNet.Running;
using NQueens;
List<int> numbers = new List<int>(Enumerable.Range(28,1));

foreach (int number in numbers)
{
    Console.WriteLine($"Size of matrix: {number}");
    var sp = new Stopwatch();
    sp.Start();
    var board = new Board(number);

    if (!board.TheBoardSolver(0))
    {
        Console.WriteLine("Solution not found.");
    }
    board.PrintBoard();
    sp.Stop();
    Console.WriteLine($"Time taken: {sp.ElapsedMilliseconds} ms. In seconds: {TimeSpan.FromMilliseconds(sp.ElapsedMilliseconds).TotalSeconds}. \n");
}

Console.ReadLine();

//BenchmarkRunner.Run<Benchmarks>();
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
    public Rocket GetNextMove(Rocket rocket)
    {
        var moves = new List<(Turn, double)>();
        var tasks = new Task[threadsCount];
        for (int i = 0; i < threadsCount; i++)
        {
            tasks[i] = new Task(() =>
            {
                Random random = new Random();
                var bestMoveInThread = SearchBestMove(rocket,
                    random, iterationsCount / threadsCount);
                var move = (bestMoveInThread.Turn, bestMoveInThread.Score);
                moves.Add(move);
            });
            tasks[i].Start();
        }
        Task.WaitAll(tasks);
        var bestMove = moves.OrderBy(x => x.Item2).First();
        return rocket.Move(bestMove.Item1, level);
    }

    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
    {
        return new() { Task.Run(() => SearchBestMove(rocket, new Random(random.Next()), iterationsCount)) };
    }
}
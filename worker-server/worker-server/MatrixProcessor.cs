using System;
using System.Threading;

namespace worker_server
{
    public static class MatrixProcessor
    {
        public static void ProcessMatrix(ref int[,] matrix, ref int progress)
        {
            int progression;
            var rand = new Random();
            int size = matrix.GetLength(0);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    matrix[x, y] = matrix[y, x] * rand.Next(-100, 100) / rand.Next(-50, 55);
                    progression = Convert.ToInt32((x * y) / 100 * Math.Pow(size, 2));
                    Interlocked.Exchange(ref progress, progression);
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }
        }
    }
}
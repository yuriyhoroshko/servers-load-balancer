using System;
using System.Net.Sockets;
using System.Threading;

namespace worker_server
{
    public static class MatrixProcessor
    {
        public static void ProcessMatrix(ref int[,] matrix, ref int progress, ref bool isDone, CancellationTokenSource token)
        {
            isDone = false;
            int progression;
            int size = matrix.GetLength(0);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    matrix[x, y] = matrix[y, x];
                        for (int z = 0; z < size; z++)
                        {
                            matrix[x, y] = matrix[y, z] + matrix[z, x];
                        }
                }
                Thread.Sleep(TimeSpan.FromSeconds(10));

                progression = Convert.ToInt32((((float)x + 1.0) / size)*100.0);
                Interlocked.Exchange(ref progress, progression);
            }

            isDone = true;
        }
    }
}
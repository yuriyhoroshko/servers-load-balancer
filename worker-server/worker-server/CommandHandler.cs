using System;
using System.Net.Sockets;
using System.Threading;
using Contract;
using Contract.ByteDefinition;

namespace worker_server
{
    public static class CommandHandler
    {
        private static int matrixSize = 0;
        private static int progress = 0;
        private static Thread tr;
        private static bool isDone = false;
        private static int[,] matrix;
        private static CancellationTokenSource token = new CancellationTokenSource();

        public static void Handle(ref byte[] byteArray, Socket handler)
        {
            if (byteArray[0] == Bytes.byteDef["task_prefix"])
            {
                if (matrixSize != 0)
                {
                    matrix = new int[matrixSize, matrixSize];
                    ByteConverter.SetMatrixFromBytes(byteArray.RemovePrefix(), ref matrix);
                    handler.Send(Bytes.type["matrix_accepted"]);
                    tr = new Thread(l => { MatrixProcessor.ProcessMatrix(ref matrix, ref progress, ref isDone, token); });
                    tr.Start();
                }
            } else if (byteArray[0] == Bytes.byteDef["size_prefix"])
            {
                matrixSize = ByteConverter.GetMatrixSize(byteArray.RemovePrefix());
                Console.WriteLine($"Received matrix size {matrixSize}x{matrixSize} , receiving matrix");
                handler.Send(Bytes.type["size_accepted"]);
            } else if (byteArray[0] == Bytes.type["progress_request"][0])
            {
                byte[] byteProgress = new[] {(byte) progress};
                Console.WriteLine($"progress: {progress}%");
                handler.Send(byteProgress.AddPrefix(Bytes.byteDef["percent_prefix"]));
            } else if (byteArray[0] == Bytes.type["collect_data"][0])
            {
                byteArray = ByteConverter.GetBytes(matrix);
                if (isDone)
                {
                    handler.Send(byteArray.AddPrefix(Bytes.byteDef["ready_prefix"]));
                }
                else
                {
                    handler.Send(new byte[2].AddPrefix(Bytes.byteDef["notyet_prefix"]));
                }
            }
            else if (byteArray[0] == Bytes.type["cancel_task"][0])
            {
                token.Cancel();
            }
        }
    }
}

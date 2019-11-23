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
        public static void Handle(byte[] byteArray,Socket handler)
        {
            if (byteArray[0] == Bytes.byteDef["task_prefix"])
            {
                if (matrixSize != 0)
                {
                    int[,] matrix = new int[matrixSize, matrixSize];
                    ByteConverter.SetMatrixFromBytes(byteArray.RemovePrefix(), ref matrix);
                    handler.Send(Bytes.type["matrix_accepted"]);
                    tr = new Thread(l =>
                    {
                        MatrixProcessor.ProcessMatrix(ref matrix,ref progress);
                    });
                    tr.Start();
                }
            } else if (byteArray[0] == Bytes.byteDef["size_prefix"])
            {
                matrixSize = ByteConverter.GetMatrixSize(byteArray.RemovePrefix());
                Console.WriteLine($"Received matrix size {matrixSize}x{matrixSize} , receiving matrix");
                handler.Send(Bytes.type["size_accepted"]);
            } else if (byteArray == Bytes.type["progress_request"])
            {
                byte[] byteProgress = new[] {(byte) progress};
                Console.WriteLine($"progress: {progress}%");
                handler.Send(byteProgress.AddPrefix(Bytes.byteDef["percent_prefix"]));
            }
        }
    }
}

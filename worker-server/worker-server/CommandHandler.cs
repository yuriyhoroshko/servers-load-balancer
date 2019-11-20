using System;
using System.Net.Sockets;
using Contract;
using Contract.ByteDefinition;

namespace worker_server
{
    public static class CommandHandler
    {
        private static int matrixSize = 0;
        public static void Handle(byte[] byteArray,Socket handler)
        {
            if (byteArray[0] == Bytes.byteDef["task_prefix"])
            {
                if (matrixSize != 0)
                {
                    int[,] matrix = new int[matrixSize, matrixSize];
                    ByteConverter.SetMatrixFromBytes(byteArray.RemovePrefix(), ref matrix);
                    foreach (var el in matrix)
                    {
                        Console.WriteLine(el);
                    }
                    handler.Send(Bytes.type["matrix_accepted"]);
                }
            } else if (byteArray[0] == Bytes.byteDef["size_prefix"])
            {
                matrixSize = ByteConverter.GetMatrixSize(byteArray.RemovePrefix());
                Console.WriteLine($"Received matrix size {matrixSize}x{matrixSize} , receiving matrix");
                handler.Send(Bytes.type["size_accepted"]);
            }
        }
    }
}

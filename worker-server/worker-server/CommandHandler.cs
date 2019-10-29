using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace worker_server
{
    public static class CommandHandler
    {
        public static void Handle(byte[] byteArray)
        {
            if (byteArray[0] == ByteDefinition.Bytes.byteDef["task_prefix"])
            {
                int[,] matrix = new int[10,10];
                ByteConverter.SetMatrixFromBytes(byteArray.RemovePrefix(),ref matrix);
                foreach (var el in matrix)
                {
                    Console.WriteLine(el);
                }
            }
        }
    }
}

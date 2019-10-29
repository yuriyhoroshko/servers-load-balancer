using System;

namespace Business
{
    public static class ByteConverter
    {
        public static byte[] GetBytes(int[,] matrix)
        {
            var buffer = new byte[matrix.GetLength(0) * matrix.GetLength(1) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(int))];
            Buffer.BlockCopy(matrix, 0, buffer, 0, buffer.Length);
            return buffer;
        }

        public static void SetMatrixFromBytes(byte[] byteArray,ref int[,] matrix)
        {
            var len = Math.Min(matrix.GetLength(0) * matrix.GetLength(1) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(int)), byteArray.Length);
            Buffer.BlockCopy(byteArray, 0, matrix, 0, len);
        }
    }
}

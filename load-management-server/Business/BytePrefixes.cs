﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public static class BytePrefixes
    {
        public static byte[] RemovePrefix(this byte[] byteArr)
        {
            byte[] output = new byte[byteArr.Length - 1];
            for (int i = 1; i < byteArr.Length; i++)
            {
                output[i - 1] = byteArr[i];
            }

            return output;
        }

        public static byte[] AddPrefix(this byte[] byteArr, byte prefix)
        {
            byte[] output = new byte[byteArr.Length + 1];
            output[0] = prefix;
            for (int i = 0; i < byteArr.Length; i++)
            {
                output[i + 1] = byteArr[i];
            }

            return output;
        }
    }
}

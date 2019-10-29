using System;
using System.Collections.Generic;
using System.Text;

namespace worker_server.ByteDefinition
{
    public static class Bytes
    {
        public static Dictionary<string, byte[]> type = new Dictionary<string, byte[]>
        {
            ["connection_request"] = new byte[] {101},
            ["connection_response"] = new byte[] {111},
        };

        public static Dictionary<string, byte> byteDef = new Dictionary<string, byte>()
        {
            ["task_prefix"] = (byte) 153,
        };
    }
}

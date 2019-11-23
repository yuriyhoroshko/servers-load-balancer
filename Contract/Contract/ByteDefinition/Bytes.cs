using System.Collections.Generic;

namespace Contract.ByteDefinition
{
    public static class Bytes
    {
        public static Dictionary<string, byte[]> type = new Dictionary<string, byte[]>
        {
            ["connection_request"] = new byte[] {101},
            ["connection_response"] = new byte[] {111},
            ["size_accepted"] = new byte[] {155},
            ["matrix_accepted"] = new byte[] {156},
            ["progress_request"] = new byte[] {127}
        };

        public static Dictionary<string, byte> byteDef = new Dictionary<string, byte>()
        {
            ["task_prefix"] = (byte) 153,
            ["size_prefix"] = (byte) 112,
            ["percent_prefix"] = (byte) 113
            
        };
    }
}

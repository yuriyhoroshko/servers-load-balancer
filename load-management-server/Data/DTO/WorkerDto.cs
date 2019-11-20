using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.DTO
{
    public class WorkerServerDto
    {
        public int WorkerServerID { get; set; }

        public string IpAddress { get; set; }

        public int Port { get; set; }

        public bool IsConnected { get; set; }
    }
}

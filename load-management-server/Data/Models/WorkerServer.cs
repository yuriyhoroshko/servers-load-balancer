using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Data.Models
{
    public class WorkerServer
    {
        [Required]
        [Key]
        public int WorkerServerID { get; set; }
    
        public string IpAddress { get; set; }

        public int Port { get; set; }

        public bool IsConnected { get; set; }

        
    }
}

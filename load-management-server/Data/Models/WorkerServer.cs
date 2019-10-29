using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class WorkerServer
    {
        [Required]
        public int WorkerID { get; set; }
    
        public string IpAddress { get; set; }

        
    }
}

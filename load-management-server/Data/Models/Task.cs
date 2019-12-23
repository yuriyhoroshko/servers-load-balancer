using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Task
    {
        public int TaskID { get; set; }

        public string Status { get; set; }

        public WorkerServer WorkerServer { get; set; }

        [ForeignKey("WorkerServer")]
        public int? ServerID { get; set; }

        public byte DonePercent { get; set; }

        public int Size { get; set; }

        public int UserId { get; set; }
    }
}

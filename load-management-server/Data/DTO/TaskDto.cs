namespace Data.DTO
{
    public class TaskDto
    {
        public int TaskID { get; set; }

        public string Status { get; set; }
        
        public int? ServerID { get; set; }

        public byte DonePercent { get; set; }

        public int Size { get; set; }

        public int UserId { get; set; }
    }
}

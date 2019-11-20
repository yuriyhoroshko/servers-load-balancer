namespace Data.DTO
{
    public class WorkerTaskDto
    {
        public string IpAddress { get; set; }

        public int Port { get; set; }

        public bool IsConnected { get; set; }

        public int TaskCount { get; set; }
    }
}

namespace PRN221_Assignment.Data
{
    public class MessageData
    {
        public string PhotoURL { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string Message { get; set; }
        public bool IsSendedByUser { get; set; }
        public bool Readed { get; set; }
        public DateTime Time { get; set; }
    }
}

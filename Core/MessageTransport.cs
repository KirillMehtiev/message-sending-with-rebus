namespace Core
{
    public class MessageTransport
    {
        public MessageTransport(Message data)
        {
            Data = data;
        }

        public Message Data { get; set; }
    }
}
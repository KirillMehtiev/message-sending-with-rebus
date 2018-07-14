namespace Core
{
    public class Message
    {
        public Message(string body, string authorName)
        {
            Body = body;
            AuthorName = authorName;
        }

        public string Body { get; set; }
        public string AuthorName { get; set; }
    }
}

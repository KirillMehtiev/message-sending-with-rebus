namespace Core
{
    public class RebusConfiguration
    {
        public RebusConfiguration(string transport, string subscription, string defaultQueue)
        {
            Transport = transport;
            Subscription = subscription;
            DefaultQueue = defaultQueue;
        }

        public string Transport { get; }
        public string Subscription { get; }
        public string DefaultQueue { get; }
    }
}
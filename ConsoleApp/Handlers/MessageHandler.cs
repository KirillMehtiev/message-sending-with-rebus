using System;
using System.Threading.Tasks;
using Core;
using Rebus.Handlers;

namespace ConsoleApp.Handlers
{
    public class MessageHandler : IHandleMessages<MessageTransport>
    {
        public async Task Handle(MessageTransport messageTransport)
        {
            var message = messageTransport.Data;

            Console.WriteLine($"Received new message from {message.AuthorName} -> {message.Body}");
        }
    }
}
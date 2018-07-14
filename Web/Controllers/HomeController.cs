using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBus _bus;

        public HomeController(IBus bus)
        {
            _bus = bus;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(MessageModel model)
        {
            var message = new Message(model.Body, model.AuthorName);
            var messageTransport = new MessageTransport(message);

            await _bus.Publish(messageTransport);

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

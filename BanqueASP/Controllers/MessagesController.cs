using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanqueASP.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        readonly ApiContext context;

        public MessagesController(ApiContext context)
        {
            this.context = context;
        }

        public IEnumerable<Models.Message> Get()
        {
            return context.Messages;
        }
        [HttpGet("{name}")]
        public IEnumerable<Models.Message> Get(string name)
        {
            return context.Messages.Where(message => message.Owner == name);
        }

        [HttpPost]
        public Models.Message Post([FromBody] Models.Message message)
        {
            var dbMessage = context.Messages.Add(message).Entity;
            context.SaveChanges();
            return dbMessage;
        }
    }
}
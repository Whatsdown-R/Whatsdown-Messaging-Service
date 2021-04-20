using MessageService.Logic;
using MessageService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whatsdown_Authentication_Service.Data;

namespace MessageService.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        readonly IConfiguration _config;
        ChatLogic logic;
        public ChatController(IConfiguration config, ChatContext context)
        {
            this.logic = new ChatLogic(context);
            this._config = config;
        }

        [HttpGet, Route("/{identificationCode}")]
        public IActionResult GetMessages(string identificationCode)
        {
            IActionResult response = Unauthorized();
            List<Message> messages = logic.GetMessages(identificationCode);
            response = Ok(new { messages = messages });
            return response;
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] MessageView view)
        {
            IActionResult response = Unauthorized();
            bool result = logic.PostMessage(view);

            if (result)
                response = Ok();
            else
                response = BadRequest();
            return response;
        }

        [HttpDelete]
        public IActionResult DeleteMessageView([FromBody] DeleteMessageView view)
        {
            IActionResult response = Unauthorized();
            bool result = logic.DeleteMessage(view);

            if (result)
                response = Ok();
          
            return response;
        }
    }
}

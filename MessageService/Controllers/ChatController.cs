using MessageService.Hubs;
using MessageService.Logic;
using MessageService.Model;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private Microsoft.AspNetCore.SignalR.IHubContext<ChatHub> _context;
        private readonly IConfiguration _config;
        ChatLogic logic;
        public ChatController(IConfiguration config, ChatContext context, Microsoft.AspNetCore.SignalR.IHubContext<ChatHub> _context)
        {
            this.logic = new ChatLogic(context);
            this._config = config;
            this._context = _context;
        }

        [HttpPost, Route("group/join")]
        public IActionResult JoinGroups(JoinGroupView view)
        {
            if(view.connectionId != null)
            foreach (string item in view.groups)
            {
                _context.Groups.AddToGroupAsync(view.connectionId, item);
            }
           
            return Ok();
        }

        [HttpGet, Route("group/{identificationCode}")]
        public IActionResult GetMessages(string identificationCode)
        {
            IActionResult response = Unauthorized();
            List<SendMessageView> messages = logic.GetMessages(identificationCode);
            response = Ok(new { messages = messages });
            return response;
        }

        [HttpPost]
        public async Task<IActionResult> PostTextMessage(MessageView view)
        {
            
            IActionResult response = Unauthorized();
            Message result = logic.PostMessage(view);

            if (result != null)
            {
                await _context.Clients.Group(view.IdentificationCode).SendAsync("Groupsend", new SendMessageView(result.senderId, result.identificationCode, result.message, result.type, result.date));
                response = Ok();
            }
            else
                response = BadRequest();
            return response;
        }
        [HttpPost, Route("image"), DisableRequestSizeLimit]
        public async Task<IActionResult> PostImageMessage([FromForm] MessageView view)
        {

            IActionResult response = Unauthorized();
            Message result = logic.PostImage(view);

            if (result != null)
            {
                await _context.Clients.Group(view.IdentificationCode).SendAsync("Groupsend", new SendMessageView(result.senderId, result.identificationCode, result.message, result.type, result.date));
                response = Ok();
            }
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

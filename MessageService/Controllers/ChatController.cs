using MessageService.Exceptions;
using MessageService.Hubs;
using MessageService.Logic;
using MessageService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Whatsdown_Authentication_Service.Data;

namespace MessageService.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private IHubContext<ChatHub> _context;
        private readonly IConfiguration _config;
        private RabbitMQProducer rabbitMQProducer;
        private readonly ILogger<ChatController> logger;
        ChatLogic logic;
        public ChatController(IConfiguration config, ChatContext context,IHubContext<ChatHub> _context, ILogger<ChatController> logger, ILoggerFactory loggerFactory)
        {
            this.logic = new ChatLogic(context, loggerFactory.CreateLogger<ChatLogic>());
            this._config = config;
            this._context = _context;
            this.logger = loggerFactory.CreateLogger<ChatController>();
            //this.rabbitMQProducer = new RabbitMQProducer("amqp://guest:guest@localhost:5672");
        }

        [HttpPost, Route("group/join")]
        public IActionResult JoinGroups(JoinGroupView view)
        {
            logger.LogInformation("JoinGroups() method called");
            try {
                logger.LogDebug("Trying to join group");
            if (view.connectionId == null || view.groups == null)
            {
                logger.LogWarning("JoinGroups() method failed as there were was a null argument");
                return BadRequest("Something went wrong.");
            }
            foreach (string item in view.groups)
            {
                _context.Groups.AddToGroupAsync(view.connectionId, item);
            }
            }catch(Exception ex)
            {
                logger.LogError(ex.Message);
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }

            logger.LogInformation("JoinGroups() method was succesfull");
            return Ok();
        }

        [HttpGet, Route("group/{identificationCode}")]
        public IActionResult GetMessages(string identificationCode)
        {
            logger.LogInformation("GetMessages() method called");
            try
            {
                IActionResult response = Unauthorized();
                List<SendMessageView> messages = logic.GetMessages(identificationCode);
                response = Ok(new { messages = messages });
                logger.LogInformation("GetMessages() method was succesfull");
                return response;
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("The id may not be empty or null.");
            }
            catch(Exception ex)
            {
                return Unauthorized();
            }
           
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> PostTextMessage(MessageView view)
        {
            logger.LogInformation("Log message in the PostTextMessage() method");
            string id = User.FindFirstValue("id");
            IActionResult response;
            try
            {
            
                Message result = logic.PostMessage(view, id);

                if (result != null)
                {
                    logger.LogDebug("Starting websocket with following parameters: senderId = " + result.senderId + " || IdentificationCode = " + result.identificationCode +
                        " || message = " + result.message + " || Type = " + result.type);

                       await _context.Clients.Group(view.IdentificationCode).SendAsync("Groupsend", new SendMessageView(result.senderId, result.identificationCode, result.message, result.type, result.date));
                    logger.LogInformation("PostTextMessage() method was succesfull");
                    response = Ok();
                }
                else
                    response = BadRequest();
            }catch(ArgumentNullException ex)
            {
                logger.LogInformation("PostTextMessage() method failed. Arguments were null");
                return BadRequest("Something went wrong");
            } catch(LengthException ex)
            {
                return BadRequest(ex.Message);
            }

            return response;
        }
        [HttpPost, Route("image"),RequestSizeLimit(10000000) , Authorize]
        public async Task<IActionResult> PostImageMessage([FromForm] MessageView view)
        {
            logger.LogInformation("Log message in the PostImageMessage() method");
            string id = User.FindFirstValue("id");
            try
            {
                IActionResult response;
                Message result = logic.PostImage(view, id);

                if (result != null)
                {
                    await _context.Clients.Group(view.IdentificationCode).SendAsync("Groupsend", new SendMessageView(result.senderId, result.identificationCode, result.message, result.type, result.date));
                    response = Ok();
                }
                else
                    response = BadRequest();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Something went wrong");
            }
            catch (LengthException ex)
            {
                return BadRequest(ex.Message);
            }

            return Unauthorized();
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


        [HttpPost, Route("friends/recent")]
        public IActionResult GetMostRecentMessages([FromBody]List<string> identificationCode)
        {
            logger.LogInformation("MostRecentMessages() method called");
            try
            {
                Console.WriteLine("Attempting to get friend messages");

                IActionResult response = Unauthorized();
                List<RecentMessageView> messages = logic.GetMostRecentMessages(identificationCode.ToList());
                response = Ok(new { messages = messages });
                logger.LogInformation("MostRecentMessages() method succesfull");
                return response;
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("The id may not be empty or null.");
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

        }
    }
}

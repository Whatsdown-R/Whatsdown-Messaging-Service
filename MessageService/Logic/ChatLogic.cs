using MessageService.Data;
using MessageService.Exceptions;
using MessageService.Model;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whatsdown_Authentication_Service.Data;

namespace MessageService.Logic
{
    public class ChatLogic
    {
        ChatRepository repository;
        private readonly ILogger<ChatLogic> logger;
        public ChatLogic(ChatContext context, ILogger<ChatLogic> logger)
        {
            this.repository = new ChatRepository(context);
            this.logger = logger;
        }

        public Message PostMessage(MessageView view, string id)
        {
            logger.LogInformation("PostMessage() Called in ChatLogic");
            if (!CheckIfNull(view, id))
                return null;

            Message message = this.repository.PostMessage(view , id);
            logger.LogInformation("PostMessage() was succesfull in ChatLogic");
            return message;

        }

        public Message PostImage(MessageView view, string id)
        {
            if (!CheckIfNull(view, id))
                return null;

        /*    int found = view.Message.IndexOf(",") + 1;
            view.Message = view.Message.Remove(0, found);*/
            return this.repository.PostMessage(view, id);


        }

        private bool CheckIfNull(MessageView view, string id)
        {
            if (view.IdentificationCode == null || view.Message == null || id == null)
            {
               
                logger.LogWarning("CheckIfNull() has found null values");
                throw new ArgumentNullException();
            }
            
            if (view.Message.Length > 250 && view.Type != "IMAGE")
            {
                logger.LogWarning("CheckIfNull() has found that the message length is over the 250 characters.");
                throw new LengthException("Message length may only be 250 characters.");
            }
            // Also add auth to check if sender is the real sender
          

            return true;
        }

        //This can only be called if the person has access to the group/friend
        public List<SendMessageView> GetMessages(string identification)
        {
            if (identification == null)
            {
                logger.LogWarning("GetMessage() method failed as the identification code was null");
                throw new ArgumentNullException();

            }

            List<Message> messages =this.repository.GetAllMessages(identification);
            List<SendMessageView> views = new List<SendMessageView>();
            foreach (Message item in messages)
            {
                views.Add(new SendMessageView(item.senderId, item.identificationCode, item.message, item.type , item.date));
            }
            views.Sort((x, y) => x.date.CompareTo(y.date));
            logger.LogInformation("GetMessage() method succesfull");
            return views;
        }

        public bool DeleteMessage(DeleteMessageView message)
        {
            if (message.messageId == null || message.senderId == null)
                throw new ArgumentNullException();

                this.repository.DeleteMessage(message);
                logger.LogInformation("Succesfully removed the message with messageID: " + message.messageId);
                return true;
           
           
        }

        public List<RecentMessageView> GetMostRecentMessages(List<string> ids)
        {
            try
            {
                logger.LogInformation("GetMostRecentMessages() method has been called");
                List<RecentMessageView> messageViews = this.repository.GetMostRecentMessage(ids);
                logger.LogInformation("GetMostRecentMessages() method was succesfull");
                return messageViews;
            }catch(Exception ex)
            {
                logger.LogError("GetMostRecentMessage in ChatLogic came across following error: " + ex.Message);
                throw new Exception();
            }
        }
    }
}

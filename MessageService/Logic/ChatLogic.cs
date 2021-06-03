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
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public ChatLogic(ChatContext context)
        {
            this.repository = new ChatRepository(context);
          
        }

        public Message PostMessage(MessageView view)
        {
            if (!CheckIfNull(view))
                return null;

            return this.repository.PostMessage(view);
            

        }

        public Message PostImage(MessageView view)
        {
            if (!CheckIfNull(view))
                return null;

        /*    int found = view.Message.IndexOf(",") + 1;
            view.Message = view.Message.Remove(0, found);*/
            return this.repository.PostMessage(view);


        }

        private bool CheckIfNull(MessageView view)
        {
            if (view.IdentificationCode == null || view.Message == null || view.SenderId == null)
            {
               
                logger.Error("Cant post a message if it has null values");
                throw new ArgumentNullException();
            }
            
            if (view.Message.Length > 250 && view.Type != "IMAGE")
            {
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
                throw new ArgumentNullException();

            }

            List<Message> messages =this.repository.GetAllMessages(identification);
            List<SendMessageView> views = new List<SendMessageView>();
            foreach (Message item in messages)
            {
                views.Add(new SendMessageView(item.senderId, item.identificationCode, item.message, item.type , item.date));
            }
            views.Sort((x, y) => x.date.CompareTo(y.date));
            return views;
        }

        public bool DeleteMessage(DeleteMessageView message)
        {
            if (message.messageId == null || message.senderId == null)
                throw new ArgumentNullException();

                this.repository.DeleteMessage(message);
                logger.Info("Succesfully removed the message with messageID: " + message.messageId);
                return true;
           
           
        }

        public List<RecentMessageView> GetMostRecentMessages(List<string> ids)
        {
            try
            {
                return this.repository.GetMostRecentMessage(ids);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception();
            }
        }
    }
}

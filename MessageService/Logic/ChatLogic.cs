using MessageService.Data;
using MessageService.Model;
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
        public ChatLogic(ChatContext context)
        {
            this.repository = new ChatRepository(context);
        }

        public bool PostMessage(MessageView view)
        {
            if (view.IdentificationCode == null)
            {
                return false;
            }
            if (view.Message == null)
            {
                return false;

            }
            if (view.Message.Length > 250 && view.Type != "IMAGE")
            {
                return false;
            }
            // Also add auth to check if sender is the real sender
            if (view.SenderId == null)
            {
                return false;
            }
            
            this.repository.PostMessage(view);

            return true;

        }


        //This can only be called if the person has access to the group/friend
        public List<Message> GetMessages(string identification)
        {
            if (identification == null)
                return null;

            return this.repository.GetAllMessages(identification);
        }

        public bool DeleteMessage(DeleteMessageView message)
        {
            try
            {
                this.DeleteMessage(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
           
        }
    }
}

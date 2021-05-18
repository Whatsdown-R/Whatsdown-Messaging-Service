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

            int found = view.Message.IndexOf(",") + 1;
            view.Message = view.Message.Remove(0, found);
            return this.repository.PostMessage(view);


        }

        private bool CheckIfNull(MessageView view)
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

            return true;
        }

        //This can only be called if the person has access to the group/friend
        public List<SendMessageView> GetMessages(string identification)
        {
            if (identification == null)
                return null;

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

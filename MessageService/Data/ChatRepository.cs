using MessageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whatsdown_Authentication_Service.Data;

namespace MessageService.Data
{
    public class ChatRepository
    {
        ChatContext dbContext;

        public ChatRepository(ChatContext chatContext)
        {
            this.dbContext = chatContext;
        }

        public Message PostMessage(MessageView message, string id)
        {
            try
            {
                Message postMessage = new Message(Guid.NewGuid().ToString(), id, message.IdentificationCode, message.Message, message.Type, DateTime.Now);
                this.dbContext.Messages.Add(postMessage);
                this.dbContext.SaveChanges();
                return postMessage;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;

        }

        public bool DeleteMessage(DeleteMessageView message)
        {
            try
            {
                Message mes = this.dbContext.Messages.SingleOrDefault<Message>(m => m.senderId == message.senderId && m.Id == message.messageId);
                if (mes != null)
                {
                    this.dbContext.Messages.Remove(mes);
                    this.dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }

        public List<Message> GetAllMessages(string identification)
        {
            try
            {
                return this.dbContext.Messages.Where(messages => messages.identificationCode == identification).OrderByDescending(date => date.date).ToList();
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;

        }

        public List<RecentMessageView> GetMostRecentMessage(List<string> identificationcode)
        {
            List<RecentMessageView> recentMessages = new List<RecentMessageView>();
            foreach (string identification in identificationcode)
            {
                Message message = dbContext.Messages.Where(c => c.identificationCode == identification).OrderByDescending(c => c.date).FirstOrDefault();
                if (message == null)
                {
                    continue;
                }
              
                RecentMessageView view = new RecentMessageView();
                view.identificationCode = identification;
                //view.date = message.date;
                view.senderId = message.senderId;
                if (message != null)
                {
                    if (message.message.Length > 15)
                    {
                        string temp = message.message.Substring(0, 14);
                        temp +=  "...";
                        view.mostRecentMessage = temp;
                    }
                    recentMessages.Add(view);
                }
            }
            return recentMessages;
        }
    }
}

using MessageService.Model;
using Microsoft.EntityFrameworkCore;


namespace Whatsdown_Authentication_Service.Data
{
    public class ChatContext : DbContext
    {

        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {

        }

        public DbSet<Message> Messages { get; set; }
       


       
    }
}

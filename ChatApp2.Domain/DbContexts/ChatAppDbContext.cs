using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp2.Domain.DbContexts
{
    public class ChatAppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<User>(user => user.HasIndex(x => x.UserName).IsUnique(false));

            builder.Entity<Chat>()
            .HasMany(c => c.Messages)
            .WithOne(e => e.Chat);
        }

        public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Chat> Chats { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<Message> Messages { get; set; }


    }

}
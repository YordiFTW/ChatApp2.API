using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp2.Domain.DbContexts
{
    public class ChatAppDbContext : IdentityDbContext<User>
    {
        public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options)
            : base(options)
        {

        }

        public ChatAppDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(user => user.HasIndex(x => x.UserName).IsUnique(false));
            modelBuilder.Entity<User>(user => user.HasIndex(x => x.UserType).IsUnique(false));

            //builder.Entity<Chat>()
            //.HasMany(c => c.Messages)
            //.WithOne(e => e.Chat);

            modelBuilder.Entity<Message>()
            .HasOne<Chat>(s => s.Chat)
            .WithMany(g => g.Messages)
            .HasForeignKey(s => s.ChatId);


            modelBuilder.Entity<Group>().HasKey(sc => new { sc.UserId, sc.ChatId });

            modelBuilder.Entity<Group>()
                .HasOne<User>(sc => sc.User)
                .WithMany(s => s.UserChats)
                .HasForeignKey(sc => sc.UserId);


            modelBuilder.Entity<Group>()
                .HasOne<Chat>(sc => sc.Chat)
                .WithMany(s => s.Group)
                .HasForeignKey(sc => sc.ChatId);
        }

        //public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }

        //public Microsoft.EntityFrameworkCore.DbSet<Group> Groups { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<Chat> Chats { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<Message> Messages { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<Group> Groups { get; set; }

    }

}
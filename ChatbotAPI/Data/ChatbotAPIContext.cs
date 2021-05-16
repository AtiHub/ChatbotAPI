using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Models;

namespace ChatbotAPI.Data {
    public class ChatbotAPIContext : DbContext{
        public ChatbotAPIContext(DbContextOptions<ChatbotAPIContext> options)
            : base(options) {
        }

        public DbSet<ChatbotAPI.Models.Question> Question { get; set; }

        public DbSet<ChatbotAPI.Models.Answer> Answer { get; set; }

        public DbSet<ChatbotAPI.Models.Category> Category { get; set; }

        public DbSet<ChatbotAPI.Models.FAQ> FAQ { get; set; }

        public DbSet<ChatbotAPI.Models.AskUs> AskUs { get; set; }
    }
}

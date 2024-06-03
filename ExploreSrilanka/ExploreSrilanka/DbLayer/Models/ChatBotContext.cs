using ExploreSrilanka.DbLayer.Domains;
using Microsoft.EntityFrameworkCore;

namespace ExploreSrilanka.DbLayer.Models
{
    public class ChatBotContext : DbContext
    {
        #pragma warning disable CS8618
        
        public ChatBotContext(DbContextOptions<ChatBotContext> options) : base(options) { }

        #pragma warning restore CS8618

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /// Question has one intent
            /// And intent has many questions
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Intent)
                .WithMany(i => i.Questions)
                .HasForeignKey(q => q.IntentId)
                .OnDelete(DeleteBehavior.NoAction);

            /// Answer has one question
            /// And questio has many answers
            modelBuilder.Entity<BotAnswer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.BotAnswers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            /// Answer has one answer type
            /// And type has many answers
            modelBuilder.Entity<BotAnswer>()
                .HasOne(a => a.AnswerType)
                .WithMany(t => t.BotAnswers)
                .HasForeignKey(a => a.AnswerTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            /// Intent keyword has one intent type
            /// Intent type has many keywords
            modelBuilder.Entity<UserIntent>()
                .HasOne(ui => ui.Intent)
                .WithMany(i => i.UserIntents)
                .HasForeignKey(ui => ui.IntentId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        /// <summary>
        /// Intent
        /// Ex : Greeting, Farewell, Find 
        /// </summary>
        public DbSet<Intent> Intents                  { get; set; }

        /// <summary>
        /// Set of questions
        /// </summary>
        public DbSet<Question> Questions              { get; set; }

        /// <summary>
        /// Answer type
        /// Ex : Text, Button
        /// </summary>
        public DbSet<AnswerType> AnswerTypes          { get; set; }

        /// <summary>
        /// Answer for question 
        /// In various format Text, Image
        /// </summary>
        public DbSet<BotAnswer> BotAnswers            { get; set; }

        /// <summary>
        /// Intent kewords
        /// Ex : What is, Search for, Find
        /// </summary>
        public DbSet<UserIntent> UserIntents          { get; set; }

        /// <summary>
        /// Chatbot dont know answer for this questions
        /// </summary>
        public DbSet<UnkownQuestions> UnkownQuestions { get; set; }
    }
}

using System.Data.Entity;
using OfficeCommunicator.Dal.Entities;

namespace OfficeCommunicator.Dal.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("OfficeCommunicatorConStr") {}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMessage>()
                .HasRequired(x => x.Sender)
                .WithMany(x => x.SentMessages)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ChatMessage>()
                .HasRequired(x => x.Recipient)
                .WithMany(x => x.ReceivedMessages)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
    }
}

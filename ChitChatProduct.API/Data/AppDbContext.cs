using ChitChatProduct.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChitChatProduct.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>(entity =>
            {
                entity.HasOne(p=>p.User)
                .WithMany(u=>u.Products)
                .HasForeignKey(p=>p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Conversation>(entity =>
            {
                // with product
                entity.HasOne(c => c.Product)
                .WithMany(p => p.Conversations)
                .HasForeignKey(c => c.ProductId);

                //with seller
                entity.HasOne(c=>c.Seller)
                .WithMany(s=>s.SalesConversations)
                .HasForeignKey(c=>c.SellerId)
                .OnDelete(DeleteBehavior.Restrict);


                //with buyer
                entity.HasOne(c => c.Buyer)
                .WithMany(s => s.BuyingConversations)
                .HasForeignKey(c => c.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

                //unique constraint
                entity.HasIndex(c => new { c.ProductId, c.BuyerId }).IsUnique();
            });

            builder.Entity<ChatMessage>(entity =>
            {
                entity.HasOne(cm => cm.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(cm => cm.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cm=>cm.Sender)
                .WithMany(s=>s.SentMessages)
                .HasForeignKey(cm=>cm.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cm=>cm.Receiver)
                .WithMany(r=>r.ReceivedMessages)
                .HasForeignKey(cm=> cm.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

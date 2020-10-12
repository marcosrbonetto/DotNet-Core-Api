using System;
using ApiDebts.Src.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiDebts.Src.DAO
{
    public class DebtsContext : DbContext
    {
        public DebtsContext(DbContextOptions<DebtsContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureContact(modelBuilder.Entity<Contact>());
            ConfigureUser(modelBuilder.Entity<User>());
            ConfigureUserToken(modelBuilder.Entity<UserToken>());

            modelBuilder.Entity<Model.Entity.User>().HasData(
              new User { Id = 1, Name = "Root", Email = "root@debts.com", UID = "root", CreationUserId = 1, CreationDate = DateTime.Now }
            );
        }

        private void ConfigureBase<T>(EntityTypeBuilder<T> tEntity) where T : Base
        {
            tEntity.HasKey((x) => x.Id);
            tEntity.Property(x => x.Id).HasIdentityOptions(100);
            tEntity.Property((x) => x.CreationDate).IsRequired();
            tEntity.Property((x) => x.CreationUserId).IsRequired();

            //Creation user
            tEntity
                .HasOne(x => x.CreationUser)
                .WithMany()
                .HasForeignKey(x => x.CreationUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            //Modification user
            tEntity
               .HasOne((x) => x.ModificationUser)
               .WithMany()
               .HasForeignKey(x => x.ModificationUserId)
               .OnDelete(DeleteBehavior.Restrict);

            //Delete user
            tEntity
               .HasOne((x) => x.DeleteUser)
               .WithMany()
               .HasForeignKey(x => x.DeleteUserId)
               .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureContact(EntityTypeBuilder<Contact> entity)
        {
            entity.ToTable("Contact");
            ConfigureBase(entity);

            entity.Property((x) => x.Name).HasMaxLength(200);
            entity.Property((x) => x.Description).HasMaxLength(4000);
            entity.Property((x) => x.UserId);
            entity.Property((x) => x.UserOwnerId).IsRequired();
            entity.Property((x) => x.LastUseDate);

            //User
            entity
               .HasOne((x) => x.User)
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            //User Owner
            entity
               .HasOne((x) => x.UserOwner)
               .WithMany()
               .HasForeignKey(x => x.UserOwnerId)
               .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureUser(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User");
            ConfigureBase(entity);

            entity.Property((x) => x.UID).HasMaxLength(300);
            entity.Property((x) => x.Name).IsRequired().HasMaxLength(200);
            entity.Property((x) => x.Email).IsRequired().HasMaxLength(500);
            entity.Property((x) => x.Picture).HasMaxLength(2000);
            entity.Property((x) => x.Code).HasMaxLength(2000);
        }

        private void ConfigureUserToken(EntityTypeBuilder<UserToken> entity)
        {
            entity.ToTable("UserToken");
            ConfigureBase(entity);

            entity.Property((x) => x.UserId).IsRequired();
            entity.Property((x) => x.LimitDate);
            entity.Property((x) => x.Data).IsRequired().HasMaxLength(2000);

            //User
            entity
               .HasOne((x) => x.User)
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
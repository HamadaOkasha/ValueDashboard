using App.Domain.Model;
//using App.Domain.Models;
using App.Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
namespace App.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<LocalizedProperty> LocalizedProperty { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<GeneralSetting> GeneralSetting { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AdminPages> AdminPages { get; set; }
        public virtual DbSet<RolePages> RolePages { get; set; }
        public virtual DbSet<MessageTemplate> MessageTemplate { get; set; }
        public virtual DbSet<MailSetting> MailSetting { get; set; }

       // public virtual DbSet<OrderImage> OrderImage { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Sector> Sector { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Value> Value { get; set; }
        public virtual DbSet<ResearchValue> ResearchValue { get; set; }
        public virtual DbSet<AlrowadVersion> AlrowadVersion { get; set; }
        public virtual DbSet<AlrowadData> AlrowadData { get; set; }

       // public virtual DbSet<Items> Items { get; set; }
       // public virtual DbSet<Order> Order { get; set; }
       // public virtual DbSet<OrderStatus> OrderStatus { get; set; }

        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<LocaleStringResource> LocaleStringResource { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
      
        public virtual int Commit()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GeneralSettingConfiguration());
            modelBuilder.ApplyConfiguration(new AdminPagesConfiguration());
           // modelBuilder.ApplyConfiguration(new ItemsConfiguration());
            modelBuilder.ApplyConfiguration(new AspNetRoleClaimsConfiguration());
            modelBuilder.ApplyConfiguration(new AspNetRolesConfiguration());
            modelBuilder.ApplyConfiguration(new AspNetUserClaimsConfiguration());
            modelBuilder.ApplyConfiguration(new AspNetUserLoginsConfiguration());
            modelBuilder.ApplyConfiguration(new AspNetUserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new AspNetUsersConfiguration());
            modelBuilder.ApplyConfiguration(new AspNetUserTokensConfiguration());
            modelBuilder.ApplyConfiguration(new RolePagesConfiguration());
            modelBuilder.ApplyConfiguration(new MessageTemplateConfiguration());
            modelBuilder.ApplyConfiguration(new MailSettingConfiguration());
           // modelBuilder.ApplyConfiguration(new OrderImageConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new SectorConfiguration());
            modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ValueConfiguration());
            modelBuilder.ApplyConfiguration(new ResearchValueConfiguration());
            modelBuilder.ApplyConfiguration(new AlrowadVersionConfiguration());
            //modelBuilder.ApplyConfiguration(new OrderStatusConfiguration());
          //  modelBuilder.ApplyConfiguration(new OrderConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

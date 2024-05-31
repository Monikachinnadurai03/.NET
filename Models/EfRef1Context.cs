using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IMS_WebApplication.Models;

public partial class EfRef1Context : DbContext
{
    public EfRef1Context()
    {
    }

    public EfRef1Context(DbContextOptions<EfRef1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientInvestment> ClientInvestments { get; set; }

    public virtual DbSet<Investment> Investments { get; set; }

    public virtual DbSet<InvestmentType> InvestmentTypes { get; set; }

    public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }
    public virtual DbSet<Login> Logins { get; set; }
    //public virtual DbSet<User> SignUps { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=MONIKA\\SQLEXPRESS;Database=EF_ref1;Trusted_Connection=True;TrustServerCertificate=True");

    [Obsolete]
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Client__75A5D7189AF87B7B");

            entity.ToTable("Client");

            entity.Property(e => e.ClientId)
                //.ValueGeneratedNever()
                .ValueGeneratedOnAdd()
                .HasColumnName("Client_ID");
            entity.Property(e => e.City)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
            entity.Property(e => e.PanCardNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PAN_Card_No");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.State)
                .HasMaxLength(20)
                .IsUnicode(false);
            
        });

        modelBuilder.Entity<ClientInvestment>(entity =>
        {
            entity.HasKey(e => e.ClientInvestmentId).HasName("PK__Client_I__125BA18A2558A3ED");

            entity.ToTable("Client_Investment");

            entity.Property(e => e.ClientInvestmentId)
                .ValueGeneratedNever()
                .HasColumnName("Client_Investment_ID");
            entity.Property(e => e.ClientId).HasColumnName("Client_ID");
            entity.Property(e => e.InvestmentAmount)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("Investment_Amount");
            entity.Property(e => e.InvestmentDate).HasColumnName("Investment_Date");
            entity.Property(e => e.InvestmentId).HasColumnName("Investment_ID");
            entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientInvestments)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Client_In__Clien__619B8048");

            entity.HasOne(d => d.Investment).WithMany(p => p.ClientInvestments)
                .HasForeignKey(d => d.InvestmentId)
                .HasConstraintName("FK__Client_In__Inves__628FA481");
        });

        modelBuilder.Entity<Investment>(entity =>
        {
            entity.HasKey(e => e.InvestmentId).HasName("PK__Investme__0C059B9CBA3B8C99");

            entity.ToTable("Investment");

            entity.Property(e => e.InvestmentId)
                .ValueGeneratedNever()
                .HasColumnName("Investment_ID");
            entity.Property(e => e.InvestmentTypeId).HasColumnName("Investment_Type_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PurchaseDate).HasColumnName("Purchase_Date");
            entity.Property(e => e.PurchasePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Purchase_Price");

            entity.HasOne(d => d.InvestmentType).WithMany(p => p.Investments)
                .HasForeignKey(d => d.InvestmentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Investmen__Inves__5BE2A6F2");
        });

        modelBuilder.Entity<InvestmentType>(entity =>
        {
            entity.HasKey(e => e.InvestmentTypeId).HasName("PK__Investme__D52DA326245C7668");

            entity.ToTable("Investment_Type");

            entity.Property(e => e.InvestmentTypeId)
                .ValueGeneratedNever()
                .HasColumnName("Investment_Type_ID");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__9A8D56252DCD41E9");

            entity.ToTable("Transaction_Detail");

            entity.Property(e => e.TransactionId)
                .ValueGeneratedNever()
                .HasColumnName("Transaction_ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TransactionDate).HasColumnName("Transaction_Date");
            entity.Property(e => e.TransactionTypeId).HasColumnName("Transaction_Type_ID");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.TransactionTypeId)
                .HasConstraintName("FK__Transacti__Trans__693CA210");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.TransactionTypeId).HasName("PK__Transact__6E05F51F573E1D6B");

            entity.ToTable("Transaction_Type");

            entity.Property(e => e.TransactionTypeId)
                .ValueGeneratedNever()
                .HasColumnName("Transaction_Type_ID");
            entity.Property(e => e.TransactionType1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Transaction_Type");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.ToTable("LOGIN");

            entity.HasKey(e => e.UserId).HasName("PK__Login__206D9170F7B58FFB");
            entity.Property(e => e.UserId).HasColumnName("User_Id").ValueGeneratedOnAdd();
            entity.Property(e => e.UserName).HasColumnName("User_Name").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(50).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.PasswordSalt).IsRequired();
        });

        OnModelCreatingPartial(modelBuilder: modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

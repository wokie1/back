﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Backendec.Models
{
    /// <summary>
    /// dbcontext описывает все моменты по которым строится бд, классы превращаются в таблицы, задаются связи
    /// и указываются ключи
    /// </summary>

    public class AppDbContext:DbContext
    {

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public virtual DbSet<Availability> Availabilitys { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Sale> Sales { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

            => optionsBuilder.UseMySql("server=localhost;database=shopdb;user=root;password=ubuntu2!;",
            new MySqlServerVersion(new Version(8, 0, 3)));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка таблицы Product
            modelBuilder.Entity<Product>()
                .ToTable("Products")
                .HasKey(p=>p.Id);

            modelBuilder.Entity<Product>()
                .Property(p => p.ExpiryDate)
                .HasColumnName("Date");

            // Настройка таблицы Availability (композитный ключ)
            modelBuilder.Entity<Availability>()
                .ToTable("Availability")
                .HasKey(a => new { a.StoreId, a.ProductId });

            modelBuilder.Entity<Availability>()
                .Property(a => a.Quantity)
                .HasColumnName("Availability");

            modelBuilder.Entity<Availability>()
                .HasOne<Store>()
                .WithMany()
                .HasForeignKey(a => a.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Availability>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка таблицы Sale (композитный ключ)
            modelBuilder.Entity<Sale>()
                .ToTable("Sales")
                .HasKey(s => new { s.CustomerId, s.ProductId, s.SaleDate });

            modelBuilder.Entity<Sale>()
                .HasOne<Customer>()
                .WithMany()
                .HasForeignKey(s=>s.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(s=>s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Остальные таблицы
            modelBuilder.Entity<Customer>()
                .ToTable("Customers")
                .HasKey(c=>c.Id);

            modelBuilder.Entity<Store>()
                .ToTable("Stores")
                .HasKey(s=>s.Id);
        }
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using Microsoft.BotBuilderSamples.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Microsoft.BotBuilderSamples.DB
{
  public partial class ApplicationDBContext : DbContext
  {
    public ApplicationDBContext()
    {
    }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
      var config = builder.Build();
      var connectionString = config.GetConnectionString("DefaultConnection");

      if (!optionsBuilder.IsConfigured)
      {

        optionsBuilder.UseNpgsql(connectionString);
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      // modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

      // modelBuilder.Entity<Employee>(entity =>
      // {
      //   entity.HasKey(e => e.EmpId)
      //             .HasName("pk_emp_id");

      //   entity.ToTable("EMPLOYEE");

      //   entity.Property(e => e.EmpId)
      //             .HasMaxLength(10)
      //             .IsUnicode(false)
      //             .HasColumnName("EMP_ID");

      //   entity.Property(e => e.EmpName)
      //             .IsRequired()
      //             .HasMaxLength(50)
      //             .IsUnicode(false)
      //             .HasColumnName("EMP_NAME");
      // });

      // OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  }
}

  // {
  //   public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
  //   {
  //   }

  //   public DbSet<User> User { get; set; }

  //   protected override void OnModelCreating(ModelBuilder modelBuilder)
  //   {
  //     base.OnModelCreating(modelBuilder);
  //   }
  // }

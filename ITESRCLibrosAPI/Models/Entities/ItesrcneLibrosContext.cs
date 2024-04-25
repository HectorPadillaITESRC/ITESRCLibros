using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ITESRCLibrosAPI.Models.Entities;

public partial class ItesrcneLibrosContext : DbContext
{
    public ItesrcneLibrosContext()
    {
    }

    public ItesrcneLibrosContext(DbContextOptions<ItesrcneLibrosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Libros> Libros { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Libros>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("libros");

            entity.HasIndex(e => e.Id, "libros_Id_IDX");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Autor).HasMaxLength(100);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.Portada).HasMaxLength(100);
            entity.Property(e => e.Titulo).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebServiceRestaurante.Models;

public partial class CadenaRestaurantesContext : DbContext
{
    public CadenaRestaurantesContext()
    {
    }

    public CadenaRestaurantesContext(DbContextOptions<CadenaRestaurantesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EmploymentDetail> EmploymentDetails { get; set; }

    public virtual DbSet<EmploymentLog> EmploymentLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-8M0EFE1\\SQLEXPRESS;Database=cadenaRestaurantes;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmploymentDetail>(entity =>
        {
            entity.HasKey(e => e.IdDetalleContratacion).HasName("PK_detalle_contratacion");

            entity.ToTable("employment_details");

            entity.Property(e => e.IdDetalleContratacion).HasColumnName("id_detalle_contratacion");
            entity.Property(e => e.Cedula)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cedula");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FinContrato)
                .HasColumnType("date")
                .HasColumnName("fin_contrato");
            entity.Property(e => e.InicioJornada).HasColumnName("inicio_jornada");
            entity.Property(e => e.NitRestaurante).HasColumnName("nit_restaurante");

            entity.HasOne(d => d.CedulaNavigation).WithMany(p => p.EmploymentDetails)
                .HasForeignKey(d => d.Cedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employment_details_workers");
        });

        modelBuilder.Entity<EmploymentLog>(entity =>
        {
            entity.HasKey(e => e.IdRegistro).HasName("PK_recruitment_log");

            entity.ToTable("employment_log");

            entity.Property(e => e.IdRegistro).HasColumnName("id_registro");
            entity.Property(e => e.EmpleadoModificado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("empleado_modificado");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FinContrato)
                .HasColumnType("date")
                .HasColumnName("fin_contrato");
            entity.Property(e => e.InicioJornada).HasColumnName("inicio_jornada");
            entity.Property(e => e.NitRestaurante).HasColumnName("nit_restaurante");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasIndex(e => e.Cedula, "IX_users").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cedula)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cedula");
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("clave");

            entity.HasOne(d => d.CedulaNavigation).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.Cedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_users_workers1");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("cedula_empleados");

            entity.ToTable("workers");

            entity.HasIndex(e => e.Correo, "correo_empleados").IsUnique();

            entity.HasIndex(e => e.Telefono, "telefono_empleados").IsUnique();

            entity.Property(e => e.Cedula)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cedula");
            entity.Property(e => e.Cargo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cargo");
            entity.Property(e => e.Correo)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Eco_Toner.Models.DB;

public partial class ProyectoEcoTonerContext : DbContext
{
    public ProyectoEcoTonerContext()
    {
    }

    public ProyectoEcoTonerContext(DbContextOptions<ProyectoEcoTonerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Impresora> Impresoras { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Modelo> Modelos { get; set; }

    public virtual DbSet<Oficina> Oficinas { get; set; }

    public virtual DbSet<OrdenDetalle> OrdenDetalles { get; set; }

    public virtual DbSet<OrdenMaestro> OrdenMaestros { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DIEGO\\SQLEXPRESS; Database=Proyecto_Eco_Toner; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Cliente__3DD0A8CBF5EC9DEA");

            entity.ToTable("Cliente");

            entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdOficina).HasColumnName("Id_Oficina");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOficinaNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdOficina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cliente__Id_Ofic__44FF419A");
        });

        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.IdContacto).HasName("PK__Contacto__86C6BBAFFF364418");

            entity.ToTable("Contacto");

            entity.Property(e => e.IdContacto).HasColumnName("Id_Contacto");
            entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Contactos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contacto__Id_Cli__47DBAE45");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("PK__Empresa__443B3D9D3C73AFB0");

            entity.ToTable("Empresa");

            entity.Property(e => e.IdEmpresa).HasColumnName("Id_Empresa");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__AB2EB6F81A35A099");

            entity.ToTable("Estado");

            entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
            entity.Property(e => e.Estado1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Estado");
        });

        modelBuilder.Entity<Impresora>(entity =>
        {
            entity.HasKey(e => e.IdImpresora).HasName("PK__Impresor__11EF88D462D27CE4");

            entity.ToTable("Impresora");

            entity.Property(e => e.IdImpresora).HasColumnName("Id_Impresora");
            entity.Property(e => e.IdModelo).HasColumnName("Id_Modelo");
            entity.Property(e => e.NumeroSerie).HasColumnName("Numero_Serie");

            entity.HasOne(d => d.IdModeloNavigation).WithMany(p => p.Impresoras)
                .HasForeignKey(d => d.IdModelo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Impresora__Id_Mo__4AB81AF0");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca).HasName("PK__Marca__28EFE28A14F4DE71");

            entity.ToTable("Marca");

            entity.Property(e => e.IdMarca).HasColumnName("Id_Marca");
            entity.Property(e => e.Marca1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Marca");
        });

        modelBuilder.Entity<Modelo>(entity =>
        {
            entity.HasKey(e => e.IdModelo).HasName("PK__Modelo__D5BA162A36D27344");

            entity.ToTable("Modelo");

            entity.Property(e => e.IdModelo).HasColumnName("Id_Modelo");
            entity.Property(e => e.IdMarca).HasColumnName("Id_Marca");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Modelos)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Modelo__Id_Marca__4222D4EF");
        });

        modelBuilder.Entity<Oficina>(entity =>
        {
            entity.HasKey(e => e.IdOficina).HasName("PK__Oficina__FBF5C5EF61AB8D3B");

            entity.ToTable("Oficina");

            entity.Property(e => e.IdOficina).HasColumnName("Id_Oficina");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_Empresa");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Oficinas)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Oficina__Id_Empr__3D5E1FD2");
        });

        modelBuilder.Entity<OrdenDetalle>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Orden_Detalle");

            entity.Property(e => e.Descripción)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IdDetalle)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id_detalle");
            entity.Property(e => e.IdOrden).HasColumnName("Id_Orden");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany()
                .HasForeignKey(d => d.IdOrden)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Det__Id_Or__52593CB8");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany()
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Det__Id_Us__534D60F1");
        });

        modelBuilder.Entity<OrdenMaestro>(entity =>
        {
            entity.HasKey(e => e.IdOrden).HasName("PK__Orden_Ma__370733B6FBBBE632");

            entity.ToTable("Orden_Maestro");

            entity.Property(e => e.IdOrden).HasColumnName("Id_Orden");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Inicio");
            entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");
            entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
            entity.Property(e => e.IdImpresora).HasColumnName("Id_Impresora");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.NoOrden)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("No_Orden");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.OrdenMaestros)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Mae__Id_Cl__4D94879B");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.OrdenMaestros)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Mae__Id_Es__4F7CD00D");

            entity.HasOne(d => d.IdImpresoraNavigation).WithMany(p => p.OrdenMaestros)
                .HasForeignKey(d => d.IdImpresora)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Mae__Id_Im__4E88ABD4");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.OrdenMaestros)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Mae__Id_Us__5070F446");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__63C76BE288555B52");

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Eco_Toner.Models.DB;

public partial class DbAb6668EcotonerContext : DbContext
{
    public DbAb6668EcotonerContext()
    {
    }

    public DbAb6668EcotonerContext(DbContextOptions<DbAb6668EcotonerContext> options)
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

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=SQL1001.site4now.net;Initial Catalog=db_ab6668_ecotoner;User Id=db_ab6668_ecotoner_admin;Password=GRUPOCUCU1.;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Cliente__3DD0A8CB3883E855");

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
                .HasConstraintName("FK__Cliente__Id_Ofic__5AEE82B9");
        });

        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.IdContacto).HasName("PK__Contacto__86C6BBAFFE952CC4");

            entity.ToTable("Contacto");

            entity.Property(e => e.IdContacto).HasColumnName("Id_Contacto");
            entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Contactos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contacto__Id_Cli__5DCAEF64");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("PK__Empresa__443B3D9D6FA7F06F");

            entity.ToTable("Empresa");

            entity.Property(e => e.IdEmpresa).HasColumnName("Id_Empresa");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__AB2EB6F84A8C2DE7");

            entity.ToTable("Estado");

            entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
            entity.Property(e => e.Estado1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Estado");
        });

        modelBuilder.Entity<Impresora>(entity =>
        {
            entity.HasKey(e => e.IdImpresora).HasName("PK__Impresor__11EF88D4532473E4");

            entity.ToTable("Impresora");

            entity.Property(e => e.IdImpresora).HasColumnName("Id_Impresora");
            entity.Property(e => e.IdModelo).HasColumnName("Id_Modelo");
            entity.Property(e => e.NumeroSerie)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Numero_Serie");

            entity.HasOne(d => d.IdModeloNavigation).WithMany(p => p.Impresoras)
                .HasForeignKey(d => d.IdModelo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Impresora__Id_Mo__60A75C0F");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca).HasName("PK__Marca__28EFE28AFE99491D");

            entity.ToTable("Marca");

            entity.Property(e => e.IdMarca).HasColumnName("Id_Marca");
            entity.Property(e => e.Marca1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Marca");
        });

        modelBuilder.Entity<Modelo>(entity =>
        {
            entity.HasKey(e => e.IdModelo).HasName("PK__Modelo__D5BA162A54A38676");

            entity.ToTable("Modelo");

            entity.Property(e => e.IdModelo).HasColumnName("Id_Modelo");
            entity.Property(e => e.IdMarca).HasColumnName("Id_Marca");
            entity.Property(e => e.NombreModelo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nombre_Modelo");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Modelos)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Modelo__Id_Marca__5812160E");
        });

        modelBuilder.Entity<Oficina>(entity =>
        {
            entity.HasKey(e => e.IdOficina).HasName("PK__Oficina__FBF5C5EF32128E54");

            entity.ToTable("Oficina");

            entity.Property(e => e.IdOficina).HasColumnName("Id_Oficina");
            entity.Property(e => e.Dirección)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_Empresa");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Oficinas)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Oficina__Id_Empr__4E88ABD4");
        });

        modelBuilder.Entity<OrdenDetalle>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PK__Orden_De__9274780B3F6072A8");

            entity.ToTable("Orden_Detalle");

            entity.Property(e => e.IdDetalle).HasColumnName("Id_Detalle");
            entity.Property(e => e.Descripción)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IdOrden).HasColumnName("Id_Orden");
            entity.Property(e => e.UsuarioAsignado).HasColumnName("Usuario_Asignado");
            entity.Property(e => e.UsuarioPrevio).HasColumnName("Usuario_Previo");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.OrdenDetalles)
                .HasForeignKey(d => d.IdOrden)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Det__Id_Or__693CA210");

            entity.HasOne(d => d.UsuarioAsignadoNavigation).WithMany(p => p.OrdenDetalleUsuarioAsignadoNavigations)
                .HasForeignKey(d => d.UsuarioAsignado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Det__Usuar__6B24EA82");

            entity.HasOne(d => d.UsuarioPrevioNavigation).WithMany(p => p.OrdenDetalleUsuarioPrevioNavigations)
                .HasForeignKey(d => d.UsuarioPrevio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Det__Usuar__6A30C649");
        });

        modelBuilder.Entity<OrdenMaestro>(entity =>
        {
            entity.HasKey(e => e.IdOrden).HasName("PK__Orden_Ma__370733B61DDD966D");

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
            entity.Property(e => e.NumeroOrden)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Numero_Orden");
            entity.Property(e => e.UsuarioCreación).HasColumnName("Usuario_creación");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.OrdenMaestros)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Mae__Id_Cl__6383C8BA");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.OrdenMaestros)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Mae__Id_Es__656C112C");

            entity.HasOne(d => d.IdImpresoraNavigation).WithMany(p => p.OrdenMaestros)
                .HasForeignKey(d => d.IdImpresora)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Mae__Id_Im__6477ECF3");

            entity.HasOne(d => d.UsuarioCreaciónNavigation).WithMany(p => p.OrdenMaestros)
                .HasForeignKey(d => d.UsuarioCreación)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orden_Mae__Usuar__66603565");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__55932E86EBBF3305");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.Rol1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Rol");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__63C76BE2C2BF2A6D");

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
            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Usuario1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Usuario");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__Id_Rol__5535A963");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

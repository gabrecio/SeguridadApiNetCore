using System;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;



namespace SEG.Repositories.DataContext
{
    public partial class SeguridadEntities : DbContext
    {     

        public SeguridadEntities(DbContextOptions<SeguridadEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<Aplicacion> Aplicacion { get; set; }
        public virtual DbSet<CentroCosto> CentroCosto { get; set; }
        public virtual DbSet<EntidadTipo> EntidadTipo { get; set; }
        public virtual DbSet<ListaPermiso> ListaPermiso { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Operacion> Operacion { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<RolPermiso> RolPermiso { get; set; }
        public virtual DbSet<SistemaForaneo> SistemaForaneo { get; set; }
       
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioForaneo> UsuarioForaneo { get; set; }
        public virtual DbSet<UsuarioRol> UsuarioRol { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=Tango02\\Tango02;Database=Seguridad;user=sa;password=Tango@02;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aplicacion>(entity =>
            {
                entity.HasIndex(e => e.Codigo)
                    .HasName("UQ_Aplicacion_Codigo")
                    .IsUnique();

                entity.HasIndex(e => e.Descripcion)
                    .HasName("UQ_Aplicacion_Descripcion")
                    .IsUnique();

                entity.Property(e => e.Codigo)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Color).IsFixedLength();

                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.Property(e => e.Observaciones).IsUnicode(false);
            });

            modelBuilder.Entity<CentroCosto>(entity =>
            {
                entity.Property(e => e.Codigo)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Nombre).IsUnicode(false);

                entity.HasOne(d => d.CentroCostoPadre)
                    .WithMany(p => p.InverseCentroCostoPadre)
                    .HasForeignKey(d => d.CentroCostoPadreId)
                    .HasConstraintName("FK_CentroCosto_CentroCosto");
            });

            modelBuilder.Entity<EntidadTipo>(entity =>
            {
                entity.Property(e => e.Nombre).IsUnicode(false);
            });

            modelBuilder.Entity<ListaPermiso>(entity =>
            {
                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.ListaPermiso)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Listap_menu");

                entity.HasOne(d => d.Operacion)
                    .WithMany(p => p.ListaPermiso)
                    .HasForeignKey(d => d.OperacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Listap_permisos");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasIndex(e => e.Clave)
                    .HasName("UQ_Menu_clave")
                    .IsUnique();

                entity.Property(e => e.Clave).IsUnicode(false);

                entity.Property(e => e.Imagen).IsUnicode(false);

                entity.Property(e => e.OrderBy).IsUnicode(false);

                entity.Property(e => e.Titulo).IsUnicode(false);

                entity.HasOne(d => d.App)
                    .WithMany(p => p.Menu)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Menu_Aplicacion");

                entity.HasOne(d => d.TipoEntidad)
                    .WithMany(p => p.Menu)
                    .HasForeignKey(d => d.TipoEntidadId)
                    .HasConstraintName("FK_Menu_EntidadTipo");
            });

            modelBuilder.Entity<Operacion>(entity =>
            {
                entity.HasIndex(e => e.Nombre)
                    .HasName("UQ_Oper_nombre")
                    .IsUnique();

                entity.Property(e => e.Imagen).IsUnicode(false);

                entity.Property(e => e.Nombre).IsUnicode(false);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasIndex(e => new { e.Nombre, e.AppId })
                    .HasName("UQ_Rol_nombre_aplicacion")
                    .IsUnique();

                entity.Property(e => e.Nombre).IsUnicode(false);

                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.HasOne(d => d.App)
                    .WithMany(p => p.Rol)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rol_Aplicacion");
            });

            modelBuilder.Entity<RolPermiso>(entity =>
            {
                entity.HasKey(e => new { e.RolId, e.ListaPermisoId })
                    .HasName("PK_Rol_7263EEA53A154D41");

                entity.HasOne(d => d.ListaPermiso)
                    .WithMany(p => p.RolPermiso)
                    .HasForeignKey(d => d.ListaPermisoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RolPermiso_permiso");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.RolPermiso)
                    .HasForeignKey(d => d.RolId)
                    .HasConstraintName("FK_RolPermiso_rol");
            });

            modelBuilder.Entity<SistemaForaneo>(entity =>
            {
                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.Property(e => e.Sistema).IsUnicode(false);
            });

         
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .HasName("UQ_Usuario_username")
                    .IsUnique();

                entity.Property(e => e.Apellido).IsUnicode(false);

                entity.Property(e => e.Documento).IsUnicode(false);

                entity.Property(e => e.Interno).IsUnicode(false);

                entity.Property(e => e.Legajo)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Mail).IsUnicode(false);

                entity.Property(e => e.Nombre).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Sexo)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Username).IsUnicode(false);
            });

            modelBuilder.Entity<UsuarioForaneo>(entity =>
            {
                entity.HasIndex(e => new { e.SistemaForaneoId, e.CodUsuarioForaneo })
                    .HasName("UK_UsuarioForaneo_Sistema_Codigo")
                    .IsUnique();

                entity.Property(e => e.CodUsuarioForaneo).IsUnicode(false);

                entity.HasOne(d => d.SistemaForaneo)
                    .WithMany(p => p.UsuarioForaneo)
                    .HasForeignKey(d => d.SistemaForaneoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuarioForaneo_SistemaForaneo");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.UsuarioForaneo)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuarioForaneo_Usuario");
            });

            modelBuilder.Entity<UsuarioRol>(entity =>
            {
                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.UsuarioRol)
                    .HasForeignKey(d => d.RolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuarioRol_rol");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.UsuarioRol)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuarioRol_usuario");
            });    

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet para Operadores
        public DbSet<Operador> Operadores { get; set; }
        
        // DbSet para RolesOperador
        public DbSet<RolOperador> RolesOperador { get; set; }
        
        // DbSet para la tabla intermedia
        public DbSet<OperadorRolOperador> OperadorRolesOperador { get; set; }

        // DbSet para Planta
        public DbSet<Area> Areas { get; set; }
        public DbSet<Linea> Lineas { get; set; }
        public DbSet<Estacion> Estaciones { get; set; }
        public DbSet<Maquina> Maquinas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configurar explícitamente cada tabla de Identity en el schema "authentication"
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("AspNetUsers", "authentication");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("AspNetRoles", "authentication");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("AspNetUserRoles", "authentication");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("AspNetUserClaims", "authentication");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("AspNetUserLogins", "authentication");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("AspNetRoleClaims", "authentication");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("AspNetUserTokens", "authentication");
            });

            // Configuración para Operadores en schema "operadores"
            builder.Entity<Operador>(entity =>
            {
                entity.ToTable("Operadores", "operadores");
                
                entity.HasIndex(e => e.NumeroEmpleado)
                    .IsUnique()
                    .HasDatabaseName("IX_Operadores_NumeroEmpleado");

                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("NOW()");
            });

            // Configuración para RolesOperador en schema "operadores"
            builder.Entity<RolOperador>(entity =>
            {
                entity.ToTable("RolesOperador", "operadores");
                
                entity.HasIndex(e => e.Nombre)
                    .IsUnique()
                    .HasDatabaseName("IX_RolesOperador_Nombre");

                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("NOW()");
            });

            // Configuración para la tabla intermedia en schema "operadores"
            builder.Entity<OperadorRolOperador>(entity =>
            {
                entity.ToTable("OperadorRolesOperador", "operadores");

                entity.HasIndex(e => new { e.OperadorId, e.RolOperadorId })
                    .IsUnique()
                    .HasDatabaseName("IX_OperadorRolesOperador_OperadorId_RolOperadorId");

                entity.Property(e => e.FechaAsignacion)
                    .HasDefaultValueSql("NOW()");

                entity.HasOne(e => e.Operador)
                    .WithMany(o => o.OperadorRoles)
                    .HasForeignKey(e => e.OperadorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.RolOperador)
                    .WithMany(r => r.OperadorRoles)
                    .HasForeignKey(e => e.RolOperadorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para Areas en schema "planta"
            builder.Entity<Area>(entity =>
            {
                entity.ToTable("Areas", "planta");
                
                entity.HasIndex(e => e.Codigo)
                    .IsUnique()
                    .HasDatabaseName("IX_Areas_Codigo");

                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("NOW()");
            });

            // Configuración para Lineas en schema "planta"
            builder.Entity<Linea>(entity =>
            {
                entity.ToTable("Lineas", "planta");
                
                entity.HasIndex(e => e.Codigo)
                    .IsUnique()
                    .HasDatabaseName("IX_Lineas_Codigo");

                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("NOW()");

                entity.HasOne(e => e.Area)
                    .WithMany(a => a.Lineas)
                    .HasForeignKey(e => e.AreaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Estaciones en schema "planta"
            builder.Entity<Estacion>(entity =>
            {
                entity.ToTable("Estaciones", "planta");
                
                entity.HasIndex(e => e.Codigo)
                    .IsUnique()
                    .HasDatabaseName("IX_Estaciones_Codigo");

                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("NOW()");

                entity.HasOne(e => e.Linea)
                    .WithMany(l => l.Estaciones)
                    .HasForeignKey(e => e.LineaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Maquinas en schema "planta"
            builder.Entity<Maquina>(entity =>
            {
                entity.ToTable("Maquinas", "planta");
                
                entity.HasIndex(e => e.Codigo)
                    .IsUnique()
                    .HasDatabaseName("IX_Maquinas_Codigo");

                entity.HasIndex(e => e.NumeroSerie)
                    .IsUnique()
                    .HasDatabaseName("IX_Maquinas_NumeroSerie");

                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("NOW()");

                entity.HasOne(e => e.Estacion)
                    .WithMany(es => es.Maquinas)
                    .HasForeignKey(e => e.EstacionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

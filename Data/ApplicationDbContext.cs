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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configurar el schema para la tabla de historial de migraciones
            optionsBuilder.UseNpgsql(x => x.MigrationsHistoryTable("__EFMigrationsHistory", "authentication"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // IMPORTANTE: Configurar cada tabla de Identity DESPUÉS de base.OnModelCreating
            
            // Usuarios
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("AspNetUsers", "authentication");
            });

            // Roles
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("AspNetRoles", "authentication");
            });

            // UserRoles
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("AspNetUserRoles", "authentication");
            });

            // UserClaims
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("AspNetUserClaims", "authentication");
            });

            // UserLogins
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("AspNetUserLogins", "authentication");
            });

            // RoleClaims
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("AspNetRoleClaims", "authentication");
            });

            // UserTokens
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("AspNetUserTokens", "authentication");
            });

            // Configuración para Operadores en schema "produccion"
            builder.Entity<Operador>(entity =>
            {
                entity.ToTable("Operadores", "operadores");
                
                entity.HasIndex(e => e.NumeroEmpleado)
                    .IsUnique()
                    .HasDatabaseName("IX_Operadores_NumeroEmpleado");

                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("NOW()");
            });
        }
    }
}
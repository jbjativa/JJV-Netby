using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Web;

namespace NB_JaimeJativa.Repositorio
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<TareaDTO> Tareas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales pueden ir aquí
            modelBuilder.Entity<TareaDTO>().ToTable("Tarea");
            modelBuilder.Entity<TareaDTO>().HasKey(e => e.ID);
            modelBuilder.Entity<TareaDTO>().Property(e => e.Titulo).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<TareaDTO>().Property(e => e.FechaCreacion).IsRequired();
            modelBuilder.Entity<TareaDTO>().Property(e => e.Completada).IsRequired();
        }
    }
}
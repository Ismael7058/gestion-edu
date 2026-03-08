using Educacion.Desktop.Models;
using Microsoft.EntityFrameworkCore;

namespace Educacion.Desktop.Data;

public class EducacionDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configura la base de datos SQLite.
        // El archivo "educacion.db" se creará automáticamente en la carpeta de ejecución.
        optionsBuilder.UseSqlite("Data Source=educacion.db");
    }
}

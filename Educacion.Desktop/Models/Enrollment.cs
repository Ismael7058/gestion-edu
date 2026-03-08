using System;

namespace Educacion.Desktop.Models;

public class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    public string? Grade { get; set; } // Puede ser nulo si aún no tiene nota

    public Student? Student { get; set; }
    public Subject? Subject { get; set; }
}

namespace Educacion.Desktop.Models;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Credits { get; set; }
    
    public int? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
}

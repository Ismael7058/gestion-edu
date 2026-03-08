using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace Educacion.Desktop.Services;

public class ImageService
{
    private readonly string _storagePath;

    public ImageService()
    {
        // Define una carpeta "Images" en el directorio base de la aplicación
        _storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
        
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public async Task<string> SaveImageAsync(string sourceFilePath)
    {
        if (string.IsNullOrEmpty(sourceFilePath) || !File.Exists(sourceFilePath))
            return string.Empty;

        // Genera un nombre único para evitar colisiones
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(sourceFilePath)}";
        var destinationPath = Path.Combine(_storagePath, fileName);

        using var sourceStream = File.OpenRead(sourceFilePath);
        using var destStream = File.Create(destinationPath);
        await sourceStream.CopyToAsync(destStream);

        return destinationPath;
    }

    public Bitmap? LoadImage(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;
        return new Bitmap(path);
    }
}

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Educacion.Desktop.Models;
using Educacion.Desktop.Services;
using Educacion.Desktop.ViewModels;

namespace Educacion.Desktop.ViewModels.Student;

public partial class StudentListViewModel : ViewModelBase
{
    private readonly IDataService<Models.Student> _studentService;

    [ObservableProperty]
    private ObservableCollection<Models.Student> _students = new();

    [ObservableProperty]
    private Models.Student? _selectedStudent;

    public StudentListViewModel(IDataService<Models.Student> studentService)
    {
        _studentService = studentService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        var items = await _studentService.GetAllAsync();
        Students = new ObservableCollection<Models.Student>(items);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedStudent is null) return;
        await _studentService.DeleteAsync(SelectedStudent.Id);
        Students.Remove(SelectedStudent);
    }

    [RelayCommand]
    private void Create()
    {
        WeakReferenceMessenger.Default.Send(new EditStudentMessage(null));
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedStudent != null)
            WeakReferenceMessenger.Default.Send(new EditStudentMessage(SelectedStudent));
    }
}
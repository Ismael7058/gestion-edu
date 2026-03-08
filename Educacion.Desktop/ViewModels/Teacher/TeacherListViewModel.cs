using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Educacion.Desktop.Models;
using Educacion.Desktop.Services;
using Educacion.Desktop.ViewModels;

namespace Educacion.Desktop.ViewModels.Teacher;

public partial class TeacherListViewModel : ViewModelBase
{
    private readonly IDataService<Models.Teacher> _teacherService;

    [ObservableProperty]
    private ObservableCollection<Models.Teacher> _teachers = new();

    [ObservableProperty]
    private Models.Teacher? _selectedTeacher;

    public TeacherListViewModel(IDataService<Models.Teacher> teacherService)
    {
        _teacherService = teacherService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        var items = await _teacherService.GetAllAsync();
        Teachers = new ObservableCollection<Models.Teacher>(items);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedTeacher is null) return;
        await _teacherService.DeleteAsync(SelectedTeacher.Id);
        Teachers.Remove(SelectedTeacher);
    }

    [RelayCommand]
    private void Create()
    {
        WeakReferenceMessenger.Default.Send(new EditTeacherMessage(null));
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedTeacher != null)
            WeakReferenceMessenger.Default.Send(new EditTeacherMessage(SelectedTeacher));
    }
}
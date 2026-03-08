using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Educacion.Desktop.Models;
using Educacion.Desktop.Services;
using Educacion.Desktop.ViewModels;

namespace Educacion.Desktop.ViewModels.Teacher;

public partial class TeacherDetailViewModel : ViewModelBase
{
    private readonly IDataService<Models.Teacher> _teacherService;
    private readonly Models.Teacher _teacher;

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _specialization = string.Empty;

    public TeacherDetailViewModel(IDataService<Models.Teacher> teacherService, Models.Teacher? teacher = null)
    {
        _teacherService = teacherService;
        _teacher = teacher ?? new Models.Teacher();

        if (teacher != null)
        {
            FirstName = teacher.FirstName;
            LastName = teacher.LastName;
            Email = teacher.Email;
            Specialization = teacher.Specialization;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        _teacher.FirstName = FirstName;
        _teacher.LastName = LastName;
        _teacher.Email = Email;
        _teacher.Specialization = Specialization;

        if (_teacher.Id == 0) await _teacherService.CreateAsync(_teacher);
        else await _teacherService.UpdateAsync(_teacher);

        WeakReferenceMessenger.Default.Send(new NavigateBackMessage());
    }

    [RelayCommand]
    private void Cancel()
    {
        WeakReferenceMessenger.Default.Send(new NavigateBackMessage());
    }
}
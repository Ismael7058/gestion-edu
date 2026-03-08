using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Educacion.Desktop.Models;
using Educacion.Desktop.Services;
using Educacion.Desktop.ViewModels;

namespace Educacion.Desktop.ViewModels.Student;

public partial class StudentDetailViewModel : ViewModelBase
{
    private readonly IDataService<Models.Student> _studentService;
    private readonly Models.Student _student;

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private DateTimeOffset _dateOfBirth = DateTimeOffset.Now;

    public StudentDetailViewModel(IDataService<Models.Student> studentService, Models.Student? student = null)
    {
        _studentService = studentService;
        _student = student ?? new Models.Student();

        if (student != null)
        {
            FirstName = student.FirstName;
            LastName = student.LastName;
            Email = student.Email;
            DateOfBirth = new DateTimeOffset(student.DateOfBirth);
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        _student.FirstName = FirstName;
        _student.LastName = LastName;
        _student.Email = Email;
        _student.DateOfBirth = DateOfBirth.DateTime;

        if (_student.Id == 0) await _studentService.CreateAsync(_student);
        else await _studentService.UpdateAsync(_student);
        
        WeakReferenceMessenger.Default.Send(new NavigateBackMessage());
    }

    [RelayCommand]
    private void Cancel()
    {
        WeakReferenceMessenger.Default.Send(new NavigateBackMessage());
    }
}
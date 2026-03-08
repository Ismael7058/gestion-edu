using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Educacion.Desktop.Models;
using Educacion.Desktop.Services;
using Educacion.Desktop.ViewModels;

namespace Educacion.Desktop.ViewModels.Enrollment;

public partial class EnrollmentDetailViewModel : ViewModelBase
{
    private readonly IDataService<Models.Enrollment> _enrollmentService;
    private readonly IDataService<Models.Student> _studentService;
    private readonly IDataService<Models.Subject> _subjectService;
    private readonly Models.Enrollment _enrollment;

    [ObservableProperty]
    private DateTimeOffset _enrollmentDate = DateTimeOffset.Now;

    [ObservableProperty]
    private string? _grade;

    [ObservableProperty]
    private Models.Student? _selectedStudent;

    [ObservableProperty]
    private Models.Subject? _selectedSubject;

    [ObservableProperty]
    private ObservableCollection<Models.Student> _students = new();

    [ObservableProperty]
    private ObservableCollection<Models.Subject> _subjects = new();

    public EnrollmentDetailViewModel(
        IDataService<Models.Enrollment> enrollmentService,
        IDataService<Models.Student> studentService,
        IDataService<Models.Subject> subjectService,
        Models.Enrollment? enrollment = null)
    {
        _enrollmentService = enrollmentService;
        _studentService = studentService;
        _subjectService = subjectService;
        _enrollment = enrollment ?? new Models.Enrollment();

        if (enrollment != null)
        {
            EnrollmentDate = new DateTimeOffset(enrollment.EnrollmentDate);
            Grade = enrollment.Grade;
        }
    }

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        var students = await _studentService.GetAllAsync();
        Students = new ObservableCollection<Models.Student>(students);

        var subjects = await _subjectService.GetAllAsync();
        Subjects = new ObservableCollection<Models.Subject>(subjects);

        if (_enrollment.StudentId != 0)
            foreach (var s in Students) if (s.Id == _enrollment.StudentId) SelectedStudent = s;

        if (_enrollment.SubjectId != 0)
            foreach (var s in Subjects) if (s.Id == _enrollment.SubjectId) SelectedSubject = s;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        _enrollment.EnrollmentDate = EnrollmentDate.DateTime;
        _enrollment.Grade = Grade;
        _enrollment.StudentId = SelectedStudent?.Id ?? 0;
        _enrollment.SubjectId = SelectedSubject?.Id ?? 0;

        if (_enrollment.Id == 0) await _enrollmentService.CreateAsync(_enrollment);
        else await _enrollmentService.UpdateAsync(_enrollment);

        WeakReferenceMessenger.Default.Send(new NavigateBackMessage());
    }

    [RelayCommand]
    private void Cancel()
    {
        WeakReferenceMessenger.Default.Send(new NavigateBackMessage());
    }
}
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Educacion.Desktop.Models;
using Educacion.Desktop.Services;
using Educacion.Desktop.ViewModels;

namespace Educacion.Desktop.ViewModels.Subject;

public partial class SubjectDetailViewModel : ViewModelBase
{
    private readonly IDataService<Models.Subject> _subjectService;
    private readonly IDataService<Models.Teacher> _teacherService;
    private readonly Models.Subject _subject;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private int _credits;

    [ObservableProperty]
    private Models.Teacher? _selectedTeacher;

    [ObservableProperty]
    private ObservableCollection<Models.Teacher> _teachers = new();

    public SubjectDetailViewModel(IDataService<Models.Subject> subjectService, IDataService<Models.Teacher> teacherService, Models.Subject? subject = null)
    {
        _subjectService = subjectService;
        _teacherService = teacherService;
        _subject = subject ?? new Models.Subject();

        if (subject != null)
        {
            Name = subject.Name;
            Credits = subject.Credits;
            // Nota: SelectedTeacher se asignará después de cargar la lista
        }
    }

    [RelayCommand]
    public async Task LoadTeachersAsync()
    {
        var teachers = await _teacherService.GetAllAsync();
        Teachers = new ObservableCollection<Models.Teacher>(teachers);
        
        if (_subject.TeacherId.HasValue)
        {
            // Lógica simple para seleccionar el profesor actual
            foreach (var t in Teachers) if (t.Id == _subject.TeacherId) SelectedTeacher = t;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        _subject.Name = Name;
        _subject.Credits = Credits;
        _subject.TeacherId = SelectedTeacher?.Id;

        if (_subject.Id == 0) await _subjectService.CreateAsync(_subject);
        else await _subjectService.UpdateAsync(_subject);

        WeakReferenceMessenger.Default.Send(new NavigateBackMessage());
    }

    [RelayCommand]
    private void Cancel()
    {
        WeakReferenceMessenger.Default.Send(new NavigateBackMessage());
    }
}
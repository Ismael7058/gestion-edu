using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Educacion.Desktop.Models;
using Educacion.Desktop.Services;
using Educacion.Desktop.ViewModels;

namespace Educacion.Desktop.ViewModels.Subject;

public partial class SubjectListViewModel : ViewModelBase
{
    private readonly IDataService<Models.Subject> _subjectService;

    [ObservableProperty]
    private ObservableCollection<Models.Subject> _subjects = new();

    [ObservableProperty]
    private Models.Subject? _selectedSubject;

    public SubjectListViewModel(IDataService<Models.Subject> subjectService)
    {
        _subjectService = subjectService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        var items = await _subjectService.GetAllAsync();
        Subjects = new ObservableCollection<Models.Subject>(items);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedSubject is null) return;
        await _subjectService.DeleteAsync(SelectedSubject.Id);
        Subjects.Remove(SelectedSubject);
    }

    [RelayCommand]
    private void Create()
    {
        WeakReferenceMessenger.Default.Send(new EditSubjectMessage(null));
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedSubject != null)
            WeakReferenceMessenger.Default.Send(new EditSubjectMessage(SelectedSubject));
    }
}
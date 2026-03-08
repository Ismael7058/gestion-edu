using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Educacion.Desktop.Models;
using Educacion.Desktop.Services;
using Educacion.Desktop.ViewModels;

namespace Educacion.Desktop.ViewModels.Enrollment;

public partial class EnrollmentListViewModel : ViewModelBase
{
    private readonly IDataService<Models.Enrollment> _enrollmentService;

    [ObservableProperty]
    private ObservableCollection<Models.Enrollment> _enrollments = new();

    [ObservableProperty]
    private Models.Enrollment? _selectedEnrollment;

    public EnrollmentListViewModel(IDataService<Models.Enrollment> enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        var items = await _enrollmentService.GetAllAsync();
        Enrollments = new ObservableCollection<Models.Enrollment>(items);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedEnrollment is null) return;
        await _enrollmentService.DeleteAsync(SelectedEnrollment.Id);
        Enrollments.Remove(SelectedEnrollment);
    }

    [RelayCommand]
    private void Create()
    {
        WeakReferenceMessenger.Default.Send(new EditEnrollmentMessage(null));
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedEnrollment != null)
            WeakReferenceMessenger.Default.Send(new EditEnrollmentMessage(SelectedEnrollment));
    }
}
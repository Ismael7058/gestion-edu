using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Educacion.Desktop.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    [RelayCommand]
    private void CreateStudent()
    {
        // Enviar mensaje con 'null' indica que queremos crear uno nuevo
        WeakReferenceMessenger.Default.Send(new EditStudentMessage(null));
    }

    [RelayCommand]
    private void CreateTeacher()
    {
        WeakReferenceMessenger.Default.Send(new EditTeacherMessage(null));
    }

    [RelayCommand]
    private void CreateSubject()
    {
        WeakReferenceMessenger.Default.Send(new EditSubjectMessage(null));
    }

    [RelayCommand]
    private void CreateEnrollment()
    {
        WeakReferenceMessenger.Default.Send(new EditEnrollmentMessage(null));
    }
}

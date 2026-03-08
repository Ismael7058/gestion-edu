using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Educacion.Desktop.Models;
using Educacion.Desktop.Services;
using Educacion.Desktop.ViewModels.Enrollment;
using Educacion.Desktop.ViewModels.Student;
using Educacion.Desktop.ViewModels.Subject;
using Educacion.Desktop.ViewModels.Teacher;
using Microsoft.Extensions.DependencyInjection;

namespace Educacion.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentViewModel;
    
    private ViewModelBase? _previousViewModel;

    private readonly DashboardViewModel _dashboardViewModel;
    private readonly StudentListViewModel _studentListViewModel;
    private readonly TeacherListViewModel _teacherListViewModel;
    private readonly SubjectListViewModel _subjectListViewModel;
    private readonly EnrollmentListViewModel _enrollmentListViewModel;

    private bool _studentsLoaded;
    private bool _teachersLoaded;
    private bool _subjectsLoaded;
    private bool _enrollmentsLoaded;

    private readonly IServiceProvider _serviceProvider;

    // Nota: Para que esto funcione, necesitarás configurar la Inyección de Dependencias
    // para que proporcione instancias de estos ViewModels.
    public MainWindowViewModel(
        DashboardViewModel dashboardViewModel,
        StudentListViewModel studentListViewModel,
        TeacherListViewModel teacherListViewModel,
        SubjectListViewModel subjectListViewModel,
        EnrollmentListViewModel enrollmentListViewModel,
        IServiceProvider serviceProvider)
    {
        _dashboardViewModel = dashboardViewModel;
        _studentListViewModel = studentListViewModel;
        _teacherListViewModel = teacherListViewModel;
        _subjectListViewModel = subjectListViewModel;
        _enrollmentListViewModel = enrollmentListViewModel;
        _serviceProvider = serviceProvider;

        // Establecemos la vista inicial que se mostrará al arrancar la app
        _currentViewModel = _dashboardViewModel;

        // Registrar mensajes de navegación
        WeakReferenceMessenger.Default.Register<EditStudentMessage>(this, (r, m) => ((MainWindowViewModel)r).GoToDetail(m.Value));
        WeakReferenceMessenger.Default.Register<EditTeacherMessage>(this, (r, m) => ((MainWindowViewModel)r).GoToDetail(m.Value));
        WeakReferenceMessenger.Default.Register<EditSubjectMessage>(this, (r, m) => ((MainWindowViewModel)r).GoToDetail(m.Value));
        WeakReferenceMessenger.Default.Register<EditEnrollmentMessage>(this, (r, m) => ((MainWindowViewModel)r).GoToDetail(m.Value));
        WeakReferenceMessenger.Default.Register<NavigateBackMessage>(this, (r, m) => ((MainWindowViewModel)r).GoBack());
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        CurrentViewModel = _dashboardViewModel;
    }

    [RelayCommand]
    private void NavigateToStudents()
    {
        CurrentViewModel = _studentListViewModel;
        if (_studentsLoaded) return;
        _studentListViewModel.LoadCommand.Execute(null);
        _studentsLoaded = true;
    }

    [RelayCommand]
    private void NavigateToTeachers()
    {
        CurrentViewModel = _teacherListViewModel;
        if (_teachersLoaded) return;
        _teacherListViewModel.LoadCommand.Execute(null);
        _teachersLoaded = true;
    }

    [RelayCommand]
    private void NavigateToSubjects()
    {
        CurrentViewModel = _subjectListViewModel;
        if (_subjectsLoaded) return;
        _subjectListViewModel.LoadCommand.Execute(null);
        _subjectsLoaded = true;
    }

    [RelayCommand]
    private void NavigateToEnrollments()
    {
        CurrentViewModel = _enrollmentListViewModel;
        if (_enrollmentsLoaded) return;
        _enrollmentListViewModel.LoadCommand.Execute(null);
        _enrollmentsLoaded = true;
    }

    // Métodos auxiliares para crear y navegar a los detalles
    private void GoToDetail(Models.Student? student)
    {
        _previousViewModel = CurrentViewModel;
        var service = _serviceProvider.GetRequiredService<IDataService<Models.Student>>();
        CurrentViewModel = new StudentDetailViewModel(service, student);
    }

    private void GoToDetail(Models.Teacher? teacher)
    {
        _previousViewModel = CurrentViewModel;
        var service = _serviceProvider.GetRequiredService<IDataService<Models.Teacher>>();
        CurrentViewModel = new TeacherDetailViewModel(service, teacher);
    }

    private void GoToDetail(Models.Subject? subject)
    {
        _previousViewModel = CurrentViewModel;
        var subjectService = _serviceProvider.GetRequiredService<IDataService<Models.Subject>>();
        var teacherService = _serviceProvider.GetRequiredService<IDataService<Models.Teacher>>();
        var vm = new SubjectDetailViewModel(subjectService, teacherService, subject);
        vm.LoadTeachersCommand.Execute(null); // Cargar datos necesarios
        CurrentViewModel = vm;
    }

    private void GoToDetail(Models.Enrollment? enrollment)
    {
        _previousViewModel = CurrentViewModel;
        var enrollmentService = _serviceProvider.GetRequiredService<IDataService<Models.Enrollment>>();
        var studentService = _serviceProvider.GetRequiredService<IDataService<Models.Student>>();
        var subjectService = _serviceProvider.GetRequiredService<IDataService<Models.Subject>>();
        var vm = new EnrollmentDetailViewModel(enrollmentService, studentService, subjectService, enrollment);
        vm.LoadDataCommand.Execute(null); // Cargar datos necesarios
        CurrentViewModel = vm;
    }

    private void GoBack()
    {
        if (_previousViewModel != null)
        {
            CurrentViewModel = _previousViewModel;
            // Recargar la lista para mostrar los cambios
            if (CurrentViewModel is StudentListViewModel s) s.LoadCommand.Execute(null);
            else if (CurrentViewModel is TeacherListViewModel t) t.LoadCommand.Execute(null);
            else if (CurrentViewModel is SubjectListViewModel sub) sub.LoadCommand.Execute(null);
            else if (CurrentViewModel is EnrollmentListViewModel e) e.LoadCommand.Execute(null);
        }
    }
}

using CommunityToolkit.Mvvm.Messaging.Messages;
using Educacion.Desktop.Models;

namespace Educacion.Desktop.ViewModels;

public class EditStudentMessage(Models.Student? value) : ValueChangedMessage<Models.Student?>(value);
public class EditTeacherMessage(Models.Teacher? value) : ValueChangedMessage<Models.Teacher?>(value);
public class EditSubjectMessage(Models.Subject? value) : ValueChangedMessage<Models.Subject?>(value);
public class EditEnrollmentMessage(Models.Enrollment? value) : ValueChangedMessage<Models.Enrollment?>(value);

public class NavigateBackMessage();
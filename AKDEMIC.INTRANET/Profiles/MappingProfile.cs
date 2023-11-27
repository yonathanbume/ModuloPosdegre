using AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation;
using AKDEMIC.INTRANET.Areas.Admin.Models.AnnouncementViewModels;
using AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels;
using AKDEMIC.INTRANET.Areas.Admin.Models.CurriculumViewModels;
using AKDEMIC.INTRANET.Areas.Admin.Models.EventViewModels;
using AKDEMIC.INTRANET.Areas.Admin.Models.Format;
using AKDEMIC.INTRANET.Areas.Admin.Models.ForumsViewModels;
using AKDEMIC.INTRANET.Areas.Admin.Models.Report_teacherViewModels;
using AKDEMIC.INTRANET.ViewModels.InstitutionalAlertViewModels;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicYearCourse;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Section;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Geo.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EventRole;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Forum;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.InstitutionalAlert;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.RolAnnouncement;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.UserEvent;
using AutoMapper;

namespace AKDEMIC.INTRANET.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentInformationTemplate, StudentViewModel>();
            CreateMap<StudentGeneralDataTemplate, GeneralDataViewModel>();
            CreateMap<AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection.CourseTemplate, CourseViewModel>();
            CreateMap<AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection.CourseEvaluationTemplate, CourseEvaluationViewModel>();
            CreateMap<StudyRecordTemplate, StudyRecordViewModel>();
            CreateMap<MeritChartDetailTemplate, MeritChartDetailViewModel>();
            CreateMap<MeritChartTemplate, MeritChartViewModel>();
            CreateMap<UpperFifthTemplate, UpperFifthViewModel>();
            CreateMap<UpperFifthDetailsTemplate, UpperFifthDetailsViewModel>();
            CreateMap<TeacherSectionTemplate, TeacherViewModel>();
            CreateMap<StudentEnrolledTemplate, StudentEnrolledViewModel>();
            CreateMap<ATSTemplate, ATSViewModel>();
            CreateMap<ForumViewModel, ForumTemplate>();
            CreateMap<CurriculumTemplate, CurriculumViewModel>();
            CreateMap<PreRequisiteTemplate, PreRequisiteViewModel>();
            CreateMap<HeadBoardCertificateTemplate, HeadBoardCertificateViewModel>();
            CreateMap<CertificateTemplate, AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels.CertificateViewModel>();
            CreateMap<EventInscriptionTemplate, AKDEMIC.INTRANET.ViewModels.EventViewModels.EventViewModel>();
            CreateMap<InstitutionalAlertTemplate, InstitutionalAlertViewModel>();
            CreateMap<InstitutionalAlertDetailTemplate, InstitutionalAlertDetailViewModel>();
            CreateMap<AnnouncementTemplate, AnnouncementViewModel>();
        }
    }
}

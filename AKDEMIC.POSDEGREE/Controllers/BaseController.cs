using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.POSDEGREE.Helpers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AKDEMIC.POSDEGREE.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IConfigurationService _configurationService;
        protected readonly ICloudStorageService _cloudStorageService;
        protected readonly IConfiguration _configuration;
        protected readonly ICareerService _careerService;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<ApplicationRole> _roleManager;

        protected BaseController() { }

        protected BaseController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        protected BaseController(ICareerService careerService)
        {
            _careerService = careerService;
        }
        protected BaseController(ICareerService careerService, IConfigurationService configurationService, UserManager<ApplicationUser> userManager)
        {
            _careerService = careerService;
            _configurationService = configurationService;
            _userManager = userManager;
        }
        protected BaseController(ICareerService careerService, IConfigurationService configurationService)
        {
            _careerService = careerService;
            _configurationService = configurationService;
        }
        protected BaseController(ICareerService careerService, UserManager<ApplicationUser> userManager)
        {
            _careerService = careerService;
            _userManager = userManager;
        }

        protected BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected BaseController(UserManager<ApplicationUser> userManager, ICloudStorageService cloudStorageService)
        {
            _userManager = userManager;
            _cloudStorageService = cloudStorageService;
        }

        protected BaseController(UserManager<ApplicationUser> userManager, IConfigurationService configurationService)
        {
            _userManager = userManager;
            _configurationService = configurationService;
        }

        protected BaseController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region General

        protected virtual bool DateTimeConflict(DateTime startA, DateTime endA, DateTime startB, DateTime endB) => startA < endB && startB < endA;
        protected virtual bool InClassesPeriod(Term term) => term.Status.Equals(ConstantHelpers.TERM_STATES.ACTIVE) && DateTime.Now >= term.ClassStartDate && DateTime.Now <= term.ClassEndDate;
        protected virtual bool InEnrollmentPeriod(Term term) => term.Status.Equals(ConstantHelpers.TERM_STATES.ACTIVE) && DateTime.Now >= term.EnrollmentStartDate && DateTime.Now <= term.EnrollmentEndDate;
        protected virtual bool InPreClassesPeriod(Term term) => term.Status.Equals(ConstantHelpers.TERM_STATES.INACTIVE) || (term.Status.Equals(ConstantHelpers.TERM_STATES.ACTIVE) && term.ClassStartDate >= DateTime.Now && term.StartDate <= DateTime.Now);
        protected virtual bool InPreEnrollmentPeriod(Term term) => term.Status.Equals(ConstantHelpers.TERM_STATES.INACTIVE) || (term.Status.Equals(ConstantHelpers.TERM_STATES.ACTIVE) && term.EnrollmentStartDate >= DateTime.Now && term.StartDate <= DateTime.Now);
        //protected virtual async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync(User.Identity.Name);

        protected async Task<Tuple<List<Career>, List<Faculty>>> GetCareersAndFacultiesByAcademicCoordinator()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return await _careerService.GetCareerFacultiesIdByCoordinator(user.Id);
        }

        #endregion
        //protected async Task<string> GetConfigurationValue(string key)
        //{
        //    var configuration = await _context.Configurations.FirstOrDefaultAsync(x => x.Key == key);

        //    if (configuration == null)
        //    {
        //        configuration = new Configuration
        //        {
        //            Key = key,
        //            Value = CORE.Helpers.ConstantHelpers.Configuration.General.DEFAULT_VALUES[key]
        //        };

        //        await _context.Configurations.AddAsync(configuration);
        //        await _context.SaveChangesAsync();
        //    }

        //    return configuration.Value;
        //}
        protected async Task<string> GetConfigurationValue(string key)
        {
            var configuration = await _configurationService.FirstOrDefaultByKey(key);

            if (configuration == null)
            {
                var value = CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.General.DEFAULT_VALUES[key];

                configuration = new Configuration
                {
                    Key = key,
                    Value = value
                };

                await _configurationService.Insert(configuration);

            }
            return configuration.Value;
        }

        #region Toastr

        protected void SuccessToastMessage(string message = ConstantHelpers.MESSAGES.SUCCESS.MESSAGE, string title = ConstantHelpers.MESSAGES.SUCCESS.TITLE)
        {
            TempData["Toastr"] = TempData["Toastr"] + "<script>toastr.options.closeButton = true; toastr.options.progressBar = true; toastr.success('" + message + "', '" + title + "');</script>";
        }

        protected void InfoToastMessage(string message = ConstantHelpers.MESSAGES.INFO.MESSAGE, string title = ConstantHelpers.MESSAGES.INFO.TITLE)
        {
            TempData["Toastr"] = TempData["Toastr"] + "<script>toastr.options.closeButton = true; toastr.options.progressBar = true; toastr.info('" + message + "', '" + title + "');</script>";
        }

        protected void WarningToastMessage(string message = ConstantHelpers.MESSAGES.WARNING.MESSAGE, string title = ConstantHelpers.MESSAGES.WARNING.TITLE)
        {
            TempData["Toastr"] = TempData["Toastr"] + "<script>toastr.options.closeButton = true; toastr.options.progressBar = true; toastr.warning('" + message + "', '" + title + "');</script>";
        }

        protected void ErrorToastMessage(string message = ConstantHelpers.MESSAGES.ERROR.MESSAGE, string title = ConstantHelpers.MESSAGES.ERROR.TITLE)
        {
            TempData["Toastr"] = TempData["Toastr"] + "<script>toastr.options.closeButton = true; toastr.options.progressBar = true; toastr.error('" + message + "', '" + title + "');</script>";
        }

        #endregion        
    }
}

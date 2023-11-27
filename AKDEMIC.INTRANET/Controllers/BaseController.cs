using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Hubs;
using AKDEMIC.INTRANET.ViewModels;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AKDEMIC.INTRANET.Filters;

namespace AKDEMIC.INTRANET.Controllers
{
    [UserAuthorizationAttribute]
    [StudentAuthorizationAttribute]
    public class BaseController : Controller
    {
        protected readonly AkdemicContext _context;
        protected readonly ICloudStorageService _cloudStorageService;
        protected readonly IConfiguration _configuration;
        protected readonly IHubContext<AkdemicHub> _hubContext;
        protected readonly ITermService _termService;
        protected readonly IUserService _userService;
        protected readonly IRoleService _roleService;
        protected readonly IDataTablesService _dataTablesService;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<ApplicationRole> _roleManager;

        protected BaseController() { }

        protected BaseController(AkdemicContext context)
        {
            _context = context;
        }
        protected BaseController(AkdemicContext context, IDataTablesService dataTablesService)
        {
            _context = context;
            _dataTablesService = dataTablesService;
        }

        protected BaseController(IDataTablesService dataTablesService)
        {
            _dataTablesService = dataTablesService;
        }

        protected BaseController(UserManager<ApplicationUser> userManager, IDataTablesService dataTablesService)
        {
            _dataTablesService = dataTablesService;
            _userManager = userManager;
        }

        protected BaseController(AkdemicContext context,UserManager<ApplicationUser> userManager, IDataTablesService dataTablesService)
        {
            _context = context;
            _dataTablesService = dataTablesService;
            _userManager = userManager;
        }

        protected BaseController(ITermService termService)
        {
            _termService = termService;
        }

        protected BaseController(ITermService termService, IDataTablesService dataTablesService)
        {
            _termService = termService;
            _dataTablesService = dataTablesService;
        }
        protected BaseController(IUserService userService, IDataTablesService dataTablesService)
        {
            _userService = userService;
            _dataTablesService = dataTablesService;
        }
        protected BaseController(IUserService userService)
        {
            _userService = userService;
        }
        protected BaseController(AkdemicContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        protected BaseController(AkdemicContext context,IUserService userService, ITermService termService)
        {
            _context = context;
            _userService = userService;
            _termService = termService;
        }
        protected BaseController(IUserService userService, ITermService termService)
        {
            _userService = userService;
            _termService = termService;
        }

        protected BaseController(IUserService userService, ICloudStorageService cloudStorageService)
        {
            _userService = userService;
            _cloudStorageService = cloudStorageService;
        }

        protected BaseController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        protected BaseController(IUserService userService, IRoleService roleService, ICloudStorageService cloudStorageService)
        {
            _userService = userService;
            _roleService = roleService;
            _cloudStorageService = cloudStorageService;
        }

        protected BaseController(UserManager<ApplicationUser> userManager, IUserService userService, IDataTablesService dataTablesService)
        {
            _userManager = userManager;
            _userService = userService;
            _dataTablesService = dataTablesService;
        }

        protected BaseController(UserManager<ApplicationUser> userManager, IUserService userService, IRoleService roleService, IDataTablesService dataTablesService)
        {
            _userManager = userManager;
            _userService = userService;
            _roleService = roleService;
            _dataTablesService = dataTablesService;
        }

        protected BaseController(AkdemicContext context, IUserService userService, IConfiguration configuration)
        {
            _context = context;
            _userService = userService;
            _configuration = configuration;
        }

        protected BaseController(AkdemicContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        protected BaseController(AkdemicContext context, IConfiguration configuration, IHubContext<AkdemicHub> hubContext)
        {
            _configuration = configuration;
            _context = context;
            _hubContext = hubContext;
        }

        protected BaseController(IUserService userService, IHubContext<AkdemicHub> hubContext)
        {
            _userService = userService;
            _hubContext = hubContext;
        }

        protected BaseController(IConfiguration configuration, IHubContext<AkdemicHub> hubContext , IDataTablesService dataTablesService, IUserService userService)
        {
            _configuration = configuration;
            _hubContext = hubContext;
            _dataTablesService = dataTablesService;
            _userService = userService;
        }

        protected BaseController(AkdemicContext context, ITermService termService)
        {
            _context = context;
            _termService = termService;
        }

        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        protected BaseController(AkdemicContext context, IUserService userService, ICloudStorageService cloudStorageService)
        {
            _context = context;
            _userService = userService;
            _cloudStorageService = cloudStorageService;
        }

        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager, ICloudStorageService cloudStorageService)
        {
            _context = context;
            _userManager = userManager;
            _cloudStorageService = cloudStorageService;
        }

        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager, IUserService userService, ITermService termService, IDataTablesService dataTablesService)
        {
            _context = context;
            _userManager = userManager;
            _userService = userService;
            _termService = termService;
            _dataTablesService = dataTablesService;
        }

        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, IHubContext<AkdemicHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _hubContext = hubContext;
        }
        protected BaseController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IHubContext<AkdemicHub> hubContext, IUserService userService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _hubContext = hubContext;
            _userService = userService;
        }
        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, IHubContext<AkdemicHub> hubContext, IUserService userService)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _hubContext = hubContext;
            _userService = userService;
        }
        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager, IHubContext<AkdemicHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }
        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager, IHubContext<AkdemicHub> hubContext, ITermService termService, IUserService userService)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
            _termService = termService;
            _userService = userService;
        }
        protected BaseController(UserManager<ApplicationUser> userManager, IHubContext<AkdemicHub> hubContext)
        {
            _userManager = userManager;
            _hubContext = hubContext;
        }

        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager, ITermService termService)
        {
            _context = context;
            _userManager = userManager;
            _termService = termService;
        }
        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager, ITermService termService, IDataTablesService dataTablesService)
        {
            _context = context;
            _userManager = userManager;
            _termService = termService;
            _dataTablesService = dataTablesService;
        }

        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        protected BaseController(AkdemicContext context, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _context = context;
            _userManager = userManager;
            _userService = userService;
        }
        protected BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected BaseController(UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        protected BaseController(UserManager<ApplicationUser> userManager, ITermService termService)
        {
            _userManager = userManager;
            _termService = termService;
        }

        protected BaseController(ITermService termService, IUserService userService, IDataTablesService dataTablesService)
        {
            _termService = termService;
            _userService = userService;
            _dataTablesService = dataTablesService;
        }

        protected BaseController(ITermService termService, IUserService userService, IRoleService roleService)
        {
            _termService = termService;
            _userService = userService;
            _roleService = roleService;
        }
        protected BaseController(UserManager<ApplicationUser> userManager, ICloudStorageService cloudStorageService)
        {
            _userManager = userManager;
            _cloudStorageService = cloudStorageService;
        }
        protected BaseController(UserManager<ApplicationUser> userManager, IUserService userService, IDataTablesService dataTablesService, ICloudStorageService cloudStorageService)
        {
            _userManager = userManager;
            _userService = userService;
            _dataTablesService = dataTablesService;
            _cloudStorageService = cloudStorageService;
        }
        #region General

        protected virtual bool DateTimeConflict(DateTime startA, DateTime endA, DateTime startB, DateTime endB) => startA < endB && startB < endA;
        protected virtual async Task<Term> GetActiveTerm() => await _termService.GetActiveTerm();
        protected virtual async Task<ApplicationUser> GetCurrentUserAsync() => await _userService.GetByUserName(HttpContext.User.Identity.Name);
        protected virtual string GetUserId() => _userService.GetUserIdByClaim(User);
        protected virtual bool InClassesPeriod(Term term) => term.Status.Equals(ConstantHelpers.TERM_STATES.ACTIVE) && DateTime.Now >= term.ClassStartDate && DateTime.Now <= term.ClassEndDate;
        protected virtual bool InEnrollmentPeriod(Term term) => term.Status.Equals(ConstantHelpers.TERM_STATES.ACTIVE) && DateTime.Now >= term.EnrollmentStartDate && DateTime.Now <= term.EnrollmentEndDate;
        protected virtual bool InPreClassesPeriod(Term term) => term.Status.Equals(ConstantHelpers.TERM_STATES.INACTIVE) || (term.Status.Equals(ConstantHelpers.TERM_STATES.ACTIVE) && term.ClassStartDate >= DateTime.Now && term.StartDate <= DateTime.Now);
        protected virtual bool InPreEnrollmentPeriod(Term term) => term.Status.Equals(ConstantHelpers.TERM_STATES.INACTIVE) || (term.Status.Equals(ConstantHelpers.TERM_STATES.ACTIVE) && term.EnrollmentStartDate >= DateTime.Now && term.StartDate <= DateTime.Now);
        protected virtual int SaveChanges() => _context.SaveChanges();
        protected virtual async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        protected virtual DataTablesStructs.SentParameters GetSentParameters() => _dataTablesService.GetSentParameters();

        protected virtual async Task<bool> InClassesPeriod(Guid termId)
        {
            var term = await _context.Terms.FindAsync(termId);
            return InClassesPeriod(term);
        }

        protected virtual async Task<bool> InEnrollmentPeriod(Guid termId)
        {
            var term = await _context.Terms.FindAsync(termId);
            return InEnrollmentPeriod(term);
        }

        protected virtual async Task<bool> InPreClassesPeriod(Guid termId)
        {
            var term = await _context.Terms.FindAsync(termId);
            return InPreClassesPeriod(term);
        }

        protected virtual async Task<bool> InPreEnrollmentPeriod(Guid termId)
        {
            var term = await _context.Terms.FindAsync(termId);
            return InPreEnrollmentPeriod(term);
        }

        protected async Task<string> GetConfigurationValue(string key)
        {
            var configuration = await _context.Configurations.FirstOrDefaultAsync(x => x.Key == key);

            if (configuration == null)
            {
                var value = CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.General.DEFAULT_VALUES[key];

                configuration = new Configuration
                {
                    Key = key,
                    Value = value
                };

                await _context.Configurations.AddAsync(configuration);
                await _context.SaveChangesAsync();
            }

            return configuration.Value;
        }

        #endregion

        /* Old stuff that must go at some point vvvvvvv */

        private NotificationViewData BuildNotification(string message, string messageType, string title = "", string messageIcon = "", string template = "")
        {
            return new NotificationViewData() { Message = message, Icon = messageIcon, Title = title, Type = messageType, Template = template };
        }

        protected virtual void AddSuccessNotification(string message)
        {
            var notif = BuildNotification(message, NotificationViewData.NotificationType.SUCCESS, NotificationViewData.DefaultTitle.SUCCESS, NotificationViewData.DefaultIcon.SUCCESS);
            TempData["Notification"] = JsonConvert.SerializeObject(notif);
        }

        protected virtual void AddInfoNotification(string message)
        {
            TempData["Notification"] = BuildNotification(message, NotificationViewData.NotificationType.INFO, NotificationViewData.DefaultTitle.INFO, NotificationViewData.DefaultIcon.INFO);
        }

        protected virtual void AddErrorNotification(string message)
        {
            TempData["Notification"] = BuildNotification(message, NotificationViewData.NotificationType.ERROR, NotificationViewData.DefaultTitle.ERROR, NotificationViewData.DefaultIcon.ERROR);
        }

        protected virtual void AddWarningNotification(string message)
        {
            TempData["Notification"] = BuildNotification(message, NotificationViewData.NotificationType.WARNING, NotificationViewData.DefaultTitle.WARNING, NotificationViewData.DefaultIcon.WARNING);
        }

        protected virtual void AddCustomNotification(string message, string title = "", string messageIcon = "", string messageType = "", string template = "")
        {
            TempData["Notification"] = BuildNotification(message, messageType, title, messageIcon, template);
        }

        #region Notifications

        protected virtual async Task SendBulkNotificationToUsersAsync(IEnumerable<string> users, string text, string url = null, string stateText = null, int backgroundStateClass = 0, bool sound = false)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var notificationDataTable = new DataTable();

                    notificationDataTable.Columns.Add("Id", typeof(Guid));
                    notificationDataTable.Columns.Add("BackgroundStateClass", typeof(int));
                    notificationDataTable.Columns.Add("SendDate", typeof(DateTime));
                    notificationDataTable.Columns.Add("State", typeof(string));
                    notificationDataTable.Columns.Add("Text", typeof(string));
                    notificationDataTable.Columns.Add("Url", typeof(string));

                    var notificationId = Guid.NewGuid();
                    var notificationRow = notificationDataTable.NewRow();
                    notificationRow[0] = notificationId;
                    notificationRow[1] = 7;
                    notificationRow[2] = DateTime.UtcNow;
                    notificationRow[3] = "Urgente";
                    notificationRow[4] = text;
                    notificationRow[5] = url;

                    notificationDataTable.Rows.Add(notificationRow);

                    var connectionString = _configuration.GetConnectionString(ConstantHelpers.DATABASES.CONNECTION_STRINGS.VALUES[new Tuple<int, int>(ConstantHelpers.GENERAL.DATABASES.DATABASE, ConstantHelpers.GENERAL.DATABASES.CONNECTION_STRINGS.CONNECTION_STRING)]);

                    using (var sqlBulkCopy = new SqlBulkCopy(connectionString))
                    {
                        sqlBulkCopy.DestinationTableName = ConstantHelpers.HUBS.AKDEMIC.DATABASE.TABLE.NOTIFICATION;
                        await sqlBulkCopy.WriteToServerAsync(notificationDataTable);
                    }

                    var userNotificationDataTable = new DataTable();
                    userNotificationDataTable.Columns.Add("Id", typeof(Guid));
                    userNotificationDataTable.Columns.Add("NotificationId", typeof(Guid));
                    userNotificationDataTable.Columns.Add("UserId", typeof(string));
                    userNotificationDataTable.Columns.Add("IsRead", typeof(bool));

                    foreach (var user in users)
                    {
                        var userNotificationRow = userNotificationDataTable.NewRow();
                        userNotificationRow[0] = Guid.NewGuid();
                        userNotificationRow[1] = user;
                        userNotificationRow[2] = notificationId;
                        userNotificationRow[3] = false;

                        userNotificationDataTable.Rows.Add(userNotificationRow);
                        await _hubContext.Clients.User(user).SendAsync(ConstantHelpers.HUBS.AKDEMIC.CLIENT_PROXY.METHOD, "", DateTime.UtcNow.ElapsedTime(), false, userNotificationRow[0], ConstantHelpers.NOTIFICATIONS.COLORS.LABELS[0], url ?? "", sound);
                    }

                    using (var sqlBulkCopy = new SqlBulkCopy(connectionString))
                    {
                        sqlBulkCopy.DestinationTableName = ConstantHelpers.HUBS.AKDEMIC.DATABASE.TABLE.USER_NOTIFICATION;
                        await sqlBulkCopy.WriteToServerAsync(userNotificationDataTable);
                    }

                    transactionScope.Complete();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        protected virtual async Task SendNotificationToUserAsync(string user, string text, string url = null, string stateText = null, int backgroundStateClass = 0, bool sound = false)
        {
            try
            {
                var userId = await _context.Users
                    .Where(x => x.Id == user || x.UserName == user || x.NormalizedUserName == user)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                var notification = new Notification
                {
                    BackgroundStateClass = backgroundStateClass,
                    State = stateText,
                    Text = text,
                    Url = url
                };

                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                var userNotification = new UserNotification
                {
                    NotificationId = notification.Id,
                    UserId = userId
                };

                await _context.UserNotifications.AddAsync(userNotification);
                await _hubContext.Clients.User(userId).SendAsync(ConstantHelpers.HUBS.AKDEMIC.CLIENT_PROXY.METHOD, text, stateText ?? "", DateTime.UtcNow.ElapsedTime(), false, userNotification.Id, ConstantHelpers.NOTIFICATIONS.COLORS.LABELS[backgroundStateClass], url ?? "", sound);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return;
            }
        }

        protected virtual async Task SendNotificationToUsersAsync(IEnumerable<string> users, string text, string url = null, string stateText = null, int backgroundStateClass = 0, bool sound = false)
        {
            try
            {
                if (users == null)
                {
                    return;
                }

                var usersIds = await _context.Users
                    .Where(x => users.Contains(x.Id) || users.Contains(x.UserName))
                    .Select(x => x.Id)
                    .ToListAsync();

                var notification = new Notification
                {
                    BackgroundStateClass = backgroundStateClass,
                    State = stateText,
                    Text = text,
                    Url = url
                };

                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                foreach (var userId in usersIds)
                {
                    try
                    {
                        var userNotification = new UserNotification
                        {
                            NotificationId = notification.Id,
                            UserId = userId
                        };

                        await _context.UserNotifications.AddAsync(userNotification);
                        await _hubContext.Clients.Users(userId).SendAsync(ConstantHelpers.HUBS.AKDEMIC.CLIENT_PROXY.METHOD, text, stateText ?? "", DateTime.UtcNow.ElapsedTime(), false, userNotification.Id, ConstantHelpers.NOTIFICATIONS.COLORS.LABELS[backgroundStateClass], url ?? "", sound);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return;
            }
        }

        protected virtual async Task SendNotificationToRole(string role, string text, string url = null, string stateText = null, int backgroundStateClass = 0, bool sound = false)
        {
            try
            {
                var users = await _context.UserRoles
                    .Where(x => x.RoleId == role || x.Role.Name == role || x.Role.NormalizedName == role)
                    .Select(x => x.UserId)
                    .ToListAsync();

                if (users.LongCount() < ConstantHelpers.ENTITY_FRAMEWORK.RECORD_LIMIT)
                {
                    await SendNotificationToUsersAsync(users, text, url, stateText, backgroundStateClass, sound);
                }
                else
                {
                    await SendBulkNotificationToUsersAsync(users, text, url, stateText, backgroundStateClass, sound);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected virtual async Task SendNotificationToRoles(IEnumerable<string> roles, string text, string url = null, string stateText = null, int backgroundStateClass = 0, bool sound = false)
        {
            try
            {
                var users = await _context.UserRoles
                    .Where(x => roles.Contains(x.RoleId) || roles.Contains(x.Role.Name) || roles.Contains(x.Role.NormalizedName))
                    .Select(x => x.UserId)
                    .ToListAsync();

                if (users.LongCount() < ConstantHelpers.ENTITY_FRAMEWORK.RECORD_LIMIT)
                {
                    await SendNotificationToUsersAsync(users, text, url, stateText, backgroundStateClass, sound);
                }
                else
                {
                    await SendBulkNotificationToUsersAsync(users, text, url, stateText, backgroundStateClass, sound);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected virtual async Task SendNotificationToStudentSections(Guid sectionId, string text, string url = null, string stateText = null, int backgroundStateClass = 0, bool sound = false)
        {
            try
            {
                var users = await _context.StudentSections
                    .Where(x => x.Id == sectionId)
                    .Select(x => x.Student.UserId)
                    .ToListAsync();

                if (users.LongCount() < ConstantHelpers.ENTITY_FRAMEWORK.RECORD_LIMIT)
                {
                    await SendNotificationToUsersAsync(users, text, url, stateText, backgroundStateClass, sound);
                }
                else
                {
                    await SendBulkNotificationToUsersAsync(users, text, url, stateText, backgroundStateClass, sound);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected virtual async Task SendNotificationToUser(string user, string text, string url = null, string stateText = null, int backgroundStateClass = 0, bool sound = false)
        {
            try
            {
                await SendNotificationToUserAsync(user, text, url, stateText, backgroundStateClass, sound);
            }
            catch (Exception)
            {
                return; 
            }
        }

        protected virtual async Task SendNotificationToUserDependencies(Guid dependencyId, string text, string url = null, string stateText = null, int backgroundStateClass = 0, bool sound = false)
        {
            try
            {
                var users = await _context.UserDependencies
                    .Where(x => x.Id == dependencyId)
                    .Select(x => x.UserId)
                    .ToListAsync();

                if (users.Count < ConstantHelpers.ENTITY_FRAMEWORK.RECORD_LIMIT)
                {
                    await SendNotificationToUsersAsync(users, text, url, stateText, backgroundStateClass, sound);
                }
                else
                {
                    await SendBulkNotificationToUsersAsync(users, text, url, stateText, backgroundStateClass, sound);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected virtual async Task SendNotificationToUsers(IEnumerable<string> users, string text, string url = null, string stateText = null, int backgroundStateClass = 0, bool sound = false)
        {
            try
            {
                if (users.LongCount() < ConstantHelpers.ENTITY_FRAMEWORK.RECORD_LIMIT)
                {
                    await SendNotificationToUsersAsync(users, text, url, stateText, backgroundStateClass, sound);
                }
                else
                {
                    await SendBulkNotificationToUsersAsync(users, text, url, stateText, backgroundStateClass, sound);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion

        #region Pagination Methods

        protected virtual int GetDataTableCurrentNumber()
        {
            return Convert.ToInt32(Request.Query[CORE.Helpers.ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.PAGING_FIRST_RECORD]);
        }

        protected virtual string GetDataTableSortField()
        {
            return Request.Query.ContainsKey(CORE.Helpers.ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.ORDER_COLUMN)
                ? Request.Query[CORE.Helpers.ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.ORDER_COLUMN].ToString()
                : null;
        }

        protected virtual object GetDataTablePaginationObject<T>(int filterRecords, List<T> pagedList)
        {
            return new
            {
                draw = ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.DRAW_COUNTER,
                recordsTotal = filterRecords,
                recordsFiltered = filterRecords,
                data = pagedList
            };
        }

        protected virtual string GetDataTableSortOrder()
        {
            return Request.Query.ContainsKey(CORE.Helpers.ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.ORDER_DIRECTION)
                ? Request.Query[CORE.Helpers.ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.ORDER_DIRECTION].ToString().ToLower()
                : CORE.Helpers.ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION;
        }

        protected virtual int GetDataTableRecordsPerPage()
        {
            return Convert.ToInt32(Request.Query[CORE.Helpers.ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.RECORDS_PER_DRAW]);
        }

        protected PaginationParameter GetDataTablePaginationParameter()
        {
            return new PaginationParameter
            {
                SortOrder = GetDataTableSortOrder(),
                SortField = GetDataTableSortField(),
                CurrentNumber = GetDataTableCurrentNumber(),
                RecordsPerPage = GetDataTableRecordsPerPage()
            };
        }

        #endregion

        #region Toastr

        protected virtual void ErrorToastMessage(string message = ConstantHelpers.MESSAGES.ERROR.MESSAGE, string title = ConstantHelpers.MESSAGES.ERROR.TITLE)
        {
            TempData["Toastr"] = TempData["Toastr"] + "<script>toastr.options.closeButton = true; toastr.options.progressBar = true; toastr.error('" + message + "', '" + title + "');</script>";
        }

        protected virtual void InfoToastMessage(string message = ConstantHelpers.MESSAGES.INFO.MESSAGE, string title = ConstantHelpers.MESSAGES.INFO.TITLE)
        {
            TempData["Toastr"] = TempData["Toastr"] + "<script>toastr.options.closeButton = true; toastr.options.progressBar = true; toastr.info('" + message + "', '" + title + "');</script>";
        }

        protected virtual void SuccessToastMessage(string message = ConstantHelpers.MESSAGES.SUCCESS.MESSAGE, string title = ConstantHelpers.MESSAGES.SUCCESS.TITLE)
        {
            TempData["Toastr"] = TempData["Toastr"] + "<script>toastr.options.closeButton = true; toastr.options.progressBar = true; toastr.success('" + message + "', '" + title + "');</script>";
        }

        protected virtual void WarningToastMessage(string message = ConstantHelpers.MESSAGES.WARNING.MESSAGE, string title = ConstantHelpers.MESSAGES.WARNING.TITLE)
        {
            TempData["Toastr"] = TempData["Toastr"] + "<script>toastr.options.closeButton = true; toastr.options.progressBar = true; toastr.warning('" + message + "', '" + title + "');</script>";
        }

        #endregion
    }
}

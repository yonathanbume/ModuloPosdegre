using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Reservations;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Reservation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Reservation.Implementations
{
    public class EnvironmentReservationRepository : Repository<EnvironmentReservation>, IEnvironmentReservationRepository
    {
        public EnvironmentReservationRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByEnvironmentId(Guid environmentId)
            => await _context.EnvironmentReservations.AnyAsync(x => x.EnvironmentId == environmentId);

        public async Task<object> GetAllByFilters(Guid rid, int reservationstate)
        {
            var environments = _context.EnvironmentReservations
               .AsQueryable();
            if (rid != Guid.Empty)
                environments = environments.Where(x => x.EnvironmentId.Equals(rid)).AsQueryable();

            if (reservationstate != 0)
                environments = environments.Where(x => x.State.Equals(reservationstate)).AsQueryable();

            var results = await environments.OrderBy(a => a.State).ThenBy(a => a.ReservatedAt).Select(x => new
            {
                id = x.Id,
                fullname = x.IsExternalUser ? _context.ExternalUsers.SingleOrDefault(a => a.Id.ToString() == x.UserId).FullName
                    : _context.Users.SingleOrDefault(a => a.Id == x.UserId).FullName,
                isexternaluser = x.IsExternalUser,
                environmentname = x.Environment.Name,
                date = string.Concat(x.ReservationStart.ToString("dd/MM/yyyy hh:mm tt"), "-", x.ReservationEnd.ToString("hh:mm tt")),
                price = x.Environment.Price,
                paymentamount = x.PaymentId == null ? 0.00M : x.Payment.Total,
                paymentstate = x.PaymentId == null ? 0 : x.Payment.Status,
                reservationstate = x.State
            }).ToListAsync();

            return results;
        }

        public async Task<object> GetAllEnvironmentReservationsByExternalUser(string userId)
        {
            var result = await _context.EnvironmentReservations.Include(x => x.Environment).Where(x => x.UserId == userId).Select(x => new
            {
                Id = x.Id,
                ReservatedAt = x.ReservatedAt.ToLocalDateTimeFormat(),
                Environment = $"{x.Environment.Code} - {x.Environment.Name}",
                DateTime = $"{x.ReservationStart.ToDateTimeFormat()} {x.ReservationEnd.ToDateTimeFormat()}",
                Price = x.Environment.Price == 0 ? "Sin costo" : $"S/. {x.Environment.Price.ToString("0.00")}",
                State = x.State,
            }).OrderBy(x => x.DateTime).ThenBy(x => x.Environment).ToListAsync();

            return result;
        }

        public async Task<Tuple<bool,string>> ReserveEnvironmentByExternalUser(Guid Id, string Date, string startTime, string endTime, Guid currentExternalUser)
        {
            ENTITIES.Models.Reservations.Environment environment = await _context.Environments.FirstOrDefaultAsync(x => x.Id == Id);
            if (currentExternalUser == Guid.Empty)
                return new Tuple<bool, string>(false,"No existe el DNI ingresado.");

            if (environment == null)
                return new Tuple<bool, string>(false, "No se pudo reservar el ambiente.");

            DateTime dtStartDate = ConvertHelpers.DatetimepickerToDateTime(Date + " " + startTime);
            DateTime dtEndDate = ConvertHelpers.DatetimepickerToDateTime(Date + " " + endTime);

            if (dtStartDate >= dtEndDate)
                return new Tuple<bool, string>(false, "La hora inicio debe ser menor a la hora final.");

            string userId = currentExternalUser.ToString();

            int myreservations = await _context.EnvironmentReservations
                .Where(x => x.EnvironmentId == environment.Id && x.UserId == userId
                && (x.State == ConstantHelpers.RESERVATION.STATUS.APPROVED
                || x.State == ConstantHelpers.RESERVATION.STATUS.PENDING)
                && GetWeekOfYear(dtStartDate) == GetWeekOfYear(x.ReservationStart)).CountAsync();
            if (myreservations >= environment.MaxReservationExternal)
                return new Tuple<bool, string>(false, "Ya no puede realizar más reservas de este ambiente durante la semana");

            var dw = (int)dtStartDate.DayOfWeek - 1;

            var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

            if (dtStartDate.Date < DateTime.Now.Date)
                return new Tuple<bool, string>(false, "No existe disponibilidad para el tiempo señalado.");
            else if (dtStartDate.Date <= DateTime.Now.Date && dtStartDate < now.AddMinutes(-30))
                return new Tuple<bool, string>(false, "No existe disponibilidad para el tiempo señalado.");

            TimeSpan tsDiff = dtEndDate - dtStartDate;
            var environmentschedule = await _context.EnvironmentSchedules
                .Where(a => a.WeekDay == dw && a.EnvironmentId == Id
                && (a.StartTime >= dtStartDate.TimeOfDay && a.EndTime <= dtEndDate.TimeOfDay))
                .ToListAsync();

            if (environmentschedule.Count != tsDiff.TotalHours)
                return new Tuple<bool, string>(false, "No existe disponibilidad para el tiempo señalado.");

            var res = await _context.EnvironmentReservations.Where(x => x.EnvironmentId == Id
               && (x.State == ConstantHelpers.RESERVATION.STATUS.APPROVED
               || x.State == ConstantHelpers.RESERVATION.STATUS.PENDING)
               && ((x.ReservationStart <= dtStartDate && x.ReservationEnd > dtStartDate)
               || (x.ReservationStart < dtEndDate && x.ReservationEnd >= dtEndDate)))
                .FirstOrDefaultAsync();

            if (res != null)
                return new Tuple<bool, string>(false, "El ambiente ya ha sido reservado para el tiempo señalado.");

            EnvironmentReservation environmentReservation = new EnvironmentReservation();
            await _context.EnvironmentReservations.AddAsync(environmentReservation);

            environmentReservation.IsExternalUser = true;
            environmentReservation.UserId = userId;
            environmentReservation.ReservatedAt = DateTime.UtcNow;
            environmentReservation.State = ConstantHelpers.RESERVATION.STATUS.PENDING;
            environmentReservation.ReservationStart = dtStartDate;
            environmentReservation.ReservationEnd = dtEndDate;
            environmentReservation.EnvironmentId = environment.Id;

            if (environment.Price != 0)
            {
                Payment payment = new Payment
                {
                    Description = "Reserva de ambiente - " + environment.Name,
                    SubTotal = environment.Price * 100.00M / 118.00M,
                    IgvAmount = environment.Price * 18.00M / 118.00M,
                    Discount = 0,
                    Total = environment.Price,
                    LateCharge = 0,
                    Type = ConstantHelpers.PAYMENT.TYPES.RESERVATION,
                    EntityId = environmentReservation.Id,
                    UserId = null,
                    IssueDate = DateTime.UtcNow,
                    Quantity = 1,
                    Status = ConstantHelpers.PAYMENT.STATUS.PENDING
                };
                await _context.Payments.AddAsync(payment);
                environmentReservation.PaymentId = payment.Id;
            }
            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, "Reserva registrada satisfactoriamente.");
        }

        public async Task<Tuple<bool,string>> ReserveEnrionmentByUser(Guid Id, string Date, string StartTime, string EndTime, string userId)
        {
            var environment = await _context.Environments.Where(x=>x.Id == Id).FirstOrDefaultAsync();
            if (environment == null)
                return new Tuple<bool, string>(false, "No se pudo reservar el ambiente.");

            DateTime dtStartDate = ConvertHelpers.DatetimepickerToDateTime(Date + " " + StartTime);
            DateTime dtEndDate = ConvertHelpers.DatetimepickerToDateTime(Date + " " + EndTime);
            if (dtStartDate >= dtEndDate)
                return new Tuple<bool, string>(false, "La hora inicio debe ser menor a la hora final.");

            var userRole = await _context.UserRoles.FirstAsync(x => x.UserId == userId);
            var role = await _context.Roles.FirstAsync(x => x.Id == userRole.RoleId);

            var myreservationsData = await _context.EnvironmentReservations
                .Where(x => x.EnvironmentId == environment.Id && x.UserId == userId
                && (x.State == ConstantHelpers.RESERVATION.STATUS.APPROVED
                || x.State == ConstantHelpers.RESERVATION.STATUS.PENDING)).ToArrayAsync();

            int myreservations = myreservationsData.Where(x => GetWeekOfYear(dtStartDate) == GetWeekOfYear(x.ReservationStart)).Count();

            //int myreservations = await _context.EnvironmentReservations
            //    .Where(x => x.EnvironmentId == environment.Id && x.UserId == userId
            //    && (x.State == ConstantHelpers.RESERVATION.STATUS.APPROVED
            //    || x.State == ConstantHelpers.RESERVATION.STATUS.PENDING)
            //    && GetWeekOfYear(dtStartDate) == GetWeekOfYear(x.ReservationStart)).CountAsync();

            switch (role.Name)
            {
                case "Docentes":
                    if (myreservations >= environment.MaxReservationTeacher)
                        return new Tuple<bool, string>(false, "Ya no puede realizar más reservas de este ambiente durante la semana.");
                    break;
                case "Alumnos":
                    if (myreservations >= environment.MaxReservationStudent)
                        return new Tuple<bool, string>(false, "Ya no puede realizar más reservas de este ambiente durante la semana.");
                    break;
                default:
                    if (myreservations >= environment.MaxReservationAdmin)
                        return new Tuple<bool, string>(false, "Ya no puede realizar más reservas de este ambiente durante la semana.");
                    break;
            }

            var dw = (int)dtStartDate.DayOfWeek - 1;

            var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

            if (dtStartDate.Date < DateTime.Now.Date)
                return new Tuple<bool, string>(false, "No existe disponibilidad para el tiempo señalado.");
            else if (dtStartDate.Date <= DateTime.Now.Date && dtStartDate < now.AddMinutes(-30))
                return new Tuple<bool, string>(false, "No existe disponibilidad para el tiempo señalado.");

            TimeSpan tsDiff = dtEndDate - dtStartDate;
            var environmentschedule = await _context.EnvironmentSchedules
                .Where(a => a.WeekDay == dw && a.EnvironmentId == Id
                && (a.StartTime >= dtStartDate.TimeOfDay && a.EndTime <= dtEndDate.TimeOfDay))
                .ToListAsync();

            if (environmentschedule.Count != tsDiff.TotalHours)
                return new Tuple<bool, string>(false, "No existe disponibilidad para el tiempo señalado.");

            var res = await _context.EnvironmentReservations.Where(x => x.EnvironmentId == Id
           && (x.State == ConstantHelpers.RESERVATION.STATUS.APPROVED
           || x.State == ConstantHelpers.RESERVATION.STATUS.PENDING)
           && ((x.ReservationStart <= dtStartDate && x.ReservationEnd > dtStartDate)
           || (x.ReservationStart < dtEndDate && x.ReservationEnd >= dtEndDate)))
            .FirstOrDefaultAsync();

            if (res != null)
                return new Tuple<bool, string>(false, "El ambiente ya ha sido reservado para el tiempo señalado.");

            EnvironmentReservation environmentReservation = new EnvironmentReservation();
            await _context.EnvironmentReservations.AddAsync(environmentReservation);

            environmentReservation.IsExternalUser = false;
            environmentReservation.UserId = userId;
            environmentReservation.ReservatedAt = DateTime.UtcNow;
            environmentReservation.State = ConstantHelpers.RESERVATION.STATUS.PENDING;
            environmentReservation.ReservationStart = dtStartDate;
            environmentReservation.ReservationEnd = dtEndDate;
            environmentReservation.EnvironmentId = environment.Id;

            if (environment.Price != 0)
            {
                Payment payment = new Payment
                {
                    Description = "Reserva de ambiente - " + environment.Name,
                    SubTotal = environment.Price * 100.00M / 118.00M,
                    IgvAmount = environment.Price * 18.00M / 118.00M,
                    Discount = 0,
                    Total = environment.Price,
                    LateCharge = 0,
                    Type = ConstantHelpers.PAYMENT.TYPES.RESERVATION,
                    EntityId = environmentReservation.Id,
                    UserId = userId,
                    IssueDate = DateTime.UtcNow,
                    Quantity = 1,
                    Status = ConstantHelpers.PAYMENT.STATUS.PENDING
                };
                await _context.Payments.AddAsync(payment);
                environmentReservation.PaymentId = payment.Id;
            }
            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, string.Empty);
        }

        public async Task<object> GetEnvironmentReservationsByUser(string userId)
        {
            var dataDB = await _context.EnvironmentReservations.Include(x => x.Environment)
                .Where(x => x.UserId == userId)
                .Select(x => new
                {
                    Id = x.Id,
                    x.ReservatedAt,
                    x.Environment.Code,
                    x.Environment.Name,
                    x.ReservationStart,
                    x.ReservationEnd,
                    x.Environment.Price,
                    x.State
                })
                .ToArrayAsync();

            var result = dataDB
                .Select(x => new
                {
                    Id = x.Id,
                    ReservatedAt = x.ReservatedAt.ToLocalDateTimeFormat(),
                    Environment = $"{x.Code} - {x.Name}",
                    DateTime = $"{x.ReservationStart.ToDateTimeFormat()} {x.ReservationEnd.ToDateTimeFormat()}",
                    Price = x.Price == 0 ? "Sin costo" : $"S/. {x.Price.ToString("0.00")}",
                    State = x.State,
                })
                .OrderBy(x => x.DateTime)
                .ThenBy(x => x.Environment)
                .ToArray();

            return result;
        }

        public async Task<object> GetReservationStatesByUserId(string userId)
        {
            var data = await _context.EnvironmentReservations
                .Where(a => a.ReservationStart > DateTime.UtcNow && a.UserId == userId)
                .Select(x => new
                {
                    x.State
                })
                .ToArrayAsync();

            var topreservation = data.GroupBy(x => x.State)
                .Select(a => new
                {
                    Name = ConstantHelpers.RESERVATION.STATUS.Status[a.Key],
                    State = a.Key,
                    Number = a.Count()
                })
                .OrderBy(a => a.Number)
                .ToList();

            return topreservation;
        }

        public async Task<object> GetTopReservations()
        {
            var data = await _context.EnvironmentReservations.Where(x => x.ReservationStart.Year == DateTime.Now.Year)
                .Select(x => new { x.Environment.Code, x.Environment.Name }).ToArrayAsync();

            var result = data.GroupBy(x => x.Code)
                .Select(x => new
                {
                    code = x.Key,
                    name = x.Select(y => y.Name).FirstOrDefault(),
                    reservas = x.Count()
                })
                .OrderByDescending(x => x.reservas)
                .Take(5)
                .ToArray();

            return result;
        }

        public async Task<object> GetTopHours()
        {
            var tophours = await _context.EnvironmentReservations
             .Where(a => a.ReservationStart.Year == DateTime.Now.Year)
             .GroupBy(a => a.ReservationStart)
             .Select(a => new
             {
                 hour = a.Key,
                 number = a.Count()
             })
             .OrderByDescending(a => a.number)
             .Take(5)
             .ToListAsync();

            tophours.ForEach(x => x.hour.ToString("hh:mm tt"));

            return tophours;
        }

        public async Task<object> GetNextEnvironmetnsByUserId(string userId)
        {
            var nextenvironments = await _context.EnvironmentReservations
              .Where(a => a.ReservationStart > DateTime.UtcNow
              && a.UserId == userId
              && a.State == ConstantHelpers.RESERVATION.STATUS.APPROVED)
              .OrderBy(a => a.ReservationStart)
              .Take(3)
              .Select(a => new
              {
                  reservatedat = a.ReservatedAt.ToString("dd/MM/yyyy"),
                  reservationtime = string.Concat(a.ReservationStart.ToString("dd/MM/yyyy hh:mm tt"), " - ", a.ReservationEnd.ToString("hh:mm tt")),
                  name = a.Environment.Name,
                  imageurl = a.Environment.ImageURL,
                  price = a.Environment.Price.ToString()
              })
              .ToListAsync();

            return nextenvironments;
        }

        private static int GetWeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                time = time.AddDays(3);
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

    }
}

using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Bogus.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class CashierDependencyRepository : Repository<CashierDependency>, ICashierDependencyRepository
    {

        protected readonly UserManager<ApplicationUser> _userManager;
        public CashierDependencyRepository(AkdemicContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<List<CashierDependency>> GetCashierDependeciesByIdList(string id)
        {
            var result = await _context.CashierDependencies
                .Where(x => x.UserId == id)
                .ToListAsync();

            return result;
        }

        public async Task<object> GetConcepts(string userId)
        {
            var cashierDependencies = _context.CashierDependencies
             .Where(x => x.UserId == userId)
             .Select(x => x.DependencyId)
             .ToHashSet();

            var concepts = await _context.Concepts
                .Where(x => cashierDependencies.Contains(x.DependencyId))
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    description = x.Description,
                    amount = x.Amount,
                    accounting = x.Classifier.Code,
                    accountingId = x.Classifier.Id,
                    dependency = x.Dependency.Name,
                    dependencyId = x.Dependency.Id,
                    isTaxed = x.IsTaxed,
                    isDivided = x.IsDividedAmount,
                    conceptDistributionId = x.ConceptDistributionId,
                    accountingPlan = x.AccountingPlanId,
                    currentAccount = x.CurrentAccountId
                }).ToListAsync();

            return concepts;
        }

        public async Task<List<CashierDependency>> GetCashierDependenciesByUserId(string userId)
        {
            var cashierDependencies = await _context.CashierDependencies.Include(x => x.Dependency)
            .Where(x => x.UserId == userId).ToListAsync();

            return cashierDependencies;
        }

        public async Task<object> GetCashierDependenciesJsonByUserId(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, ConstantHelpers.ROLES.TREASURY) || await _userManager.IsInRoleAsync(user, ConstantHelpers.ROLES.SUPERADMIN) || await _userManager.IsInRoleAsync(user, ConstantHelpers.ROLES.ECONOMIC_MANAGEMENT_ADMIN))
                {
                    return new
                    {
                        items = await _context.Dependencies
                        .Where(x => x.IsActive)
                        .Select(x => new
                        {
                            id = x.Id,
                            text = x.Name
                        })
                        .OrderBy(x => x.text)
                        .ToListAsync()
                    };
                }
            }

            var data = await _context.CashierDependencies
                .Where(x => x.UserId == userId && x.Dependency.IsActive)
                .Select(x => new
                {
                    id = x.Dependency.Id,
                    text = x.Dependency.Name
                })
                .OrderBy(x => x.text)
                .ToListAsync();

            var result = new
            {
                items = data
            };

            return result;
        }

        public async Task<object> GetAllConceptsDatatatable()
        {
            var concepts = await _context.Concepts
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    description = x.Description,
                    amount = x.Amount,
                    accounting = x.Classifier.Code,
                    accountingId = x.Classifier.Id,
                    dependency = x.Dependency.Name,
                    dependencyId = x.Dependency.Id,
                    isTaxed = x.IsTaxed,
                    isDivided = x.IsDividedAmount,
                    conceptDistributionId = x.ConceptDistributionId,
                    accountingPlan = x.AccountingPlanId,
                    currentAccount = x.CurrentAccountId
                }).ToListAsync();

            return concepts;
        }


    }
}

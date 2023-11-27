using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class PerformanceInfoRepository<T> : Repository<T>, IPerformanceInfoRepository
        where T : class
    {
        public DbSet<T> dbSet;
        public Stopwatch stopwatch;

        public PerformanceInfoRepository(AkdemicContext context) : base(context) {
            dbSet = _context.Set<T>();
            stopwatch = new Stopwatch();
        }
        
        public List<PerformanceInfo> GetPerformanceInfo()
        {
            var stopwatchList = new List<PerformanceInfo>();
            dynamic result = null;

            stopwatchList.Add(RunTest("AsEnumerable", stopwatch, () =>
            {
                result = dbSet.AsEnumerable();
            }));

            stopwatchList.Add(RunTest("AsQueryable", stopwatch, () =>
            {
                result = dbSet.AsQueryable();
            }));

            stopwatchList.Add(RunTest("ToArray", stopwatch, () =>
            {
                result = dbSet.ToArray();
            }));

            stopwatchList.Add(RunTest("ToArrayAsync", stopwatch, () =>
            {
                result = dbSet.ToArrayAsync().GetAwaiter().GetResult();
            }));

            //stopwatchList.Add(RunTest("ToAsyncEnumerable", stopwatch, () =>
            //{
            //    result = dbSet.ToAsyncEnumerable();
            //}));

            stopwatchList.Add(RunTest("ToList", stopwatch, () =>
            {
                result = dbSet.ToList();
            }));

            stopwatchList.Add(RunTest("ToListAsync", stopwatch, () =>
            {
                result = dbSet.ToListAsync().GetAwaiter().GetResult();
            }));

            stopwatchList.Add(RunTest("AsEnumerable + ToArray", stopwatch, () =>
            {
                result = dbSet.AsEnumerable();
                result = dbSet.ToArray();
            }));

            stopwatchList.Add(RunTest("AsEnumerable + ToArrayAsync", stopwatch, () =>
            {
                result = dbSet.AsEnumerable();
                result = dbSet.ToArrayAsync().GetAwaiter().GetResult();
            }));

            stopwatchList.Add(RunTest("AsEnumerable + ToList", stopwatch, () =>
            {
                result = dbSet.AsEnumerable();
                result = dbSet.ToList();
            }));

            stopwatchList.Add(RunTest("AsEnumerable + ToListAsync", stopwatch, () =>
            {
                result = dbSet.AsEnumerable();
                result = dbSet.ToListAsync().GetAwaiter().GetResult();
            }));

            stopwatchList.Add(RunTest("AsQueryable + ToArray", stopwatch, () =>
            {
                result = dbSet.AsEnumerable();
                result = dbSet.ToArray();
            }));

            stopwatchList.Add(RunTest("AsQueryable + ToArrayAsync", stopwatch, () =>
            {
                result = dbSet.AsEnumerable();
                result = dbSet.ToArrayAsync().GetAwaiter().GetResult();
            }));

            stopwatchList.Add(RunTest("AsQueryable + ToList", stopwatch, () =>
            {
                result = dbSet.AsEnumerable();
                result = dbSet.ToList();
            }));

            stopwatchList.Add(RunTest("AsQueryable + ToListAsync", stopwatch, () =>
            {
                result = dbSet.AsEnumerable();
                result = dbSet.ToListAsync().GetAwaiter().GetResult();
            }));

            stopwatchList.Add(RunTest("ToArray + ToList", stopwatch, () =>
            {
                result = dbSet.ToArray();
                result = dbSet.ToList();
            }));

            stopwatchList.Add(RunTest("ToArray + ToListAsync", stopwatch, () =>
            {
                result = dbSet.ToArray();
                result = dbSet.ToListAsync().GetAwaiter().GetResult();
            }));

            //stopwatchList.Add(RunTest("ToAsyncEnumerable + ToArray", stopwatch, () =>
            //{
            //    result = dbSet.ToAsyncEnumerable();
            //    result = dbSet.ToArray();
            //}));

            //stopwatchList.Add(RunTest("ToAsyncEnumerable + ToArrayAsync", stopwatch, () =>
            //{
            //    result = dbSet.ToAsyncEnumerable();
            //    result = dbSet.ToArrayAsync().GetAwaiter().GetResult();
            //}));

            //stopwatchList.Add(RunTest("ToAsyncEnumerable + ToList", stopwatch, () =>
            //{
            //    result = dbSet.ToAsyncEnumerable();
            //    result = dbSet.ToList();
            //}));

            //stopwatchList.Add(RunTest("ToAsyncEnumerable + ToListAsync", stopwatch, () =>
            //{
            //    result = dbSet.ToAsyncEnumerable();
            //    result = dbSet.ToListAsync().GetAwaiter().GetResult();
            //}));

            stopwatchList.Add(RunTest("ToList + ToArray", stopwatch, () =>
            {
                result = dbSet.ToList();
                result = dbSet.ToArray();
            }));

            stopwatchList.Add(RunTest("ToList + ToArrayAsync", stopwatch, () =>
            {
                result = dbSet.ToList();
                result = dbSet.ToArrayAsync().GetAwaiter().GetResult();
            }));

            stopwatchList.Add(RunTest("AsQueryable + AsEnumerable + ToArray", stopwatch, () =>
            {
                result = dbSet.AsQueryable();
                result = dbSet.AsEnumerable();
                result = dbSet.ToArray();
            }));

            stopwatchList.Add(RunTest("AsQueryable + AsEnumerable + ToArrayAsync", stopwatch, () =>
            {
                result = dbSet.AsQueryable();
                result = dbSet.AsEnumerable();
                result = dbSet.ToArrayAsync().GetAwaiter().GetResult();
            }));

            stopwatchList.Add(RunTest("AsQueryable + AsEnumerable + ToList", stopwatch, () =>
            {
                result = dbSet.AsQueryable();
                result = dbSet.AsEnumerable();
                result = dbSet.ToList();
            }));

            stopwatchList.Add(RunTest("AsQueryable + AsEnumerable + ToListAsync", stopwatch, () =>
            {
                result = dbSet.AsQueryable();
                result = dbSet.AsEnumerable();
                result = dbSet.ToListAsync().GetAwaiter().GetResult();
            }));

            //stopwatchList.Add(RunTest("AsQueryable + ToAsyncEnumerable + ToArray", stopwatch, () =>
            //{
            //    result = dbSet.AsQueryable();
            //    result = dbSet.ToAsyncEnumerable();
            //    result = dbSet.ToArray();
            //}));

            //stopwatchList.Add(RunTest("AsQueryable + ToAsyncEnumerable + ToArrayAsync", stopwatch, () =>
            //{
            //    result = dbSet.AsQueryable();
            //    result = dbSet.ToAsyncEnumerable();
            //    result = dbSet.ToArrayAsync().GetAwaiter().GetResult();
            //}));

            //stopwatchList.Add(RunTest("AsQueryable + ToAsyncEnumerable + ToList", stopwatch, () =>
            //{
            //    result = dbSet.AsQueryable();
            //    result = dbSet.ToAsyncEnumerable();
            //    result = dbSet.ToList();
            //}));

            //stopwatchList.Add(RunTest("AsQueryable + ToAsyncEnumerable + ToListAsync", stopwatch, () =>
            //{
            //    result = dbSet.AsQueryable();
            //    result = dbSet.ToAsyncEnumerable();
            //    result = dbSet.ToListAsync().GetAwaiter().GetResult();
            //}));

            stopwatch.Stop();

            return stopwatchList;
        }

        private PerformanceInfo RunTest(string name, Stopwatch stopwatch, Action action)
        {
            stopwatch.Restart();
            var task = Task.Run(action);
            task.Wait();

            var performanceInfo = new PerformanceInfo
            {
                Name = name,
                StopwatchEllapsed = stopwatch.Elapsed,
                StopwatchEllapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                StopwatchEllapsedTicks = stopwatch.ElapsedTicks
            };

            stopwatch.Stop();

            return performanceInfo;
        }
    }
}

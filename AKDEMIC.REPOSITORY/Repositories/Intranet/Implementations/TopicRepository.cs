using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class TopicRepository : Repository<Topic>,ITopicRepository
    {
        public TopicRepository(AkdemicContext context) : base(context) { }

        public async Task<List<Topic>> GetCustomAllWithIncludesByForum(Guid forumId, string search = null)
        {
            var query = _context.Topics.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var searchTrim = search.ToUpper().Trim();
                query = query.Where(x => x.Title.ToUpper().Contains(searchTrim));
            }

            var result = await query
                .Include(x => x.User)
                .Include(x => x.Posts)
                .Where(x => x.ForumId == forumId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Topic>> GetTopicsByForumId(Guid forumId)
            => await _context.Topics.Include(x => x.User).Include(x => x.Posts).Where(x => x.ForumId == forumId).OrderByDescending(x => x.CreatedAt).ToArrayAsync();

        public async Task<object> GetTopicsHome()
        {
            await Task.Delay(10);
            var topics = _context.Topics
                .Include(x => x.Forum)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    title = x.Title,
                    category = x.Forum.Name,
                    count = x.Posts.Count(),
                    forumid = x.ForumId,
                    categoryid = x.Id,
                    createdat = x.CreatedAt
                })
                .Take(3)
                .ToList();

            return topics;
        }

        public async Task<bool> HasPosts(Guid id)
        {
            return await _context.Topics.AnyAsync(x => x.Id == id && x.Posts.Count()> 0);
        }
    }
}

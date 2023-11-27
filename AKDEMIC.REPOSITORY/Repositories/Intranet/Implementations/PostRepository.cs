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
    public class PostRepository : Repository<Post> , IPostRepository
    {
        public PostRepository(AkdemicContext context) :base(context) { }

        public async Task<Post> GetByTopicAndLevel(Guid topicId, int level)
        {
            var query = _context.Posts
                .Where(x => x.TopicId == topicId && x.Level == level);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Post> GetPostByPostCitedId(Guid id)
            => await _context.Posts.Include(x => x.User).Include(x => x.PostCited).Include(x => x.PostCited.User).Include(x => x.Topic.Forum)
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Post> GetPostByTopicId(Guid topicId)
            => await _context.Posts.Where(x => x.TopicId == topicId && x.Level == 0).FirstOrDefaultAsync();

        public async Task<IEnumerable<Post>> GetPostsByTopicIdAndForumId(Guid topicId, Guid forumId)
            => await _context.Posts.Include(x => x.User).Include(x => x.Topic).Include(x => x.PostCited).Include(x => x.PostCited.User)
                .Include(x => x.Topic.Forum).Where(x => x.Topic.Id == topicId && x.Topic.ForumId == forumId)
                .OrderBy(x => x.CreatedAt).ToArrayAsync();

        public async Task<IEnumerable<Post>> GetPostsByTopicIdAndForumIdNotDeleted(Guid topicId, Guid forumId)
        {
            var query = _context.Posts
                         .Include(x => x.User)
                         .Include(x => x.Topic)
                         .Include(x => x.PostCited)
                         .Include(x => x.PostCited.User)
                         .Include(x => x.Topic.Forum)
                         .Where(x => x.Topic.Id == topicId && x.Topic.ForumId == forumId && x.DeletedAt == null)
                         .OrderBy(x => x.CreatedAt);

            return await query.ToListAsync();
        }

        public async Task<Post> GetWithIncludes(Guid id)
        {
            var query = _context.Posts
                         .Include(x => x.User)
                         .Include(x => x.Topic)
                         .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Post> GetWithIncludesByTopic(Guid topicId)
        {
            var query = _context.Posts
                         .Include(x => x.User)
                         .Include(x => x.Topic)
                         .Where(x => x.Topic.Id == topicId);

            return await query.FirstOrDefaultAsync();
        }
    }
}

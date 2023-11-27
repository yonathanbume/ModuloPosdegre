using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.ViewModels.ForumViewModels;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS + "," + ConstantHelpers.ROLES.TEACHERS + "," + ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Route("foro")]
    public class ForumController : BaseController
    {
        private readonly ITopicService _topicService;
        private readonly IForumService _forumService;
        private readonly IPostService _postService;

        public ForumController(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            ITopicService topicService,
            IForumService forumService,
            IPostService postService,
            ICloudStorageService cloudStorageService)
            : base( userService, cloudStorageService)
        {
            _topicService = topicService;
            _forumService = forumService;
            _postService = postService;
        }

        /// <summary>
        /// Obtiene la vista inicial de foro
        /// </summary>
        /// <returns>Retorna una vista</returns>
        public async Task<IActionResult> Index()
        {
            var forumViewModel = new ForumViewModel
            {
                Forums = await _forumService.IndexForumJobExchange(GetUserId(), ConstantHelpers.Solution.Intranet)
            };
            return View(forumViewModel);
        }

        /// <summary>
        /// Obtiene la vista del foro
        /// </summary>
        /// <param name="id">Identificador del foro</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("categoria/{id}")]
        public async Task<IActionResult> Category(Guid id)
        {
            var categoryViewModel = new CategoryViewModel
            {
                GategoryID = id,
                Topics = await _topicService.GetCustomAllWithIncludesByForum(id)
            };
            var category = await _forumService.Get(id);
            ViewBag.CategoryId = category.Id;
            ViewBag.CategoryName = category.Name;
            return View(categoryViewModel);
        }

        /// <summary>
        /// Agrega un post
        /// </summary>
        /// <param name="post">Modelo que contiene el post</param>
        [Route("FillPost")]
        public async Task FillPost(Post post)
        {
            while (post.PostCitedId.HasValue)
            {

                post.PostCited = await _postService.GetPostByPostCitedId(post.PostCitedId.Value);
                await FillPost(post.PostCited);
                return;
            }
            return;
        }

        /// <summary>
        /// Obtiene la vista del tema, dentro del foro
        /// </summary>
        /// <param name="categoryId">Identificador del foro</param>
        /// <param name="topicId">Identificador del tema</param>
        /// <returns>Retorna una vista</returns>
        [Route("tema/{categoryid}/{topicid}")]
        public async Task<IActionResult> Topic(Guid categoryId, Guid topicId)
        {
            var userId = GetUserId();
            var topicViewModel = new TopicViewModel
            {
                TopicId = topicId,
                CurrentUserId = userId,
                CurrentUser = await GetCurrentUserAsync()
            };

            var posts = await _postService.GetPostsByTopicIdAndForumIdNotDeleted(topicId, categoryId);

            foreach (var post in posts)
            {
                await FillPost(post);
            }

            topicViewModel.Posts = posts;
            var category = await _forumService.Get(categoryId);
            ViewBag.CategoryId = category.Id;
            ViewBag.CategoryName = category.Name;
            var topic = await _topicService.Get(topicId);
            ViewBag.TopicId = topic.Id;
            ViewBag.TopicTitle = topic.Title;
            return View(topicViewModel);
        }

        /// <summary>
        /// Obtiene la vista del tema
        /// </summary>
        /// <param name="category">Foro</param>
        /// <param name="topic">Tema</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("categoria/{category}/{topic}")]
        public IActionResult Topic(string category, string topic)
        {
            return View();
        }

        /// <summary>
        /// Crea un nuevo tema dentro del foro
        /// </summary>
        /// <param name="topicViewModel">Modelo que contiene el tema</param>
        /// <returns>Redirecciona al mismo tema</returns>
        [HttpPost]
        public async Task<IActionResult> CreateTopic(TopicViewModel topicViewModel)
        {
            var path = "";
            var fileName = "";
            if (topicViewModel.File != null)
            {
                var extension = Path.GetExtension(topicViewModel.File.FileName);
                path = await _cloudStorageService.UploadFile(topicViewModel.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.FORUM_FILES,
                    extension, CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            var userId = GetUserId();
            var topic = new Topic
            {
                ForumId = topicViewModel.ForumId,
                Title = topicViewModel.Title,
                UserId = userId
            };
            await _topicService.Insert(topic);

            var post = new Post
            {
                Topic = topic,
                Message = topicViewModel.Message ?? "-",
                PathFile = path,
                FileName = fileName,
                UserId = userId,
                Level = 0
            };
            await _postService.Insert(post);
            return RedirectToAction("Topic", new { categoryId = topicViewModel.ForumId, topicId = topic.Id });
        }

        /// <summary>
        /// Crea un nuevo post dentro del tema
        /// </summary>
        /// <param name="postViewModel">Modelo que contiene el post</param>
        /// <returns>Redirecciona al mismo tema</returns>
        [HttpPost("crear-post/post")]
        public async Task<IActionResult> CreatePost(PostViewModel postViewModel)
        {
            var path = "";
            var fileName = "";
            if (postViewModel.File != null)
            {
                string extension = Path.GetExtension(postViewModel.File.FileName);
                path = await _cloudStorageService.UploadFile(postViewModel.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.FORUM_FILES,
                    extension, CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            var userId = GetUserId();
            var postCited = postViewModel.PostCitedId.HasValue ?
                await _postService.Get(postViewModel.PostCitedId.Value)
                : await _postService.GetPostByTopicId(postViewModel.TopicId);

            var topic = await _topicService.Get(postViewModel.TopicId);
            var post = new Post
            {
                Topic = topic,
                PathFile = path,
                FileName = fileName,
                PostCited = postCited,
                Message = postViewModel.Message ?? "-",
                UserId = userId,
                Level = postCited.Level == 0 ? 1 : 2
            };

            await _postService.Insert(post);
            return RedirectToAction("Topic", new { categoryId = topic.ForumId, topicId = topic.Id });
        }

        /// <summary>
        /// Elimina el post
        /// </summary>
        /// <param name="id">Identificador del post</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("post/eliminar/post")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var post = await _postService.GetWithIncludes(id);

            var firstPostForCategory = await _postService.GetPostByTopicId(post.TopicId);

            if (post == firstPostForCategory)
            {
                var topic = await _topicService.Get(post.TopicId);
                await _topicService.Delete(topic);
                await _postService.Delete(post);
                return Ok("Se eliminó el tema correctamente");
            }
            else
            {
                await _postService.Delete(post);
                return Ok();
            }
        }
    }
}

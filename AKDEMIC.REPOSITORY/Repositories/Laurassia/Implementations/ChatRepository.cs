using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Laurassia;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Laurassia.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Laurassia.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Laurassia.Implementations
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        public ChatRepository(AkdemicContext context) : base(context) { }

        public async Task<Chat> GetFromUsers(string receptorId, string emisorId)
        {
            var query = _context.Chats
                    .Include(x => x.Mensaje)
                    .Where(x => x.ReceptorId == receptorId && x.EmisorId == emisorId);

            if (query.Any(x => x.ReceptorId == receptorId && x.Mensaje.Any(y => y.Nombre == false.ToString())))//si el mensaje no ha sido leido
            {
                await query.ForEachAsync(x =>
                 {
                     x.Mensaje.ToList().ForEach(y =>
                     {
                         y.Nombre = true.ToString();
                     });
                 });
            }

            await _context.SaveChangesAsync();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Chat> GetWithIncludes(Guid id)
        {
            var query = _context.Chats
                .Include(x => x.Mensaje)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Chat> SingleOrDefaultByConditions(string emisorId = null, string receptorId = null)
        {
            var query = _context.Chats.AsQueryable();

            if (emisorId != null)
                query = query.Where(x => x.EmisorId == emisorId);

            if (receptorId != null)
                query = query.Where(x => x.ReceptorId == receptorId);

            return await query.SingleOrDefaultAsync();
        }

        public async Task<List<ChatTemplate>> HasUnreadMessages(string receptorId)
        {
            var chats =await _context.Chats.Include(x => x.Mensaje).Where(x => x.Mensaje.Any(y => y.Nombre == false.ToString()) && (x.ReceptorId == receptorId || x.EmisorId == receptorId)).ToListAsync();
            var chatTemplates = new List<ChatTemplate>();
            foreach (var item in chats)
            {
                chatTemplates.Add(new ChatTemplate { User = _context.Users.FirstOrDefault(x => x.Id == item.EmisorId), Count = item.Mensaje.Count(x => x.Nombre == false.ToString()) });
            }

            return chatTemplates;
        }

        public async Task<object> ChatList(string userId, List<string> connectedUsers, string name)
        {
            var chats = _context.Messages
                .Include(x => x.Chat.Emisor)
                .Include(x => x.Chat.Receptor)
                .Where(x => x.Chat.EmisorId == userId || x.Chat.ReceptorId == userId);

            if (!string.IsNullOrEmpty(name))
            {
                chats = chats.Where(x => x.Chat.Emisor.FullName.Contains(name) ||
                                         x.Chat.Receptor.FullName.Contains(name) ||
                                         x.Chat.Emisor.UserName.Contains(name) ||
                                         x.Chat.Receptor.UserName.Contains(name));
            }

            var chat2 = await chats
                                .OrderByDescending(x => x.Fecha)
                                .ToListAsync();

            var connecteds = connectedUsers.Where(x => userId != x).ToList(); //1

            var applicationUsers = new List<ApplicationUser>();

            foreach (var chat in chat2)
            {
                if (chat.Chat.EmisorId == userId)
                    applicationUsers.Add(chat.Chat.Receptor);
                if (chat.Chat.ReceptorId == userId)
                    applicationUsers.Add(chat.Chat.Emisor);
            }

            //si tiene mensajes sin leer de alguien
            var unconnectedUsers = await HasUnreadMessages(userId);
            foreach (var item in unconnectedUsers)
            {
                if (item.User.Id != userId)
                    applicationUsers.Add(item.User);
            }

            var result = applicationUsers.Distinct()
                         .Select(x => new
                         {
                             Id = x.Id,
                             User = x.UserName + ": " + x.FullName,
                             Email = x.Email,
                             Count = unconnectedUsers.FirstOrDefault(y => y.User.Id == x.Id) == null ? 0 : unconnectedUsers.FirstOrDefault(y => y.User.Id == x.Id).Count,
                             IsConnected = connectedUsers.Where(y => y == x.Id).Count() > 0,
                             userImg = (x.Picture == null) ? "../images/demo/user.png" : "/imagenes/" + x.Picture
                         }).ToList();

            return result;
        }
    }
}

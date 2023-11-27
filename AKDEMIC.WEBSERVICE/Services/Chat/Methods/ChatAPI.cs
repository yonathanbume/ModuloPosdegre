using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using AKDEMIC.WEBSERVICE.Services.Chat.Extensions;
using AKDEMIC.WEBSERVICE.Services.Chat.Models.Extension;
using AKDEMIC.WEBSERVICE.Services.Chat.Models.Request;
using AKDEMIC.WEBSERVICE.Services.Chat.Models.Result;
using Newtonsoft.Json;

namespace AKDEMIC.WEBSERVICE.Services.Chat.Methods
{
    public class ChatAPI
    {
        //private static string URL = "http://sigau-chat.azurewebsites.net";
        private static string URL = "https://localhost:44302";
        private static string URL_CHAT_ADD(byte system) => $"{URL}/api/chat/{system}/chat/agregar";
        private static string URL_CHATS_GET(byte system) => $"{URL}/api/chat/{system}/chats/get";
        private static string URL_MESSAGE_ADD(byte system) => $"{URL}/api/chat/{system}/mensaje/agregar";
        private static string URL_MESSAGES_GET(byte system, Guid chatId) => $"{URL}/api/chat/{system}/mensajes/{chatId}/get";
        private static string URL_CHATS_BY_USER_GET(byte system , string userId) => $"{URL}/api/chat/{system}/chats/{userId}/get";

        private static string JsonStringPorGet(string url)
        {
            try
            {
                WebClient client = new WebClient
                {
                    Encoding = Encoding.UTF8
                };
                return client.DownloadString(url);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string JsonStringPost(string url, string parameters)
        {
            try
            {
                WebClient client = new WebClient
                {
                    Headers = { [HttpRequestHeader.ContentType] = "application/json" },
                    Encoding = Encoding.UTF8
                };              
                return client.UploadString(url, parameters);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static byte[] JsonStringFormDataPost(string url, MultipartFormBuilder parameters)
        {
            try
            {
                //WebClient client = new WebClient
                //{
                //    Headers = { [HttpRequestHeader.ContentType] = "multipart/form-data" },
                //    Encoding = Encoding.UTF8
                //};

                //using (var stream = parameters.GetStream())
                //{
                //    return client.UploadData(address, method, stream.ToArray());
                //}
               

                using (var client = new WebClient { Encoding = Encoding.UTF8 })
                {

                    return client.UploadMultipart(url, "POST", parameters);
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static AddChatResult AddChat(AddChatRequest model)
        {
            try
            {
                var parameters = JsonConvert.SerializeObject(model);
                var jsonString = JsonStringPost(URL_CHAT_ADD(CORE.Helpers.ConstantHelpers.Chat.System.Enrollment), parameters);
                if (jsonString == null) return null;
                return JsonConvert.DeserializeObject<AddChatResult>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //private static T Deserialize<T>(byte[] param)
        //{
        //    using (MemoryStream ms = new MemoryStream(param))
        //    {
        //        IFormatter br = new BinaryFormatter();
        //        return (T)br.Deserialize(ms);
        //    }
        //}

        

        private static T Deserialize<T>(byte[] buffer)
        {
            using (StreamReader sr = new StreamReader(new MemoryStream(buffer)))
            {
                return (T)JsonConvert.DeserializeObject(sr.ReadToEnd());
            }
        }

        public static AddChatResult AddMessage(AddMessageRequest model)
        {
            try
            {
                //var parameters = JsonConvert.SerializeObject(model);
        //        byte[] data;
                MultipartFormBuilder parameters = new MultipartFormBuilder();
                parameters.AddField("Text", model.Text);
                parameters.AddField("ChatId", model.ChatId.ToString());
                parameters.AddField("DateTime", model.DateTime.ToString());
                parameters.AddField("UserId", model.UserId);
                if (model.File != null)
                {
                    parameters.AddFile("File", model.File);
                }               
                var jsonString = JsonStringFormDataPost(URL_MESSAGE_ADD(CORE.Helpers.ConstantHelpers.Chat.System.Enrollment), parameters);
                if (jsonString == null) return null;
               
                string jsonStringUTF8 = System.Text.Encoding.UTF8.GetString(jsonString);
                var result = JsonConvert.DeserializeObject<AddChatResult>(jsonStringUTF8);                
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static GetChatsResult GetChats()
        {
            try
            {
                var jsonString = JsonStringPorGet(URL_CHATS_GET(CORE.Helpers.ConstantHelpers.Chat.System.Enrollment));
                if (jsonString == null) return null;
                return JsonConvert.DeserializeObject<GetChatsResult>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static GetChatsResult GetChatsByUser(string userId)
        {
            try
            {
                var jsonString = JsonStringPorGet(URL_CHATS_BY_USER_GET(CORE.Helpers.ConstantHelpers.Chat.System.Enrollment, userId));
                if (jsonString == null) return null;
                return JsonConvert.DeserializeObject<GetChatsResult>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static GetMessagesResult GetMessages(Guid chatId)
        {
            try
            {
                var jsonString = JsonStringPorGet(URL_MESSAGES_GET(CORE.Helpers.ConstantHelpers.Chat.System.Enrollment, chatId));
                if (jsonString == null) return null;
                return JsonConvert.DeserializeObject<GetMessagesResult>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

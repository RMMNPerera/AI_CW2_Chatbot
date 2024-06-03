using ExploreSrilanka.Classes.Enum;
using System.Net;

namespace ExploreSrilanka.Models.ChatBot
{
    public class ChatResponse
    {
        /// <summary>
        /// Defalut constructor
        /// </summary>
        public ChatResponse()
        {
            Buttons  = new List<ChatBotButton>();
            Messeges = new List<string>();
        }

        /// <summary>
        /// List of buttons
        /// </summary>
        public List<ChatBotButton> Buttons { get; set; }

        /// <summary>
        /// List of messeges
        /// </summary>
        public List<string> Messeges       { get; set; }

        /// <summary>
        /// Type of response
        /// </summary>
        public ChatResponseType Type       { get; set; }

        /// <summary>
        /// Status response code
        /// </summary>
        public HttpStatusCode Status       { get; set; }
    }
}

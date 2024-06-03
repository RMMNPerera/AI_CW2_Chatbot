using ExploreSrilanka.Classes.Enum;
using ExploreSrilanka.DbLayer.Domains;
using ExploreSrilanka.Models.ChatBot;
using ExploreSrilanka.ServiceLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace ExploreSrilanka.Controllers
{
    public class ChatBotController : Controller
    {
        private readonly IChatBotService _chatBotService;

        public ChatBotController(IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get user input and send a response
        /// </summary>
        /// <param name="userInput">User input as a text</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ChatMessege(string userInput)
        {
            bool waitingForAnswer  = HttpContext.Session.GetString("waitingForAnswer") == "true";
            int? unknownQuestionId = HttpContext.Session.GetInt32("unknownQuestionId");

            var res  = new List<BotAnswer>();
            var data = new ChatResponse
            {
                Status = HttpStatusCode.OK,
                Type = ChatResponseType.Text
            };

            // Waiting for an answer
            if (waitingForAnswer && unknownQuestionId != null && unknownQuestionId != default && unknownQuestionId != 0)
            {
                if (userInput.ToLower().Contains("no") || userInput.ToLower().Contains("dont know"))
                {
                    string? error = await _chatBotService.DeleteUnknownQuestion((int)unknownQuestionId);
					data.Messeges.Add("No Problem! Sorry I couldn't help you.");
				}
                else
                {
                    string? error = await _chatBotService.UpdateUnkownQuestionsAnswer(new UnkownQuestions { Id = (int)unknownQuestionId, Answer = userInput });

                    if (string.IsNullOrEmpty(error))
                        data.Messeges.Add("Thank you! I will remember it.");
                    else
                        data.Messeges.Add("Can not save answer !");

                }
				HttpContext.Session.SetString("waitingForAnswer", "false");
				HttpContext.Session.SetInt32("unknownQuestionId", 0);
			}
            else
            {
                res = await _chatBotService.GetResponse(userInput);

                // No answer received
                if ((res.Count == 1 || res.Count == 0) && string.IsNullOrEmpty(res?.FirstOrDefault()?.Text))
                {
					var id = await _chatBotService.SaveUnkownQuestions(new UnkownQuestions { Question = userInput });

                    if (id == default)
                    {
                        data.Messeges.Add("Sorry I couldn't find an answer");
                    }
                    else
                    {
                        HttpContext.Session.SetString("waitingForAnswer", "true");
                        HttpContext.Session.SetInt32("unknownQuestionId", (int)id);
						data.Messeges.Add("Sorry I couldn't find an answer");
						data.Messeges.Add("If you know can you please tell me the answer?");
                    }

                }
                // We do have answer
                else
                {
                    data = GetResponseGetChatResponse(res, data);
                }
            }

            return Json(data);
        }

        #region Helper

        /// <summary>
        /// Convert answer to ChatResponse
        /// </summary>
        /// <param name="botAnswers">List of answers</param>
        /// <param name="data">ChatResponse initialized</param>
        /// <returns></returns>
        private ChatResponse GetResponseGetChatResponse(List<BotAnswer> botAnswers, ChatResponse data) 
        {
            // Get intent type id
            long intentType = botAnswers.Select(x => x.Question.IntentId).FirstOrDefault();

            switch(intentType) 
            {
                case (int)IntentType.Greeting:
                    // If there is many answers pick one random
                    var randomAnswer = botAnswers[new Random().Next(botAnswers.Count)];

                    data.Type = (ChatResponseType)randomAnswer.AnswerTypeId;

                    if (data.Type == ChatResponseType.Text)
                        data.Messeges.Add(randomAnswer.Text);

                    break;
                case (int)IntentType.Farewell:

                    var randomAnswerFarewell = botAnswers[new Random().Next(botAnswers.Count)];

                    data.Type = (ChatResponseType)randomAnswerFarewell.AnswerTypeId;

                    if (data.Type == ChatResponseType.Text)
                        data.Messeges.Add(randomAnswerFarewell.Text);

                    break;
                case (int)IntentType.Find:

                    AddToMessege(botAnswers, data);

                    break;
                default:     
                    
                    data.Type = ChatResponseType.Text;

                    // Got answer with no intent
                    if (botAnswers.Any())
                        AddToMessege(botAnswers, data);
                    // No answer
                    //else
                    //    data.Messeges.Add("Sorry I could't find an answer");
                    break;
            }

            return data;
        }

        /// <summary>
        /// Add messege to chat response
        /// </summary>
        /// <param name="botAnswer"></param>
        /// <param name="data"></param>
        private void AddToMessege(List<BotAnswer> botAnswer, ChatResponse data) 
        {
            foreach (BotAnswer answer in botAnswer)
            {
                if (answer.AnswerTypeId == (int)ChatResponseType.Text) 
                {           
                    data.Messeges.Add(answer.Text);
                }
            }
        }

        #endregion
    }
}


using ExploreSrilanka.Classes.Enum;
using ExploreSrilanka.DbLayer.Domains;
using ExploreSrilanka.DbLayer.Repositories;

namespace ExploreSrilanka.ServiceLayer.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly IChatBotRepo _chatBotRepo;

        public ChatBotService(IChatBotRepo chatBotRepo)
        {
            _chatBotRepo = chatBotRepo;
        }

        /// <summary>
        /// Get user input and return a response
        /// </summary>
        /// <param name="userInput">Unser input in string</param>
        /// <returns>
        /// Success : Answer , Else : Error messege
        /// </returns>
        public async Task<List<BotAnswer>> GetResponse(string userInput) 
        {
            var answers       = new List<BotAnswer>();
            var answersFromDb = new List<BotAnswer>();

            var answer = new BotAnswer
            {
                AnswerTypeId = (int)ChatResponseType.Text,
                Text = string.Empty
            };

            answers.Add(answer);

            // All intents keywords
            var userIntentKeywords  = await UserIntents();

            // Query ,Question we need to looking for
            var searchQuery = GetQuery(userInput, userIntentKeywords, out List<string> queryWords);

            // Get user real intent based on query words
            var intent = GetUserIntent(userIntentKeywords, queryWords, searchQuery);

            /* 
             * Can not find intent
             * Not sure what is looking for 
             * But search similar question
            */
            if (intent is null)
                return await AnswersFromKeyword(searchQuery);
            /* 
             * We know exactly what user is looking for
             * Give answer with level of accurate
            */
            else
            {
                var questions = await Questions(intent.IntentId);

                if (questions is null)
                {
                    // Save to db
                    await SaveUnkownQuestions(new UnkownQuestions { Question = userInput });
                    return answers;
                }

                Question? closestMatchQuestion = new Question();

                closestMatchQuestion = questions.Where(x => x.Text.ToLower().Contains(searchQuery.ToLower())).FirstOrDefault();

                if (closestMatchQuestion is null)
                    return answers;


                answersFromDb = await Answers(closestMatchQuestion.Id);
            }

            return answersFromDb;
        }

        /// <summary>
        /// Add unkown answers to database
        /// </summary>
        /// <param name="questions">String question</param>
        /// <returns>
        /// Success : question id, Else : null
        /// </returns>
        public async Task<int?> SaveUnkownQuestions(UnkownQuestions questions)
        {
            if (string.IsNullOrEmpty(questions?.Question?.Trim()))
                return null;

            return await _chatBotRepo.SaveUnkownQuestions(questions);
        }

        /// <summary>
        /// Add unkown answers to database
        /// </summary>
        /// <param name="questions">String question</param>
        /// <returns>
        /// Success : null, Else : Erorr message
        /// </returns>
        public async Task<string?> UpdateUnkownQuestionsAnswer(UnkownQuestions questions) 
        {
            if (string.IsNullOrEmpty(questions?.Answer?.Trim()))
                return null;

            return await _chatBotRepo.UpdateUnkownQuestionsAnswer(questions);
        }

		/// <summary>
		/// Delete Unknown Question
		/// </summary>
		/// <param name="questionId"></param>
		/// <returns></returns>
		public async Task<string?> DeleteUnknownQuestion(int questionId)
		{
			if (questionId == 0)
				return null;

			return await _chatBotRepo.DeleteUnknownQuestionAsync(questionId);
		}

		#region Helpers

		/// <summary>
		/// Get all intents with keywords
		/// </summary>
		/// <returns></returns>
		private async Task<List<Intent>> IntentWithKeywords()
        {
            return await _chatBotRepo.IntentWithKeywords();
        }

        /// <summary>
        /// Get all intents keywords
        /// Decending by length
        /// </summary>
        /// <returns></returns>
        private async Task<List<UserIntent>> UserIntents()
        {
           return await _chatBotRepo.UserIntents();
        }

        /// <summary>
        /// Question for intent id
        /// </summary>
        /// <param name="intentId">Intent type id</param>
        /// <returns></returns>
        private async Task<List<Question>> Questions(long intentId)
        {
            return await _chatBotRepo.Questions(intentId);
        }

        /// <summary>
        /// Get answers for a question
        /// </summary>
        /// <param name="questionId">Unique question id</param>
        /// <returns></returns>
        private async Task<List<BotAnswer>> Answers(long questionId) 
        {
            if (questionId == default)
                return new List<BotAnswer>();

            return await _chatBotRepo.Answers(questionId);
        }

        /// <summary>
        /// Get answers for a question
        /// </summary>
        /// <param name="questionId">Unique question id</param>
        /// <returns></returns>
        private async Task<List<BotAnswer>> AnswersFromKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return new List<BotAnswer>();

            return await _chatBotRepo.AnswersFromKeyword(keyword);
        }

        /// <summary>
        /// What user looking for?
        /// Example : What is sigiriya
        ///           "What is" -> Intent of find something
        ///           find for -> "Sigiriya"
        ///           return "Sigiriya"
        ///           
        ///           "Hello" -> Intent of greeting
        ///           There is no question
        ///           Return "Hello"
        /// </summary>
        /// <param name="userInput">User insert string</param>
        /// <param name="userIntentKewords">List of intents</param>
        /// <returns></returns>
        private string GetQuery(string userInput, List<UserIntent> userIntentKewords, out List<string> intentWords) 
        {
            userInput = userInput.ToLower();

            // Intent words, Like = "Hello", "Search for"
            List<string> queryWords = new List<string>();
			var input_words = userInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (input_words.Length > 0)
            {
                foreach (var word in userIntentKewords)
                {
                    if (input_words[0] == word.Keyword)
                    {
                        // Add keywords to list
                        queryWords.Add(word.Keyword);

                        break;
                    }
                }
            }
            // Set out pare list
            intentWords = queryWords;

            // No intent found? Return same user input
            return userInput;
        }

        /// <summary>
        /// Get user real intent
        /// </summary>
        /// <param name="userIntentKewords">Intent keywords</param>
        /// <param name="queryWords">Filtered intent words</param>
        /// <param name="searchQuery">What user looking for</param>
        /// <returns></returns>
        private UserIntent? GetUserIntent(List<UserIntent> userIntentKewords, List<string> queryWords, string searchQuery) 
        {
            #pragma warning disable CS8600

            UserIntent intent = new UserIntent();

            if (queryWords.Any())
                intent = userIntentKewords.Where(x => x.Keyword.Contains(queryWords.First())).FirstOrDefault();

            if (intent is null || !queryWords.Any())
                intent = userIntentKewords.Where(x => x.Keyword.Contains(searchQuery)).FirstOrDefault();

            #pragma warning restore CS8600

            return intent;
        }

        #endregion
    }
}

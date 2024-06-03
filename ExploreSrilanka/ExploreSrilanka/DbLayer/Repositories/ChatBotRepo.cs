using ExploreSrilanka.DbLayer.Domains;
using ExploreSrilanka.DbLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploreSrilanka.DbLayer.Repositories
{
    public class ChatBotRepo : IChatBotRepo
    {
        private readonly ChatBotContext _context;

        public ChatBotRepo(ChatBotContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all intents with keywords
        /// </summary>
        /// <returns></returns>
        public async Task<List<Intent>> IntentWithKeywords()
        {
            var list = await _context.Intents.Include(x => x.UserIntents).ToListAsync();

            if (list == null)
                return new List<Intent>();

            return list;
        }

        /// <summary>
        /// Get all intents keywords
        /// Decending by length
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserIntent>> UserIntents()
        {
            var list = await _context.UserIntents.OrderByDescending(x => x.Keyword.Length).ToListAsync();

            if (list == null)
                return new List<UserIntent>();

            return list;
        }

        /// <summary>
        /// Question for intent id
        /// </summary>
        /// <param name="intentId">Intent type id</param>
        /// <returns></returns>
        public async Task<List<Question>> Questions(long intentId)
        {
            var list = await _context.Questions.Where(x => x.IntentId == intentId).ToListAsync();

            if (list == null)
                return new List<Question>();

            return list;
        }

        /// <summary>
        /// Get answers for a question
        /// </summary>
        /// <param name="questionId">Unique question id</param>
        /// <returns></returns>
        public async Task<List<BotAnswer>> Answers(long questionId)
        {
            var list = await _context.BotAnswers.Where(x => x.QuestionId == questionId).ToListAsync();

            if (list == null)
                return new List<BotAnswer>();

            return list;
        }

        /// <summary>
        /// Get answers for a keyword
        /// </summary>
        /// <param name="keyword">Some matching words</param>
        /// <returns></returns>
        public async Task<List<BotAnswer>> AnswersFromKeyword(string keyword)
        {
			// Check if the keyword is related to "Current Time"
			if (keyword.ToLower().Contains("current time") || keyword.ToLower().Contains("time now") || keyword.ToLower().Contains("time now?") || keyword.ToLower().Contains("time?"))
			{
				var currentTimeAnswer = new BotAnswer
				{
                    AnswerTypeId =1,
					Text = $"The current time is {DateTime.Now.ToString("hh:mm tt")}"
				};

				return new List<BotAnswer> { currentTimeAnswer };
			}

			var list = await _context.Questions.Where(x => x.Text.ToLower().Contains(keyword.ToLower()))
                                               .Include(x => x.BotAnswers)
                                               .SelectMany(x => x.BotAnswers)
                                               .ToListAsync();

			// If no answers found, query the UnkownQuestions table
			if (list == null || !list.Any())
			{
				var unknownQuestions = await _context.UnkownQuestions
													 .Where(x => x.Question.ToLower().Contains(keyword.ToLower()))
													 .ToListAsync();

				if (unknownQuestions.Any())
				{
					var unknownAnswers = unknownQuestions.Select(uq => new BotAnswer
					{
						AnswerTypeId = 1,
						Text = uq.Answer
					}).ToList();

					return unknownAnswers;
				}

				return new List<BotAnswer>();
			}

            // If list has more than one answer, randomly select one
            if (list.Count > 1)
            {
               var random = new Random();
               var selectedAnswer = list[random.Next(list.Count)];
               return new List<BotAnswer> { selectedAnswer };
            }

            return list;
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
            try 
            {
                await _context.UnkownQuestions.AddAsync(questions);
                await _context.SaveChangesAsync();
                return questions.Id;
            }
            catch(Exception) 
            {
                return null;
            }
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
            var found = await _context.UnkownQuestions.Where(x => x.Id == questions.Id).FirstOrDefaultAsync();

            if (found == null)
                return "Can not found record";

            found.Answer = questions.Answer;

            try
            {
                await _context.SaveChangesAsync();
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

		/// <summary>
		/// Delete an unknown question from the database
		/// </summary>
		/// <param name="questionId">The ID of the question to delete</param>
		/// <returns>
		/// Success : null, Else : Error message
		/// </returns>
		public async Task<string?> DeleteUnknownQuestionAsync(int questionId)
		{
			var found = await _context.UnkownQuestions.FirstOrDefaultAsync(x => x.Id == questionId);

			if (found == null)
			{
				return "Record not found";
			}

			try
			{
				_context.UnkownQuestions.Remove(found);
				await _context.SaveChangesAsync();
				return null;
			}
			catch (Exception ex)
			{
				// Log the exception message if logging is set up
				return $"Error deleting record: {ex.Message}";
			}
		}


	}
}

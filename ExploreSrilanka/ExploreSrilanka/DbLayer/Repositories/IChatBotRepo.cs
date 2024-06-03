using ExploreSrilanka.DbLayer.Domains;

namespace ExploreSrilanka.DbLayer.Repositories
{
    public interface IChatBotRepo
    {
        /// <summary>
        /// Get all intents with keywords
        /// </summary>
        /// <returns></returns>
        Task<List<Intent>> IntentWithKeywords();

        /// <summary>
        /// Get all intents keywords
        /// Decending by length
        /// </summary>
        /// <returns></returns>
        Task<List<UserIntent>> UserIntents();

        /// <summary>
        /// Question for intent id
        /// </summary>
        /// <param name="intentId">Intent type id</param>
        /// <returns></returns>
        Task<List<Question>> Questions(long intentId);

        /// <summary>
        /// Get answers for a question
        /// </summary>
        /// <param name="questionId">Unique question id</param>
        /// <returns></returns>
        Task<List<BotAnswer>> Answers(long questionId);

        /// <summary>
        /// Get answers for a keyword
        /// </summary>
        /// <param name="keyword">Some matching words</param>
        /// <returns></returns>
        Task<List<BotAnswer>> AnswersFromKeyword(string keyword);

        /// <summary>
        /// Add unkown answers to database
        /// </summary>
        /// <param name="questions">String question</param>
        /// <returns>
        /// Success : question id, Else : null
        /// </returns>
        Task<int?> SaveUnkownQuestions(UnkownQuestions questions);

        /// <summary>
        /// Update unkown answers to database
        /// </summary>
        /// <param name="questions">String question</param>
        /// <returns>
        /// Success : null, Else : Erorr message
        /// </returns>
        Task<string?> UpdateUnkownQuestionsAnswer(UnkownQuestions questions);

		/// <summary>
		/// Delete unkown questions from database
		/// </summary>
		/// <param name="questionId"></param>
		/// <returns></returns>
		Task<string?> DeleteUnknownQuestionAsync(int questionId);

	}
}
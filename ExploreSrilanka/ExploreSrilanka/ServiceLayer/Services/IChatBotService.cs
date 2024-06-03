using ExploreSrilanka.DbLayer.Domains;

namespace ExploreSrilanka.ServiceLayer.Services
{
    public interface IChatBotService
    {
        /// <summary>
        /// Get user input and return a response
        /// </summary>
        /// <param name="userInput">Unser input in string</param>
        /// <returns>
        /// Success : Answer , Else : Error messege
        /// </returns>
        Task<List<BotAnswer>> GetResponse(string userInput);

        /// <summary>
        /// Add unkown answers to database
        /// </summary>
        /// <param name="questions">String question</param>
        /// <returns>
        /// Success : question id, Else : null
        /// </returns>
        Task<int?> SaveUnkownQuestions(UnkownQuestions questions);

        /// <summary>
        /// Add unkown answers to database
        /// </summary>
        /// <param name="questions">String question</param>
        /// <returns>
        /// Success : null, Else : Erorr message
        /// </returns>
        Task<string?> UpdateUnkownQuestionsAnswer(UnkownQuestions questions);

		/// <summary>
		/// Delete Unknown Question
		/// </summary>
		/// <param name="questionId"></param>
		/// <returns></returns>
		Task<string?> DeleteUnknownQuestion(int questionId);

	}
}
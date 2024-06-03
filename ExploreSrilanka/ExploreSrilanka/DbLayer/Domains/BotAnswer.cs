namespace ExploreSrilanka.DbLayer.Domains
{
    public class BotAnswer
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BotAnswer()
        {
            Text       = string.Empty;
            Question   = new Question();
            AnswerType = new AnswerType();
        }

        /// <summary>
        /// Pk
        /// </summary>
        public int Id                { get; set; }

        /// <summary>
        /// Question id Fk
        /// </summary>
        public int QuestionId        { get; set; }

        /// <summary>
        /// Answer type id Fk
        /// </summary>
        public int AnswerTypeId      { get; set; }

        /// <summary>
        /// Answer in text
        /// </summary>
        public string Text           { get; set; }

        /// <summary>
        /// Image if exits
        /// </summary>
        public byte[]? Image         { get; set; }

        /// <summary>
        /// Is this response in html
        /// </summary>
        public bool IsHtml           { get; set; }

        /// <summary>
        /// Response as buttons?
        /// </summary>
        public bool IsButton         { get; set; }

        /// <summary>
        /// Question for this answer
        /// </summary>
        public Question Question     { get; set; }

        /// <summary>
        /// Asnwer type for this answer
        /// </summary>
        public AnswerType AnswerType { get; set; }
    }
}

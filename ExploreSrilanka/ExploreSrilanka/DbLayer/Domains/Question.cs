namespace ExploreSrilanka.DbLayer.Domains
{
    public class Question
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Question()
        {
            Text       = string.Empty;
            BotAnswers = new List<BotAnswer>();
            Intent     = new Intent();
        }

        /// <summary>
        /// Pk
        /// </summary>
        public int Id                     { get; set; }

        /// <summary>
        /// Intent type id Fk
        /// </summary>
        public int IntentId               { get; set; }

        /// <summary>
        /// Question in text
        /// </summary>
        public string Text                { get; set; }

        /// <summary>
        /// Answer/Asnwers for this question
        /// </summary>
        public List<BotAnswer> BotAnswers { get; set; }

        /// <summary>
        /// Intent for this question
        /// </summary>
        public Intent Intent              { get; set; }
    }
}

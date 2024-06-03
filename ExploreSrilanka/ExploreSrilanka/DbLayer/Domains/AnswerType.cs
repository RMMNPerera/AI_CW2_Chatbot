namespace ExploreSrilanka.DbLayer.Domains
{
    public class AnswerType
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AnswerType()
        {
            Name       = string.Empty;
            BotAnswers = new List<BotAnswer>();
        }

        /// <summary>
        /// Pk
        /// </summary>
        public int Id                     { get; set; }

        /// <summary>
        /// Type name
        /// </summary>
        public string Name                { get; set; }

        /// <summary>
        /// List of answer for this type
        /// </summary>
        public List<BotAnswer> BotAnswers { get; set; }
    }
}

namespace ExploreSrilanka.DbLayer.Domains
{
    public class UnkownQuestions
    {
        public UnkownQuestions()
        {
            Question = string.Empty;
        }

        /// <summary>
        /// Pk
        /// </summary>
        public int Id          { get; set; }

        /// <summary>
        /// User question
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// User answer
        /// </summary>
        public string? Answer  { get; set; }
    }
}

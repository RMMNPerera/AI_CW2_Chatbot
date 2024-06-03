namespace ExploreSrilanka.DbLayer.Domains
{
    public class Intent
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Intent()
        {
            Name        = string.Empty;
            Questions   = new List<Question>();
            UserIntents = new List<UserIntent>();
        }

        /// <summary>
        /// Pk
        /// </summary>
        public int Id                       { get; set; }

        /// <summary>
        /// Intent name
        /// </summary>
        public string Name                  { get; set; }

        /// <summary>
        /// Question for this intent
        /// </summary>
        public List<Question> Questions     { get; set; }

        /// <summary>
        /// Keywords for this intent
        /// </summary>
        public List<UserIntent> UserIntents { get; set; }
    }
}

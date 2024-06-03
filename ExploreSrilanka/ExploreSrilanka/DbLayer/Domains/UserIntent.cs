namespace ExploreSrilanka.DbLayer.Domains
{
    public class UserIntent
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UserIntent()
        {
            Keyword = string.Empty;
            Intent  = new Intent();
        }

        /// <summary>
        /// Pk
        /// </summary>
        public int Id         { get; set; }

        /// <summary>
        /// Intent id fk
        /// </summary>
        public int IntentId   { get; set; }

        /// <summary>
        /// Keyword that match with a intent
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// Intent for this keyword
        /// </summary>
        public Intent Intent  { get; set; }
    }
}

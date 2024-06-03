namespace ExploreSrilanka.Models.ChatBot
{
    public class ChatBotButton
    {
        /// <summary>
        /// Defalut constructor
        /// </summary>
        /// <param name="buttonName">Name for button</param>
        /// <param name="value">Value id needed</param>
        /// <param name="classes">Button classes</param>
        /// <param name="href">Href url</param>
        /// <param name="id">Button id</param>
        public ChatBotButton(string buttonName, string classes,
                             string? value = null, string? href = null,
                             string? id = null)
        {
            Name      = buttonName;
            Value     = value;
            className = classes;
            hrefUrl   = href;
            buttonId  = id;
        }

        /// <summary>
        /// Button name
        /// </summary>
        public string Name      { get; set; }

        /// <summary>
        /// Button value if needed
        /// </summary>
        public string? Value    { get; set; }

        /// <summary>
        /// Class names
        /// Ex = 'btn btn-primary'
        /// </summary>
        public string className { get; set; }

        /// <summary>
        /// Need to redirect after clicked?
        /// </summary>
        public string? hrefUrl  { get; set; }

        /// <summary>
        /// Button unique id
        /// </summary>
        public string? buttonId { get; set; }
    }
}

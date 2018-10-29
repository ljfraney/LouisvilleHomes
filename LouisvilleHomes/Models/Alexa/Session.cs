namespace LouisvilleHomes.Models.Alexa
{
    public class Session
    {
        public bool New { get; set; }

        public string SessionId { get; set; }

        public Application Application { get; set; }

        public Attributes Attributes { get; set; }

        public User User { get; set; }

        public Session()
        {
            Application = new Application();
            Attributes = new Attributes();
            User = new User();
        }
    }
}
namespace RBTemplate.Infra.CrossCutting.Identity.SendEmail
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
namespace RBTemplate.Infra.CrossCutting.Identity.Authorization
{
    public class TokenDescriptor
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int MinutesValid { get; set; }
        public int MinutesRefreshTokenValid { get; set; }
    }
}

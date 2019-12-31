using System;
using System.Collections.Generic;
using System.Text;

namespace RBTemplate.Infra.CrossCutting.Identity.Authorization
{
    public class RefreshTokenData
    {
        public string UsuarioId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}

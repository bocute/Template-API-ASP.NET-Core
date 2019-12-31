using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RBTemplate.Infra.CrossCutting.Identity.Models.AccountVIewModel
{
    public class LoginFacebookViewModel
    {
        [Required(ErrorMessage = "O ID do facebook deve ser informado")]
        public long? FacebookId { get; set; }
        [Required(ErrorMessage = "O nome deve ser informado")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O e-mail deve ser informado")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O token de acsso do facebook deve ser informado")]
        public string AccessToken { get; set; }
        public string PictureUrl { get; set; }
        public string TipoUsuario { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ymagi.Infra.CrossCutting.Identity.Models.AccountVIewModel
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "A senha e a senha de confirmação não coincidem.")]
        public string SenhaConfirmacao { get; set; }

        public string Code { get; set; }
    }
}

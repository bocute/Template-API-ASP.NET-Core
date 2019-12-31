using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RBTemplate.Infra.CrossCutting.Identity.Models.AccountVIewModel
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        [StringLength(100, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.", MinimumLength = 8)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [Display(Name = "Confirme a senha")]
        [Compare("Senha", ErrorMessage = "A senha e a confirmação de senha não combinam.")]
        public string SenhaConfirmacao { get; set; }
    }
}

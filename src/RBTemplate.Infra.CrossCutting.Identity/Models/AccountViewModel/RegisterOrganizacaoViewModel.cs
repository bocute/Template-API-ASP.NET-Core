using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ymagi.Infra.CrossCutting.Identity.Models.AccountVIewModel
{
    public class RegisterOrganizacaoViewModel
    {
        [Required(ErrorMessage = "A Razão Social é obrigatória.")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "O nome fantasia é obrigatório.")]
        public string NomeFantasia { get; set; }

        public string CNPJ { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Deve ser informado se a organização é um movimento.")]
        public bool OrganizacaoMovimento { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        [StringLength(100, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.", MinimumLength = 6)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [Display(Name = "Confirme a senha")]
        [Compare("Senha", ErrorMessage = "A senha e a confirmação de senha não combinam.")]
        public string SenhaConfirmacao { get; set; }        
    }
}

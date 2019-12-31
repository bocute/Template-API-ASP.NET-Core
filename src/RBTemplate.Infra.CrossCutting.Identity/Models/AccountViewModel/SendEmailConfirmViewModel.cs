using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RBTemplate.Infra.CrossCutting.Identity.Models.AccountVIewModel
{
    public class SendEmailConfirmViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A url é obrigatória")]
        public string Url { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RBTemplate.Infra.CrossCutting.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public long? FacebookId { get; set; }
        public string PictureUrl { get; set; }
    }
}

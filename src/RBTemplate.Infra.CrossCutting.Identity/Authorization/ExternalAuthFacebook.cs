using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RBTemplate.Infra.CrossCutting.Identity.Authorization
{
    public class ExternalAuthFacebook
    {
        [Required]
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [Required]
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}

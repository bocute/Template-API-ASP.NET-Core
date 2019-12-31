using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RBTemplate.Infra.CrossCutting.Identity.Authorization
{
    public class ExternalAuthFacebookTokenValidation
    {
        [JsonProperty("data")]
        public ExternalAuthFacebookTokenData Data { get; set; }
    }
}
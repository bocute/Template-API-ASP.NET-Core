using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ymagi.Infra.CrossCutting.Identity.Authorization
{
    public class ExternalAuthFacebookTokenData
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("application")]
        public string Application { get; set; }
        [JsonProperty("data_access_expires_at")]
        public int DataAccessExpires { get; set; }
        [JsonProperty("expires_at")]
        public int Expires { get; set; }
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiscordBot.Models
{
    public class G2GResponse
    {
        public int Code { get; set; }

        public Payload Payload { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> OtherProps { get; set; }
    }

    public class Payload
    {
        public List<Result> Results { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> OtherProps { get; set; }
    }

    public class Result
    {
        [JsonPropertyName("converted_unit_price")]
        public decimal Converted_Unit_Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [JsonExtensionData]
        public Dictionary<JToken, JToken> OtherProps { get; set; }
    }
}


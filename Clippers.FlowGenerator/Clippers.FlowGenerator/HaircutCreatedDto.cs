using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clippers.FlowGenerator
{
    public class HaircutIdWrapper
    {
        [JsonPropertyName("haircutId")]
        public string HaircutId { get; set; } 
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardRat.Common.Models
{
    public class ExecuteCodeDto
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("assemblies")]
        public string[] Assemblies { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OSMRouting
{
    public class Node
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

		[JsonPropertyName("lat")]
		public double lat {  get; set; }

		[JsonPropertyName("lon")]
		public double lon { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OSMRouting
{
	public class Way
	{
		[JsonPropertyName("id")]
		public long Id { get; set; }
		public List<Node> Nodes { get; set; }
	}
}

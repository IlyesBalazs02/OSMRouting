using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSMRouting
{
	public class Way
	{
		public long Id { get; set; }
		public List<Node> Nodes { get; set; }
	}
}

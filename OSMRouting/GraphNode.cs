using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSMRouting
{
	public class GraphNode
	{
        public Dictionary<Node, double> Neighbours { get; set; }

        public GraphNode()
        {
            Neighbours = new Dictionary<Node, double>(); 
        }
    }
}

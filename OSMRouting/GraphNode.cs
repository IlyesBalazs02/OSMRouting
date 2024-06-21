using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSMRouting
{
	public class GraphNode
	{
		public Node nodePointer { get; private set; }

		public Dictionary<Node, double> Neighbours { get; set; }

        public GraphNode(Node nodePointer)
        {
            this.nodePointer = nodePointer;
            Neighbours = new Dictionary<Node, double>();
        }
    }
}

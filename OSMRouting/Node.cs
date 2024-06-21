using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OSMRouting
{
    public class Node : GraphNode
    {
		public Node(long id, double lat, double lon)
		{
			Id = id;
			Lat = lat;
			Lon = lon;
		}

		public long Id { get; set; }

		public double Lat {  get; set; }

		public double Lon { get; set; }
    }
}

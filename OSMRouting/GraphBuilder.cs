using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OSMRouting
{
	public class GraphBuilder
	{
		private List<Node> nodeList;
		private List<Way> wayList;
		public GraphBuilder(string jsonResponse)
		{
			nodeList = new List<Node>();
			wayList = new List<Way>();
			ProcessJsonResponse(jsonResponse);
		}

		public void ProcessJsonResponse(string jsonResponse)
		{

			JObject jsonObject = JsonConvert.DeserializeObject<JObject>(jsonResponse);
			JArray elements = jsonObject["elements"] as JArray;

			// Process nodes
			foreach (JObject element in elements)
			{
				string type = element["type"].ToString();
				if (type == "node")
				{
					long id = element["id"].Value<long>();
					double lat = element["lat"].Value<double>();
					double lon = element["lon"].Value<double>();

					nodeList.Add(new Node(id, lat, lon));
				}
			}

			// Process ways
			foreach (JObject element in elements)
			{
				string type = element["type"].ToString();
				if (type == "way")
				{
					long id = element["id"].Value<long>();
					Way way = new Way(id);

					JArray nodesArray = element["nodes"] as JArray;
					foreach (JValue nodeElement in nodesArray)
					{
						long nodeId = nodeElement.Value<long>();
						Node node = nodeList.Find(n => n.Id == nodeId);
						if (node != null)
						{
							way.Nodes.Add(node);
						}
					}

					wayList.Add(way);
				}
			}

			
		}
	}

}

﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OSMRouting
{
	public class GraphBuilder
	{
		private List<Node> nodeList;
		private List<Way> wayList;
		private List<GraphNode> graphNodeList;
		public GraphBuilder(string jsonResponse)
		{
			nodeList = new List<Node>();
			wayList = new List<Way>();
			graphNodeList = new List<GraphNode>();
			ProcessJsonResponse(jsonResponse);
			CreateGraph(SelectGraphNodes());
		}

		public List<GraphNode> GraphNodeList()
		{
			return graphNodeList;
		}

		//TODO Skip nodes that appear twice
		//TODO Use this CalculateDistance method only at the end when the final route is returned, until then use something that requires less calculation
		private void CreateGraph(List<Node> tmp)
		{
			double distanceBetweenNodes = 0;
			double prevLat;
			double prevLon;

			foreach (var way in wayList)
			{
				Node prev = null;
				prevLat = way.Nodes[0].Lat;
				prevLon = way.Nodes[0].Lon;
				foreach (var node in way.Nodes)
				{
					distanceBetweenNodes += CalculateDistance(prevLat, prevLon, node.Lat, node.Lon);
					prevLat = node.Lat;
					prevLon = node.Lon;

					//the node is one of the nodes of the graph
					if (tmp.Contains(node))
					{
						//if it's not the first element of the way collection
						if (prev != null)
						{
							if (!graphNodeList.Contains(node)) graphNodeList.Add(node);

							node.Neighbours.Add(prev, distanceBetweenNodes);
							prev.Neighbours.Add(node, distanceBetweenNodes);
							distanceBetweenNodes = 0;
						}
						else
						{
							//if it's the first element 
							prev = node;
							if (!graphNodeList.Contains(prev)) graphNodeList.Add(prev);
						}
					}
				}

			}

			foreach (var node in graphNodeList)
			{
                Console.WriteLine((node as Node).Id);
				foreach(var item in node.Neighbours)
				{
                    Console.WriteLine("\t" + item.Key.Id + "   " + item.Value);
                }
			}

		}

		//return all the nodes which form the graph, which will be:
			// the nodes that appear more than once meaning there is multiple way goin from that node
			// the first and last nodes of a way collection (end of the roads/dead ends)
		private List<Node> SelectGraphNodes()
		{
			Dictionary<Node, int> nodeCounts = new Dictionary<Node, int>();

			foreach (var item in wayList)
			{
				if (!nodeCounts.ContainsKey(item.Nodes.First())) nodeCounts[item.Nodes.First()] = 1;
				if (!nodeCounts.ContainsKey(item.Nodes.Last())) nodeCounts[item.Nodes.Last()] = 1;

				//counts how many times do each node appear in the way collections
				foreach (var node in item.Nodes)
				{

					if (nodeCounts.ContainsKey(node))
					{
						nodeCounts[node]++;
					}
					else
					{
						nodeCounts[node] = 1;
					}

				}
			}

			//the result
			var result = nodeCounts.Where(kvp => kvp.Value >= 2).Select(kvp => kvp.Key).ToList();


			return result;
		}

		private void ProcessJsonResponse(string jsonResponse)
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



		public const double RADIUS_OF_EARTH = 6371; // Earth's radius in kilometers

		//TODO: Only use this if the route is already found

		public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
		{
			// Convert latitude and longitude from degrees to radians
			double lat1Radians = DegreesToRadians(lat1);
			double lon1Radians = DegreesToRadians(lon1);
			double lat2Radians = DegreesToRadians(lat2);
			double lon2Radians = DegreesToRadians(lon2);

			// Calculate differences between latitudes and longitudes
			double latDiff = lat2Radians - lat1Radians;
			double lonDiff = lon2Radians - lon1Radians;

			// Haversine formula
			double a = Math.Pow(Math.Sin(latDiff / 2), 2) +
					   Math.Cos(lat1Radians) * Math.Cos(lat2Radians) *
					   Math.Pow(Math.Sin(lonDiff / 2), 2);
			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			// Calculate distance
			double distanceInKilometers = RADIUS_OF_EARTH * c;
			var distanceInMeters = distanceInKilometers * 1000;
			return distanceInMeters;
		}

		private static double DegreesToRadians(double degrees)
		{
			return degrees * Math.PI / 180.0;
		}
	}

}

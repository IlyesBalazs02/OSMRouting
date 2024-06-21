﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
		public GraphBuilder(string jsonResponse)
		{
			nodeList = new List<Node>();
			wayList = new List<Way>();
			ProcessJsonResponse(jsonResponse);
			getTheNodes();
		}

		private void getTheNodes()
		{
			Dictionary<Node, int> nodeCounts = new Dictionary<Node, int>();

			//intersections
			foreach (var item in wayList)
			{
				var a = item.Nodes.First();
				
				//First and Last nodes of the list
				if (!nodeCounts.ContainsKey(item.Nodes.First())) nodeCounts[item.Nodes.First()] = 1;
				if (!nodeCounts.ContainsKey(item.Nodes.Last())) nodeCounts[item.Nodes.Last()] = 1;

				//intersections
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

			var result = nodeCounts.Where(kvp => kvp.Value >= 2).Select(kvp => kvp.Key).ToList();

			foreach (var node in result)
			{
				Console.WriteLine(node.Id);
			}
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

			foreach (var way in wayList)
			{
				way.Distance = calculateWayDistance(way);
			}

		}

		private double calculateWayDistance(Way way)
		{
			double sum = 0;
			Node tmp = way.Nodes[0];
			foreach (var node in way.Nodes)
			{
				sum += CalculateDistance(tmp.Lat, tmp.Lon, node.Lat, node.Lon);
			}

			return sum;
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

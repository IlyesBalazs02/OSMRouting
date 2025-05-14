namespace OSMRouting
{
	public class Program
	{
		static void Main(string[] args)
		{
			//double maxLat = 47.71424;
			//double maxLon = 15.80773;
			//double minLat = 47.70812;
			//double minLon = 15.80381;

			//Way: Szőkeföldi utca (112539464)
			//TODO Handle these situations
			double maxLat = 47.2498;
			double maxLon = 16.6412;
			double minLat = 47.2146;
			double minLon = 16.6099;

			var jsonResponse = new OsmHttpRequest(minLat, minLon, maxLat, maxLon).getJsonResponse();
			var graph = new GraphBuilder(jsonResponse);

			var graphNodes = graph.GraphNodeList();

			var tree = new KDTree(graphNodes);

			double myLat = 47.4979;
			double myLon = 19.0402;

			GraphNode nearest1 = tree.FindNearest(47.2201, 16.6102);
			GraphNode nearest2 = tree.FindNearest(47.2406, 16.6293);


			var aStar = new AStar(graphNodes);
			//var resultCoordinates = aStar.FindPath(graphNodes.First(), graphNodes.Last()); //TODO work with 2 random coordinate
			var resultCoordinates = aStar.FindPath(graphNodes.Find(t => t.Id == nearest1.Id), graphNodes.Find(t => t.Id == nearest2.Id)); //TODO work with 2 random coordinate

			//foreach ( var coord in resultCoordinates ) 
			//{ 
			//	Console.WriteLine(coord.Lat + " " + coord.Lon);
			//}

            foreach ( var node in graph.ReconstructPath(resultCoordinates))
			{
                Console.WriteLine(node.Lat + " " + node.Lon);
            }

			/*
																		  ;
			var tree = new KdTree(allNodes);

			// your query position
			double myLat = 47.4979;
			double myLon = 19.0402;

			GraphNode nearest = tree.FindNearest(myLat, myLon);
			Console.WriteLine($"Closest node ID is {nearest.Id} at ({nearest.Lat}, {nearest.Lon})");*/

			//Implement a k-d tree so it will be able to create a route between two random coordinates
		}
	}
}




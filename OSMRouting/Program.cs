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

			var aStar = new AStar(graphNodes);
			//var resultCoordinates = aStar.FindPath(graphNodes.First(), graphNodes.Last()); //TODO work with 2 random coordinate
			var resultCoordinates = aStar.FindPath(graphNodes.Find(t => t.Id == 6196523587), graphNodes.Find(t => t.Id == 685881194)); //TODO work with 2 random coordinate

			//foreach ( var coord in resultCoordinates ) 
			//{ 
			//	Console.WriteLine(coord.Lat + " " + coord.Lon);
			//}

            foreach ( var node in graph.ReconstructPath(resultCoordinates))
			{
                Console.WriteLine(node.Lat + " " + node.Lon);
            }

			//Implement a k-d tree so it will be able to create a route between two random coordinates
        }
	}
}




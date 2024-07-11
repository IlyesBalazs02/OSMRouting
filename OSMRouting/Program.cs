namespace OSMRouting
{
	public class Program
	{
		static void Main(string[] args)
		{
			double maxLat = 47.71424;
			double maxLon = 15.80773;
			double minLat = 47.70812;
			double minLon = 15.80381;

			var jsonResponse = new OsmHttpRequest(minLat, minLon, maxLat, maxLon).getJsonResponse();
			var graph = new GraphBuilder(jsonResponse);

			var graphNodes = graph.GraphNodeList();

			var aStar = new AStar(graphNodes);
			//var resultCoordinates = aStar.FindPath(graphNodes.First(), graphNodes.Last()); //TODO work with 2 random coordinate
			var resultCoordinates = aStar.FindPath(graphNodes.First(), graphNodes[67]); //TODO work with 2 random coordinate

			foreach ( var coord in resultCoordinates ) 
			{ 
				Console.WriteLine(coord.Lat + " " + coord.Lon);
			}

			var asd = graph.ReconstructPath(resultCoordinates);
        }
	}
}




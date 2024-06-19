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

			var a = new OsmHttpRequest(minLat, minLon, maxLat, maxLon);
		}
	}
}




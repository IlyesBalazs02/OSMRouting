using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSMRouting
{
	public class OsmHttpRequest
	{
		public string getJsonResponse()
		{
			return jsonResult;
		}


		private string jsonResult;
		public OsmHttpRequest(double minLat, double minLon, double maxLat, double maxLon)
		{
			string requestUrl = "https://overpass-api.de/api/interpreter?data=[out:json];way[%27highway%27](" + minLat + ",%20" + minLon + ",%20" + maxLat + ",%20" + maxLon + ");out%20body;%3E;out%20skel%20qt;";

			Process(requestUrl);
		}

		private void Process(string requestUrl)
		{
			jsonResult = sendHttpRequest(requestUrl).GetAwaiter().GetResult();
		}

		private async Task<string> sendHttpRequest(string requestUrl)
		{
			HttpClient httpClient = new HttpClient();
			string responseBody;
			try
			{
				using HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
				response.EnsureSuccessStatusCode();
				responseBody = await response.Content.ReadAsStringAsync();
			}
			catch (HttpRequestException e)
			{
				throw new HttpRequestException(e.Message);
			}

			return responseBody;
		}

		
	}
}


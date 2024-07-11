using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoutingConsoleApp
{
	public partial class Form1 : Form
	{
		private GMapControl gmap;
		List<PointLatLng> points;

		private GMapOverlay routesOverlay;
		private GMapRoute currentPolyline;
		public Form1()
		{
			InitializeComponent();
			InitializeMap();
		}

		private void InitializeMap()
		{
			// Create a new instance of GMapControl
			gmap = new GMapControl
			{
				Dock = DockStyle.Fill
			};

			// Add the GMapControl to the form's controls
			this.Controls.Add(gmap);

			// Set the map provider to OpenStreetMap
			gmap.MapProvider = GMapProviders.OpenStreetMap;

			// Set the initial position to display (latitude, longitude)
			gmap.Position = new PointLatLng(51.5074, -0.1278); // Example: London

			// Set zoom levels
			gmap.MinZoom = 2;
			gmap.MaxZoom = 18;
			gmap.Zoom = 10;

			// Enable mouse wheel zooming
			gmap.MouseWheelZoomEnabled = true;
			gmap.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;

			// Enable map dragging
			gmap.CanDragMap = true;

			// Enable map markers
			gmap.MarkersEnabled = true;

			// Other settings
			gmap.ShowCenter = false;
			gmap.DragButton = MouseButtons.Right;

			gmap.MouseClick += Gmap_MouseClick;
			points = new List<PointLatLng>()
			{
				new PointLatLng(51.5074, -0.1278), // London
        new PointLatLng(48.8566, 2.3522)
			};
			GMapRoute polyline = new GMapRoute(points, "My Route")
			{
				Stroke = new Pen(Color.Red, 3) // Define color and thickness of the polyline
			};
			routesOverlay = new GMapOverlay("routes"); // Create a new overlay
			routesOverlay.Routes.Add(polyline); // Add the polyline to the overlay
			gmap.Overlays.Add(routesOverlay); // Add the overlay to the map
		}
		private void Gmap_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				PointLatLng point = gmap.FromLocalToLatLng(e.X, e.Y);
				Console.WriteLine("Latitude: {0}, Longitude: {1}", point.Lat, point.Lng);
				points.Add(point);

				RefreshPolyline();

			}
		}
		private void RefreshPolyline()
		{
			// Clear the existing route
			routesOverlay.Routes.Clear();

			// Create a new route with the updated points list
			currentPolyline = new GMapRoute(points, "My Route")
			{
				Stroke = new Pen(Color.Red, 3)
			};

			// Add the new route to the overlay
			routesOverlay.Routes.Add(currentPolyline);

			// Refresh the map
			gmap.Refresh();
		}
	}
}

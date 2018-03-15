using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Net.Http;
using Newtonsoft.Json;

namespace Maps2
{
	public class PinPage : ContentPage
	{
		Map map;

		public PinPage()
		{
			map = new Map
			{
				IsShowingUser = true,
				HeightRequest = 100,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			var radius = "5000";
			var type = "restaurant";
			var apiKey = "{YOUR API KEY}";
			var centerLat = 36.9628066;
			var centerLng = -122.0194722;
			map.MoveToRegion(MapSpan.FromCenterAndRadius(
				new Position(centerLat, centerLng), Distance.FromMiles(3))); // Santa Cruz golf course
			var client = new HttpClient();
			var uri = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location="
				+ centerLat + "," 
				+ centerLng + "&radius=" 
				+ radius + "&type=" 
				+ type + "&key=" 
				+ apiKey;
			// Don't know what the "keyword property does yet
			string obstring = client.GetStringAsync(uri).Result;
			var responses = JsonConvert.DeserializeObject<GoogleResponse>(obstring);
			if (responses.results != null)
			{
				foreach (var response in responses.results)
				{
					var position = new Position(response.geometry.location.lat, response.geometry.location.lng); // Latitude, Longitude
					var pin = new Pin
					{
						Type = PinType.Place,
						Position = position,
						Label = response.name,
						Address = response.vicinity
					};
					map.Pins.Add(pin);
				}
			}
			//var position = new Position(36.9628066, -122.0194722); // Latitude, Longitude
			//var pin = new Pin
			//{
			//	Type = PinType.Place,
			//	Position = position,
			//	Label = "Santa Cruz",
			//	Address = "custom detail info"
			//};
			//map.Pins.Add(pin);


			// create buttons
			var morePins = new Button { Text = "Add more pins" };
			morePins.Clicked += (sender, e) =>
			{
				map.Pins.Add(new Pin
				{
					Position = new Position(36.9641949, -122.0177232),
					Label = "Boardwalk"
				});
				map.Pins.Add(new Pin
				{
					Position = new Position(36.9571571, -122.0173544),
					Label = "Wharf"
				});
				map.MoveToRegion(MapSpan.FromCenterAndRadius(
					new Position(36.9628066, -122.0194722), Distance.FromMiles(1.5)));

			};
			var reLocate = new Button { Text = "Re-center" };
			reLocate.Clicked += (sender, e) =>
			{
				map.MoveToRegion(MapSpan.FromCenterAndRadius(
					new Position(36.9628066, -122.0194722), Distance.FromMiles(3)));
			};
			var buttons = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					morePins, reLocate
				}
			};

			// put the page together
			Content = new StackLayout
			{
				Spacing = 0,
				Children = {
					map,
					buttons
				}
			};
		}
	}
}

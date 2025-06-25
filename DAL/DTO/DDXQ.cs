using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class DDXQ
    {
        public int MaDD { get; set; }
        public int MaCN { get; set; }
        public string TenDD { get; set; }
        public string LoaiDD { get; set; }
        public string DiaChi { get; set; }
        public decimal DanhGia { get; set; }
        public decimal ViDo { get; set; }
        public decimal KinhDo { get; set; }
        public decimal KhoangCach { get; set; }
        public string ThoiGianDiChuyen { get; set; }
        public DateTime ThoiGianCapNhat { get; set; }
    }

	public class RouteData
	{
		public decimal Distance { get; set; } // km
		public string Duration { get; set; } // e.g., "10 mins"
	}

	// Models for Google Places API response
	public class GooglePlacesResponse
	{
		public List<GooglePlace> Results { get; set; }
		public string Status { get; set; }
		public string ErrorMessage { get; set; }
		public string NextPageToken { get; set; }
	}

	public class Place
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("vicinity")]
		public string Vicinity { get; set; }
		[JsonProperty("rating")]
		public decimal? Rating { get; set; }
		[JsonProperty("geometry")]
		public Geometry Geometry { get; set; }
	}

	public class Geometry
	{
		[JsonProperty("location")]
		public Location Location { get; set; }
	}

	public class Location
	{
		[JsonProperty("lat")]
		public decimal Lat { get; set; }
		[JsonProperty("lng")]
		public decimal Lng { get; set; }
	}

	// Models for Google Routes API response
	public class RouteResponse
	{
		[JsonProperty("routes")]
		public List<Route> Routes { get; set; }
	}

	public class Route
	{
		[JsonProperty("distanceMeters")]
		public int? DistanceMeters { get; set; }
		[JsonProperty("duration")]
		public string Duration { get; set; }
	}

	public class GooglePlace
	{
		public string Name { get; set; }
		public string Vicinity { get; set; }
		public decimal? Rating { get; set; }
		public Geometry Geometry { get; set; }
	}
}

using DAL.DTO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
	public class DDXQ_DAL
	{
		private static DDXQ_DAL Instance;
		private readonly string connectionString = Properties.Resources.MySqlConnection;
		private readonly string googleApiKey = Properties.Resources.Google_API_Key;
		private readonly HttpClient httpClient = new() { Timeout = TimeSpan.FromSeconds(30) };

		private DDXQ_DAL() { }

		public static DDXQ_DAL GetInstance()
		{
			return Instance ??= new DDXQ_DAL();
		}

		#region Helpers

		private DDXQ ReadLocation(MySqlDataReader reader) => new()
		{
			MaDD = reader.GetInt32("MaDD"),
			MaCN = reader.GetInt32("MaCN"),
			TenDD = reader.GetString("TenDD"),
			LoaiDD = reader.GetString("LoaiDD"),
			DiaChi = reader.GetString("DiaChi"),
			DanhGia = reader.GetDecimal("DanhGia"),
			ViDo = reader.GetDecimal("ViDo"),
			KinhDo = reader.GetDecimal("KinhDo"),
			KhoangCach = reader.GetDecimal("KhoangCach"),
			ThoiGianDiChuyen = reader.GetString("ThoiGianDiChuyen"),
			ThoiGianCapNhat = reader.GetDateTime("ThoiGianCapNhat")
		};

		private void AddLocationParameters(MySqlCommand cmd, DDXQ location)
		{
			cmd.Parameters.AddWithValue("@MaCN", location.MaCN);
			cmd.Parameters.AddWithValue("@TenDD", location.TenDD);
			cmd.Parameters.AddWithValue("@LoaiDD", location.LoaiDD);
			cmd.Parameters.AddWithValue("@DiaChi", location.DiaChi);
			cmd.Parameters.AddWithValue("@DanhGia", location.DanhGia);
			cmd.Parameters.AddWithValue("@ViDo", location.ViDo);
			cmd.Parameters.AddWithValue("@KinhDo", location.KinhDo);
			cmd.Parameters.AddWithValue("@KhoangCach", location.KhoangCach);
			cmd.Parameters.AddWithValue("@ThoiGianDiChuyen", location.ThoiGianDiChuyen);
		}

		private static readonly Dictionary<string, string> TypeMapping = new()
		{
			{ "restaurant", "Nhà hàng" },
			{ "hotel", "Khách sạn" },
			{ "cafe", "Quán cà phê" },
			{ "bar", "Quán bar" },
			{ "amusement_park", "Công viên giải trí" },
			{ "aquarium", "Thủy cung" },
			{ "bowling_alley", "Sân chơi bowling" },
			{ "casino", "Sòng bạc" },
			{ "movie_theater", "Rạp chiếu phim" },
			{ "museum", "Bảo tàng" },
			{ "night_club", "Câu lạc bộ đêm" },
			{ "park", "Công viên" },
			{ "tourist_attraction", "Điểm tham quan du lịch" },
			{ "zoo", "Sở thú" },
			{ "lodging", "Nhà nghỉ" },
			{ "rv_park", "Khu cắm trại RV" },
			{ "campground", "Khu cắm trại" },
			{ "gym", "Phòng tập thể dục" },
			{ "stadium", "Sân vận động" },
			{ "spa", "Trung tâm spa" }
		};

		#endregion

		#region Async CRUD

		public async Task<List<DDXQ>> GetAllAsync()
		{
			var result = new List<DDXQ>();
			try
			{
				using var conn = new MySqlConnection(connectionString);
				var cmd = new MySqlCommand("SELECT * FROM DiaDiemXungQuanh", conn);
				await conn.OpenAsync();
				using var reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					result.Add(ReadLocation((MySqlDataReader)reader));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[GetAllAsync] {ex.Message}");
			}
			return result;
		}

		public async Task<DDXQ> GetByIdAsync(int id)
		{
			try
			{
				using var conn = new MySqlConnection(connectionString);
				var cmd = new MySqlCommand("SELECT * FROM DiaDiemXungQuanh WHERE MaDD = @MaDD", conn);
				cmd.Parameters.AddWithValue("@MaDD", id);
				await conn.OpenAsync();
				using var reader = await cmd.ExecuteReaderAsync();
				return await reader.ReadAsync() ? ReadLocation((MySqlDataReader)reader) : null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[GetByIdAsync] {ex.Message}");
				return null;
			}
		}

		public async Task<int> CreateAsync(DDXQ data)
		{
			using var conn = new MySqlConnection(connectionString);
			var cmd = new MySqlCommand(@"
                INSERT INTO DiaDiemXungQuanh (MaCN, TenDD, LoaiDD, DiaChi, DanhGia, KinhDo, ViDo, KhoangCach, ThoiGianDiChuyen, ThoiGianCapNhat)
                VALUES (@MaCN, @TenDD, @LoaiDD, @DiaChi, @DanhGia, @KinhDo, @ViDo, @KhoangCach, @ThoiGianDiChuyen, NOW());
                SELECT LAST_INSERT_ID();", conn);
			AddLocationParameters(cmd, data);
			await conn.OpenAsync();
			return Convert.ToInt32(await cmd.ExecuteScalarAsync());
		}

		public async Task<bool> UpdateAsync(int id, DDXQ data)
		{
			using var conn = new MySqlConnection(connectionString);
			var cmd = new MySqlCommand(@"
                UPDATE DiaDiemXungQuanh
                SET MaCN = @MaCN, TenDD = @TenDD, LoaiDD = @LoaiDD, DiaChi = @DiaChi, DanhGia = @DanhGia,
                    KinhDo = @KinhDo, ViDo = @ViDo, KhoangCach = @KhoangCach, ThoiGianDiChuyen = @ThoiGianDiChuyen,
                    ThoiGianCapNhat = NOW()
                WHERE MaDD = @MaDD", conn);
			AddLocationParameters(cmd, data);
			cmd.Parameters.AddWithValue("@MaDD", id);
			await conn.OpenAsync();
			return await cmd.ExecuteNonQueryAsync() > 0;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			using var conn = new MySqlConnection(connectionString);
			var cmd = new MySqlCommand("DELETE FROM DiaDiemXungQuanh WHERE MaDD = @MaDD", conn);
			cmd.Parameters.AddWithValue("@MaDD", id);
			await conn.OpenAsync();
			return await cmd.ExecuteNonQueryAsync() > 0;
		}

		#endregion

		#region Google API

		public async Task<string> FetchAndSaveNearbyLocationsAsync(int branchId, int radius, string type, int limit)
		{
			if (string.IsNullOrEmpty(googleApiKey))
			{
				throw new Exception("Thiếu GOOGLE_API_KEY trong cấu hình");
			}

			if (!TypeMapping.ContainsKey(type))
			{
				throw new Exception($"Loại địa điểm không được hỗ trợ: {type}");
			}

			// Ensure minimum radius to avoid empty results
			radius = Math.Max(radius, 5000); // Minimum 5km

			// Truy vấn tọa độ chi nhánh
			ChiNhanh branch;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT KinhDo, ViDo FROM ChiNhanh WHERE MaCN = @MaCN AND IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCN", branchId);
					await conn.OpenAsync();
					using (var reader = await cmd.ExecuteReaderAsync())
					{
						if (!await reader.ReadAsync())
						{
							throw new Exception($"Không tìm thấy chi nhánh với MaCN = {branchId}");
						}
						branch = new ChiNhanh
						{
							KinhDo = reader.GetDecimal("KinhDo"),
							ViDo = reader.GetDecimal("ViDo")
						};
					}
				}
				// Enhanced coordinate validation
				if (branch.ViDo < -90 || branch.ViDo > 90 || branch.KinhDo < -180 || branch.KinhDo > 180 || (branch.ViDo == 0 && branch.KinhDo == 0))
				{
					throw new Exception($"Tọa độ chi nhánh không hợp lệ: ViDo={branch.ViDo}, KinhDo={branch.KinhDo}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi truy vấn ChiNhanh: {ex.Message}\nStackTrace: {ex.StackTrace}");
				throw new Exception($"Lỗi khi truy vấn chi nhánh với MaCN = {branchId}");
			}

			var origin = $"{branch.ViDo},{branch.KinhDo}";
			var loaiDD = TypeMapping[type];
			Console.WriteLine($"Requesting places for branchId: {branchId}, origin: {origin}, radius: {radius}, type: {type}, loaiDD: {loaiDD}");

			// Collect all results with pagination
			var allResults = new List<GooglePlace>();
			string nextPageToken = null;

			try
			{
				do
				{
					var url = nextPageToken == null
						? $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={origin}&radius={radius}&type={type}&language=vi&key={googleApiKey}"
						: $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?pagetoken={nextPageToken}&key={googleApiKey}";
					Console.WriteLine($"Google Places API URL: {url}");

					var response = await httpClient.GetAsync(url);
					response.EnsureSuccessStatusCode();
					var content = await response.Content.ReadAsStringAsync();
					Console.WriteLine($"Google Places API Response: {content}");

					var placesResponse = JsonConvert.DeserializeObject<GooglePlacesResponse>(content);

					// Check API status
					if (placesResponse?.Status != "OK" && placesResponse?.Status != "ZERO_RESULTS")
					{
						Console.WriteLine($"API Error: Status={placesResponse?.Status}, ErrorMessage={placesResponse?.ErrorMessage}");
						throw new Exception($"Google Places API error: {placesResponse?.Status} - {placesResponse?.ErrorMessage}");
					}

					if (placesResponse?.Results == null || !placesResponse.Results.Any())
					{
						Console.WriteLine($"No results returned. Status: {placesResponse?.Status}, Error: {placesResponse?.ErrorMessage}");
						return "Không có địa điểm nào được trả về từ Google Places API.";
					}

					allResults.AddRange(placesResponse.Results);
					nextPageToken = placesResponse.NextPageToken;

					if (!string.IsNullOrEmpty(nextPageToken))
					{
						await Task.Delay(2000); // Wait for next_page_token to become valid
					}
				} while (!string.IsNullOrEmpty(nextPageToken) && allResults.Count < limit);

				Console.WriteLine($"Found {allResults.Count} places");
				var limitedPlaces = allResults.Take(limit).ToList();

				// Lấy danh sách bản ghi hiện có
				List<int> existingLocationIds;
				try
				{
					using (var conn = new MySqlConnection(connectionString))
					{
						var query = "SELECT MaDD FROM DiaDiemXungQuanh WHERE MaCN = @MaCN AND LoaiDD = @LoaiDD ORDER BY MaDD ASC";
						var cmd = new MySqlCommand(query, conn);
						cmd.Parameters.AddWithValue("@MaCN", branchId);
						cmd.Parameters.AddWithValue("@LoaiDD", loaiDD);
						await conn.OpenAsync();
						using (var reader = await cmd.ExecuteReaderAsync())
						{
							existingLocationIds = new List<int>();
							while (await reader.ReadAsync())
							{
								existingLocationIds.Add(reader.GetInt32("MaDD"));
							}
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Lỗi khi truy vấn bản ghi hiện có: {ex.Message}\nStackTrace: {ex.StackTrace}");
					throw new Exception("Lỗi khi kiểm tra bản ghi hiện có");
				}

				// Lấy khoảng cách và thời gian di chuyển
				var tasks = limitedPlaces.Select(async (place, index) =>
				{
					if (place.Geometry?.Location == null)
					{
						Console.WriteLine($"Địa điểm {place.Name} thiếu thông tin tọa độ, bỏ qua.");
						return null;
					}
					var destination = $"{place.Geometry.Location.Lat},{place.Geometry.Location.Lng}";
					try
					{
						var routeData = await GetRouteDistanceAndDurationAsync(origin, destination);
						return new { Place = place, RouteData = routeData, Index = index };
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Không thể lấy tuyến đường cho địa điểm {place.Name}: {ex.Message}");
						return null;
					}
				});
				var results = await Task.WhenAll(tasks);
				var validResults = results.Where(r => r != null).ToList();

				// Cập nhật hoặc tạo bản ghi
				for (int i = 0; i < validResults.Count; i++)
				{
					var result = validResults[i];
					var locationData = new DDXQ
					{
						MaCN = branchId,
						TenDD = result.Place.Name,
						LoaiDD = loaiDD,
						DiaChi = result.Place.Vicinity ?? string.Empty,
						DanhGia = result.Place.Rating ?? 0,
						ViDo = result.Place.Geometry.Location.Lat,
						KinhDo = result.Place.Geometry.Location.Lng,
						KhoangCach = result.RouteData.Distance, // Keep in meters for consistency with JS
						ThoiGianDiChuyen = result.RouteData.Duration
					};

					try
					{
						if (i < existingLocationIds.Count)
						{
							Console.WriteLine($"Updating location MaDD={existingLocationIds[i]}: {JsonConvert.SerializeObject(locationData)}");
							await UpdateAsync(existingLocationIds[i], locationData);
						}
						else
						{
							Console.WriteLine($"Creating new location: {JsonConvert.SerializeObject(locationData)}");
							await CreateAsync(locationData);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Lỗi khi xử lý địa điểm {locationData.TenDD}: {ex.Message}\nStackTrace: {ex.StackTrace}");
					}
				}

				return $"Đã xử lý {validResults.Count} địa điểm cho MaCN={branchId} và LoaiDD={loaiDD}";
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"HTTP Error: {ex.Message}, Status: {ex.StatusCode}, Inner: {ex.InnerException?.Message}");
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi gọi Google Places API: {ex.Message}\nStackTrace: {ex.StackTrace}");
				throw;
			}
		}

		// Get distance and duration from Routes API
		private async Task<RouteData> GetRouteDistanceAndDurationAsync(string origin, string destination)
		{
			if (string.IsNullOrEmpty(googleApiKey))
			{
				throw new Exception("Thiếu GOOGLE_API_KEY trong cấu hình");
			}

			var coords = origin.Split(',').Select(x => decimal.TryParse(x, out var result) ? result : (decimal?)null).ToArray();
			var originLat = coords[0];
			var originLng = coords[1];
			coords = destination.Split(',').Select(x => decimal.TryParse(x, out var result) ? result : (decimal?)null).ToArray();
			var destLat = coords[0];
			var destLng = coords[1];

			if (!originLat.HasValue || !originLng.HasValue || !destLat.HasValue || !destLng.HasValue)
			{
				throw new Exception($"Tọa độ không hợp lệ: origin={origin}, destination={destination}");
			}

			var url = "https://routes.googleapis.com/directions/v2:computeRoutes";
			var requestBody = new
			{
				origin = new
				{
					location = new { latLng = new { latitude = originLat, longitude = originLng } }
				},
				destination = new
				{
					location = new { latLng = new { latitude = destLat, longitude = destLng } }
				},
				travelMode = "DRIVE",
				routingPreference = "TRAFFIC_AWARE",
				computeAlternativeRoutes = false,
				languageCode = "en-US",
				units = "METRIC"
			};

			try
			{
				var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
				var request = new HttpRequestMessage(HttpMethod.Post, url)
				{
					Content = content
				};
				request.Headers.Add("X-Goog-Api-Key", googleApiKey);
				request.Headers.Add("X-Goog-FieldMask", "routes.duration,routes.distanceMeters");

				var response = await httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();
				var responseContent = await response.Content.ReadAsStringAsync();
				var routeResponse = JsonConvert.DeserializeObject<RouteResponse>(responseContent);

				if (routeResponse?.Routes == null || !routeResponse.Routes.Any())
				{
					throw new Exception("Không tìm được tuyến đường từ Routes API");
				}

				var route = routeResponse.Routes[0];
				if (route.DistanceMeters == null || route.Duration == null)
				{
					throw new Exception("Thiếu thông tin khoảng cách hoặc thời gian từ Routes API");
				}

				return new RouteData
				{
					Distance = route.DistanceMeters.Value, // Keep in meters
					Duration = route.Duration
				};
			}
			catch (Exception ex)
			{
				var errorMessage = ex is HttpRequestException httpEx && httpEx.InnerException != null
					? $"Lỗi Routes API: {httpEx.Message} - {httpEx.InnerException.Message}"
					: $"Lỗi khi gọi Routes API: {ex.Message}";
				Console.WriteLine(errorMessage);
				throw new Exception(errorMessage);
			}
		}

		#endregion
	}
}


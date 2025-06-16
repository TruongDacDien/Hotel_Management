using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace DAL.Data
{
	public class CloudinaryService
	{
		private readonly Cloudinary _cloudinary;

		public CloudinaryService()
		{
			var account = new Account(Properties.Resources.Cloudinary_Cloud_Name, Properties.Resources.Cloudinary_API_Key, Properties.Resources.Cloudinary_API_Secret);
			_cloudinary = new Cloudinary(account) { Api = { Secure = true } };
		}

		public async Task<string> UploadImageAsync(string filePath, string publicId)
		{
			var uploadParams = new ImageUploadParams
			{
				File = new FileDescription(filePath),
				Folder = "hotel_management",        // ← thư mục trên Cloudinary
				PublicId = publicId,                // tên file (không chứa đường dẫn)
				Overwrite = true
			};

			var result = await _cloudinary.UploadAsync(uploadParams);
			return result.SecureUrl.ToString();
		}

		public async Task<bool> DeleteImageAsync(string publicId)
		{
			try
			{
				var deletionParams = new DeletionParams(publicId);
				var result = await _cloudinary.DestroyAsync(deletionParams);

				// Kiểm tra kết quả
				return result.Result == "ok";
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}

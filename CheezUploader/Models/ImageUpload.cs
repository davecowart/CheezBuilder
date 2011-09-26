using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace CheezUploader.Models {
	public class ImageUpload {

		public ImageUpload() { }
		public ImageUpload(string filename) {
			Load(filename);
		}

		readonly IEnumerable<string> _allowedExtensions = new string[] { ".png", ".jpg", ".jpeg", ".gif" };

		[Required]
		public string Filename { get; set; }
		public string OriginalPath { get; set; }
		public string ResizedPath { get; set; }
		public string DisplayPath { get { return ResizedPath ?? OriginalPath; } }

		private void Load(string filename) {
			if (!_allowedExtensions.Any(e => filename.ToLowerInvariant().EndsWith(e)))
				throw new FileNotFoundException("Request was made for a file with an invalid extension");
			Filename = filename;
			var filePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings[Constants.UploadDirectory]);
			var imagePath = ConfigurationManager.AppSettings[Constants.UploadDirectory];

			if (File.Exists(Path.Combine(filePath, filename)))
				OriginalPath = Path.Combine(imagePath, filename);
			else
				throw new FileNotFoundException("Requested file does not exists");

			if (File.Exists(Path.Combine(filePath, Constants.ResizedDirectory, filename)))
				ResizedPath = Path.Combine(imagePath, Constants.ResizedDirectory, filename);
		}

	}
}
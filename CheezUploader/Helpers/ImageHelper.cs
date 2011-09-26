using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;

namespace CheezUploader.Helpers {
	public static class ImageHelper {
		public static void ResizeIfNeeded(string filename) {
			int MAX_WIDTH = int.Parse(ConfigurationManager.AppSettings[Constants.MaxWidth]);
			int MAX_HEIGHT = int.Parse(ConfigurationManager.AppSettings[Constants.MaxHeight]);

			var path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings[Constants.UploadDirectory]);
			using (var image = Image.FromFile(Path.Combine(path, filename))) {
				if (image.Width <= MAX_WIDTH && image.Height <= MAX_HEIGHT) return;

				var aspectRatio = (float)image.Width / image.Height;
				int height = 0, width = 0;
				if (aspectRatio > 1) { //landscape
					//landscape height can't be greater than width, therefore can't be greater than 500 after resize, so resized based on width alone
					width = MAX_WIDTH;
					height = (int)Math.Round(MAX_WIDTH / aspectRatio);
				} else { //portrait
					var defaultAspectRatio = (float)MAX_WIDTH / MAX_HEIGHT;
					if (aspectRatio < defaultAspectRatio) {
						//resize based on Height as limiting dimension
						height = MAX_HEIGHT;
						width = (int)Math.Round(aspectRatio * MAX_HEIGHT);
					} else {
						//resize based on Width as limiting dimension
						width = MAX_WIDTH;
						height = (int)Math.Round(aspectRatio * MAX_WIDTH);
					}
				}
				using (var bitmap = new Bitmap(width, height)) {
					using (var graphics = Graphics.FromImage((Image)bitmap)) {
						graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
						graphics.DrawImage(image, 0, 0, width, height);
						Directory.CreateDirectory(Path.Combine(path, Constants.ResizedDirectory));
						bitmap.Save(Path.Combine(path, Constants.ResizedDirectory, filename));
					}
				}
			}
		}
	}
}
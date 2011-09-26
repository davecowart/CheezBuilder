using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CheezUploader.Helpers;
using CheezUploader.Models;

namespace CheezUploader.Controllers {
	public class ImageController : Controller {
		public ActionResult Index() {
			return View(new ImageUpload());
		}

		[HttpPost]
		public ActionResult Upload(ImageUpload image) {
			if (!ModelState.IsValid) return View("Index", image);
			var path = Server.MapPath(ConfigurationManager.AppSettings[Constants.UploadDirectory]);
			var filename = Guid.NewGuid().ToString();
			var extension = Regex.Match(Request.Files[0].FileName, @"\.[a-zA-Z0-9]+$").Value;

			image.Filename = filename + extension;
			image.OriginalPath = Path.Combine(path, image.Filename);
			Directory.CreateDirectory(path);
			Request.Files[0].SaveAs(image.OriginalPath);
			ImageHelper.ResizeIfNeeded(image.Filename);

			return RedirectToRoute("ImageDisplay", new { filename = image.Filename });
		}

		public ActionResult Display(string filename) {
			var image = new ImageUpload(filename);
			return View(image);
		}
	}
}

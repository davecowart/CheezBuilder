using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheezUploader.Models;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace CheezUploader.Controllers {
	public class ImageController : Controller {
		public ActionResult Index() {
			var image = new ImageUpload();
			return View(image);
		}

		[HttpPost]
		public ActionResult Upload(ImageUpload image) {
			if (!ModelState.IsValid) return View("Index", image);
			var path = Server.MapPath(ConfigurationManager.AppSettings["UploadDirectory"]);
			var filename = Guid.NewGuid().ToString();
			var extension = Regex.Match(Request.Files[0].FileName, @"\.[a-zA-Z0-9]+$").Value;

			image.Filename = filename + extension;
			image.OriginalPath = Path.Combine(path, image.Filename);
			Request.Files[0].SaveAs(image.OriginalPath);

			return RedirectToRoute("ImageDisplay", new { filename = image.Filename });
		}

		public ActionResult Display(string filename) {
			var image = new ImageUpload(filename);
			return View(image);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.IO;
using ColorFlow.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ColorFlow.Controllers
{
    public class HomeController : Controller
    {
        BitmapFilter filter = new BitmapFilter();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        string GetFilteredImage(string filterType)
        {
            string output;
            switch (filterType)
            {
                case "grayscale":
                    output = Convert.ToBase64String(filter.Grayscale());
                    break;
                case "sepiatone":
                    output = Convert.ToBase64String(filter.SepiaTone());
                    break;
                case "invert":
                    output = Convert.ToBase64String(filter.Invert());
                    break;
                case "darkblue":
                    output = Convert.ToBase64String(filter.DarkBlue());
                    break;
                case "mirror":
                    output = Convert.ToBase64String(filter.Mirror());
                    break;
                default:
                    output = Convert.ToBase64String(filter.Grayscale());
                    break;
            }
            return output;
        }


        [HttpPost]
        public ActionResult view()
        {
            var file = Request.Files[0] as HttpPostedFileBase;
            BinaryReader br = new BinaryReader(file.InputStream);
            byte[] imageBytes = br.ReadBytes(file.ContentLength);
            filter = new BitmapFilter(imageBytes);
            ImageData data = new ImageData()
            {
                Image = filter.Image,
                Height = filter.Height,
                Width = filter.Width
            };
            return View(data);
        }

        [HttpPost]
        public ActionResult FilterImage(string json)
        {
            // gathering input
            JObject data = JsonConvert.DeserializeObject(json) as JObject;
            string inputImage = data["Image"].ToString();
            string filterType = data["Filter"].ToString();
            byte[] imageBytes = Convert.FromBase64String(inputImage);
            var stopwatch = Stopwatch.StartNew();
            filter.Image = imageBytes;
            string resultImage = GetFilteredImage(filterType);
            stopwatch.Stop();

            string duration = Math.Round(((double)stopwatch.ElapsedMilliseconds / 1000), 3).ToString();

            // returning results
            JsonResult response = new JsonResult()
            {
                Data = resultImage + "@@@" + duration,
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return response;
        }

    }
}

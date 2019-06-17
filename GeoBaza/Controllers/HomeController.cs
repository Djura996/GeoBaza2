using GeoBaza.Data;
using GeoBaza.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GeoBaza.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpGet]
        public ActionResult GetJsonLocation()
        {
            DataManipulation data = new DataManipulation();
            var locations = data.GetLocations();
            var features1 = new features() { type = "feature", properties = locations };

            var complexJson = new {
                type = "FeatureCollection",
                crs = new crs(),
                features = features1

            };
            var a = Json(complexJson, JsonRequestBehavior.AllowGet);

            return a;

        }
        [HttpGet]
        public ActionResult GetJsonRivers()
        {
            DataManipulation data = new DataManipulation();
            var rivers = data.GetRivers();
            var features1 = new features() { type = "feature", properties = rivers };

            var complexJson = new
            {
                type = "FeatureCollection",
                crs = new crs(),
                features = features1

            };
            var a = Json(complexJson, JsonRequestBehavior.AllowGet);

            return a;

        }


    }
}
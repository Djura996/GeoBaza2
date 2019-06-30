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
            DataManipulation data = new DataManipulation();
            var categories = data.getCategories();

            var model = new LayersModel() { categories = categories};
            return View( model );
        }

        [HttpGet]
        public ActionResult GetJsonLocation()
        {
            DataManipulation data = new DataManipulation();
            var locations = data.GetLocations(); // GET LIST OF FEATURES
           
            var complexJson = new {
                type = "FeatureCollection",
                crs = new crs(),
                features = locations

            };
            var a = Json(complexJson, JsonRequestBehavior.AllowGet);

            return   a;

        }

        [HttpGet]
        public ActionResult GetJsonLocationByCategory(string category)
        {
            DataManipulation data = new DataManipulation();
            var locations = new List<features>();
            if (category != string.Empty)
            {
                locations = data.GetLocationsByCategory(category); // GET LIST OF FEATURES
            }
            else
            {
                locations = data.GetLocations();
            }
            var complexJson = new
            {
                type = "FeatureCollection",
                crs = new crs(),
                features = locations

            };
            var a = Json(complexJson, JsonRequestBehavior.AllowGet);

            return a;

        }

        [HttpPost]
        public ActionResult SaveLocation(LayersModel model) {

            DataManipulation data = new DataManipulation();
            if (model.features.properties.gid != 0)
            {
                data.UpdateFeature(model.features.properties.fclass, model.features.properties.name, model.features.properties.address, model.features.properties.gid);
            }
            else
            {
                data.AddFeature(model.features.properties.fclass, model.features.properties.name, model.features.properties.address,model.features.geometry.coordinates[0], model.features.geometry.coordinates[1]);
            }

            return RedirectToAction("Index");
        }



    }
}
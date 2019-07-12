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
       
        public ActionResult Index() //ucitavanje stranice, pokupi kategorije, smesti ih u model i posalje u View
        {
            DataManipulation data = new DataManipulation();
            var categories = data.getCategories();

            var model = new LayersModel() { categories = categories};
            return View( model );
        }

        [HttpGet]
        public ActionResult GetJsonLocation() // metod koji kupi sve lokacije iz baze i mapira ga u json file
        {
            DataManipulation data = new DataManipulation();
            var locations = data.GetLocations(); // GET LIST OF FEATURES
           
            var complexJson = new {
                type = "FeatureCollection",
                crs = new crs(),
                features = locations

            };
            var a = Json(complexJson, JsonRequestBehavior.AllowGet);

            return   a;// vraca json format za javascript

        }

        [HttpGet]
        public ActionResult GetJsonLocationByCategory(string category)//kupi lokacije po kategorijama iz baze i vraca u json
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
        public ActionResult SaveLocation(LayersModel model) { // metod za snimanje lokacija

            DataManipulation data = new DataManipulation();
            if (model.features.properties.gid != 0) // update
            {
                data.UpdateFeature(model.features.properties.fclass, model.features.properties.name, model.features.properties.address, model.features.properties.gid);
            }
            else // za novi element
            {
                data.AddFeature(model.features.properties.fclass, model.features.properties.name, model.features.properties.address,model.features.geometry.coordinates[0], model.features.geometry.coordinates[1]);
            }

            return RedirectToAction("Index"); //refresh, pozove prvu funkciju
        }



    }
}
using GeoBaza.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GeoBaza.Data
{
    public class DataManipulation
    {
       public string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=su; Database=NS_Locations;";// konekcija za bazu

        List<string> roads = new List<string>();

        public List<string> getCategories()
        {
            var categories = new List<string>();
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connstring);  //biblioteka za rad sa Postgis bazama podataka
            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand("SELECT  distinct(fclass) FROM public.lokacije order by fclass asc", connection); //deo biblioteke
            NpgsqlDataReader dataReader = command.ExecuteReader();
            for (int i = 0; dataReader.Read(); i++) // prolazimo kroz listu svih lokacija i stavljamo i ih u listu kategorija
            {
                    categories.Add(dataReader[0].ToString());//.Replace("_", " "));
            }

                connection.Close();
                return categories;
            }
            catch (Exception msg)
            {

                throw;
            }

        }

        public List<features> GetLocations() // uzima sve lokacije
        {
            var locations = new List<features>();
            try
            {
               
                NpgsqlConnection connection = new NpgsqlConnection(connstring);  //biblioteka za rad sa Postgis bazama podataka
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT  fclass,name, ST_AsText(geom) as geom, gid, address FROM public.lokacije", connection);

                var decimalStyle = CultureInfo.CurrentCulture;

                NpgsqlDataReader dataReader = command.ExecuteReader();
                for (int i = 0; dataReader.Read(); i++) //svaku kordinatu iz baze mapiramo u objekat features koji kasnije pretvaramo u json 
                {
                    var coordinatesString = dataReader[2].ToString().Replace("MULTIPOINT(", "").Replace(")", "");
                    var a = coordinatesString.Substring(0, coordinatesString.IndexOf(" "));
                    var b = coordinatesString.Substring(coordinatesString.IndexOf(" ")+1);
                    var arr = new decimal[] { Decimal.Parse(a,decimalStyle), Decimal.Parse(b,decimalStyle) };
                    //var r = "[" +  + "]";
                    locations.Add( new features() { type="Feature", properties = new properties() { fclass = dataReader[0].ToString() , name = dataReader[1].ToString() ?? null, gid = Convert.ToInt32(dataReader[3]), address = dataReader[4].ToString() ?? null }, geometry = new geometry() { type = "Point", coordinates = arr } } );
                    
                }
                connection.Close();
                return locations;
            }
            catch (Exception msg)
            {
                
                throw;
            }
        }

        public List<features> GetLocationsByCategory(string category)
        {
            var locations = new List<features>();
            var categoryFilter = category;//.Replace(" ","_");
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);  //biblioteka za rad sa Postgis bazama podataka
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT  fclass,name, ST_AsText(geom) as geom, gid, address FROM public.lokacije where fclass ='"+ categoryFilter + "'", connection);

                var decimalStyle = CultureInfo.CurrentCulture;

                NpgsqlDataReader dataReader = command.ExecuteReader();
                for (int i = 0; dataReader.Read(); i++)
                {
                    var coordinatesString = dataReader[2].ToString().Replace("MULTIPOINT(", "").Replace(")", "");
                    var a = coordinatesString.Substring(0, coordinatesString.IndexOf(" "));
                    var b = coordinatesString.Substring(coordinatesString.IndexOf(" ") + 1);
                    var arr = new decimal[] { Decimal.Parse(a, decimalStyle), Decimal.Parse(b, decimalStyle) };
                    //var r = "[" +  + "]";
                    locations.Add(new features() { type = "Feature", properties = new properties() { fclass = dataReader[0].ToString(), name = dataReader[1].ToString() ?? null, gid = Convert.ToInt32(dataReader[3]), address = dataReader[4].ToString() ?? null }, geometry = new geometry() { type = "Point", coordinates = arr } });

                }
                connection.Close();
                return locations;
            }
            catch (Exception msg)
            {

                throw;
            }
        }

        public void UpdateFeature(string fclass, string name, string address, int gid)
        {
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);  //biblioteka za rad sa Postgis bazama podataka
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("Update public.lokacije set fclass='"+ fclass +"', name ='"+ name + "', address ='"+ address + "' where gid ="+ gid.ToString(), connection);
                //INSERT INTO public.lokacije(gid, osm_id, code, fclass, name, geom, address) VALUES(?, ?, ?, ?, ?, ?, ?);
                var response  = command.ExecuteNonQuery();
                connection.Close();
              
            }
            catch (Exception msg)
            {

                throw;
            }

        }

        public void AddFeature(string fclass, string name, string address, decimal longt, decimal lat)
        {
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);  //biblioteka za rad sa Postgis bazama podataka
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("Insert into public.lokacije(gid, osm_id, code, fclass, name, geom, address) VALUES( (select max(gid)+1 from public.lokacije), null, null,'"+fclass+"','"+name+ "',ST_GeomFromText('MULTIPOINT(" + longt.ToString()+" "+lat.ToString()+")', 4326),'" + address+"') ",connection);
                
                var response = command.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception msg)
            {

                throw;
            }

        }


    }
}
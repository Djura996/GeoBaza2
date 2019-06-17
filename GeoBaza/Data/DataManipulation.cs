using GeoBaza.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GeoBaza.Data
{
    public class DataManipulation
    {
       
     
        List<string> roads = new List<string>();


        public List<properties> GetLocations()
        {
            var locations = new List<properties>();
            try
            {
                string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=su; Database=NS_Locations;";
                NpgsqlConnection connection = new NpgsqlConnection(connstring);  //biblioteka za rad sa Postgis bazama podataka
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT  fclass,name, ST_AsText(geom) as geom FROM public.lokacije", connection);
                
                NpgsqlDataReader dataReader = command.ExecuteReader();
                for (int i = 0; dataReader.Read(); i++)
                {
                    locations.Add(new properties() { fclass = dataReader[0].ToString() , name = dataReader[1].ToString() ?? null, geometry =  new geometry() {type="point", coordinates = "["+ dataReader[2].ToString().Replace("MULTIPOINT(", "").Replace(")", "").Replace(" ",",") + "]" } });

                }
                connection.Close();
                return locations;
            }
            catch (Exception msg)
            {
                
                throw;
            }
        }
        //public List<string> GetRoads()
        //{
        //    try
        //    {
        //        string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=su; Database=NS_Locations;";
        //        NpgsqlConnection connection = new NpgsqlConnection(connstring);
        //        connection.Open();
        //        NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.putevi", connection);
        //        NpgsqlDataReader dataReader = command.ExecuteReader();
        //        for (int i = 0; dataReader.Read(); i++)
        //        {
        //            locations.Add(dataReader[0].ToString() + "," + dataReader[1].ToString() + "," + dataReader[2].ToString() + "\r\n");
        //        }
        //        connection.Close();
        //        return locations;
        //    }
        //    catch (Exception msg)
        //    {

        //        throw;
        //    }
        //}
        public List<properties> GetRivers()
        {
            var rivers = new List<properties>();
            try
            {
                string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=su; Database=NS_Locations;";
                NpgsqlConnection connection = new NpgsqlConnection(connstring);  //biblioteka za rad sa Postgis bazama podataka
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT fclass, name, ST_AsText(geom) AS geom FROM public.reke", connection);
                
                NpgsqlDataReader dataReader = command.ExecuteReader();
                for (int i = 0; dataReader.Read(); i++)
                {
                    rivers.Add(new properties() { fclass = dataReader[0].ToString(), name = dataReader[1].ToString() ?? null, geometry = new geometry() { type = "point", coordinates = dataReader[2].ToString().Replace("MULTIPOLYGON(((", "[").Replace(")))", "]").Replace(",","],[").Replace(" ", ",") } });

                }
                connection.Close();
                return rivers;
            }
            catch (Exception msg)
            {

                throw;
            }

        }
    }
}
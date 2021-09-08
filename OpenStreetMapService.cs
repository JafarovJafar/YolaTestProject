using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YolaTestProject
{
    public class Geojson
    {
        public string type { get; set; }
        public List<List<List<object>>> coordinates { get; set; }
    }

    public class Root
    {
        public int place_id { get; set; }
        public string licence { get; set; }
        public string osm_type { get; set; }
        public long osm_id { get; set; }
        public List<string> boundingbox { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string display_name { get; set; }
        public string @class { get; set; }
        public string type { get; set; }
        public double importance { get; set; }
        public string icon { get; set; }
        public Geojson geojson { get; set; }
    }

    class OpenStreetMapService : IGeoService
    {
        private HttpWebRequest _httpWebRequest;
        private HttpWebResponse _response;

        private string _url;

        List<MapPoint> resultPoints = new List<MapPoint>();

        public MapPoint[] GetMapPoints(string query)
        {
            _url = $"https://nominatim.openstreetmap.org/search?q={query}&format=json&polygon_geojson=1&limit=1";

            _httpWebRequest = (HttpWebRequest)WebRequest.Create(_url);
            _httpWebRequest.ContentType = "text/json";
            _httpWebRequest.Method = "GET";
            _httpWebRequest.UserAgent = CONSTANTS.USER_AGENT;

            _response = (HttpWebResponse)_httpWebRequest.GetResponse();

            using (StreamReader stream = new StreamReader(_response.GetResponseStream()))
            {
                string result = stream.ReadToEnd();

                // тут нейминги не особо важны, потому что структура не будет меняться
                // и это локальные переменные, к которым сложно придумать подходящие названия
                List<Root> level1 = JsonConvert.DeserializeObject<List<Root>>(result);

                List<List<List<object>>> coordinates;

                foreach (Root root in level1)
                {
                    if (root.geojson != null)
                    {
                        coordinates = root.geojson.coordinates;

                        foreach (List<List<object>> coordinate in coordinates)
                        {
                            switch (root.geojson.type)
                            {
                                case "MultiPolygon":
                                    foreach (object coord in coordinate[0])
                                    {
                                        JArray array = coord as JArray;
                                        resultPoints.Add(new MapPoint(array.First.Value<double>(), array.Last.Value<double>()));
                                    }
                                    break;

                                case "Polygon":
                                    foreach (List<object> coord in coordinate)
                                    {
                                        resultPoints.Add(new MapPoint((double)coord[0], (double)coord[1]));
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            return resultPoints.ToArray();
        }
    }
}
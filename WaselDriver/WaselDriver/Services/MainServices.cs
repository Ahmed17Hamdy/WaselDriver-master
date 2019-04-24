using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Models;

namespace WaselDriver.Services
{
 public   class MainServices
    {
        public async Task<ObservableCollection<Country>> GetAllCountries()
        {
            using (var client = new HttpClient())
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("lang", "");
                string content = JsonConvert.SerializeObject(values);
                try
                {
                    var response = await client.PostAsync("http://wassel.alsalil.net/api/settinginfo", new StringContent(content, Encoding.UTF8, "text/json"));
                    var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    var Req = JsonConvert.DeserializeObject<Response<string, MainResponseMessage>>(serverResponse);
                    var Categories = Req.message.country;
                    return Categories;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

    }
}

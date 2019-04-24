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
  public  class DelivaryService
    {
        public async Task<ObservableCollection<TirhalModel>> GetAllterhal()
        {

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync("http://Wassel.alsalil.net/api/alltirhalmodel");
                    var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    var Req = JsonConvert.DeserializeObject<Response<string, ObservableCollection<TirhalModel>>>(serverResponse);
                    var Tirhal = Req.message;
                    return Tirhal;
                }
                catch (Exception)
                {
                    return new ObservableCollection<TirhalModel>();
                }
            }
        }
    }
}

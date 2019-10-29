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
  public static class DelivaryService
    {
        public static async Task<ObservableCollection<TirhalModel>> GetAllterhal()
        {

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync("http://Wassel.alsalil.net/api/alltirhalmodel");
                    var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    var Req = JsonConvert.DeserializeObject<Response<string, ResponseMessage<ObservableCollection<TirhalModel>>>>(serverResponse);
                    var Tirhal = Req.message.data;
                    return Tirhal;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace WaselDriver.Models
{
 public   class DelivaryObject
    {
        
    public int id { get; set; }
    public string created_at { get; set; }
    public string user_id { get; set; }
    public string updated_at { get; set; }
    public string driver_id { get; set; }
        public string latto { get; set; }
        public string lngto { get; set; }
    public int done { get; set; }
    public string latfrom { get; set; }
        public string lngfrom { get; set; }
        public string car_model_id { get; set; }
    }
    public class TirhalModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string PPrice { get; set; }
        public string Availability { get; set; }
        public int? price { get; set; }
        public int? available { get; set; }
        public object image { get; set; }
    }
    public class TirhalOrder 
    {
        public int id { get; set; }
        public string created_at { get; set; }
        public string user_id { get; set; }
        public string updated_at { get; set; }
        public string driver_id { get; set; }
        public string latto { get; set; }
        public string lngto { get; set; }
        public int done { get; set; }
        public string latfrom { get; set; }
        public string lngfrom { get; set; }
        public string car_model_id { get; set; }
    }
    public class CheckedTirhalOrder : TirhalOrder
    {
        public string order_id { get; set; }

    }
}

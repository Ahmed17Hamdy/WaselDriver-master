using System;
using System.Collections.Generic;
using System.Text;

namespace WaselDriver.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string confirmpass { get; set; }
        public string firebase_token { get; set; }
        public string device_id { get; set; }
        public string user_hash { get; set; }
        public string pocket { get; set; }
        public string debt { get; set; }
        public string points { get; set; }
        public string status { get; set; }
        public string forgetcode { get; set; }
        public string suspensed { get; set; }
        public string maintance { get; set; }
        public string delivery { get; set; }
        public int role { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
  //      public List<City> rates { get; set; }
    }
}

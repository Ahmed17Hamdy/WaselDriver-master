using System;
using System.Collections.Generic;
using System.Text;
using WaselDriver.Models.Interface;


namespace WaselDriver.Models
{
    public class City : CountryInterface
    {
        public int id { get; set; }
        public string name { get; set; }
        public string enname { get; set; }
        public string parent { get; set; }
    }
}
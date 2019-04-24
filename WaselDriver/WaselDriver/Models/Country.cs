using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WaselDriver.Models.Interface;

namespace WaselDriver.Models
{
    public class Country : CountryInterface
    {
        public int id { get; set; }
        public string name { get; set; }
        public string enname { get; set; }
        public string parent { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string TrueImage { get; set; }
        public ObservableCollection<City> cities { get; set; }
    }
}

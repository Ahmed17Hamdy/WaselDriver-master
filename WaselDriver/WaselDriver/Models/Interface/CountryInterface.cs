using System;
using System.Collections.Generic;
using System.Text;

namespace WaselDriver.Models.Interface
{
    public interface CountryInterface
    {
        int id { get; set; }
        string name { get; set; }
        string enname { get; set; }
        string parent { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace WaselDriver.Models.Interface
{
      public interface IValidator
    {
        string Message { get; set; }
        bool Check(string value);
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WaselDriver.Models
{
    public class Response<T, I>
    {
        public bool success { get; set; }
        public T data { get; set; }
        public I message { get; set; }
    }

    public class MainResponseMessage
    {
      
        public ObservableCollection<Country> country { get; set; }
    }
}

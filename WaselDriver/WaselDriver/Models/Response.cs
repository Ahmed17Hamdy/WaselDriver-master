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
    public class ResponseMessage<T>
    {
        public int current_page { get; set; }
        public T data { get; set; }
        public string first_page_url { get; set; }
        public int from { get; set; }
        public int last_page { get; set; }
        public string last_page_url { get; set; }
        public string next_page_url { get; set; }
        public string path { get; set; }
        public int per_page { get; set; }
        public string prev_page_url { get; set; }
        public int to { get; set; }
        public int total { get; set; }
    }
    public class MainResponseMessage
    {
        public ObservableCollection<Country> country { get; set; }
    }
}

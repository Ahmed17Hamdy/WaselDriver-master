using System;
using System.Collections.Generic;
using System.Text;
using WaselDriver.Models.Interface;
namespace WaselDriver.Models
{
    public class RequiredValidator : IValidator
    {
        public string Message { get; set; } = "This field is required";

        public bool Check(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}

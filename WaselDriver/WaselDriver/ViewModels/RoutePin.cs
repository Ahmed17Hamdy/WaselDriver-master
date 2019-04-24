using TK.CustomMap;
using TK.CustomMap.Overlays;
using Xamarin.Forms;

namespace WaselDriver.ViewModels
{
    internal class RoutePin : TKCustomMapPin
    {
        public TKRoute Route { get; set; }
        public bool IsSource { get; set; }
        
    }
}
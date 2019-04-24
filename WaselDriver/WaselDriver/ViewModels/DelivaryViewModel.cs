using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Helper;
using WaselDriver.Models;
using WaselDriver.Services;
using Xamarin.Forms;

namespace WaselDriver.ViewModels
{
    public class DelivaryViewModel : INotifyPropertyChanged
    {

        public DelivaryViewModel()
        {
            PageDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            ConnectedIsVisable = (CrossConnectivity.Current.IsConnected) ? true : false;
            NotConnectedIsVisable = !ConnectedIsVisable;
            //DataGetter();
            CarTypegetter();


        }

        private bool _connectedIsVisable;

        public bool ConnectedIsVisable
        {
            get { return _connectedIsVisable; }
            set { _connectedIsVisable = value; OnPropertyChanged(); }
        }

        private bool _notConnectedIsVisable;

        public bool NotConnectedIsVisable
        {
            get { return _notConnectedIsVisable; }
            set { _notConnectedIsVisable = value; OnPropertyChanged(); }
        }


        public FlowDirection PageDirection { get; set; }

        private int _country;
        public int Country
        {
            get { return _country; }
            set { _country = value; OnPropertyChanged(); }
        }

        private int _city;
        public int City
        {
            get { return _city; }
            set { _city = value; OnPropertyChanged(); }
        }

        private List<string> _imgs;
        public List<string> Imgs
        {
            get { return _imgs; }
            set { _imgs = value; }
        }

       
        private ObservableCollection<TirhalModel> _cartype;
        public ObservableCollection<TirhalModel> CarType
        {
            get
            {
                return _cartype;
            }
            set
            {
                _cartype = value;
                OnPropertyChanged();
            }
        }

        private bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; OnPropertyChanged(); }
        }


        private DelivaryObject _selectedOrder;
        public DelivaryObject SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
            }
        }
      
        private string _errormessage;
        public string ErrorMessage
        {
            get { return _errormessage; }
            set
            {
                _errormessage = value;
                OnPropertyChanged();
            }
        }
        private async Task CarTypegetter()
        {
            DelivaryService service = new DelivaryService();

            var CartypeBack = await service.GetAllterhal();
            foreach (var item in CartypeBack)
            {
                var baramImg = item.image;
                item.image = "http://wassel.alsalil.net/users/images/" + baramImg;
                if (item.price == null || item.available == null || item.available == 0)
                {
                    item.PPrice = "لم يتم تحديد سعر من السائق";
                    item.Availability = "غير متاح";
                }
                else
                {
                    item.PPrice = item.price.ToString();
                    item.Availability = "متاح";
                }
            }
            CarType = CartypeBack;
        }

       
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

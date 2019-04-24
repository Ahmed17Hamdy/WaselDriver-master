using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using WaselDriver.Models;

namespace WaselDriver.Helper
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        private const string LascarmodelidKey = "last_carmodelid_key";
        private static readonly string Carmodelkey= string.Empty;
        public static string CarModelID
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LascarmodelidKey, Carmodelkey);


            set
            =>
                AppSettings.AddOrUpdateValue(LascarmodelidKey, value);

        }
        private const string LastlattoKey = "last_latto_key";
        private static readonly string LattoKey = string.Empty;
        public static string Latto
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastlattoKey, LattoKey);


            set
            =>
                AppSettings.AddOrUpdateValue(LastlattoKey, value);

        }
        private const string LastlatfromKey = "last_latfrom_key";
        private static readonly string LatfromKey = string.Empty;
        public static string Latfrom
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastlatfromKey, LatfromKey);


            set
            =>
                AppSettings.AddOrUpdateValue(LastlatfromKey, value);

        }
        


        private const string LastlngtoKey = "last_lngto_key";
        private static readonly string LngtoKey = string.Empty;
        public static string Lngto
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastlngtoKey, LngtoKey);


            set
            =>
                AppSettings.AddOrUpdateValue(LastlngtoKey, value);

        }
        private const string LastlngfromKey = "last_lngfrom_key";
        private static readonly string LngfromKey = string.Empty;
        public static string Lngfrom
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastlngfromKey, LngfromKey);


            set
            =>
                AppSettings.AddOrUpdateValue(LastlngfromKey, value);

        }
        private const string LastPlacetoKey = "last_Placeto_key";
        private static readonly string PlacetoKey = string.Empty;
        public static string Placeto
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastPlacetoKey, PlacetoKey);


            set
            =>
                AppSettings.AddOrUpdateValue(LastPlacetoKey, value);

        }

        private const string LastPlacefromKey = "last_Placefrom_key";
        private static readonly string PlacefromKey = string.Empty;
        public static string PlaceFrom
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastPlacefromKey, PlacefromKey);


            set
            =>
                AppSettings.AddOrUpdateValue(LastPlacefromKey, value);

        }
      
        private const string LastNameKey = "last_Name_key";
        private static readonly string LastProfileName = string.Empty;
        public static string ProfileName
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastProfileName, LastNameKey);


            set
            =>
                AppSettings.AddOrUpdateValue(LastProfileName, value);

        }

        private const string LastEmailSettingsKey = "last_email_key";
        private static readonly string SettingsDefault = string.Empty;
        public static string LastUsedEmail
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastEmailSettingsKey, SettingsDefault);


            set
            =>
                AppSettings.AddOrUpdateValue(LastEmailSettingsKey, value);

        }
        private const string LastUserStatusSettingsKey = "last_userstatus_key";
        private static readonly string SettingsStatus = string.Empty;
        public static string LastUserStatus
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastUserStatusSettingsKey, SettingsStatus);


            set
            =>
                AppSettings.AddOrUpdateValue(LastUserStatusSettingsKey, value);

        }

        private const string LastSignalIDSettingKey = "last_email_key";
        private static readonly string SignalID = string.Empty;
        public static string LastSignalID
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastSignalIDSettingKey, SignalID);


            set
            =>
                AppSettings.AddOrUpdateValue(LastSignalIDSettingKey, value);

        }

        private const string LastlatKey = "last_lat_key";
        private static readonly string LatKey = string.Empty;
        public static string LastLat
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastlatKey, LatKey);


            set
            =>
                AppSettings.AddOrUpdateValue(LastlatKey, value);

        }
        private const string LastlngKey = "last_lng_key";
        private static readonly string LngKey = string.Empty;
        public static string LastLng
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastlngKey, LngKey);


            set
            =>
                AppSettings.AddOrUpdateValue(LastlngKey, value);

        }
        //private const string LastServiceSettingsKey = "last_service_key";
        //private static readonly string ServiceSettingsDefault = string.Empty;
        //public static string LastUsedService
        //{
        //    get
        //    =>
        //         AppSettings.GetValueOrDefault(LastServiceSettingsKey, ServiceSettingsDefault);


        //    set
        //    =>
        //        AppSettings.AddOrUpdateValue(LastServiceSettingsKey, value);

        //}

        private const string LastRoleSettingsKey = "last_role_key";
        private static readonly int SettingsRole = 0;
        public static int LastUseeRole
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastRoleSettingsKey, SettingsRole);


            set
            =>
                AppSettings.AddOrUpdateValue(LastRoleSettingsKey, value);

        }

        private const string LastGravity = "last_Gravity_key";
        private static readonly string GravitySettings = string.Empty;
        public static string LastUserGravity
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastGravity, GravitySettings);


            set
            =>
                AppSettings.AddOrUpdateValue(LastGravity, value);

        }

        private const string LastUserHash = "User_Hash";
        private static readonly string UserHashDefault = string.Empty;
        public static string UserHash
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastUserHash, UserHashDefault);


            set
            =>
                AppSettings.AddOrUpdateValue(LastUserHash, value);

        }

        private const string LastUserFirebaseToken = "Firebase_Token";
        private static readonly string LastFirebaseToken = string.Empty;
        public static string UserFirebaseToken
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastUserFirebaseToken, LastFirebaseToken);


            set
            =>
                AppSettings.AddOrUpdateValue(LastUserFirebaseToken, value);

        }


        private const string PublicParamter = "PublicParamter";
        private static readonly string ParamSettings = string.Empty;
        public static string LastPublicParamter
        {
            get
            =>
                 AppSettings.GetValueOrDefault(PublicParamter, ParamSettings);


            set
            =>
                AppSettings.AddOrUpdateValue(PublicParamter, value);

        }

        private const string SecondParamter = "SecondParamter";
        private static readonly string SecondSettings = string.Empty;
        public static string LastSecondParamter
        {
            get
            =>
                 AppSettings.GetValueOrDefault(SecondParamter, SecondSettings);


            set
            =>
                AppSettings.AddOrUpdateValue(SecondParamter, value);

        }


        private const string LastIDSettingsKey = "last_ID_key";
        private static readonly int SettingsIDDefault = 0;
        public static int LastUsedID
        {
            get => AppSettings.GetValueOrDefault(LastIDSettingsKey, SettingsIDDefault);
            set => AppSettings.AddOrUpdateValue(LastIDSettingsKey, value);

        }

        private const string LastServiceID = "last_ID_key";
        private static readonly int SettingsServiceID = 0;
        public static int LastServeceIDParam
        {
            get => AppSettings.GetValueOrDefault(LastServiceID, SettingsServiceID);
            set => AppSettings.AddOrUpdateValue(LastServiceID, value);

        }

        private const string _countryID = "last_Country";
        private static readonly int SettingsCountriesDefault = 0;
        public static int LastCountry
        {
            get => AppSettings.GetValueOrDefault(_countryID, SettingsCountriesDefault);
            set => AppSettings.AddOrUpdateValue(_countryID, value);

        }

        private const string _cityID = "last_City";
        private static readonly int SettingsCityDefault = 0;
        public static int LastCity
        {
            get => AppSettings.GetValueOrDefault(_cityID, SettingsCityDefault);
            set => AppSettings.AddOrUpdateValue(_cityID, value);

        }
        private const string _notification = "last_notification";
        private static readonly string Settingsnotification =null;
        public static string LastNotify
        {
            get => AppSettings.GetValueOrDefault(_notification, Settingsnotification);
            set => AppSettings.AddOrUpdateValue(_notification, value);

        }
        private const string LastDriverIDSettingsKey = "last_Driver_ID_key";
        private static readonly int SettingsDriverIDDefault = 0;
        public static int LastUsedDriverID
        {
            get => AppSettings.GetValueOrDefault(LastDriverIDSettingsKey, SettingsDriverIDDefault);
            set => AppSettings.AddOrUpdateValue(LastDriverIDSettingsKey, value);

        }
        private const string LastCarModelSettingsKey = "last_CarModel_key";
        private static readonly int SettingsCarModelDefault = 0;
        public static int LastUsedCarModel
        {
            get => AppSettings.GetValueOrDefault(LastCarModelSettingsKey, SettingsCarModelDefault);
            set => AppSettings.AddOrUpdateValue(LastCarModelSettingsKey, value);

        }
        private const string LastdoneKey = "last_done_key";
        private static readonly int SettingdoneDefault = 0;
        public static int Lastdone
        {
            get => AppSettings.GetValueOrDefault(LastdoneKey, SettingdoneDefault);
            set => AppSettings.AddOrUpdateValue(LastdoneKey, value);

        }
        private const string LastidKey = "last_id_key";
        private static readonly int SettingidDefault = 0;
        public static int LastOrderid
        {
            get => AppSettings.GetValueOrDefault(LastidKey, SettingidDefault);
            set => AppSettings.AddOrUpdateValue(LastidKey, value);

        }
    }
}

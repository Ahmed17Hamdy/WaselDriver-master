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
        private const string LastregisterKey = "last_register_key";
        private static readonly string RegisterKey = string.Empty;
        public static string LastRegister
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastregisterKey, RegisterKey);


            set
            =>
                AppSettings.AddOrUpdateValue(LastregisterKey, value);

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

        private const string LastSignalIDSettingKey = "last_signal_key";
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


       

        private const string LastIDSettingsKey = "last_ID_key";
        private static readonly int SettingsIDDefault = 0;
        public static int LastUsedID
        {
            get => AppSettings.GetValueOrDefault(LastIDSettingsKey, SettingsIDDefault);
            set => AppSettings.AddOrUpdateValue(LastIDSettingsKey, value);

        }

        private const string LastDriverIDSettingsKey = "last_DriverID_key";
        private static readonly int SettingsDriverIDDefault = 0;
        public static int LastUsedDriverID
        {
            get => AppSettings.GetValueOrDefault(LastDriverIDSettingsKey, SettingsDriverIDDefault);
            set => AppSettings.AddOrUpdateValue(LastDriverIDSettingsKey, value);

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


        private const string _countryID = "last_Country";
        private static readonly int SettingsCountriesDefault = 0;
        public static int LastCountry
        {
            get => AppSettings.GetValueOrDefault(_countryID, SettingsCountriesDefault);
            set => AppSettings.AddOrUpdateValue(_countryID, value);

        }

        
        
    }
}

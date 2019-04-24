using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Models;

namespace WaselDriver.Services
{
  public  class UserServices
    {
        public async Task<string> InsertUser(User user)
        {
            using (var client = new HttpClient())
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("name", user.name);
                values.Add("email", user.email);
                values.Add("password", user.password);
                values.Add("confirmpass", user.confirmpass);
                values.Add("firebase_token", user.firebase_token);
                values.Add("device_id", user.device_id);
                values.Add("country", user.country);
                values.Add("phone", user.phone);
                values.Add("role", user.role.ToString());
                string content = JsonConvert.SerializeObject(values);
                try
                {
                    var response = await client.PostAsync("http://wassel.alsalil.net/api/register", new StringContent(content, Encoding.UTF8, "text/json"));
                    var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    
                    return serverResponse;
                }
                catch (Exception)
                {
                    return "false";
                }
            }

        }
        public async Task<string> login(string email, string password, string device_id, string firebase_token)
        {
            using (var client = new HttpClient())
            {


                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("email", email);
                values.Add("password", password);
                values.Add("firebase_token", firebase_token);
                values.Add("device_id", device_id);
                try
                {
                    string content = JsonConvert.SerializeObject(values);
                    var response = await client.PostAsync("http://wassel.alsalil.net/api/login", new StringContent(content, Encoding.UTF8, "text/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        return serverResponse;
                    }
                    else
                    {
                        return "false";
                    }

                }
                catch (Exception)
                {
                    return "Error";
                }
            }
        }
        public async Task<User> UpdateUser(User user)
        {
            using (var client = new HttpClient())
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("name", user.name);
                values.Add("email", user.email);
                values.Add("user_id", user.id.ToString());
                values.Add("country", user.country);
                values.Add("user_hash", WaselDriver.Helper.Settings.UserHash);
                values.Add("phone", user.phone);
                try
                {
                    string content = JsonConvert.SerializeObject(values);
                    var response = await client.PostAsync("http://wassel.alsalil.net/api/updateprofile", new StringContent(content, Encoding.UTF8, "text/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        var Req = JsonConvert.DeserializeObject<Response<string, User>>(serverResponse);
                        return Req.message;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public async Task<string> BackupEmail(string _email)
        {
            using (var client = new HttpClient())
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("email", _email);
                values.Add("user_hash", WaselDriver.Helper.Settings.UserHash);

                string content = JsonConvert.SerializeObject(values);
                try
                {
                    var response = await client.PostAsync("http://wassel.alsalil.net/api/forgetpassword", new StringContent(content, Encoding.UTF8, "text/json"));
                    var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    return serverResponse;
                }
                catch (Exception)
                {
                    return "False";
                }
            }
        }

        public async Task<string> CodeVerfication(string mail, string Code)
        {
            using (var client = new HttpClient())
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("email", mail);
                values.Add("forgetcode", Code);
                values.Add("user_hash", WaselDriver.Helper.Settings.UserHash);

                string content = JsonConvert.SerializeObject(values);
                try
                {
                    var response = await client.PostAsync("http://wassel.alsalil.net/api/activcode", new StringContent(content, Encoding.UTF8, "text/json"));
                    var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    return serverResponse;
                }
                catch (Exception)
                {
                    return "False";
                }
            }
        }

        public async Task<string> ResetPassword(string NewBassword)
        {
            using (var client = new HttpClient())
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("new_password", NewBassword);
                values.Add("confirmpassword", NewBassword);
                values.Add("user_hash", WaselDriver.Helper.Settings.UserHash);

                string content = JsonConvert.SerializeObject(values);
                try
                {
                    var response = await client.PostAsync("http://wassel.alsalil.net/api/rechangepass", new StringContent(content, Encoding.UTF8, "text/json"));
                    var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    return serverResponse;
                }
                catch (Exception)
                {
                    return "False";
                }
            }
        }
        public async Task<User> UpdateUserLocation(User user)
        {
            using (var client = new HttpClient())
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                
                values.Add("user_id", user.id.ToString());
                
                try
                {
                    string content = JsonConvert.SerializeObject(values);
                    var response = await client.PostAsync("http://wassel.alsalil.net/api/updateprofile", new StringContent(content, Encoding.UTF8, "text/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        var Req = JsonConvert.DeserializeObject<Response<string, User>>(serverResponse);
                        return Req.message;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public async Task<string> ChangePassword(string OldPass, string NewBassword)
        {
            using (var client = new HttpClient())
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("old_password", OldPass);
                values.Add("new_password", NewBassword);
                values.Add("confirmpassword", NewBassword);
                values.Add("user_hash", WaselDriver.Helper.Settings.UserHash);
                values.Add("user_id", WaselDriver.Helper.Settings.LastUsedID.ToString());

                string content = JsonConvert.SerializeObject(values);
                try
                {
                    var response = await client.PostAsync("http://wassel.alsalil.net/api/rechangepass", new StringContent(content, Encoding.UTF8, "text/json"));
                    var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    return serverResponse;
                }
                catch (Exception)
                {
                    return "False";
                }
            }
        }
    }
}

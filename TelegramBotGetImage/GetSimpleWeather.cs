using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace TelegramBotGetImage
{
    class GetSimpleWeather
    {
        public static WeatherResponse WeatherGetNow()                                    //////////////////////////////////////// Погода сейчас
        {
            string url =
                "http://api.openweathermap.org/data/2.5/weather?q=Samara&units=metric&appid=96e1acdb11e7e23103af509121e8c25f";
            //ключ должен работать, но не факт 6fa095c114c44b8983cf448560847507

            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);

            HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();

            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);
            
            //Dictionary<string, string> newDictionary = new Dictionary<string, string>() {{ weatherResponse.Name, weatherResponse.Main.temp }};
            return weatherResponse;
        }
       
        public static WeatherResponseFor5Days WeatherGetFor5Days()                                 //////////////////////////////////////// Погода на 3- 5 дней
        {
            string url = "http://api.openweathermap.org/data/2.5/forecast?q=Samara&units=metric&appid=96e1acdb11e7e23103af509121e8c25f";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            WeatherResponseFor5Days weatherResponse = JsonConvert.DeserializeObject<WeatherResponseFor5Days>(response);

            return weatherResponse;
        }
    }
}
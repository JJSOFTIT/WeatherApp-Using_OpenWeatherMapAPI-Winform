using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        const string APPID = "542ffd081e67f4512b705f89d2a611b2";
        string cityName = "Colombo";
        public Form1()
        {
            InitializeComponent();
            getWeather("America"); // one day weather
            getForcast("America"); // more than one day
        }

        void getWeather(string city)
        {
            using (WebClient web = new WebClient())
            {

                string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&cnt=6", city, APPID);

                var json = web.DownloadString(url);

                var result = JsonConvert.DeserializeObject<WeatherInfo.Root>(json);

                WeatherInfo.Root outPut = result;

                lbl_cityName.Text = string.Format("{0}", outPut.name);
                lbl_country.Text = string.Format("{0}", outPut.sys.country);
                lbl_Temp.Text = string.Format("{0} \u00B0"+"C", outPut.main.temp);

                picture_Main.Image = setIcon(outPut.weather[0].icon);
            }

        }
        void getForcast(string city)
        {
            int day = 5;
            string url = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&units=metric&cnt={1}&APPID={2}",city,day,APPID);
            using (WebClient web = new WebClient())
            {
                var json = web.DownloadString(url);
                var Object = JsonConvert.DeserializeObject<weatherForcast>(json);

                weatherForcast forcast = Object;

                //next day
                lbl_day_2.Text = string.Format("{0}", getDate(forcast.list[1].dt).DayOfWeek); // returning Day
                lbl_cond_2.Text = string.Format("{0}", forcast.list[1].weather[0].main); // weather condition
                lbl_des_2.Text = string.Format("{0}", forcast.list[1].weather[0].description); // weather description
                lbl_temp_2.Text = string.Format("{0}\u00B0" + "C", forcast.list[1].temp.day); // weather temp
                lbl_wind_2.Text = string.Format("{0} km/h", forcast.list[1].speed); // weather wind speed

                // day after tomarow
                lbl_day_3.Text = string.Format("{0}", getDate(forcast.list[2].dt).DayOfWeek); // returning Day
                lbl_cond_3.Text = string.Format("{0}", forcast.list[2].weather[0].main); // weather condition
                lbl_des_3.Text = string.Format("{0}", forcast.list[2].weather[0].description); // weather description
                lbl_temp_3.Text = string.Format("{0}\u00B0" + "C", forcast.list[2].temp.day); // weather temp
                lbl_wind_3.Text = string.Format("{0} km/h", forcast.list[2].speed); // weather wind speed

                //weather icon
                pic_1.Image = setIcon(forcast.list[1].weather[0].icon);  
                pic_2.Image = setIcon(forcast.list[2].weather[0].icon); 
            }
        }

        DateTime getDate(double millisecound)
        {

            DateTime day = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).ToLocalTime();
            day = day.AddSeconds(millisecound).ToLocalTime();

            return day;
        }

        Image setIcon(string iconID)
        {
            string url = string.Format("http://openweathermap.org/img/w/{0}.png", iconID); // weather icon resource 
            var request = WebRequest.Create(url);
            using (var response = request.GetResponse())
            using (var weatherIcon = response.GetResponseStream())
            {
                Image weatherImg = Bitmap.FromStream(weatherIcon);
                return weatherImg;
            }
        }

        BitmapImage seticon()
        {
            BitmapImage weatherImg = new BitmapImage();
            weatherImg..BeginInit();
            weatherImg.UriSource = new Uri(url);
            weatherImg.EndInit();

            return weatherImg;
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            if (txt_cityname.Text != "")
            {
                getWeather(txt_cityname.Text);
                getForcast(txt_cityname.Text);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_cityname.Text != "")
            {
                using (StreamWriter str = new StreamWriter("my_weather.txt"))
                {
                    str.WriteLine("City Name: "+lbl_cityName.Text);
                    str.WriteLine("Country Name: "+lbl_country.Text);
                    str.WriteLine("Temp Name: "+lbl_Temp.Text);

                }
            }
        }
    }
}

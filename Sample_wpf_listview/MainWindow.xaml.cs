using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.IO;
using System.Net;
using System.Drawing;

namespace Sample_wpf_listview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void wndMain_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnGetWeather_Click(object sender, RoutedEventArgs e)
        {
            // 天気予報の取得   
            // https://weather.tsukumijima.net/
            string baseUrl = "https://weather.tsukumijima.net/api/forecast";
            //東京都のID
            string cityname = "130010";

            string url = $"{baseUrl}?city={cityname}";
            string json = new HttpClient().GetStringAsync(url).Result;

            var jsonNode = System.Text.Json.Nodes.JsonNode.Parse(json);

            this.txtLocation.Text = (string)(jsonNode?["title"]);

            var forecasts = (JsonArray)jsonNode?["forecasts"];


            List<Item> items = new List<Item>();

            for (int count = 0; count < 3; count++)
            {
                string imageUrl = (string)((forecasts[count]["image"])["url"]);
                imageUrl = imageUrl.Replace(".svg", ".png");
                BitmapImage image = new BitmapImage(new Uri(imageUrl));
                items.Add(
                    new Item()
                    {
                        date    = (string)(forecasts[count]["date"]),
                        telop   = (string)(forecasts[count]["telop"]),
                        tempMin = (string)(((forecasts[count]["temperature"])["min"])["celsius"]),
                        tempMax = (string)(((forecasts[count]["temperature"])["max"])["celsius"]),
                        rain06  = (string)((forecasts[count]["chanceOfRain"])["T00_06"]),
                        rain12  = (string)((forecasts[count]["chanceOfRain"])["T06_12"]),
                        rain18  = (string)((forecasts[count]["chanceOfRain"])["T12_18"]),
                        rain24  = (string)((forecasts[count]["chanceOfRain"])["T18_24"]),
                        image   = image,
                    }
                ); ;
            }

            this.lvItemList.ItemsSource = items;

        }
    }
}

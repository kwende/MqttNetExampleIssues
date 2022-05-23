using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AndroidForeverHang
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                try
                {
                    IMqttClient client = new MqttFactory().CreateMqttClient();
                    var clientOptions = new MqttClientOptionsBuilder()
                           .WithClientId("client")
                           .WithTcpServer("172.17.5.8", 1234)
                           .WithKeepAlivePeriod(TimeSpan.FromSeconds(5))
                           .WithCommunicationTimeout(TimeSpan.FromSeconds(5))
                           .Build();
                    await client.ConnectAsync(clientOptions);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex); 
                }
            }); 
        }
    }
}

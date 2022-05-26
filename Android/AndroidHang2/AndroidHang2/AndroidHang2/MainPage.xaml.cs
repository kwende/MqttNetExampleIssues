using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AndroidHang2
{
    public partial class MainPage : ContentPage
    {
        private Socket _socket = null; 

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCancel()
        {
            _socket?.Close(); 
            _socket?.Dispose();

            _socket = null; 
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    using(cancellationTokenSource.Token.Register(OnCancel))
                    {
                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                        try
                        {
                            await _socket.ConnectAsync("172.222.222.222", 1234);
                        }
                        catch(Exception ex)
                        {
                            // if your network is off, the timeout will occur, OnCancel will be called,
                            // close and dispose will be called, but this will never get hit. 
                        }
                    }
                }
            }); 
        }
    }
}

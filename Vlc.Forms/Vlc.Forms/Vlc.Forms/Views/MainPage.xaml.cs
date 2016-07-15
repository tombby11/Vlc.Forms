using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Vlc.Forms.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
           
        }

        private void Button_Play1_OnClicked(object sender, EventArgs e)
        {
            var channel = "http://us177.myvnc.com:1935/drm/ft.stream/playlist.m3u8";
            MessagingCenter.Send<object, string>(this, "Play", channel);
        }

        private void Button_Play2_OnClicked(object sender, EventArgs e)
        {
            var channel = "rtmp://38.96.148.30/live/omantv1";
            MessagingCenter.Send<object, string>(this, "Play",channel);
        }
        private void Button_Release_OnClicked(object sender, EventArgs e)
        {
             MessagingCenter.Send<object>(this, "Release");
        }

        private void Button_VolumeUp_OnClicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<object>(this, "VolumeUp");
        }
        private void Button_VolumeDown_OnClicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<object>(this, "VolumeDown");
        }
    }
}

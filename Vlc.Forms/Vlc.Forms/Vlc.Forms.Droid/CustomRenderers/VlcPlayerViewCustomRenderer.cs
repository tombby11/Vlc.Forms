using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Vlc.Forms;
using Vlc.Forms.Droid.CustomRenderers;
using Vlc.Forms.Droid.Players;
using Vlc.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Xamarin.Forms.View;

[assembly:ExportRenderer(typeof(VlcPlayerView), typeof(VlcPlayerViewCustomRenderer))]
namespace Vlc.Forms.Droid.CustomRenderers
{
  
    public class VlcPlayerViewCustomRenderer:ViewRenderer
    {
        VlcPlayer _videoView;
        public VlcPlayerViewCustomRenderer()
        {
            MessagingCenter.Subscribe<object,string>(this,"Play",OnPlay);
            MessagingCenter.Subscribe<object>(this, "VolumeUp", OnVolumeUp);
            MessagingCenter.Subscribe<object>(this, "VolumeDown", OnVolumeDown);
        }

     


        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            var vlcPlayerView = Element as VlcPlayerView;

            if ((vlcPlayerView == null) || (e.OldElement != null)) return;
            
            _videoView = new VlcPlayer(Context);
            _videoView.PlayerStateChanged += OnPlayerStateChanged;
            SetNativeControl(_videoView);
        }

        private void OnPlayerStateChanged(object sender, PlayerState state)
        {
            Toast.MakeText(Context, $"{state}", ToastLength.Short).Show();
        }

        private void OnVolumeUp(object obj)
        {
            _videoView.SetVolume(_videoView.Volume + 10);
        }

        private void OnVolumeDown(object obj)
        {
            _videoView.SetVolume(_videoView.Volume - 10);
        }

        private void OnRelease(object obj)
        {
            try {
                _videoView.Release();
                _videoView.Dispose();
            }
            catch (Exception ex) {


            }

        }
        private void OnPlay(object obj, string media)
        {
            try
            {
                if (_videoView.IsPlaying)
                {
                    _videoView.Stop();
                }
                _videoView.Play(Android.Net.Uri.Parse(media));
            }
            catch (Exception ex)
            {
                    
                throw;
            }
            
        }
    }
}
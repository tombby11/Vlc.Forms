using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Uri = Android.Net.Uri;
using Org.Videolan.Libvlc;

namespace Vlc.Forms.Droid.Players
{
    public enum PlayerState
    {
        Idle = 0, Opening = 1, Playing = 3, Paused = 4, Stopping = 5, Ended = 6, Error = 7
    }
    public sealed class VlcPlayer : SurfaceView
    {
        private readonly MediaPlayer _mediaPlayer;
        private LibVLCLibVLC _libvlc;
        private MediaLibVLC _media;
        private PlayerState _playerState;
        private int _volume;

        public event EventHandler<PlayerState> PlayerStateChanged;

        public VlcPlayer(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            
        }

        public VlcPlayer(Context context) : base(context)
        {
            LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);
            _libvlc = new LibVLCLibVLC();
            _mediaPlayer = new MediaPlayer(_libvlc);
            _mediaPlayer.VLCVout.SetVideoView(this);
            _mediaPlayer.VLCVout.AttachViews();
            PlayerState = (PlayerState)_mediaPlayer.PlayerState;
            Volume = 50;
            MonitorPlayerState();
        }

      

        public int Volume
        {
            get { return _volume; }
            set
            {
                if (value>100 || value<0)
                {
                    return;
                }
                _volume = value; 
            }
        }

        public PlayerState PlayerState
        {
            get { return _playerState; }
            set
            {
                if (Equals(_playerState, value))
                    return;

                _playerState = value;
                PlayerStateChanged?.Invoke(this,value);
            }
        }

        public MediaLibVLC Media
        {
            get { return _media; }
            set
            {
                _media = value;
                if (_mediaPlayer != null)
                {
                    _mediaPlayer.Media = value;
                }
            }
        }

        public bool IsPlaying => _mediaPlayer != null && _mediaPlayer.IsPlaying;
        public bool IsDisposed { get; set; }
        public void Stop()
        {
            if (IsDisposed) {
                return;
            }
            _mediaPlayer?.Stop();
        }

        public void SetMedia(Uri media)
        {
            if (IsDisposed) {
                return;
            }
            if (_mediaPlayer == null) return;
            _mediaPlayer.Media = new MediaLibVLC(_libvlc, media);
        }

        public void Play()
        {
            if (IsDisposed) {
                return;
            }
            _mediaPlayer?.Play();
        }

        public void Play(Uri media)
        {
            if (IsDisposed) {
                return;
            }
            try
            {
                if (_mediaPlayer == null) return;
                _mediaPlayer.Media = new MediaLibVLC(_libvlc, media);
                _mediaPlayer.SetVolume(Volume); 
                _mediaPlayer.Play();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Release()
        {
            if (IsDisposed) {
                return;
            }
            try
            {
                if (_mediaPlayer == null || _mediaPlayer.IsReleased)
                {
                    return;
                }

                _mediaPlayer.Stop();
                _mediaPlayer.VLCVout.DetachViews();
                _mediaPlayer.Release();
                _mediaPlayer.Dispose();
                _libvlc?.Release();
                _libvlc = null;
                Dispose();
                IsDisposed = true;
                 
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EventHardwareAccelerationError()
        {
            Release();
            Toast.MakeText(Context, "Error with hardware acceleration", ToastLength.Long).Show();
        }

        public void SetVolume(int level)
        {
            Volume = level; 
            _mediaPlayer.SetVolume(level);
        } 
        private async void  MonitorPlayerState()
        {
            while (true)
            {
                await Task.Delay(500);
                if (_mediaPlayer!=null)
                {
                    try
                    {
                        if (IsDisposed)
                        {
                            return;
                        }
                        PlayerState = (PlayerState)_mediaPlayer.PlayerState;
                    }
                    catch (Exception ex)
                    {
                        
                        //throw;
                    }
                  
                }
            }
        }
    }
}
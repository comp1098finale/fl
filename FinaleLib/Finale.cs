using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Microsoft.Win32;
using System.Security;

namespace FinaleLib
{
    
    public partial class Finale : Form
    {
        private System.Windows.Forms.Timer t;
        private System.Windows.Forms.Timer t2;
        private System.Windows.Forms.Timer t_terminate;
        private CoreAudioDevice defaultPlaybackDevice;
        private int LN = 0;
        private byte cnt = 0;
        private Thread r;
        private String yt;
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        public Finale()
        {
            if (!IsBrowserEmulationSet())
            {
                SetBrowserEmulationVersion();
            }
            t_terminate = new System.Windows.Forms.Timer();
            

            LN = new Random().Next(1, 10);
            switch (LN)
            {
                
                case 4:
                case 7:
                    yt = "DcJFdCmN98s";
                    t_terminate.Interval = 145000;
                    break;
                case 2:
                case 3:
                    yt = "2Z4m4lnjxkY";
                    t_terminate.Interval = 165000;
                    LN = 1;
                    break;

                case 1:
                case 5:
                case 6:
                    t_terminate.Interval = 228000;
                    yt = "8fvTxv46ano";
                    LN = 2;
                    break;
                default:
                    t_terminate.Interval = 106000;
                    yt = "IdRo2NJJcUU";
                    LN = 3;
                    break;                    
            }
            if (Environment.MachineName.ToUpper() == "GC-5CBZX52")
                t_terminate.Interval = 15000;

            t_terminate.Tick += T_terminate_Tick;
            t_terminate.Start();
            InitializeComponent();
            Load += Finale_Load;
            FormClosing += Finale_FormClosing;
        }

        private void Finale_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;          
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:                   
                    e.Cancel = true;
                    browser.Url = new Uri("https://player.vimeo.com/video/45196609?autoplay=1&loop=1");
                    t_terminate.Interval = 23000;
                    break;
            }
            base.OnFormClosing(e);
        }

        private void T_terminate_Tick(object sender, EventArgs e)
        {
            close();
        }

        private void close()
        {
            this.FormClosing -= this.Finale_FormClosing;
            r.Abort(); //terminate our volume adjuster ;)            
            Application.Exit();
        }

        private void ThreadWorker()
        {
            keybd_event((byte)Keys.VolumeUp, 0, 0, 0); // increase volume
            System.Threading.Thread.Sleep(50);
            defaultPlaybackDevice.Mute(false);
            defaultPlaybackDevice.Volume = 50;
        }

        private void Finale_Load(object sender, EventArgs e)
        {
            defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            defaultPlaybackDevice.Mute(false);
            defaultPlaybackDevice.Volume = 100;
            r = new Thread(ThreadWorker);
            r.Start();

            // Full screen
            this.FormBorderStyle = FormBorderStyle.None;
            Screen screen = Screen.FromControl(this); // get screen for form
            this.Bounds = screen.Bounds;
            this.TopMost = true;
            this.BackColor = Color.Black;
            t = new System.Windows.Forms.Timer();
            t.Tick += T_Tick;
            t.Interval = 10;
            t.Start();                

            t2 = new System.Windows.Forms.Timer();
            t2.Tick += T2_Tick;
            t2.Interval = 14;
            t2.Start();            
            LuckyNumber.Text = "LUCKY NUMBER: " + LN.ToString();
            LuckyNumber.BringToFront();
            LuckyNumber.Height = this.Height;
            LuckyNumber.Width = this.Width;
            LuckyNumber.Visible = true;
            LuckyNumber.ForeColor = Color.Black;
            LuckyNumber.Update();
            LuckyNumber.Refresh();
            browser.Hide();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            this.Activate();
        }

        #region IE FIXES

        private const string BrowserEmulationKey = InternetExplorerRootKey + @"\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        private enum BrowserEmulationVersion
        {
            Default = 0,
            Version7 = 7000,
            Version8 = 8000,
            Version8Standards = 8888,
            Version9 = 9000,
            Version9Standards = 9999,
            Version10 = 10000,
            Version10Standards = 10001,
            Version11 = 11000,
            Version11Edge = 11001
        }
        private static BrowserEmulationVersion GetBrowserEmulationVersion()
        {
            BrowserEmulationVersion result;

            result = BrowserEmulationVersion.Default;

            try
            {
                RegistryKey key;

                key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
                if (key != null)
                {
                    string programName;
                    object value;

                    programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
                    value = key.GetValue(programName, null);

                    if (value != null)
                    {
                        result = (BrowserEmulationVersion)Convert.ToInt32(value);
                    }
                }
            }
            catch (SecurityException)
            {
                // The user does not have the permissions required to read from the registry key.
            }
            catch (UnauthorizedAccessException)
            {
                // The user does not have the necessary registry rights.
            }

            return result;
        }

        private static bool IsBrowserEmulationSet()
        {
            return GetBrowserEmulationVersion() != BrowserEmulationVersion.Default;
        }
        private static bool SetBrowserEmulationVersion(BrowserEmulationVersion browserEmulationVersion)
        {
            bool result;

            result = false;

            try
            {
                RegistryKey key;

                key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);

                if (key != null)
                {
                    string programName;

                    programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);

                    if (browserEmulationVersion != BrowserEmulationVersion.Default)
                    {
                        // if it's a valid value, update or create the value
                        key.SetValue(programName, (int)browserEmulationVersion, RegistryValueKind.DWord);
                    }
                    else
                    {
                        // otherwise, remove the existing value
                        key.DeleteValue(programName, false);
                    }

                    result = true;
                }
            }
            catch (SecurityException)
            {
                // The user does not have the permissions required to read from the registry key.
            }
            catch (UnauthorizedAccessException)
            {
                // The user does not have the necessary registry rights.
            }

            return result;
        }
        private const string InternetExplorerRootKey = @"Software\Microsoft\Internet Explorer";

        private static int GetInternetExplorerMajorVersion()
        {
            int result;

            result = 0;

            try
            {
                RegistryKey key;

                key = Registry.LocalMachine.OpenSubKey(InternetExplorerRootKey);

                if (key != null)
                {
                    object value;

                    value = key.GetValue("svcVersion", null) ?? key.GetValue("Version", null);

                    if (value != null)
                    {
                        string version;
                        int separator;

                        version = value.ToString();
                        separator = version.IndexOf('.');
                        if (separator != -1)
                        {
                            int.TryParse(version.Substring(0, separator), out result);
                        }
                    }
                }
            }
            catch (SecurityException)
            {
                // The user does not have the permissions required to read from the registry key.
            }
            catch (UnauthorizedAccessException)
            {
                // The user does not have the necessary registry rights.
            }

            return result;
        }
        private static bool SetBrowserEmulationVersion()
        {
            int ieVersion;
            BrowserEmulationVersion emulationCode;

            ieVersion = GetInternetExplorerMajorVersion();

            if (ieVersion >= 11)
            {
                emulationCode = BrowserEmulationVersion.Version11;
            }
            else
            {
                switch (ieVersion)
                {
                    case 10:
                        emulationCode = BrowserEmulationVersion.Version10;
                        break;
                    case 9:
                        emulationCode = BrowserEmulationVersion.Version9;
                        break;
                    case 8:
                        emulationCode = BrowserEmulationVersion.Version8;
                        break;
                    default:
                        emulationCode = BrowserEmulationVersion.Version7;
                        break;
                }
            }

            return SetBrowserEmulationVersion(emulationCode);
        }

        #endregion
        private void setYt(String id)
        {
            browser.Visible = true;
            browser.DocumentText = @"<div id=""ytplayer""></div>
                                    <script>
                                    // Load the IFrame Player API code asynchronously.
                                    var tag = document.createElement('script');
                                                    tag.src = ""https://www.youtube.com/player_api"";
                                                    var firstScriptTag = document.getElementsByTagName('script')[0];
                                                    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
                                                    // Replace the 'ytplayer' element with an <iframe> and
                                                    // YouTube player after the API code downloads.
                                                    var player;
                                                    function onYouTubePlayerAPIReady()
                                                    {
                                                        player = new YT.Player('ytplayer', {
                                            height: '" + this.Height + @"',
                                            width: '" + this.Width + @"',
                                            videoId: '" + id + @"',
                                            playerVars:
                                                    {
                                                        'autoplay': 1,
                                                'showinfo': 0,
                                                'controls': 0
                                            }
                                                });
                                            }
                                    </script>";
        }
        private void T2_Tick(object sender, EventArgs e)
        {            
            cnt += 3;
            this.BackColor = Color.FromArgb(cnt, cnt, cnt);
            LuckyNumber.Update();
            LuckyNumber.Refresh();
            if (cnt >= 255)
            {
                
                cnt--;
                t2.Dispose();
                LuckyNumber.Visible = false;
                setYt(yt);                
            }
        }
    }
}

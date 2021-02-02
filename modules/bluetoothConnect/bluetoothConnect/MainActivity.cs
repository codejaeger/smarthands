using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Android.Views;
using Android.Bluetooth;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

using Java.IO;
using Java.Util;

namespace bluetoothConnect
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        BluetoothConnection myConnection = new BluetoothConnection();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Button buttonConnect = FindViewById<Button>(Resource.Id.button1);
            Button buttonDisconnect = FindViewById<Button>(Resource.Id.button2);

            Button button1On = FindViewById<Button>(Resource.Id.button3);
            TextView connected = FindViewById<TextView>(Resource.Id.textView1);

            System.Threading.Thread listenThread = new System.Threading.Thread(listener);
            listenThread.Abort();

            myConnection = new BluetoothConnection();

            buttonConnect.Click += delegate {

                //listenThread.Start();
                
                myConnection.getPairedDevice();
                myConnection.Connect();

                connected.Text = "Connected!";
                buttonDisconnect.Enabled = true;
                buttonConnect.Enabled = false;

                if (listenThread.IsAlive == false)
                {
                    listenThread.Start();
                }
            };

            buttonDisconnect.Click += delegate {

                try
                {
                    //  buttonDisconnect.Enabled = false;
                    buttonConnect.Enabled = true;
                    listenThread.Abort();
                    myConnection.Close();

                    //myConnection.thisSocket.OutputStream.WriteByte(187);

                    connected.Text = "Disconnected!";
                }
                catch { }
            };


            button1On.Click += delegate {

                try
                {
                    myConnection.write(1);
                }
                catch (Exception outPutEX)
                {
                    throw outPutEX;
                }

            };
        }

        void listener()
        {
            byte read;

            TextView readTextView = FindViewById<TextView>(Resource.Id.textView2);
            
            while (true)
            {
                try
                {
                    read = myConnection.read();
                    RunOnUiThread(() =>
                    {
                        if (read == 1)
                        {
                            readTextView.Text = "Relais AN";
                        }
                    });
                }
                catch { }
            }
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }


    public class BluetoothConnection
    {
        // UniqueID to connect to any device
        private const string UuidUniverseProfile = "00001101-0000-1000-8000-00805f9b34fb";
        // represent bluetooth data from UART
        private BluetoothDevice result { get; set; }
        // get input/output stream of this communication
        private BluetoothSocket mSocket { get; set; }
        // Convert bytes to readble string
        private BufferedReader reader;
        private System.IO.Stream inStream;
        private System.IO.Stream outStream;
        private InputStreamReader inReader;

        public BluetoothConnection()
        {
            reader = null;
        }

        public byte read()
        {
            byte[] data = new byte[1];
            int bytesread = inStream.Read(data, 0, 1);
            inStream.Close();
            if (bytesread > 0)
                return data[0];
            else
                return 0;
        }

        public void write(byte data)
        {
            outStream.WriteByte(data);
            outStream.Close();
        }

        private UUID _getUUIDFromString()
        {
            return UUID.FromString(UuidUniverseProfile);
        }

        private void _close(IDisposable aConnectedObject)
        {
            if (aConnectedObject == null) return;
            try
            {
                aConnectedObject.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
            aConnectedObject = null;
        }

        private void _openDeviceConnection(BluetoothDevice btDevice)
        {
            try
            {
                mSocket = btDevice.CreateRfcommSocketToServiceRecord(_getUUIDFromString());
                mSocket.Connect();
                inStream = mSocket.InputStream;
                outStream = mSocket.OutputStream;
                inReader = new InputStreamReader(inStream);
                
            }
            catch(IOException e)
            {
                Close();
                throw e;
            }
        }

        public void Close()
        {
            _close(mSocket);
            _close(inStream);
            _close(outStream);
            _close(inReader);
        }


        public void getPairedDevice()
        {
            BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
            var devices = btAdapter.BondedDevices;
            if(devices != null && devices.Count() > 0)
            {
                foreach (BluetoothDevice mDevice in devices)
                {
                    if (mDevice.Name == "HC-05")
                    {
                        result = mDevice;
                        break;
                    }
                }
            }
        }

        public void Connect()
        {
            _openDeviceConnection(result);
        }

    }
}
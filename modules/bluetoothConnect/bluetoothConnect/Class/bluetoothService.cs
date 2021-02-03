using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Widget;
using Java.Util;
using Android.Bluetooth;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

using bluetoothConnect;

namespace bluetoothConnect.Class.Service
{
    public class BluetoothService
    {
        private BluetoothAdapter mBluetoothAdapter;
        private BluetoothDevice mbarBotDevice;
        private BluetoothSocket mSocket;
        private System.IO.Stream mInStream;
        private System.IO.Stream mOutStream;
        //private Toast toastMessenger;
        //public MainActivity context;


        //public BluetoothService(Context context)
        //{
        //    this.context = context;


        //    context.RunOnUiThread(() =>
        //    toastMessenger = new Toast(this.context));
        //}

        #region Write, Read
        public async void ReadData(byte[] InputData)
        {
            int numBytes; // bytes returned from read()

            Stopwatch timer = new Stopwatch();
            timer.Start();

            while (true)
            {
                try
                {
                    System.Console.WriteLine("reciving...");
                    if (timer.ElapsedMilliseconds >= 20000)
                    {
                        break;
                    }
                    numBytes = await mInStream.ReadAsync(InputData, 0, 1);
                    // Send the obtained bytes to the UI activity.
                    if (numBytes > 0)
                    {
                        System.Console.WriteLine(InputData[0]);
                    }

                }
                catch (System.IO.IOException e)
                {
                    System.Console.WriteLine("InputStream failure ERROR:4084");
                    System.Console.WriteLine(e.Message);
                    throw;
                }

            }
        }

        //Call this from the main activity to send data to the remote device.
        //public async void WriteAsync(byte[] bytes)
        //{
        //    try
        //    {
        //        await mOutStream.WriteAsync(bytes, 0, 1);
        //        System.Console.WriteLine("--SEND, ASYNC :  " + ASCIIEncoding.ASCII.GetString(bytes));
        //        ShowToastMessage("Writing: " + ASCIIEncoding.ASCII.GetString(bytes), ToastLength.Long);

        //    }
        //    catch (System.IO.IOException e)
        //    {
        //        System.Console.WriteLine("Could not SEND Error:6548");
        //        System.Console.WriteLine(e.Message);

        //        throw;
        //    }
        //}

        public void Write(byte bytes)
        {
            try
            {
                mOutStream.WriteByte(bytes);
                System.Console.WriteLine("--SEND, NO ASYNC :  " + bytes);
                //ShowToastMessage("Writing: " + bytes, ToastLength.Long);
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine("Could not SEND Error:6548");
                System.Console.WriteLine(e.Message);

                throw;
            }
        }


        #endregion

        #region Initialize Bluetooth sequence

        public bool ConnectActivateBluetooth()
        {

            if (!DeviceHasBT()) return false;

            if (!EnableBTdevice().Result) return false;

            if (!GetBondedDevices()) return false;

            if (!SocketConnect()) return false;

            if (!InOutSocketInit()) return false;

            //ShowToastMessage("CONNECTED TO BARBOT", ToastLength.Short);

            return true;
        }

        public bool DeviceHasBT()
        {
            mBluetoothAdapter = mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (mBluetoothAdapter == null)
            {
                //ShowToastMessage("Error no BT found on device", ToastLength.Long);
                return false;
            }
            else
            {
                //ShowToastMessage("Got adapter", ToastLength.Short);
            }
            return true;

        }

        public async Task<bool> EnableBTdevice()
        {
            int REQUEST_ENABLE_BT = 0;


            // Enable BT
            if (!mBluetoothAdapter.IsEnabled)
            {
                //Intent enableBT = new Intent(BluetoothAdapter.ActionRequestEnable);
                //Activity s = new Activity();

                //// TODO: Check for cancelation

                ////Thread BToutput = new Thread(() => { context.StartActivityForResult(enableBT, REQUEST_ENABLE_BT); });
                //context.StartActivityForResult(enableBT, REQUEST_ENABLE_BT);

                //context.adapterOnActivityResult(REQUEST_ENABLE_BT, Result.Canceled, enableBT);


                if (!mBluetoothAdapter.IsEnabled)
                {
                    //ShowToastMessage("BT not enabled", ToastLength.Long);

                    return false;
                }
            }
            //ShowToastMessage("Enabled BT", ToastLength.Long);
            return true;
        }

        public bool GetBondedDevices()
        {
            // Get Bonded Devices
            mbarBotDevice = (from x in mBluetoothAdapter.BondedDevices
                             where x.Name == ("HC-05")
                             select x).FirstOrDefault();

            if (mbarBotDevice == null)
            {
                //ShowToastMessage("Manual pair connect to BarBot please.", ToastLength.Long);
                return false;
            }
            else
            {
                //ShowToastMessage("Found BarBot bond", ToastLength.Short);
                return true;
            }
        }

        public bool SocketConnect()
        {
            BluetoothSocket tmpSocket;
            // Set up socket
            try
            {
                UUID id = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
                tmpSocket = mbarBotDevice.CreateInsecureRfcommSocketToServiceRecord(id);
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine("Socket's listen() method failed");
                //ShowToastMessage("Found BarBot bond", ToastLength.Short);
                return false;
            }

            mSocket = tmpSocket;


            // Drive on a diffrent Socket
            mBluetoothAdapter.CancelDiscovery();
            try
            {
                mSocket.Connect();
            }
            catch (Exception e)
            {
                try
                {
                    System.Console.WriteLine("\n\nClosing connection:\n");
                    System.Console.WriteLine(e.Message);
                    mSocket.Close();
                    return false;

                }
                catch (Exception r)
                {
                    System.Console.WriteLine("Could not close the client socket");
                    System.Console.WriteLine(r.Message);
                    return false;
                }
            }
            Thread.Sleep(1500);
            return true;
        }

        private bool InOutSocketInit()
        {

            // Get the input and output streams; using temp objects because
            // member streams are .
            try
            {
                mInStream = mSocket.InputStream;
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine("InputStream Socket fail to establish ERROR:4856");
                System.Console.WriteLine(e.Message);
                return false;
            }


            try
            {
                mOutStream = mSocket.OutputStream;
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine("OutputStream Socket fail to establish ERROR:4857");
                System.Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        // Call this method from the main activity to shut down the connection.
        public void cancelSocketServ()
        {
            try
            {
                mSocket.Close();
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine("Could not cancel socket");
                throw;
            }
        }

        #endregion

        //public void ShowToastMessage(String message, ToastLength ts)
        //{
        //    context.RunOnUiThread(() =>
        //    {
        //        toastMessenger = Toast.MakeText(context, message, ts);
        //        toastMessenger.Show();
        //    });
        //}

    }
}

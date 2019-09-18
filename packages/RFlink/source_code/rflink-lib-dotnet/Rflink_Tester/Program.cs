using System;
using System.Diagnostics;
using System.IO;
using RFLink;

namespace Rflink_Tester
{
    class MainClass
    {
        private static RFLinkLib rflink = null;
        private static bool isrunning = true;
        private static bool restart = false;

        public static void Main (string [] args)
        {
            Console.WriteLine ("Hello World!");

            string myHexNumber = "0095";//"80DC";
            Int16 decValue = Convert.ToInt16 (myHexNumber, 16); // This will be -337314240
            Int16 decmask = 0x7FFF;
            decValue = (short)((decValue >> 15)*(decValue & decmask));

            Int16 tmp = -228;
            var toto = ((float)tmp /(float) 10).ToString ("0.00");

            //Int16 tmp2 = 0x8000;

            Console.WriteLine ($" { tmp.ToString ("x") }");
            /* Change cursrent culture
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            */
            var os = Environment.OSVersion;
            var platform = os.Platform;

            // TODO: run "uname" to determine OS type
            if (platform == PlatformID.Unix) {

                var libusblink = Path.Combine (AppDomain.CurrentDomain.BaseDirectory, "libusb-1.0.so");

                // RaspBerry Pi armel dependency check and needed symlink
                // TODO: check for armhf version
                if (File.Exists ("/lib/arm-linux-gnueabi/libusb-1.0.so.0.1.0") && !File.Exists (libusblink)) {
                    ShellCommand ("ln", " -s \"/lib/arm-linux-gnueabi/libusb-1.0.so.0.1.0\" \"" + libusblink + "\"");
                }

                // Debian/Ubuntu 64bit dependency and needed symlink check
                if (File.Exists ("/lib/x86_64-linux-gnu/libusb-1.0.so.0") && !File.Exists (libusblink)) {
                    ShellCommand ("ln", " -s \"/lib/x86_64-linux-gnu/libusb-1.0.so.0\" \"" + libusblink + "\"");
                }

            }
            //
            Console.CancelKeyPress += new ConsoleCancelEventHandler (Console_CancelKeyPress);
            //
            AppDomain.CurrentDomain.SetupInformation.ShadowCopyFiles = "true";
            //
            rflink = new RFLinkLib ();
            rflink.Debug = true;
            rflink.Connect ();
            //
            do { System.Threading.Thread.Sleep (2000); } while (isrunning);
        }
        private static void ShellCommand (string command, string args)
        {
            try {
                var processInfo = new ProcessStartInfo (command, args);
                processInfo.RedirectStandardOutput = false;
                processInfo.UseShellExecute = false;
                processInfo.CreateNoWindow = true;
                var process = new Process ();
                process.StartInfo = processInfo;
                process.Start ();
            } catch { }
        }
        private static void Console_CancelKeyPress (object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine ("\n\nProgram interrupted!\n");
            Quit (false);
        }
        internal static void Quit (bool restartService)
        {
            restart = restartService;
            ShutDown ();
            isrunning = false;
        }

        private static void ShutDown ()
        {
            Console.Write ("rflink is now exiting...\n");
            //
            if (rflink != null) {
                rflink.Disconnect ();
                rflink = null;
            }
            //
            int exitCode = 0;
            if (restart) {
                exitCode = 1;
                Console.Write ("\n\n...RESTART!\n\n");
            } else {
                Console.Write ("\n\n...QUIT!\n\n");
            }
            //
            Environment.Exit (exitCode);
        }
    }
}

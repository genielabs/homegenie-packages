using System;
namespace RFLink
{
    public static class RFLink_control
    {
        #region Constantes
        private const string beginRequest = "10;";
        private const string endRequest = ";\r\n";
        #endregion

        #region Enumerates
        public enum Command{
            ON,
            OFF,
            ALLON,
            ALLOFF,
            UP,
            DOWN,
            STOP,
            PAIR
        }
        #endregion

        #region Comments
        //10;REBOOT; => Reboot RFLink Gateway hardware
        //10;PING; => a "keep alive" function. Is replied with: 20;99;PONG;
        //10;VERSION; => Version and build indicator. Is replied with: 20;99;"RFLink Gateway software version";
        //10;RFDEBUG=ON; => ON/OFF to Enable/Disable showing of RF packets. Is replied with: 20;99;RFDEBUG="state";
        //10;RFUDEBUG=ON; => ON/OFF to Enable/Disable showing of undecoded RF packets. Is replied with: 20;99;RFUDEBUG="state";
        //10;QRFDEBUG=ON; => ON/OFF to Enable/Disable showing of undecoded RF packets. Is replied with: 20;99;QRFDEBUG="state";
        //QRFDEBUG is a faster version of RFUDEBUG but all pulse times are shown in hexadecimal and need to be multiplied by 30
        //10;TRISTATEINVERT; => Toggle Tristate ON/OFF inversion
        //10;RTSCLEAN; => Clean Rolling code table stored in internal EEPROM
        //10;RTSRECCLEAN=9 => Clean Rolling code record number (value from 0 - 15)
        //10;RTSSHOW; => Show Rolling code table stored in internal EEPROM (includes RTS settings)
        //10;RTSINVERT; => Toggle RTS ON/OFF inversion
        //10;RTSLONGTX; => Toggle RTS long transmit ON/OFF
        #endregion

        #region Evenements
        //Item is deleted if the last request is very old
        public  delegate void LogHandle (string id);
        public static event LogHandle EvtLog;
        #endregion

        #region Gateway command
        public static byte [] RebootRequest ()
        {
            return convertToByteWithFormat ("REBOOT");
        }
        public static byte [] PingRequest ()
        {
            return convertToByteWithFormat ("PING");
        }
        public static byte [] VersionRequest ()
        {
            return convertToByteWithFormat ("VERSION");
        }
        public static byte [] StatusRequest ()
        {
            return convertToByteWithFormat ("STATUS");
        }
        public static byte [] RFDebugRequest (bool param)
        {
            return convertToByteWithFormat ("RFDEBUG=" + (param ? "ON" : "OFF"));
        }
        public static byte [] RFUDebugRequest (bool param)
        {
            return convertToByteWithFormat ("RFUDEBUG=" + (param ? "ON" : "OFF"));
        }
        public static byte [] QRFDebugRequest (bool param)
        {
            return convertToByteWithFormat ("QRFDEBUG=" + (param ? "ON" : "OFF"));
        }

        public static byte [] RemoteRequest (string protocol, string id, string Chanal,string cmd)
        {
            return convertToByteWithFormat (protocol + ";" + id + ";" + Chanal + ";" + cmd );
        }

        #endregion

        #region Gateway Mods
        public static byte [] RF433Request (bool param)
        {
            return convertToByteWithFormat ("setRF433=" + (param ? "ON" : "OFF"));
        }


        public static byte [] NodoNRFRequest (bool param)
        {
            return convertToByteWithFormat ("setNodoNRF=" + (param ? "ON" : "OFF"));
        }

        public static byte [] MiLightRequest (bool param)
        {
            return convertToByteWithFormat ("setMilight=" + (param ? "ON" : "OFF"));
        }

        public static byte [] BLERequest (bool param)
        {
            return convertToByteWithFormat ("setBLE=" + (param ? "ON" : "OFF"));
        }

        public static byte [] MySensorRequest (bool param)
        {
            return convertToByteWithFormat ("setMySensors=" + (param ? "ON" : "OFF"));
        }

        public static byte [] LivingColorsRequest (bool param)
        {
            return convertToByteWithFormat ("setLivingColors=" + (param ? "ON" : "OFF"));
        }

        public static byte [] AnslutaRequest (bool param)
        {
            return convertToByteWithFormat ("setAnsluta=" + (param ? "ON" : "OFF"));
        }

        public static byte [] GPIORequest (bool param)
        {
            return convertToByteWithFormat ("setGPIO=" + (param ? "ON" : "OFF"));
        }
        #endregion

        #region Private methods
        private static byte [] convertToByteWithFormat (string data)
        {
            EvtLog?.Invoke (data);
            return ConvertToByte (beginRequest + data + endRequest);
        }
        private static byte [] ConvertToByte (string data)
        {
            return System.Text.Encoding.UTF8.GetBytes ( data );
        }
        #endregion

    }
}

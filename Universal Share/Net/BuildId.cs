using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace Universal_Share.Net {
    internal class BuildId {
        private static readonly SHA512       Sha512 = SHA512.Create();
        private readonly        List<string> _fref  = new List<string>();


        private void GrabHdware() {
            this._fref.RemoveRange( 0, this._fref.Count );
            this._fref.AddRange( GetNetworkMacs() );
            this._fref.Add( Environment.UserName );
            this._fref.Add( Environment.MachineName );
            this._fref.Add( Environment.OSVersion.VersionString );
        }

        private IEnumerable<string> GetNetworkMacs() {
            foreach ( var n in NetworkInterface.GetAllNetworkInterfaces() ) {
                yield return Convert.ToBase64String( n.GetPhysicalAddress().GetAddressBytes() );
            }
        }

        private string _id = "";

        private void MakeId() {
            this._id = "";
            foreach ( var pref in this._fref ) {
                this._id += Encoding.UTF8.GetString( Sha512.ComputeHash( Encoding.UTF8.GetBytes( pref ) ) );
            }
        }

        private const int CHUNK = 4;
        private string Computeid() {
            var ret = "";
            GrabHdware();
            MakeId();

            var intervall = Sha512.ComputeHash( new[] { (byte) 000, (byte) 111, (byte) 222, (byte) 123 } ).Length / CHUNK;

            for ( var i = 0; i < CHUNK; i++ ) {

                var byt = Sha512.ComputeHash( Encoding.Unicode.GetBytes( this._id.Substring( intervall * i, intervall ) ) ).ToList();
                byt.Add( 111 );
                byt.Add( 111 );

                ret += Convert.ToBase64String( byt.ToArray() );
            }

            return ret;
        }

        public string MakeIdS => Computeid();
        public string FullId => ComputeFullId();

        private string ComputeFullId() {
            GrabHdware();
            MakeId();
            return Convert.ToBase64String( Encoding.UTF8.GetBytes( this._id ) );
        }
    }
}
/*
namespace Security {
    /// <summary>
    /// Generates a 16 byte Unique Identification code of a computer
    /// Example: 4876-8DB5-EE85-69D3-FE52-8CF7-395D-2EA9
    /// </summary>
    public class FingerPrint {
        private static string fingerPrint = string.Empty;

        public static string Value() {
            if ( string.IsNullOrEmpty( fingerPrint ) ) {
                fingerPrint = GetHash( "CPU >> "    +
                                       cpuId()      +
                                       "\nBIOS >> " +
                                       biosId()     +
                                       "\nBASE >> " +
                                       baseId()     +
                                       //"\nDISK >> "+ diskId() + "\nVIDEO >> " + 
                                       videoId()   +
                                       "\nMAC >> " +
                                       macId() );
            }

            return fingerPrint;
        }

        private static string GetHash(string s) {
            MD5           sec = new MD5CryptoServiceProvider();
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[]        bt  = enc.GetBytes( s );
            return GetHexString( sec.ComputeHash( bt ) );
        }

        private static string GetHexString(byte[] bt) {
            string s = string.Empty;
            for ( int i = 0; i < bt.Length; i++ ) {
                byte b = bt[i];
                int  n, n1, n2;
                n  = (int) b;
                n1 = n          & 15;
                n2 = ( n >> 4 ) & 15;
                if ( n2 > 9 )
                    s += ( (char) ( n2 - 10 + (int) 'A' ) ).ToString();
                else
                    s += n2.ToString();
                if ( n1 > 9 )
                    s += ( (char) ( n1 - 10 + (int) 'A' ) ).ToString();
                else
                    s += n1.ToString();
                if ( ( i + 1 ) != bt.Length && ( i + 1 ) % 2 == 0 ) s += "-";
            }

            return s;
        }

        #region Original Device ID Getting Code

        //Return a hardware identifier
        private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue) {
            string                     result = "";
            ManagementClass            mc     = new ManagementClass( wmiClass );
            ManagementObjectCollection moc    = mc.GetInstances();
            foreach ( ManagementObject mo in moc ) {
                if ( mo[wmiMustBeTrue].ToString() == "True" ) {
                    //Only get the first one
                    if ( result == "" ) {
                        try {
                            result = mo[wmiProperty].ToString();
                            break;
                        } catch { }
                    }
                }
            }

            return result;
        }

        //Return a hardware identifier
        private static string identifier(string wmiClass, string wmiProperty) {
            string                     result = "";
            ManagementClass            mc     = new ManagementClass( wmiClass );
            ManagementObjectCollection moc    = mc.GetInstances();
            foreach ( ManagementObject mo in moc ) {
                //Only get the first one
                if ( result == "" ) {
                    try {
                        result = mo[wmiProperty].ToString();
                        break;
                    } catch { }
                }
            }

            return result;
        }

        public static string cpuId() {
            //Uses first CPU identifier available in order of preference
            //Don't get all identifiers, as it is very time consuming
            string retVal = identifier( "Win32_Processor", "UniqueId" );
            if ( retVal == "" ) //If no UniqueID, use ProcessorID
            {
                retVal = identifier( "Win32_Processor", "ProcessorId" );
                if ( retVal == "" ) //If no ProcessorId, use Name
                {
                    retVal = identifier( "Win32_Processor", "Name" );
                    if ( retVal == "" ) //If no Name, use Manufacturer
                    {
                        retVal = identifier( "Win32_Processor", "Manufacturer" );
                    }

                    //Add clock speed for extra security
                    retVal += identifier( "Win32_Processor", "MaxClockSpeed" );
                }
            }

            return retVal;
        }

        //BIOS Identifier
        public static string biosId() { return identifier( "Win32_BIOS", "Manufacturer" ) + identifier( "Win32_BIOS", "SMBIOSBIOSVersion" ) + identifier( "Win32_BIOS", "IdentificationCode" ) + identifier( "Win32_BIOS", "SerialNumber" ) + identifier( "Win32_BIOS", "ReleaseDate" ) + identifier( "Win32_BIOS", "Version" ); }

        //Main physical hard drive ID
        public static string diskId() { return identifier( "Win32_DiskDrive", "Model" ) + identifier( "Win32_DiskDrive", "Manufacturer" ) + identifier( "Win32_DiskDrive", "Signature" ) + identifier( "Win32_DiskDrive", "TotalHeads" ); }

        //Motherboard ID
        public static string baseId() { return identifier( "Win32_BaseBoard", "Model" ) + identifier( "Win32_BaseBoard", "Manufacturer" ) + identifier( "Win32_BaseBoard", "Name" ) + identifier( "Win32_BaseBoard", "SerialNumber" ); }

        //Primary video controller ID
        public static string videoId() { return identifier( "Win32_VideoController", "DriverVersion" ) + identifier( "Win32_VideoController", "Name" ); }

        //First enabled network card ID
        public static string macId() { return identifier( "Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled" ); }

        #endregion
    }
}*/
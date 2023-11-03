using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Linq;
using OpenSwimScoreboard;
using OpenSwimScoreboard.Parameters;

namespace OpenSwimScoreboard.DataReaders
{
    /// <summary>
    /// Used to detect and select available serial ports, and specify baud rate.
    /// </summary>
    public static class SerialPorts
    {
        private static string[] _availablePorts = null;

        public static string[] AvailablePorts
        {
            private set { }
            get
            {
                if (_availablePorts == null)
                {
                    _availablePorts = SerialPort.GetPortNames().OrderBy(n => n).ToArray();
                }
                return _availablePorts;
            }
        }
        public static string[] AvailableBaudRates
        {
            private set { }
            get
            {
                return new string[]
                {
                    "9600",
                    "2400"
                };
            }
        }

        public static bool RequestToRun { get; set; } = true;

        public static bool SetSerialPortPrefs(string portName, string baudRate)
        {
            if (!string.IsNullOrWhiteSpace(portName) && !string.IsNullOrWhiteSpace(baudRate))
            {
                int baudRateInt;
                if (int.TryParse(baudRate, out baudRateInt))
                {
                    Preferences.BaudRate = baudRateInt;
                    Preferences.InputSerialPort = portName;
                    return true;
                }
            }
            return false;
        }
    }
}

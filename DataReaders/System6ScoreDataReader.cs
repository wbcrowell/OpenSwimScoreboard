using OpenSwimScoreboard.Forms;
using OpenSwimScoreboard.Parameters;
using OpenSwimScoreboard.Scoreboard.TimingData;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;

namespace OpenSwimScoreboard.DataReaders
{
    /// <summary>
    /// IScoreDataReader that reads typical data stream from Colorado Timing Systems timing console and posts it to the ScoreboardRegister.
    /// </summary>
    public class System6ScoreDataReader : IScoreDataReader
    {
        private const int DEFAULT_BAUD_RATE = 9600;
        private string _portName;
        private int _baudRate;
        private SerialPort _serialPort;
        private ScoreboardRegister _scoreboardRegister;
        private long _totalBytesRead = 0;
        private FileStream _writeFileStream= null;
        private DateTime _startWriteTime;

        public string FileName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string SerialPortName { get; set; }

        public int? BaudRate { get; set; }

        public bool IsRunning { get; set; } = false;

        public event EventHandler DataUpdate;

        public System6ScoreDataReader(PortDefinition portDefinition = null)
        {
            Preferences.ErrorMessages += $"Starting System6 Reader. ";
            IsRunning = true; //Assumes data reader will be started ASAP; avoids attemts to create the reader after stop. Set to false if constructor fails to create connection.

            var parity = Parity.Even;

            if (portDefinition != null)
            {
                SerialPortName = portDefinition.PortName;
                BaudRate = portDefinition.BaudRate ?? DEFAULT_BAUD_RATE;
            }
            else
            {
                SerialPortName = Preferences.SerialPort;
                BaudRate = Preferences.BaudRate ?? DEFAULT_BAUD_RATE;
            }
            Preferences.ErrorMessages += $"Serial port set: {SerialPortName} @ {BaudRate} baud.";

            string[] serialPortNames = SerialPort.GetPortNames();
            if (string.IsNullOrWhiteSpace(SerialPortName) || !serialPortNames.Contains(SerialPortName))
            {
                if (serialPortNames.Any())
                {
                    string oldSerialPortName = SerialPortName;
                    SerialPortName = serialPortNames.Last();
                    if (Preferences.MainInterfaceForm != null)
                    {
                        if (string.IsNullOrWhiteSpace(oldSerialPortName))
                        {
                            Preferences.ErrorMessages += $"Serial port not specified. Trying {SerialPortName}. ";

                        }
                        else
                        {
                            Preferences.ErrorMessages += $"Cannot find serial port {oldSerialPortName}. Trying {SerialPortName}. ";
                            Debug.WriteLine($"Cannot find serial port {oldSerialPortName}. Trying {SerialPortName}.");
                        }
                    }
                }
                else
                {
                    SerialPortName = null;
                    IsRunning = false;
                    Debug.WriteLine("Cannot open serial port. No serial ports available.");
                    Preferences.ErrorMessages += "Cannot open serial port. No serial ports available. ";
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(SerialPortName))
                {
                    Preferences.ErrorMessages += $"Serial port name is blank. ";
                }
                else
                {
                    Preferences.ErrorMessages += $"Serial port name is invalid. ";
                }
            }

            if (SerialPortName != null)
            {
                _serialPort = new SerialPort
                {
                    BaudRate = BaudRate ?? DEFAULT_BAUD_RATE,
                    Parity = parity,
                    PortName = SerialPortName,
                };
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceived);
            }
            else
            {
                _serialPort = null;
                IsRunning = false;
            }
        }

        public void Start(ScoreboardRegister scoreboardRegister)
        {
            IsRunning = true;
            _totalBytesRead = 0;
            if (_serialPort is null)
            {
                Debug.WriteLine("Cannot connect serial port. The connection has not been defined.");
                Preferences.ErrorMessages += "Cannot connect serial port. The connection has not been defined. ";

                if (Preferences.MainInterfaceForm != null)
                {
                    ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Serial port not found. Use \"offline data only\" mode to display scoreboard data");
                }
            }
            if (Preferences.WriteScoreboardDataFile)
            {
                try
                {
                    if (!Directory.Exists($"{Directory.GetCurrentDirectory()}\\testdata"))
                    {
                        Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\testdata");
                    }
                    string fileName = $"testdata_{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}";
                    string path = $"{Directory.GetCurrentDirectory()}\\testdata\\{fileName}";
                    _writeFileStream = new FileStream(path, FileMode.Create);
                    _startWriteTime = DateTime.Now;
                }
                catch (Exception ex)
                {
                    if (Preferences.MainInterfaceForm != null)
                    {
                        ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Could not open scoreboard data file to write. Received error '{ex.Message}.'");
                    }
                }
            }

            _scoreboardRegister = scoreboardRegister;

            if (!(_serialPort is null))
            {
                try
                {
                    _serialPort.Open();
                    if (Preferences.MainInterfaceForm != null)
                    {
                        ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Connected via {_serialPort.PortName} at {_serialPort.BaudRate} baud. {_totalBytesRead} bytes read.");
                    }
                }
                catch (Exception ex)
                {
                    if (Preferences.MainInterfaceForm != null)
                    {
                        ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Unable to connect to {_serialPort.PortName}. Received error '{ex.Message}.'");
                    }
                }
            }
        }

        public void Stop()
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
            }
        }

        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_serialPort.IsOpen) return;

            int bytes = _serialPort.BytesToRead;
            byte[] buffer = new byte[bytes];
            _serialPort.Read(buffer, 0, bytes);

            _scoreboardRegister.ProcessBytes(buffer);

            _totalBytesRead += bytes;
            if (Preferences.MainInterfaceForm != null)
            {

                ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Connected via {_serialPort.PortName} at {_serialPort.BaudRate} baud. {_totalBytesRead} bytes read.");
            }

            if (_writeFileStream != null)
            {
                var milliseconds = (DateTime.Now - _startWriteTime).TotalMilliseconds;
                if(milliseconds > Constants.MAX_WRITE_MILLISECONDS) 
                {
                    _writeFileStream.Close();
                    _writeFileStream.Dispose();
                    _writeFileStream = null;
                    _startWriteTime = DateTime.MinValue;
                }
                else
                {
                    _writeFileStream.Write(buffer, 0, bytes);
                    var progress = Convert.ToInt32(100 * milliseconds / Constants.MAX_WRITE_MILLISECONDS);
                    if (Preferences.MainInterfaceForm != null)
                    {

                        ((MainForm)Preferences.MainInterfaceForm).WriteStatusProgress.Report(progress);
                    }
                }
            }
        }

        public void Dispose()
        {
            IsRunning = false;
            if(_serialPort != null)
            {
                _serialPort.DataReceived -= new SerialDataReceivedEventHandler(SerialDataReceived);
                _serialPort.Dispose();
                _serialPort = null;
            }
        }
    }
}

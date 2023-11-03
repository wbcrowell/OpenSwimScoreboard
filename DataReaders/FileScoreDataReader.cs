using OpenSwimScoreboard.Scoreboard.TimingData;
using System;
using System.IO;
using System.Threading;
using OpenSwimScoreboard.Forms;
using OpenSwimScoreboard.Parameters;
using System.IO.Ports;
using System.Windows;
using System.Diagnostics;

namespace OpenSwimScoreboard.DataReaders
{
    /// <summary>
    /// Reads from test data file, and sends to ScoreboardRegister at the same approximate rate as a 9600 baud connection. 
    /// </summary>
    public class FileScoreDataReader: IScoreDataReader
    {
        private static bool _goToken = true;
        private SerialPort _serialOutputPort;

        public string FileName { get; set; } = null;

        public bool IsRunning { get; set; }

        public event EventHandler DataUpdate;

        public void Start(ScoreboardRegister scoreboardRegister)
        {
            int i = 0;
            DateTime dateTime= DateTime.Now;

            var dataFile = FileName ?? "meet.bin";

            var path = $"{Directory.GetCurrentDirectory()}\\testdata\\{dataFile}";

            //Race 1.  Runs the same race data multiple times pause in the middle to test reconnection.
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                stream.Seek(0, SeekOrigin.Begin);

                while (_goToken & stream.Position <= stream.Length)
                {
                    var dataInt = stream.ReadByte();
                    if(dataInt < 0)
                    {
                        break;
                    }

                    var data = Convert.ToByte(dataInt);

                    //Tests the serial out functionality *******************
                    if (!string.IsNullOrWhiteSpace(Preferences.OutputSerialPort) && Preferences.OutputSerialPort != _serialOutputPort?.PortName)
                    {
                        if (_serialOutputPort != null)
                        {
                            _serialOutputPort.Close();
                            _serialOutputPort.Dispose();
                        }
                        _serialOutputPort = new SerialPort
                        {
                            BaudRate = 9600,
                            Parity = Parity.Even,
                            PortName = Preferences.OutputSerialPort,
                            WriteTimeout = 500,
                        };
                        try
                        {
                            _serialOutputPort.Open();
                        }
                        catch (Exception ex)
                        {
                            if (Preferences.MainInterfaceForm != null)
                            {
                                ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Unable to connect with output port. Please correct configuration or switch to parallel mode.");
                            }
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(Preferences.OutputSerialPort))
                    {
                        _serialOutputPort.Close();
                        _serialOutputPort.Dispose();
                        _serialOutputPort = null;
                    }

                    int bytes = 1;
                    var buffer = new byte[bytes];
                    buffer[bytes - 1] = data;

                    if(Preferences.DataMode == Preferences.DataModeType.Serial)
                    {
                        if(_serialOutputPort != null && _serialOutputPort.IsOpen)
                        {
                            try
                            {
                                _serialOutputPort.Write(buffer, 0, bytes);
                            }
                            catch (Exception e)
                            {
                                if (Preferences.MainInterfaceForm != null)
                                {
                                    ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Output port error. Please correct configuration or switch to parallel mode.");
                                }
                            }
                            scoreboardRegister.ProcessByte(data);
                        }
                        else
                        {
                            if (Preferences.MainInterfaceForm != null)
                            {
                                ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Output port error. Please correct configuration or switch to parallel mode.");
                            }
                        }
                    }
                    else
                    {
                        scoreboardRegister.ProcessByte(data);
                    }

                    //End serial out test.           *******************


                    if (i % 15 == 0)
                    {
                        Thread.Sleep(1);
                    }
                    Debug.Print(i.ToString());
                    if(i % 100 == 0)
                    {
                        var s = Convert.ToDouble((DateTime.Now - dateTime).TotalSeconds);
                        dateTime = DateTime.Now;
                        if (Preferences.MainInterfaceForm != null)
                        {
                            ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Race 1. {(100.0 / s).ToString("0.00")} baud. Stream at {stream.Position} of {stream.Length}.");
                        }
                    }
                    i++;
                }
            }
            Thread.Sleep(6000); //Pause for 6 seconds

            //Race 2 - 11
            for (int j = 0; j < 10; j++)
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    stream.Seek(0, SeekOrigin.Begin);

                    while (_goToken & stream.Position <= stream.Length)
                    {
                        var dataInt = stream.ReadByte();
                        if (dataInt < 0)
                        {
                            break;
                        }

                        var data = Convert.ToByte(dataInt);


                        //Tests the serial out functionality *******************
                        if (!string.IsNullOrWhiteSpace(Preferences.OutputSerialPort) && Preferences.OutputSerialPort != _serialOutputPort?.PortName)
                        {
                            if (_serialOutputPort != null)
                            {
                                _serialOutputPort.Close();
                                _serialOutputPort.Dispose();
                            }
                            _serialOutputPort = new SerialPort
                            {
                                BaudRate = 9600,
                                Parity = Parity.Even,
                                PortName = Preferences.OutputSerialPort,
                                WriteTimeout = 500,
                            };
                            try
                            {
                                _serialOutputPort.Open();
                            }
                            catch (Exception ex)
                            {
                                if (Preferences.MainInterfaceForm != null)
                                {
                                    ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Unable to connect with output port. Please correct configuration or switch to parallel mode.");
                                }
                            }
                        }
                        else if (string.IsNullOrWhiteSpace(Preferences.OutputSerialPort))
                        {
                            _serialOutputPort.Close();
                            _serialOutputPort.Dispose();
                            _serialOutputPort = null;
                        }

                        int bytes = 1;
                        var buffer = new byte[bytes];
                        buffer[bytes - 1] = data;

                        if (Preferences.DataMode == Preferences.DataModeType.Serial)
                        {
                            if (_serialOutputPort != null && _serialOutputPort.IsOpen)
                            {
                                try
                                {
                                    _serialOutputPort.Write(buffer, 0, bytes);
                                }
                                catch (Exception e)
                                {
                                    if (Preferences.MainInterfaceForm != null)
                                    {
                                        ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Output port error. Please correct configuration or switch to parallel mode.");
                                    }
                                }
                                scoreboardRegister.ProcessByte(data);
                            }
                            else
                            {
                                ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Output port error. Please correct configuration or switch to parallel mode.");
                            }
                        }
                        else
                        {
                            scoreboardRegister.ProcessByte(data);
                        }
                        //End serial out test.           *******************


                        if (i % 15 == 0)
                        {
                            Thread.Sleep(1);
                        }
                        if (i % 100 == 0)
                        {
                            var s = Convert.ToDouble((DateTime.Now - dateTime).TotalSeconds);
                            dateTime = DateTime.Now;
                            if (Preferences.MainInterfaceForm != null)
                            {
                                ((MainForm)Preferences.MainInterfaceForm).ScoreDataProgress.Report($"Race 2. {(100.0 / s).ToString("0.00")} baud. Stream at {stream.Position} of {stream.Length}.");
                            }
                        }
                        i++;
                    }
                }
            }
        }

        public void Stop()
        {
            _goToken = false;
        }

        public void Dispose() { }
    }
}

using OpenSwimScoreboard.Scoreboard.TimingData;
using System;
using System.IO;
using System.Threading;
using OpenSwimScoreboard.Forms;
using OpenSwimScoreboard.Parameters;

namespace OpenSwimScoreboard.DataReaders
{
    /// <summary>
    /// Reads from test data file, and sends to ScoreboardRegister at the same approximate rate as a 9600 baud connection. 
    /// </summary>
    public class FileScoreDataReader: IScoreDataReader
    {
        private static bool _goToken = true;

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
                    scoreboardRegister.ProcessByte(data);

                    if (i % 15 == 0)
                    {
                        Thread.Sleep(1);
                    }
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
                        scoreboardRegister.ProcessByte(data);

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

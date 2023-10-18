using OpenSwimScoreboard.Scoreboard.TimingData;
using System;
using System.Threading.Tasks;

namespace OpenSwimScoreboard.DataReaders
{
    /// <summary>
    /// Interface for the reader of live scoreboard data from the timing console, or a test stream.
    /// </summary>
    public interface IScoreDataReader
    {
        //Only used for test stream; location of test data file.
        public string FileName { get; set; }

        public bool IsRunning { get; set; }

        public void Start(ScoreboardRegister scoreboardRegister)
        {
        }

        public void Stop()
        {
        }

        public void Dispose()
        {
        }
    }
}

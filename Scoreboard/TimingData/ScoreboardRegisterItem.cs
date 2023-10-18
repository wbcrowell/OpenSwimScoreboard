using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using OpenSwimScoreboard.Parameters;

namespace OpenSwimScoreboard.Scoreboard.TimingData
{
    public class ScoreboardRegisterItem
    {
        private DateTime _lastUpdate = DateTime.Now;

        public byte Channel { get; set; }

        public int SegmentNum { get; set; }

        public Dictionary<int, int> Data { get; } = new Dictionary<int, int>();

        public bool IsDataReadout { get; set; }

        public double MillisecondsSinceLastUpdate
        {
            get
            {
                return (DateTime.Now - _lastUpdate).TotalMilliseconds;
            }
            private set { }
        }

        private ScoreboardRegisterItem() { }

        public void Reset()
        {
            for (int i = 0; i < Data.Count; i++)
            {
                Data[i] = Constants.SPACE_ASCII;
            }
            IsDataReadout = false;
            _lastUpdate = DateTime.Now;
        }

        public void ResetIfExpired(double resetMilliseconds)
        {
            if (MillisecondsSinceLastUpdate > resetMilliseconds)
            {
                Reset();
            }
        }

        public static ScoreboardRegisterItem Create(byte byteIn)
        {
            ScoreboardRegisterItem returnValue = new ScoreboardRegisterItem();

            if (byteIn > 0x7f)
            {
                returnValue.IsDataReadout = (byteIn & 1) == 0;    // bit #0 = 0 -> DATA is readout, 1=DATA is Format
                returnValue.Channel = Convert.ToByte(byteIn >> 1 & 0x1f ^ 0x1f);
                //TODO: square up max channel and lanes displayed!

                if (byteIn > 0xbe)
                {
                    for (int i = 0; i < returnValue.Data.Count; i++)
                    {
                        returnValue.Data[i] = Constants.SPACE_ASCII;
                    }
                }
                else if (byteIn > 0xa9 && byteIn < 0xbe)
                {
                    //Do something??
                }
            }
            else
            {
                var segmentNum = (byteIn & 0xf0) >> 4;
                if (segmentNum >= 8)
                {
                    Debug.WriteLine("Segment is unexpectedly large.");
                }

                var segmentData = (byte)(byteIn & 0x0f);
                if (returnValue.Channel > 0 && segmentData == 0)
                {
                    segmentData = Constants.SPACE_ASCII;
                }
                else
                {
                    segmentData = (byte)(segmentData ^ 0x0f + 48);
                }
                returnValue.Data[segmentNum] = segmentData;

            }
            returnValue._lastUpdate = DateTime.Now;

            return returnValue;
        }
    }
}

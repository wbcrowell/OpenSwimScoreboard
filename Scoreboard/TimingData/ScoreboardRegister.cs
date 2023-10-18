using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Timers;
using OpenSwimScoreboard.Parameters;

namespace OpenSwimScoreboard.Scoreboard.TimingData
{
    //TODO: make singleton
    public class ScoreboardRegister
    {
        private const double PURGE_INTERVAL = 2000;
        private const double DISCONNECT_WARNING_THRESHOLD = 5000;

        private static byte? _currentChannelId = null;
        private static bool _isPaused = false;
        private static DateTime _lastUpdate = DateTime.Now;

        public Dictionary<byte, Channel> Channels { get; private set; } = new Dictionary<byte, Channel>();

        public bool IsConnected
        {
            get
            {
                return (DateTime.Now - _lastUpdate).TotalMilliseconds < DISCONNECT_WARNING_THRESHOLD;
            }
            private set { }
        }

        public ScoreboardRegister()
        {
            //TimeoutChecker = new Timer
            //{
            //    Interval = PURGE_INTERVAL,
            //};
            //TimeoutChecker.Elapsed += PurgeOldData;
            //TimeoutChecker.Enabled = true;
        }

        public void ProcessBytes(byte[] bytesIn)
        {
            for (int i = 0; i < bytesIn.Length; i++)
            {
                ProcessByte(bytesIn[i]);
            }
            if (bytesIn.Length > 0)
            {
                _lastUpdate = DateTime.Now;
            }
        }

        public void ProcessByte(byte byteIn)
        {
            if (!_isPaused)
            {
                byte? ccid = _currentChannelId; //create a temporary copy of _currentChannelId, which can be nulled in the middle of this operation :(

                if (byteIn > 0x7f)
                {
                    bool isDataReadout = (byteIn & 1) == 0;    // bit #0 = 0 -> DATA is readout, 1=DATA is Format
                    if (isDataReadout) //Record to channel only if is readout (format is ignored)
                    {
                        _currentChannelId = Convert.ToByte(byteIn >> 1 & 0x1f ^ 0x1f);
                        ccid = _currentChannelId;
                    }
                    else
                    {
                        _currentChannelId = ccid = null;
                    }

                    if (ccid.HasValue)
                    {
                        if (!Channels.ContainsKey(ccid.Value))
                        {
                            Channels.Add(ccid.Value, new Channel());
                        }
                        //System.Diagnostics.Debug.WriteLine($"Channel set to {ccid.Value}. ByteIn is {byteIn}");
                        //TODO: square up max channel and lanes displayed!

                        if (byteIn > 190)
                        {
                            var thisChannel = Channels[ccid.Value];
                            if (thisChannel != null)
                            {
                                var moduleKeys = thisChannel.Modules.Keys.ToList();
                                foreach (var moduleKey in moduleKeys)
                                {
                                    thisChannel.Update(moduleKey, Constants.SPACE_ASCII);
                                }
                            }
                        }
                        else if (byteIn > 169 && byteIn < 190)
                        {
                            //Do something??
                        }
                    }
                }
                else
                {
                    if (ccid.HasValue)
                    {
                        var segmentNum = (byteIn & 0xf0) >> 4;
                        if (segmentNum >= 8)
                        {
                            System.Diagnostics.Debug.WriteLine($"Segment number greater than 8! {segmentNum}");
                        }

                        byte segmentData = (byte)(byteIn & 0x0f);
                        if (ccid > 0 && segmentData == 0)
                        {
                            segmentData = Constants.SPACE_ASCII;
                        }
                        else
                        {
                            segmentData = (byte)(segmentData ^ 0x0f + 48);
                        }
                        //System.Diagnostics.Debug.WriteLine($"    Segment {segmentNum} data: {segmentData}");

                        if (!Channels.ContainsKey(ccid.Value))
                        {
                            Channels.Add(ccid.Value, new Channel());
                        }
                        Channels[ccid.Value].Update(segmentNum, segmentData);
                    }
                }
                _lastUpdate = DateTime.Now;
            }
        }

        public static void Pause()
        {
            _isPaused = true;
            _currentChannelId = null;
        }

        public static void UnPause()
        {
            _isPaused = false;
            _currentChannelId = null;
        }

        public void PurgeOldData(Object source, System.Timers.ElapsedEventArgs e)
        {
            if ((DateTime.Now - _lastUpdate) > TimeSpan.FromMilliseconds(PURGE_INTERVAL))
            {
                var channelIds = Channels.Keys.ToList();
                foreach (var channelId in channelIds)
                {
                    Channels[channelId].PurgeIfOlderThanMilliseconds(PURGE_INTERVAL);
                }
            }
        }

        public void Debug()
        {
            System.Diagnostics.Debug.WriteLine("==========");
            var channelIds = Channels.Keys.ToList().OrderBy(c => c);
            foreach (var channelId in channelIds)
            {
                byte[] channelIdArray = { channelId };
                string line = $"{BitConverter.ToString(channelIdArray)}: {string.Join(',', Channels[channelId].Modules.ToList().Select(m => $"[{m.Key}>{m.Value}]").ToArray())}";
                System.Diagnostics.Debug.WriteLine(line);
                System.Diagnostics.Debug.WriteLine("----------");
            }
            System.Diagnostics.Debug.WriteLine("==========");
        }
    }
}

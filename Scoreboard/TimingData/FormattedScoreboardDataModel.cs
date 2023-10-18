using System;
using System.Collections.Generic;
using System.Linq;
using OpenSwimScoreboard.Parameters;

namespace OpenSwimScoreboard.Scoreboard.TimingData
{
    public class FormattedScoreboardDataModel
    {
        public string MeetTitle { get; set; }
        public string CurrentEvent { get; set; }
        public string CurrentHeat { get; set; }
        public string CurrentClock { get; set; }

        public string[] LaneNumber { get; set; }
        public string[] LanePlace { get; set; }
        public string[] LaneTime { get; set; }

        public FormattedScoreboardDataModel() { }

        //This constructor is used when live data IS available.
        public FormattedScoreboardDataModel(Dictionary<byte, Channel> scoreboardRegister)
        {
            Dictionary<byte, int[]> rawInput = new Dictionary<byte, int[]>();

            if (scoreboardRegister != null)
            {
                var channelIds = scoreboardRegister.Keys.ToList();
                foreach (var channelId in channelIds)
                {
                    var moduleArray = new int[16];
                    var moduleIds = scoreboardRegister[channelId].Modules.Keys.ToList();
                    foreach (var moduleId in moduleIds)
                    {
                        moduleArray[moduleId] = scoreboardRegister[channelId].Modules[moduleId];
                    }
                    rawInput.Add(channelId, moduleArray);
                }
            }

            CurrentEvent = $"{getChar(rawInput, 0x0c, 0)}{getChar(rawInput, 0x0c, 1)}{getChar(rawInput, 0x0c, 2)}";
            CurrentHeat = $"{getChar(rawInput, 0x0c, 5)}{getChar(rawInput, 0x0c, 6)}{getChar(rawInput, 0x0c, 7)}";
            CurrentClock = getLaneTime(rawInput, 0x00);

            LaneNumber = new string[16];
            LanePlace = new string[16];
            LaneTime = new string[16];

            for (byte i = 0x01; i < 0x0b; i++) // Lanes 1-10 are in adjacent channels
            {
                var index = Convert.ToInt32(i);
                LaneNumber[index] = getChar(rawInput, i, 0);
                LanePlace[index] = getChar(rawInput, i, 1);
                LaneTime[index] = getLaneTime(rawInput, i);
            }
            for (byte j = 0x17; j < 0x19; j++) // Lanes 11 and 12 are appended to the end of the channel list
            {
                var index = Convert.ToInt32(j - 12);
                LaneNumber[index] = getChar(rawInput, j, 0);
                LanePlace[index] = getChar(rawInput, j, 1);
                LaneTime[index] = getLaneTime(rawInput, j);
            }
        }

        //This constructor is used when live data IS NOT available, so that event, heat, and lane placement data can be displayed.
        public FormattedScoreboardDataModel(string currentEvent, string currentHeat, int numLanes)
        {
            CurrentEvent = currentEvent;
            CurrentHeat = currentHeat;
            CurrentClock = "";

            LaneNumber = new string[16];
            LanePlace = new string[16];
            LaneTime = new string[16];
            for(int  i = 1; i <= numLanes; i++)
            {
                LaneNumber[i] = i.ToString();
                LanePlace[i] = "";
                LaneTime[i] = "";
            }
        }

        private string getChar(Dictionary<byte, int[]> rawInput, byte channel, int offset)
        {
            if (!rawInput.ContainsKey(channel))
            {
                return Constants.BLANK_CHAR;
            }
            char ch = (char)rawInput[channel][offset];
            if (ch == Convert.ToChar(Constants.SPACE_ASCII) || ch == Convert.ToChar(Constants.QUESTION_MARK_ASCII) || ch == Convert.ToChar(Constants.NULL_ASCII))
            {
                return Constants.BLANK_CHAR;
            }
            return Convert.ToString(ch);
        }

        private string getLaneTime(Dictionary<byte, int[]> rawInput, byte channel)
        {
            List<string> digitList = new List<int> { 2, 3, 4, 5, 6, 7 }.Select(o => getChar(rawInput, channel, o)).ToList();
            if (digitList[4] != Constants.BLANK_CHAR || digitList[5] != Constants.BLANK_CHAR)
            {
                digitList.Insert(4, ".");
            }
            if (digitList[1] != Constants.BLANK_CHAR)
            {
                digitList.Insert(2, ":");
            }
            return string.Join("", digitList);
        }


        public static FormattedScoreboardDataModel TestSample()
        {
            var returnValue = new FormattedScoreboardDataModel();

            returnValue.CurrentEvent = "123";
            returnValue.CurrentHeat = "321";
            returnValue.CurrentClock = DateTime.Now.ToShortDateString();

            returnValue.LaneNumber = new string[10];
            returnValue.LanePlace = new string[10];
            returnValue.LaneTime = new string[10];

            for (byte i = 0x00; i < 0x0a; i++)
            {
                var index = Convert.ToInt32(i);
                returnValue.LaneNumber[index] = index.ToString();
                returnValue.LanePlace[index] = (7 - index).ToString();
                returnValue.LaneTime[index] = $"00:{60 - index}.00";
            }

            return returnValue;
        }
    }
}

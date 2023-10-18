using Newtonsoft.Json;
using OpenSwimScoreboard.Scoreboard.NamesData;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.IO.Packaging;

namespace OpenSwimScoreboard.Utilities
{
    public class StartListUtilities
    {
        public class NamesData
        {
            public string Status { get; set; }
            public Dictionary<int, Event> Events { get; set; }
        }

        //Reads the JSON-formatted event, heat, and swimmer data stored in the session.js file.
        //To bypass browser security for local files, this is wrapped with, and stored as, javascript, rather than JSON.
        public static NamesData ReadNamesData(string currentDirectory)
        {
            var readDirectory = $"{currentDirectory}\\scoreboard_html\\scoreboardfiles\\";
            var readFilename = "session.js";

            if (Directory.Exists(readDirectory) && File.Exists(readDirectory + readFilename))
            {
                var fileString = File.ReadAllText(readDirectory + readFilename);
                if (!string.IsNullOrWhiteSpace(fileString))
                {
                    //Convert the js file back to JSON
                    if (fileString.IndexOf("{") > 0)
                    {
                        fileString = fileString.Substring(fileString.IndexOf("{"));
                    }
                    var fileEvents = JsonConvert.DeserializeObject<Dictionary<int, Event>>(fileString);
                    var numNames = fileEvents.SelectMany(e => e.Value.Heats).SelectMany(h => h.Value.Entries).Count();
                    var writeDate = File.GetLastWriteTime(readDirectory + readFilename);
                    var returnMessage = $"{numNames} names uploaded to scoreboard on {writeDate.ToShortDateString()} at {writeDate.ToShortTimeString()}.";

                    return new NamesData
                    {
                        Status = returnMessage, 
                        Events = fileEvents,
                    };
                }
            }

            return new NamesData
            {
                Status = "There is no current list of athlete names.",
                Events = null,
            };
        }

        //Reads start list (.scb) files created by MeetManager software, which contain event, heat, and swimmer information.
        //JSON is wrapped with javascript to bypass browser security for local files.
        public static string WriteNamesData(string readPath, string currentDirectory)
        {
            var writeDirectory = $"{currentDirectory}\\scoreboard_html\\scoreboardfiles\\";
            var writeFilename = "session.js";
            var returnMessage = "";

            if (string.IsNullOrWhiteSpace(readPath))
            {
                if (File.Exists(writeDirectory + writeFilename))
                {
                    File.Delete(writeDirectory + writeFilename);
                    returnMessage = "Deleted existing names list. Scoreboard will not show athlete names.\r\n(Scoreboards opened before uploading names may require refresh)";
                }
                else
                {
                    returnMessage = "No names list. Scoreboard will not show athlete names.";
                }
            }
            else
            {
                try
                {
                    Session thisSession = new OpenSwimScoreboard.Scoreboard.NamesData.Session(readPath);
                    Task.Run(() => thisSession.WriteJsonToFile(writeDirectory, writeFilename));
                    var namesCount = thisSession.Events.SelectMany(e => e.Value.Heats).SelectMany(h => h.Value.Entries).Count();
                    returnMessage = $"Sucessfully wrote names of {namesCount} swimmers for {thisSession.Events.Count} events.\r\n(Scoreboards opened before uploading names may require refresh)";
                }
                catch (Exception e)
                {
                    returnMessage = $"Error writing athlete names: {e.Message}";
                }
            }
            return returnMessage;
        }
    }
}

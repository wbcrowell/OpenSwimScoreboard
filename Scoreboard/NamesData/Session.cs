using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace OpenSwimScoreboard.Scoreboard.NamesData
{
    /// <summary>
    /// Container for session data extracted from start list (.scb) files.
    /// Also provides utility functions for finding, reading,and extracting data from those files, then serializing it and saving it for use by the html scoreboard page(s).
    /// </summary>
    public class Session
    {
        public string Name { get; set; }

        /// <summary>
        /// The child Events associated with this Session.
        /// </summary>
        public SortedDictionary<int, Event> Events { get; set; }

        /// <summary>
        /// Serializes this Session as JSON.
        /// </summary>
        public string EventsJson
        {
            get
            {
                if (Events == null)
                {
                    return "";
                }
                return JsonConvert.SerializeObject(Events);
            }
            private set { }
        }

        public Session() { }

        /// <summary>
        /// Constructs Session object and children from start list (.scb) files.
        /// Start list (.scb) files are created by MeetManager: File > Export > Start Lists for Scoreboard > Start Lists for CTS
        /// </summary>
        /// <param name="directoryNameAndPath">The path of the directory containing the .scb files.</param>
        /// <exception cref="DirectoryNotFoundException">Exception thrown if specified path is not valid or directory is not available.</exception>
        public Session(string directoryNameAndPath)
        {
            if (Directory.Exists(directoryNameAndPath))
            {
                Events = new SortedDictionary<int, Event>();
                string[] fileNames = Directory.GetFiles(directoryNameAndPath);
                BuildEvents(fileNames);
            }
            else
            {
                throw new DirectoryNotFoundException("Cannot find specified start list (.scb) directory.");
            }
        }

        /// <summary>
        /// Wraps EventsJson in javascript and writes it to a file.
        /// Javascript wrapper is necessary because most browsers will not load a local json file.
        /// </summary>
        /// <param name="directory">The directory to which the serialized Session should be written.</param>
        /// <param name="filename">"The name of the file containing serialized Session data.</param>
        public async void WriteJsonToFile(string directory, string filename)
        {
            if(!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            await File.WriteAllTextAsync(directory + filename, $"scoreboardNames={EventsJson}");
        }

        /// <summary>
        /// Reads all start list (.scb) is specified list of files, and creates a tree below this Session, consisting of Events, Heats, and Swimmers.
        /// </summary>
        /// <param name="datafiles">A list of filenames to be read.</param>
        /// <exception cref="Exception">Thrown if there is a problem parsing an event number from the profferred file name.</exception>
        private void BuildEvents(string[] datafiles)
        {
            foreach (string file in datafiles)
            {
                if (File.Exists(file))
                {

                    string[] lines = File.ReadAllLines(file);

                    Event thisEvent = new Event
                    {
                        Name = lines[0],                                    //The first line contains the event number and name.
                        Number = file.Substring(file.LastIndexOf("E") + 1)
                        .ToLower()
                        .Replace(".scb", "")
                        .Trim(),
                        Heats = new SortedDictionary<int, Heat>(),
                    };

                    for (int i = 1; i < lines.Length; i++)
                    {
                        int heat = (i - 1) / 10 + 1;                         //Each subsequent group of 10 lines contains info on a heat of swimmers.
                        int lane = (i - 1) % 10 + 1;
                        int separatorPosition = lines[i].IndexOf("--");      //Swimmer and team name are separated by "--".

                        Swimmer thisSwimmer = new Swimmer
                        {
                            Name = "",
                            Team = "",
                            Lane = lane,
                        };
                        if (separatorPosition > 0)
                        {
                            var name = lines[i].Substring(0, separatorPosition).Trim();
                            var commaPosition = name.IndexOf(",");
                            var lastName = commaPosition > -1 ? name.Substring(0, commaPosition).Trim() : "";
                            thisSwimmer = new Swimmer
                            {
                                Name = name,
                                LastName = lastName,
                                Team = lines[i].Substring(separatorPosition + 2).Trim(),
                                Lane = lane,
                            };
                        } 
                        else if (!string.IsNullOrWhiteSpace(lines[i]))
                        {
                            var name = lines[i].Trim();
                            var commaPosition = name.IndexOf(",");
                            var lastName = commaPosition > -1 ? name.Substring(0, commaPosition).Trim() : "";
                            thisSwimmer = new Swimmer
                            {
                                Name = name,
                                LastName = lastName,
                                Team = "",
                                Lane = lane,
                            };
                        }

                        if (!thisEvent.Heats.ContainsKey(heat))
                        {
                            Heat thisHeat = new Heat
                            {
                                HeatNumber = heat,
                                Entries = new SortedDictionary<int, Swimmer>(),
                            };
                            thisHeat.Entries.Add(lane, thisSwimmer);
                            thisEvent.Heats[heat] = thisHeat;
                        }
                        else
                        {
                            thisEvent.Heats[heat].Entries.Add(lane, thisSwimmer);
                        }
                    }

                    //The event number is contained in the name of the .scb file.
                    int eventInt;
                    if (int.TryParse(thisEvent.Number, out eventInt))
                    {
                        Events.Add(eventInt, thisEvent);
                    }
                    else
                    {
                        throw new Exception($"Unable to add file {file.Substring(file.LastIndexOf("\\"))}. Unable to parse event number from file name.");
                    }
                }
            }
        }
    }
}

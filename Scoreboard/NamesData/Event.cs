using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSwimScoreboard.Scoreboard.NamesData
{
    /// <summary>
    /// Container for event data extracted from start list (.scb) file.
    /// </summary>
    public class Event
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public SortedDictionary<int, Heat> Heats { get; set; }
    }
}

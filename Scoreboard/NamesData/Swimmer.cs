using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSwimScoreboard.Scoreboard.NamesData
{
    /// <summary>
    /// Container for swimmer data extracted from start list (.scb) file.
    /// </summary>
    public class Swimmer
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Team { get; set; }

        public int Lane { get; set; }
    }
}

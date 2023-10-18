using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSwimScoreboard.Scoreboard.NamesData
{
    /// <summary>
    /// Contaner for heat data extracted from start list (.scb) file.
    /// </summary>
    public class Heat
    {
        public int HeatNumber { get; set; }

        public SortedDictionary<int, Swimmer> Entries { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using OpenSwimScoreboard.Parameters;

namespace OpenSwimScoreboard.Scoreboard.TimingData
{
    public class Channel
    {
        private DateTime _lastUpdate;

        public Dictionary<int, int> Modules { get; set; } = new Dictionary<int, int>();

        public double MillisecondsSinceUpdate
        {
            get
            {
                return (DateTime.Now - _lastUpdate).TotalMilliseconds;
            }
        }

        public void Update(int moduleId, int data)
        {
            if (Modules.ContainsKey(moduleId))
            {
                Modules[moduleId] = data;
            }
            else
            {
                Modules.Add(moduleId, data);
            }
            _lastUpdate = DateTime.Now;
        }

        public void PurgeIfOlderThanMilliseconds(double milliseconds)
        {
            if (MillisecondsSinceUpdate > milliseconds)
            {
                var moduleIds = Modules.Keys.ToList();
                foreach (var moduleId in moduleIds)
                {
                    Modules[moduleId] = Constants.SPACE_ASCII;
                }
            }
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using OpenSwimScoreboard.BackgroundServices;
using OpenSwimScoreboard.Extensions;
using OpenSwimScoreboard.Scoreboard.TimingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OpenSwimScoreboard.Hubs
{
    /// <summary>
    /// SignalR hub that connects the live scoreboard data service (ScoreboardBackgroundService) to any connected html page that is displaying the scoreboard html.
    /// Updated data is sent via SignalR several times per second.
    /// </summary>
    public class ScoreboardHub : Hub
    {
        private readonly ScoreboardBackgroundService _scoreboardBackgroundService;

        public ScoreboardHub(ScoreboardBackgroundService scoreboardBackgroundService)
        {
            _scoreboardBackgroundService = scoreboardBackgroundService;
        }

        public ChannelReader<FormattedScoreboardDataModel> StreamScoreboardData()
        {
            return _scoreboardBackgroundService.StreamScoreboardData().AsChannelReader(10);
        }
    }
}

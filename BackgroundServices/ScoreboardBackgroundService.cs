﻿using Microsoft.Extensions.Hosting;
using OpenSwimScoreboard.DataReaders;
using OpenSwimScoreboard.Parameters;
using OpenSwimScoreboard.Scoreboard.TimingData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace OpenSwimScoreboard.BackgroundServices
{
    /// <summary>
    /// Background service that reads serial data stream generated by the timing console, and translates it into updates for a ScoreboardRegister, which stores the most recent values for specified channels. 
    /// Runs until cancelation token is received.
    /// </summary>
    public class ScoreboardBackgroundService : BackgroundService
    {
        private readonly Subject<FormattedScoreboardDataModel> _subject = new Subject<FormattedScoreboardDataModel>();
        private readonly Random _random = new Random();
        private static bool _checkLock = false;
        private static IScoreDataReader dr;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ScoreboardRegister scoreboardRegister = new ScoreboardRegister();

            //Uncomment this DataReader to connect with a System6 console

            dr = new System6ScoreDataReader();

            //-----------------------------------------------------------


            //Uncomment this DataReader to connect to the FileDataReader,
            // which is used for testing

            //dr = new FileScoreDataReader
            //{
            //    FileName = "meet.bin" //FileName points to the file of
            //                          // pre-recorded test stream data
            //};

            //-----------------------------------------------------------

            Task.Run(() => dr.Start(scoreboardRegister)); //start reading incoming data and post to ScoreboardRegister

            while (!stoppingToken.IsCancellationRequested)
            {
                if(Preferences.UseOfflineDataOnly)
                {
                    if(!string.IsNullOrWhiteSpace(Preferences.CurrentEvent) && !string.IsNullOrWhiteSpace(Preferences.CurrentHeat))
                    {
                        _subject.OnNext(new FormattedScoreboardDataModel(Preferences.CurrentEvent, Preferences.CurrentHeat, Preferences.NumLanes));
                    }
                }
                else if (scoreboardRegister.IsConnected)
                {
                    ScoreboardRegister.Pause(); //stop updating ScoreboardRegister to read it
                    //scoreboardRegister.Debug();
                    _subject.OnNext(new FormattedScoreboardDataModel(scoreboardRegister.Channels));
                }
                ScoreboardRegister.UnPause();

                if(dr.GetType() == typeof(System6ScoreDataReader))
                {
                    CheckConnectionPref(scoreboardRegister);
                }

                await Task.Delay(190); //Reads every 0.19 s
            }
            dr.Stop();
            dr.Dispose();
        }

        public IObservable<FormattedScoreboardDataModel> StreamScoreboardData()
        {
            return _subject;
        }

        private void CheckConnectionPref(ScoreboardRegister sr)
        {
            if (!_checkLock)
            {
                _checkLock = true;
                if (SerialPorts.RequestToRun != dr.IsRunning)
                {
                    if (SerialPorts.RequestToRun)
                    {
                        //Can only open a serial connection once, so we have to re-create.
                        dr = new System6ScoreDataReader(new PortDefinition
                        {
                            PortName = Parameters.Preferences.InputSerialPort,
                            BaudRate = Parameters.Preferences.BaudRate,
                        });
                        Task.Run(() => dr.Start(sr)); //start reading incoming data and post to ScoreboardRegister
                    }
                    else
                    {
                        dr.IsRunning = false;
                        dr.Stop();
                        dr.Dispose();
                    }
                }
                _checkLock = false;
            }
        }

    }
}

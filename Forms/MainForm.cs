using Newtonsoft.Json;
using OpenSwimScoreboard.DataReaders;
using OpenSwimScoreboard.Parameters;
using OpenSwimScoreboard.Scoreboard.NamesData;
using OpenSwimScoreboard.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenSwimScoreboard.Forms
{
    /// <summary>
    /// Displays a configuration and monitoring form for OpenSwimScoreboard. Configures html scoreboard and monitors score data reading service (ScoreboardBackgroundService).
    /// </summary>
    public partial class MainForm : Form
    {
        private static string _status = "";
        private string _lastStatus = "";
        private Timer _statusTimer;

        private string _currentDirectory;

        public IProgress<int> WriteStatusProgress { get; set; }
        public IProgress<string> ScoreDataProgress { get; set; }

        private Dictionary<int, Event> _events;
        private List<int> _eventNumbers;

        public MainForm()
        {
            Preferences.MainInterfaceForm = this;

            //Initialize progress bar showing progress writing incoming data stream to a test data file (used for testing and debugging).
            WriteStatusProgress = new Progress<int>(value =>
            {
                writeStatusProgressBar.Value = value;
                if (value > 99)
                {
                    testFileCheckBox.Checked = false;
                    writeStatusProgressBar.Visible = false;
                }
            });

            //_statusTimer is used to periodically check for data connection timeout and update status message serviceStatusLabel.
            _statusTimer = new Timer
            {
                Interval = 5000,
            };
            _statusTimer.Tick += (sender, eventArgs) =>
            {
                this.serviceStatusLabel.Text = "Idle. If connection not re-established within 30 seconds: Disconnect, confirm port name and baud rate, and re-connect.";
            };
            //Receives progress messages from DataReader and updates status message serviceStatusLabel.
            ScoreDataProgress = new Progress<string>(value =>
            {
                serviceStatusLabel.Text = value;
                _statusTimer.Stop();
                _statusTimer.Start();
            });
            _statusTimer.Start();

            InitializeComponent();

            //Initialize various dynamic labels and dropdowns.
            _currentDirectory = Directory.GetCurrentDirectory();
            linkLabel.Text = WebRedirectUrl();
            namesDataResultLabel.Text = StartListUtilities.ReadNamesData(_currentDirectory).Status;

            testFileCheckBox.Text = $"Write test file ({Math.Round(Constants.MAX_WRITE_MILLISECONDS / 60000)} min.)";

            portNameComboBox.Items.AddRange(SerialPorts.AvailablePorts);
            outputPortNameComboBox.Items.AddRange(SerialPorts.AvailablePorts);
            baudRateComboBox.Items.AddRange(SerialPorts.AvailableBaudRates);

            //readerComboBox.Items.AddRange(TypeUtilities.GetImplementationsOfInterface<IScoreDataReader>().Select(t => t.Name).ToArray());
            //readerComboBox.Text = nameof(System6ScoreDataReader);

            if (portNameComboBox.Items.Count < 1)
            {
                dataConnectionLabel.Text = "No serial data connection found. Check your connection with the timing console and re-start this program.";
            }
            else
            {
                if (!string.IsNullOrEmpty(Preferences.InputSerialPort) && portNameComboBox.Items.Contains(Preferences.InputSerialPort))
                {
                    portNameComboBox.Text = Preferences.InputSerialPort;
                }
                if (!string.IsNullOrEmpty(Preferences.OutputSerialPort) && portNameComboBox.Items.Contains(Preferences.OutputSerialPort))
                {
                    outputPortNameComboBox.Text = Preferences.OutputSerialPort;
                }
                var baudRateString = Preferences.BaudRate.HasValue ? Preferences.BaudRate.ToString() : "";
                if (baudRateComboBox.Items.Contains(baudRateString))
                {
                    baudRateComboBox.Text = baudRateString;
                }
                //Wire up event manually; dropdown must be populated first or warning is displayed.
                portNameComboBox.SelectedIndexChanged += new System.EventHandler(this.portNameComboBox_SelectedIndexChanged);
                baudRateComboBox.SelectedIndexChanged += new System.EventHandler(this.baudRateComboBox_SelectedIndexChanged);
                outputPortNameComboBox.SelectedIndexChanged += new EventHandler(this.outputPortNameComboBox_SelectedIndexChanged);
            }

            refreshStartList();
        }

        //Erases name of selected .scb file which, after "Submit" button is clicked, causes structured start list json file at
        // /scoreboard/html/scoreboardfiles/session.js to be deleted, which causes html scoreboard to not have swimmer name,
        // swimmer team affiliation, or event name information. 
        private void namesDataCancelButton_Click(object sender, EventArgs e)
        {
            namesDataText.Text = "";
        }

        //Sets selected FOLDER containing .scb files for current session. .scb files are generated by Meet Manager, and contain start lists for every event and heat.
        //"Submit" button must be clicked for these files to be processed.
        private void namesDataChooseButton_Click(object sender, EventArgs e)
        {
            var chooser = namesDataFolderBrowser.ShowDialog();
            if (chooser == DialogResult.OK)
            {
                namesDataText.Text = namesDataFolderBrowser.SelectedPath;
            }
        }

        //Process start list (.scb) files and shows status. Structured start lists are saved as a json file at /scoreboard/html/scoreboardfiles/session.js,
        // and read into the html scoreboard via javascript.
        private void namesDataSubmitButton_Click(object sender, EventArgs e)
        {
            namesDataResultLabel.Text = StartListUtilities.WriteNamesData(namesDataText.Text, _currentDirectory);
            MessageBox.Show("Refresh any open scoreboard browser windows to see changes", "Refresh scoreboard", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            refreshStartList();
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = ((LinkLabel)sender).Text;
            openLink(target);
        }

        //The number of lanes displayed in the html scoreboard is determined by the query string value (1 through 10). This slider changes the displayed link to the html scoreboard.
        private void lanesTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            var oldLanes = numLanesText.Text;
            numLanesText.Text = ((TrackBar)sender).Value.ToString();
            linkLabel.Text = WebRedirectUrl();
            if (oldLanes != numLanesText.Text)
            {
                Preferences.NumLanes = Convert.ToInt32(numLanesText.Text);
                DialogResult result = MessageBox.Show("To change the number of lanes, you will need to open a different web page. Would you like to do that now?", "Refresh scoreboard", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    string target = linkLabel.Text;
                    openLink(target);
                }
            }
        }

        //Opens the clicked link in the default browser.
        private void openLink(string target)
        {
            try
            {
                Process.Start(new ProcessStartInfo(target) { UseShellExecute = true, Arguments = "l=8" });
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        //Returns a link that opens the html scoreboard. This is a link to a redirect to the page actually displaying the scoreboard (which has a query string).
        //Because the file:// spec does not include query strings, this vital part of the link is truncated when WinForms sends it to the default browser.
        private string WebRedirectUrl()
        {
            return $"file:///{_currentDirectory.Replace("\\", "/")}/scoreboard_html/html{numLanesText.Text}.html";
        }

        //Returns a link that opens the html scoreboard. This is the actual link to the displays the scoreboard.
        //It includes a query string, which is truncated when WinForms sends the address to the default browser, so cannot be used for that purpose.
        private string WebUrl()
        {
            return $"file:///{_currentDirectory.Replace("\\", "/")}/scoreboard_html/html.html?{numLanesText.Text}";
        }


        //Copies the link to the html scoreboard.
        private void copyLinkButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(WebUrl());
        }

        //Sets baud rate.
        private void portNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SerialPorts.SetSerialPortPrefs(portNameComboBox.Text, baudRateComboBox.Text))
            {
                MessageBox.Show("Could not update serial port info. Please check to make sure port name and baud rate are not blank.");
            }
            //else if(outputPortNameComboBox.Text == portNameComboBox.Text)
            //{
            //    outputPortNameComboBox.SelectedIndex = -1;
            //    if(tabControlMode.SelectedTab.Name == "tabPageModeSerial")
            //    {
            //        MessageBox.Show("Please select an output port that is not the same as the input port.");
            //    }
            //    updateOutputPorts();
            //}
        }

        //private void updateOutputPorts()
        //{
        //    var outputPortRange = SerialPorts.AvailablePorts;
        //    if(!string.IsNullOrWhiteSpace(portNameComboBox.Text))
        //    {
        //        outputPortRange = outputPortRange.Where(o => o != portNameComboBox.Text).ToArray();
        //    }
        //    outputPortNameComboBox.Items.Clear();
        //    if (outputPortRange.Any())
        //    {
        //        outputPortNameComboBox.Items.AddRange(outputPortRange);
        //    }
        //    else
        //    {
        //        MessageBox.Show("You do not have enough serial (COM) ports to support scoreboard data input and output. Please add another serial port to your system.");
        //    }
        //}

        //Sets port name.
        private void baudRateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SerialPorts.SetSerialPortPrefs(portNameComboBox.Text, baudRateComboBox.Text))
            {
                MessageBox.Show("Could not update serial port info. Please check to make sure port name and baud rate are not blank.");
            }
        }

        //Handles clicks of connect/disconnect button, managing serial connection with scoreboard data stream.
        private void serialPortButton_Click(object sender, EventArgs e)
        {
            //Disconnect...
            if (SerialPorts.RequestToRun)
            {
                SerialPorts.RequestToRun = false;
                portNameComboBox.Enabled = true;
                baudRateComboBox.Enabled = true;
                testFileCheckBox.Enabled = true;
                //readerComboBox.Enabled = true;
                serialPortButton.Text = "Connect";
            }
            //Connect...
            else
            {
                SerialPorts.RequestToRun = true;
                portNameComboBox.Enabled = false;
                baudRateComboBox.Enabled = false;
                testFileCheckBox.Enabled = false;
                //readerComboBox.Enabled = false;
                serialPortButton.Text = "Disconnect";
            }
        }

        //Checking this box causes incoming scoreboard data stream to be copied to a file (in /TestData)
        // which can be used later for testing purposes.
        private void testFileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.WriteScoreboardDataFile = ((CheckBox)sender).Checked;
            writeStatusProgressBar.Value = 0;
            writeStatusProgressBar.Visible = true;
        }

        //Replaces live scoreboard with offline data from .scb files. Shows event name, heat number and names of swimmers in the heat; does not show live times.
        //Must use + and - buttons to advance event and heat!!
        private void checkBoxUseOfflineDataOnly_CheckedChanged(object sender, EventArgs e)
        {
            var ckd = ((CheckBox)sender).Checked;

            if (ckd)
            {
                if (_events == null || !_events.Any() || _eventNumbers == null || !_eventNumbers.Any())
                {
                    ((CheckBox)sender).Checked = false;
                    MessageBox.Show("No .scb files have been saved. Please click the CHOOSE button at the top of the window, and locate these files.");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(textBoxEvent.Text))
                    {
                        buttonPlusEventHeat_Click(null, null);
                    }
                    else
                    {
                        Preferences.CurrentEvent = textBoxEvent.Text;
                        Preferences.CurrentHeat = textBoxHeat.Text;
                    }

                    Preferences.UseOfflineDataOnly = true;
                }
            }
            else
            {
                Preferences.UseOfflineDataOnly = false;
            }

            labelEvent.Enabled = ckd;
            labelHeat.Enabled = ckd;
        }

        private void buttonPlusEventHeat_Click(object sender, EventArgs e)
        {
            if (_events != null && _events.Any() && _eventNumbers != null && _eventNumbers.Any())
            {
                int? currEvent = string.IsNullOrWhiteSpace(textBoxEvent.Text) ? (int?)null : Convert.ToInt32(textBoxEvent.Text);
                int? currHeat = string.IsNullOrWhiteSpace(textBoxHeat.Text) ? (int?)null : Convert.ToInt32(textBoxHeat.Text);
                if (currEvent.HasValue)
                {
                    if (currHeat == null)
                    {
                        currHeat = _events[currEvent.Value].Heats.First().Key;
                    }
                    else if (currHeat.Value != _events[currEvent.Value].Heats.Last().Key)
                    {
                        currHeat = _events[currEvent.Value].Heats.SkipWhile(h => h.Key != currHeat.Value).Skip(1).First().Key;
                    }
                    else if (currEvent.Value != _eventNumbers.Last())
                    {
                        currEvent = _eventNumbers.SkipWhile(e => e != currEvent.Value).Skip(1).First();
                        currHeat = _events[currEvent.Value].Heats.First().Key;
                    }
                    textBoxEvent.Text = Preferences.CurrentEvent = currEvent.Value.ToString();
                    textBoxHeat.Text = Preferences.CurrentHeat = currHeat.Value.ToString();
                }
                else
                {
                    currEvent = _eventNumbers.First();
                    if (currEvent.HasValue)
                    {
                        textBoxEvent.Text = Preferences.CurrentEvent = currEvent.Value.ToString();
                        textBoxHeat.Text = Preferences.CurrentHeat = _events[currEvent.Value].Heats.First().Key.ToString();
                    }
                }
            }
        }

        private void buttonMinusEventHeat_Click(object sender, EventArgs e)
        {
            if (_events != null && _events.Any() && _eventNumbers != null && _eventNumbers.Any())
            {
                int? currEvent = string.IsNullOrWhiteSpace(textBoxEvent.Text) ? (int?)null : Convert.ToInt32(textBoxEvent.Text);
                int? currHeat = string.IsNullOrWhiteSpace(textBoxHeat.Text) ? (int?)null : Convert.ToInt32(textBoxHeat.Text);
                if (currEvent.HasValue)
                {
                    if (currHeat.Value != _events[currEvent.Value].Heats.First().Key)
                    {
                        currHeat = _events[currEvent.Value].Heats.TakeWhile(h => h.Key != currHeat.Value).Last().Key;
                    }
                    else if (currEvent.Value != _eventNumbers.First())
                    {
                        currEvent = _eventNumbers.TakeWhile(e => e != currEvent.Value).Last();
                        currHeat = _events[currEvent.Value].Heats.Last().Key;
                    }
                    textBoxEvent.Text = Preferences.CurrentEvent = currEvent.Value.ToString();
                    textBoxHeat.Text = Preferences.CurrentHeat = currHeat.Value.ToString();
                }
            }
        }

        private bool refreshStartList()
        {
            _currentDirectory = Directory.GetCurrentDirectory();
            var scbEventsList = StartListUtilities.ReadNamesData(_currentDirectory).Events;
            if (scbEventsList == null || scbEventsList.Count == 0)
            {
                _events = null;
                _eventNumbers = null;
                textBoxEvent.Text = "";
                textBoxHeat.Text = "";

                Preferences.UseOfflineDataOnly = false;
                if (tabControlMode.SelectedTab.Name == "tabPageModeOffline")
                {
                    MessageBox.Show("Cannot display offline data. No .scb files have been saved. Please click the CHOOSE button at the top of the window, and locate these files.");
                }

                return false;
            }
            else
            {
                _events = scbEventsList;
                _eventNumbers = _events.Where(e => e.Value.Heats != null && e.Value.Heats.Any()).ToDictionary(e => e.Key).Keys.ToList();

                if (_events != null && _events.Any() && _eventNumbers != null && _eventNumbers.Any())
                {
                    Preferences.CurrentEvent = textBoxEvent.Text = _eventNumbers.First().ToString();
                    Preferences.CurrentHeat = textBoxHeat.Text = _events[_eventNumbers.First()].Heats.First().Key.ToString();
                }
                else
                {
                }

                return true;
            }
        }

        private void buttonShowStatusMsgs_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Preferences.ErrorMessages);
        }

        private void tabControlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((TabControl)sender).SelectedTab.Name)
            {
                case "tabPageModeSerial":
                    Preferences.UseOfflineDataOnly = false;
                    Preferences.DataMode = Preferences.DataModeType.Serial;

                    if (outputPortNameComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("No output port selected. Choose an output serial (COM) port to send data to the scoreboard.");
                    }
                    break;
                case "tabPageModeParallel":
                    Preferences.UseOfflineDataOnly = false;
                    Preferences.DataMode = Preferences.DataModeType.Parallel;
                    break;
                case "tabPageModeOffline":
                    if (_events == null || !_events.Any() || _eventNumbers == null || !_eventNumbers.Any())
                    {
                        ((CheckBox)sender).Checked = false;
                        MessageBox.Show("Cannot display offline data. No .scb files have been saved. Please click the CHOOSE button at the top of the window, and locate these files.");
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(textBoxEvent.Text))
                        {
                            buttonPlusEventHeat_Click(null, null);
                        }
                        else
                        {
                            Preferences.CurrentEvent = textBoxEvent.Text;
                            Preferences.CurrentHeat = textBoxHeat.Text;
                        }

                        Preferences.UseOfflineDataOnly = true;
                    }

                    break;
            }
        }

        private void outputPortNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(outputPortNameComboBox.Text))
            {
                Preferences.OutputSerialPort = outputPortNameComboBox.Text;
            }
            else
            {
                MessageBox.Show("Please select a valid port name for output.");
            }
        }
    }
}

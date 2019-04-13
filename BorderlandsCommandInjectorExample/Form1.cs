using System;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO.Pipes;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System.Timers;
using BorderlandsCommandInjectorExample.Classes;
using System.Collections;

namespace BorderlandsCommandInjectorExample
{
    public partial class Form1 : Form
    {
        private NamedPipeClientStream pipe;
        private NamedPipeClientStream eventPipe;
        private List<string> responseList = new List<string>();
        private List<string> eventList = new List<string>();
        private BackgroundWorker BW = new BackgroundWorker();
        private BackgroundWorker verifyModuleWorker = new BackgroundWorker();

        private bool isPLuginLoaderInstalled = false;
        private bool isCommandInjectorInstalled = false;

        private System.Timers.Timer processCheckTimer = new System.Timers.Timer();

        private string commandText { get; set; }

        public Form1()
        {
            InitializeComponent();

            // Create BackgroundWorker to create a seperate eventPipe
            BW.DoWork += BW_DoWork;
            BW.RunWorkerCompleted += BW_RunWorkerCompleted;
            BW.RunWorkerAsync();

            verifyModuleWorker.DoWork += VerifyModuleWorker_DoWork;
            verifyModuleWorker.RunWorkerAsync();

            processCheckTimer.Interval = 2000;
            processCheckTimer.Elapsed += ProcessCheckTimer_Elapsed;
            processCheckTimer.AutoReset = true;
            processCheckTimer.Enabled = true;
        }

        /// <summary>
        /// Checking every two seconds whether either of the games + CommandInjector is running to update the status in the UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            verifyModuleWorker.RunWorkerAsync();
        }

        private void VerifyModuleWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("Borderlands2");
                if (processes.Length == 0)
                    processes = Process.GetProcessesByName("BorderlandsPreSequel");

                // Check if either of both games is running and if PluginLoader has been loaded!
                // && processes.FirstOrDefault().Modules.Cast<ProcessModule>().ToList().Any(x => x.FileName.Contains("ddraw"))
                if (processes.Length > 0)
                {
                    var proc = processes.FirstOrDefault();
                    var path = proc.MainModule.FileName.Replace(proc.MainModule.ModuleName, "");

                    isPLuginLoaderInstalled = File.Exists(Path.Combine(path, "ddraw.dll"));
                    isCommandInjectorInstalled = File.Exists(Path.Combine(path, "Plugins", proc.MainModule.ModuleName.Contains("Borderlands2") ? "CommandInjector.dll" : "CommandInjectorTPS.dll"));
                    if (!isCommandInjectorInstalled)
                        isCommandInjectorInstalled = File.Exists(Path.Combine(path, "Plugins", proc.MainModule.ModuleName.Contains("Borderlands2") ? "CommandInjector-UHD.dll" : "CommandInjectorTPS-UHD.dll"));

                    this.lblPluginLoaderStatus.Invoke((MethodInvoker)delegate
                    {
                        if (isPLuginLoaderInstalled && processes.FirstOrDefault().Modules.Cast<ProcessModule>().ToList().Any(x => x.FileName.Contains("ddraw")))
                        {
                            this.lblPluginLoaderStatus.ForeColor = System.Drawing.Color.Green;
                            this.lblPluginLoaderStatus.Text = "PluginLoader is installed and seems to have been loaded by the game!";
                        }
                        if (isPLuginLoaderInstalled && !processes.FirstOrDefault().Modules.Cast<ProcessModule>().ToList().Any(x => x.FileName.Contains("ddraw")))
                        {
                            this.lblPluginLoaderStatus.ForeColor = System.Drawing.Color.Red;
                            this.lblPluginLoaderStatus.Text = "PluginLoader installed, but not loaded by the game!";
                        }
                        if (!isPLuginLoaderInstalled)
                        {
                            this.lblPluginLoaderStatus.ForeColor = System.Drawing.Color.Red;
                            this.lblPluginLoaderStatus.Text = "PluginLoader does not seem to be installed!";
                        }
                    });

                    this.lblModuleRunning.Invoke((MethodInvoker)delegate
                    {
                        if (isCommandInjectorInstalled && processes.FirstOrDefault().Modules.Cast<ProcessModule>().ToList().Any(x => x.FileName.Contains("CommandInjector")))
                        {
                            this.lblModuleRunning.ForeColor = System.Drawing.Color.Green;
                            this.lblModuleRunning.Text = "CommandInjector is installed and seems to have been loaded by the game!";
                        }
                        if (isCommandInjectorInstalled && !processes.FirstOrDefault().Modules.Cast<ProcessModule>().ToList().Any(x => x.FileName.Contains("CommandInjector")))
                        {
                            this.lblModuleRunning.ForeColor = System.Drawing.Color.Red;
                            this.lblModuleRunning.Text = "CommandInjector installed, but not loaded by the game!";
                        }
                        if (!isCommandInjectorInstalled)
                        {
                            this.lblModuleRunning.ForeColor = System.Drawing.Color.Red;
                            this.lblModuleRunning.Text = "CommandInjector does not seem to be installed!";
                        }
                    });
                }

                // Check if either of both games is running and if CommandInjector has been loaded!
                //if (processes.Length > 0 && processes.FirstOrDefault().Modules.Cast<ProcessModule>().ToList().Any(x => x.FileName.Contains("CommandInjector")))
                //{

                //}

                // Check if either of both games is running and if CommandInjector has not been loaded!
                //if (processes.Length > 0 && !processes.FirstOrDefault().Modules.Cast<ProcessModule>().ToList().Any(x => x.FileName.Contains("CommandInjector")))
                //{
                //    this.lblModuleRunning.Invoke((MethodInvoker)delegate
                //    {
                //        this.lblModuleRunning.ForeColor = System.Drawing.Color.Red;
                //        this.lblModuleRunning.Text = "CommandInjector not loaded by the game! Verify install!";
                //    });
                //}

                // If neither of both games is running
                else if (processes.Length == 0)
                {
                    this.lblModuleRunning.Invoke((MethodInvoker)delegate
                    {
                        this.lblModuleRunning.ForeColor = System.Drawing.Color.Red;
                        this.lblModuleRunning.Text = "Neither BL2 nor TPS seem to be running!";
                    });

                    this.lblPluginLoaderStatus.Invoke((MethodInvoker)delegate
                    {
                        this.lblPluginLoaderStatus.ForeColor = System.Drawing.Color.Red;
                        this.lblPluginLoaderStatus.Text = "Neither BL2 nor TPS seem to be running!";
                    });
                }
            }
            catch (Win32Exception) { }
        }

        private void BW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Run EventListener again
            BW.RunWorkerAsync();
        }

        private void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            eventPipe = new NamedPipeClientStream(".", "BLCommandInjectorEvents", PipeDirection.InOut);
            while (!eventPipe.IsConnected)
            {
                try
                {
                    eventPipe.Connect();
                }
                catch (Exception ex) { }
            }

            // Create the WaitHandle to wait for the NamedEvent the DLL fires
            WaitHandle waitEventHandle = new EventWaitHandle(false, EventResetMode.ManualReset, "GlobalReadLevelTransitionEvent");
            try
            {
                // Wait until CommandInjector fired the namedevent
                waitEventHandle.WaitOne();
                if (eventPipe.IsConnected)
                {
                    // Set Pipemode to MessageMode
                    eventPipe.ReadMode = PipeTransmissionMode.Message;
                    var streamReader = new StreamReader(eventPipe);

                    string eventCheck = "";
                    eventList.Clear();

                    // While there is still data to read, add the current line to the List
                    while ((eventCheck = streamReader.ReadLine()) != null)
                    {
                        if (eventCheck != string.Empty)
                            eventList.Add(eventCheck);
                    }

                    streamReader.Close();
                    streamReader.Dispose();

                    // Update UI from Non-UI-Thread
                    if (eventList.Count > 0)
                    {
                        this.rtbEvents.Invoke((MethodInvoker)delegate
                        {
                            this.rtbEvents.AppendText(eventList[0] + "\r\n");
                        });
                    }
                }
            }
            catch (IOException ioexc)
            {
                // Ignore "Pipe Interrupted/Broken" exception
                if (ioexc.HResult.ToString("X") != "800700E9")
                {
                    //Debugger.Break();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void textBoxCommand_TextChanged(object sender, EventArgs e)
        {
            commandText = (sender as TextBox)?.Text;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            pipe = new NamedPipeClientStream(".", "BLCommandInjector", PipeDirection.InOut);

            if (!pipe.IsConnected)
                pipe.Connect();

            if (pipe.IsConnected)
            {
                try
                {
                    // Create a new StreaWriter which disposes itself after usage
                    using (var sw = new StreamWriter(pipe))
                    {
                        // Don't send empty commands to the pipe, ya doofus!
                        if (!string.IsNullOrWhiteSpace(commandText))
                        {
                            sw.WriteLine(commandText);

                            // Flush the Buffer, so the Pipe gets a signal to make sure it's read from completely!!
                            sw.Flush();
                        }

                        //
                        // If you want to get a response, read it here, otherwise, server will close the connection!
                        // 

                        // Set Pipemode to MessageMode
                        pipe.ReadMode = PipeTransmissionMode.Message;

                        // No "using" this time, since otherwise we get an exception, since the pipe closes automatically when the Stream closes/disposes
                        var reader = new StreamReader(pipe);
                        

                        string check = "";

                        // Create a new empty List which will hold our console output line by line.
                        responseList.Clear();

                        // While there is still data to read, add the current line to the List
                        while ((check = reader.ReadLine()) != null)
                        {
                            if (check != string.Empty)
                                responseList.Add(check);
                        }

                        this.richTextBox1.Invoke((MethodInvoker)delegate { this.richTextBox1.ResetText(); });
                        foreach (var line in responseList)
                        {
                            this.richTextBox1.Invoke((MethodInvoker)delegate { richTextBox1.AppendText(line + "\r\n"); });
                        }
                    }
                }
                catch (IOException ioexc)
                {
                    // Ignore "Pipe Interrupted/Broken" exception
                    if (ioexc.HResult.ToString("X") != "80131620")
                    {
                        //Debugger.Break();
                    }
                }
            }
            // Clear the Command TextBox in the UI
            this.textBoxCommand.Invoke((MethodInvoker)delegate { this.textBoxCommand.Text = ""; });
        }
    }
}

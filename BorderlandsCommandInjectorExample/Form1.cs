using System;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO.Pipes;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace BorderlandsCommandInjectorExample
{
    public partial class Form1 : Form
    {
        private NamedPipeClientStream pipe;
        private NamedPipeClientStream eventPipe;
        private List<string> responseList = new List<string>();
        private List<string> eventList = new List<string>();
        private BackgroundWorker BW = new BackgroundWorker();

        private string commandText { get; set; }

        public Form1()
        {
            InitializeComponent();

            // Create BackgroundWorker to create a seperate eventPipe
            BW.DoWork += BW_DoWork;
            BW.RunWorkerCompleted += BW_RunWorkerCompleted;
            BW.RunWorkerAsync();
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

                        // Refresh the DataSource in the UI
                        if (responseList.Count > 0)
                        {
                            //this.rtbEvents.DataSource = null;
                            //this.listBoxEvents.DataSource = responseList;
                        }
                        richTextBox1.ResetText();
                        foreach (var line in responseList)
                        {
                            richTextBox1.AppendText(line + "\r\n");
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
            this.textBoxCommand.Text = "";
        }
    }
}

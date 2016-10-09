﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KGTCopyFile
{
    public partial class Form1 : Form
    {
        const int BLOCK_SIZE = 4096;

        private BackgroundWorker worker = new BackgroundWorker();
        private String sourceFilePath = null;
        private String destinationFilePath = null;

        enum UIState { Waiting, Copying, Paused};
        private UIState state = UIState.Waiting;

        private void SetUIState(UIState newState)
        {
            state = newState;
            switch (state)
            {
                case UIState.Waiting:
                    copyButton.Text = "Copy";
                    cancelButton.Enabled = false;
                    copyProgressBar.Value = 0;
                    break;
                case UIState.Paused:
                    copyButton.Text = "Resume";
                    cancelButton.Enabled = true;
                    break;
                case UIState.Copying:
                    copyButton.Text = "Pause";
                    cancelButton.Enabled = true;
                    break;
            }
        }

        private void worker_Init()
        {
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open);
            FileStream destinationStream = new FileStream(destinationFilePath, FileMode.CreateNew);

            for (int i = 0; i < 100; i++)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(100);
                    worker.ReportProgress(i);
                }
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            copyProgressBar.Value = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("File was successfully copied.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public Form1()
        {
            InitializeComponent();
            worker_Init();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void StartCopying()
        {
            sourceFilePath = sourceTextBox.Text;
            if (!File.Exists(sourceFilePath))
            {
                //MessageBox.Show("Source file doesn´t exist at specified path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
            }

            String fileName = Path.GetFileName(sourceFilePath);
            String destinationFolderPath = destinationTextBox.Text;
            if (!Directory.Exists(destinationFolderPath))
            {
                //MessageBox.Show("Destination folder doesn´t exist at specified path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
            }

            destinationFilePath = Path.Combine(destinationFolderPath, fileName);
            if (File.Exists(destinationFilePath))
            {
                //MessageBox.Show("A file called " + fileName + " already exists in destination folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
            }

            SetUIState(UIState.Copying);
            
            worker.RunWorkerAsync();
        }

        private void PauseCopying()
        {
            SetUIState(UIState.Paused);
        }

        private void ResumeCopying()
        {
            SetUIState(UIState.Copying);
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case UIState.Waiting:
                    StartCopying();
                    break;
                case UIState.Paused:
                    ResumeCopying();
                    break;
                case UIState.Copying:
                    PauseCopying();
                    break;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }

            SetUIState(UIState.Waiting);
        }
    }
}
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace KGTCopyFile
{
    public partial class Form1 : Form
    {
        const int BLOCK_SIZE = 1024 * 1024; // size of copied file chunks

        private BackgroundWorker worker = new BackgroundWorker();
        private ManualResetEvent pauseEvent = new ManualResetEvent(true);

        private string sourceFilePath;
        private string destinationFilePath;

        enum UIState { Waiting, Copying, Paused};
        private UIState state = UIState.Waiting;

        private void SetUIState(UIState newState)
        {
            // makes sure correct text is displayed on the copy button, keeps cancel button enabled only when needed
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

        public Form1()
        {
            InitializeComponent();
            worker_Init();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void copyButton_Click(object sender, EventArgs e)
        {
            // depending on current state, copyButton can start/pause/resume copying
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
            if (worker.IsBusy) // if worker is running
            {
                worker.CancelAsync();
                if (state == UIState.Paused) // background worker must first resume execution before it can cancel copying
                {
                    worker_ResumeWork();
                }
            }

            SetUIState(UIState.Waiting);
        }

        private void StartCopying()
        {
            // check sanity of inputs
            sourceFilePath = sourceTextBox.Text;
            if (!File.Exists(sourceFilePath))
            {
                MessageBox.Show("Source file doesn´t exist at specified path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fileName = Path.GetFileName(sourceFilePath);
            string destinationFolderPath = destinationTextBox.Text;
            if (!Directory.Exists(destinationFolderPath))
            {
                MessageBox.Show("Destination folder doesn´t exist at specified path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // keep the same filename at destination path
            destinationFilePath = Path.Combine(destinationFolderPath, fileName);
            if (File.Exists(destinationFilePath))
            {
                MessageBox.Show("File " + fileName + " already exists in destination folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // avoid starting starting new copying when old one hasn´t yet finished
            if (worker.IsBusy) return;

            SetUIState(UIState.Copying);
            worker.RunWorkerAsync();
            worker_ResumeWork();
        }

        private void PauseCopying()
        {
            worker_PauseWork();
            SetUIState(UIState.Paused);
        }

        private void ResumeCopying()
        {
            worker_ResumeWork();
            SetUIState(UIState.Copying);
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
            byte[] buffer = new byte[BLOCK_SIZE];
            long fileSize = sourceStream.Length;
            int bytesCopied = 0;
            int bytesRead;

            while ((bytesRead = sourceStream.Read(buffer, 0, BLOCK_SIZE)) > 0)
            {
                pauseEvent.WaitOne(Timeout.Infinite); // worker waits here indefinitely when copying is paused

                if (worker.CancellationPending == true)
                {
                    // cancel copying and delete the uncomplete file
                    e.Cancel = true;
                    sourceStream.Close();
                    destinationStream.Close();
                    File.Delete(destinationFilePath);
                    return;
                }

                destinationStream.Write(buffer, 0, bytesRead);

                bytesCopied += bytesRead;
                int percentage = (int)(((float)bytesCopied / fileSize) * 100);

                worker.ReportProgress(percentage);
            }

            sourceStream.Close();
            destinationStream.Close();
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (state == UIState.Copying) // update progressbar only when copying is active
            {
                copyProgressBar.Value = e.ProgressPercentage;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetUIState(UIState.Waiting);

            if (!e.Cancelled)
            {
                copyProgressBar.Value = 100;
                MessageBox.Show("File was successfully copied.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void worker_PauseWork()
        {
            // notifies worker to pause work
            pauseEvent.Reset();
        }

        private void worker_ResumeWork()
        {
            // notifies worker to resume work
            pauseEvent.Set();
        }
    }
}

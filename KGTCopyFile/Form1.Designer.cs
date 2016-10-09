namespace KGTCopyFile
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.copyButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.sourceTextBox = new System.Windows.Forms.TextBox();
            this.destinationTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.copyProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // copyButton
            // 
            this.copyButton.Location = new System.Drawing.Point(18, 94);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(223, 32);
            this.copyButton.TabIndex = 0;
            this.copyButton.Text = "Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Enabled = false;
            this.cancelButton.Location = new System.Drawing.Point(247, 94);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(106, 32);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.Location = new System.Drawing.Point(113, 9);
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.Size = new System.Drawing.Size(240, 20);
            this.sourceTextBox.TabIndex = 2;
            // 
            // destinationTextBox
            // 
            this.destinationTextBox.Location = new System.Drawing.Point(113, 39);
            this.destinationTextBox.Name = "destinationTextBox";
            this.destinationTextBox.Size = new System.Drawing.Size(240, 20);
            this.destinationTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Source filepath:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Destination folder:";
            // 
            // copyProgressBar
            // 
            this.copyProgressBar.Location = new System.Drawing.Point(18, 75);
            this.copyProgressBar.Name = "copyProgressBar";
            this.copyProgressBar.Size = new System.Drawing.Size(335, 13);
            this.copyProgressBar.Step = 1;
            this.copyProgressBar.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 134);
            this.Controls.Add(this.copyProgressBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.destinationTextBox);
            this.Controls.Add(this.sourceTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.copyButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox sourceTextBox;
        private System.Windows.Forms.TextBox destinationTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar copyProgressBar;
    }
}


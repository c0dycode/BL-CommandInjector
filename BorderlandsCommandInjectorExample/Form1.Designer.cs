namespace BorderlandsCommandInjectorExample
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
            this.btnSend = new System.Windows.Forms.Button();
            this.textBoxCommand = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.rtbEvents = new System.Windows.Forms.RichTextBox();
            this.lblModuleRunning = new System.Windows.Forms.Label();
            this.lblPluginLoaderStatus = new System.Windows.Forms.Label();
            this.lblLineCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(272, 20);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // textBoxCommand
            // 
            this.textBoxCommand.Location = new System.Drawing.Point(13, 22);
            this.textBoxCommand.Name = "textBoxCommand";
            this.textBoxCommand.Size = new System.Drawing.Size(253, 20);
            this.textBoxCommand.TabIndex = 1;
            this.textBoxCommand.TextChanged += new System.EventHandler(this.textBoxCommand_TextChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 49);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(965, 746);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // rtbEvents
            // 
            this.rtbEvents.Location = new System.Drawing.Point(12, 801);
            this.rtbEvents.Name = "rtbEvents";
            this.rtbEvents.Size = new System.Drawing.Size(965, 81);
            this.rtbEvents.TabIndex = 4;
            this.rtbEvents.Text = "";
            // 
            // lblModuleRunning
            // 
            this.lblModuleRunning.AutoSize = true;
            this.lblModuleRunning.Location = new System.Drawing.Point(354, 29);
            this.lblModuleRunning.Name = "lblModuleRunning";
            this.lblModuleRunning.Size = new System.Drawing.Size(123, 13);
            this.lblModuleRunning.TabIndex = 5;
            this.lblModuleRunning.Text = "CommandInjector status:";
            // 
            // lblPluginLoaderStatus
            // 
            this.lblPluginLoaderStatus.AutoSize = true;
            this.lblPluginLoaderStatus.Location = new System.Drawing.Point(354, 4);
            this.lblPluginLoaderStatus.Name = "lblPluginLoaderStatus";
            this.lblPluginLoaderStatus.Size = new System.Drawing.Size(103, 13);
            this.lblPluginLoaderStatus.TabIndex = 6;
            this.lblPluginLoaderStatus.Text = "PluginLoader status:";
            // 
            // lblLineCount
            // 
            this.lblLineCount.AutoSize = true;
            this.lblLineCount.Location = new System.Drawing.Point(13, 4);
            this.lblLineCount.Name = "lblLineCount";
            this.lblLineCount.Size = new System.Drawing.Size(0, 13);
            this.lblLineCount.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 894);
            this.Controls.Add(this.lblLineCount);
            this.Controls.Add(this.lblPluginLoaderStatus);
            this.Controls.Add(this.lblModuleRunning);
            this.Controls.Add(this.rtbEvents);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBoxCommand);
            this.Controls.Add(this.btnSend);
            this.Name = "Form1";
            this.Text = "Borderlands 2 & TPS Command Injector";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox textBoxCommand;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox rtbEvents;
        private System.Windows.Forms.Label lblModuleRunning;
        private System.Windows.Forms.Label lblPluginLoaderStatus;
        private System.Windows.Forms.Label lblLineCount;
    }
}


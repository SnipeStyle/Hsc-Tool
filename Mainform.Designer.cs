namespace HscTool
{
    partial class Mainform
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
            this.label1 = new System.Windows.Forms.Label();
            this.MapButton = new System.Windows.Forms.Button();
            this.MapTextBox = new System.Windows.Forms.TextBox();
            this.extractProgressBar = new System.Windows.Forms.ProgressBar();
            this.startButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.OutputButton = new System.Windows.Forms.Button();
            this.outputDirBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.searchValueUpDown = new System.Windows.Forms.NumericUpDown();
            this.valueSearchCheckBox = new System.Windows.Forms.CheckBox();
            this.stringsCheckBox = new System.Windows.Forms.CheckBox();
            this.hscCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchValueUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Map Path:";
            // 
            // MapButton
            // 
            this.MapButton.Location = new System.Drawing.Point(389, 7);
            this.MapButton.Name = "MapButton";
            this.MapButton.Size = new System.Drawing.Size(75, 23);
            this.MapButton.TabIndex = 1;
            this.MapButton.Text = "Select";
            this.MapButton.UseVisualStyleBackColor = true;
            this.MapButton.Click += new System.EventHandler(this.MapButton_Click);
            // 
            // MapTextBox
            // 
            this.MapTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MapTextBox.Location = new System.Drawing.Point(81, 10);
            this.MapTextBox.Name = "MapTextBox";
            this.MapTextBox.ReadOnly = true;
            this.MapTextBox.Size = new System.Drawing.Size(302, 20);
            this.MapTextBox.TabIndex = 2;
            // 
            // extractProgressBar
            // 
            this.extractProgressBar.Location = new System.Drawing.Point(12, 250);
            this.extractProgressBar.Name = "extractProgressBar";
            this.extractProgressBar.Size = new System.Drawing.Size(459, 23);
            this.extractProgressBar.TabIndex = 6;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(201, 208);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Output Dir:";
            // 
            // OutputButton
            // 
            this.OutputButton.Location = new System.Drawing.Point(389, 39);
            this.OutputButton.Name = "OutputButton";
            this.OutputButton.Size = new System.Drawing.Size(75, 23);
            this.OutputButton.TabIndex = 1;
            this.OutputButton.Text = "Select";
            this.OutputButton.UseVisualStyleBackColor = true;
            this.OutputButton.Click += new System.EventHandler(this.OutputButton_Click);
            // 
            // outputDirBox
            // 
            this.outputDirBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outputDirBox.Location = new System.Drawing.Point(81, 41);
            this.outputDirBox.Name = "outputDirBox";
            this.outputDirBox.ReadOnly = true;
            this.outputDirBox.Size = new System.Drawing.Size(302, 20);
            this.outputDirBox.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.searchValueUpDown);
            this.groupBox1.Controls.Add(this.valueSearchCheckBox);
            this.groupBox1.Controls.Add(this.stringsCheckBox);
            this.groupBox1.Controls.Add(this.hscCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(15, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(449, 102);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select The Tasks You Want To Perform :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Index:";
            // 
            // searchValueUpDown
            // 
            this.searchValueUpDown.Enabled = false;
            this.searchValueUpDown.Location = new System.Drawing.Point(204, 65);
            this.searchValueUpDown.Name = "searchValueUpDown";
            this.searchValueUpDown.Size = new System.Drawing.Size(120, 20);
            this.searchValueUpDown.TabIndex = 1;
            this.searchValueUpDown.ValueChanged += new System.EventHandler(this.searchValueUpDown_ValueChanged);
            // 
            // valueSearchCheckBox
            // 
            this.valueSearchCheckBox.AutoSize = true;
            this.valueSearchCheckBox.Location = new System.Drawing.Point(6, 66);
            this.valueSearchCheckBox.Name = "valueSearchCheckBox";
            this.valueSearchCheckBox.Size = new System.Drawing.Size(136, 17);
            this.valueSearchCheckBox.TabIndex = 0;
            this.valueSearchCheckBox.Text = "Search for a value type";
            this.valueSearchCheckBox.UseVisualStyleBackColor = true;
            this.valueSearchCheckBox.CheckedChanged += new System.EventHandler(this.valueSearchCheckBox_CheckedChanged);
            // 
            // stringsCheckBox
            // 
            this.stringsCheckBox.AutoSize = true;
            this.stringsCheckBox.Location = new System.Drawing.Point(6, 43);
            this.stringsCheckBox.Name = "stringsCheckBox";
            this.stringsCheckBox.Size = new System.Drawing.Size(89, 17);
            this.stringsCheckBox.TabIndex = 0;
            this.stringsCheckBox.Text = "Dump Strings";
            this.stringsCheckBox.UseVisualStyleBackColor = true;
            // 
            // hscCheckBox
            // 
            this.hscCheckBox.AutoSize = true;
            this.hscCheckBox.Location = new System.Drawing.Point(7, 20);
            this.hscCheckBox.Name = "hscCheckBox";
            this.hscCheckBox.Size = new System.Drawing.Size(105, 17);
            this.hscCheckBox.TabIndex = 0;
            this.hscCheckBox.Text = "Extract Hsc Files";
            this.hscCheckBox.UseVisualStyleBackColor = true;
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 288);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.extractProgressBar);
            this.Controls.Add(this.outputDirBox);
            this.Controls.Add(this.MapTextBox);
            this.Controls.Add(this.OutputButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MapButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Mainform";
            this.Text = "Hsc Tool - Created By SnipeStyle";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchValueUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button MapButton;
        private System.Windows.Forms.TextBox MapTextBox;
        private System.Windows.Forms.ProgressBar extractProgressBar;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OutputButton;
        private System.Windows.Forms.TextBox outputDirBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox valueSearchCheckBox;
        private System.Windows.Forms.CheckBox stringsCheckBox;
        private System.Windows.Forms.CheckBox hscCheckBox;
        private System.Windows.Forms.NumericUpDown searchValueUpDown;
        private System.Windows.Forms.Label label3;
    }
}


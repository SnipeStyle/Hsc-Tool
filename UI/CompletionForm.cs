using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HscTool
{
    public partial class CompletionForm : Form
    {
        public CompletionForm()
        {
            InitializeComponent();
        }

        public void AddCompletionTime(TimeSpan Time)
        {
            double timeInSeconds = Math.Round(Time.TotalSeconds, 3);
            string message = "All tasks have been completed in " + timeInSeconds + " seconds.";
            this.MessageBox.AppendText(Environment.NewLine + message);
        }

        public void AddBlankLine()
        {
            this.MessageBox.AppendText(Environment.NewLine);
        }

        public void AddMessages(List<string> Messages)
        {
            foreach (string str in Messages)
            {
                this.MessageBox.AppendText(Environment.NewLine + "- " + str);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CompletionForm_SizeChanged(object sender, EventArgs e)
        {
            CloseButton.Left = (this.ClientSize.Width - CloseButton.Width) / 2;
        }

        public void ShowDialog(TimeSpan Time, List<string> Messages)
        {
            AddCompletionTime(Time);
            AddBlankLine();
            AddMessages(Messages);
            this.ShowDialog();
        }
    }
}

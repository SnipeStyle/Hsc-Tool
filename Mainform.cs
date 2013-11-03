using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using Assembly.IO;
using HscTool.Blam;

namespace HscTool
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
        }

        private string mapPath = "";
        private string mapName = "";
        private string folderSelectedByUser = "";
        private string outputPath = "";

        Int32 valueTypeIndex;
        bool containsScenario;
        bool containsGlobal;

        UInt32 scenarioHsdtAddress;
        UInt32 globalHsdtAddress;

        #region UI Interaction
        private void MapButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Halo4 Cashe File(*.map)|*.map";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                mapPath = ofd.FileName;
                mapName = Path.GetFileNameWithoutExtension(mapPath);
                MapTextBox.Text = mapPath;
            }
        }

        private void OutputButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                outputDirBox.Text = fbd.SelectedPath;
                folderSelectedByUser = fbd.SelectedPath;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (mapPath == "")
            {
                MessageBox.Show("Please select a map file.", "No map file selected.");
                return;
            }

            if (folderSelectedByUser == "")
            {
                MessageBox.Show("Please select the directory, where you want to store the extracted information.", "No output directory selected.");
                return;
            }

            if (hscCheckBox.Checked == false && stringsCheckBox.Checked == false && valueSearchCheckBox.Checked == false)
            {
                MessageBox.Show("Please select the tasks you want to perform.", "No tasks selected.");
                return;
            }

            ToggleControls(false);
            ResetAddresses();

            outputPath = Path.Combine(folderSelectedByUser, mapName);
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            /// the worker we'll be using.
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = false;
            worker.WorkerReportsProgress = true;
            worker.DoWork += bw_DoWork;
            worker.ProgressChanged += bw_ProgressChanged;
            worker.RunWorkerCompleted += bw_RunWorkerCompleted;

            /// start the actual operations
            worker.RunWorkerAsync();

        }

        private void ToggleControls(Boolean state)
        {
            MapButton.Enabled = state;
            OutputButton.Enabled = state;

            hscCheckBox.Enabled = state;
            stringsCheckBox.Enabled = state;
            valueSearchCheckBox.Enabled = state;
            searchValueUpDown.Enabled = state;

            startButton.Enabled = state;
        }

        private void ResetAddresses()
        {
            scenarioHsdtAddress = 0;
            globalHsdtAddress = 0;
        }

        private void valueSearchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (valueSearchCheckBox.Checked)
            {
                searchValueUpDown.Enabled = true;
            }
            else
            {
                searchValueUpDown.Enabled = false;
                searchValueUpDown.Value = 0;
            }
        }
        #endregion

        #region Worker
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var startTime = DateTime.Now;
            var completionMessages = new List<string>();
            var worker = sender as BackgroundWorker;
            var reader = new EndianReader(new FileStream(mapPath, FileMode.Open, FileAccess.Read), Endian.BigEndian);
            var map = new CasheFile(reader);
            

            GetTagAdresses(reader, map);

            if (hscCheckBox.Checked)
            {
                ExtractHscMethod(reader, map, completionMessages);
            }
            worker.ReportProgress(33);

            if (stringsCheckBox.Checked)
            {
                DumpStringsMethod(reader, map, completionMessages);
            }
            worker.ReportProgress(66);

            if (valueSearchCheckBox.Checked)
            {
                SearchValueTypeMethod(reader, map, completionMessages);
            }

            var completionTime = DateTime.Now - startTime;
            var result = new Tuple<TimeSpan, List<string>>(completionTime, completionMessages);
            worker.ReportProgress(100);
            e.Result = result;
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show("An error occured during one of the tasks: " + e.Error.Message, "Error!");
            else
            {
                var resultMessages = e.Result as Tuple<TimeSpan, List<string>>;
                var form = new CompletionForm();
                form.ShowDialog(resultMessages.Item1, resultMessages.Item2);
            }
            extractProgressBar.Value = 0;
            ToggleControls(true);
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            extractProgressBar.Value = e.ProgressPercentage;
        }
        #endregion

        #region Map Methods
        private void ExtractHscMethod(IReader reader, CasheFile Map, List<string> messageList)
        {
            /// the list which will be used to store all individual files and be used to ectract them later
            SortedList<string, HscFile> sourceFiles = new SortedList<string, HscFile>();

            /// filter the map's tags by their tagclass. the hsdt tags are the interesting ones.
            List<Tag> hsdtTags = Map.GetTagsByClass("hsdt");

            if (hsdtTags.Count == 0)
            {
                messageList.Add("This map doesn't contain any hsdt tags.");
                return;
            }

            #region Collecting the files...
            foreach (Tag tag in hsdtTags)
            {
                /// move the reader to the start of the tag data
                reader.SeekTo(tag.metaAddress - Map.MapMagic);
                Int32 sourceFileBlockCount = reader.ReadInt32();
                if (sourceFileBlockCount > 0)
                {
                    UInt32 sourceFileBlockAddress = reader.ReadUInt32();

                    reader.SeekTo(sourceFileBlockAddress - Map.MapMagic);
                    for (int i = 0; i < sourceFileBlockCount; i++)
                    {
                        HscFile sourceFile = new HscFile(reader);

                        /// if our file list doesnt contain this file yet, it will be added, so we can extract it later.
                        if (!sourceFiles.ContainsKey(sourceFile.Name))
                        {
                            sourceFiles.Add(sourceFile.Name, sourceFile);
                        }
                    }
                }
            }
            #endregion

            if (sourceFiles.Count == 0)
            {
                messageList.Add("This map doesn't contain any script files.");
                return;
            }

            #region Extract the files...
            foreach (KeyValuePair<string, HscFile> hscFile in sourceFiles)
            {
                reader.SeekTo(hscFile.Value.Address - Map.MapMagic);
                var data = reader.ReadBlock(hscFile.Value.Size);
                var path = Path.Combine(outputPath,hscFile.Value.Name + ".hsc");
                var stream = System.IO.File.Open(path, FileMode.Create);
                stream.Write(data, 0, data.Length);
                stream.Dispose();
                stream.Close();
            }
            messageList.Add(sourceFiles.Count + " script files have been extracted.");
            #endregion
        }

        private void DumpStringsMethod(IReader reader, CasheFile Map, List<string> messageList)
        {
            var stringsDir = Path.Combine(outputPath, "Strings");
            bool dumpedScenario = false;
            bool dumpedGlobal = false;

            #region scenario hsdt
            if (containsScenario)
            {
                    reader.SeekTo((scenarioHsdtAddress - Map.MapMagic) + 0x54);
                    UInt32 size = reader.ReadUInt32();
                    reader.Skip(8);
                    UInt32 address = reader.ReadUInt32();
                    if(size > 0)
                    {
                        var strings = ReadStringTableNew(reader, (address  - Map.MapMagic), size);

                        if (strings.Count != 0)
                        {
                            if (!Directory.Exists(stringsDir))
                                Directory.CreateDirectory(stringsDir);

                            var fileName = Path.Combine(stringsDir, (mapName + "_scenario" + ".txt"));
                            var writer = new StreamWriter(fileName, false);

                            foreach (string str in strings)
                            {
                                writer.WriteLine(str);
                            }

                            writer.Dispose();
                            writer.Close();

                            dumpedScenario = true;
                        }
                    }
            }
#endregion

            #region global hsdt
            if (containsGlobal)
            {
                reader.SeekTo((globalHsdtAddress - Map.MapMagic) + 0x54);
                UInt32 size = reader.ReadUInt32();
                reader.Skip(8);
                UInt32 address = reader.ReadUInt32();
                if (size > 0)
                {
                    var strings = ReadStringTableNew(reader, address - Map.MapMagic, size);

                    if (strings.Count != 0)
                    {
                        if (!Directory.Exists(stringsDir))
                            Directory.CreateDirectory(stringsDir);

                        var fileName = Path.Combine(stringsDir, (mapName + "_global" + ".txt"));
                        var writer = new StreamWriter(fileName, false);

                        foreach (string str in strings)
                        {
                            writer.WriteLine(str);
                        }

                        writer.Dispose();
                        writer.Close();

                        dumpedGlobal = true;
                    }
                }
            }
            #endregion

            if (dumpedScenario && dumpedGlobal)
            {
                messageList.Add("All global and scenario strings have been dumped.");
            }
            else if (!dumpedScenario && !dumpedGlobal)
            {
                messageList.Add("This map doesn't contain any strings.");
            }
            else if (dumpedScenario)
            {
                messageList.Add("All scenario strings have been dumped.");
            }
            else if (dumpedGlobal)
            {
                messageList.Add("All global strings have been dumped.");
            }
        }

        private void SearchValueTypeMethod(IReader reader, CasheFile Map, List<string> messageList)
        {         
            var occurrences = new List<string>();

            #region Scenario
            if (containsScenario)
            {
                reader.SeekTo((scenarioHsdtAddress - Map.MapMagic) + 0x48);
                Int32 count = reader.ReadInt32();
                UInt32 address = reader.ReadUInt32();

                if (count != 0 && address != 0)
                {
                    reader.SeekTo(address - Map.MapMagic);

                    for (Int32 currentNode = 0; currentNode < count; currentNode++)
                    {
                        var node = new ScriptNode(reader);
                        ProcessScriptNode(node, "Scenario", currentNode, valueTypeIndex, occurrences);
                    }
                }
            }
            #endregion

            #region Global
            if (containsGlobal)
            {
                reader.SeekTo((scenarioHsdtAddress - Map.MapMagic) + 0x48);
                Int32 count = reader.ReadInt32();
                UInt32 address = reader.ReadUInt32();

                if (count != 0 && address != 0)
                {
                    reader.SeekTo(address - Map.MapMagic);

                    for (Int32 currentNode = 0; currentNode < count; currentNode++)
                    {
                        var node = new ScriptNode(reader);
                        ProcessScriptNode(node, "Global", currentNode, valueTypeIndex, occurrences);
                    }
                }
            }
            #endregion

            if (occurrences.Count > 0)
            {
                var ValueTypeDir = Path.Combine(outputPath, "Value Types");
                if (!Directory.Exists(ValueTypeDir))
                    Directory.CreateDirectory(ValueTypeDir);

                var fileName = Path.Combine(ValueTypeDir, ("Value Type" + valueTypeIndex + ".txt"));

                var writer = new StreamWriter(fileName, false);

                foreach (string str in occurrences)
                {
                    writer.WriteLine(str);
                }
                writer.Dispose();
                writer.Close();

                messageList.Add(occurrences.Count + " occurrences of the value type " + valueTypeIndex + " have been found.");
            }
            else
            {
                messageList.Add("No occurences of the value type " + valueTypeIndex + " have been found.");
            }
        }
        #endregion


        private void GetTagAdresses(IReader reader, CasheFile Map)
        {
            var scnrTag = Map.GetTagByClass("scnr");
            var stringsDir = Path.Combine(outputPath, "Strings");

            if (scnrTag == null)
                return;

            #region scnr
            reader.SeekTo((scnrTag.metaAddress - Map.MapMagic) + 0x498 + 0xE);
            Int16 scenarioTagIndex = reader.ReadInt16();

            reader.Skip(0xC + 0xE);
            Int16 globalHscnTagIndex = reader.ReadInt16();
            #endregion

            if (scenarioTagIndex > -1)
            {
                var tag = Map.Tags[scenarioTagIndex];
                if (tag.Class.Name == "hsdt")
                {
                    containsScenario = true;
                    scenarioHsdtAddress = tag.metaAddress;
                }
            }
            if (globalHscnTagIndex > -1)
            {
                var tag = Map.Tags[globalHscnTagIndex];
                if (tag.Class.Name == "hscn")
                {
                    reader.SeekTo((tag.metaAddress - Map.MapMagic) + 0x18 + 0xE);
                    Int16 globalHsdtTagIndex = reader.ReadInt16();
                    if (globalHsdtTagIndex > -1)
                    {
                        var hsdtTag = Map.Tags[globalHsdtTagIndex];
                        if (hsdtTag.Class.Name == "hsdt")
                        {
                            containsGlobal = true;
                            globalHsdtAddress = hsdtTag.metaAddress;
                        }
                    }
                }
            }
        }

        private void ProcessScriptNode(ScriptNode node, string location, Int32 currentNodeIndex, Int32 index, List<String> occurrences)
        {
            /// Expression
            if (node.Expression == ExpressionType.Expression)
            {
                // eliminate those annoying function_names
                if (node.ReturnType != 2)
                {
                    if (node.Identity == index || node.ReturnType == index)
                    {
                        string str = location + " ----- Expression Type: Expression ----- Node: " + currentNodeIndex;
                        occurrences.Add(str);
                    }
                }
            }
                ///Global Reference & Script Parameter & Local Reference & External Global
            else if (node.Expression == ExpressionType.GlobalReference || node.Expression == ExpressionType.ScriptParameter ||
                node.Expression == ExpressionType.LocalReference || node.Expression == ExpressionType.ExternalGlobal)
            {
                if (node.Identity == index || node.ReturnType == index)
                {
                    string str = location + " ----- Expression Type: " + node.Expression + " ----- Node: " + currentNodeIndex;
                    occurrences.Add(str);
                }
            }
                /// Script Reference & External Script
            else if (node.Expression == ExpressionType.ScriptReference || node.Expression == ExpressionType.ExternalScript)
            {
                if (node.ReturnType == index)
                {
                    string str = location + " ----- Expression Type: " + node.Expression + " ----- Node: " + currentNodeIndex;
                    occurrences.Add(str);
                }
            }
        }

        /// <summary>
        /// Reads the null terminated strings out of a data block of a given length. The whole method is a mess and should rot in hell.
        /// </summary>
        /// <param name="Reader"></param>
        /// <param name="Offset"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        private List<string> ReadStringTableNew(IReader Reader, UInt32 Offset, UInt32 Size)
        {
            var builder = new StringBuilder();
            var result = new List<string>();

            Reader.SeekTo(Offset);
            byte[] data = Reader.ReadBlock((int)Size);

            int index = 0;
            bool newString = true;

            for (int i = 0; i <= (data.Length -1); i++)
            {
                if (newString)
                {
                    index = i;
                }

                byte currentByte = data[i];

                if (currentByte == 0xCD)
                    break;

                if (currentByte == 0x0 || currentByte == 0xFF)
                {
                    string currentString = builder.ToString();

                    if (currentString == "")
                    {
                        currentString = ".";
                    }

                    string str = index + " ----------- " + currentString;
                    result.Add(str);
                    builder.Clear();
                    newString = true;
                }
                else
                {
                    builder.Append((char)currentByte);
                    newString = false;
                }
            }
            return result;
        }

        private void searchValueUpDown_ValueChanged(object sender, EventArgs e)
        {
            valueTypeIndex = (int)searchValueUpDown.Value;
        }        
    }
}

﻿using HipHopFile;
using RenderWareFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static IndustrialPark.Models.BSP_IO_Shared;
using static IndustrialPark.Models.Model_IO_Assimp;

namespace IndustrialPark
{
    public partial class ImportModel : Form
    {
        public ImportModel()
        {
            InitializeComponent();
            
            buttonOK.Enabled = false;
            TopMost = true;
        }

        List<string> filePaths = new List<string>();

        private void buttonImportRawData_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = GetImportFilter()
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string s in openFileDialog.FileNames)
                    filePaths.Add(s);

                UpdateListBox();
            }
        }

        private void UpdateListBox()
        {
            listBox1.Items.Clear();

            foreach (string s in filePaths)
                listBox1.Items.Add(Path.GetFileName(s));

            buttonOK.Enabled = listBox1.Items.Count > 0;
        }
        
        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                filePaths.RemoveAt(listBox1.SelectedIndex);
                UpdateListBox();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static List<Section_AHDR> GetAssets(out bool success, out bool overwrite)
        {
            ImportModel a = new ImportModel();
            if (a.ShowDialog() == DialogResult.OK)
            {
                List<Section_AHDR> AHDRs = new List<Section_AHDR>();

                foreach (string filePath in a.filePaths)
                {
                    string assetName = Path.GetFileNameWithoutExtension(filePath) + ".dff";
                    AssetType assetType = AssetType.MODL;
                    byte[] assetData = Path.GetExtension(filePath).ToLower().Equals(".dff") ?
                                File.ReadAllBytes(filePath) :
                                ReadFileMethods.ExportRenderWareFile(
                                    CreateDFFFromAssimp(filePath,
                                    a.checkBoxFlipUVs.Checked),
                                    currentRenderWareVersion);
                    
                    AHDRs.Add(
                        new Section_AHDR(
                            Functions.BKDRHash(assetName),
                            assetType,
                            ArchiveEditorFunctions.AHDRFlagsFromAssetType(assetType),
                            new Section_ADBG(0, assetName, "", 0),
                            assetData));
                }

                success = true;
                overwrite = a.checkBoxOverwrite.Checked;
                return AHDRs;
            }
            else
            {
                success = false;
                overwrite = false;
                return null;
            }
        }
    }
}

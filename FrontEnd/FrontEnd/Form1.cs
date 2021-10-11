using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FrontEnd
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            using(CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.InitialDirectory = "C:\\Users";
                dialog.IsFolderPicker = checkBox1.Checked;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    textBox1.Text = dialog.FileName;
                    SelectedFiles.CurrentFile = dialog.FileName;
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (SelectedFiles.CurrentFile == null || SelectedFiles.CurrentFile.Length == 0)
            {
                MessageBox.Show("No se pudo insertar el archivo. Asegurese de haber seleccionado un archivo primero.", "Ruta invalida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (File.Exists(SelectedFiles.CurrentFile))
            {
                SelectedFiles.InsertFile(SelectedFiles.CurrentFile.Split('\\').Last(), SelectedFiles.CurrentFile);
            } else if (Directory.Exists(SelectedFiles.CurrentFile))
            {
                SelectedFiles.InsertDirectory(SelectedFiles.CurrentFile);
            } else
            {
                MessageBox.Show("La ruta de archivo seleccionada no existe.", "Ruta invalida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            listBox1.Items.Clear();
            listBox1.Items.AddRange(SelectedFiles.FileList.Keys.ToArray());
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("No ha seleccionado ningun archivo.", "No hay seleccion.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SelectedFiles.FileList.Remove((string)listBox1.Items[listBox1.SelectedIndex]);
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            SelectedFiles.FileList.Clear();
            listBox1.Items.Clear();
        }
    }
}

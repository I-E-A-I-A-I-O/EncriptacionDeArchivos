using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FrontEnd
{
    public static class SelectedFiles
    {
        static Dictionary<string, string> fileList = new Dictionary<string, string>();
        public static Dictionary<string, string> FileList => fileList;
        public static string CurrentFile { get; set; }

        public static void InsertFile(string name, string path)
        {
            if (FileList.ContainsValue(path) || FileList.ContainsKey(name))
            {
                MessageBox.Show($"El archivo {name} ya se encuentra en la coleccion.", "Archivo insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            FileList.Add(name, path);
        }

        public static void InsertDirectory(string path)
        {
            var files = Directory.GetFiles(path);
            var ignoredFiles = new List<string>();
            if (files.Length == 0)
            {
                MessageBox.Show("La carpeta esta vacia", "Carpeta vacia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (string file in files)
            {
                if (FileList.ContainsValue(file) || FileList.ContainsKey(file.Split('\\').Last()))
                {
                    ignoredFiles.Add(file.Split('\\').Last());
                    continue;
                }
                FileList.Add(file.Split('\\').Last(), file);
            }

            if (ignoredFiles.Count == 0)
            {
                return;
            }

            if (ignoredFiles.Count == files.Length)
            {
                MessageBox.Show("Los archivos de esta carpeta ya estaban contenidos en la coleccion. Fueron ignorados.", "Archivos ignorados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
                MessageBox.Show($"Los archivos {string.Join(", ", ignoredFiles.ToArray())} ya estaban contenidos en la coleccion. Fueron ignorados.", "Archivos ignorados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void EmptyTemp()
        {
            var files = Directory.GetFiles($"{Directory.GetCurrentDirectory()}\\temp");
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
    }
}

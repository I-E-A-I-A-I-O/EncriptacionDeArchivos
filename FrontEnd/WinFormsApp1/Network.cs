using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Forms;
using System.IO;

namespace FrontEnd
{
    class Network
    {
        const string URL = "https://localhost:3000";

        public async static void PostFiles(MainWindow window)
        {
            window.Invoke(new Action(() =>
            {
                window.SetLoadingLabel("Enviando archivos...");
                window.SetLoadingValue(-1);
            }));


            using HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using HttpClient httpClient = new(clientHandler);

            var files = Directory.GetFiles($"{Directory.GetCurrentDirectory()}\\temp");

            try
            {
                MultipartFormDataContent form = new();
                for (var i = 0; i < files.Length; i++)
                {
                    form.Add(new ByteArrayContent(File.ReadAllBytes(files[i])), "files", files[i].Split('\\').Last());
                }
                var post = await httpClient.PostAsync($"{URL}/file", form);
                post.EnsureSuccessStatusCode();
                SelectedFiles.FileList.Clear();
                SelectedFiles.CurrentFile = null;
                window.Invoke(new Action(() =>
                {
                    window.ClearList();
                    window.ClearTextBox();
                    window.HideLoading();
                    window.ToggleControlEnabled();
                }));
                SelectedFiles.EmptyTemp();
                MessageBox.Show("Archivos encriptados y enviados!", "Operacion exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception e)
            {
                window.Invoke(new Action(() =>
                {
                    window.HideLoading();
                    window.ToggleControlEnabled();
                }));
                MessageBox.Show(e.StackTrace, "Peticion fallida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void GetHello()
        {
            Task.Run(async () =>
            {
                using HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using  HttpClient httpClient = new(clientHandler);
                try
                {
                    var response = await httpClient.GetStringAsync($"{URL}/hello");
                    MessageBox.Show(response, "Peticion completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, "Peticion fallida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }
    }
}

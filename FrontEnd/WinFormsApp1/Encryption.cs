using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;

namespace FrontEnd
{
    static class Encryption
    {
        public static void EncryptFiles(MainWindow window)
        {
            var dir = Directory.GetCurrentDirectory();
            window.ShowLoading();
            window.SetLoadingLabel("Encriptando archivos...");
            window.SetLoadingEnd(SelectedFiles.FileList.Keys.Count);
            window.ToggleControlEnabled();
            var task = Task.Run(() =>
            {
                var pem = File.ReadAllText($"{dir}\\utils\\PublicKey.pem");
                using var rsa = new RSACryptoServiceProvider();
                rsa.ImportFromPem(pem);
                for (var i = 0; i < SelectedFiles.FileList.Keys.Count; i++)
                {
                    try
                    {
                        using var aes = Aes.Create();
                        using var aesEncryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                        var path = SelectedFiles.FileList.Values.ElementAt(i);
                        var bytes = File.ReadAllBytes(path);
                        var encryptedData = AESEncrypt(bytes, aesEncryptor);
                        var encryptedAes = rsa.Encrypt(aes.Key, true);
                        File.WriteAllBytes($"{dir}\\temp\\{SelectedFiles.FileList.Keys.ElementAt(i)}.enc", ConcatArrays(encryptedAes, aes.Key, encryptedData));
                        window.Invoke(new Action(() =>
                        {
                            window.SetLoadingValue(i);
                        }));
                    } catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error encriptando", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                window.Invoke(new Action(() =>
                {
                    window.HideLoading();
                }));
                Network.PostFiles(window);
            });
        }

        static byte[] AESEncrypt(byte[] data, ICryptoTransform encryptor)
        {
            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return ms.ToArray();
        }

        static T[] ConcatArrays<T>(params T[][] list)
        {
            var result = new T[list.Sum(a => a.Length)];
            int offset = 0;
            for (int x = 0; x < list.Length; x++)
            {
                list[x].CopyTo(result, offset);
                offset += list[x].Length;
            }
            return result;
        }
    }
}

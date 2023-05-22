using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Security.Cryptography;

namespace DocShare
{
    public partial class downloads : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["folderid"] != null)
            {
                string filename = Request.QueryString["filename"];
                DecryptFile_Click(filename);
            }
        }

        private void DecryptFile_Click(string filePath)
        {
            //Get the Input File Name and Extension
            string fileName = filePath;

            //Build the File Path for the original (input) and the decrypted (output) file
            string input = Server.MapPath("~/Files/") + fileName;
            string output = Server.MapPath("~/temp/") + fileName;

            //Save the Input File, Decrypt it and save the decrypted file in output path.
            //FileUpload1.SaveAs(input);
            this.Decrypt(input, output);
            

            //Download the Decrypted File.
             Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(output));
              Response.WriteFile(output);
              Response.Flush();
              Response.End();
        }

        private void Decrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "CODINGVILA";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                            {
                                fsOutput.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }
    }
}
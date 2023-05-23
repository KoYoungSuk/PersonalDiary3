using FluentFTP;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDiaryUpdater
{
    public class Global
    {
        //This is censored due to security issue.
        //This is censored due to security issue.

        //FTP Server Information

        public String connectionString(String address, String port, String sid, String id, String pw)
        {
            String connstr = String.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));User Id={3};Password={4}", address, port, sid, id, pw);
            return connstr;
        }

        public DialogResult errormessage(String errormsg)
        {
            return MessageBox.Show(errormsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public DialogResult informationmessage(String msg)
        {
            return MessageBox.Show(msg, "PersonalDiary", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        public void UploadFTP(String ftp_address, String ftp_id, String ftp_pw, String FileName, String SafeFileName, String Version)
        {
            //String full_address = ftp_address + "/Upload/PersonalDiary2/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + SafeFileName;

            System.Diagnostics.Debug.WriteLine("ftp_address: " + ftp_address);
            //System.Diagnostics.Debug.WriteLine("full_address: " + full_address);

            System.Diagnostics.Debug.WriteLine("FileName: " + FileName);
            /*
            FtpWebRequest fwq = (FtpWebRequest)WebRequest.Create(full_address);
            fwq.Method = WebRequestMethods.Ftp.UploadFile;

            fwq.Credentials = new NetworkCredential(ftp_id, ftp_pw);

            byte[] filebytes = null;
            using (BinaryReader br = new BinaryReader(File.Open(FileName, FileMode.Open)))
            {
                long dataLength = br.BaseStream.Length;
                filebytes = new byte[dataLength];
                filebytes = br.ReadBytes((int) dataLength);
            };

            fwq.ContentLength = filebytes.LongLength;
            using (Stream requestStream = fwq.GetRequestStream())
            {
                requestStream.Write(filebytes, 0, filebytes.Length);
            };

            using (FtpWebResponse fwr = (FtpWebResponse)fwq.GetResponse())
            {
                informationmessage("Successfully Uploaded."); 
            };

            */

            //FtpClient fc = new FtpClient(ftp_address, ftp_id, ftp_pw);
            FtpClient fc = new FtpClient("ftp://kysot.yspersonal.com", "kys", "gaeun1318hyoam!$"); 
            fc.Connect();
           
            fc.UploadFile(FileName, "/Upload/PersonalDiary2/" + String.Concat(Version.Where(c => !char.IsWhiteSpace(c))) + "/" + SafeFileName , FtpRemoteExists.Overwrite, true); 

            informationmessage("Successfully Uploaded."); 

        }
    }
}

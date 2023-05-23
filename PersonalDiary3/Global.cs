using FluentFTP;
using Microsoft.Win32;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

//SFTP Server: 일기장 파일 업로드 전용(Ubuntu 20.04 LTS 64-bit)

namespace PersonalDiaryUpdater
{
    public class Global
    {
        #region["데이터베이스 연결 정보 스트링(파라미터로 받아와서 리턴)"]
        public String connectionString(String address, String port, String sid, String id, String pw)
        {
            String connstr = String.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));User Id={3};Password={4}", address, port, sid, id, pw);
            return connstr;
        }
        #endregion

        #region["OS 버전 확인"]
        public String checkOS()
        {
            string HKLMWinNTCurrent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            string osName = Registry.GetValue(HKLMWinNTCurrent, "productName", "").ToString();
            string osBuild = Registry.GetValue(HKLMWinNTCurrent, "CurrentBuildNumber", "").ToString();
            String label;
            String[] osName_arr = osName.Split(' ');
            if (osName_arr[1].Equals("10"))
            {
                if (Int32.Parse(osBuild) > 21000)
                {
                    label = "Your Operating System : Windows 11 " + osName_arr[2] + " Build: " + osBuild;
                }
                else
                {
                    label = "Your Operating System : " + osName + " Build: " + osBuild;
                }
            }
            else
            {
                label = "Your Operating System : " + osName + " Build: " + osBuild;
            }
            return label;
        }
        #endregion

        #region["오류 메시지"] 
        public DialogResult errormessage(String errormsg)
        {
            return MessageBox.Show(errormsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region["정보 메시지"] 
        public DialogResult informationmessage(String msg)
        {
            return MessageBox.Show(msg, "PersonalDiary", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }
        #endregion

        /*
        public void UploadFTP(String FileName, String SafeFileName, String Version)
        {
            //.NET 6.0에서는 FluentFTP을 사용해야 함.
            
            //String full_address = ftp_address + "/Upload/PersonalDiary2/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + SafeFileName;

            //System.Diagnostics.Debug.WriteLine("ftp_address: " + ftp_address);
            //System.Diagnostics.Debug.WriteLine("full_address: " + full_address);

            //System.Diagnostics.Debug.WriteLine("FileName: " + FileName);
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


            FtpClient fc = new FtpClient("ftp://kysot.yspersonal.com", "kys", "password"); 
            fc.Connect();
           
            fc.UploadFile(FileName, "/Upload/PersonalDiary2/" + String.Concat(Version.Where(c => !char.IsWhiteSpace(c))) + "/" + SafeFileName , FtpRemoteExists.Overwrite, true); 

            informationmessage("Successfully Uploaded."); 

        }
       */

        #region["SFTP 파일 업로드"]
        public void UploadSFTP(String FileName, String SafeFileName) //FileName: 전체경로, SafeFileName: 부분경로 
        {
            StreamReader sr = new StreamReader("sftp.txt"); //텍스트파일에서 SFTP 정보를 불러온다. 

            String full = sr.ReadToEnd();

            String sftp_address = full.Split(',')[0];
            
            String sftp_id = full.Split(',')[1];
            String sftp_pw = full.Split(',')[2];
            
            sftp_address = sftp_address.Trim();
            sftp_id = sftp_id.Trim();
            sftp_pw = sftp_pw.Trim(); 

            SftpClient sc = new SftpClient(sftp_address, sftp_id, sftp_pw); 

            FileStream fs = new FileStream(FileName, FileMode.Open);  
            sc.Connect();

            sc.UploadFile(fs, "/mnt/hdd3/Secret Documents/Self-Criticism/Before 2020-07/" + SafeFileName); //SFTP 업로드 

            sc.Disconnect(); 
        }
        #endregion

        #region["SFTP 파일 삭제"] 
        //일기장 데이터베이스에서 삭제할때 파일도 같이 삭제 
        public void DeleteSFTP(String SafeFileName)
        {
            StreamReader sr = new StreamReader("sftp.txt");

            String full = sr.ReadToEnd();

            String sftp_address = full.Split(',')[0];

            String sftp_id = full.Split(',')[1];
            String sftp_pw = full.Split(',')[2];

            sftp_address = sftp_address.Trim();
            sftp_id = sftp_id.Trim();
            sftp_pw = sftp_pw.Trim();

            System.Diagnostics.Debug.WriteLine("sftp_pw: " + sftp_pw);

            SftpClient sc = new SftpClient(sftp_address, sftp_id, sftp_pw);

            //FileStream fs = new FileStream(FileName, FileMode.Open);
            sc.Connect();

            sc.DeleteFile("/mnt/hdd3/Secret Documents/Self-Criticism/Before 2020-07/" + SafeFileName);

            sc.Disconnect();
        }
        #endregion  
    }
}

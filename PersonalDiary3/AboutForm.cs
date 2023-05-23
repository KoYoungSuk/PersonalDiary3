using FluentFTP;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//업데이트 관련 코드는 일단 제거함 (2023-05-22) 

namespace PersonalDiaryUpdater
{
    public partial class AboutForm : Form
    {
        Global g = new Global();
        /*
        OracleConnection conn = null;
        Boolean updatecheck = false;
        Boolean errorcheck = false; 
        */
        public AboutForm(OracleConnection conn)
        {
            InitializeComponent();
            //this.conn = conn;
            //checkUpdate();  //Checking Update 
            //button3.Enabled = updatecheck; 
        }

        /*
        private void checkUpdate()
        {
            try
            {
                DiaryDAO diarydao = new DiaryDAO(conn);

                string maxver = diarydao.GetMaxVer();
                maxver = maxver.Split(' ')[1];

                double doumaxver = Convert.ToDouble(maxver);

                if (doumaxver > 3.65) //Current Version: 3.65( 프로그램 코드 수정할때마다 여기 있는 버전을 바꿔줘야함) 
                {
                    label4.Text = "You can update to Version " + doumaxver;
                    updatecheck = true;
                }
            }
            catch (Exception ex)
            {
                label4.Text = ex.Message;
                errorcheck = true;
            }
        }
        */

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
            try
            {
                if(!errorcheck)
                {
                    if (updatecheck)
                    {
                         DiaryDAO diarydao = new DiaryDAO(conn);

                         string maxver = diarydao.GetMaxVer(); //최신 버전 찾기 

                         FtpClient fc = new FtpClient("ftp://kysot.yspersonal.com", "kys", "password");

                         fc.Connect();

                         DirectoryInfo di = new DirectoryInfo("C:\\Temp");

                         if (!di.Exists)
                         {
                             di.Create();
                         }
                         fc.DownloadFile("C:\\Temp\\personaldiary.zip", "//Upload/PersonalDiary2/" + String.Concat(maxver.Where(c => !char.IsWhiteSpace(c))) + "/net6.0-windows.zip", FtpLocalExists.Overwrite);

                         g.informationmessage("Program must be Closed.");

                         Process.Start("PersonalDiaryUpdate.exe"); //업데이트 프로그램 실행 
                         Application.Exit();
                    }
               }
               else
               {
                    DialogResult dr = g.informationmessage("Update Server Error. You can update manually. continue?");
                    if (dr == DialogResult.OK)
                    {   
                         //수동 업데이트 모드(Manual Update Mode) 
                         if (File.Exists("C:\\Temp\\personaldiary.zip"))
                         {
                             Process.Start("PersonalDiaryUpdate.exe");
                         }
                         else
                         {
                             g.informationmessage("You can download zip file from this website.");
                             Process.Start(new ProcessStartInfo("http://kysot.yspersonal.com:81/update/personaldiary2/") { UseShellExecute = true });
                             //다운로드 링크(구현예정) 
                         }
                    }
               }
            }
            catch (Exception ex)
            {
                g.errormessage(ex.Message);
            }
            */
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {

        }
    }
}

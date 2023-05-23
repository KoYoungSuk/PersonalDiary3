using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalDiaryUpdater
{
    public partial class FTPSettingForm : Form
    {
        Global g = new Global();
        public FTPSettingForm()
        {
            InitializeComponent();
            if (File.Exists("sftp.txt"))
            {
                button3.Enabled = true; 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //메모리 관리 (가비지 컬렉션) 
            GC.Collect();
            GC.WaitForPendingFinalizers();

            String ftpaddress = textBox1.Text;
            String ftpid = textBox2.Text;
            String ftppw = textBox3.Text;

            try
            {   //SFTP 주소, 아이디, 비밀번호를 텍스트 문서로 저장 
                StreamWriter sw = File.CreateText("sftp.txt");
                sw.WriteLine(ftpaddress + "," + ftpid + "," + ftppw);
                sw.Close();
                g.informationmessage("Saved.");
                this.Hide(); 
            }
            catch (Exception ex)
            {
                g.errormessage(ex.Message); 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(File.Exists("sftp.txt"))
            { 
                //메모리 관리 (가비지 컬렉션) 
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //SFTP 주소, 아이디, 비밀번호를 저장한 텍스트 문서 파일 삭제 
                File.Delete("sftp.txt");
                g.informationmessage("Successfully deleted."); 
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick(); 
            }
        }

        private void FTPSettingForm_Load(object sender, EventArgs e)
        {

        }
    }
}

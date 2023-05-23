using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PersonalDiaryUpdater
{
    public partial class MainForm : Form
    {
        Global g = new Global();
        OracleConnection conn = null;
        //Boolean updatecheck = false; 
        public MainForm(OracleConnection conn)
        {
            InitializeComponent();
            this.conn = conn;
            label7.Text = g.checkOS(); 
            label6.Text = "Success Connect to DataBase, Everything are good. at [ " + DateTime.Now.ToString() + " ] ";
            getDiary(false); //일기장 목록 로드 
        }

        public void getLabel()
        {
            try
            {
                DiaryDAO diarydao = new DiaryDAO(conn);
                int number = diarydao.getDiaryCount(); //일기장 목록 개수 가져오기 
                label4.Text = "NUMBER: " + number;
            }catch(Exception ex)
            {
                g.errormessage(ex.Message);
            }
        }
        public void getDiary(Boolean desc) //desc: 정렬순서 정하는거 
        {
            try
            {
                DiaryDAO diarydao = new DiaryDAO(conn);
                DataTable dt = diarydao.getDiaryList2(desc); 
                dt.Columns.RemoveAt(1); 
                dataGridView1.DataSource = dt; //그리드뷰에 집어넣기 
                getLabel(); 
            }catch(Exception ex)
            {
                g.errormessage(ex.Message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            conn.Close();
            if (File.Exists("sftp.txt"))
            {
                //SFTP 로그인 정보를 삭제할껀지 물어봄 
                DialogResult dr = g.informationmessage("Do you want to delete sftp setting?");
                if (dr == DialogResult.OK) //삭제한다고 하면 삭제함 
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete("sftp.txt");
                    g.informationmessage("Successfully deleted.");
                }
            }
            Application.Exit();
        }

        private void exitXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn.Close();
            if (File.Exists("sftp.txt"))
            {
               //SFTP 로그인 정보를 삭제할껀지 물어봄 
               DialogResult dr = g.informationmessage("Do you want to delete sftp setting?");
               if (dr == DialogResult.OK) //삭제한다고 하면 삭제함 
               {
                   GC.Collect();
                   GC.WaitForPendingFinalizers();
                   File.Delete("sftp.txt");
                   g.informationmessage("Successfully deleted.");
               }
              
            }
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label8.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            getDiary(false);
        }

        private void descendDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getDiary(true);
        }

        private void ascendAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getDiary(false);
        }

        private void clearDiaryCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }

        private void updateDiaryUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getDiary(false);
        }

        private void homePageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://home.yspersonal.com") { UseShellExecute = true });
        }

        private void aboutPersonalDiaryIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm(conn); 
            af.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DetailForm df = new DetailForm(null, true, conn); //새로 작성하는 모드 
            df.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String title = textBox1.Text;
            DetailForm df = new DetailForm(title, true, conn); //선택한 셀의 제목에 맞춰서 상세 일기장 내용으로 이동 (수정) 
            df.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                String title = textBox1.Text;
                DiaryDAO diarydao = new DiaryDAO(conn);
                int result = diarydao.DeleteDiary(title);
                if(result == 1)
                {
                    g.informationmessage("Success Deleted.");
                    g.DeleteSFTP(title + ".txt"); //TXT FILE ONLY. //SFTP 서버에 있는 일기장 파일 삭제 
                    getDiary(false); //일기장 목록 다시 로드 
                }
                else
                {
                    g.errormessage("Unknown Error Message");
                }
            }catch(Exception ex)
            {
                g.errormessage(ex.Message);
            }
        }

        #region["텍스트상자에서 엔터키를 누르면 그 제목에 맞는 상세 일기장 내용으로 이동"] 
        private void text_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                String title = textBox1.Text;
                DetailForm df = new DetailForm(title, false, conn);
                df.Show();
            }
        }
        #endregion

        #region["셀을 한번 클릭하면 텍스트상자에 제목 출력"] 
        private void GridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
        }
        #endregion

        #region("셀을 한번 더 출력하면 출력한 제목에 따라서 상세 일기장 내용으로 이동"] 
        private void GridView_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            String title = textBox1.Text;
            DetailForm df = new DetailForm(title, false, conn);
            df.Show();
        }
        #endregion

        private void blogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://blog.naver.com/vheh5678") { UseShellExecute = true });
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            if (File.Exists("sftp.txt")) //SFTP 로그인 정보가 저장되어 있는 경우 삭제할껀지 물어봄. 
            {
                DialogResult dr = g.informationmessage("Do you want to delete sftp setting?");
                if (dr == DialogResult.OK) 
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete("sftp.txt"); //삭제하겠다고 하면 삭제함. 
                    g.informationmessage("Successfully deleted.");
                }
                else
                {
                    Application.Exit(); 
                }
            }
            Application.Exit();
        }

        private void uploadProgramToolStripMenuItem_Click(object sender, EventArgs e){ }

        private void fTPServerSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FTPSettingForm fsf = new FTPSettingForm();
            fsf.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}

using Oracle.ManagedDataAccess.Client;
using Renci.SshNet;
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
    public partial class DetailForm : Form
    {
        //제목이 Null: 작성 모드
        //제목이 Null이 아님: 수정/읽기 모드(modify boolean값 파라미터로 구분)

        String title;
        Global g = new Global();
        OracleConnection conn = null;
        String g_FileName = string.Empty;
        String g_SafeFileName = string.Empty; 
        public DetailForm(String title, Boolean modify, OracleConnection conn)
        {
            InitializeComponent();
            this.conn = conn;
            if (title != null) //제목이 존재할때(읽기/수정 모드) 
            {
                this.title = title;
                getDiary();
            }
            else
            {   //작성 모드 
                this.title = String.Empty;
                textBox1.ReadOnly = false; 
                button1.Text = "Write";
                button3.Enabled = false;
                groupBox1.Visible = false; 
            }
            if (modify) //작성/수정 모드
            {
                radioButton2.Checked = true;
            }
            else //읽기  모드 
            {
                radioButton1.Checked = true;
            }
        }

        #region["일기장 정보 가져오기"] 
        public void getDiary()
        {
            try
            {
                DiaryDAO diarydao = new DiaryDAO(conn);
                SortedList<String, String> diarylist = diarydao.getDiaryListByTitle(title);
                if (diarylist != null)
                {
                    textBox1.Text = diarylist["title"];
                    textBox2.Text = diarylist["context"];
                    textBox3.Text = diarylist["savedate"];
                    textBox4.Text = diarylist["modifydate"];
                }
            }
            catch (Exception ex)
            {
                g.errormessage(ex.Message);
            }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        #region["일기장 정보 삭제"] 
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                String title = textBox1.Text; 
                DiaryDAO diarydao = new DiaryDAO(conn);
                int result = diarydao.DeleteDiary(title);
                if(result == 1)
                {
                    g.informationmessage("Successfully Deleted.");
                    this.Hide();
                }
                else
                {
                    g.errormessage("Unknown Error Message");
                }
            }
            catch (Exception ex)
            { 
               g.errormessage(ex.Message);
            }
        }
        #endregion

        #region["읽기 모드"] 
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.ReadOnly = true;
            button1.Enabled = false;
        }
        #endregion

        #region["작성/수정 모드"] 
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.ReadOnly = false;
            button1.Enabled = true;
        }
        #endregion


        private void button1_Click(object sender, EventArgs e)
        {
            String title = textBox1.Text;
            String content = textBox2.Text;
            int result = 0;
            DiaryDTO diarydto = null; 
            DiaryDAO diarydao = new DiaryDAO(conn);
            try
            {
                if(button1.Text.Equals("Write")) //작성 모드일때 
                {
                    diarydto = new DiaryDTO(title, content, DateTime.Now.ToString(), null); 
                    result = diarydao.insertDiary(diarydto); 
                }
                else  //수정 모드일때 
                {
                    diarydto = new DiaryDTO(title, content, null, DateTime.Now.ToString()); 
                    result = diarydao.UpdateDiary(diarydto);
                }
               
                if (result == 1)
                {
                    if(button1.Text.Equals("Write")) //작성 모드일때 
                    {
                        g.informationmessage("Success Writed.");
                    }
                    else //수정 모드일때 
                    {
                        g.informationmessage("Success Modified.");
                    }
                   
                    this.Hide();

                    String tempdir = "C:\\Temp\\PersonalDiary_TempDoc"; //임시 파일저장경로 

                    DirectoryInfo di = new DirectoryInfo(tempdir);
                    if (!di.Exists) 
                    {
                        di.Create(); //경로가 존재하지 않으면 생성 
                    }
                    g_FileName = tempdir + "\\" + title + ".txt";
                    g_SafeFileName = title + ".txt";
                    if (!File.Exists(g_FileName))
                    { 
                        //임시 파일 저장 
                        StreamWriter sw = File.CreateText(g_FileName);
                        sw.Write(textBox2.Text);
                        sw.Flush();
                        sw.Close();
                    }

                    //SFTP 서버에 업로드 
                    g.UploadSFTP(g_FileName, g_SafeFileName);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete(g_FileName); //임시 파일 삭제 
                    //Directory.Delete(tempdir); 
                }
                else
                {
                    g.errormessage("Unknown Error Message");
                }
            }catch(Exception ex)
            {
                g.errormessage(ex.Message);
                if(result == 1)
                {
                    diarydao.DeleteDiary(title); 
                }
            }
        }

        private void saveAsDocumentSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Text Document(*.txt)|*.txt|Diary Document(*.diary)|*.diary|All Files(*.*)|*.*";
            saveFileDialog1.Title = "Save as Text Document";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.FileName = textBox1.Text; 
            StreamWriter sw = null;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sw = File.CreateText(saveFileDialog1.FileName);
                    sw.Write(textBox2.Text);
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception ex)
                {
                    g.errormessage(ex.Message);
                }
            }
        }

        private void aboutPersonalDiaryIIAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm(conn);
            af.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "Text Document(*.txt)|*.txt|Diary Document(*.diary)|*.diary|All Files(*.*)|*.*";
                openFileDialog1.Title = "Open Text Document";
                openFileDialog1.FileName = "";
                StreamReader sr = null;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    String[] newfilenamearr = openFileDialog1.SafeFileName.Split('.');
                    String newfilename = newfilenamearr[0];
                    sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                    textBox1.Text = newfilename;
                    textBox2.Text = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                g.errormessage(ex.Message);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e){}
        private void label3_Click(object sender, EventArgs e){}
        private void DetailForm_Load(object sender, EventArgs e){}
    }
}

using Oracle.ManagedDataAccess.Client;

namespace PersonalDiaryUpdater
{
    public partial class Form1 : Form
    {
        Global g = new Global();
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //�����ͺ��̽� �������� �������� 
                String fulladdress = textBox1.Text;
                string dbid = textBox2.Text;
                string dbpw = textBox3.Text;
                string address = fulladdress.Split(':')[0];
                string sidandport = fulladdress.Split(':')[1];
                string port = sidandport.Split('/')[0];
                string sid = sidandport.Split('/')[1];


                String sql = "CREATE TABLE DIARY ( TITLE VARCHAR2(100) NOT NULL PRIMARY KEY, CONTEXT CLOB, SAVEDATE TIMESTAMP, MODIFYDATE TIMESTAMP )"; //���̺� ���� SQL 
                String connstr = g.connectionString(address, port, sid, dbid, dbpw); //���� ��Ʈ���� ����ֱ� 
                OracleConnection conn = new OracleConnection(connstr);
                conn.Open();

                DiaryDAO diarydao = new DiaryDAO(conn);

                Boolean existstatus = diarydao.getTableExists("DIARY");  //���̺� ���� �˻�

                if (existstatus) //���̺��� �����ϸ� �̵��ϱ� 
                {
                    MainForm mf = new MainForm(conn);
                    mf.Show();
                    this.Hide();
                }
                else //�������� ������ ���̺� ����� 
                {
                    DialogResult dr = g.informationmessage("You need to create table. Continue?");

                    if(dr == DialogResult.OK)
                    {
                        int result = diarydao.createTable(sql);
                        if(result != 0)
                        {
                            //���̺� ����� �̵��ϱ� 
                            g.informationmessage("Table is successfully created.");
                            MainForm mf = new MainForm(conn);
                            mf.Show();
                            this.Hide();
                        }
                        else
                        {
                            g.errormessage("Unknown Error Message"); 
                        }
                    }
                    else
                    {
                        g.errormessage("You need to create Table.");
                    }
                }

            }catch(Exception ex)
            {
                g.errormessage(ex.Message);
            }
        }

        private void text_KeyDown(object sender, KeyEventArgs e)
        {
         
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            //��й�ȣ�� �Է��ϰ� Enter Ű�� ������ �α���/���̺� ���� ���� 
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void Form1_Load(object sender, EventArgs e) {}
        private void label1_Click(object sender, EventArgs e){}
    }
}
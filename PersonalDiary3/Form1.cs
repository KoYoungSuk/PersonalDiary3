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
                //데이터베이스 연결정보 가져오기 
                String fulladdress = textBox1.Text;
                string dbid = textBox2.Text;
                string dbpw = textBox3.Text;
                string address = fulladdress.Split(':')[0];
                string sidandport = fulladdress.Split(':')[1];
                string port = sidandport.Split('/')[0];
                string sid = sidandport.Split('/')[1];


                String sql = "CREATE TABLE DIARY ( TITLE VARCHAR2(100) NOT NULL PRIMARY KEY, CONTEXT CLOB, SAVEDATE TIMESTAMP, MODIFYDATE TIMESTAMP )"; //테이블 생성 SQL 
                String connstr = g.connectionString(address, port, sid, dbid, dbpw); //연결 스트링에 집어넣기 
                OracleConnection conn = new OracleConnection(connstr);
                conn.Open();

                DiaryDAO diarydao = new DiaryDAO(conn);

                Boolean existstatus = diarydao.getTableExists("DIARY");  //테이블 존재 검사

                if (existstatus) //테이블이 존재하면 이동하기 
                {
                    MainForm mf = new MainForm(conn);
                    mf.Show();
                    this.Hide();
                }
                else //존재하지 않으면 테이블 만들기 
                {
                    DialogResult dr = g.informationmessage("You need to create table. Continue?");

                    if(dr == DialogResult.OK)
                    {
                        int result = diarydao.createTable(sql);
                        if(result != 0)
                        {
                            //테이블 만들고 이동하기 
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
            //비밀번호를 입력하고 Enter 키를 쳤을때 로그인/테이블 생성 진행 
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void Form1_Load(object sender, EventArgs e) {}
        private void label1_Click(object sender, EventArgs e){}
    }
}
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDiaryUpdater
{
    public class MemberDAO
    {
        OracleConnection? conn;
        Global g = new Global();
        String db_url;
        String db_port;
        String db_sid;
        String db_id;
        String db_pw;

        public MemberDAO(string db_url, string db_port, string db_sid, string db_id, string db_pw)
        {
            this.db_url = db_url;
            this.db_port = db_port;
            this.db_sid = db_sid;
            this.db_id = db_id;
            this.db_pw = db_pw;
        }

        public void connectDB()
        {
            String connstr = g.connectionString(db_url, db_port, db_sid, db_id, db_pw);
            conn = new OracleConnection(connstr);
            conn.Open();
        }

        public void disconnectDB()
        {
            if (conn != null)
            {
                conn.Close();
                conn = null;
            }
        }

        public Boolean Login(String password)
        {
            connectDB();
            String sql = "select password from member where id = 'admin'";
            Boolean loginstatus = false;
            OracleCommand scmd = new OracleCommand(sql, conn);
            OracleDataReader dr = scmd.ExecuteReader();
            String password_db = "";
            if(dr.Read())
            {
                password_db = dr["password"].ToString();
            }
            if (BCrypt.Net.BCrypt.Verify(password, password_db)){
                loginstatus = true;
            }
            dr.Close();
            return loginstatus;
        }
    }
}

using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDiaryUpdater
{
    public class DiaryDAO
    {
        OracleConnection? conn;
        Global g = new Global();
        /*
        String db_url;
        String db_port;
        String db_sid;
        String db_id;
        String db_pw;
        */
        public DiaryDAO(OracleConnection conn)
        {
            this.conn = conn;
        }

        public Boolean getTableExists(String tname)
        {
            Boolean existstatus = false;
            String sql = "select TNAME from TAB where TNAME = :tname"; 
            OracleCommand scmd = new OracleCommand(sql, conn);
            scmd.BindByName = true;
            scmd.Parameters.Add(new OracleParameter("tname", tname));
            OracleDataReader dr = scmd.ExecuteReader();
            string status = null;
            while(dr.Read())
            {
                status = dr["TNAME"].ToString();
            }
            if(string.IsNullOrEmpty(status))
            {
                existstatus = false;
            }
            else
            {
                existstatus = true;
            }
            dr.Close();
            scmd.Dispose();
            return existstatus;
        }

        public int createTable(String sql)
        {
            OracleCommand ccmd = new OracleCommand(sql, conn);
            int result = ccmd.ExecuteNonQuery();
            ccmd.Dispose();
            return result; 
        }
        

        public int insertError()
        {
            String sql = "insert into errorlist (title, context, savedate, modifydate) values (:title, :context, :savedate, :modifydate)";
            OracleCommand icmd = new OracleCommand(sql, conn);
            icmd.BindByName = true;
            /*
            icmd.Parameters.Add(new OracleParameter("title", diarydto.Title));
            icmd.Parameters.Add(new OracleParameter("context", diarydto.Content));
            icmd.Parameters.Add(new OracleParameter("savedate", DateTime.Parse(diarydto.Savedate)));
            icmd.Parameters.Add(new OracleParameter("modifydate", diarydto.Modifydate));
            */
            int result = icmd.ExecuteNonQuery();
            icmd.Dispose();
            return result;
        }

        public int uploadVer(String versionid, String filename, String time)
        {
            String sql = String.Empty;

            //테이블이 존재하지 않을때 
            if(!getTableExists("VERSION"))
            {
                DialogResult dr =  g.informationmessage("Do you want to create Table?");

                if(dr == DialogResult.OK)
                {
                    sql = "CREATE TABLE VERSION ( NUM VARCHAR2(50) NOT NULL PRIMARY KEY, PNAME VARCHAR2(100), FNAME VARCHAR2(100), SAVEDATE TIMESTAMP )";
                    createTable(sql);
                }
                else
                {
                    g.informationmessage("You need to create Table.");
                    return -1; 
                }
            }
            
            sql = "insert into version (num, pname, fname, savedate) values (:num, :pname, :fname, :savedate)";
            OracleCommand icmd = new OracleCommand(sql, conn);
            icmd.BindByName = true;
            icmd.Parameters.Add(new OracleParameter("num", versionid));
            icmd.Parameters.Add(new OracleParameter("pname", "PersonalDiary3"));
            icmd.Parameters.Add(new OracleParameter("fname", filename));
            icmd.Parameters.Add(new OracleParameter("savedate", DateTime.Parse(time)));
            int result = icmd.ExecuteNonQuery();
            icmd.Dispose();
            return result;
        }

        public DataTable getVerList()
        {
            String sql = String.Empty;

            //테이블이 존재하지 않을때 
            if (!getTableExists("VERSION"))
            {
                DialogResult dr = g.informationmessage("Do you want to create Table?");

                if (dr == DialogResult.OK)
                {
                    sql = "CREATE TABLE VERSION ( NUM VARCHAR2(50) NOT NULL PRIMARY KEY, PNAME VARCHAR2(100), FNAME VARCHAR2(100), SAVEDATE TIMESTAMP )";
                    createTable(sql);
                }
                else
                {
                    g.informationmessage("You need to create Table.");
                    return null; 
                }
            }

            DataTable dt = new DataTable();

            sql = "select * from version order by num";
            OracleCommand scmd = new OracleCommand(sql, conn);
            OracleDataAdapter oda = new OracleDataAdapter(scmd);
            oda.Fill(dt);
            scmd.Dispose();
            return dt; 
        }


        public String GetMaxVer()
        {
            String sql = String.Empty;

            //테이블이 존재하지 않을때 
            if (!getTableExists("VERSION"))
            {
                DialogResult dr = g.informationmessage("Do you want to create Table?");

                if (dr == DialogResult.OK)
                {
                    sql = "CREATE TABLE VERSION ( NUM VARCHAR2(50) NOT NULL PRIMARY KEY, PNAME VARCHAR2(100), FNAME VARCHAR2(100), SAVEDATE TIMESTAMP )";
                    createTable(sql);
                }
                else
                {
                    g.informationmessage("You need to create Table.");
                    return null;
                }
            }

            sql = "select max(num) as maxver from version order by num"; 

            OracleCommand scmd = new OracleCommand(sql, conn); 
            OracleDataReader odr = scmd.ExecuteReader();
            string versionid = string.Empty;

            while (odr.Read())
            {
                versionid = odr["maxver"].ToString();
            }

            odr.Close(); 
            scmd.Dispose();
            return versionid;
        }
    }

}


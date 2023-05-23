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

        public DiaryDAO(OracleConnection conn)
        {
            this.conn = conn;
        }



        public int insertDiary(DiaryDTO diarydto)
        {
            String sql = "insert into diary (title, context, savedate, modifydate) values (:title, :context, :savedate, :modifydate)";
            OracleCommand icmd = new OracleCommand(sql, conn);
            icmd.BindByName = true;
            icmd.Parameters.Add(new OracleParameter("title", diarydto.Title));
            icmd.Parameters.Add(new OracleParameter("context", diarydto.Content));
            icmd.Parameters.Add(new OracleParameter("savedate", DateTime.Parse(diarydto.Savedate)));
            icmd.Parameters.Add(new OracleParameter("modifydate", diarydto.Modifydate));
            int result = icmd.ExecuteNonQuery();
            icmd.Dispose();
            return result;
        }

        public int UpdateDiary(DiaryDTO diarydto)
        {
            String sql = "update diary set context=:context, modifydate=:modifydate where title = :title";
            OracleCommand ucmd = new OracleCommand(sql, conn);
            ucmd.BindByName = true;
            ucmd.Parameters.Add(new OracleParameter("context", diarydto.Content));
            ucmd.Parameters.Add(new OracleParameter("modifydate", DateTime.Parse(diarydto.Modifydate)));
            ucmd.Parameters.Add(new OracleParameter("title", diarydto.Title));
            int result = ucmd.ExecuteNonQuery();
            ucmd.Dispose(); 
            return result;
        }

        public int DeleteDiary(String title)
        {
            String sql = "delete from diary where title = :title";
            OracleCommand dcmd = new OracleCommand(sql, conn);
            dcmd.BindByName = true;
            dcmd.Parameters.Add(new OracleParameter("title", title));
            int result = dcmd.ExecuteNonQuery();
            dcmd.Dispose();
            return result;
        }

        public SortedList<String, String> getDiaryListByTitle(String title)
        {
            SortedList<String, String> diarylist = new SortedList<String, String>();
            String sql = "select * from diary where title = :title";
            OracleCommand scmd = new OracleCommand(sql, conn);
            scmd.BindByName = true;
            scmd.Parameters.Add(new OracleParameter("title", title));
            OracleDataReader dr = scmd.ExecuteReader();
            while (dr.Read())
            {
                diarylist.Add("title", dr["title"].ToString());
                diarylist.Add("context", dr["context"].ToString());
                diarylist.Add("savedate", dr["savedate"].ToString());
                diarylist.Add("modifydate", dr["modifydate"].ToString());
            }
            dr.Close();
            scmd.Dispose(); 
            return diarylist;
        }


        public DataTable getDiaryList2(Boolean desc)
        {
            DataTable dt = new DataTable();
            String sql = null;
            if (desc)
            {
                sql = "select * from diary order by title desc";
            }
            else
            {
                sql = "select * from diary order by title";
            }
            OracleCommand scmd = new OracleCommand(sql, conn);
            OracleDataAdapter oda = new OracleDataAdapter();
            oda.SelectCommand = scmd;
            oda.Fill(dt);
            oda.Dispose();
            scmd.Dispose(); 
            return dt;
        }


        public List<DiaryDTO> getDiaryList(Boolean desc)
        {
            List<DiaryDTO> diarylist = new List<DiaryDTO>();
            String sql = null;
            if (desc)
            {
                sql = "select * from diary order by title desc";
            }
            else
            {
                sql = "select * from diary order by title";
            }
            OracleCommand scmd = new OracleCommand(sql, conn);
            OracleDataReader dr = scmd.ExecuteReader();
            while (dr.Read())
            {
                DiaryDTO diarydto = new DiaryDTO();
                diarydto.Title = dr["title"].ToString();
                diarydto.Content = dr["context"].ToString();
                diarydto.Savedate = dr["savedate"].ToString();
                diarydto.Modifydate = dr["modifydate"].ToString();
                diarylist.Add(diarydto);
            }
            dr.Close();
            scmd.Dispose(); 
            return diarylist;
        }

        #region["일기장 개수 가져오기"] 
        public int getDiaryCount()
        {
            int diarynumber = 0;
            String sql = "select count(*) diarynumber from diary";
            OracleCommand scmd = new OracleCommand(sql, conn);
            OracleDataReader dr = scmd.ExecuteReader();
            while (dr.Read())
            {
                diarynumber = Int32.Parse(dr["diarynumber"].ToString());
            }
            dr.Close();
            scmd.Dispose();
            return diarynumber;
        }
        #endregion


        #region["테이블 존재 유무 검사"]
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
        #endregion

        #region["테이블 생성"]
        public int createTable(String sql)
        {
            OracleCommand ccmd = new OracleCommand(sql, conn);
            int result = ccmd.ExecuteNonQuery();
            ccmd.Dispose();
            return result; 
        }
        #endregion

        /*
        public int insertError()
        {
            String sql = "insert into errorlist (logno, ltime, comnm, os, errcontent, exenm) values (:logno, :ltime, :comnm, :os, :errcontent, :exenm)";
            OracleCommand icmd = new OracleCommand(sql, conn);
            icmd.BindByName = true;

            
            icmd.Parameters.Add(new OracleParameter("title", diarydto.Title));
            icmd.Parameters.Add(new OracleParameter("context", diarydto.Content));
            icmd.Parameters.Add(new OracleParameter("savedate", DateTime.Parse(diarydto.Savedate)));
            icmd.Parameters.Add(new OracleParameter("modifydate", diarydto.Modifydate));
            



            int result = icmd.ExecuteNonQuery();
            icmd.Dispose();
            return result;
        }
        */

        /*
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
            //Default Update Server Information 
            String dbaddress = "koyoungsuk2.dyndns.org";
            string dbid = "kys";
            string dbpw = "password";
            string dbport = "1521";
            string dbsid = "XE";
            //Password must be censored. 

            String connstr = g.connectionString(dbaddress, dbport, dbsid, dbid, dbpw); 
            OracleConnection conn2 = new OracleConnection(connstr);
            
            conn2.Open();


            String sql = String.Empty;

            sql = "select max(num) as maxver from version where pname = 'PersonalDiary2' order by num"; 

            OracleCommand scmd = new OracleCommand(sql, conn2); 
            OracleDataReader odr = scmd.ExecuteReader();
            string versionid = string.Empty;

            while (odr.Read())
            {
                versionid = odr["maxver"].ToString();
            }

            odr.Close(); 
            scmd.Dispose();
            conn2.Close(); 
            return versionid;
        }
        */
    }

}


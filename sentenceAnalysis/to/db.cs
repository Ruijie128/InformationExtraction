using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;



namespace sentenceAnalysis.to
{
    public class db
    {
        public static string connStr = "Database=websitedata;Data Source=127.0.0.1;User Id=root;Password=zhangruijie;pooling=false;CharSet=utf8;port=3306";
        public static MySqlConnection npgConnection = new MySqlConnection(connStr);


        // extract data from websitedata
        //public static void ExcuteData(List<string> content, List<string> uri, List<string> publishtime)
        //{
        //  //  string content = null;
        //    // string sqlStr = "select uri,Content, publishtime   FROM data_sina where uri = (select docuri from targetinfo where id = 138)";
        //    string uriSql = "select docuri from targetinfo order by docuri"; ;
        //    MySqlCommand uriComm = npgConnection.CreateCommand();
        //    uriComm.CommandText = uriSql;
        //    using (MySqlDataAdapter adapter = new MySqlDataAdapter(uriComm))
        //    {
        //        DataTable dt = new DataTable();
        //        adapter.Fill(dt);
        //        foreach (DataRow row in dt.Rows) 
        //        {
        //            string docuri = Convert.ToString(row["docuri"]);
        //            uri.Add(docuri);
        //            string sqlStr = "select Content, publishtime  FROM wxg_data_sina where uri ='" + docuri + "' order by Content";
        //            MySqlCommand cmd = npgConnection.CreateCommand();
        //            cmd.CommandText = sqlStr;
        //            npgConnection.Open();
        //            MySqlDataReader sqlReader = cmd.ExecuteReader();
        //            while (sqlReader.Read())
        //            {
        //               //  uri = sqlReader[0].ToString();
        //                 content.Add( sqlReader[0].ToString());
        //                 publishtime.Add( sqlReader[1].ToString());
        //            }
        //            npgConnection.Close();
        //        }
        //    }

        // //   return content;

        //}

        public static void ExcuteData(List<string> content, List<string> uri, List<string> publishtime)
        {
            //  string content = null;
            //string sqlStr = "select uri,Content, publishtime   FROM data_sina where uri = (select docuri from targetinfo where id = 138)";
            string sqlStr = "SELECT Text, PublishTime, Source FROM gazetteer.ts_spatial_data_origin  where TimeFlag = 1";
            mysqlConn.Open();
            MySqlCommand comm = new MySqlCommand(sqlStr, mysqlConn);
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                if (reader[0].ToString() != null && reader[0].ToString() != "")
                {
                    content.Add(reader[0].ToString());
                    publishtime.Add(reader[1].ToString());
                    uri.Add(reader[2].ToString());
                }
            }
            mysqlConn.Close();

        }
        //public static void ExcuteData(List<string> content, List<string> uri, List<string> publishtime)
        //{
        //    //  string content = null;
        //    //string sqlStr = "select uri,Content, publishtime   FROM data_sina where uri = (select docuri from targetinfo where id = 138)";
        //    string uriSql = "select docuri from targetinfo order by docuri"; ;
        //    MySqlCommand uriComm = npgConnection.CreateCommand();
        //    uriComm.CommandText = uriSql;
        //    using (MySqlDataAdapter adapter = new MySqlDataAdapter(uriComm))
        //    {
        //        DataTable dt = new DataTable();
        //        adapter.Fill(dt);
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            string docuri = Convert.ToString(row["docuri"]);
        //            uri.Add(docuri);
        //            string sqlStr = "select Content, publishtime  FROM wxg_data_sina where uri ='" + docuri + "' order by Content";
        //            MySqlCommand cmd = npgConnection.CreateCommand();
        //            cmd.CommandText = sqlStr;
        //            npgConnection.Open();
        //            MySqlDataReader sqlReader = cmd.ExecuteReader();
        //            while (sqlReader.Read())
        //            {
        //                //  uri = sqlReader[0].ToString();
        //                content.Add(sqlReader[0].ToString());
        //                publishtime.Add(sqlReader[1].ToString());
        //            }
        //            npgConnection.Close();
        //        }
        //    }

        //    //   return content;

        //}

        public static string connMySqlStr = "Database=gazetteer;Data Source=127.0.0.1;User Id=root;Password=zhangruijie;pooling=false;CharSet=utf8;port=3306";
        public static MySqlConnection mysqlConn = new MySqlConnection(connMySqlStr);

        public static void insertMysql(List<sentenceAnalysis.to.qua_Combination> list, string content, string uri, string publishTime)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int count = 0;
                string countStr = "select count(*) from test_temp_table";
                mysqlConn.Open();
                MySqlCommand countComm = new MySqlCommand(countStr, mysqlConn);
                MySqlDataReader reader = countComm.ExecuteReader();
                while (reader.Read())
                {
                    count = Convert.ToInt32(reader[0]);
                }
                mysqlConn.Close();
                int ID = count + i + 2;
                string objectName = list[i].ship;
                string dateTime = publishTime;
                int timeFlag = 0;
                if (list[i].time != null)
                {
                    dateTime = list[i].time;
                    timeFlag = 1;
                }
                string FeatureName = list[i].place;
                string EventName = list[i].action;
                string source = uri;
                string insertSql = "insert into test_temp_table values (" + ID + ", null,'" + objectName + "','" + dateTime + "'," + timeFlag + ",'" + publishTime + "', null,'" + FeatureName + "',0,0,0,'" + EventName + "', 0, '" + source + "',null)";

                mysqlConn.Open();
                MySqlCommand insertComm = new MySqlCommand(insertSql, mysqlConn);
                insertComm.ExecuteNonQuery();
                mysqlConn.Close();
            }
        }

        public static ArrayList selectContent()
        {
            ArrayList list = new ArrayList();
            string sqlStr = "SELECT Text FROM gazetteer.ts_spatial_data_origin where TimeFlag = 1 and ObjectName like '%华盛顿%';";
            mysqlConn.Open();
            MySqlCommand comm = new MySqlCommand(sqlStr, mysqlConn);
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                if (reader[0].ToString() != null && reader[0].ToString() != "")
                    list.Add(reader[0].ToString());
            }
            mysqlConn.Close();
            return list;
        }

        public static void ExcuteTets()
        {

        }

        public static void insertTest(List<sentenceAnalysis.to.qua_Combination> list, string content, string text, string publishTime, string uri)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int count = 0;
                string countStr = "select count(*) from ts_spatial_all_data_zhang"; // 8236
                mysqlConn.Open();
                MySqlCommand countComm = new MySqlCommand(countStr, mysqlConn);
                MySqlDataReader reader = countComm.ExecuteReader();
                while (reader.Read())
                {
                    count = Convert.ToInt32(reader[0]);
                }
                mysqlConn.Close();
                int ID = count + 1;
                string objectName = list[i].ship;
                string dateTime = publishTime;
                int timeFlag = 0;
                if (list[i].time != null)
                {
                    dateTime = list[i].time;
                    timeFlag = 1;
                }
                string FeatureName = list[i].place;
                string EventName = list[i].action;
                string source = text;
                string insertSql = "insert into ts_spatial_all_data_zhang values (" + ID + ",'" + objectName + "','" + dateTime + "','" + publishTime + "'," + timeFlag + ",'" + FeatureName + "','" + list[i].move + "','" + list[i].moveType + "','" + EventName + "','" + uri + "','" + source + "')";

                mysqlConn.Open();
                MySqlCommand insertComm = new MySqlCommand(insertSql, mysqlConn);
                insertComm.ExecuteNonQuery();
                mysqlConn.Close();
            }
        }
    }
}

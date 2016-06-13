using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;

namespace toSpatialDataRuijie
{
    class Program
    {
        public static ArrayList objectName = new ArrayList();
        public static ArrayList DateTime = new ArrayList();
        public static ArrayList DateTime2 = new ArrayList();
        public static ArrayList TimeFlag = new ArrayList();
        public static ArrayList PublishTime = new ArrayList();
        public static ArrayList FeatureID = new ArrayList();
        public static ArrayList FeatureName = new ArrayList();
        public static ArrayList Movement = new ArrayList();
        public static ArrayList MovementName = new ArrayList();
        public static ArrayList EventName = new ArrayList();
        public static ArrayList SourceID = new ArrayList();
        public static ArrayList Source = new ArrayList();

        static void Main(string[] args)
        {
            selectFromTemp();
            ChangeMovement();
            timeToDateTime();
       //     timeFlag();
            timeChange();
            CreateFeatureID();
            ChangeMovement();

            for (int i = 0; i < objectName.Count; i++)
            {
                DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();

                dtFormat.ShortDatePattern = "yyyy-MM-dd";

                //DateTime dt = Convert.ToDateTime(DateTime[i].ToString());
                //DateTime dt2 = dt.Date;

                string sql = "insert into ts_spatial_data values (" + i + ", null,'" + objectName[i].ToString() + "','" + DateTime[i].ToString() + "','" + DateTime[i].ToString().Replace(" ", "") + "',''," + Convert.ToInt32(TimeFlag[i]) + ",0,'" + Convert.ToString(PublishTime[i]) + "'," + Convert.ToInt32(FeatureID[i]) + " ,'" + FeatureName[i].ToString() + "'," + Convert.ToInt32(Movement[i]) + ",'" + MovementName[i].ToString() + "',0,'" + EventName[i].ToString() + "', 0, '" + SourceID[i].ToString() + "',0,'" + Source[i] + "',0,null,0)";
                mysqlConn.Open();
                MySqlCommand comm = new MySqlCommand (sql, mysqlConn);
                comm.ExecuteNonQuery();
                mysqlConn.Close();
            }
        }

        public static string connMySqlStr = "Database=gazetteer;Data Source=127.0.0.1;User Id=root;Password=zhangruijie;pooling=false;CharSet=utf8;port=3306";
        public static MySqlConnection mysqlConn = new MySqlConnection(connMySqlStr);
 
        public static void selectFromTemp()
        {
            string sql = "select * from ts_spatial_all_data_zhang";
            mysqlConn.Open();
            MySqlCommand comm = new MySqlCommand(sql, mysqlConn);
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                
                objectName.Add(reader[1].ToString());
                DateTime.Add(reader[2].ToString());
                
                PublishTime.Add(reader[3].ToString());
                TimeFlag.Add(reader[4].ToString());
                FeatureID.Add(0);
                FeatureName.Add(reader[5].ToString());
                MovementName.Add(reader[6].ToString());
                Movement.Add(reader[7].ToString());
                EventName.Add(reader[8].ToString());
                SourceID.Add(reader[9].ToString());
                Source.Add(reader[10].ToString());
            }

            //objectName.RemoveRange(0, 9932);
            //DateTime.RemoveRange(0, 9932);
            //PublishTime.RemoveRange(0, 9932);
            //TimeFlag.RemoveRange(0, 9932);
            //FeatureID.RemoveRange(0, 9932);
            //FeatureName.RemoveRange(0, 9932);
            //MovementName.RemoveRange(0, 9932);
            //Movement.RemoveRange(0, 9932);
            //EventName.RemoveRange(0, 9932);
            //SourceID.RemoveRange(0, 9932);
            mysqlConn.Close();
        }

        public static void timeToDateTime()
        {
            for (int i = 0; i < PublishTime.Count; i++)
            {
                PublishTime[i] = PublishTime[i].ToString().Split(' ')[0];
                PublishTime[i] = Convert.ToDateTime(PublishTime[i].ToString()).Year.ToString() + '-' + Convert.ToDateTime(PublishTime[i].ToString()).Month.ToString() + '-' + Convert.ToDateTime(PublishTime[i].ToString()).Day.ToString();
            }
        }

        public static void timeFlag()
        {
            for (int i = 0; i < TimeFlag.Count; i++)
            {
                if (Convert.ToInt32(TimeFlag[i]) == 1)
                    TimeFlag[i] = 0;
                if (Convert.ToInt32(TimeFlag[i]) == 0)
                    TimeFlag[i] = 1;
            }
        }
        public static void timeChange()
        {
            timeToDateTime();
            string strPattern = "^((?<year>\\d{2,4})年)?((?<month>\\d{1,2})月)?((?<day>\\d{1,2})日)?([\u4E00-\u9FA5]+)?$";
            Regex rex = new Regex(strPattern);
            for (int i = 0; i < DateTime.Count; i++)
            {
                MatchCollection matches = rex.Matches(DateTime[i].ToString());
                string year = "", month = "", day = "";
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    year =   groups[1].ToString().Replace("年", "");
                    month = groups[2].ToString().Replace("月", "");
                    day = groups[3].ToString().Replace("日", "");
                }
                if (day == "")
                    TimeFlag[i] = 0;
                else
                    TimeFlag[i] = Convert.ToInt32(TimeFlag[i])*1;

                if (year == "")
                { year = Convert.ToDateTime(PublishTime[i].ToString()).Year.ToString();
              //  TimeFlag[i] = 0;
                }
                if (month == "")
                {
                    month = Convert.ToDateTime(PublishTime[i].ToString()).Month.ToString(); 
                   // TimeFlag[i] = 0; 
                }
                if (day == "")
                {
                    day = Convert.ToDateTime(PublishTime[i].ToString()).Day.ToString(); //TimeFlag[i] = 0; 
                }
                DateTime[i] = year.ToString() + '-' + month.ToString() + '-' + day.ToString();
            }
        }

        public static void CreateFeatureID()
        {
            for (int i = 0; i < FeatureName.Count; i++) // 调试使用 int i = 0; i < FeatureName.Count; i++
            {
                string sql = "select FeatureID from gkb_vocabulary where Term = '" +FeatureName[i].ToString()+"'";
                mysqlConn.Open();
                MySqlCommand comm = new MySqlCommand(sql, mysqlConn);
                MySqlDataReader reader = comm.ExecuteReader();
                int tempId = 0;
                while (reader.Read())
                {
                    tempId = Convert.ToInt32(reader[0]);
                }
                mysqlConn.Close();
                FeatureID[i] = tempId;
            }

        }

        public static void ChangeMovement()
        {
            for (int i = 0; i < Movement.Count; i++)
            {
                Movement[i] = Movement[i].ToString().Replace("move","");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace match
{
    class db
    {
        public static string connMySqlStr = "Database=gazetteer;Data Source=127.0.0.1;User Id=root;Password=zhangruijie;pooling=false;CharSet=utf8;port=3306";
        public static MySqlConnection mysqlConn = new MySqlConnection(connMySqlStr);

        public static List<match.unit> getData(string sqlStr)
        {
            List<match.unit> list = new List<unit>();
            mysqlConn.Open();
            MySqlCommand comm = new MySqlCommand(sqlStr, mysqlConn);
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                if (reader[0].ToString() != null && reader[0].ToString() != "")
                {
                    match.unit temp = new unit();
                    temp.objectName = reader[0].ToString();
                    temp.dateTime2 = reader[1].ToString();
                    temp.publishTime = reader[2].ToString();
                //    temp.featureID = reader[3].ToString();
                    temp.movement = reader[3].ToString();
                    temp.source = reader[4].ToString();
                    list.Add(temp);
                }
            }
            mysqlConn.Close();
            return list;
        }
    }
}

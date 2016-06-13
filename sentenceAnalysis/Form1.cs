using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using sentenceAnalysis.to;
using System.IO;
using System.Collections;

namespace sentenceAnalysis
{
    public partial class Form1 : Form
    {
       
        public int count_next = 0;
        public ArrayList list = db.selectContent(); // 暂时注释
        public ArrayList newList = new ArrayList();
    //    public ArrayList list = new ArrayList();
        public Form1()
        {
         //   writeInto();
            readInfo();
            InitializeComponent();
        }
        public void writeInto()
        {
            FileStream fs = new FileStream("target.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            for (int i = 0; i < list.Count; i++)
            {
                sw.WriteLine(list[i]);
            }
            sw.Close();
            fs.Close();
        }
        public void readInfo()
        {
           
            FileStream fs = new FileStream("target.txt", FileMode.Open);
            StreamReader reader = new StreamReader(fs, Encoding.Default);
            string sline = reader.ReadLine();
            while (sline!=null)
            {
                newList.Add(sline);
                sline = reader.ReadLine();
            }
            reader.Close();
            fs.Close();
        }
        private void search_Click(object sender, EventArgs e)
        {

            string content = search_Content.Text;
            //count_next = 0;
      //      content = newList[count_next].ToString();  //87 暂时注释
            search_Content.Text = content;
            count_next++;
            
            //分词
            string result = function.parse(content);
            parse_result.Clear();
            //parse_result.Text +="分词结果为： \n";
            parse_result.Text += result;

            //命名实体识别结果
            List<sentenceAnalysis.to.word> place_entity = new List<sentenceAnalysis.to.word>();
            List<sentenceAnalysis.to.word> ship_entity = new List<sentenceAnalysis.to.word>();
            List<sentenceAnalysis.to.word> event_entity = new List<sentenceAnalysis.to.word>();
            List<sentenceAnalysis.to.word> time_entity = new List<sentenceAnalysis.to.word>();
            function.seaArea(function.whole_page);
            function.entity_recog(function.whole_page, place_entity, ship_entity, event_entity, time_entity);
            entity_recog.Clear();
            entity_recog.Text +=("命名实体： ");
           // function.entity_recog(result);
            
            entity_recog.Text += "\r\n";
            entity_recog.Text +=("目标实体： ");
            for (int i = 0; i < ship_entity.Count; i++)
            {
                entity_recog.Text += (ship_entity[i].character) + "||";
            }

            entity_recog.Text += "\r\n";
            entity_recog.Text +=("地理实体：");
            for (int i = 0; i < place_entity.Count; i++)
            {
                entity_recog.Text += (place_entity[i].character) + "||";
            }
           // entity_recog.Text += (place_entity);
            // 行为分析结果
            //action.Clear();
            entity_recog.Text += "\r\n";
            entity_recog.Text += ("事件实体： ");
            for (int i = 0; i < event_entity.Count; i++)
            {
                entity_recog.Text += (event_entity[i].character) + "||";
            }
            //action.Text += (event_entity);

            // 词组分析结果
            phrase_result.Clear();
            phrase_result.Text += ("目标词组：");
            function.place_phrase_recog( function.whole_page);
            phrase_result.Text += "\r\n";
            phrase_result.Text += ("地名词组：");
            for (int i = 0; i < function.place_phrase.Count; i++)
            {
                phrase_result.Text += function.place_phrase[i].character;
            }

                /*  for (int i = 0; i < function.place_phrase.Count; i++)
                  {
                      if (i + 2 < function.place_phrase.Count)
                      {
                          if (function.place_phrase[i + 1].position - function.place_phrase[i].position == 1 && function.place_phrase[i + 2].position - function.place_phrase[i + 1].position == 1)
                          {
                              phrase_result.Text += function.place_phrase[i].character + function.place_phrase[i + 1].character + function.place_phrase[i + 2].character;
                              i = i + 2;
                          }
                          else
                          {
                              phrase_result.Text += function.place_phrase[i].character + function.place_phrase[i + 1].character;
                              i = i + 1;
                          }
                      }
                      else
                      {
                          phrase_result.Text += function.place_phrase[i].character + function.place_phrase[i + 1].character;
                          i = i + 1;
                      }
                  } */
                // 转喻结果
                metonymy.Clear();
               function.metonymy_place(result);
            for (int i = 0; i < function.metonymy.Count; i++)
            {

                metonymy.Text += function.metonymy[i] +"||";
            }


                //四元组分析结果
                quadruple.Clear();
         //   List<sentenceAnalysis.to.qua_Combination> com  = new List<qua_Combination>();
                List<sentenceAnalysis.to.qua_Combination> result_Qua = new List<qua_Combination>();
                result_Qua = function.quadruple(content);
               // quadruple.Text += result_Qua;
             //   quadruple.Text += "\n";
                for (int i = 0; i < result_Qua.Count; i++)
            {
                if (result_Qua[i].place != null)
                {
                    quadruple.Text += "<";
                    quadruple.Text += result_Qua[i].ship + ",";

                    if (Input_Date.Text != "")
                        result_Qua[i].time = Input_Date.Text;
                    quadruple.Text += result_Qua[i].time + ",";
                    quadruple.Text += result_Qua[i].move + " ";
                    quadruple.Text += result_Qua[i].place + "(";
                    quadruple.Text += result_Qua[i].moveType + "),";
                    quadruple.Text += result_Qua[i].action + ">";
                    quadruple.Text += "\r\n";
                }
            } 
         

                
        }

        public void writeInto(List<string> list)
        {
            FileStream fs = new FileStream("allTargetData.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            for (int i = 0; i < list.Count; i++)
            {
                sw.WriteLine(list[i]);
            }
            sw.Close();
            fs.Close();
        }

        public List<string> readAllInfo()
        {
            List<string> content = new List<string>();
            FileStream fs = new FileStream("allTargetData.txt", FileMode.Open);
            StreamReader reader = new StreamReader(fs, Encoding.Default);
            string sline = reader.ReadLine();
            while (sline != null)
            {
                content.Add(sline);
                sline = reader.ReadLine();
            }
            reader.Close();
            fs.Close();
            return content;
        }

        public void readAllInfo(List<string> content, List<string> uri, List<string> publishTime)
        {
            DirectoryInfo theFolder = new DirectoryInfo(@"C:\Users\zhangruijie\Downloads\新浪2013");
            foreach (FileInfo nextFile in theFolder.GetFiles())
            {
                FileStream fs = new FileStream( @"C:\Users\zhangruijie\Downloads\新浪2013\"+nextFile.Name, FileMode.Open);
                StreamReader reader = new StreamReader(fs, Encoding.UTF8);
                string sline = reader.ReadLine();
                if (sline != null)
                    uri.Add(sline);
                sline = reader.ReadLine();
                sline = reader.ReadLine();
                if (sline != null)
                    publishTime.Add(sline);
                sline = reader.ReadToEnd();
                if (sline != null)
                    content.Add(sline);
                reader.Close();
                fs.Close();
            }

            List<string> newList =new List<string>();
            
          //  return newList;
        }

        private void export_Click(object sender, EventArgs e)
        {
            List<string> content = new List<string>();
            List<string> uri = new List<string>();
            List<string> publishTime = new List<string>();
            //readAllInfo(content, uri, publishTime);
            db.ExcuteData(content, uri, publishTime);
      //       writeInto(content);
            content = readAllInfo();
            for (int i = 0; i < content.Count; i++)
            {

                List<string> tempList = splitContent(content[i]);
                Console.WriteLine(i + ": " + content[i]);
                for (int j = 0; j < tempList.Count; j++)
                {
                    Console.WriteLine(j + ": " + tempList[j]);
                    search_Content.Text = tempList[j].ToString();
                    List<sentenceAnalysis.to.qua_Combination> result_Qua = new List<qua_Combination>();

                    result_Qua = function.quadruple(tempList[j].ToString());
                    Console.WriteLine(DateTime.Now);
                    db.insertTest(result_Qua, tempList[j].ToString(), tempList[j].ToString(), publishTime[i].ToString(), uri[i].ToString());
                }


            }

            MessageBox.Show("over");
            /*      db.ExcuteData(content, uri, publishTime);
              for (int i = 0; i < content.Count; i++)
              {
                  List<sentenceAnalysis.to.qua_Combination> result_Qua = new List<qua_Combination>();
                  result_Qua = function.quadruple(content[i]);
                  //        if(result_Qua.Count !=0)
                  db.insertMysql(result_Qua, content[i], uri[i], publishTime[i]);
              } */
          
        }

        // 分句
        public List<string> splitContent(string str)
        {
            string[] stringForResult = str.Split('。');
            List<string> list = new List<string>();
            for (int i = 0; i < stringForResult.Count(); i++)
            {
                string[] temp = stringForResult[i].Split('；');
                for (int j = 0; j < temp.Length; j++)
                    list.Add(temp[j]);
            }
            return list;
        }
    }
}

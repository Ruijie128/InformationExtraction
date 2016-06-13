using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using sentenceAnalysis.wordsSeg;
using System.Runtime.InteropServices;
using System.Collections;
using System.Text;


namespace sentenceAnalysis.to
{
    // all of the functions are writen here

    // 地名词组
    public enum countries
    {
        美国,
        中国,
        韩国,
        日本,
        英国, 加拿大, 菲律宾
    }

    public class function
    {
        public static List<sentenceAnalysis.to.word> whole_page = new List<sentenceAnalysis.to.word>();

        // public static ArrayList place_entity = new ArrayList();
        // 航母号词组
        //   public static ArrayList ship_entity = new ArrayList();
        //时间词组
        //   public static ArrayList time_entity = new ArrayList();
        //动作 or事件词组
        //  public static ArrayList event_entity = new ArrayList();
        public static List<sentenceAnalysis.to.word> place_phrase = new List<sentenceAnalysis.to.word>();
        //转喻的词 
        public static ArrayList metonymy = new ArrayList();

        // 国家


        // sentenceAnalysis.wordsSeg.nlpir nlp = new sentenceAnalysis.wordsSeg.nlpir();
        // parse the sentence and give the speeches of these words
        public static string parse(string content)
        {

            string str = toParse(content);
            whole_page = toList(str);
            //whole_page = deleteULEword(whole_page);
            return str;
        }

        // 切分结果转换为list
        public static List<sentenceAnalysis.to.word> toList(string str)
        {
            List<sentenceAnalysis.to.word> temp_list = new List<sentenceAnalysis.to.word>();
            string[] stringForResult = str.Split(' ');
            for (int j = 0; j < stringForResult.Length; j++)
            {
                if (stringForResult[j] != "" && stringForResult[j] != " ")
                {
                    string[] temp = stringForResult[j].Split('/');
                    sentenceAnalysis.to.word single = new sentenceAnalysis.to.word();
                    single.character = temp[0];
                    single.position = j;
                    if (temp.Length == 1)
                        single.speech = null;
                    else
                        single.speech = temp[1];
                    temp_list.Add(single);
                }
            }

            return deleteULEword(temp_list);
        }

        // 结果分词
        public static string toParse(string content)
        {
            if (!nlpir.NLPIR_Init("../../", 0, ""))//给出Data文件所在的路径，注意根据实际情况修改。
            {
                // System.Console.WriteLine("Init ICTCLAS failed!");
                MessageBox.Show("Init ICTCLAS failed!");
                return null;
            }
            //System.Console.WriteLine("Init ICTCLAS success!");
            //    MessageBox.Show("Init ICTCLAS success!");
            nlpir.NLPIR_AddUserWord("[华盛顿号] ship");
            addShipWords();
            //   int shipCount = nlpir.NLPIR_ImportUserDict(@"../../Data/ship.txt");
            int eventCount = nlpir.NLPIR_ImportUserDict(@"../../Data/event.txt");
            //      int placeCount = nlpir.NLPIR_ImportUserDict(@"../../Data/places.txt");
            //      MessageBox.Show(" user dic " + shipCount + " user event  dic " + eventCount);
            nlpir.NLPIR_NWI_Complete();
            nlpir.NLPIR_SaveTheUsrDic();
            int count = nlpir.NLPIR_GetParagraphProcessAWordCount(content);// get the count after parsing
            result_t[] result = new result_t[count];
            nlpir.NLPIR_ParagraphProcessAW(count, result);
            int i = 1;
            string[,] character = new string[count, 2];
            foreach (result_t r in result)
            {
                String sWhichDic = "";
                switch (r.word_type)
                {
                    case 0:
                        sWhichDic = "核心词典";
                        break;
                    case 1:
                        sWhichDic = "用户词典";
                        break;
                    case 2:
                        sWhichDic = "专业词典";
                        break;
                    default:
                        break;
                }
                //  MessageBox.Show("No." + (i++) + "start:" + r.start + "length:" + r.length + ",POS_ID:" + r.POS_id + ",Word_ID:" + r.word_ID + ", UserDefine:" + sWhichDic + "\n");//, s.Substring(r.start, r.length)

            }
            StringBuilder sResult = new StringBuilder(600);
            //准备存储空间         

            IntPtr intPtr = nlpir.NLPIR_ParagraphProcess(content);//切分结果保存为IntPtr类型
            String str = Marshal.PtrToStringAnsi(intPtr);//将切分结果转换为string
            return str;
        }
        //删除虚词
        public static List<sentenceAnalysis.to.word> deleteULEword(List<sentenceAnalysis.to.word> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].speech == "ule" || list[i].speech == "rz" || list[i].speech == "f")
                {
                    list.RemoveAt(i);
                    i--;
                }
                if (i >= 0 && (list[i].character == "出港" || list[i].character == "离港"))
                {
                    list[i].speech = "anchor";
                    list[i].character = list[i].character + "横须贺";
                }

                if (i >= 0 && (list[i].character == "入港" || list[i].character == "返港" || list[i].character == "回港" || list[i].character == "进港"))
                {
                    list[i].speech = "anchor";
                    list[i].character = list[i].character + "横须贺";
                }
                if (i >= 0 && (list[i].character == "母港" || list[i].character == "基地" || list[i].character == "新母港"))
                {

                    list[i].character = "横须贺";
                    list[i].speech = "ns";
                }

                if (i >= 0 && (list[i].character == "位于" && i - 1 >= 0 && list[i - 1].speech.Contains("move")))
                {

                    list.RemoveAt(i);
                    i--;
                }
            }
            return list;
        }

        // 添加航母词汇
        public static void addShipWords()
        {
            StreamReader sReader = new StreamReader(@"../../Data/event.txt", Encoding.Default);
            string str = sReader.ReadLine();
            while (str != null)
            {
                nlpir.NLPIR_AddUserWord(str);
                str = sReader.ReadLine();
            }
        }
        // entity recog
        // 地名词为 nsf or ns
        public static void entity_recog(List<sentenceAnalysis.to.word> list, List<sentenceAnalysis.to.word> place_entity, List<sentenceAnalysis.to.word> ship_entity, List<sentenceAnalysis.to.word> event_entity, List<sentenceAnalysis.to.word> time_entity)
        {
            place_entity.Clear();
            ship_entity.Clear();
            event_entity.Clear();
            time_entity.Clear();
            seaArea(list);
            // 首先识别nsf 的词，其为地理名词
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].character == "出港" || list[i].character == "离港")
                {
                    list[i].speech = "anchor";
                    list[i].character = list[i].character + "横须贺";
                }

                if (list[i].character == "入港" || list[i].character == "返港" || list[i].character == "进港")
                {
                    list[i].speech = "anchor";
                    list[i].character = list[i].character + "横须贺";
                }
                if (list[i].character == "母港" || list[i].character == "基地")
                {

                    list[i].character = "横须贺";
                    list[i].speech = "ns";
                }

                if (list[i].character == "位于" && i - 1 >= 0 && list[i - 1].speech.Contains("move"))
                {

                    list.RemoveAt(i);
                    i--;
                }

                if (list[i].speech == "nsf" || list[i].speech == "ns" || list[i].speech == "jidi")
                {
                    place_entity.Add(list[i]);

                }
                else if (list[i].speech == "ship")
                {
                    ship_entity.Add(list[i]);
                }
                else if (list[i].speech == "event" || list[i].speech == "vn") // vn 动名词 现暂时加入 测试结果
                {
                    event_entity.Add(list[i]);
                }
                else if (list[i].speech == "t")
                {
                    if (i + 1 < list.Count)
                    {

                        while (i + 1 < list.Count && list[i + 1].speech == "t")
                        {
                            combinePhrase(list, i);
                            //      i = i - 1;
                        }
                    }

                    time_entity.Add(list[i]);
                }
            }
            // 单独对国家处理  转喻的问题
            for (int i = 0; i < list.Count; i++)
            {
                if ((list[i].speech == "nsf" || list[i].character == "中国" || list[i].speech == "b") && i - 1 > 0 && list[i - 1].speech != null && !list[i - 1].speech.Contains("move"))
                {
                    list[i].metonymy = 1;
                }
            }
            place_entity = isPlace(place_entity);
        }



        // 动词+地名 词组分析 关键点分析 或者地名+movement 锚点分析
        public static List<sentenceAnalysis.to.word> move_Place(List<sentenceAnalysis.to.word> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].speech != null && (list[i].speech.Contains("ns") || list[i].speech.Contains("jidi"))) //&& list[i].metonymy == 0
                {
                    // 简化版本  只有动词+地名 不可能 地名+动词

                    if (i - 1 >= 0 && list[i - 1].speech != null && list[i - 1].speech.Contains("move"))
                    {
                        //if (i + 1 < list.Count && list[i + 1].speech != null && list[i + 1].speech.Contains("move"))
                        //{
                        //    list[i - 1].character = list[i - 1].character + list[i].character + list[i + 1].character;
                        //    list[i - 1].speech = "anchor";
                        //    list.RemoveAt(i);
                        //    list.RemoveAt(i);
                        //    i--;
                        //}
                        //else
                        //{
                        list[i - 1].character = list[i - 1].character + list[i].character;
                        list[i - 1].speech = "anchor";
                        list.RemoveAt(i);
                        i--;
                        //}
                    }
                    //if (i - 1 > 0 && list[i - 1].speech != null && list[i - 1].speech.Contains("move"))
                    //{
                    //    if (i + 1 < list.Count && list[i + 1].speech !=null && list[i + 1].speech.Contains("move"))
                    //    {
                    //        list[i - 1].character = list[i - 1].character + list[i].character + list[i + 1].character;
                    //        list[i - 1].speech = "anchor";
                    //        list.RemoveAt(i);
                    //        list.RemoveAt(i );
                    //        i--;
                    //    }
                    //    else
                    //    {
                    //        list[i - 1].character = list[i - 1].character + list[i].character;
                    //        list[i - 1].speech = "anchor";
                    //        list.RemoveAt(i);
                    //        i--;
                    //    }
                    //}
                    //else
                    //{
                    //    if (i + 1 < list.Count && list[i + 1].speech != null && list[i + 1].speech.Contains("move"))
                    //    {
                    //        list[i].character =  list[i].character + list[i + 1].character;
                    //        list[i ].speech = "anchor";
                    //        list.RemoveAt(i + 1);

                    //    }

                    //}
                }
            }
            return list;
        }

        // 词组分析
        // 有两种 1 地名词组 2 航母号词组
        public static void place_phrase_recog(List<sentenceAnalysis.to.word> list)
        {
            place_phrase.Clear();

            // 检查连续出现三个以上的地名词
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].speech == "nsf" || list[i].speech == "ns" || list[i].speech == "jidi")
                {
                    sentenceAnalysis.to.word next = null;
                    if (i + 1 < list.Count)
                    {
                        next = list[i + 1];
                        if (list[i - 1].speech != null && list[i + 1].speech != null && (list[i - 1].speech.Contains("ns") || list[i + 1].speech.Contains("ns") || list[i - 1].speech.Contains("jidi") || list[i + 1].speech.Contains("jidi")))
                        {
                            //   place_phrase.Add(whole_page[i-1].speech == "nsf"  || whole_page[i - 1].speech == "ns"? whole_page[i-1] : null);
                            if (list[i - 1].speech == "nsf" || list[i - 1].speech == "ns" || list[i - 1].speech == "jidi")
                            {
                                combinePhrase(list, i - 1);
                                place_phrase.Add(list[i - 1]);
                                list[i - 1].metonymy = 0;

                                if (i + 1 < list.Count)
                                {
                                    if (next.speech == "nsf" || next.speech == "ns")
                                    {
                                        combinePhrase(list, i - 1);
                                        place_phrase.Add(list[i - 1]);

                                        list[i - 1].metonymy = 0;
                                        /*   foreach (string coun in Enum.GetNames(typeof(sentenceAnalysis.to.countries)))
                                           {
                                               if (coun == whole_page[i +1 ].character)
                                                   whole_page[i + 1].metonymy = 0;//此时不转喻 记录位置
                                           }*/
                                    }
                                }

                                /*  foreach (string coun in Enum.GetNames(typeof(sentenceAnalysis.to.countries)))
                                  {
                                      if (coun == whole_page[i - 1].character)
                                         whole_page[i-1].metonymy = 0 ;//此时不转喻 记录位置
                                  }*/
                            }
                            else
                            {
                                if (next.speech == "nsf" || next.speech == "ns" || next.speech == "jidi")
                                {
                                    combinePhrase(list, i);
                                    place_phrase.Add(list[i]);

                                    list[i].metonymy = 0;
                                    /*   foreach (string coun in Enum.GetNames(typeof(sentenceAnalysis.to.countries)))
                                       {
                                           if (coun == whole_page[i +1 ].character)
                                               whole_page[i + 1].metonymy = 0;//此时不转喻 记录位置
                                       }*/
                                }
                            }
                            //  place_phrase.Add(list[i]);
                            // list[i].metonymy = 1;

                            /* foreach (string coun in Enum.GetNames(typeof(sentenceAnalysis.to.countries)))
                            {
                                if (coun == whole_page[i ].character)
                                    whole_page[i - 1].metonymy = 0;//此时不转喻 记录位置
                            }*/

                            //  place_phrase.Add(next.speech == "nsf"  || whole_page[i + 1].speech == "ns"? next:null);


                        }
                    }
                }
            }



            //  return place_phrase; 
        }


        // 水域 海域处理
        public static void seaArea(List<sentenceAnalysis.to.word> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].speech == "nsf" || list[i].speech == "ns")
                {
                    if (i + 1 < list.Count && (list[i + 1].character.Equals("水域") || list[i + 1].character.Equals("海域")))
                    {
                        list[i].character = list[i].character + list[i + 1].character;
                        list.RemoveAt(i + 1);
                    }
                }
            }
        }

        // 基地词组识别
        //public static void ship_phrase_recog(string str)
        //{ }
        // 航母号词组识别
        public static void ship_phrase_recog(string str)
        { }

        //转喻
        public static void metonymy_place(string str)
        {
            // 转喻包括地名词组 如果词组中出现 国家名的话 不转喻
            //第二种情况是 特殊词语+国家
            // 默认全部转喻 但是地名词组中为地理名词 设置其为0
            // 转喻只出现在国家里面 所以从里面排除
            metonymy.Clear();
            for (int i = 0; i < whole_page.Count; i++)
            {
                if (whole_page[i].speech == "nsf" || whole_page[i].character == "中国")
                {
                    if (whole_page[i].metonymy == 1)
                        metonymy.Add(i);
                }
            }
        }

        // 四元组分析 其中至少要有航母和地名 并且以句子为单元
        public static List<sentenceAnalysis.to.qua_Combination> quadruple(string content)
        {
            //   string result = null;
            Console.WriteLine("quadruple()....");
            List<sentenceAnalysis.to.qua_Combination> result = new List<qua_Combination>();
            // 对输入内容按照句号进行切分 
            List<string> paragraph = new List<string>();
            string[] stringForResult = content.Split('。');
            List<List<sentenceAnalysis.to.word>> sentences = new List<List<sentenceAnalysis.to.word>>();
            //     List<sentenceAnalysis.to.qua_Combination> combination = new List<sentenceAnalysis.to.qua_Combination>();
            for (int j = 0; j < stringForResult.Length && (stringForResult[j] != ""); j++)
            {
                paragraph.Add(stringForResult[j]);
            }
            for (int i = 0; i < paragraph.Count(); i++)
            {
                string tempParaStr = paragraph[i];
                //if (!isNatural(tempParaStr))
                //{
                //    tempParaStr = null;
                //}
                string str = toParse(tempParaStr);
                Console.WriteLine(str);
                List<sentenceAnalysis.to.word> one_sentence = toList(str);
                place_phrase_recog(one_sentence);
                sentences.Add(one_sentence);
                // 在每个句子中查找是否有四元组
                List<sentenceAnalysis.to.qua_Combination> temp = new List<qua_Combination>();

                temp = isQuadruple(one_sentence, paragraph[i]);
                for (int i_temp = 0; i_temp < temp.Count; i_temp++)
                {
                    result.Add(temp[i_temp]);
                }

            }
            return result;
            //  return combination;
        }

        public static List<sentenceAnalysis.to.qua_Combination> isQuadruple(List<sentenceAnalysis.to.word> list, string str)
        {
            //   string result = null;
            List<sentenceAnalysis.to.qua_Combination> result = new List<qua_Combination>();
            sentenceAnalysis.to.qua_Combination com = new qua_Combination();
            List<sentenceAnalysis.to.word> place_Qua = new List<sentenceAnalysis.to.word>();
            List<sentenceAnalysis.to.word> ship_Qua = new List<sentenceAnalysis.to.word>();
            List<sentenceAnalysis.to.word> event_Qua = new List<sentenceAnalysis.to.word>();
            List<sentenceAnalysis.to.word> time_Qua = new List<sentenceAnalysis.to.word>();
            seaArea(list);
            entity_recog(list, place_Qua, ship_Qua, event_Qua, time_Qua);
            place_Qua = isPlace(place_Qua);
            list = move_Place(list);
            List<word> anchorList = new List<word>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].speech == "anchor")
                    anchorList.Add(list[i]);
            }
            //  sentenceAnalysis.to.qua_Combination com = new sentenceAnalysis.to.qua_Combination();
            // 存在锚点和航母号
            if (anchorList.Count != 0 && ship_Qua.Count != 0)
            {
                // 表示该句中四元组存在
                //判断模式
                if (anchorList.Count == 1 && ship_Qua.Count == 1)
                {

                    //  result += ship_Qua[0].character + "||";
                    com.ship = ship_Qua[0].character;
                    if (time_Qua.Count != 0)
                    {
                        for (int i = 0; i < time_Qua.Count; i++)
                        {
                            //result += time_Qua[i].character + "||";
                            com.time = time_Qua[i].character;
                        }
                    }
                    if (event_Qua.Count != 0)
                    {
                        for (int i = 0; i < event_Qua.Count; i++)
                        {
                            // result += event_Qua[i].character + "||";
                            com.action = event_Qua[i].character; // 修改 因为不一定只有一个事件
                        }
                    }

                    //   result += place_Qua[0].character + "||";
                    List<sentenceAnalysis.to.word> temp_list = new List<word>();
                    com.place = anchor_split(anchorList[0].character, com.place, temp_list);
                    for (int i = 0; i < temp_list.Count; i++)
                    {

                        com.move = temp_list[i].character;
                        com.moveType = temp_list[i].speech;

                    }
                    //    com.place = place_Qua[0].character;
                    result.Add(com);
                }
                else if (anchorList.Count > 1 && ship_Qua.Count == 1)
                {
                    result = sentenceAnalysis.to.match_models.singleShip_MultiPlaces(str, ship_Qua[0].character);
                }
                else if (anchorList.Count == 1 && ship_Qua.Count > 1)
                {
                    List<sentenceAnalysis.to.word> temp_list = new List<word>();
                    string place = anchor_split(anchorList[0].character, com.place, temp_list);
                    result = sentenceAnalysis.to.match_models.multiShip_SinglePlace(str, place);
                }
                else
                {
                    result = sentenceAnalysis.to.match_models.multiShips_multiPlaces(str);
                }

            }

            return result;
            //  return com;

        }

        public static void combinePhrase(List<sentenceAnalysis.to.word> list, int i)
        {
            // 用于合并词组 例如方位词 +地点 西太平洋 组合之后这将变成一个新的词 词性为地点 ns 
            // 地名词组合并  日本横须贺
            //航母号合并
            //词性取最后一个词的词性
            list[i].character = list[i].character + list[i + 1].character;
            list[i].metonymy = 0;
            list[i].position = list[i + 1].position;
            list[i].speech = list[i + 1].speech;
            for (int j = i + 1; j < list.Count - 1; j++)
            {
                list[j] = list[j + 1];
            }
            list.Remove(list[list.Count - 1]);
        }

        public static List<sentenceAnalysis.to.word> isPlace(List<sentenceAnalysis.to.word> list)
        {
            List<sentenceAnalysis.to.word> temp = new List<word>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].metonymy == 0)
                    temp.Add(list[i]);
            }

            return temp;
        }

        // 对锚点分解 在日本停靠
        public static string anchor_split(string str, string place, List<sentenceAnalysis.to.word> movelist)
        {
            string temp = toParse(str);
            List<sentenceAnalysis.to.word> sentence = toList(temp);
            place_phrase_recog(sentence);
            for (int i = 0; i < sentence.Count; i++)
            {
                if (sentence[i].speech.Contains("ns")||sentence[i].speech.Contains("jidi"))
                {
                    place = sentence[i].character;
                }

                if (sentence[i].speech.Contains("move"))
                {
                    movelist.Add(sentence[i]);
                }
            }
            return place;
        }

        // 因为全是字母或者数字无法分词 则对其进行判断
        public static bool isNatural(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex("[\u4E00-\u9FA5]+$");
            bool result = reg1.IsMatch(str);
            return result;
        }

    }
}

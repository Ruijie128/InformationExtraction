using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sentenceAnalysis.to
{
    public class match_models
    {
        //1单目标地名模式

        //2 单目标多地名模式
        /**
         * 在一句话中只出现一个航空母舰的名字 但是可能会出现多个地点 多个行为动词
         * 所以根据地点划分 有几个地点是几个四元组
         * 可能会出现多个时间 先根据“,”来划分，如果有其他的情况，则再继续进行细分 活着调整
         * 在此情况下，传递航母号
         * 
         **/
        public static List<sentenceAnalysis.to.qua_Combination>  singleShip_MultiPlaces(string str, string ship)
        {
            string[] stringForResult = str.Split('，');
            string result = null;
     //       List<string> sentence = new List<string>();
           
            List<sentenceAnalysis.to.qua_Combination> list = new List<qua_Combination>();
            for (int j = 0; j < stringForResult.Length && (stringForResult[j] != ""); j++)
            {
               
           //     sentence.Add(stringForResult[j]);
                string parseResult = function.toParse(stringForResult[j]);
                List<sentenceAnalysis.to.word> singlePart = function.toList(parseResult);
                function.place_phrase_recog(singlePart);// 地名词组识别 方便之后将这个识别为整个词组 放在一起

                // 由于是单目标 对应多个地名 所以所有的四元组的目标全是一致的，地点、行为则每个句子各有不同
                List<sentenceAnalysis.to.word> place_Qua = new List<sentenceAnalysis.to.word>();
                List<sentenceAnalysis.to.word> ship_Qua = new List<sentenceAnalysis.to.word>();
                List<sentenceAnalysis.to.word> event_Qua = new List<sentenceAnalysis.to.word>();
                List<sentenceAnalysis.to.word> time_Qua = new List<sentenceAnalysis.to.word>();
                function.entity_recog(singlePart, place_Qua, ship_Qua, event_Qua, time_Qua);
                place_Qua = function.isPlace(place_Qua);
                singlePart = function.move_Place(singlePart);
                List<word> anchorList = new List<word>();
                for (int i = 0; i < singlePart.Count; i++)
                {
                    if (singlePart[i].speech == "anchor")
                        anchorList.Add(singlePart[i]);
                }
                // 可知目标应该是可能为空的 但是地名不为空

                if (anchorList.Count != 0)
                {
                    
                    for (int anchori = 0; anchori < anchorList.Count; anchori++)
                    {
                        // 此单句中存在四元组
                        sentenceAnalysis.to.qua_Combination com = new qua_Combination();
                        result += ship + "||";
                   //     result += place_Qua[0].character + "||";
                        com.ship = ship;
                        List<sentenceAnalysis.to.word> temp_list = new List<word>();
                        com.place = function.anchor_split(anchorList[anchori].character, com.place, temp_list);
                        for (int i = 0; i < temp_list.Count; i++)
                        {

                            com.move = temp_list[i].character;
                            com.moveType = temp_list[i].speech;

                        }
                        if (event_Qua.Count != 0)
                        {
                            int i_event = 0;
                            while (i_event < event_Qua.Count)
                            {
                                result += event_Qua[i_event].character + "||";
                                com.action += event_Qua[i_event].character;
                                i_event++;
                            }

                        }
                        if (time_Qua.Count != 0)
                        {
                            int i_time = 0;
                            while (i_time < time_Qua.Count)
                            {
                                result += time_Qua[i_time].character + "||";
                                com.time += time_Qua[i_time].character;
                                i_time++;
                            }
                        }
                        list.Add(com);
                    }
                }
                
            }

            return list ;
        }
        //3 多目标单地名模式

        public static List<sentenceAnalysis.to.qua_Combination> multiShip_SinglePlace(string str, string place)
        {
            string result = null;
            string[] stringForResult = str.Split('，');
          //  List<string> sentence = new List<string>();
           
            List<sentenceAnalysis.to.qua_Combination> list = new List<qua_Combination>();

         //   sentenceAnalysis.to.qua_Combination com = new qua_Combination();
            List<word> anchorList = new List<word>();
            for (int j = 0; j < stringForResult.Length && (stringForResult[j] != ""); j++)
            {
            //    sentence.Add(stringForResult[j]);
                
                string parseResult = function.toParse(stringForResult[j]);
                List<sentenceAnalysis.to.word> singlePart = function.toList(parseResult);
                function.place_phrase_recog(singlePart);// 地名词组识别 方便之后将这个识别为整个词组 放在一起

                // 由于是单地名 对应多个目标 所以所有的四元组的地点标全是一致的，目标、行为则每个句子各有不同
                List<sentenceAnalysis.to.word> place_Qua = new List<sentenceAnalysis.to.word>();
                List<sentenceAnalysis.to.word> ship_Qua = new List<sentenceAnalysis.to.word>();
                List<sentenceAnalysis.to.word> event_Qua = new List<sentenceAnalysis.to.word>();
                List<sentenceAnalysis.to.word> time_Qua = new List<sentenceAnalysis.to.word>();
                //entity_recog(list, place_Qua, ship_Qua, event_Qua, time_Qua);
                function.entity_recog(singlePart, place_Qua, ship_Qua, event_Qua, time_Qua);
                place_Qua = function.isPlace(place_Qua);
                singlePart = function.move_Place(singlePart);
                
                for (int i = 0; i < singlePart.Count; i++)
                {
                    if (singlePart[i].speech == "anchor")
                        anchorList.Add(singlePart[i]);
                }
                // 可知地名应该是可能为空的 但是目标不为空

                if (ship_Qua.Count != 0 && anchorList.Count != 0)
                {
                    // 此单句中存在四元组

                    result += ship_Qua[0].character + "||";
                    result += place + "||";
                 //   com.ship = ship_Qua[0].character;

                    int i_ship = 0;
                    while (i_ship < ship_Qua.Count)
                    {
                        sentenceAnalysis.to.qua_Combination com = new qua_Combination();
                        result += ship_Qua[i_ship].character + "||";
                        com.ship = ship_Qua[i_ship].character;

                        List<sentenceAnalysis.to.word> temp_list = new List<word>();
                        com.place = function.anchor_split(anchorList[0].character, com.place, temp_list);
                        for (int i = 0; i < temp_list.Count; i++)
                        {

                            com.move = temp_list[i].character;
                            com.moveType = temp_list[i].speech;

                        }

                        if (event_Qua.Count != 0)
                        {
                            int i_event = 0;
                            while (i_event < event_Qua.Count)
                            {
                                result += event_Qua[i_event].character + "||";
                                com.action += event_Qua[i_event].character;
                                i_event++;
                            }
                        }
                        if (time_Qua.Count != 0)
                        {
                            int i_time = 0;
                            while (i_time < time_Qua.Count)
                            {
                                result += time_Qua[i_time].character + "||";
                                com.time += time_Qua[i_time].character;
                                i_time++;
                            }

                        }
                        list.Add(com);



                        i_ship++;
                    }

                    
                }

            }

            return list;
        }
        //4 多目标多地点复杂模式

        public static List<sentenceAnalysis.to.qua_Combination> multiShips_multiPlaces(string str)
        {
            string[] stringForResult = str.Split('，');
            //  List<string> sentence = new List<string>();
       //     sentenceAnalysis.to.qua_Combination com = new qua_Combination();
            List<sentenceAnalysis.to.qua_Combination> list = new List<qua_Combination>();
            string resultForQua = null;
         //   sentenceAnalysis.to.qua_Combination com = new qua_Combination();
            for (int j = 0; j < stringForResult.Length && (stringForResult[j] != ""); j++)
            {
                //    sentence.Add(stringForResult[j]);
                //sentenceAnalysis.to.qua_Combination com = new qua_Combination();
                string parseResult = function.toParse(stringForResult[j]);
                List<sentenceAnalysis.to.word> singlePart = function.toList(parseResult);
                function.place_phrase_recog(singlePart);// 地名词组识别 方便之后将这个识别为整个词组 放在一起

                // 由于是多个地名 对应多个目标 所以以每个逗号为单位，目标、行为则每个句子各有不同
                List<sentenceAnalysis.to.word> place_Qua = new List<sentenceAnalysis.to.word>();
                List<sentenceAnalysis.to.word> ship_Qua = new List<sentenceAnalysis.to.word>();
                List<sentenceAnalysis.to.word> event_Qua = new List<sentenceAnalysis.to.word>();
                List<sentenceAnalysis.to.word> time_Qua = new List<sentenceAnalysis.to.word>();
                function.entity_recog(singlePart, place_Qua, ship_Qua, event_Qua, time_Qua);
                place_Qua = function.isPlace(place_Qua);
                singlePart = function.move_Place(singlePart);
                List<word> anchorList = new List<word>();
                for (int i = 0; i < singlePart.Count; i++)
                {
                    if (singlePart[i].speech == "anchor")
                        anchorList.Add(singlePart[i]);
                }
                // 可知地名应该是可能为空的 但是目标不为空

                if (ship_Qua.Count != 0 && anchorList.Count != 0)
                {
                    // 此单句中存在四元组

                /*    com.ship += ship_Qua[0].character ;
                    resultForQua += ship_Qua[0].character + "||";
                    com.place += place_Qua[0].character ;
                    resultForQua += place_Qua[0].character + "||";*/

                    int i_ship = 0; 
                    while (i_ship < ship_Qua.Count)
                    {
                        int i_ancor = 0;
                        while (i_ancor < anchorList.Count)
                        {
                            sentenceAnalysis.to.qua_Combination com = new qua_Combination();
                            resultForQua += ship_Qua[i_ship].character + "||";
                            com.ship += ship_Qua[i_ship].character;
                            


                            List<sentenceAnalysis.to.word> temp_list = new List<word>();
                            com.place = function.anchor_split(anchorList[i_ancor].character, com.place, temp_list);
                            for (int i = 0; i < temp_list.Count; i++)
                            {

                                com.move = temp_list[i].character;
                                com.moveType = temp_list[i].speech;

                            }

                            if (event_Qua.Count != 0)
                            {
                                int i_event = 0;
                                while (i_event < event_Qua.Count)
                                {
                                    resultForQua += event_Qua[i_event].character + "||";
                                    com.action += event_Qua[i_event].character;
                                    i_event++;
                                }
                            }
                            if (time_Qua.Count != 0)
                            {
                                int i_time = 0;
                                while (i_time < time_Qua.Count)
                                {
                                    resultForQua += time_Qua[i_time].character + "||";
                                    com.time += time_Qua[i_time].character;
                                    i_time++;
                                }
                            }

                            resultForQua += "\n";
                            list.Add(com);
                            i_ancor++;
                        }
                        i_ship++;
                    }

                   

                    
                }
                
            }

            return list;
        }
    }
}

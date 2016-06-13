using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace match
{
    class Program
    {
        static void Main(string[] args)
        {

            string strOrigin = " select gazetteer.ts_spatial_data_origin.ObjectName, gazetteer.ts_spatial_data_origin.DateTime2, gazetteer.ts_spatial_data_origin.publishtime, gazetteer.ts_spatial_data_origin.Movement,gazetteer.ts_spatial_data_origin.Source from  gazetteer.ts_spatial_data_origin where(   gazetteer.ts_spatial_data_origin.TimeFlag = 1  ) order by publishtime";
            string strSpatial = " select gazetteer.ts_spatial_data.ObjectName, gazetteer.ts_spatial_data.DateTime2, gazetteer.ts_spatial_data.publishtime,gazetteer.ts_spatial_data.Movement, gazetteer.ts_spatial_data.Source from  gazetteer.ts_spatial_data where(   gazetteer.ts_spatial_data.TimeFlag = 1  ) order by publishtime";

            List<match.unit> listOrigin = match.db.getData(strOrigin);
            List<match.unit> listSpatial = match.db.getData(strSpatial);

            int count = 0;
            int i = 0, j = 0;
            while (i < listOrigin.Count && j < listSpatial.Count)
            {
                int origin =Convert.ToInt32( listOrigin[i].publishTime.Replace("-", ""));
                int spatial = Convert.ToInt32( listSpatial[j].publishTime.Replace("-", ""));
                if (listSpatial[j].objectName == listOrigin[i].objectName && listSpatial[j].dateTime2 == listOrigin[i].dateTime2 && listSpatial[j].publishTime == listOrigin[i].publishTime && listSpatial[j].movement == listOrigin[i].movement && listSpatial[j].source == listOrigin[i].source)
                {
                    i++; j++;
                    count++;
                }
                else if (origin > spatial)
                {
                    j++;
                }
                else
                {
                    i++;
                }
            }
        }
    }
}

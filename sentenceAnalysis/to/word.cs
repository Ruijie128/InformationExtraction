using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sentenceAnalysis.to
{
    public class word
    {
        public string character { get; set; }
        public int position { get; set; }
        public string speech { get; set; }
        public int metonymy
        {
            get;
            set ;
            
        } // 0 表示不转喻 表示地名  1 表示 转喻 政府或者军队
    }
}

using System.Collections.Generic;

namespace LineManager
{

    /// <summary>
    /// 
    /// </summary>

    public class FaHuoInfo
    {

        public string CustomerId { get; set; }

        
        public List<FaHuoItem> Items { get; set; } = new List<FaHuoItem>(); 

    }


    public class FaHuoItem
    {
        /// <summary>
        /// 型号
        /// </summary>
        public string XingHao { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }
    }
}
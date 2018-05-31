using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LineManager
{

    /// <summary>
    /// 条件数据
    /// </summary>
    public class Condition
    {

        /// <summary>
        /// 剩余最小长度
        /// </summary>
        public  int MinLeaveLength { get; set; }


        /// <summary>
        /// 限定长度
        /// </summary>
        public int Length { get; set; }


        /// <summary>
        /// 小于限定长度的个数
        /// </summary>
        public int LessLengthCount { get; set; }

        /// <summary>
        ///  大于等于限定长度的个数
        /// </summary>
        public  int GEqualsLengthCount { get; set; }

    }
}

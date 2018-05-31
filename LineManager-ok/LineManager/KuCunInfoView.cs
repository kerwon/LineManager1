using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LineManager
{
    public class KuCunInfoView
    {
        /// <summary>
        /// 排号
        /// </summary>
        [Display(Name = "排号")]
        public int 排号 { get; set; }


        /// <summary>
        /// 轮号
        /// </summary>
        [Display(Name = "轮号")]
        public int 轮号 { get; set; }


        public string 型号 { get; set; }


        public string 生产厂家 { get; set; }



        public int 长度 { get; set; }




        public string 是否整轮 { get; set; }

         
    }
}
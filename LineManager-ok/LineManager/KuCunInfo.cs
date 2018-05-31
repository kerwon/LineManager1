using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineManager
{

    public class KuCunInfo
    {
        /// <summary>
        /// 排号
        /// </summary>
        [Display(Name = "排号")]
        public int PaiHao { get; set; }


        /// <summary>
        /// 轮号
        /// </summary>
        [Display(Name = "轮号")]
        public int LunHao { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        [Display(Name = "排号")]
        public string XingHao { get; set; }


        /// <summary>
        /// 生产厂家
        /// </summary>
        [Display(Name = "排号")]
        public string Brand { get; set; }


        /// <summary>
        /// 长度
        /// </summary>
        [Display(Name = "排号")]
        public int Length { get; set; }


        /// <summary>
        /// 是否整轮
        /// </summary>

        [Display(Name = "排号")]
        public string IsWheel { get; set; }



        public static List<KuCunInfo> MapCunInfos(List<KuCunInfoView> kuCunInfos)
        {
            var result = new List<KuCunInfo>();

            if (kuCunInfos != null && kuCunInfos.Any())
            {
                kuCunInfos.ForEach(m =>
                {
                    result.Add(new KuCunInfo()
                    {
                        PaiHao = m.排号,
                        LunHao = m.轮号,
                        XingHao = m.型号,
                        Brand = m.生产厂家,
                        Length = m.长度,
                        IsWheel = m.是否整轮
                    });
                });
            }

            return result;
        }

    }
}

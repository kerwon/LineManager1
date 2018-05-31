using System.Collections.Generic;
using System.Linq;
using NPOI.HSSF.Record;

namespace LineManager
{


    public class LineResultInfoView
    {

        public string 出库目的地 { get; set; }

        public string 电缆型号 { get; set; }


        public string 生产厂家 { get; set; }


        public string 线轮位置 { get; set; }

        public string 是否整轮 { get; set; }


        public int 出库长度 { get; set; }


        public int 剩余长度 { get; set; }



        public static List<LineResultInfoView> MapLineResultInfoView(List<LineResultInfo> list)
        {
            var result = new List<LineResultInfoView>();

            if (list != null && list.Any())
            {
                list.ForEach(m =>
                {

                    var flag = m.PaiHao.GetValueOrDefault() == 0 || m.LunHao.GetValueOrDefault() == 0;

                    result.Add(new LineResultInfoView()
                    {
                        出库目的地 = m.CustomerId,
                        电缆型号 = m.XingHao,
                        生产厂家 = m.Brand,
                        线轮位置 = flag ? "库存不足" : $"{m.PaiHao}排 {m.LunHao}号",
                        是否整轮 = m.IsWheel,
                        出库长度 = m.Length.GetValueOrDefault(),
                        剩余长度 = m.LeaveLength.GetValueOrDefault() //m.Length.GetValueOrDefault()
                    });
                });
            }

            return result;
        }

    }
}
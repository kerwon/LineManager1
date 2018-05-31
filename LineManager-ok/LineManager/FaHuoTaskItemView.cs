using System.Collections.Generic;
using System.Linq;

namespace LineManager
{
    public class  FaHuoTaskItemView
    {
        public string 序号 { get; set; }

        public string 型号 { get; set; }

        public int 长度 { get; set; }


        public static List<FaHuoTaskItemView> MapTaskItems(List<FaHuoInfo> faHuoInfos)
        {
            var result = new List<FaHuoTaskItemView>();
            if (faHuoInfos != null && faHuoInfos.Any())
            {
                faHuoInfos.ForEach(m =>
                {
                    m.Items.ForEach(i =>
                    {
                        result.Add(new FaHuoTaskItemView()
                        {
                            序号 = m.CustomerId,
                            长度 = i.Length,
                            型号 = i.XingHao
                        });
                    });
                });
            }

            return result;
        }

    }
}
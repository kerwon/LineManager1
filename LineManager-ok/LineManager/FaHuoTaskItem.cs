using System.Collections.Generic;
using System.Linq;

namespace LineManager
{
    public class  FaHuoTaskItem
    {
        public string CustomerId { get; set; }

        public  string XingHao { get; set; }
         
        public int Length { get; set; }


        public static List<FaHuoTaskItem> MapTaskItems(List<FaHuoInfo> faHuoInfos)
        {

            var result = new List<FaHuoTaskItem>();
            if (faHuoInfos != null && faHuoInfos.Any())
            { 
                faHuoInfos.ForEach(m =>
                {
                    m.Items.ForEach(i =>
                    {
                        result.Add(new FaHuoTaskItem()
                        {
                            CustomerId = m.CustomerId,
                            Length = i.Length,
                            XingHao =  i.XingHao
                        });
                    });
                });
            }

            return result;
        }

    }
}
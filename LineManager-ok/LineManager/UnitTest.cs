using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LineManager
{
    public class UnitTest
    {

        public static void Test()
        {
            //var kuCunContent = File.ReadAllText(@"C:\Users\yifalei\Desktop\新建文件夹\LineManager\LineManager\bin\Debug\库存信息-636623591882140833.txt");
            //var kuCunInfo = JsonConvert.DeserializeObject<List<KuCunInfo>>(kuCunContent);

            //var faHuoContent =File.ReadAllText(@"C:\Users\yifalei\Desktop\新建文件夹\LineManager\LineManager\bin\Debug\发货任务信息-636623591882140833.txt");

            //var faHuoInfo = JsonConvert.DeserializeObject<List<FaHuoTaskItem>>(faHuoContent);
            var condition = new Condition()
            {
                MinLeaveLength = 100,
                GEqualsLengthCount = 2,
                Length = 500,
                LessLengthCount = 3
            };

            //var testData = CalcResultInfo2.Calc(kuCunInfo, faHuoInfo, condition);

            //var dataJson = JsonConvert.SerializeObject(testData);

            //Console.WriteLine();


            //var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"result-{DateTime.Now.Ticks}.xlsx");

            //ExcelHelper.DownLoad(testData, filePath);

            //Console.WriteLine(filePath);

            List<LineResultInfo> testData = null;
            var kuCunInfo = InitKuCunInfos();
            var faHuoInfo = new List<FaHuoTaskItem>()
            {
                new FaHuoTaskItem()
                {
                    CustomerId = "1",
                    Length = 700,
                    XingHao = "A"
                }
            };
             

             //testData = CalcResultInfo2.Calc(kuCunInfo, faHuoInfo, condition);

            kuCunInfo = InitKuCunInfos();
            
            faHuoInfo = new List<FaHuoTaskItem>()
            {
                new FaHuoTaskItem()
                {
                    CustomerId = "1",
                    Length = 500,
                    XingHao = "A"
                }
            };

            //testData = CalcResultInfo2.Calc(kuCunInfo, faHuoInfo, condition);

            kuCunInfo = InitKuCunInfos();
            faHuoInfo = new List<FaHuoTaskItem>()
            {
                new FaHuoTaskItem()
                {
                    CustomerId = "1",
                    Length = 400,
                    XingHao = "A"
                }
            };

            //testData = CalcResultInfo2.Calc(kuCunInfo, faHuoInfo, condition);

            kuCunInfo = InitKuCunInfos();
            faHuoInfo = new List<FaHuoTaskItem>()
            {
                new FaHuoTaskItem()
                {
                    CustomerId = "1",
                    Length = 100,
                    XingHao = "A"
                }
            };

            //testData = CalcResultInfo2.Calc(kuCunInfo, faHuoInfo, condition);
            
            kuCunInfo = InitKuCunInfos();
            faHuoInfo = new List<FaHuoTaskItem>()
            {
                new FaHuoTaskItem()
                {
                    CustomerId = "1",
                    Length = 1400,
                    XingHao = "A"
                }
            };

            testData = CalcResultInfo2.Calc(kuCunInfo, faHuoInfo, condition);
        }

        private static List<KuCunInfo> InitKuCunInfos()
        {
            var baseKuCunInfo = new KuCunInfo()
            {
                XingHao = "A",
                LunHao = 40,
                PaiHao = 30,
                Length = 200,
            };

            var kuCunInfo = new List<KuCunInfo>()
            {
                new KuCunInfo()
                {
                    XingHao = baseKuCunInfo.XingHao,
                    LunHao = baseKuCunInfo.LunHao + 1,
                    PaiHao = baseKuCunInfo.PaiHao + 1,
                    Length = 200,
                    IsWheel = "否"
                },
                new KuCunInfo()
                {
                    XingHao = baseKuCunInfo.XingHao,
                    LunHao = baseKuCunInfo.LunHao + 1,
                    PaiHao = baseKuCunInfo.PaiHao + 1,
                    Length = 300,
                    IsWheel = "否"
                },
                new KuCunInfo()
                {
                    XingHao = baseKuCunInfo.XingHao,
                    LunHao = baseKuCunInfo.LunHao + 1,
                    PaiHao = baseKuCunInfo.PaiHao + 1,
                    Length = 600,
                    IsWheel = "否"
                },
                new KuCunInfo()
                {
                    XingHao = baseKuCunInfo.XingHao,
                    LunHao = baseKuCunInfo.LunHao + 1,
                    PaiHao = baseKuCunInfo.PaiHao + 1,
                    Length = 1000,
                    IsWheel = "是"
                },
            };
            return kuCunInfo;
        }
    }
}

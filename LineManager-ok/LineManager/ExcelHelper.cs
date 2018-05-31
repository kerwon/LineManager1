using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Npoi.Mapper;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace LineManager
{
    public class ExcelHelper
    {
        
        public static List<KuCunInfoView> ReadKuCunInfosExcel(MemoryStream ms)
        {
            var mapper = new Mapper(ms);

            var data = mapper.Take<KuCunInfoView>("库存信息");
            if (data != null && data.Any())
            {
                return data.Select(m => m.Value).ToList();
            }

            throw new Exception("获取库存信息出错!");
        }


        public static List<FaHuoInfo> ReadFaHuoInfosExcel(MemoryStream ms)
        {
            var mapper = new Mapper(ms);

            mapper.Map<FaHuoInfo>("序号", o => o.CustomerId)
                .Map(column => true,
                    (column, target) =>
                    {
                        if (column.CurrentValue == null)
                            return true;

                        var inx = column.Attribute.Index / 2;
                        var mod = column.Attribute.Index % 2;
                        var isNew = mod == 1;

                        var faHuo = target as FaHuoInfo;
                        if (faHuo == null)
                            throw new Exception("读取发货任务信息Excel数据出错!");

                        FaHuoItem item = isNew ? new FaHuoItem() : faHuo.Items.Last();


                        var tmpCellVal = column.CurrentValue?.ToString() ?? string.Empty;

                        if (column.HeaderValue.Equals("型号"))
                        {
                            item.XingHao = tmpCellVal;
                        }

                        if (column.HeaderValue.Equals("长度"))
                        {
                            int.TryParse(tmpCellVal, out int tmpval);
                            item.Length = tmpval;
                        }

                        if (isNew)
                            faHuo.Items.Add(item);
                        return true;
                    });


            var data = mapper.Take<FaHuoInfo>("发货任务信息");

            if (data != null && data.Any())
            {
                return data.Select(m => m.Value).ToList();
            }

            throw new Exception("获取发货任务信息出错!");
        }



        public static void DownLoad(List<LineResultInfo> resultInfos, string filePath)
        {
            //var mapperFirst = new Mapper();
            //var data = LineResultInfoView.MapLineResultInfoView(resultInfos);

            //var result = data.OrderBy(m => m.出库目的地).ThenBy(m => m.电缆型号).ToList();
            //mapperFirst.Put(data, "线缆出库信息表", true);
            //mapperFirst.Save(filePath);

            //return;

            IWorkbook wkbook = new XSSFWorkbook();
            //2. 在该工作簿中创建工作表  
            ISheet sheet = wkbook.CreateSheet("线缆出库信息表");

            // 向该工作表中插入行和单元格  
            var rowIndex = 0;
            var colIndex = 0;

            IRow firstRow = sheet.CreateRow(rowIndex++);
            firstRow.CreateCell(0).SetCellValue("线缆出库信息一览表");
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));

            IRow headRow = sheet.CreateRow(rowIndex++);
            // 在该行中创建单元格  
            headRow.CreateCell(colIndex++).SetCellValue("出库目的地");
            headRow.CreateCell(colIndex++).SetCellValue("电缆型号");
            headRow.CreateCell(colIndex++).SetCellValue("生产厂家");
            headRow.CreateCell(colIndex++).SetCellValue("线轮位置");
            headRow.CreateCell(colIndex++).SetCellValue("是否整轮");
            headRow.CreateCell(colIndex++).SetCellValue("出库长度");
            headRow.CreateCell(colIndex++).SetCellValue("剩余长度");

            //var lookUpLines = resultInfos.Where(m => m.CustomerId == "1").ToLookup(m => new { m.CustomerId });
            var lookUpLines = resultInfos.ToLookup(m => new { m.CustomerId });

            foreach (var lookUpCustomerLine in lookUpLines)
            {//出库目的地
                colIndex = 0;
                IRow row = sheet.CreateRow(rowIndex);
                var customerCell = row.CreateCell(colIndex++);
                customerCell.SetCellValue(lookUpCustomerLine.Key.CustomerId);

                var firstRowIndex = rowIndex;
                var totalRowCount = 0;
                var lookupXingHao = lookUpCustomerLine.ToLookup(m => new { m.XingHao });

                var xinghaoFirst = true;
                foreach (var xinghaoGroup in lookupXingHao)
                {
                    var tmpCount = xinghaoGroup.Count();
                    totalRowCount += tmpCount;
                 
                    if (xinghaoFirst)
                    {
                        xinghaoFirst = false;

                        var xingHaoCell = row.CreateCell(1);
                        xingHaoCell.SetCellValue(xinghaoGroup.Key.XingHao);
                        sheet.AddMergedRegion(new CellRangeAddress(rowIndex, firstRowIndex + totalRowCount - 1, 1, 1));
                    }
                    else
                    {
                        row = sheet.CreateRow(++rowIndex);
                        var xingHaoCell = row.CreateCell(1);
                        xingHaoCell.SetCellValue(xinghaoGroup.Key.XingHao);
                        sheet.AddMergedRegion(new CellRangeAddress(rowIndex, firstRowIndex + totalRowCount - 1, 1, 1));
                    }

                    var brandFirst = true;
                    //var newRowIndex = firstRowIndex + 1;
                    var tmpInx = 0;
                    var tmpList = xinghaoGroup.OrderBy(p => p.PaiHao).ThenBy(m => m.LunHao).ToList();
                    foreach (var lineResultInfo in tmpList)
                    {
                        tmpInx++;
                        colIndex = 2;
                        if (brandFirst)
                        {
                            row.CreateCell(colIndex++).SetCellValue(lineResultInfo.Brand);
                            row.CreateCell(colIndex++).SetCellValue($"{lineResultInfo.PaiHao}排 {lineResultInfo.LunHao}轮");
                            row.CreateCell(colIndex++).SetCellValue(lineResultInfo.IsWheel);
                            row.CreateCell(colIndex++).SetCellValue(lineResultInfo.Length.GetValueOrDefault());
                            row.CreateCell(colIndex++).SetCellValue(lineResultInfo.LeaveLength.GetValueOrDefault());
                            brandFirst = false;
                        }
                        else
                        {
                            //if (tmpInx != tmpCount)
                            //    rowIndex++;
                            var newRow = sheet.CreateRow(++rowIndex);

                            newRow.CreateCell(colIndex++).SetCellValue(lineResultInfo.Brand);
                            newRow.CreateCell(colIndex++).SetCellValue($"{lineResultInfo.PaiHao}排 {lineResultInfo.LunHao}轮");
                            newRow.CreateCell(colIndex++).SetCellValue(lineResultInfo.IsWheel);
                            newRow.CreateCell(colIndex++).SetCellValue(lineResultInfo.Length.GetValueOrDefault());
                            newRow.CreateCell(colIndex++).SetCellValue(lineResultInfo.LeaveLength.GetValueOrDefault());
                        }
                    }
                }

                sheet.AddMergedRegion(new CellRangeAddress(firstRowIndex, firstRowIndex + totalRowCount - 1, 0, 0));
                rowIndex = firstRowIndex + totalRowCount;
            }

            var mapper = new Mapper(wkbook);

            mapper.Save(filePath);



        }


    }
}
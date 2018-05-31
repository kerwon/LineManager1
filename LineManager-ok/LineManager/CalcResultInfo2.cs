using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineManager
{
    public partial class CalcResultInfo2
    {
        public static List<LineResultInfo> Calc(List<KuCunInfo> kuCunInfos, List<FaHuoTaskItem> faHuoTaskItems, Condition condition)
        {
            var result = new List<LineResultInfo>();

            kuCunInfos.RemoveAll(m => m.Length == 0);

            //var lookupFaHuoInfos = faHuoTaskItems.Where(m => m.XingHao.Equals("500014666")).ToLookup(m => new { m.XingHao });
            var lookupFaHuoInfos = faHuoTaskItems.ToLookup(m => new { m.XingHao });

            foreach (var fahuoXingHaoGroup in lookupFaHuoInfos)
            {// 按发货指定型号分组

                var faHuoTaskItemList = fahuoXingHaoGroup.ToList();

                var allXingHaoKuCunList = kuCunInfos.Where(m => m.XingHao.Equals(fahuoXingHaoGroup.Key.XingHao))
                                                    .OrderBy(m => m.PaiHao).ThenBy(m => m.LunHao)//.OrderBy(m => m.IsWheel) //.OrderBy(m => m.IsWheel?.Length ?? 0)//优先截取的线缆
                                                    .ThenBy(m => m.Length)
                                                    .ToList();

                //按型号遍历客服发货列表
                foreach (var taskItem in faHuoTaskItemList)
                {
                    allXingHaoKuCunList.RemoveAll(m => m.Length == 0);
                    //var isAllWheelFlag = allXingHaoKuCunList.All(m => isWheel.Equals(m.IsWheel));

                    var totalLength = allXingHaoKuCunList.Sum(m => m.Length);

                    var noWheelKuCunList = allXingHaoKuCunList.Where(m => !isWheel.Equals(m.IsWheel)).ToList();
                    var noWheelKuCunTotalLength = noWheelKuCunList.Sum(m => m.Length);

                    var wheelKuCunList = allXingHaoKuCunList.Where(m => isWheel.Equals(m.IsWheel)).ToList();
                    //var wheelTotalLength = wheelKuCunList.Sum(m => m.Length);

                    var handled = false;

                    #region  库存总量(totalLength)不满足此次出货长度,不予出货

                    if (taskItem.Length > totalLength)
                    {
                        result.Add(new LineResultInfo()
                        {
                            CustomerId = taskItem.CustomerId,
                            XingHao = taskItem.XingHao,
                            Remark = "库存总量不满足此次出货长度,不予出货"
                        });
                        continue;
                    }

                    #endregion 库存总量(totalLength)不满足此次出货长度,不予出货

                    var tmpWheelResultList = new List<LineResultInfo>();
                    var tmpWheelDelKucunInfos = new List<KuCunInfo>();
                    var joinFaHuoTaskLength = 0;

                    if (taskItem.Length == noWheelKuCunTotalLength)
                    {// 所有剩余非整轮长度之和 刚好满足出库长度, 直接进行出库操作
                        //noWheelKuCunList.ForEach(noWheelItem =>
                        //{
                        //    result.Add(MapperSingleResultInfo(noWheelItem, taskItem.CustomerId));
                        //});
                        //result.AddRange(MatchedResultInfo(noWheelKuCunList,));
                        MatchedResultInfo(noWheelKuCunList, result, taskItem.CustomerId, false, "剩余全部出库");
                        //noWheelKuCunList.Clear();
                        noWheelKuCunList.RemoveAll(item => true);
                        //allXingHaoKuCunList.RemoveAll(kuCunInfo => tmpDelKucunInfos.Contains(kuCunInfo));
                        //break;
                        continue;
                    }

                    while (true)
                    {
                        if (handled)
                            break;

                        var tmpLeaveFaHuoLength = taskItem.Length - joinFaHuoTaskLength;

                        if (tmpLeaveFaHuoLength > 0)
                        {// 需要加入新的整轮
                            var tmpFaHuoTaskItem = wheelKuCunList.FirstOrDefault(m => !tmpWheelDelKucunInfos.Contains(m));

                            if (tmpFaHuoTaskItem != null)
                            {
                                if (tmpLeaveFaHuoLength == tmpFaHuoTaskItem.Length)
                                {// 刚好满足
                                    handled = true;
                                    result.Add(new LineResultInfo()
                                    {
                                        CustomerId = taskItem.CustomerId,
                                        XingHao = taskItem.XingHao,
                                        Brand = tmpFaHuoTaskItem.Brand,
                                        IsFirstCut = true,
                                        IsWheel = noWheel,
                                        Length = tmpLeaveFaHuoLength,//taskItem.Length,
                                        LeaveLength = tmpFaHuoTaskItem.Length - tmpLeaveFaHuoLength,//taskItem.Length,
                                        LunHao = tmpFaHuoTaskItem.LunHao,
                                        PaiHao = tmpFaHuoTaskItem.PaiHao,
                                        Remark = string.Empty//$"第一次截取长度{taskItem.Length}"
                                    });

                                    tmpFaHuoTaskItem.Length -= tmpLeaveFaHuoLength;
                                    //tmpFaHuoTaskItem.IsWheel = noWheel;
                                    //tmpWheelDelKucunInfos.Add(tmpFaHuoTaskItem);
                                    //break;
                                }
                                else if (tmpLeaveFaHuoLength > tmpFaHuoTaskItem.Length)
                                {// 还需要加入 // 不满足出库,需继续加入整轮线缆
                                    tmpWheelResultList.Add(MapperSingleResultInfo(tmpFaHuoTaskItem, taskItem.CustomerId, false, "整轮线缆出库"));
                                    tmpWheelDelKucunInfos.Add(tmpFaHuoTaskItem);
                                    joinFaHuoTaskLength += tmpFaHuoTaskItem.Length;
                                }
                                else if (tmpLeaveFaHuoLength < tmpFaHuoTaskItem.Length)
                                {// 需出库长度 小于整轮长度 
                                    if (tmpLeaveFaHuoLength == noWheelKuCunTotalLength)
                                    {// 剩余刚好
                                        handled = true;

                                        MatchedResultInfo(noWheelKuCunList, result, taskItem.CustomerId, false, "剩余部分完全出库");
                                        //noWheelKuCunList.RemoveAll(m => true);
                                        tmpWheelDelKucunInfos.AddRange(noWheelKuCunList);
                                    }
                                    else if (tmpLeaveFaHuoLength > noWheelKuCunTotalLength)
                                    {// 直接裁剪 | 需要裁剪
                                        if (tmpFaHuoTaskItem.Length >= tmpLeaveFaHuoLength + condition.MinLeaveLength)
                                        {//满足 需要截取
                                            handled = true;
                                            result.Add(new LineResultInfo()
                                            {
                                                CustomerId = taskItem.CustomerId,
                                                XingHao = taskItem.XingHao,
                                                Brand = tmpFaHuoTaskItem.Brand,
                                                IsFirstCut = true,
                                                IsWheel = noWheel,
                                                Length = tmpLeaveFaHuoLength,//taskItem.Length,
                                                LeaveLength = tmpFaHuoTaskItem.Length - tmpLeaveFaHuoLength,//taskItem.Length,
                                                LunHao = tmpFaHuoTaskItem.LunHao,
                                                PaiHao = tmpFaHuoTaskItem.PaiHao,
                                                Remark = $"第一次截取长度{tmpLeaveFaHuoLength}"
                                            });
                                            tmpFaHuoTaskItem.Length -= tmpLeaveFaHuoLength;//taskItem.Length;
                                            tmpFaHuoTaskItem.IsWheel = noWheel;
                                            //break;
                                        }
                                        else
                                        { // 需要重新选取 满足的裁剪对象
                                            var tmpOthreFaHuoTaskItem = wheelKuCunList.FirstOrDefault(m => !tmpWheelDelKucunInfos.Contains(m) && m.Length >= tmpLeaveFaHuoLength + condition.MinLeaveLength);

                                            if (tmpOthreFaHuoTaskItem != null)
                                            {////满足 需要截取
                                                handled = true;
                                                result.Add(new LineResultInfo()
                                                {
                                                    CustomerId = taskItem.CustomerId,
                                                    XingHao = taskItem.XingHao,
                                                    Brand = tmpFaHuoTaskItem.Brand,
                                                    IsFirstCut = true,
                                                    IsWheel = noWheel,
                                                    Length = tmpLeaveFaHuoLength,//taskItem.Length,
                                                    LeaveLength = tmpOthreFaHuoTaskItem.Length - tmpLeaveFaHuoLength,//taskItem.Length,
                                                    LunHao = tmpOthreFaHuoTaskItem.LunHao,
                                                    PaiHao = tmpOthreFaHuoTaskItem.PaiHao,
                                                    Remark = $"第一次截取长度{tmpLeaveFaHuoLength}"
                                                });
                                                tmpOthreFaHuoTaskItem.Length -= tmpLeaveFaHuoLength;//taskItem.Length;
                                                tmpOthreFaHuoTaskItem.IsWheel = noWheel;
                                                //break;
                                            }
                                            else
                                            {
                                                //为空 , 无法满足出库长度
                                                //handled = true;
                                                //result.Add(MapperSingleBaseInfo(allXingHaoKuCunList.FirstOrDefault(), taskItem, true));
                                                break;
                                            }
                                        }
                                    }
                                    else // tmpLeaveFaHuoLength < noWheelKuCunTotalLength    tmpLeaveFaHuoLength < tmpFaHuoTaskItem.Length
                                    {// 需出库长度 小于整轮长度
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //tmpFaHuoTaskItem 为空 , 无法满足出库长度
                                //handled = true;
                                //tmpWheelDelKucunInfos.Clear();
                                //result.Add(MapperSingleBaseInfo(allXingHaoKuCunList.FirstOrDefault(), taskItem, true));
                                break;
                            }
                        }
                        else
                        {// 需要判定是否满足 所有剩余非整轮长度之和 >=  剩余出库长度  需计算是否不需要裁剪 就满足出库长度
                            break;
                        }
                    }

                    if (handled)
                    {
                        MatchedResultInfo(tmpWheelDelKucunInfos, result, taskItem.CustomerId);
                        allXingHaoKuCunList.RemoveAll(m => tmpWheelDelKucunInfos.Contains(m));
                        continue;
                    }

                    // 剩余出库长度  所有剩余非整轮长度之和 >= 剩余出库长度  需计算是否不需要裁剪 就满足出库长度 
                    var tmpLeaveTaskItemLength = taskItem.Length - joinFaHuoTaskLength;

                    var isMatch = false;
                    var allNoWheelXingHangArr = noWheelKuCunList.ToArray();  //allXingHaoList.ToArray();

                    List<Tuple<int, KuCunInfo[]>> allCombinationList = new List<Tuple<int, KuCunInfo[]>>();
                    List<Tuple<int, KuCunInfo[]>> lessCombinationNoWheelList = new List<Tuple<int, KuCunInfo[]>>();
                    List<Tuple<int, KuCunInfo[]>> gEqualsCombinationNoWheelList = new List<Tuple<int, KuCunInfo[]>>();

                    Tuple<int, KuCunInfo[]> lessNoWheelItem = null;
                    Tuple<int, KuCunInfo[]> gEqualNoWheelItem = null;

                    Tuple<int, KuCunInfo[]> lessPartNoWheelItem = null;
                    Tuple<int, KuCunInfo[]> gEqualPartNoWheelItem = null;
                    var sumTotalCount = 0;
                    if (condition.Length > 0)
                    {
                        if (condition.LessLengthCount > 0)
                        {
                            var lessAllNoWheelList = allNoWheelXingHangArr.Where(m => m.Length < condition.Length).ToArray();
                            var minCount = Math.Min(lessAllNoWheelList.Count(), condition.LessLengthCount);
                            sumTotalCount += minCount;
                            var finalLeaveLength = int.MaxValue;

                            for (int i = 1; i <= minCount; i++)
                            {
                                if (isMatch) break;

                                var tmpList = PermutationAndCombination<KuCunInfo>.GetCombination(lessAllNoWheelList, i);
                                foreach (var kuCunItem in tmpList)
                                {
                                    var combinaItemList = kuCunItem.ToList();
                                    var total = combinaItemList.Sum(item => item.Length);
                                    var tuplue = new Tuple<int, KuCunInfo[]>(total, kuCunItem);
                                    lessCombinationNoWheelList.Add(tuplue);

                                    if (tmpLeaveTaskItemLength == total)
                                    {//刚好匹配 不需要截取的半轮或半轮组合视为整轮
                                        isMatch = true;
                                        MatchedResultInfo(combinaItemList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                        tmpWheelDelKucunInfos.AddRange(combinaItemList);
                                        break;
                                    }

                                    var tmpFinalLeaveLength = tmpLeaveTaskItemLength - total;
                                    if (tmpFinalLeaveLength > 0 && tmpFinalLeaveLength < finalLeaveLength)
                                    {
                                        finalLeaveLength = tmpFinalLeaveLength;
                                        lessPartNoWheelItem = tuplue;
                                    }
                                }
                            }
                        }

                        if (condition.GEqualsLengthCount > 0)
                        {
                            var gEqualsAllNoWheelList = allNoWheelXingHangArr.Where(m => m.Length >= condition.Length).ToArray();
                            var minCount = Math.Min(gEqualsAllNoWheelList.Count(), condition.GEqualsLengthCount);
                            sumTotalCount += minCount;
                            var finalLeaveLength = int.MaxValue;

                            for (int i = 1; i <= minCount; i++)
                            {
                                if (isMatch) break;

                                var tmpList = PermutationAndCombination<KuCunInfo>.GetCombination(gEqualsAllNoWheelList, i);
                                foreach (var kuCunItem in tmpList)
                                {
                                    var combinaItemList = kuCunItem.ToList();
                                    var total = combinaItemList.Sum(item => item.Length);
                                    var tuplue = new Tuple<int, KuCunInfo[]>(total, kuCunItem);
                                    gEqualsCombinationNoWheelList.Add(tuplue);

                                    if (tmpLeaveTaskItemLength == total)
                                    {//刚好匹配 不需要截取的半轮或半轮组合视为整轮
                                        isMatch = true;
                                        MatchedResultInfo(combinaItemList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                        tmpWheelDelKucunInfos.AddRange(combinaItemList);
                                        break;
                                    }

                                    var tmpFinalLeaveLength = tmpLeaveTaskItemLength - total;
                                    if (tmpFinalLeaveLength > 0 && tmpFinalLeaveLength < finalLeaveLength)
                                    {
                                        finalLeaveLength = tmpFinalLeaveLength;
                                        gEqualPartNoWheelItem = tuplue;
                                    }
                                }
                            }
                        }

                        if (!isMatch)
                        {
                            if (lessCombinationNoWheelList.Any() && gEqualsCombinationNoWheelList.Any())
                            {// 两端都没匹配到, 需要校验配对 
                                //var existMatch = false;
                                var finalLeaveLength = int.MaxValue;

                                foreach (var lessTuple in lessCombinationNoWheelList)
                                {
                                    if (isMatch) break;

                                    foreach (var gEqualTuple in gEqualsCombinationNoWheelList)
                                    {
                                        var tmpFinalLeaveLength = tmpLeaveTaskItemLength - lessTuple.Item1 - gEqualTuple.Item1;
                                        if (tmpFinalLeaveLength == 0)
                                        {// 刚好匹配
                                            //existMatch = true;
                                            isMatch = true;
                                            lessNoWheelItem = lessTuple;
                                            gEqualNoWheelItem = gEqualTuple;

                                            var lessList = lessNoWheelItem.Item2.ToList();
                                            MatchedResultInfo(lessList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                            tmpWheelDelKucunInfos.AddRange(lessList);

                                            var gEqualList = gEqualNoWheelItem.Item2.ToList();
                                            MatchedResultInfo(gEqualList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                            tmpWheelDelKucunInfos.AddRange(gEqualList);

                                            lessNoWheelItem = null;
                                            gEqualNoWheelItem = null;
                                        }
                                        else if (tmpFinalLeaveLength < 0)
                                        {// 不匹配 ,继续下次匹配
                                            continue;
                                        }
                                        else if (tmpFinalLeaveLength > 0)
                                        {
                                            if (tmpFinalLeaveLength < finalLeaveLength)
                                            {
                                                //existMatch = true;
                                                lessNoWheelItem = lessTuple;
                                                gEqualNoWheelItem = gEqualTuple;
                                                finalLeaveLength = tmpFinalLeaveLength;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (!isMatch)
                        {
                            var finalList = new List<Tuple<int, KuCunInfo[]>>();
                            var finalLength = int.MaxValue;
                            //var lessPartLength = int.MaxValue;
                            if (lessPartNoWheelItem != null)
                            {
                                //lessPartLength = tmpLeaveTaskItemLength - lessPartNoWheelItem.Item1;
                                var tmpPartLength = tmpLeaveTaskItemLength - lessPartNoWheelItem.Item1;
                                if (tmpPartLength < finalLength)
                                {
                                    finalLength = tmpPartLength;
                                    finalList.Clear();
                                    finalList.Add(lessPartNoWheelItem);
                                }
                            }

                            //var gEquealPartLength = int.MaxValue;
                            if (gEqualPartNoWheelItem != null)
                            {
                                //gEquealPartLength = tmpLeaveTaskItemLength - gEqualPartNoWheelItem.Item1;
                                var tmpPartLength = tmpLeaveTaskItemLength - gEqualPartNoWheelItem.Item1;
                                if (tmpPartLength < finalLength)
                                {
                                    finalLength = tmpPartLength;
                                    finalList.Clear();
                                    finalList.Add(gEqualPartNoWheelItem);
                                }
                            }

                            //var twoPartLength = int.MaxValue;
                            if (lessNoWheelItem != null && gEqualNoWheelItem != null)
                            {//两边部分匹配 
                                //twoPartLength = tmpLeaveTaskItemLength - lessNoWheelItem.Item1 -gEqualNoWheelItem.Item1;
                                var tmpPartLength = tmpLeaveTaskItemLength - lessNoWheelItem.Item1 - gEqualNoWheelItem.Item1;
                                if (tmpPartLength < finalLength)
                                {
                                    finalLength = tmpPartLength;
                                    finalList.Clear();
                                    finalList.Add(lessNoWheelItem);
                                    finalList.Add(gEqualNoWheelItem);
                                }
                            }

                            if (finalList.Any())
                            {
                                var finalKuCunList = finalList.SelectMany(m => m.Item2).OrderBy(m => m.Length).ToList();

                                var firstItem = finalKuCunList.FirstOrDefault();
                                var rMinItem = finalKuCunList.FirstOrDefault(m => m.Length >= condition.Length);

                                var tmpLeaveLength = tmpLeaveTaskItemLength - finalKuCunList.Sum(m => m.Length);
                                var lessFinalKuCunList = finalKuCunList.Where(m => m.Length <= condition.Length);
                                var lessFinalKuCunCount = lessFinalKuCunList.Count();
                                var gthanFinalKuCunList = finalKuCunList.Where(m => m.Length > condition.Length);
                                var gthanFinalKuCunCount = gthanFinalKuCunList.Count();
                                if (finalKuCunList.Count >= condition.LessLengthCount + condition.GEqualsLengthCount)
                                {// 组合个数已达上限 , 取其一边 , 需截取 
                                    if (tmpLeaveLength + firstItem.Length <= condition.Length)
                                    {
                                        var needJoinList = finalKuCunList.Skip(1).ToList();
                                        joinFaHuoTaskLength += needJoinList.Sum(m => m.Length);
                                        MatchedResultInfo(needJoinList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                        tmpWheelDelKucunInfos.AddRange(needJoinList);
                                    }
                                    else
                                    {
                                        finalKuCunList.Remove(rMinItem);
                                        joinFaHuoTaskLength += finalKuCunList.Sum(m => m.Length);
                                        MatchedResultInfo(finalKuCunList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                        tmpWheelDelKucunInfos.AddRange(finalKuCunList);
                                    }
                                }
                                else
                                {
                                    var useAll = false;
                                    if (tmpLeaveLength <= condition.Length)
                                    {// 存在 小端 未都全部用完 或 大端 未都全部用完
                                        if (lessFinalKuCunCount < condition.LessLengthCount)
                                        {// 小端 未用完 , 剩余 截取
                                            useAll = true;
                                        }
                                        else
                                        {// 小端全用完
                                            if (tmpLeaveLength + firstItem.Length <= condition.Length)
                                            {// 剩余出库长度 满足小端 条件
                                                var needJoinList = finalKuCunList.Skip(1).ToList();
                                                joinFaHuoTaskLength += needJoinList.Sum(m => m.Length);
                                                MatchedResultInfo(needJoinList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                                tmpWheelDelKucunInfos.AddRange(needJoinList);
                                            }
                                            else
                                            {
                                                if (gthanFinalKuCunCount < condition.GEqualsLengthCount)
                                                {// 大端 未用完
                                                    var needJoinList = finalKuCunList.Skip(1).ToList();
                                                    joinFaHuoTaskLength += needJoinList.Sum(m => m.Length);
                                                    MatchedResultInfo(needJoinList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                                    tmpWheelDelKucunInfos.AddRange(needJoinList);
                                                }
                                                else
                                                {// 大端已用完
                                                    finalKuCunList.Remove(rMinItem);
                                                    joinFaHuoTaskLength += finalKuCunList.Sum(m => m.Length);
                                                    MatchedResultInfo(finalKuCunList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                                    tmpWheelDelKucunInfos.AddRange(finalKuCunList);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {// 只用看 大端
                                        if (gthanFinalKuCunCount < condition.GEqualsLengthCount)
                                        {// 大端 未用完
                                            useAll = true;
                                        }
                                        else
                                        {// 大端已用完
                                            finalKuCunList.Remove(rMinItem);
                                            joinFaHuoTaskLength += finalKuCunList.Sum(m => m.Length);
                                            MatchedResultInfo(finalKuCunList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                            tmpWheelDelKucunInfos.AddRange(finalKuCunList);

                                        }
                                    }
                                    if (useAll)
                                    {
                                        var needJoinList = finalKuCunList.ToList();
                                        joinFaHuoTaskLength += needJoinList.Sum(m => m.Length);
                                        MatchedResultInfo(needJoinList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                        tmpWheelDelKucunInfos.AddRange(needJoinList);

                                    }
                                }
                                #region 

                                //if (tmpLeaveLength <= condition.Length)
                                //{ 
                                //    if (tmpLeaveLength + firstItem.Length <= condition.Length)
                                //    {//left
                                //        var needJoinList = finalKuCunList.Skip(1).ToList();
                                //        MatchedResultInfo(needJoinList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                //        tmpWheelDelKucunInfos.AddRange(needJoinList);
                                //        joinFaHuoTaskLength += needJoinList.Sum(m => m.Length);
                                //    }
                                //    else
                                //    {//
                                //        finalKuCunList.Remove(rMinItem);
                                //        MatchedResultInfo(finalKuCunList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                //        tmpWheelDelKucunInfos.AddRange(finalKuCunList);
                                //        joinFaHuoTaskLength += finalKuCunList.Sum(m => m.Length);
                                //    }
                                //}
                                //else
                                //{
                                //    finalKuCunList.Remove(rMinItem);
                                //    MatchedResultInfo(finalKuCunList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                //    tmpWheelDelKucunInfos.AddRange(finalKuCunList);
                                //    joinFaHuoTaskLength += finalKuCunList.Sum(m => m.Length);
                                //}  
                                #endregion
                            }
                        }
                    }
                    else
                    {
                        #region 保留原来逻辑

                        for (int i = 1; i <= allNoWheelXingHangArr.Length; i++)
                        {
                            var tmpList = PermutationAndCombination<KuCunInfo>.GetCombination(allNoWheelXingHangArr, i);// 从小到大组合相加 
                            foreach (var kuCunItem in tmpList)
                            {
                                var combinaItemList = kuCunItem.ToList();
                                var total = combinaItemList.Sum(item => item.Length);
                                allCombinationList.Add(new Tuple<int, KuCunInfo[]>(total, kuCunItem));

                                if (total == tmpLeaveTaskItemLength)
                                {//刚好匹配 不需要截取的半轮或半轮组合视为整轮
                                    isMatch = true;
                                    MatchedResultInfo(combinaItemList, tmpWheelResultList, taskItem.CustomerId, false, "剩余部分完全出库");
                                    tmpWheelDelKucunInfos.AddRange(combinaItemList);
                                    break;
                                }
                            }

                            if (isMatch)
                            {
                                break;
                            }
                        }

                        #endregion
                    }

                    if (isMatch)
                    {// 刚好 满足出库条件  部分剩余非整轮长度之和 = 剩余出库长度  不需要裁剪 直接满足出库长度 
                        result.AddRange(tmpWheelResultList);
                        allXingHaoKuCunList.RemoveAll(kuCunInfo => tmpWheelDelKucunInfos.Contains(kuCunInfo));
                        //continue;
                    }
                    else
                    {// 需要从整轮中截取
                        tmpLeaveTaskItemLength = taskItem.Length - joinFaHuoTaskLength;
                        var tmpFaHuoTaskItem = allNoWheelXingHangArr.FirstOrDefault(m => !tmpWheelDelKucunInfos.Contains(m) && m.Length >= tmpLeaveTaskItemLength + condition.MinLeaveLength);

                        if (tmpFaHuoTaskItem != null)
                        {//满足 需要截取
                            handled = true;
                            result.Add(new LineResultInfo()
                            {
                                CustomerId = taskItem.CustomerId,
                                XingHao = taskItem.XingHao,
                                Brand = tmpFaHuoTaskItem.Brand,
                                IsFirstCut = true,
                                IsWheel = noWheel,
                                Length = tmpLeaveTaskItemLength,//taskItem.Length,
                                LeaveLength = tmpFaHuoTaskItem.Length - tmpLeaveTaskItemLength,//taskItem.Length,
                                LunHao = tmpFaHuoTaskItem.LunHao,
                                PaiHao = tmpFaHuoTaskItem.PaiHao,
                                Remark = $"第一次截取长度{tmpLeaveTaskItemLength}"
                            });
                            tmpFaHuoTaskItem.Length -= tmpLeaveTaskItemLength;//taskItem.Length;
                            tmpFaHuoTaskItem.IsWheel = noWheel;
                            result.AddRange(tmpWheelResultList);
                            allXingHaoKuCunList.RemoveAll(kuCunInfo => tmpWheelDelKucunInfos.Contains(kuCunInfo));
                            //continue;
                        }
                        else
                        { // 需要重新选取 满足的裁剪对象
                            var tmpOtherFaHuoTaskItem = wheelKuCunList.FirstOrDefault(m => !tmpWheelDelKucunInfos.Contains(m) && m.Length >= tmpLeaveTaskItemLength + condition.MinLeaveLength);
                            if (tmpOtherFaHuoTaskItem != null)
                            {////满足 需要截取
                                handled = true;
                                result.Add(new LineResultInfo()
                                {
                                    CustomerId = taskItem.CustomerId,
                                    XingHao = taskItem.XingHao,
                                    Brand = tmpOtherFaHuoTaskItem.Brand,
                                    IsFirstCut = true,
                                    IsWheel = noWheel,
                                    Length = tmpLeaveTaskItemLength,//taskItem.Length,
                                    LeaveLength = tmpOtherFaHuoTaskItem.Length - tmpLeaveTaskItemLength,//taskItem.Length,
                                    LunHao = tmpOtherFaHuoTaskItem.LunHao,
                                    PaiHao = tmpOtherFaHuoTaskItem.PaiHao,
                                    Remark = $"第一次截取长度{tmpLeaveTaskItemLength}"
                                });
                                tmpOtherFaHuoTaskItem.Length -= tmpLeaveTaskItemLength;//taskItem.Length;
                                tmpOtherFaHuoTaskItem.IsWheel = noWheel;
                                result.AddRange(tmpWheelResultList);
                                allXingHaoKuCunList.RemoveAll(kuCunInfo => tmpWheelDelKucunInfos.Contains(kuCunInfo));
                                //continue;
                            }
                            else
                            {
                                //为空 , 无法满足出库长度
                                handled = true;
                                result.Add(MapperSingleBaseInfo(allXingHaoKuCunList.FirstOrDefault(), taskItem, true));
                                //continue;
                            }
                        }
                    }
                }
            }
             
            return result;
        }


        private static void MatchedResultInfo(List<KuCunInfo> list, List<LineResultInfo> result, string customerId, bool isFirstCut = false, string remark = "")
        {
            list.ForEach(m =>
            {
                result.Add(new LineResultInfo()
                {
                    Brand = m.Brand,
                    CustomerId = customerId,//taskItem.CustomerId,
                    IsWheel = m.IsWheel,
                    LeaveLength = 0,  // 全部出库
                    Length = m.Length,
                    LunHao = m.LunHao,
                    PaiHao = m.PaiHao,
                    XingHao = m.XingHao,
                    IsFirstCut = isFirstCut,
                    Remark = remark,
                });

                m.Length = 0;
            });
            //list.Clear();
        }

    }


    public partial class CalcResultInfo2
    {


        private static string isWheel = "是";

        private static string noWheel = "截取";



        /// <summary>
        /// IsWheel,IsFirstCut,Length,LeaveLength,Remark 重新赋值
        /// </summary>
        /// <param name="kuCunInfo"></param>
        /// <param name="faHuoTaskItem"></param>
        /// <param name="isInit"></param>
        /// <returns></returns>
        private static LineResultInfo MapperSingleBaseInfo(KuCunInfo kuCunInfo, FaHuoTaskItem faHuoTaskItem, bool isInit)
        {
            if (kuCunInfo == null)
                return new LineResultInfo();

            return new LineResultInfo()
            {
                CustomerId = faHuoTaskItem.CustomerId,
                XingHao = faHuoTaskItem.XingHao,

                Brand = kuCunInfo.Brand ?? string.Empty,
                PaiHao = isInit ? 0 : kuCunInfo.PaiHao,
                LunHao = isInit ? 0 : kuCunInfo.LunHao,
                IsWheel = isInit ? string.Empty : kuCunInfo.IsWheel,

                Length = isInit ? 0 : kuCunInfo.Length,
                LeaveLength = 0,

                IsFirstCut = false,
                Remark = string.Empty
            };
        }


        /// <summary>
        /// 整轮线缆出库
        /// </summary>
        /// <param name="kuCunInfo"></param>
        /// <param name="customerId"></param>
        /// <param name="isFirstCut"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private static LineResultInfo MapperSingleResultInfo(KuCunInfo kuCunInfo, string customerId, bool isFirstCut = false, string remark = "")
        {
            return new LineResultInfo()
            {
                Brand = kuCunInfo.Brand,
                CustomerId = customerId,
                IsWheel = kuCunInfo.IsWheel,
                LeaveLength = 0,
                Length = kuCunInfo.Length,
                LunHao = kuCunInfo.LunHao,
                PaiHao = kuCunInfo.PaiHao,
                XingHao = kuCunInfo.XingHao,
                IsFirstCut = isFirstCut,
                Remark = remark,
            };
        }

    }
}

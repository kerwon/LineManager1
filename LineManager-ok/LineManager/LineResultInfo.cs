namespace LineManager
{ 
     
    public class LineResultInfo
    {

        public string  CustomerId { get; set; }



        public string XingHao { get; set; }



        public string Brand { get; set; }




        public int? PaiHao { get; set; }



        public int? LunHao { get; set; }


        /// <summary>
        /// 是否整轮
        /// </summary>
        public string IsWheel { get; set; }


        /// <summary>
        /// 出库长度
        /// </summary>
        public int? Length { get; set; }


        /// <summary>
        /// 剩余长度
        /// </summary>
        public int? LeaveLength { get; set; }


        ///// <summary>
        ///// 有新的线缆,参与截取
        ///// </summary>
        //public  bool JoinNewLine { get; set; }


        /// <summary>
        /// 是否是第一次截取
        /// </summary>
        public bool? IsFirstCut { get; set; }

        public  string Remark { get; set; }
    }
}
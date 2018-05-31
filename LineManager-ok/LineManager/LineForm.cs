using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;



namespace LineManager
{
    public partial class LineForm : Form
    {

        public string KuCunFilePath = string.Empty;

        public string FaHuoTaskFilePath = string.Empty;


        public List<KuCunInfo> KuCunInfoList = null;

        public List<FaHuoTaskItem> FaHuoTaskItemList = null;


        public List<LineResultInfo> LineResultDatas = null;

        public LineForm()
        {
            InitializeComponent();

            this.btnExecResult.Enabled = false;

            btnKcInfo.Enabled = false;
            btnFhInfo.Enabled = false;

            btnKcInfo.Visible = false;
            btnFhInfo.Visible = false;
        }

        #region OK

        private void txtLine_Leave(object sender, EventArgs e)
        {
            var txt = this.txtLine.Text;
            var msg = "线缆截取剩余最小长度";

            if (int.TryParse(txt, out int lineLine))
            {
                if (lineLine <= 0)
                {
                    MessageBox.Show("请输入大于0的正整数", msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("请输入大于0的正整数", msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion OK 


        #region 上传库存/上传发货任务信息


        private void btnUpdateExcel_Click(object sender, EventArgs e)
        {
            var dialog = this.ofdKcExcel.ShowDialog();

            if (dialog == DialogResult.OK)
            {
                var bytes = File.ReadAllBytes(this.ofdKcExcel.FileName);
                KuCunFilePath = ofdKcExcel.FileName;

                using (var ms = new MemoryStream(bytes))
                {
                    var result = ExcelHelper.ReadKuCunInfosExcel(ms);
                    FillDataInfos(result, this.dgvKuCunInfo);

                    this.KuCunInfoList = KuCunInfo.MapCunInfos(result);
                }

                using (var ms = new MemoryStream(bytes))
                {
                    var result = ExcelHelper.ReadFaHuoInfosExcel(ms);
                    var data = FaHuoTaskItemView.MapTaskItems(result);
                    FillDataInfos(data, this.dgvFaHuoTaskInfo);

                    FaHuoTaskItemList = FaHuoTaskItem.MapTaskItems(result);
                }
            }
            CheckExecResult();
        }


        /// <summary>
        /// 上传库存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnKcInfo_Click(object sender, EventArgs e)
        {
            var dialog = this.ofdKcExcel.ShowDialog();

            if (dialog == DialogResult.OK)
            {
                var bytes = File.ReadAllBytes(this.ofdKcExcel.FileName);
                KuCunFilePath = ofdKcExcel.FileName;

                using (var ms = new MemoryStream(bytes))
                {
                    var result = ExcelHelper.ReadKuCunInfosExcel(ms);
                    FillDataInfos(result, this.dgvKuCunInfo);

                    this.KuCunInfoList = KuCunInfo.MapCunInfos(result);
                }
            }
            CheckExecResult();
        }



        /// <summary>
        /// 上传发货任务信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFhInfo_Click(object sender, EventArgs e)
        {
            var dialog = this.ofdFhExcel.ShowDialog();

            if (dialog == DialogResult.OK)
            {
                var bytes = File.ReadAllBytes(this.ofdFhExcel.FileName);
                FaHuoTaskFilePath = ofdFhExcel.FileName;

                using (var ms = new MemoryStream(bytes))
                {
                    var result = ExcelHelper.ReadFaHuoInfosExcel(ms);
                    var data = FaHuoTaskItemView.MapTaskItems(result);
                    FillDataInfos(data, this.dgvFaHuoTaskInfo);

                    FaHuoTaskItemList = FaHuoTaskItem.MapTaskItems(result);
                }
            }

            CheckExecResult();
        }

        private void FillDataInfos(object kuCunInfos, DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();
            dataGridView.DataSource = kuCunInfos;
            dataGridView.ReadOnly = true;

            if (dataGridView == dgvKuCunInfo)
            {
                tabControl.SelectedTab = tabKuCunInfo;
            }
            else if (dataGridView == dgvFaHuoTaskInfo)
            {
                tabControl.SelectedTab = tabFaHuoInfo;
            }
            else if (dataGridView == dvgLinesInfo)
            {
                tabControl.SelectedTab = tabLinesInfo;
            }
        }


        private void CheckExecResult()
        {
            var flag = this.KuCunInfoList != null && KuCunInfoList.Any() && FaHuoTaskItemList != null && this.FaHuoTaskItemList.Any();
            if (flag)
            {
                this.btnExecResult.Enabled = true;
            }
            else
            {
                this.btnExecResult.Enabled = false;
            }

            #region  copy 
            if (this.KuCunInfoList == null || !this.KuCunInfoList.Any())
            {
                MessageBox.Show("请上传库存信息");
                return;
            }

            if (this.FaHuoTaskItemList == null || !this.FaHuoTaskItemList.Any())
            {
                MessageBox.Show("请上传发货任务信息");
                return;
            }

            if (int.TryParse(this.txtLine.Text ?? string.Empty, out int minLength))
            {
                if (minLength <= 0)
                {
                    MessageBox.Show("剩余最小长度必须是大于0的正整数");
                    return;
                }
            }
            else
            {
                MessageBox.Show("请输入有效的剩余最小长度");
                return;
            }

            var ticks = DateTime.Now.Ticks;

            var kuCunfilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"库存信息-{ticks}.txt");
            File.WriteAllText(kuCunfilePath, JsonConvert.SerializeObject(KuCunInfoList));

            var faHuoFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"发货任务信息-{ticks}.txt");
            File.WriteAllText(faHuoFilePath, JsonConvert.SerializeObject(FaHuoTaskItemList));

            var condition = InitCondition();
            var result = CalcResultInfo2.Calc(this.KuCunInfoList, this.FaHuoTaskItemList, condition);
            LineResultDatas = result;
            var data = LineResultInfoView.MapLineResultInfoView(result);
            var finalData = data.OrderBy(m => m.出库目的地).ThenBy(m => m.电缆型号).ThenBy(m => m.线轮位置).ToList();

            FillDataInfos(finalData, dvgLinesInfo);
            #endregion

        }

        #endregion 上传发货任务信息

        private void btnExecResult_Click(object sender, EventArgs e)
        {

            //if (this.KuCunInfoList == null || !this.KuCunInfoList.Any())
            //{
            //    MessageBox.Show("请上传库存信息");
            //    return;
            //}

            //if (this.FaHuoTaskItemList == null || !this.FaHuoTaskItemList.Any())
            //{
            //    MessageBox.Show("请上传发货任务信息");
            //    return;
            //}

            //if (int.TryParse(this.txtLine.Text ?? string.Empty, out int minLength))
            //{
            //    if (minLength <= 0)
            //    {
            //        MessageBox.Show("剩余最小长度必须是大于0的正整数");
            //        return;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("请输入有效的剩余最小长度");
            //    return;
            //}

            var ticks = DateTime.Now.Ticks;
            if (this.LineResultDatas == null && !this.LineResultDatas.Any())
            {
                MessageBox.Show("无计算结果，不予处理");
                return;
            }

            //var kuCunfilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"库存信息-{ticks}.txt");
            //File.WriteAllText(kuCunfilePath, JsonConvert.SerializeObject(KuCunInfoList));

            //var faHuoFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"发货任务信息-{ticks}.txt");
            //File.WriteAllText(faHuoFilePath, JsonConvert.SerializeObject(FaHuoTaskItemList));

            //var result = CalcResultInfo2.Calc(this.KuCunInfoList, this.FaHuoTaskItemList, minLength);
            //LineResultDatas = result;
            //var data = LineResultInfoView.MapLineResultInfoView(result);
            //var finalData = data.OrderBy(m => m.出库目的地).ThenBy(m => m.电缆型号).ToList();

            //FillDataInfos(finalData, dvgLinesInfo);

            var linesFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"线缆出库信息表-{ticks}.txt");
            File.WriteAllText(linesFilePath, JsonConvert.SerializeObject(LineResultDatas));

            this.saveExcelFileDialog.Filter = "Excel文件（*.xlsx）| *.xlsx";
            var dialog = this.saveExcelFileDialog.ShowDialog();
            if (dialog == DialogResult.OK)
            {
                var filePath = this.saveExcelFileDialog.FileName;
                ExcelHelper.DownLoad(this.LineResultDatas, filePath);
            }
        }


        #region  保存参数到文件
        private void LineForm_Load(object sender, EventArgs e)
        {
            try
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Condition.xml");
                var condition = XmlHelper.ReadObjectFromFile<Condition>(filePath);
                if (condition != null)
                {
                    txtLine.Text = (condition.MinLeaveLength) + "";
                    txtLength.Text = (condition.Length) + "";
                    txtUpLimit.Text = (condition.GEqualsLengthCount) + "";
                    txtDownLimit.Text = (condition.LessLengthCount) + "";
                }  
            }
            catch ( Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void LineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Condition.xml");

                var condition = InitCondition();

                XmlHelper.SaveObjectToFile(condition, filePath);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private Condition InitCondition()
        {
            int.TryParse(txtLine.Text, out int leaveLength);

            int.TryParse(txtLength.Text, out int length);

            int.TryParse(txtUpLimit.Text, out int upLimit);

            int.TryParse(txtDownLimit.Text, out int downLimit);

            var condition = new Condition()
            {
                GEqualsLengthCount = upLimit,
                Length = length,
                LessLengthCount = downLimit,
                MinLeaveLength = leaveLength
            };
            return condition;
        }

        #endregion 保存参数到文件


        #region 简单校验
        private void txtLength_Leave(object sender, EventArgs e)
        {
            CheckInputInt(this.txtLength.Text,this.lblLength.Text);
        }

        private void txtUpLimit_Leave(object sender, EventArgs e)
        {
            CheckInputInt(this.txtUpLimit.Text, this.lblUpLimit.Text);
        }

        private void txtDownLimit_Leave(object sender, EventArgs e)
        {  
            CheckInputInt(this.txtDownLimit.Text, this.lblDownLimit.Text);
        }

        private static void CheckInputInt(string txt, string msg)
        { 

            if (int.TryParse(txt, out int lineLine))
            {
                if (lineLine < 0)
                {
                    MessageBox.Show("请输入大于等于0的正整数", msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("请输入大于等于0的正整数", msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion 简单校验
    }
}

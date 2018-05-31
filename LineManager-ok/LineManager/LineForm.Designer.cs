namespace LineManager
{
    partial class LineForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.FuncPannel = new System.Windows.Forms.Panel();
            this.btnUpdateExcel = new System.Windows.Forms.Button();
            this.btnExecResult = new System.Windows.Forms.Button();
            this.btnDownExcel = new System.Windows.Forms.Button();
            this.lblLine = new System.Windows.Forms.Label();
            this.txtLine = new System.Windows.Forms.TextBox();
            this.btnFhInfo = new System.Windows.Forms.Button();
            this.btnKcInfo = new System.Windows.Forms.Button();
            this.ofdKcExcel = new System.Windows.Forms.OpenFileDialog();
            this.ofdFhExcel = new System.Windows.Forms.OpenFileDialog();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabKuCunInfo = new System.Windows.Forms.TabPage();
            this.dgvKuCunInfo = new System.Windows.Forms.DataGridView();
            this.tabFaHuoInfo = new System.Windows.Forms.TabPage();
            this.dgvFaHuoTaskInfo = new System.Windows.Forms.DataGridView();
            this.tabLinesInfo = new System.Windows.Forms.TabPage();
            this.dvgLinesInfo = new System.Windows.Forms.DataGridView();
            this.saveExcelFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.lblLength = new System.Windows.Forms.Label();
            this.txtLength = new System.Windows.Forms.TextBox();
            this.lblUpLimit = new System.Windows.Forms.Label();
            this.txtUpLimit = new System.Windows.Forms.TextBox();
            this.lblDownLimit = new System.Windows.Forms.Label();
            this.txtDownLimit = new System.Windows.Forms.TextBox();
            this.FuncPannel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabKuCunInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKuCunInfo)).BeginInit();
            this.tabFaHuoInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFaHuoTaskInfo)).BeginInit();
            this.tabLinesInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgLinesInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // FuncPannel
            // 
            this.FuncPannel.Controls.Add(this.lblDownLimit);
            this.FuncPannel.Controls.Add(this.txtDownLimit);
            this.FuncPannel.Controls.Add(this.lblUpLimit);
            this.FuncPannel.Controls.Add(this.txtUpLimit);
            this.FuncPannel.Controls.Add(this.lblLength);
            this.FuncPannel.Controls.Add(this.txtLength);
            this.FuncPannel.Controls.Add(this.btnUpdateExcel);
            this.FuncPannel.Controls.Add(this.btnExecResult);
            this.FuncPannel.Controls.Add(this.btnDownExcel);
            this.FuncPannel.Controls.Add(this.lblLine);
            this.FuncPannel.Controls.Add(this.txtLine);
            this.FuncPannel.Controls.Add(this.btnFhInfo);
            this.FuncPannel.Controls.Add(this.btnKcInfo);
            this.FuncPannel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FuncPannel.Location = new System.Drawing.Point(0, 633);
            this.FuncPannel.Name = "FuncPannel";
            this.FuncPannel.Size = new System.Drawing.Size(1196, 109);
            this.FuncPannel.TabIndex = 0;
            // 
            // btnUpdateExcel
            // 
            this.btnUpdateExcel.Location = new System.Drawing.Point(513, 29);
            this.btnUpdateExcel.Name = "btnUpdateExcel";
            this.btnUpdateExcel.Size = new System.Drawing.Size(434, 35);
            this.btnUpdateExcel.TabIndex = 6;
            this.btnUpdateExcel.Text = "上传【库存信息,发货任务信息】";
            this.btnUpdateExcel.UseVisualStyleBackColor = true;
            this.btnUpdateExcel.Click += new System.EventHandler(this.btnUpdateExcel_Click);
            // 
            // btnExecResult
            // 
            this.btnExecResult.Location = new System.Drawing.Point(979, 27);
            this.btnExecResult.Name = "btnExecResult";
            this.btnExecResult.Size = new System.Drawing.Size(205, 38);
            this.btnExecResult.TabIndex = 2;
            this.btnExecResult.Text = "计算并下载【线缆出库信息】";
            this.btnExecResult.UseVisualStyleBackColor = true;
            this.btnExecResult.Click += new System.EventHandler(this.btnExecResult_Click);
            // 
            // btnDownExcel
            // 
            this.btnDownExcel.Location = new System.Drawing.Point(1027, 20);
            this.btnDownExcel.Name = "btnDownExcel";
            this.btnDownExcel.Size = new System.Drawing.Size(155, 38);
            this.btnDownExcel.TabIndex = 5;
            this.btnDownExcel.Text = "下载【线缆出库信息】";
            this.btnDownExcel.UseVisualStyleBackColor = true;
            this.btnDownExcel.Visible = false;
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new System.Drawing.Point(2, 23);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(149, 12);
            this.lblLine.TabIndex = 4;
            this.lblLine.Text = "线缆截取【剩余最少长度】";
            // 
            // txtLine
            // 
            this.txtLine.Location = new System.Drawing.Point(175, 20);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(100, 21);
            this.txtLine.TabIndex = 3;
            this.txtLine.Text = "100";
            this.txtLine.Leave += new System.EventHandler(this.txtLine_Leave);
            // 
            // btnFhInfo
            // 
            this.btnFhInfo.Location = new System.Drawing.Point(667, 20);
            this.btnFhInfo.Name = "btnFhInfo";
            this.btnFhInfo.Size = new System.Drawing.Size(203, 38);
            this.btnFhInfo.TabIndex = 1;
            this.btnFhInfo.Text = "上传【发货任务信息】";
            this.btnFhInfo.UseVisualStyleBackColor = true;
            this.btnFhInfo.Click += new System.EventHandler(this.btnFhInfo_Click);
            // 
            // btnKcInfo
            // 
            this.btnKcInfo.Location = new System.Drawing.Point(442, 20);
            this.btnKcInfo.Name = "btnKcInfo";
            this.btnKcInfo.Size = new System.Drawing.Size(219, 38);
            this.btnKcInfo.TabIndex = 0;
            this.btnKcInfo.Text = "上传【库存信息】";
            this.btnKcInfo.UseVisualStyleBackColor = true;
            this.btnKcInfo.Click += new System.EventHandler(this.btnKcInfo_Click);
            // 
            // ofdKcExcel
            // 
            this.ofdKcExcel.FileName = "上传库存信息";
            this.ofdKcExcel.Filter = "Excel文件(*.xlsx)|*.xlsx";
            // 
            // ofdFhExcel
            // 
            this.ofdFhExcel.FileName = "上传库存信息";
            this.ofdFhExcel.Filter = "Excel文件(*.xlsx)|*.xlsx";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabKuCunInfo);
            this.tabControl.Controls.Add(this.tabFaHuoInfo);
            this.tabControl.Controls.Add(this.tabLinesInfo);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1196, 633);
            this.tabControl.TabIndex = 2;
            // 
            // tabKuCunInfo
            // 
            this.tabKuCunInfo.Controls.Add(this.dgvKuCunInfo);
            this.tabKuCunInfo.Location = new System.Drawing.Point(4, 22);
            this.tabKuCunInfo.Name = "tabKuCunInfo";
            this.tabKuCunInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabKuCunInfo.Size = new System.Drawing.Size(1188, 607);
            this.tabKuCunInfo.TabIndex = 0;
            this.tabKuCunInfo.Text = "库存信息";
            this.tabKuCunInfo.UseVisualStyleBackColor = true;
            // 
            // dgvKuCunInfo
            // 
            this.dgvKuCunInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKuCunInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKuCunInfo.Location = new System.Drawing.Point(3, 3);
            this.dgvKuCunInfo.Name = "dgvKuCunInfo";
            this.dgvKuCunInfo.RowTemplate.Height = 23;
            this.dgvKuCunInfo.Size = new System.Drawing.Size(1182, 601);
            this.dgvKuCunInfo.TabIndex = 0;
            // 
            // tabFaHuoInfo
            // 
            this.tabFaHuoInfo.Controls.Add(this.dgvFaHuoTaskInfo);
            this.tabFaHuoInfo.Location = new System.Drawing.Point(4, 22);
            this.tabFaHuoInfo.Name = "tabFaHuoInfo";
            this.tabFaHuoInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabFaHuoInfo.Size = new System.Drawing.Size(1188, 607);
            this.tabFaHuoInfo.TabIndex = 1;
            this.tabFaHuoInfo.Text = "发货任务信息";
            this.tabFaHuoInfo.UseVisualStyleBackColor = true;
            // 
            // dgvFaHuoTaskInfo
            // 
            this.dgvFaHuoTaskInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFaHuoTaskInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFaHuoTaskInfo.Location = new System.Drawing.Point(3, 3);
            this.dgvFaHuoTaskInfo.Name = "dgvFaHuoTaskInfo";
            this.dgvFaHuoTaskInfo.RowTemplate.Height = 23;
            this.dgvFaHuoTaskInfo.Size = new System.Drawing.Size(1182, 601);
            this.dgvFaHuoTaskInfo.TabIndex = 1;
            // 
            // tabLinesInfo
            // 
            this.tabLinesInfo.Controls.Add(this.dvgLinesInfo);
            this.tabLinesInfo.Location = new System.Drawing.Point(4, 22);
            this.tabLinesInfo.Name = "tabLinesInfo";
            this.tabLinesInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabLinesInfo.Size = new System.Drawing.Size(1188, 607);
            this.tabLinesInfo.TabIndex = 2;
            this.tabLinesInfo.Text = "线缆出库信息表";
            this.tabLinesInfo.UseVisualStyleBackColor = true;
            // 
            // dvgLinesInfo
            // 
            this.dvgLinesInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvgLinesInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvgLinesInfo.Location = new System.Drawing.Point(3, 3);
            this.dvgLinesInfo.Name = "dvgLinesInfo";
            this.dvgLinesInfo.RowTemplate.Height = 23;
            this.dvgLinesInfo.Size = new System.Drawing.Size(1182, 601);
            this.dvgLinesInfo.TabIndex = 2;
            // 
            // saveExcelFileDialog
            // 
            this.saveExcelFileDialog.DefaultExt = "xlsx";
            // 
            // btnUpdateExcel
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(50, 52);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(101, 12);
            this.lblLength.TabIndex = 8;
            this.lblLength.Text = "线缆【限定长度】";
            this.txtLength.Location = new System.Drawing.Point(175, 49);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(100, 21);
            this.txtLength.TabIndex = 7;
            this.txtLength.Text = "500";
            this.txtLength.Leave += new System.EventHandler(this.txtLength_Leave);
            // 
            // lblUpLimit
            // 
            this.lblUpLimit.AutoSize = true;
            this.lblUpLimit.Location = new System.Drawing.Point(287, 26);
            this.lblUpLimit.Name = "lblUpLimit";
            this.lblUpLimit.Size = new System.Drawing.Size(149, 12);
            this.lblUpLimit.TabIndex = 10;
            this.lblUpLimit.Text = "线缆个数【大于限定长度】";
            // 
            // txtUpLimit
            // 
            this.txtUpLimit.Location = new System.Drawing.Point(442, 22);
            this.txtUpLimit.Name = "txtUpLimit";
            this.txtUpLimit.Size = new System.Drawing.Size(65, 21);
            this.txtUpLimit.TabIndex = 9;
            this.txtUpLimit.Text = "2";
            this.txtUpLimit.Leave += new System.EventHandler(this.txtUpLimit_Leave);
            // 
            // lblDownLimit
            // 
            this.lblDownLimit.AutoSize = true;
            this.lblDownLimit.Location = new System.Drawing.Point(287, 52);
            this.lblDownLimit.Name = "lblDownLimit";
            this.lblDownLimit.Size = new System.Drawing.Size(149, 12);
            this.lblDownLimit.TabIndex = 12;
            this.lblDownLimit.Text = "线缆个数【小于限定长度】";
            // 
            // txtDownLimit
            // 
            this.txtDownLimit.Location = new System.Drawing.Point(442, 49);
            this.txtDownLimit.Name = "txtDownLimit";
            this.txtDownLimit.Size = new System.Drawing.Size(65, 21);
            this.txtDownLimit.TabIndex = 11;
            this.txtDownLimit.Text = "3";
            this.txtDownLimit.Leave += new System.EventHandler(this.txtDownLimit_Leave);
            // 
            // LineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1196, 742);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.FuncPannel);
            this.Name = "LineForm";
            this.Text = "线缆库存管理系统";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LineForm_FormClosing);
            this.Load += new System.EventHandler(this.LineForm_Load);
            this.FuncPannel.ResumeLayout(false);
            this.FuncPannel.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabKuCunInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKuCunInfo)).EndInit();
            this.tabFaHuoInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFaHuoTaskInfo)).EndInit();
            this.tabLinesInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dvgLinesInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel FuncPannel;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.TextBox txtLine;
        private System.Windows.Forms.Button btnExecResult;
        private System.Windows.Forms.Button btnFhInfo;
        private System.Windows.Forms.Button btnKcInfo;
        private System.Windows.Forms.OpenFileDialog ofdKcExcel;
        private System.Windows.Forms.OpenFileDialog ofdFhExcel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabKuCunInfo;
        private System.Windows.Forms.DataGridView dgvKuCunInfo;
        private System.Windows.Forms.TabPage tabFaHuoInfo;
        private System.Windows.Forms.DataGridView dgvFaHuoTaskInfo;
        private System.Windows.Forms.TabPage tabLinesInfo;
        private System.Windows.Forms.DataGridView dvgLinesInfo;
        private System.Windows.Forms.Button btnDownExcel;
        private System.Windows.Forms.SaveFileDialog saveExcelFileDialog;
        private System.Windows.Forms.Button btnUpdateExcel;
        private System.Windows.Forms.Label lblDownLimit;
        private System.Windows.Forms.TextBox txtDownLimit;
        private System.Windows.Forms.Label lblUpLimit;
        private System.Windows.Forms.TextBox txtUpLimit;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.TextBox txtLength;
    }
}


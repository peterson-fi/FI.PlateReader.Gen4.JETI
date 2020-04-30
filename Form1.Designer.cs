namespace FI.PlateReader.Gen4.JETI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnStart = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.btnInsertPlate = new System.Windows.Forms.ToolStripButton();
            this.btnEjectPlate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnApplyProtocol = new System.Windows.Forms.ToolStripButton();
            this.btnResetProtocol = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSaveData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.labelClock = new System.Windows.Forms.ToolStripLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripLabel();
            this.labelFixed = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabAssayProtocol = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nudDelay = new System.Windows.Forms.NumericUpDown();
            this.nudScans = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboScanType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxAnalysis = new System.Windows.Forms.GroupBox();
            this.nudWavelengthB = new System.Windows.Forms.NumericUpDown();
            this.nudWavelengthA = new System.Windows.Forms.NumericUpDown();
            this.cboBandB = new System.Windows.Forms.ComboBox();
            this.cboBandA = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.groupBoxEmission = new System.Windows.Forms.GroupBox();
            this.nudIntegration = new System.Windows.Forms.NumericUpDown();
            this.label58 = new System.Windows.Forms.Label();
            this.cboDetector = new System.Windows.Forms.ComboBox();
            this.label45 = new System.Windows.Forms.Label();
            this.groupBoxExcitation = new System.Windows.Forms.GroupBox();
            this.nudCurrent = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cboLed = new System.Windows.Forms.ComboBox();
            this.groupBoxMicroplate = new System.Windows.Forms.GroupBox();
            this.cboPlateFormat = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbColumnWellSelection = new System.Windows.Forms.Label();
            this.lbRowWellSelection = new System.Windows.Forms.Label();
            this.chartPlate = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnWellSelectionAll = new System.Windows.Forms.Button();
            this.btnWellSelectionReset = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tabMeasure = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chartWaveform = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lbLegendMax = new System.Windows.Forms.Label();
            this.lbLegendMin = new System.Windows.Forms.Label();
            this.lbColumn = new System.Windows.Forms.Label();
            this.lbRow = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.chartLegend = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label6 = new System.Windows.Forms.Label();
            this.cboPlotSelection = new System.Windows.Forms.ComboBox();
            this.chartResultMap = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabResults = new System.Windows.Forms.TabPage();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.labelPlateTime = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.labelScanTime = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.labelCurrentScan = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.labelTotalScans = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAutoscale = new System.Windows.Forms.ToolStripButton();
            this.labelRowOffset = new System.Windows.Forms.ToolStripLabel();
            this.labelColumnOffset = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabAssayProtocol.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScans)).BeginInit();
            this.groupBoxAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWavelengthB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWavelengthA)).BeginInit();
            this.groupBoxEmission.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntegration)).BeginInit();
            this.groupBoxExcitation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCurrent)).BeginInit();
            this.groupBoxMicroplate.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPlate)).BeginInit();
            this.tabMeasure.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartWaveform)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartLegend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartResultMap)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStart,
            this.btnStop,
            this.btnInsertPlate,
            this.btnEjectPlate,
            this.toolStripSeparator1,
            this.btnApplyProtocol,
            this.btnResetProtocol,
            this.toolStripSeparator3,
            this.btnSaveData,
            this.toolStripSeparator2,
            this.labelClock,
            this.labelStatus,
            this.labelFixed,
            this.toolStripSeparator4});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStrip.Size = new System.Drawing.Size(2260, 34);
            this.toolStrip.TabIndex = 4;
            this.toolStrip.Text = "toolStrip2";
            // 
            // btnStart
            // 
            this.btnStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(52, 29);
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(53, 33);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnInsertPlate
            // 
            this.btnInsertPlate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnInsertPlate.Image = ((System.Drawing.Image)(resources.GetObject("btnInsertPlate.Image")));
            this.btnInsertPlate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInsertPlate.Name = "btnInsertPlate";
            this.btnInsertPlate.Size = new System.Drawing.Size(103, 33);
            this.btnInsertPlate.Text = "Insert Plate";
            this.btnInsertPlate.Click += new System.EventHandler(this.btnInsertPlate_Click);
            // 
            // btnEjectPlate
            // 
            this.btnEjectPlate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnEjectPlate.Image = ((System.Drawing.Image)(resources.GetObject("btnEjectPlate.Image")));
            this.btnEjectPlate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEjectPlate.Name = "btnEjectPlate";
            this.btnEjectPlate.Size = new System.Drawing.Size(95, 33);
            this.btnEjectPlate.Text = "Eject Plate";
            this.btnEjectPlate.Click += new System.EventHandler(this.btnEjectPlate_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            // 
            // btnApplyProtocol
            // 
            this.btnApplyProtocol.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnApplyProtocol.Image = ((System.Drawing.Image)(resources.GetObject("btnApplyProtocol.Image")));
            this.btnApplyProtocol.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApplyProtocol.Name = "btnApplyProtocol";
            this.btnApplyProtocol.Size = new System.Drawing.Size(135, 33);
            this.btnApplyProtocol.Text = "Apply Protocol";
            this.btnApplyProtocol.Click += new System.EventHandler(this.btnApplyProtocol_Click);
            // 
            // btnResetProtocol
            // 
            this.btnResetProtocol.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnResetProtocol.Image = ((System.Drawing.Image)(resources.GetObject("btnResetProtocol.Image")));
            this.btnResetProtocol.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResetProtocol.Name = "btnResetProtocol";
            this.btnResetProtocol.Size = new System.Drawing.Size(130, 33);
            this.btnResetProtocol.Text = "Reset Protocol";
            this.btnResetProtocol.ToolTipText = "Reset Protocol";
            this.btnResetProtocol.Click += new System.EventHandler(this.btnResetProtocol_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 38);
            // 
            // btnSaveData
            // 
            this.btnSaveData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnSaveData.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveData.Image")));
            this.btnSaveData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(95, 33);
            this.btnSaveData.Text = "Save Data";
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 38);
            // 
            // labelClock
            // 
            this.labelClock.Name = "labelClock";
            this.labelClock.Size = new System.Drawing.Size(101, 33);
            this.labelClock.Text = "Clock Label";
            // 
            // labelStatus
            // 
            this.labelStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(106, 33);
            this.labelStatus.Text = "Status Label";
            // 
            // labelFixed
            // 
            this.labelFixed.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.labelFixed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFixed.Name = "labelFixed";
            this.labelFixed.Size = new System.Drawing.Size(55, 33);
            this.labelFixed.Text = "Task:";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 38);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabAssayProtocol);
            this.tabControl.Controls.Add(this.tabMeasure);
            this.tabControl.Controls.Add(this.tabResults);
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(19, 60);
            this.tabControl.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(2640, 1124);
            this.tabControl.TabIndex = 5;
            // 
            // tabAssayProtocol
            // 
            this.tabAssayProtocol.Controls.Add(this.groupBox3);
            this.tabAssayProtocol.Controls.Add(this.groupBoxAnalysis);
            this.tabAssayProtocol.Controls.Add(this.groupBoxEmission);
            this.tabAssayProtocol.Controls.Add(this.groupBoxExcitation);
            this.tabAssayProtocol.Controls.Add(this.groupBoxMicroplate);
            this.tabAssayProtocol.Controls.Add(this.groupBox2);
            this.tabAssayProtocol.Location = new System.Drawing.Point(4, 42);
            this.tabAssayProtocol.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.tabAssayProtocol.Name = "tabAssayProtocol";
            this.tabAssayProtocol.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.tabAssayProtocol.Size = new System.Drawing.Size(2632, 1078);
            this.tabAssayProtocol.TabIndex = 0;
            this.tabAssayProtocol.Text = "Assay Protocol";
            this.tabAssayProtocol.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudDelay);
            this.groupBox3.Controls.Add(this.nudScans);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.cboScanType);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(13, 148);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox3.Size = new System.Drawing.Size(1211, 192);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Scan Type";
            // 
            // nudDelay
            // 
            this.nudDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDelay.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudDelay.Location = new System.Drawing.Point(789, 122);
            this.nudDelay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudDelay.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.nudDelay.Name = "nudDelay";
            this.nudDelay.Size = new System.Drawing.Size(229, 30);
            this.nudDelay.TabIndex = 85;
            // 
            // nudScans
            // 
            this.nudScans.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudScans.Location = new System.Drawing.Point(344, 122);
            this.nudScans.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudScans.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudScans.Name = "nudScans";
            this.nudScans.Size = new System.Drawing.Size(229, 30);
            this.nudScans.TabIndex = 84;
            this.nudScans.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudScans.ValueChanged += new System.EventHandler(this.nudScans_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(639, 125);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 25);
            this.label7.TabIndex = 27;
            this.label7.Text = "Delay [s]";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(20, 125);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 25);
            this.label5.TabIndex = 26;
            this.label5.Text = "Number of Scans";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboScanType
            // 
            this.cboScanType.BackColor = System.Drawing.Color.White;
            this.cboScanType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScanType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboScanType.FormattingEnabled = true;
            this.cboScanType.Location = new System.Drawing.Point(347, 51);
            this.cboScanType.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboScanType.Name = "cboScanType";
            this.cboScanType.Size = new System.Drawing.Size(372, 33);
            this.cboScanType.TabIndex = 25;
            this.cboScanType.Tag = "";
            this.cboScanType.SelectedIndexChanged += new System.EventHandler(this.cboScanType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 25);
            this.label3.TabIndex = 23;
            this.label3.Text = "Select Method";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxAnalysis
            // 
            this.groupBoxAnalysis.Controls.Add(this.nudWavelengthB);
            this.groupBoxAnalysis.Controls.Add(this.nudWavelengthA);
            this.groupBoxAnalysis.Controls.Add(this.cboBandB);
            this.groupBoxAnalysis.Controls.Add(this.cboBandA);
            this.groupBoxAnalysis.Controls.Add(this.label19);
            this.groupBoxAnalysis.Controls.Add(this.label4);
            this.groupBoxAnalysis.Controls.Add(this.label30);
            this.groupBoxAnalysis.Controls.Add(this.label32);
            this.groupBoxAnalysis.Location = new System.Drawing.Point(7, 558);
            this.groupBoxAnalysis.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBoxAnalysis.Name = "groupBoxAnalysis";
            this.groupBoxAnalysis.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBoxAnalysis.Size = new System.Drawing.Size(1216, 194);
            this.groupBoxAnalysis.TabIndex = 81;
            this.groupBoxAnalysis.TabStop = false;
            this.groupBoxAnalysis.Text = "Analysis Parameters";
            // 
            // nudWavelengthB
            // 
            this.nudWavelengthB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudWavelengthB.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudWavelengthB.Location = new System.Drawing.Point(972, 55);
            this.nudWavelengthB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudWavelengthB.Maximum = new decimal(new int[] {
            700,
            0,
            0,
            0});
            this.nudWavelengthB.Minimum = new decimal(new int[] {
            320,
            0,
            0,
            0});
            this.nudWavelengthB.Name = "nudWavelengthB";
            this.nudWavelengthB.Size = new System.Drawing.Size(229, 30);
            this.nudWavelengthB.TabIndex = 86;
            this.nudWavelengthB.Value = new decimal(new int[] {
            320,
            0,
            0,
            0});
            // 
            // nudWavelengthA
            // 
            this.nudWavelengthA.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudWavelengthA.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudWavelengthA.Location = new System.Drawing.Point(352, 51);
            this.nudWavelengthA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudWavelengthA.Maximum = new decimal(new int[] {
            700,
            0,
            0,
            0});
            this.nudWavelengthA.Minimum = new decimal(new int[] {
            320,
            0,
            0,
            0});
            this.nudWavelengthA.Name = "nudWavelengthA";
            this.nudWavelengthA.Size = new System.Drawing.Size(229, 30);
            this.nudWavelengthA.TabIndex = 83;
            this.nudWavelengthA.Value = new decimal(new int[] {
            320,
            0,
            0,
            0});
            // 
            // cboBandB
            // 
            this.cboBandB.BackColor = System.Drawing.Color.White;
            this.cboBandB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBandB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBandB.FormattingEnabled = true;
            this.cboBandB.Location = new System.Drawing.Point(972, 119);
            this.cboBandB.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboBandB.Name = "cboBandB";
            this.cboBandB.Size = new System.Drawing.Size(228, 33);
            this.cboBandB.TabIndex = 85;
            this.cboBandB.Tag = "";
            // 
            // cboBandA
            // 
            this.cboBandA.BackColor = System.Drawing.Color.White;
            this.cboBandA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBandA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBandA.FormattingEnabled = true;
            this.cboBandA.Location = new System.Drawing.Point(352, 119);
            this.cboBandA.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboBandA.Name = "cboBandA";
            this.cboBandA.Size = new System.Drawing.Size(228, 33);
            this.cboBandA.TabIndex = 83;
            this.cboBandA.Tag = "";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(647, 125);
            this.label19.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(230, 25);
            this.label19.TabIndex = 74;
            this.label19.Text = "Wavelength Band B [nm]";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(20, 125);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(231, 25);
            this.label4.TabIndex = 73;
            this.label4.Text = "Wavelength Band A [nm]";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(20, 58);
            this.label30.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(180, 25);
            this.label30.TabIndex = 64;
            this.label30.Text = "Wavelength A [nm]";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(647, 58);
            this.label32.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(179, 25);
            this.label32.TabIndex = 66;
            this.label32.Text = "Wavelength B [nm]";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxEmission
            // 
            this.groupBoxEmission.Controls.Add(this.nudIntegration);
            this.groupBoxEmission.Controls.Add(this.label58);
            this.groupBoxEmission.Controls.Add(this.cboDetector);
            this.groupBoxEmission.Controls.Add(this.label45);
            this.groupBoxEmission.Location = new System.Drawing.Point(633, 352);
            this.groupBoxEmission.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBoxEmission.Name = "groupBoxEmission";
            this.groupBoxEmission.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBoxEmission.Size = new System.Drawing.Size(589, 192);
            this.groupBoxEmission.TabIndex = 80;
            this.groupBoxEmission.TabStop = false;
            this.groupBoxEmission.Text = "Emission";
            // 
            // nudIntegration
            // 
            this.nudIntegration.DecimalPlaces = 1;
            this.nudIntegration.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudIntegration.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudIntegration.Location = new System.Drawing.Point(345, 122);
            this.nudIntegration.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudIntegration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudIntegration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudIntegration.Name = "nudIntegration";
            this.nudIntegration.Size = new System.Drawing.Size(229, 30);
            this.nudIntegration.TabIndex = 82;
            this.nudIntegration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label58.Location = new System.Drawing.Point(20, 58);
            this.label58.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(85, 25);
            this.label58.TabIndex = 61;
            this.label58.Text = "Detector";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboDetector
            // 
            this.cboDetector.AutoCompleteCustomSource.AddRange(new string[] {
            "Greiner Low Profile",
            "Greiner Standard",
            "Corning Low Profile",
            "Corning Standard"});
            this.cboDetector.BackColor = System.Drawing.Color.White;
            this.cboDetector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDetector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDetector.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboDetector.FormattingEnabled = true;
            this.cboDetector.Location = new System.Drawing.Point(149, 51);
            this.cboDetector.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboDetector.Name = "cboDetector";
            this.cboDetector.Size = new System.Drawing.Size(424, 33);
            this.cboDetector.TabIndex = 75;
            this.cboDetector.Tag = "";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(20, 125);
            this.label45.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(195, 25);
            this.label45.TabIndex = 73;
            this.label45.Text = "Integration Time [ms]";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxExcitation
            // 
            this.groupBoxExcitation.Controls.Add(this.nudCurrent);
            this.groupBoxExcitation.Controls.Add(this.label2);
            this.groupBoxExcitation.Controls.Add(this.label9);
            this.groupBoxExcitation.Controls.Add(this.cboLed);
            this.groupBoxExcitation.Location = new System.Drawing.Point(13, 352);
            this.groupBoxExcitation.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBoxExcitation.Name = "groupBoxExcitation";
            this.groupBoxExcitation.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBoxExcitation.Size = new System.Drawing.Size(589, 192);
            this.groupBoxExcitation.TabIndex = 36;
            this.groupBoxExcitation.TabStop = false;
            this.groupBoxExcitation.Text = "Excitation";
            // 
            // nudCurrent
            // 
            this.nudCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudCurrent.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudCurrent.Location = new System.Drawing.Point(347, 125);
            this.nudCurrent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudCurrent.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudCurrent.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudCurrent.Name = "nudCurrent";
            this.nudCurrent.Size = new System.Drawing.Size(229, 30);
            this.nudCurrent.TabIndex = 83;
            this.nudCurrent.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 25);
            this.label2.TabIndex = 78;
            this.label2.Text = "Wavelength [nm]";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(20, 125);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(124, 25);
            this.label9.TabIndex = 39;
            this.label9.Text = "Current [mA]";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboLed
            // 
            this.cboLed.BackColor = System.Drawing.Color.White;
            this.cboLed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLed.FormattingEnabled = true;
            this.cboLed.Location = new System.Drawing.Point(347, 51);
            this.cboLed.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboLed.Name = "cboLed";
            this.cboLed.Size = new System.Drawing.Size(228, 33);
            this.cboLed.TabIndex = 79;
            this.cboLed.Tag = "";
            // 
            // groupBoxMicroplate
            // 
            this.groupBoxMicroplate.Controls.Add(this.cboPlateFormat);
            this.groupBoxMicroplate.Controls.Add(this.label1);
            this.groupBoxMicroplate.Location = new System.Drawing.Point(12, 11);
            this.groupBoxMicroplate.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBoxMicroplate.Name = "groupBoxMicroplate";
            this.groupBoxMicroplate.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBoxMicroplate.Size = new System.Drawing.Size(1211, 124);
            this.groupBoxMicroplate.TabIndex = 35;
            this.groupBoxMicroplate.TabStop = false;
            this.groupBoxMicroplate.Text = "Microplate";
            // 
            // cboPlateFormat
            // 
            this.cboPlateFormat.BackColor = System.Drawing.Color.White;
            this.cboPlateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlateFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPlateFormat.FormattingEnabled = true;
            this.cboPlateFormat.Location = new System.Drawing.Point(347, 51);
            this.cboPlateFormat.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboPlateFormat.Name = "cboPlateFormat";
            this.cboPlateFormat.Size = new System.Drawing.Size(372, 33);
            this.cboPlateFormat.TabIndex = 25;
            this.cboPlateFormat.Tag = "";
            this.cboPlateFormat.SelectedIndexChanged += new System.EventHandler(this.cboPlateFormat_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 58);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 25);
            this.label1.TabIndex = 23;
            this.label1.Text = "Select Plate Format";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbColumnWellSelection);
            this.groupBox2.Controls.Add(this.lbRowWellSelection);
            this.groupBox2.Controls.Add(this.chartPlate);
            this.groupBox2.Controls.Add(this.btnWellSelectionAll);
            this.groupBox2.Controls.Add(this.btnWellSelectionReset);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(1235, 11);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox2.Size = new System.Drawing.Size(1372, 1010);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Well Selection";
            // 
            // lbColumnWellSelection
            // 
            this.lbColumnWellSelection.AutoSize = true;
            this.lbColumnWellSelection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbColumnWellSelection.Location = new System.Drawing.Point(409, 931);
            this.lbColumnWellSelection.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbColumnWellSelection.MaximumSize = new System.Drawing.Size(78, 53);
            this.lbColumnWellSelection.MinimumSize = new System.Drawing.Size(78, 53);
            this.lbColumnWellSelection.Name = "lbColumnWellSelection";
            this.lbColumnWellSelection.Size = new System.Drawing.Size(78, 53);
            this.lbColumnWellSelection.TabIndex = 35;
            this.lbColumnWellSelection.Text = "1";
            this.lbColumnWellSelection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbRowWellSelection
            // 
            this.lbRowWellSelection.AutoSize = true;
            this.lbRowWellSelection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbRowWellSelection.Location = new System.Drawing.Point(299, 931);
            this.lbRowWellSelection.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbRowWellSelection.MaximumSize = new System.Drawing.Size(78, 53);
            this.lbRowWellSelection.MinimumSize = new System.Drawing.Size(78, 53);
            this.lbRowWellSelection.Name = "lbRowWellSelection";
            this.lbRowWellSelection.Size = new System.Drawing.Size(78, 53);
            this.lbRowWellSelection.TabIndex = 34;
            this.lbRowWellSelection.Text = "A";
            this.lbRowWellSelection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chartPlate
            // 
            this.chartPlate.Location = new System.Drawing.Point(20, 69);
            this.chartPlate.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.chartPlate.Name = "chartPlate";
            this.chartPlate.Size = new System.Drawing.Size(1296, 831);
            this.chartPlate.TabIndex = 33;
            this.chartPlate.Text = "wellSelectionChart";
            this.chartPlate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chartPlate_MouseDown);
            this.chartPlate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartPlate_MouseMove);
            this.chartPlate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chartPlate_MouseUp);
            // 
            // btnWellSelectionAll
            // 
            this.btnWellSelectionAll.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnWellSelectionAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWellSelectionAll.Location = new System.Drawing.Point(840, 918);
            this.btnWellSelectionAll.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnWellSelectionAll.Name = "btnWellSelectionAll";
            this.btnWellSelectionAll.Size = new System.Drawing.Size(193, 56);
            this.btnWellSelectionAll.TabIndex = 29;
            this.btnWellSelectionAll.Text = "Select All";
            this.btnWellSelectionAll.UseVisualStyleBackColor = true;
            this.btnWellSelectionAll.Click += new System.EventHandler(this.btnWellSelectionAll_Click);
            // 
            // btnWellSelectionReset
            // 
            this.btnWellSelectionReset.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnWellSelectionReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWellSelectionReset.Location = new System.Drawing.Point(1079, 918);
            this.btnWellSelectionReset.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnWellSelectionReset.Name = "btnWellSelectionReset";
            this.btnWellSelectionReset.Size = new System.Drawing.Size(193, 56);
            this.btnWellSelectionReset.TabIndex = 28;
            this.btnWellSelectionReset.Text = "Reset";
            this.btnWellSelectionReset.UseVisualStyleBackColor = true;
            this.btnWellSelectionReset.Click += new System.EventHandler(this.btnWellSelectionReset_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(80, 942);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(151, 25);
            this.label8.TabIndex = 25;
            this.label8.Text = "Mouse Location";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabMeasure
            // 
            this.tabMeasure.Controls.Add(this.groupBox1);
            this.tabMeasure.Controls.Add(this.groupBox6);
            this.tabMeasure.Location = new System.Drawing.Point(4, 42);
            this.tabMeasure.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.tabMeasure.Name = "tabMeasure";
            this.tabMeasure.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.tabMeasure.Size = new System.Drawing.Size(2632, 1078);
            this.tabMeasure.TabIndex = 1;
            this.tabMeasure.Text = "Measure";
            this.tabMeasure.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chartWaveform);
            this.groupBox1.Location = new System.Drawing.Point(1355, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox1.Size = new System.Drawing.Size(1259, 1026);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fluorescence Spectrum";
            // 
            // chartWaveform
            // 
            this.chartWaveform.BorderlineColor = System.Drawing.Color.Black;
            chartArea4.Name = "ChartArea1";
            this.chartWaveform.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartWaveform.Legends.Add(legend4);
            this.chartWaveform.Location = new System.Drawing.Point(12, 51);
            this.chartWaveform.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.chartWaveform.Name = "chartWaveform";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartWaveform.Series.Add(series4);
            this.chartWaveform.Size = new System.Drawing.Size(1235, 961);
            this.chartWaveform.TabIndex = 39;
            this.chartWaveform.Text = "chart2";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lbLegendMax);
            this.groupBox6.Controls.Add(this.lbLegendMin);
            this.groupBox6.Controls.Add(this.lbColumn);
            this.groupBox6.Controls.Add(this.lbRow);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Controls.Add(this.chartLegend);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.cboPlotSelection);
            this.groupBox6.Controls.Add(this.chartResultMap);
            this.groupBox6.Location = new System.Drawing.Point(12, 11);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox6.Size = new System.Drawing.Size(1331, 1026);
            this.groupBox6.TabIndex = 44;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Microplate Chart";
            // 
            // lbLegendMax
            // 
            this.lbLegendMax.AutoSize = true;
            this.lbLegendMax.Location = new System.Drawing.Point(653, 942);
            this.lbLegendMax.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbLegendMax.MaximumSize = new System.Drawing.Size(160, 49);
            this.lbLegendMax.MinimumSize = new System.Drawing.Size(160, 49);
            this.lbLegendMax.Name = "lbLegendMax";
            this.lbLegendMax.Size = new System.Drawing.Size(160, 49);
            this.lbLegendMax.TabIndex = 52;
            this.lbLegendMax.Text = "Max";
            this.lbLegendMax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbLegendMin
            // 
            this.lbLegendMin.AutoSize = true;
            this.lbLegendMin.Location = new System.Drawing.Point(5, 942);
            this.lbLegendMin.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbLegendMin.MaximumSize = new System.Drawing.Size(160, 49);
            this.lbLegendMin.MinimumSize = new System.Drawing.Size(160, 49);
            this.lbLegendMin.Name = "lbLegendMin";
            this.lbLegendMin.Size = new System.Drawing.Size(160, 49);
            this.lbLegendMin.TabIndex = 51;
            this.lbLegendMin.Text = "Min";
            this.lbLegendMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbColumn
            // 
            this.lbColumn.AutoSize = true;
            this.lbColumn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbColumn.Location = new System.Drawing.Point(1235, 942);
            this.lbColumn.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbColumn.MaximumSize = new System.Drawing.Size(85, 53);
            this.lbColumn.MinimumSize = new System.Drawing.Size(85, 53);
            this.lbColumn.Name = "lbColumn";
            this.lbColumn.Size = new System.Drawing.Size(85, 53);
            this.lbColumn.TabIndex = 50;
            this.lbColumn.Text = "1";
            this.lbColumn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbRow
            // 
            this.lbRow.AutoSize = true;
            this.lbRow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbRow.Location = new System.Drawing.Point(1123, 942);
            this.lbRow.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbRow.MaximumSize = new System.Drawing.Size(85, 53);
            this.lbRow.MinimumSize = new System.Drawing.Size(85, 53);
            this.lbRow.Name = "lbRow";
            this.lbRow.Size = new System.Drawing.Size(85, 53);
            this.lbRow.TabIndex = 49;
            this.lbRow.Text = "A";
            this.lbRow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(904, 951);
            this.label11.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(151, 25);
            this.label11.TabIndex = 48;
            this.label11.Text = "Mouse Location";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chartLegend
            // 
            chartArea5.Name = "ChartArea1";
            this.chartLegend.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chartLegend.Legends.Add(legend5);
            this.chartLegend.Location = new System.Drawing.Point(124, 918);
            this.chartLegend.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.chartLegend.Name = "chartLegend";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.chartLegend.Series.Add(series5);
            this.chartLegend.Size = new System.Drawing.Size(600, 99);
            this.chartLegend.TabIndex = 38;
            this.chartLegend.Text = "chart1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(667, 49);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(174, 25);
            this.label6.TabIndex = 33;
            this.label6.Text = "Select Plot Display";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboPlotSelection
            // 
            this.cboPlotSelection.BackColor = System.Drawing.SystemColors.Window;
            this.cboPlotSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlotSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPlotSelection.FormattingEnabled = true;
            this.cboPlotSelection.Location = new System.Drawing.Point(940, 42);
            this.cboPlotSelection.Margin = new System.Windows.Forms.Padding(0);
            this.cboPlotSelection.Name = "cboPlotSelection";
            this.cboPlotSelection.Size = new System.Drawing.Size(339, 33);
            this.cboPlotSelection.TabIndex = 32;
            this.cboPlotSelection.Tag = "";
            this.cboPlotSelection.SelectedIndexChanged += new System.EventHandler(this.cboPlotSelection_SelectedIndexChanged);
            // 
            // chartResultMap
            // 
            chartArea6.Name = "ChartArea1";
            this.chartResultMap.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chartResultMap.Legends.Add(legend6);
            this.chartResultMap.Location = new System.Drawing.Point(12, 75);
            this.chartResultMap.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.chartResultMap.Name = "chartResultMap";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.chartResultMap.Series.Add(series6);
            this.chartResultMap.Size = new System.Drawing.Size(1296, 831);
            this.chartResultMap.TabIndex = 0;
            this.chartResultMap.Text = "chart1";
            this.chartResultMap.Click += new System.EventHandler(this.chartResultMap_Click);
            this.chartResultMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartResultMap_MouseMove);
            // 
            // tabResults
            // 
            this.tabResults.Location = new System.Drawing.Point(4, 42);
            this.tabResults.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabResults.Name = "tabResults";
            this.tabResults.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabResults.Size = new System.Drawing.Size(2632, 1078);
            this.tabResults.TabIndex = 2;
            this.tabResults.Text = "Results";
            this.tabResults.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.labelPlateTime,
            this.toolStripSeparator6,
            this.toolStripLabel5,
            this.labelScanTime,
            this.toolStripSeparator7,
            this.toolStripLabel7,
            this.labelCurrentScan,
            this.toolStripSeparator8,
            this.toolStripLabel2,
            this.labelTotalScans,
            this.toolStripSeparator5,
            this.btnAutoscale,
            this.labelRowOffset,
            this.labelColumnOffset,
            this.toolStripSeparator9});
            this.toolStrip1.Location = new System.Drawing.Point(0, 1192);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(2260, 34);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(93, 33);
            this.toolStripLabel3.Text = "Plate Time";
            // 
            // labelPlateTime
            // 
            this.labelPlateTime.Name = "labelPlateTime";
            this.labelPlateTime.Size = new System.Drawing.Size(46, 33);
            this.labelPlateTime.Text = "0:00";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(92, 33);
            this.toolStripLabel5.Text = "Scan Time";
            // 
            // labelScanTime
            // 
            this.labelScanTime.Name = "labelScanTime";
            this.labelScanTime.Size = new System.Drawing.Size(46, 33);
            this.labelScanTime.Text = "0:00";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(112, 33);
            this.toolStripLabel7.Text = "Current Scan";
            // 
            // labelCurrentScan
            // 
            this.labelCurrentScan.Name = "labelCurrentScan";
            this.labelCurrentScan.Size = new System.Drawing.Size(22, 33);
            this.labelCurrentScan.Text = "1";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(99, 33);
            this.toolStripLabel2.Text = "Total Scans";
            // 
            // labelTotalScans
            // 
            this.labelTotalScans.Name = "labelTotalScans";
            this.labelTotalScans.Size = new System.Drawing.Size(22, 33);
            this.labelTotalScans.Text = "1";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 38);
            // 
            // btnAutoscale
            // 
            this.btnAutoscale.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnAutoscale.Image = ((System.Drawing.Image)(resources.GetObject("btnAutoscale.Image")));
            this.btnAutoscale.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAutoscale.Name = "btnAutoscale";
            this.btnAutoscale.Size = new System.Drawing.Size(126, 33);
            this.btnAutoscale.Text = "AutoScale Off";
            this.btnAutoscale.Click += new System.EventHandler(this.btnAutoscale_Click);
            // 
            // labelRowOffset
            // 
            this.labelRowOffset.Name = "labelRowOffset";
            this.labelRowOffset.Size = new System.Drawing.Size(100, 33);
            this.labelRowOffset.Text = "Row Offset";
            // 
            // labelColumnOffset
            // 
            this.labelColumnOffset.Name = "labelColumnOffset";
            this.labelColumnOffset.Size = new System.Drawing.Size(128, 33);
            this.labelColumnOffset.Text = "Column Offset";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 38);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2260, 1226);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.MaximumSize = new System.Drawing.Size(2777, 1282);
            this.MinimumSize = new System.Drawing.Size(2254, 1282);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabAssayProtocol.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScans)).EndInit();
            this.groupBoxAnalysis.ResumeLayout(false);
            this.groupBoxAnalysis.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWavelengthB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWavelengthA)).EndInit();
            this.groupBoxEmission.ResumeLayout(false);
            this.groupBoxEmission.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntegration)).EndInit();
            this.groupBoxExcitation.ResumeLayout(false);
            this.groupBoxExcitation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCurrent)).EndInit();
            this.groupBoxMicroplate.ResumeLayout(false);
            this.groupBoxMicroplate.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPlate)).EndInit();
            this.tabMeasure.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartWaveform)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartLegend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartResultMap)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnStart;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripButton btnInsertPlate;
        private System.Windows.Forms.ToolStripButton btnEjectPlate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnApplyProtocol;
        private System.Windows.Forms.ToolStripButton btnResetProtocol;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnSaveData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel labelClock;
        private System.Windows.Forms.ToolStripLabel labelStatus;
        private System.Windows.Forms.ToolStripLabel labelFixed;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabAssayProtocol;
        private System.Windows.Forms.GroupBox groupBoxAnalysis;
        private System.Windows.Forms.ComboBox cboBandB;
        private System.Windows.Forms.ComboBox cboBandA;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.GroupBox groupBoxEmission;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.ComboBox cboDetector;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.GroupBox groupBoxExcitation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboLed;
        private System.Windows.Forms.GroupBox groupBoxMicroplate;
        private System.Windows.Forms.ComboBox cboPlateFormat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbColumnWellSelection;
        private System.Windows.Forms.Label lbRowWellSelection;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPlate;
        private System.Windows.Forms.Button btnWellSelectionAll;
        private System.Windows.Forms.Button btnWellSelectionReset;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tabMeasure;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartWaveform;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label lbLegendMax;
        private System.Windows.Forms.Label lbLegendMin;
        private System.Windows.Forms.Label lbColumn;
        private System.Windows.Forms.Label lbRow;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLegend;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboPlotSelection;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartResultMap;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.NumericUpDown nudIntegration;
        private System.Windows.Forms.NumericUpDown nudWavelengthB;
        private System.Windows.Forms.NumericUpDown nudWavelengthA;
        private System.Windows.Forms.NumericUpDown nudCurrent;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nudDelay;
        private System.Windows.Forms.NumericUpDown nudScans;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboScanType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabResults;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripLabel labelPlateTime;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripLabel labelScanTime;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripLabel labelCurrentScan;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel labelTotalScans;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnAutoscale;
        private System.Windows.Forms.ToolStripLabel labelRowOffset;
        private System.Windows.Forms.ToolStripLabel labelColumnOffset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
    }
}


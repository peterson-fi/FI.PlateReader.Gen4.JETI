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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabAssayProtocol = new System.Windows.Forms.TabPage();
            this.groupBoxAnalysis = new System.Windows.Forms.GroupBox();
            this.cboBandB = new System.Windows.Forms.ComboBox();
            this.cboBandA = new System.Windows.Forms.ComboBox();
            this.cboWavelengthB = new System.Windows.Forms.ComboBox();
            this.cboWavelengthA = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.groupBoxEmission = new System.Windows.Forms.GroupBox();
            this.cboIntegration = new System.Windows.Forms.ComboBox();
            this.label58 = new System.Windows.Forms.Label();
            this.cboDetector = new System.Windows.Forms.ComboBox();
            this.label45 = new System.Windows.Forms.Label();
            this.groupBoxExcitation = new System.Windows.Forms.GroupBox();
            this.cboLedPower = new System.Windows.Forms.ComboBox();
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
            this.tabResults = new System.Windows.Forms.TabPage();
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.labelLED = new System.Windows.Forms.ToolStripLabel();
            this.labelLEDhours = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabAssayProtocol.SuspendLayout();
            this.groupBoxAnalysis.SuspendLayout();
            this.groupBoxEmission.SuspendLayout();
            this.groupBoxExcitation.SuspendLayout();
            this.groupBoxMicroplate.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPlate)).BeginInit();
            this.tabResults.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartWaveform)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartLegend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartResultMap)).BeginInit();
            this.statusStrip.SuspendLayout();
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
            this.toolStripSeparator4,
            this.labelLED,
            this.labelLEDhours});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip.Size = new System.Drawing.Size(1710, 27);
            this.toolStrip.TabIndex = 4;
            this.toolStrip.Text = "toolStrip2";
            // 
            // btnStart
            // 
            this.btnStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(44, 24);
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(44, 24);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnInsertPlate
            // 
            this.btnInsertPlate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnInsertPlate.Image = ((System.Drawing.Image)(resources.GetObject("btnInsertPlate.Image")));
            this.btnInsertPlate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInsertPlate.Name = "btnInsertPlate";
            this.btnInsertPlate.Size = new System.Drawing.Size(86, 24);
            this.btnInsertPlate.Text = "Insert Plate";
            this.btnInsertPlate.Click += new System.EventHandler(this.btnInsertPlate_Click);
            // 
            // btnEjectPlate
            // 
            this.btnEjectPlate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnEjectPlate.Image = ((System.Drawing.Image)(resources.GetObject("btnEjectPlate.Image")));
            this.btnEjectPlate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEjectPlate.Name = "btnEjectPlate";
            this.btnEjectPlate.Size = new System.Drawing.Size(82, 24);
            this.btnEjectPlate.Text = "Eject Plate";
            this.btnEjectPlate.Click += new System.EventHandler(this.btnEjectPlate_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btnApplyProtocol
            // 
            this.btnApplyProtocol.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnApplyProtocol.Image = ((System.Drawing.Image)(resources.GetObject("btnApplyProtocol.Image")));
            this.btnApplyProtocol.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApplyProtocol.Name = "btnApplyProtocol";
            this.btnApplyProtocol.Size = new System.Drawing.Size(112, 24);
            this.btnApplyProtocol.Text = "Apply Protocol";
            this.btnApplyProtocol.Click += new System.EventHandler(this.btnApplyProtocol_Click);
            // 
            // btnResetProtocol
            // 
            this.btnResetProtocol.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnResetProtocol.Image = ((System.Drawing.Image)(resources.GetObject("btnResetProtocol.Image")));
            this.btnResetProtocol.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResetProtocol.Name = "btnResetProtocol";
            this.btnResetProtocol.Size = new System.Drawing.Size(109, 24);
            this.btnResetProtocol.Text = "Reset Protocol";
            this.btnResetProtocol.ToolTipText = "Reset Protocol";
            this.btnResetProtocol.Click += new System.EventHandler(this.btnResetProtocol_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // btnSaveData
            // 
            this.btnSaveData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.btnSaveData.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveData.Image")));
            this.btnSaveData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(80, 24);
            this.btnSaveData.Text = "Save Data";
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // labelClock
            // 
            this.labelClock.Name = "labelClock";
            this.labelClock.Size = new System.Drawing.Size(85, 24);
            this.labelClock.Text = "Clock Label";
            // 
            // labelStatus
            // 
            this.labelStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(89, 24);
            this.labelStatus.Text = "Status Label";
            // 
            // labelFixed
            // 
            this.labelFixed.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.labelFixed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFixed.Name = "labelFixed";
            this.labelFixed.Size = new System.Drawing.Size(45, 24);
            this.labelFixed.Text = "Task:";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabAssayProtocol);
            this.tabControl.Controls.Add(this.tabResults);
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(12, 38);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1760, 719);
            this.tabControl.TabIndex = 5;
            // 
            // tabAssayProtocol
            // 
            this.tabAssayProtocol.Controls.Add(this.groupBoxAnalysis);
            this.tabAssayProtocol.Controls.Add(this.groupBoxEmission);
            this.tabAssayProtocol.Controls.Add(this.groupBoxExcitation);
            this.tabAssayProtocol.Controls.Add(this.groupBoxMicroplate);
            this.tabAssayProtocol.Controls.Add(this.groupBox2);
            this.tabAssayProtocol.Location = new System.Drawing.Point(4, 38);
            this.tabAssayProtocol.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabAssayProtocol.Name = "tabAssayProtocol";
            this.tabAssayProtocol.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabAssayProtocol.Size = new System.Drawing.Size(1752, 677);
            this.tabAssayProtocol.TabIndex = 0;
            this.tabAssayProtocol.Text = "Assay Protocol";
            this.tabAssayProtocol.UseVisualStyleBackColor = true;
            // 
            // groupBoxAnalysis
            // 
            this.groupBoxAnalysis.Controls.Add(this.cboBandB);
            this.groupBoxAnalysis.Controls.Add(this.cboBandA);
            this.groupBoxAnalysis.Controls.Add(this.cboWavelengthB);
            this.groupBoxAnalysis.Controls.Add(this.cboWavelengthA);
            this.groupBoxAnalysis.Controls.Add(this.label19);
            this.groupBoxAnalysis.Controls.Add(this.label4);
            this.groupBoxAnalysis.Controls.Add(this.label30);
            this.groupBoxAnalysis.Controls.Add(this.label32);
            this.groupBoxAnalysis.Location = new System.Drawing.Point(4, 223);
            this.groupBoxAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxAnalysis.Name = "groupBoxAnalysis";
            this.groupBoxAnalysis.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxAnalysis.Size = new System.Drawing.Size(811, 124);
            this.groupBoxAnalysis.TabIndex = 81;
            this.groupBoxAnalysis.TabStop = false;
            this.groupBoxAnalysis.Text = "Analysis Parameters";
            // 
            // cboBandB
            // 
            this.cboBandB.BackColor = System.Drawing.Color.White;
            this.cboBandB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBandB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBandB.FormattingEnabled = true;
            this.cboBandB.Location = new System.Drawing.Point(648, 76);
            this.cboBandB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboBandB.Name = "cboBandB";
            this.cboBandB.Size = new System.Drawing.Size(153, 28);
            this.cboBandB.TabIndex = 85;
            this.cboBandB.Tag = "";
            // 
            // cboBandA
            // 
            this.cboBandA.BackColor = System.Drawing.Color.White;
            this.cboBandA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBandA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBandA.FormattingEnabled = true;
            this.cboBandA.Location = new System.Drawing.Point(235, 76);
            this.cboBandA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboBandA.Name = "cboBandA";
            this.cboBandA.Size = new System.Drawing.Size(153, 28);
            this.cboBandA.TabIndex = 83;
            this.cboBandA.Tag = "";
            // 
            // cboWavelengthB
            // 
            this.cboWavelengthB.BackColor = System.Drawing.Color.White;
            this.cboWavelengthB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWavelengthB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboWavelengthB.FormattingEnabled = true;
            this.cboWavelengthB.Location = new System.Drawing.Point(648, 33);
            this.cboWavelengthB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboWavelengthB.Name = "cboWavelengthB";
            this.cboWavelengthB.Size = new System.Drawing.Size(153, 28);
            this.cboWavelengthB.TabIndex = 84;
            this.cboWavelengthB.Tag = "";
            // 
            // cboWavelengthA
            // 
            this.cboWavelengthA.BackColor = System.Drawing.Color.White;
            this.cboWavelengthA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWavelengthA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboWavelengthA.FormattingEnabled = true;
            this.cboWavelengthA.Location = new System.Drawing.Point(235, 33);
            this.cboWavelengthA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboWavelengthA.Name = "cboWavelengthA";
            this.cboWavelengthA.Size = new System.Drawing.Size(153, 28);
            this.cboWavelengthA.TabIndex = 82;
            this.cboWavelengthA.Tag = "";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(431, 80);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(195, 20);
            this.label19.TabIndex = 74;
            this.label19.Text = "Wavelength Band B [nm]";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 80);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(194, 20);
            this.label4.TabIndex = 73;
            this.label4.Text = "Wavelength Band A [nm]";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(13, 37);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(150, 20);
            this.label30.TabIndex = 64;
            this.label30.Text = "Wavelength A [nm]";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(431, 37);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(151, 20);
            this.label32.TabIndex = 66;
            this.label32.Text = "Wavelength B [nm]";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxEmission
            // 
            this.groupBoxEmission.Controls.Add(this.cboIntegration);
            this.groupBoxEmission.Controls.Add(this.label58);
            this.groupBoxEmission.Controls.Add(this.cboDetector);
            this.groupBoxEmission.Controls.Add(this.label45);
            this.groupBoxEmission.Location = new System.Drawing.Point(421, 92);
            this.groupBoxEmission.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxEmission.Name = "groupBoxEmission";
            this.groupBoxEmission.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxEmission.Size = new System.Drawing.Size(393, 123);
            this.groupBoxEmission.TabIndex = 80;
            this.groupBoxEmission.TabStop = false;
            this.groupBoxEmission.Text = "Emission";
            // 
            // cboIntegration
            // 
            this.cboIntegration.BackColor = System.Drawing.Color.White;
            this.cboIntegration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIntegration.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboIntegration.FormattingEnabled = true;
            this.cboIntegration.Location = new System.Drawing.Point(231, 76);
            this.cboIntegration.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboIntegration.Name = "cboIntegration";
            this.cboIntegration.Size = new System.Drawing.Size(153, 28);
            this.cboIntegration.TabIndex = 83;
            this.cboIntegration.Tag = "";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label58.Location = new System.Drawing.Point(13, 37);
            this.label58.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(74, 20);
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
            this.cboDetector.Location = new System.Drawing.Point(100, 33);
            this.cboDetector.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboDetector.Name = "cboDetector";
            this.cboDetector.Size = new System.Drawing.Size(284, 28);
            this.cboDetector.TabIndex = 75;
            this.cboDetector.Tag = "";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(13, 80);
            this.label45.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(167, 20);
            this.label45.TabIndex = 73;
            this.label45.Text = "Integration Time [ms]";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxExcitation
            // 
            this.groupBoxExcitation.Controls.Add(this.cboLedPower);
            this.groupBoxExcitation.Controls.Add(this.label2);
            this.groupBoxExcitation.Controls.Add(this.label9);
            this.groupBoxExcitation.Controls.Add(this.cboLed);
            this.groupBoxExcitation.Location = new System.Drawing.Point(8, 92);
            this.groupBoxExcitation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxExcitation.Name = "groupBoxExcitation";
            this.groupBoxExcitation.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxExcitation.Size = new System.Drawing.Size(393, 123);
            this.groupBoxExcitation.TabIndex = 36;
            this.groupBoxExcitation.TabStop = false;
            this.groupBoxExcitation.Text = "Excitation";
            // 
            // cboLedPower
            // 
            this.cboLedPower.BackColor = System.Drawing.Color.White;
            this.cboLedPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLedPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLedPower.FormattingEnabled = true;
            this.cboLedPower.Location = new System.Drawing.Point(231, 76);
            this.cboLedPower.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboLedPower.Name = "cboLedPower";
            this.cboLedPower.Size = new System.Drawing.Size(153, 28);
            this.cboLedPower.TabIndex = 82;
            this.cboLedPower.Tag = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 20);
            this.label2.TabIndex = 78;
            this.label2.Text = "LED Wavelength [nm]";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 80);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 20);
            this.label9.TabIndex = 39;
            this.label9.Text = "LED Power [%]";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboLed
            // 
            this.cboLed.BackColor = System.Drawing.Color.White;
            this.cboLed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLed.FormattingEnabled = true;
            this.cboLed.Location = new System.Drawing.Point(231, 33);
            this.cboLed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboLed.Name = "cboLed";
            this.cboLed.Size = new System.Drawing.Size(153, 28);
            this.cboLed.TabIndex = 79;
            this.cboLed.Tag = "";
            // 
            // groupBoxMicroplate
            // 
            this.groupBoxMicroplate.Controls.Add(this.cboPlateFormat);
            this.groupBoxMicroplate.Controls.Add(this.label1);
            this.groupBoxMicroplate.Location = new System.Drawing.Point(8, 7);
            this.groupBoxMicroplate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxMicroplate.Name = "groupBoxMicroplate";
            this.groupBoxMicroplate.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxMicroplate.Size = new System.Drawing.Size(807, 79);
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
            this.cboPlateFormat.Location = new System.Drawing.Point(231, 33);
            this.cboPlateFormat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboPlateFormat.Name = "cboPlateFormat";
            this.cboPlateFormat.Size = new System.Drawing.Size(249, 28);
            this.cboPlateFormat.TabIndex = 25;
            this.cboPlateFormat.Tag = "";
            this.cboPlateFormat.SelectedIndexChanged += new System.EventHandler(this.cboPlateFormat_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 20);
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
            this.groupBox2.Location = new System.Drawing.Point(823, 7);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(915, 646);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Well Selection";
            // 
            // lbColumnWellSelection
            // 
            this.lbColumnWellSelection.AutoSize = true;
            this.lbColumnWellSelection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbColumnWellSelection.Location = new System.Drawing.Point(273, 596);
            this.lbColumnWellSelection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbColumnWellSelection.MaximumSize = new System.Drawing.Size(53, 35);
            this.lbColumnWellSelection.MinimumSize = new System.Drawing.Size(53, 35);
            this.lbColumnWellSelection.Name = "lbColumnWellSelection";
            this.lbColumnWellSelection.Size = new System.Drawing.Size(53, 35);
            this.lbColumnWellSelection.TabIndex = 35;
            this.lbColumnWellSelection.Text = "1";
            this.lbColumnWellSelection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbRowWellSelection
            // 
            this.lbRowWellSelection.AutoSize = true;
            this.lbRowWellSelection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbRowWellSelection.Location = new System.Drawing.Point(199, 596);
            this.lbRowWellSelection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRowWellSelection.MaximumSize = new System.Drawing.Size(53, 35);
            this.lbRowWellSelection.MinimumSize = new System.Drawing.Size(53, 35);
            this.lbRowWellSelection.Name = "lbRowWellSelection";
            this.lbRowWellSelection.Size = new System.Drawing.Size(53, 35);
            this.lbRowWellSelection.TabIndex = 34;
            this.lbRowWellSelection.Text = "A";
            this.lbRowWellSelection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chartPlate
            // 
            this.chartPlate.Location = new System.Drawing.Point(13, 44);
            this.chartPlate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chartPlate.Name = "chartPlate";
            this.chartPlate.Size = new System.Drawing.Size(864, 532);
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
            this.btnWellSelectionAll.Location = new System.Drawing.Point(560, 587);
            this.btnWellSelectionAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnWellSelectionAll.Name = "btnWellSelectionAll";
            this.btnWellSelectionAll.Size = new System.Drawing.Size(129, 36);
            this.btnWellSelectionAll.TabIndex = 29;
            this.btnWellSelectionAll.Text = "Select All";
            this.btnWellSelectionAll.UseVisualStyleBackColor = true;
            this.btnWellSelectionAll.Click += new System.EventHandler(this.btnWellSelectionAll_Click);
            // 
            // btnWellSelectionReset
            // 
            this.btnWellSelectionReset.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnWellSelectionReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWellSelectionReset.Location = new System.Drawing.Point(719, 587);
            this.btnWellSelectionReset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnWellSelectionReset.Name = "btnWellSelectionReset";
            this.btnWellSelectionReset.Size = new System.Drawing.Size(129, 36);
            this.btnWellSelectionReset.TabIndex = 28;
            this.btnWellSelectionReset.Text = "Reset";
            this.btnWellSelectionReset.UseVisualStyleBackColor = true;
            this.btnWellSelectionReset.Click += new System.EventHandler(this.btnWellSelectionReset_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(53, 603);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 20);
            this.label8.TabIndex = 25;
            this.label8.Text = "Mouse Location";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabResults
            // 
            this.tabResults.Controls.Add(this.groupBox1);
            this.tabResults.Controls.Add(this.groupBox6);
            this.tabResults.Location = new System.Drawing.Point(4, 38);
            this.tabResults.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabResults.Name = "tabResults";
            this.tabResults.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabResults.Size = new System.Drawing.Size(1752, 677);
            this.tabResults.TabIndex = 1;
            this.tabResults.Text = "Results";
            this.tabResults.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chartWaveform);
            this.groupBox1.Location = new System.Drawing.Point(903, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(839, 657);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fluorescence Spectrum";
            // 
            // chartWaveform
            // 
            this.chartWaveform.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartArea1";
            this.chartWaveform.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartWaveform.Legends.Add(legend1);
            this.chartWaveform.Location = new System.Drawing.Point(8, 33);
            this.chartWaveform.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chartWaveform.Name = "chartWaveform";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartWaveform.Series.Add(series1);
            this.chartWaveform.Size = new System.Drawing.Size(823, 615);
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
            this.groupBox6.Location = new System.Drawing.Point(8, 7);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Size = new System.Drawing.Size(887, 657);
            this.groupBox6.TabIndex = 44;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Microplate Chart";
            // 
            // lbLegendMax
            // 
            this.lbLegendMax.AutoSize = true;
            this.lbLegendMax.Location = new System.Drawing.Point(436, 603);
            this.lbLegendMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLegendMax.MaximumSize = new System.Drawing.Size(107, 31);
            this.lbLegendMax.MinimumSize = new System.Drawing.Size(107, 31);
            this.lbLegendMax.Name = "lbLegendMax";
            this.lbLegendMax.Size = new System.Drawing.Size(107, 31);
            this.lbLegendMax.TabIndex = 52;
            this.lbLegendMax.Text = "Max";
            this.lbLegendMax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbLegendMin
            // 
            this.lbLegendMin.AutoSize = true;
            this.lbLegendMin.Location = new System.Drawing.Point(4, 603);
            this.lbLegendMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLegendMin.MaximumSize = new System.Drawing.Size(107, 31);
            this.lbLegendMin.MinimumSize = new System.Drawing.Size(107, 31);
            this.lbLegendMin.Name = "lbLegendMin";
            this.lbLegendMin.Size = new System.Drawing.Size(107, 31);
            this.lbLegendMin.TabIndex = 51;
            this.lbLegendMin.Text = "Min";
            this.lbLegendMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbColumn
            // 
            this.lbColumn.AutoSize = true;
            this.lbColumn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbColumn.Location = new System.Drawing.Point(823, 603);
            this.lbColumn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbColumn.MaximumSize = new System.Drawing.Size(53, 35);
            this.lbColumn.MinimumSize = new System.Drawing.Size(53, 35);
            this.lbColumn.Name = "lbColumn";
            this.lbColumn.Size = new System.Drawing.Size(53, 35);
            this.lbColumn.TabIndex = 50;
            this.lbColumn.Text = "1";
            this.lbColumn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbRow
            // 
            this.lbRow.AutoSize = true;
            this.lbRow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbRow.Location = new System.Drawing.Point(748, 603);
            this.lbRow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRow.MaximumSize = new System.Drawing.Size(53, 35);
            this.lbRow.MinimumSize = new System.Drawing.Size(53, 35);
            this.lbRow.Name = "lbRow";
            this.lbRow.Size = new System.Drawing.Size(53, 35);
            this.lbRow.TabIndex = 49;
            this.lbRow.Text = "A";
            this.lbRow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(603, 609);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(128, 20);
            this.label11.TabIndex = 48;
            this.label11.Text = "Mouse Location";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chartLegend
            // 
            chartArea2.Name = "ChartArea1";
            this.chartLegend.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartLegend.Legends.Add(legend2);
            this.chartLegend.Location = new System.Drawing.Point(83, 587);
            this.chartLegend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chartLegend.Name = "chartLegend";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartLegend.Series.Add(series2);
            this.chartLegend.Size = new System.Drawing.Size(400, 63);
            this.chartLegend.TabIndex = 38;
            this.chartLegend.Text = "chart1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(444, 31);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(151, 20);
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
            this.cboPlotSelection.Location = new System.Drawing.Point(627, 27);
            this.cboPlotSelection.Margin = new System.Windows.Forms.Padding(0);
            this.cboPlotSelection.Name = "cboPlotSelection";
            this.cboPlotSelection.Size = new System.Drawing.Size(227, 28);
            this.cboPlotSelection.TabIndex = 32;
            this.cboPlotSelection.Tag = "";
            this.cboPlotSelection.SelectedIndexChanged += new System.EventHandler(this.cboPlotSelection_SelectedIndexChanged);
            // 
            // chartResultMap
            // 
            chartArea3.Name = "ChartArea1";
            this.chartResultMap.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartResultMap.Legends.Add(legend3);
            this.chartResultMap.Location = new System.Drawing.Point(8, 48);
            this.chartResultMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chartResultMap.Name = "chartResultMap";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartResultMap.Series.Add(series3);
            this.chartResultMap.Size = new System.Drawing.Size(864, 532);
            this.chartResultMap.TabIndex = 0;
            this.chartResultMap.Text = "chart1";
            this.chartResultMap.Click += new System.EventHandler(this.chartResultMap_Click);
            this.chartResultMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartResultMap_MouseMove);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 751);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(1710, 36);
            this.statusStrip.TabIndex = 7;
            this.statusStrip.Text = "statusStrip1";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(133, 30);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // labelLED
            // 
            this.labelLED.Name = "labelLED";
            this.labelLED.Size = new System.Drawing.Size(75, 24);
            this.labelLED.Text = "LED hours";
            // 
            // labelLEDhours
            // 
            this.labelLEDhours.Name = "labelLEDhours";
            this.labelLEDhours.Size = new System.Drawing.Size(75, 24);
            this.labelLEDhours.Text = "LED Clock";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1710, 787);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.toolStrip);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximumSize = new System.Drawing.Size(1858, 841);
            this.MinimumSize = new System.Drawing.Size(1707, 832);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabAssayProtocol.ResumeLayout(false);
            this.groupBoxAnalysis.ResumeLayout(false);
            this.groupBoxAnalysis.PerformLayout();
            this.groupBoxEmission.ResumeLayout(false);
            this.groupBoxEmission.PerformLayout();
            this.groupBoxExcitation.ResumeLayout(false);
            this.groupBoxExcitation.PerformLayout();
            this.groupBoxMicroplate.ResumeLayout(false);
            this.groupBoxMicroplate.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPlate)).EndInit();
            this.tabResults.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartWaveform)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartLegend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartResultMap)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
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
        private System.Windows.Forms.ComboBox cboWavelengthB;
        private System.Windows.Forms.ComboBox cboWavelengthA;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.GroupBox groupBoxEmission;
        private System.Windows.Forms.ComboBox cboIntegration;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.ComboBox cboDetector;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.GroupBox groupBoxExcitation;
        private System.Windows.Forms.ComboBox cboLedPower;
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
        private System.Windows.Forms.TabPage tabResults;
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
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel labelLED;
        private System.Windows.Forms.ToolStripLabel labelLEDhours;
    }
}


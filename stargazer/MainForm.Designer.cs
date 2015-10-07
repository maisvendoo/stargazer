namespace stargazer
{
    partial class MainForm
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
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.itemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.itemQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.itemTools = new System.Windows.Forms.ToolStripMenuItem();
            this.itemEphemeride = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.panelEphemeride = new System.Windows.Forms.Panel();
            this.buttonEphCalc = new System.Windows.Forms.Button();
            this.textYear = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboSec = new System.Windows.Forms.ComboBox();
            this.comboMin = new System.Windows.Forms.ComboBox();
            this.comboHour = new System.Windows.Forms.ComboBox();
            this.comboDay = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BodiesList = new System.Windows.Forms.ComboBox();
            this.labelCelBody = new System.Windows.Forms.Label();
            this.groupCalcResults = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelTrueAnomaly = new System.Windows.Forms.Label();
            this.labelRadiusVector = new System.Windows.Forms.Label();
            this.labelAltitude = new System.Windows.Forms.Label();
            this.labelEccAnomaly = new System.Windows.Forms.Label();
            this.labelLat = new System.Windows.Forms.Label();
            this.labelLon = new System.Windows.Forms.Label();
            this.panelBodyPos = new System.Windows.Forms.Panel();
            this.mainMenu.SuspendLayout();
            this.panelEphemeride.SuspendLayout();
            this.groupCalcResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemFile,
            this.itemTools});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(784, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "mainMenu";
            // 
            // itemFile
            // 
            this.itemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemQuit});
            this.itemFile.Name = "itemFile";
            this.itemFile.Size = new System.Drawing.Size(37, 20);
            this.itemFile.Text = "File";
            // 
            // itemQuit
            // 
            this.itemQuit.Name = "itemQuit";
            this.itemQuit.Size = new System.Drawing.Size(97, 22);
            this.itemQuit.Text = "Quit";
            this.itemQuit.Click += new System.EventHandler(this.itemQuit_Click);
            // 
            // itemTools
            // 
            this.itemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemEphemeride});
            this.itemTools.Name = "itemTools";
            this.itemTools.Size = new System.Drawing.Size(48, 20);
            this.itemTools.Text = "Tools";
            // 
            // itemEphemeride
            // 
            this.itemEphemeride.Name = "itemEphemeride";
            this.itemEphemeride.Size = new System.Drawing.Size(189, 22);
            this.itemEphemeride.Text = "Calculate ephemeride";
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 540);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(784, 22);
            this.statusBar.TabIndex = 1;
            this.statusBar.Text = "statusBar";
            // 
            // panelEphemeride
            // 
            this.panelEphemeride.Controls.Add(this.panelBodyPos);
            this.panelEphemeride.Controls.Add(this.groupCalcResults);
            this.panelEphemeride.Controls.Add(this.buttonEphCalc);
            this.panelEphemeride.Controls.Add(this.textYear);
            this.panelEphemeride.Controls.Add(this.label6);
            this.panelEphemeride.Controls.Add(this.label5);
            this.panelEphemeride.Controls.Add(this.label4);
            this.panelEphemeride.Controls.Add(this.label3);
            this.panelEphemeride.Controls.Add(this.label2);
            this.panelEphemeride.Controls.Add(this.comboSec);
            this.panelEphemeride.Controls.Add(this.comboMin);
            this.panelEphemeride.Controls.Add(this.comboHour);
            this.panelEphemeride.Controls.Add(this.comboDay);
            this.panelEphemeride.Controls.Add(this.label1);
            this.panelEphemeride.Controls.Add(this.BodiesList);
            this.panelEphemeride.Controls.Add(this.labelCelBody);
            this.panelEphemeride.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEphemeride.Location = new System.Drawing.Point(0, 24);
            this.panelEphemeride.Name = "panelEphemeride";
            this.panelEphemeride.Size = new System.Drawing.Size(784, 516);
            this.panelEphemeride.TabIndex = 2;
            // 
            // buttonEphCalc
            // 
            this.buttonEphCalc.Location = new System.Drawing.Point(654, 71);
            this.buttonEphCalc.Name = "buttonEphCalc";
            this.buttonEphCalc.Size = new System.Drawing.Size(75, 23);
            this.buttonEphCalc.TabIndex = 14;
            this.buttonEphCalc.Text = "Calculate";
            this.buttonEphCalc.UseVisualStyleBackColor = true;
            this.buttonEphCalc.Click += new System.EventHandler(this.buttonEphCalc_Click);
            // 
            // textYear
            // 
            this.textYear.Location = new System.Drawing.Point(220, 77);
            this.textYear.Name = "textYear";
            this.textYear.Size = new System.Drawing.Size(71, 20);
            this.textYear.TabIndex = 13;
            this.textYear.Text = "1";
            this.textYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(591, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "s";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(520, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "m";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(453, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "h";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(383, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "d";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "y";
            // 
            // comboSec
            // 
            this.comboSec.FormattingEnabled = true;
            this.comboSec.Location = new System.Drawing.Point(539, 76);
            this.comboSec.Name = "comboSec";
            this.comboSec.Size = new System.Drawing.Size(46, 21);
            this.comboSec.TabIndex = 7;
            // 
            // comboMin
            // 
            this.comboMin.FormattingEnabled = true;
            this.comboMin.Location = new System.Drawing.Point(472, 76);
            this.comboMin.Name = "comboMin";
            this.comboMin.Size = new System.Drawing.Size(46, 21);
            this.comboMin.TabIndex = 6;
            // 
            // comboHour
            // 
            this.comboHour.FormattingEnabled = true;
            this.comboHour.Location = new System.Drawing.Point(401, 76);
            this.comboHour.Name = "comboHour";
            this.comboHour.Size = new System.Drawing.Size(46, 21);
            this.comboHour.TabIndex = 5;
            // 
            // comboDay
            // 
            this.comboDay.FormattingEnabled = true;
            this.comboDay.Location = new System.Drawing.Point(315, 76);
            this.comboDay.Name = "comboDay";
            this.comboDay.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboDay.Size = new System.Drawing.Size(64, 21);
            this.comboDay.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(216, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Kerbal system UT";
            // 
            // BodiesList
            // 
            this.BodiesList.FormattingEnabled = true;
            this.BodiesList.Location = new System.Drawing.Point(45, 76);
            this.BodiesList.Name = "BodiesList";
            this.BodiesList.Size = new System.Drawing.Size(121, 21);
            this.BodiesList.TabIndex = 1;
            this.BodiesList.SelectedIndexChanged += new System.EventHandler(this.BodiesList_SelectedIndexChanged);
            // 
            // labelCelBody
            // 
            this.labelCelBody.AutoSize = true;
            this.labelCelBody.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCelBody.Location = new System.Drawing.Point(41, 43);
            this.labelCelBody.Name = "labelCelBody";
            this.labelCelBody.Size = new System.Drawing.Size(109, 20);
            this.labelCelBody.TabIndex = 0;
            this.labelCelBody.Text = "Celestial Body";
            // 
            // groupCalcResults
            // 
            this.groupCalcResults.Controls.Add(this.labelLon);
            this.groupCalcResults.Controls.Add(this.labelLat);
            this.groupCalcResults.Controls.Add(this.labelEccAnomaly);
            this.groupCalcResults.Controls.Add(this.labelAltitude);
            this.groupCalcResults.Controls.Add(this.labelRadiusVector);
            this.groupCalcResults.Controls.Add(this.labelTrueAnomaly);
            this.groupCalcResults.Controls.Add(this.label12);
            this.groupCalcResults.Controls.Add(this.label11);
            this.groupCalcResults.Controls.Add(this.label10);
            this.groupCalcResults.Controls.Add(this.label9);
            this.groupCalcResults.Controls.Add(this.label8);
            this.groupCalcResults.Controls.Add(this.label7);
            this.groupCalcResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupCalcResults.Location = new System.Drawing.Point(12, 132);
            this.groupCalcResults.Name = "groupCalcResults";
            this.groupCalcResults.Size = new System.Drawing.Size(310, 367);
            this.groupCalcResults.TabIndex = 15;
            this.groupCalcResults.TabStop = false;
            this.groupCalcResults.Text = "Body position";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(21, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 16);
            this.label7.TabIndex = 0;
            this.label7.Text = "True anomaly:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(21, 177);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 16);
            this.label8.TabIndex = 1;
            this.label8.Text = "Eccentric anomaly:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(21, 85);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 16);
            this.label9.TabIndex = 2;
            this.label9.Text = "Radius-vector:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(21, 141);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 16);
            this.label10.TabIndex = 3;
            this.label10.Text = "Altitude:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(21, 230);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 16);
            this.label11.TabIndex = 4;
            this.label11.Text = "Ecliptic latitude:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(21, 269);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(115, 16);
            this.label12.TabIndex = 5;
            this.label12.Text = "Ecliptic longtitude:";
            // 
            // labelTrueAnomaly
            // 
            this.labelTrueAnomaly.AutoSize = true;
            this.labelTrueAnomaly.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTrueAnomaly.Location = new System.Drawing.Point(156, 44);
            this.labelTrueAnomaly.Name = "labelTrueAnomaly";
            this.labelTrueAnomaly.Size = new System.Drawing.Size(15, 16);
            this.labelTrueAnomaly.TabIndex = 6;
            this.labelTrueAnomaly.Text = "0";
            // 
            // labelRadiusVector
            // 
            this.labelRadiusVector.AutoSize = true;
            this.labelRadiusVector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelRadiusVector.Location = new System.Drawing.Point(156, 85);
            this.labelRadiusVector.Name = "labelRadiusVector";
            this.labelRadiusVector.Size = new System.Drawing.Size(15, 16);
            this.labelRadiusVector.TabIndex = 7;
            this.labelRadiusVector.Text = "0";
            // 
            // labelAltitude
            // 
            this.labelAltitude.AutoSize = true;
            this.labelAltitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelAltitude.Location = new System.Drawing.Point(156, 141);
            this.labelAltitude.Name = "labelAltitude";
            this.labelAltitude.Size = new System.Drawing.Size(15, 16);
            this.labelAltitude.TabIndex = 8;
            this.labelAltitude.Text = "0";
            // 
            // labelEccAnomaly
            // 
            this.labelEccAnomaly.AutoSize = true;
            this.labelEccAnomaly.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelEccAnomaly.Location = new System.Drawing.Point(156, 177);
            this.labelEccAnomaly.Name = "labelEccAnomaly";
            this.labelEccAnomaly.Size = new System.Drawing.Size(15, 16);
            this.labelEccAnomaly.TabIndex = 9;
            this.labelEccAnomaly.Text = "0";
            // 
            // labelLat
            // 
            this.labelLat.AutoSize = true;
            this.labelLat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLat.Location = new System.Drawing.Point(156, 230);
            this.labelLat.Name = "labelLat";
            this.labelLat.Size = new System.Drawing.Size(15, 16);
            this.labelLat.TabIndex = 10;
            this.labelLat.Text = "0";
            // 
            // labelLon
            // 
            this.labelLon.AutoSize = true;
            this.labelLon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLon.Location = new System.Drawing.Point(156, 269);
            this.labelLon.Name = "labelLon";
            this.labelLon.Size = new System.Drawing.Size(15, 16);
            this.labelLon.TabIndex = 11;
            this.labelLon.Text = "0";
            // 
            // panelBodyPos
            // 
            this.panelBodyPos.Location = new System.Drawing.Point(335, 142);
            this.panelBodyPos.Name = "panelBodyPos";
            this.panelBodyPos.Size = new System.Drawing.Size(437, 357);
            this.panelBodyPos.TabIndex = 16;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panelEphemeride);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Stargazer";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.panelEphemeride.ResumeLayout(false);
            this.panelEphemeride.PerformLayout();
            this.groupCalcResults.ResumeLayout(false);
            this.groupCalcResults.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem itemFile;
        private System.Windows.Forms.ToolStripMenuItem itemQuit;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripMenuItem itemTools;
        private System.Windows.Forms.ToolStripMenuItem itemEphemeride;
        private System.Windows.Forms.Panel panelEphemeride;
        private System.Windows.Forms.ComboBox BodiesList;
        private System.Windows.Forms.Label labelCelBody;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboSec;
        private System.Windows.Forms.ComboBox comboMin;
        private System.Windows.Forms.ComboBox comboHour;
        private System.Windows.Forms.ComboBox comboDay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textYear;
        private System.Windows.Forms.Button buttonEphCalc;
        private System.Windows.Forms.GroupBox groupCalcResults;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelLon;
        private System.Windows.Forms.Label labelLat;
        private System.Windows.Forms.Label labelEccAnomaly;
        private System.Windows.Forms.Label labelAltitude;
        private System.Windows.Forms.Label labelRadiusVector;
        private System.Windows.Forms.Label labelTrueAnomaly;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panelBodyPos;

    }
}


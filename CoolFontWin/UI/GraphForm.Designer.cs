using ZedGraph;
using System;
using System.Drawing;

namespace CoolFont.UI
{
    partial class GraphForm
    {
        public CoolFontWin Cfw { get; set; }
        private ZedGraph.ZedGraphControl z1;
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
            this.z1 = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // zedGraphControl
            // 
            this.z1.Location = new System.Drawing.Point(0, 0);
            this.z1.Name = "zedGraphControl1";
            this.z1.Size = new System.Drawing.Size(680, 414);
            this.z1.TabIndex = 0;
            // 
            // Form
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(680, 414);
            this.Controls.Add(this.z1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.Load += new System.EventHandler(GraphForm_Load);
        }

        private void GraphForm_Load(object sender, System.EventArgs e)
        {
            if (this.Cfw != null)
            {
                //get the data
            }
            z1.IsShowPointValues = true;
            //z1.GraphPane.Title = "Test Case for C#";
            double[] x = new double[100];
            double[] y = new double[100];
            int i;
            for (i = 0; i < 100; i++)
            {
                x[i] = (double)i / 100.0 * Math.PI * 2.0;
                y[i] = Math.Sin(x[i]);
            }
            z1.GraphPane.AddCurve("Sine Wave", x, y, Color.Red, SymbolType.Square);
            z1.AxisChange();
            z1.Invalidate();
        }

        #endregion
    }
}
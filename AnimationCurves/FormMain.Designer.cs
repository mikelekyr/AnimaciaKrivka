using AnimationCurves.UserControls;

namespace AnimationCurves
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            radioButtonBezierCurve = new RadioButton();
            radioButtonBezierSpline = new RadioButton();
            groupBoxState = new GroupBox();
            radioButtonDeleteNode = new RadioButton();
            radioButtonInsertNode = new RadioButton();
            radioButtonEdit = new RadioButton();
            doubleBufferPanel = new DoubleBufferPanel();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBoxState.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(groupBoxState);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(190, 728);
            panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButtonBezierCurve);
            groupBox1.Controls.Add(radioButtonBezierSpline);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(161, 74);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Curve type";
            // 
            // radioButtonBezierCurve
            // 
            radioButtonBezierCurve.AutoSize = true;
            radioButtonBezierCurve.Checked = true;
            radioButtonBezierCurve.Location = new Point(11, 24);
            radioButtonBezierCurve.Name = "radioButtonBezierCurve";
            radioButtonBezierCurve.Size = new Size(88, 19);
            radioButtonBezierCurve.TabIndex = 1;
            radioButtonBezierCurve.TabStop = true;
            radioButtonBezierCurve.Text = "Bezier curve";
            radioButtonBezierCurve.UseVisualStyleBackColor = true;
            radioButtonBezierCurve.CheckedChanged += RadioButtonBezierCurve_CheckedChanged;
            // 
            // radioButtonBezierSpline
            // 
            radioButtonBezierSpline.AutoSize = true;
            radioButtonBezierSpline.Location = new Point(11, 49);
            radioButtonBezierSpline.Name = "radioButtonBezierSpline";
            radioButtonBezierSpline.Size = new Size(90, 19);
            radioButtonBezierSpline.TabIndex = 0;
            radioButtonBezierSpline.Text = "Bezier spline";
            radioButtonBezierSpline.UseVisualStyleBackColor = true;
            radioButtonBezierSpline.CheckedChanged += RadioButtonBezierSpline_CheckedChanged;
            // 
            // groupBoxState
            // 
            groupBoxState.Controls.Add(radioButtonDeleteNode);
            groupBoxState.Controls.Add(radioButtonInsertNode);
            groupBoxState.Controls.Add(radioButtonEdit);
            groupBoxState.Location = new Point(12, 92);
            groupBoxState.Name = "groupBoxState";
            groupBoxState.Size = new Size(161, 103);
            groupBoxState.TabIndex = 3;
            groupBoxState.TabStop = false;
            groupBoxState.Text = "Editor state";
            // 
            // radioButtonDeleteNode
            // 
            radioButtonDeleteNode.AutoSize = true;
            radioButtonDeleteNode.Location = new Point(11, 72);
            radioButtonDeleteNode.Name = "radioButtonDeleteNode";
            radioButtonDeleteNode.Size = new Size(119, 19);
            radioButtonDeleteNode.TabIndex = 2;
            radioButtonDeleteNode.Text = "Insert delete node";
            radioButtonDeleteNode.UseVisualStyleBackColor = true;
            radioButtonDeleteNode.CheckedChanged += RadioButtonDeleteNode_CheckedChanged;
            // 
            // radioButtonInsertNode
            // 
            radioButtonInsertNode.AutoSize = true;
            radioButtonInsertNode.Location = new Point(11, 47);
            radioButtonInsertNode.Name = "radioButtonInsertNode";
            radioButtonInsertNode.Size = new Size(84, 19);
            radioButtonInsertNode.TabIndex = 1;
            radioButtonInsertNode.Text = "Insert node";
            radioButtonInsertNode.UseVisualStyleBackColor = true;
            radioButtonInsertNode.CheckedChanged += RadioButtonInsertNode_CheckedChanged;
            // 
            // radioButtonEdit
            // 
            radioButtonEdit.AutoSize = true;
            radioButtonEdit.Checked = true;
            radioButtonEdit.Location = new Point(11, 22);
            radioButtonEdit.Name = "radioButtonEdit";
            radioButtonEdit.Size = new Size(45, 19);
            radioButtonEdit.TabIndex = 0;
            radioButtonEdit.TabStop = true;
            radioButtonEdit.Text = "Edit";
            radioButtonEdit.UseVisualStyleBackColor = true;
            radioButtonEdit.CheckedChanged += RadioButtonEdit_CheckedChanged;
            // 
            // doubleBufferPanel
            // 
            doubleBufferPanel.BackColor = Color.White;
            doubleBufferPanel.Dock = DockStyle.Fill;
            doubleBufferPanel.Location = new Point(190, 0);
            doubleBufferPanel.Name = "doubleBufferPanel";
            doubleBufferPanel.Size = new Size(1168, 728);
            doubleBufferPanel.TabIndex = 1;
            doubleBufferPanel.Paint += DoubleBufferPanel_Paint;
            doubleBufferPanel.MouseDown += DoubleBufferPanel_MouseDown;
            doubleBufferPanel.MouseMove += DoubleBufferPanel_MouseMove;
            doubleBufferPanel.MouseUp += DoubleBufferPanel_MouseUp;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1358, 728);
            Controls.Add(doubleBufferPanel);
            Controls.Add(panel1);
            KeyPreview = true;
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Animation and curves";
            KeyDown += FormMain_KeyDown;
            KeyUp += FormMain_KeyUp;
            panel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBoxState.ResumeLayout(false);
            groupBoxState.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private GroupBox groupBoxState;
        private RadioButton radioButtonDeleteNode;
        private RadioButton radioButtonInsertNode;
        private RadioButton radioButtonEdit;
        private DoubleBufferPanel doubleBufferPanel;
        private GroupBox groupBox1;
        private RadioButton radioButtonBezierSpline;
        private RadioButton radioButtonBezierCurve;
    }
}

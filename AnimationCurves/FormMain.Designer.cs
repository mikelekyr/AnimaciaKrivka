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
            groupBoxState = new GroupBox();
            radioButtonDeleteNode = new RadioButton();
            radioButtonInsertNode = new RadioButton();
            radioButtonEdit = new RadioButton();
            doubleBufferPanel2 = new DoubleBufferPanel();
            panel1.SuspendLayout();
            groupBoxState.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBoxState);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(190, 450);
            panel1.TabIndex = 0;
            // 
            // groupBoxState
            // 
            groupBoxState.Controls.Add(radioButtonDeleteNode);
            groupBoxState.Controls.Add(radioButtonInsertNode);
            groupBoxState.Controls.Add(radioButtonEdit);
            groupBoxState.Location = new Point(12, 12);
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
            radioButtonDeleteNode.CheckedChanged += radioButtonDeleteNode_CheckedChanged;
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
            radioButtonInsertNode.CheckedChanged += radioButtonInsertNode_CheckedChanged;
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
            radioButtonEdit.CheckedChanged += radioButtonEdit_CheckedChanged;
            // 
            // doubleBufferPanel2
            // 
            doubleBufferPanel2.BackColor = Color.White;
            doubleBufferPanel2.Dock = DockStyle.Fill;
            doubleBufferPanel2.Location = new Point(190, 0);
            doubleBufferPanel2.Name = "doubleBufferPanel2";
            doubleBufferPanel2.Size = new Size(610, 450);
            doubleBufferPanel2.TabIndex = 1;
            doubleBufferPanel2.Paint += doubleBufferPanel1_Paint;
            doubleBufferPanel2.MouseDown += doubleBufferPanel2_MouseDown;
            doubleBufferPanel2.MouseMove += doubleBufferPanel2_MouseMove;
            doubleBufferPanel2.MouseUp += doubleBufferPanel2_MouseUp;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(doubleBufferPanel2);
            Controls.Add(panel1);
            KeyPreview = true;
            Name = "FormMain";
            Text = "Animation and curves";
            Load += Form1_Load;
            KeyDown += FormMain_KeyDown;
            panel1.ResumeLayout(false);
            groupBoxState.ResumeLayout(false);
            groupBoxState.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DoubleBufferPanel doubleBufferPanel1;
        private GroupBox groupBoxState;
        private RadioButton radioButtonDeleteNode;
        private RadioButton radioButtonInsertNode;
        private RadioButton radioButtonEdit;
        private DoubleBufferPanel doubleBufferPanel2;
    }
}

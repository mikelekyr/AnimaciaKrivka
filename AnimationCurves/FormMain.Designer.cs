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
            doubleBufferPanel1 = new DoubleBufferPanel();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(190, 450);
            panel1.TabIndex = 0;
            // 
            // doubleBufferPanel1
            // 
            doubleBufferPanel1.BackColor = Color.White;
            doubleBufferPanel1.Location = new Point(193, 0);
            doubleBufferPanel1.Name = "doubleBufferPanel1";
            doubleBufferPanel1.Size = new Size(606, 450);
            doubleBufferPanel1.TabIndex = 1;
            doubleBufferPanel1.Paint += doubleBufferPanel1_Paint;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(doubleBufferPanel1);
            Controls.Add(panel1);
            Name = "FormMain";
            Text = "Animation and curves";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DoubleBufferPanel doubleBufferPanel1;
    }
}


using System.Drawing;

namespace multiBaseCalc
{
    partial class Form1
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
            display = new DisplayControl();
            labelBase = new System.Windows.Forms.Label();
            SuspendLayout();
            //
            // display
            //
            display.Location = new Point(20, 9);
            display.Size = new Size(531, 63);
            display.Font = new System.Drawing.Font("Consolas", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            display.KeyDown += Form1_KeyDown;
            // 
            // labelBase
            // 
            labelBase.AutoSize = true;
            labelBase.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            labelBase.Location = new System.Drawing.Point(520, 190);
            labelBase.Name = "labelBase";
            labelBase.Size = new System.Drawing.Size(38, 15);
            labelBase.TabIndex = 1;
            labelBase.Text = "label2";
            labelBase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(630, 460);
            Controls.Add(labelBase);
            Controls.Add(display);
            Name = "Form1";
            Text = "Multi Base Calc";
            KeyDown += Form1_KeyDown;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            MaximizeBox = false;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DisplayControl display;
        private System.Windows.Forms.Label labelBase;
    }
}


namespace Ex05_Othello.UI
{
    public partial class FormGameSettings
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
            this.buttonBoardSize = new System.Windows.Forms.Button();
            this.buttonPlayHumanVsPC = new System.Windows.Forms.Button();
            this.buttonPlayHumanVsHuman = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonBoardSize
            // 
            this.buttonBoardSize.Location = new System.Drawing.Point(9, 20);
            this.buttonBoardSize.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBoardSize.Name = "buttonBoardSize";
            this.buttonBoardSize.Size = new System.Drawing.Size(319, 41);
            this.buttonBoardSize.TabIndex = 0;
            this.buttonBoardSize.Text = "Board size: 6x6 (click to increase)";
            this.buttonBoardSize.UseVisualStyleBackColor = true;
            this.buttonBoardSize.Click += new System.EventHandler(this.buttonBoardSize_Click);
            // 
            // buttonPlayHumanVsPC
            // 
            this.buttonPlayHumanVsPC.Location = new System.Drawing.Point(9, 98);
            this.buttonPlayHumanVsPC.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPlayHumanVsPC.Name = "buttonPlayHumanVsPC";
            this.buttonPlayHumanVsPC.Size = new System.Drawing.Size(154, 41);
            this.buttonPlayHumanVsPC.TabIndex = 1;
            this.buttonPlayHumanVsPC.Text = "Play agianst the computer";
            this.buttonPlayHumanVsPC.UseVisualStyleBackColor = true;
            this.buttonPlayHumanVsPC.Click += new System.EventHandler(this.buttonPlayHumanVsPC_Click);
            // 
            // buttonPlayHumanVsHuman
            // 
            this.buttonPlayHumanVsHuman.Location = new System.Drawing.Point(174, 98);
            this.buttonPlayHumanVsHuman.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPlayHumanVsHuman.Name = "buttonPlayHumanVsHuman";
            this.buttonPlayHumanVsHuman.Size = new System.Drawing.Size(154, 41);
            this.buttonPlayHumanVsHuman.TabIndex = 2;
            this.buttonPlayHumanVsHuman.Text = "Play agianst your friend";
            this.buttonPlayHumanVsHuman.UseVisualStyleBackColor = true;
            this.buttonPlayHumanVsHuman.Click += new System.EventHandler(this.buttonPlayHumanVsHuman_Click);
            // 
            // FormGameSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(337, 160);
            this.Controls.Add(this.buttonPlayHumanVsHuman);
            this.Controls.Add(this.buttonPlayHumanVsPC);
            this.Controls.Add(this.buttonBoardSize);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "FormGameSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Othello - Game Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonBoardSize;
        private System.Windows.Forms.Button buttonPlayHumanVsPC;
        private System.Windows.Forms.Button buttonPlayHumanVsHuman;
    }
}
namespace Chess
{
    partial class ModePicker
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
            this.btn_PvP = new System.Windows.Forms.Button();
            this.btn_PvAI = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_PvP
            // 
            this.btn_PvP.BackgroundImage = global::Chess.Properties.Resources.human_hand;
            this.btn_PvP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_PvP.Font = new System.Drawing.Font("Felix Titling", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_PvP.Location = new System.Drawing.Point(12, 12);
            this.btn_PvP.Name = "btn_PvP";
            this.btn_PvP.Size = new System.Drawing.Size(237, 472);
            this.btn_PvP.TabIndex = 1;
            this.btn_PvP.Text = "HUMAN VS. HUMAN";
            this.btn_PvP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_PvP.UseVisualStyleBackColor = true;
            this.btn_PvP.Click += new System.EventHandler(this.btn_PvP_Click);
            // 
            // btn_PvAI
            // 
            this.btn_PvAI.BackgroundImage = global::Chess.Properties.Resources.ai_hand;
            this.btn_PvAI.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_PvAI.Font = new System.Drawing.Font("Felix Titling", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_PvAI.Location = new System.Drawing.Point(255, 12);
            this.btn_PvAI.Name = "btn_PvAI";
            this.btn_PvAI.Size = new System.Drawing.Size(231, 472);
            this.btn_PvAI.TabIndex = 2;
            this.btn_PvAI.Text = "HUMAN VS. ROBOT";
            this.btn_PvAI.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_PvAI.UseVisualStyleBackColor = true;
            this.btn_PvAI.Click += new System.EventHandler(this.btn_PvAI_Click);
            // 
            // ModePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(498, 496);
            this.ControlBox = false;
            this.Controls.Add(this.btn_PvAI);
            this.Controls.Add(this.btn_PvP);
            this.MaximizeBox = false;
            this.Name = "ModePicker";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Mode";
            this.Load += new System.EventHandler(this.ModePicker_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_PvP;
        private System.Windows.Forms.Button btn_PvAI;
    }
}
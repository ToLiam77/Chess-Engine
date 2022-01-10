namespace Chess
{
    partial class PromotionPiecePicker
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
            this.btn_queen = new System.Windows.Forms.Button();
            this.btn_rook = new System.Windows.Forms.Button();
            this.btn_bishop = new System.Windows.Forms.Button();
            this.btn_knight = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_queen
            // 
            this.btn_queen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_queen.Location = new System.Drawing.Point(12, 12);
            this.btn_queen.Name = "btn_queen";
            this.btn_queen.Size = new System.Drawing.Size(90, 90);
            this.btn_queen.TabIndex = 0;
            this.btn_queen.UseVisualStyleBackColor = true;
            this.btn_queen.Click += new System.EventHandler(this.Queen_Clicked);
            // 
            // btn_rook
            // 
            this.btn_rook.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_rook.Location = new System.Drawing.Point(108, 12);
            this.btn_rook.Name = "btn_rook";
            this.btn_rook.Size = new System.Drawing.Size(90, 90);
            this.btn_rook.TabIndex = 1;
            this.btn_rook.UseVisualStyleBackColor = true;
            this.btn_rook.Click += new System.EventHandler(this.Rook_Clicked);
            // 
            // btn_bishop
            // 
            this.btn_bishop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_bishop.Location = new System.Drawing.Point(12, 108);
            this.btn_bishop.Name = "btn_bishop";
            this.btn_bishop.Size = new System.Drawing.Size(90, 90);
            this.btn_bishop.TabIndex = 2;
            this.btn_bishop.UseVisualStyleBackColor = true;
            this.btn_bishop.Click += new System.EventHandler(this.Bishop_Clicked);
            // 
            // btn_knight
            // 
            this.btn_knight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_knight.Location = new System.Drawing.Point(108, 108);
            this.btn_knight.Name = "btn_knight";
            this.btn_knight.Size = new System.Drawing.Size(90, 90);
            this.btn_knight.TabIndex = 3;
            this.btn_knight.UseVisualStyleBackColor = true;
            this.btn_knight.Click += new System.EventHandler(this.Knight_Clicked);
            // 
            // PromotionPiecePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 209);
            this.ControlBox = false;
            this.Controls.Add(this.btn_knight);
            this.Controls.Add(this.btn_bishop);
            this.Controls.Add(this.btn_rook);
            this.Controls.Add(this.btn_queen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PromotionPiecePicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Promotion piece";
            this.Load += new System.EventHandler(this.PromotionPiecePicker_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_queen;
        private System.Windows.Forms.Button btn_rook;
        private System.Windows.Forms.Button btn_bishop;
        private System.Windows.Forms.Button btn_knight;
    }
}
namespace BarnCaseApi
{
    partial class Production
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Production));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnSellProducts = new System.Windows.Forms.Button();
            this.lblMilk = new System.Windows.Forms.Label();
            this.lblEggs = new System.Windows.Forms.Label();
            this.lblWool = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(355, 82);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // btnSellProducts
            // 
            this.btnSellProducts.Location = new System.Drawing.Point(160, 373);
            this.btnSellProducts.Name = "btnSellProducts";
            this.btnSellProducts.Size = new System.Drawing.Size(264, 36);
            this.btnSellProducts.TabIndex = 1;
            this.btnSellProducts.Text = "Sell All Products";
            this.btnSellProducts.UseVisualStyleBackColor = true;
            // 
            // lblMilk
            // 
            this.lblMilk.AutoSize = true;
            this.lblMilk.Location = new System.Drawing.Point(44, 220);
            this.lblMilk.Name = "lblMilk";
            this.lblMilk.Size = new System.Drawing.Size(53, 20);
            this.lblMilk.TabIndex = 2;
            this.lblMilk.Text = "Milk: 0";
            // 
            // lblEggs
            // 
            this.lblEggs.AutoSize = true;
            this.lblEggs.Location = new System.Drawing.Point(34, 257);
            this.lblEggs.Name = "lblEggs";
            this.lblEggs.Size = new System.Drawing.Size(63, 20);
            this.lblEggs.TabIndex = 3;
            this.lblEggs.Text = "Eggs: 0";
            // 
            // lblWool
            // 
            this.lblWool.AutoSize = true;
            this.lblWool.Location = new System.Drawing.Point(31, 294);
            this.lblWool.Name = "lblWool";
            this.lblWool.Size = new System.Drawing.Size(66, 20);
            this.lblWool.TabIndex = 4;
            this.lblWool.Text = "Wool: 0 ";
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(239, 450);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(90, 20);
            this.lblBalance.TabIndex = 6;
            this.lblBalance.Text = "Cash: $500";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(24, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(101, 50);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // Production
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 507);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.lblWool);
            this.Controls.Add(this.lblEggs);
            this.Controls.Add(this.lblMilk);
            this.Controls.Add(this.btnSellProducts);
            this.Controls.Add(this.progressBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Production";
            this.Text = "Production";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnSellProducts;
        private System.Windows.Forms.Label lblMilk;
        private System.Windows.Forms.Label lblEggs;
        private System.Windows.Forms.Label lblWool;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
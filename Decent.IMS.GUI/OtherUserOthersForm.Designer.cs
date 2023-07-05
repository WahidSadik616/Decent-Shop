namespace Decent.IMS.GUI
{
    partial class OtherUserOthersForm
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
            this.btnCost = new MetroFramework.Controls.MetroButton();
            this.btnPayment = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // btnCost
            // 
            this.btnCost.Location = new System.Drawing.Point(165, 158);
            this.btnCost.Name = "btnCost";
            this.btnCost.Size = new System.Drawing.Size(193, 32);
            this.btnCost.TabIndex = 0;
            this.btnCost.Text = "Cost";
            this.btnCost.UseSelectable = true;
            this.btnCost.Click += new System.EventHandler(this.btnCost_Click);
            // 
            // btnPayment
            // 
            this.btnPayment.Location = new System.Drawing.Point(165, 218);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(193, 32);
            this.btnPayment.TabIndex = 1;
            this.btnPayment.Text = "Payment";
            this.btnPayment.UseSelectable = true;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // OtherUserOthersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 514);
            this.Controls.Add(this.btnPayment);
            this.Controls.Add(this.btnCost);
            this.Name = "OtherUserOthersForm";
            this.Text = "Others Form";
            this.Load += new System.EventHandler(this.OtherUserOthersForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroButton btnCost;
        private MetroFramework.Controls.MetroButton btnPayment;
    }
}
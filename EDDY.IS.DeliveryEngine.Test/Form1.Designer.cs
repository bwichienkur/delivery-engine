namespace EDDY.IS.DeliveryEngine.Test
{
    partial class Form1
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
            this.btnStartLeadProcessing = new System.Windows.Forms.Button();
            this.btnStartBatchProcessing = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartLeadProcessing
            // 
            this.btnStartLeadProcessing.Location = new System.Drawing.Point(103, 118);
            this.btnStartLeadProcessing.Name = "btnStartLeadProcessing";
            this.btnStartLeadProcessing.Size = new System.Drawing.Size(169, 23);
            this.btnStartLeadProcessing.TabIndex = 0;
            this.btnStartLeadProcessing.Text = "Start Lead Processing";
            this.btnStartLeadProcessing.UseVisualStyleBackColor = true;
            this.btnStartLeadProcessing.Click += new System.EventHandler(this.btnStartLeadProcessing_Click);
            // 
            // btnStartBatchProcessing
            // 
            this.btnStartBatchProcessing.Location = new System.Drawing.Point(103, 164);
            this.btnStartBatchProcessing.Name = "btnStartBatchProcessing";
            this.btnStartBatchProcessing.Size = new System.Drawing.Size(169, 23);
            this.btnStartBatchProcessing.TabIndex = 1;
            this.btnStartBatchProcessing.Text = "Start Batch Processing";
            this.btnStartBatchProcessing.UseVisualStyleBackColor = true;
            this.btnStartBatchProcessing.Click += new System.EventHandler(this.btnStartBatchProcessing_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnStartBatchProcessing);
            this.Controls.Add(this.btnStartLeadProcessing);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartLeadProcessing;
        private System.Windows.Forms.Button btnStartBatchProcessing;
    }
}


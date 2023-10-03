
namespace S4M
{
    partial class Main
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPartia = new System.Windows.Forms.Button();
            this.btnSymZam = new System.Windows.Forms.Button();
            this.btnRecept = new System.Windows.Forms.Button();
            this.btnSped = new System.Windows.Forms.Button();
            this.btnZPMD = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnPartia);
            this.flowLayoutPanel1.Controls.Add(this.btnZPMD);
            this.flowLayoutPanel1.Controls.Add(this.btnSymZam);
            this.flowLayoutPanel1.Controls.Add(this.btnRecept);
            this.flowLayoutPanel1.Controls.Add(this.btnSped);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(458, 460);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnPartia
            // 
            this.btnPartia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPartia.Location = new System.Drawing.Point(3, 3);
            this.btnPartia.Name = "btnPartia";
            this.btnPartia.Size = new System.Drawing.Size(452, 60);
            this.btnPartia.TabIndex = 3;
            this.btnPartia.Text = "Symfonia - Partie";
            this.btnPartia.UseVisualStyleBackColor = true;
            this.btnPartia.Click += new System.EventHandler(this.btnPartia_Click);
            // 
            // btnSymZam
            // 
            this.btnSymZam.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnSymZam.Location = new System.Drawing.Point(3, 135);
            this.btnSymZam.Name = "btnSymZam";
            this.btnSymZam.Size = new System.Drawing.Size(452, 60);
            this.btnSymZam.TabIndex = 0;
            this.btnSymZam.Text = "Symfonia - Zamówienia";
            this.btnSymZam.UseVisualStyleBackColor = true;
            this.btnSymZam.Click += new System.EventHandler(this.btnSymZam_Click);
            // 
            // btnRecept
            // 
            this.btnRecept.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRecept.Location = new System.Drawing.Point(3, 201);
            this.btnRecept.Name = "btnRecept";
            this.btnRecept.Size = new System.Drawing.Size(452, 60);
            this.btnRecept.TabIndex = 1;
            this.btnRecept.Text = "Symfonia - Receptury";
            this.btnRecept.UseVisualStyleBackColor = true;
            this.btnRecept.Click += new System.EventHandler(this.btnRecept_Click);
            // 
            // btnSped
            // 
            this.btnSped.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnSped.Location = new System.Drawing.Point(3, 267);
            this.btnSped.Name = "btnSped";
            this.btnSped.Size = new System.Drawing.Size(452, 60);
            this.btnSped.TabIndex = 2;
            this.btnSped.Text = "Spedycja - Fresh";
            this.btnSped.UseVisualStyleBackColor = true;
            this.btnSped.Click += new System.EventHandler(this.btnSped_Click);
            // 
            // btnZPMD
            // 
            this.btnZPMD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnZPMD.Location = new System.Drawing.Point(3, 69);
            this.btnZPMD.Name = "btnZPMD";
            this.btnZPMD.Size = new System.Drawing.Size(452, 60);
            this.btnZPMD.TabIndex = 4;
            this.btnZPMD.Text = "Symfonia - ZPMD";
            this.btnZPMD.UseVisualStyleBackColor = true;
            this.btnZPMD.Click += new System.EventHandler(this.btnZPMD_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 484);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "S4M";
            this.Load += new System.EventHandler(this.Main_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnSymZam;
        private System.Windows.Forms.Button btnRecept;
        private System.Windows.Forms.Button btnSped;
        private System.Windows.Forms.Button btnPartia;
        private System.Windows.Forms.Button btnZPMD;
    }
}


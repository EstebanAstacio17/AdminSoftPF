namespace AdminSoftPF
{
    partial class Manzana
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDetalle = new System.Windows.Forms.TextBox();
            this.txtManzana = new System.Windows.Forms.TextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.dgvManzana = new System.Windows.Forms.DataGridView();
            this.btnActualizarManzana = new System.Windows.Forms.Button();
            this.btnEstadoManzana = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManzana)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 20);
            this.label3.TabIndex = 31;
            this.label3.Text = "Manzana";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(41, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 30;
            this.label2.Text = "Detalle:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 288);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 29;
            this.label1.Text = "Manzana:";
            // 
            // txtDetalle
            // 
            this.txtDetalle.BackColor = System.Drawing.Color.White;
            this.txtDetalle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetalle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetalle.Location = new System.Drawing.Point(106, 332);
            this.txtDetalle.Name = "txtDetalle";
            this.txtDetalle.Size = new System.Drawing.Size(180, 28);
            this.txtDetalle.TabIndex = 28;
            // 
            // txtManzana
            // 
            this.txtManzana.BackColor = System.Drawing.Color.White;
            this.txtManzana.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtManzana.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtManzana.Location = new System.Drawing.Point(106, 284);
            this.txtManzana.Name = "txtManzana";
            this.txtManzana.Size = new System.Drawing.Size(180, 28);
            this.txtManzana.TabIndex = 27;
            this.txtManzana.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtManzana_KeyPress);
            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.SeaGreen;
            this.btnAgregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.ForeColor = System.Drawing.Color.White;
            this.btnAgregar.Location = new System.Drawing.Point(296, 278);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(112, 41);
            this.btnAgregar.TabIndex = 25;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // dgvManzana
            // 
            this.dgvManzana.AllowUserToAddRows = false;
            this.dgvManzana.AllowUserToDeleteRows = false;
            this.dgvManzana.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvManzana.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvManzana.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvManzana.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvManzana.Location = new System.Drawing.Point(12, 32);
            this.dgvManzana.MultiSelect = false;
            this.dgvManzana.Name = "dgvManzana";
            this.dgvManzana.ReadOnly = true;
            this.dgvManzana.RowHeadersVisible = false;
            this.dgvManzana.RowHeadersWidth = 51;
            this.dgvManzana.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvManzana.Size = new System.Drawing.Size(396, 240);
            this.dgvManzana.TabIndex = 24;
            this.dgvManzana.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvManzana_CellClick);
            // 
            // btnActualizarManzana
            // 
            this.btnActualizarManzana.BackColor = System.Drawing.Color.DarkCyan;
            this.btnActualizarManzana.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActualizarManzana.ForeColor = System.Drawing.Color.White;
            this.btnActualizarManzana.Location = new System.Drawing.Point(296, 278);
            this.btnActualizarManzana.Name = "btnActualizarManzana";
            this.btnActualizarManzana.Size = new System.Drawing.Size(112, 41);
            this.btnActualizarManzana.TabIndex = 32;
            this.btnActualizarManzana.Text = "Actualizar";
            this.btnActualizarManzana.UseVisualStyleBackColor = false;
            this.btnActualizarManzana.Click += new System.EventHandler(this.btnActualizarManzana_Click);
            // 
            // btnEstadoManzana
            // 
            this.btnEstadoManzana.BackColor = System.Drawing.Color.DarkOrange;
            this.btnEstadoManzana.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstadoManzana.ForeColor = System.Drawing.Color.White;
            this.btnEstadoManzana.Location = new System.Drawing.Point(296, 325);
            this.btnEstadoManzana.Name = "btnEstadoManzana";
            this.btnEstadoManzana.Size = new System.Drawing.Size(112, 41);
            this.btnEstadoManzana.TabIndex = 33;
            this.btnEstadoManzana.Text = "Estado";
            this.btnEstadoManzana.UseVisualStyleBackColor = false;
            this.btnEstadoManzana.Click += new System.EventHandler(this.btnEstadoManzana_Click);
            // 
            // Manzana
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 378);
            this.Controls.Add(this.btnEstadoManzana);
            this.Controls.Add(this.btnActualizarManzana);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDetalle);
            this.Controls.Add(this.txtManzana);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.dgvManzana);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Manzana";
            this.Text = "Manzana";
            this.Load += new System.EventHandler(this.Manzana_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvManzana)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDetalle;
        private System.Windows.Forms.TextBox txtManzana;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.DataGridView dgvManzana;
        private System.Windows.Forms.Button btnActualizarManzana;
        private System.Windows.Forms.Button btnEstadoManzana;
    }
}
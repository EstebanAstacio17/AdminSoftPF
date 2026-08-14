namespace AdminSoftPF
{
    partial class Cuotas
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCuota = new System.Windows.Forms.TextBox();
            this.txtDetalle = new System.Windows.Forms.TextBox();
            this.btnEstadoCuota = new System.Windows.Forms.Button();
            this.btnAgregarCuota = new System.Windows.Forms.Button();
            this.dgvCuotas = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.btnActualizarCuota = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCuotas)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(44, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "Cuota:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(38, 288);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Detalle:";
            // 
            // txtCuota
            // 
            this.txtCuota.BackColor = System.Drawing.Color.White;
            this.txtCuota.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCuota.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCuota.Location = new System.Drawing.Point(110, 332);
            this.txtCuota.Name = "txtCuota";
            this.txtCuota.Size = new System.Drawing.Size(180, 28);
            this.txtCuota.TabIndex = 11;
            this.txtCuota.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSoloNumeros_KeyPress);
            // 
            // txtDetalle
            // 
            this.txtDetalle.BackColor = System.Drawing.Color.White;
            this.txtDetalle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetalle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetalle.Location = new System.Drawing.Point(110, 284);
            this.txtDetalle.Name = "txtDetalle";
            this.txtDetalle.Size = new System.Drawing.Size(180, 28);
            this.txtDetalle.TabIndex = 10;
            this.txtDetalle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSoloLetras_KeyPress);
            // 
            // btnEstadoCuota
            // 
            this.btnEstadoCuota.BackColor = System.Drawing.Color.DarkOrange;
            this.btnEstadoCuota.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstadoCuota.ForeColor = System.Drawing.Color.White;
            this.btnEstadoCuota.Location = new System.Drawing.Point(296, 326);
            this.btnEstadoCuota.Name = "btnEstadoCuota";
            this.btnEstadoCuota.Size = new System.Drawing.Size(112, 41);
            this.btnEstadoCuota.TabIndex = 9;
            this.btnEstadoCuota.Text = "Estado";
            this.btnEstadoCuota.UseVisualStyleBackColor = false;
            this.btnEstadoCuota.Click += new System.EventHandler(this.btnEstadoCuota_Click);
            // 
            // btnAgregarCuota
            // 
            this.btnAgregarCuota.BackColor = System.Drawing.Color.SeaGreen;
            this.btnAgregarCuota.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarCuota.ForeColor = System.Drawing.Color.White;
            this.btnAgregarCuota.Location = new System.Drawing.Point(296, 279);
            this.btnAgregarCuota.Name = "btnAgregarCuota";
            this.btnAgregarCuota.Size = new System.Drawing.Size(112, 41);
            this.btnAgregarCuota.TabIndex = 8;
            this.btnAgregarCuota.Text = "Agregar";
            this.btnAgregarCuota.UseVisualStyleBackColor = false;
            this.btnAgregarCuota.Click += new System.EventHandler(this.btnAgregarCuota_Click);
            // 
            // dgvCuotas
            // 
            this.dgvCuotas.AllowUserToAddRows = false;
            this.dgvCuotas.AllowUserToDeleteRows = false;
            this.dgvCuotas.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCuotas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCuotas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCuotas.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCuotas.Location = new System.Drawing.Point(12, 32);
            this.dgvCuotas.MultiSelect = false;
            this.dgvCuotas.Name = "dgvCuotas";
            this.dgvCuotas.ReadOnly = true;
            this.dgvCuotas.RowHeadersVisible = false;
            this.dgvCuotas.RowHeadersWidth = 51;
            this.dgvCuotas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCuotas.Size = new System.Drawing.Size(396, 241);
            this.dgvCuotas.TabIndex = 7;
            this.dgvCuotas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCuotas_CellClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(214, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Cuotas de Mantenimiento";
            // 
            // btnActualizarCuota
            // 
            this.btnActualizarCuota.BackColor = System.Drawing.Color.DarkCyan;
            this.btnActualizarCuota.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActualizarCuota.ForeColor = System.Drawing.Color.White;
            this.btnActualizarCuota.Location = new System.Drawing.Point(296, 278);
            this.btnActualizarCuota.Name = "btnActualizarCuota";
            this.btnActualizarCuota.Size = new System.Drawing.Size(112, 41);
            this.btnActualizarCuota.TabIndex = 17;
            this.btnActualizarCuota.Text = "Actualizar";
            this.btnActualizarCuota.UseVisualStyleBackColor = false;
            this.btnActualizarCuota.Click += new System.EventHandler(this.btnActualizarCuota_Click);
            // 
            // Cuotas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 378);
            this.Controls.Add(this.btnActualizarCuota);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCuota);
            this.Controls.Add(this.txtDetalle);
            this.Controls.Add(this.btnEstadoCuota);
            this.Controls.Add(this.btnAgregarCuota);
            this.Controls.Add(this.dgvCuotas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Cuotas";
            this.Text = "Cuotas";
            this.Load += new System.EventHandler(this.Cuotas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCuotas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCuota;
        private System.Windows.Forms.TextBox txtDetalle;
        private System.Windows.Forms.Button btnEstadoCuota;
        private System.Windows.Forms.Button btnAgregarCuota;
        private System.Windows.Forms.DataGridView dgvCuotas;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnActualizarCuota;
    }
}
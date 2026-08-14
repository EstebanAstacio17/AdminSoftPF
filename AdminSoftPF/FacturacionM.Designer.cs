namespace AdminSoftPF
{
    partial class FacturacionM
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvFacturacionMasiva = new System.Windows.Forms.DataGridView();
            this.txtDetalleFacturacionMasiva = new System.Windows.Forms.TextBox();
            this.btnFacturarCuota = new System.Windows.Forms.Button();
            this.btnNuvaFacturacion = new System.Windows.Forms.Button();
            this.btnSeleccionarCuota = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboCuotas = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvCambioCuotaMasiva = new System.Windows.Forms.DataGridView();
            this.txtCuotaActual = new System.Windows.Forms.TextBox();
            this.btnNuevoCambioCuotas = new System.Windows.Forms.Button();
            this.btnAplicarNuevaCuota = new System.Windows.Forms.Button();
            this.btnSeleccionarClientes = new System.Windows.Forms.Button();
            this.cboCuotaFacturar = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFacturacionMasiva)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCambioCuotaMasiva)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(13, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Facturacion Simple Masiva";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cboCuotaFacturar);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.dgvFacturacionMasiva);
            this.panel1.Controls.Add(this.txtDetalleFacturacionMasiva);
            this.panel1.Controls.Add(this.btnFacturarCuota);
            this.panel1.Controls.Add(this.btnNuvaFacturacion);
            this.panel1.Controls.Add(this.btnSeleccionarCuota);
            this.panel1.Location = new System.Drawing.Point(16, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1265, 313);
            this.panel1.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(940, 20);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(151, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "Cuota a Facturar";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(352, 282);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Detalle de Factura";
            // 
            // dgvFacturacionMasiva
            // 
            this.dgvFacturacionMasiva.AllowUserToAddRows = false;
            this.dgvFacturacionMasiva.AllowUserToDeleteRows = false;
            this.dgvFacturacionMasiva.BackgroundColor = System.Drawing.Color.White;
            this.dgvFacturacionMasiva.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFacturacionMasiva.Location = new System.Drawing.Point(4, 53);
            this.dgvFacturacionMasiva.Margin = new System.Windows.Forms.Padding(4);
            this.dgvFacturacionMasiva.MultiSelect = false;
            this.dgvFacturacionMasiva.Name = "dgvFacturacionMasiva";
            this.dgvFacturacionMasiva.ReadOnly = true;
            this.dgvFacturacionMasiva.RowHeadersVisible = false;
            this.dgvFacturacionMasiva.RowHeadersWidth = 51;
            this.dgvFacturacionMasiva.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFacturacionMasiva.Size = new System.Drawing.Size(1093, 215);
            this.dgvFacturacionMasiva.TabIndex = 14;
            // 
            // txtDetalleFacturacionMasiva
            // 
            this.txtDetalleFacturacionMasiva.BackColor = System.Drawing.Color.White;
            this.txtDetalleFacturacionMasiva.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetalleFacturacionMasiva.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetalleFacturacionMasiva.Location = new System.Drawing.Point(540, 276);
            this.txtDetalleFacturacionMasiva.Margin = new System.Windows.Forms.Padding(4);
            this.txtDetalleFacturacionMasiva.Name = "txtDetalleFacturacionMasiva";
            this.txtDetalleFacturacionMasiva.Size = new System.Drawing.Size(557, 30);
            this.txtDetalleFacturacionMasiva.TabIndex = 14;
            this.txtDetalleFacturacionMasiva.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormatoTextDetalle_KeyPress);
            // 
            // btnFacturarCuota
            // 
            this.btnFacturarCuota.BackColor = System.Drawing.Color.SteelBlue;
            this.btnFacturarCuota.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFacturarCuota.ForeColor = System.Drawing.Color.White;
            this.btnFacturarCuota.Location = new System.Drawing.Point(1105, 140);
            this.btnFacturarCuota.Margin = new System.Windows.Forms.Padding(4);
            this.btnFacturarCuota.Name = "btnFacturarCuota";
            this.btnFacturarCuota.Size = new System.Drawing.Size(153, 80);
            this.btnFacturarCuota.TabIndex = 4;
            this.btnFacturarCuota.Text = "Facturar";
            this.btnFacturarCuota.UseVisualStyleBackColor = false;
            this.btnFacturarCuota.Click += new System.EventHandler(this.btnFacturarCuota_Click);
            // 
            // btnNuvaFacturacion
            // 
            this.btnNuvaFacturacion.BackColor = System.Drawing.Color.SteelBlue;
            this.btnNuvaFacturacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNuvaFacturacion.ForeColor = System.Drawing.Color.White;
            this.btnNuvaFacturacion.Location = new System.Drawing.Point(1105, 228);
            this.btnNuvaFacturacion.Margin = new System.Windows.Forms.Padding(4);
            this.btnNuvaFacturacion.Name = "btnNuvaFacturacion";
            this.btnNuvaFacturacion.Size = new System.Drawing.Size(153, 80);
            this.btnNuvaFacturacion.TabIndex = 5;
            this.btnNuvaFacturacion.Text = "Nueva Facturacion";
            this.btnNuvaFacturacion.UseVisualStyleBackColor = false;
            this.btnNuvaFacturacion.Click += new System.EventHandler(this.btnNuvaFacturacion_Click);
            // 
            // btnSeleccionarCuota
            // 
            this.btnSeleccionarCuota.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSeleccionarCuota.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionarCuota.ForeColor = System.Drawing.Color.White;
            this.btnSeleccionarCuota.Location = new System.Drawing.Point(1105, 53);
            this.btnSeleccionarCuota.Margin = new System.Windows.Forms.Padding(4);
            this.btnSeleccionarCuota.Name = "btnSeleccionarCuota";
            this.btnSeleccionarCuota.Size = new System.Drawing.Size(153, 80);
            this.btnSeleccionarCuota.TabIndex = 3;
            this.btnSeleccionarCuota.Text = "Seleccionar";
            this.btnSeleccionarCuota.UseVisualStyleBackColor = false;
            this.btnSeleccionarCuota.Click += new System.EventHandler(this.btnSeleccionarCuota_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cboCuotas);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dgvCambioCuotaMasiva);
            this.panel2.Controls.Add(this.txtCuotaActual);
            this.panel2.Controls.Add(this.btnNuevoCambioCuotas);
            this.panel2.Controls.Add(this.btnAplicarNuevaCuota);
            this.panel2.Controls.Add(this.btnSeleccionarClientes);
            this.panel2.Location = new System.Drawing.Point(16, 336);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1265, 286);
            this.panel2.TabIndex = 2;
            // 
            // cboCuotas
            // 
            this.cboCuotas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCuotas.FormattingEnabled = true;
            this.cboCuotas.Location = new System.Drawing.Point(928, 23);
            this.cboCuotas.Margin = new System.Windows.Forms.Padding(4);
            this.cboCuotas.Name = "cboCuotas";
            this.cboCuotas.Size = new System.Drawing.Size(169, 33);
            this.cboCuotas.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.SteelBlue;
            this.label4.Location = new System.Drawing.Point(13, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(252, 26);
            this.label4.TabIndex = 6;
            this.label4.Text = "Cambio de Cuota Masiva";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(791, 32);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Nueva Cuota";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Antigua Cuota";
            // 
            // dgvCambioCuotaMasiva
            // 
            this.dgvCambioCuotaMasiva.AllowUserToAddRows = false;
            this.dgvCambioCuotaMasiva.AllowUserToDeleteRows = false;
            this.dgvCambioCuotaMasiva.BackgroundColor = System.Drawing.Color.White;
            this.dgvCambioCuotaMasiva.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCambioCuotaMasiva.Location = new System.Drawing.Point(4, 65);
            this.dgvCambioCuotaMasiva.Margin = new System.Windows.Forms.Padding(4);
            this.dgvCambioCuotaMasiva.MultiSelect = false;
            this.dgvCambioCuotaMasiva.Name = "dgvCambioCuotaMasiva";
            this.dgvCambioCuotaMasiva.ReadOnly = true;
            this.dgvCambioCuotaMasiva.RowHeadersVisible = false;
            this.dgvCambioCuotaMasiva.RowHeadersWidth = 51;
            this.dgvCambioCuotaMasiva.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCambioCuotaMasiva.Size = new System.Drawing.Size(1093, 212);
            this.dgvCambioCuotaMasiva.TabIndex = 11;
            // 
            // txtCuotaActual
            // 
            this.txtCuotaActual.BackColor = System.Drawing.Color.White;
            this.txtCuotaActual.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCuotaActual.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCuotaActual.Location = new System.Drawing.Point(159, 26);
            this.txtCuotaActual.Margin = new System.Windows.Forms.Padding(4);
            this.txtCuotaActual.Name = "txtCuotaActual";
            this.txtCuotaActual.Size = new System.Drawing.Size(166, 30);
            this.txtCuotaActual.TabIndex = 10;
            this.txtCuotaActual.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormatoTextFact_KeyPress);
            // 
            // btnNuevoCambioCuotas
            // 
            this.btnNuevoCambioCuotas.BackColor = System.Drawing.Color.SteelBlue;
            this.btnNuevoCambioCuotas.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNuevoCambioCuotas.ForeColor = System.Drawing.Color.White;
            this.btnNuevoCambioCuotas.Location = new System.Drawing.Point(1105, 201);
            this.btnNuevoCambioCuotas.Margin = new System.Windows.Forms.Padding(4);
            this.btnNuevoCambioCuotas.Name = "btnNuevoCambioCuotas";
            this.btnNuevoCambioCuotas.Size = new System.Drawing.Size(153, 80);
            this.btnNuevoCambioCuotas.TabIndex = 8;
            this.btnNuevoCambioCuotas.Text = "Nuevo Cambio de Cuotas";
            this.btnNuevoCambioCuotas.UseVisualStyleBackColor = false;
            this.btnNuevoCambioCuotas.Click += new System.EventHandler(this.btnNuevoCambioCuotas_Click);
            // 
            // btnAplicarNuevaCuota
            // 
            this.btnAplicarNuevaCuota.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAplicarNuevaCuota.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAplicarNuevaCuota.ForeColor = System.Drawing.Color.White;
            this.btnAplicarNuevaCuota.Location = new System.Drawing.Point(1105, 113);
            this.btnAplicarNuevaCuota.Margin = new System.Windows.Forms.Padding(4);
            this.btnAplicarNuevaCuota.Name = "btnAplicarNuevaCuota";
            this.btnAplicarNuevaCuota.Size = new System.Drawing.Size(153, 80);
            this.btnAplicarNuevaCuota.TabIndex = 7;
            this.btnAplicarNuevaCuota.Text = "Aplicar Nueva Cuota";
            this.btnAplicarNuevaCuota.UseVisualStyleBackColor = false;
            this.btnAplicarNuevaCuota.Click += new System.EventHandler(this.btnAplicarNuevaCuota_Click);
            // 
            // btnSeleccionarClientes
            // 
            this.btnSeleccionarClientes.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSeleccionarClientes.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionarClientes.ForeColor = System.Drawing.Color.White;
            this.btnSeleccionarClientes.Location = new System.Drawing.Point(1105, 26);
            this.btnSeleccionarClientes.Margin = new System.Windows.Forms.Padding(4);
            this.btnSeleccionarClientes.Name = "btnSeleccionarClientes";
            this.btnSeleccionarClientes.Size = new System.Drawing.Size(153, 80);
            this.btnSeleccionarClientes.TabIndex = 6;
            this.btnSeleccionarClientes.Text = "Seleccionar Clientes";
            this.btnSeleccionarClientes.UseVisualStyleBackColor = false;
            this.btnSeleccionarClientes.Click += new System.EventHandler(this.btnSeleccionarClientes_Click);
            // 
            // cboCuotaFacturar
            // 
            this.cboCuotaFacturar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCuotaFacturar.FormattingEnabled = true;
            this.cboCuotaFacturar.Location = new System.Drawing.Point(1108, 13);
            this.cboCuotaFacturar.Name = "cboCuotaFacturar";
            this.cboCuotaFacturar.Size = new System.Drawing.Size(145, 33);
            this.cboCuotaFacturar.TabIndex = 16;
            // 
            // FacturacionM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(1297, 638);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FacturacionM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FacturacionM";
            this.Load += new System.EventHandler(this.FacturacionM_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFacturacionMasiva)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCambioCuotaMasiva)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSeleccionarCuota;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvCambioCuotaMasiva;
        private System.Windows.Forms.TextBox txtCuotaActual;
        private System.Windows.Forms.Button btnNuevoCambioCuotas;
        private System.Windows.Forms.Button btnAplicarNuevaCuota;
        private System.Windows.Forms.Button btnSeleccionarClientes;
        private System.Windows.Forms.Button btnFacturarCuota;
        private System.Windows.Forms.Button btnNuvaFacturacion;
        private System.Windows.Forms.TextBox txtDetalleFacturacionMasiva;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvFacturacionMasiva;
        private System.Windows.Forms.ComboBox cboCuotas;
        private System.Windows.Forms.ComboBox cboCuotaFacturar;
    }
}
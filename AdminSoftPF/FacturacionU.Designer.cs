namespace AdminSoftPF
{
    partial class FacturacionU
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
            this.label5 = new System.Windows.Forms.Label();
            this.dgvFactUnica = new System.Windows.Forms.DataGridView();
            this.lblDireccionSeleccionada = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboTipoCuota = new System.Windows.Forms.ComboBox();
            this.txtDetalleFactExtra = new System.Windows.Forms.TextBox();
            this.txtValorExtra = new System.Windows.Forms.TextBox();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnCrearfactura = new System.Windows.Forms.Button();
            this.btnSeleccionarDireccion = new System.Windows.Forms.Button();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFactUnica)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(21, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Seleccionar Cliente";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.dgvFactUnica);
            this.panel1.Controls.Add(this.lblDireccionSeleccionada);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cboTipoCuota);
            this.panel1.Controls.Add(this.txtDetalleFactExtra);
            this.panel1.Controls.Add(this.txtValorExtra);
            this.panel1.Controls.Add(this.btnLimpiar);
            this.panel1.Controls.Add(this.btnCrearfactura);
            this.panel1.Controls.Add(this.btnSeleccionarDireccion);
            this.panel1.Controls.Add(this.txtDireccion);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(16, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1265, 608);
            this.panel1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 559);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(213, 25);
            this.label5.TabIndex = 14;
            this.label5.Text = "Detalle de Cuota Extra:";
            // 
            // dgvFactUnica
            // 
            this.dgvFactUnica.AllowUserToAddRows = false;
            this.dgvFactUnica.AllowUserToDeleteRows = false;
            this.dgvFactUnica.BackgroundColor = System.Drawing.Color.White;
            this.dgvFactUnica.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFactUnica.Location = new System.Drawing.Point(9, 97);
            this.dgvFactUnica.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvFactUnica.MultiSelect = false;
            this.dgvFactUnica.Name = "dgvFactUnica";
            this.dgvFactUnica.ReadOnly = true;
            this.dgvFactUnica.RowHeadersVisible = false;
            this.dgvFactUnica.RowHeadersWidth = 51;
            this.dgvFactUnica.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFactUnica.Size = new System.Drawing.Size(1247, 348);
            this.dgvFactUnica.TabIndex = 13;
            this.dgvFactUnica.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFactUnica_CellClick);
            // 
            // lblDireccionSeleccionada
            // 
            this.lblDireccionSeleccionada.AutoSize = true;
            this.lblDireccionSeleccionada.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDireccionSeleccionada.Location = new System.Drawing.Point(10, 490);
            this.lblDireccionSeleccionada.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDireccionSeleccionada.Name = "lblDireccionSeleccionada";
            this.lblDireccionSeleccionada.Size = new System.Drawing.Size(102, 25);
            this.lblDireccionSeleccionada.TabIndex = 11;
            this.lblDireccionSeleccionada.Text = "Direccion";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(785, 474);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 25);
            this.label3.TabIndex = 9;
            this.label3.Text = "Tipo Cuota:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(785, 515);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "Valor Extra:";
            // 
            // cboTipoCuota
            // 
            this.cboTipoCuota.BackColor = System.Drawing.Color.White;
            this.cboTipoCuota.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTipoCuota.FormattingEnabled = true;
            this.cboTipoCuota.Location = new System.Drawing.Point(918, 471);
            this.cboTipoCuota.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboTipoCuota.Name = "cboTipoCuota";
            this.cboTipoCuota.Size = new System.Drawing.Size(189, 33);
            this.cboTipoCuota.TabIndex = 7;
            // 
            // txtDetalleFactExtra
            // 
            this.txtDetalleFactExtra.BackColor = System.Drawing.Color.White;
            this.txtDetalleFactExtra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetalleFactExtra.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetalleFactExtra.Location = new System.Drawing.Point(248, 556);
            this.txtDetalleFactExtra.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDetalleFactExtra.Name = "txtDetalleFactExtra";
            this.txtDetalleFactExtra.Size = new System.Drawing.Size(689, 30);
            this.txtDetalleFactExtra.TabIndex = 6;
            this.txtDetalleFactExtra.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDetalle_KeyPress);
            // 
            // txtValorExtra
            // 
            this.txtValorExtra.BackColor = System.Drawing.Color.White;
            this.txtValorExtra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValorExtra.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorExtra.Location = new System.Drawing.Point(918, 513);
            this.txtValorExtra.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtValorExtra.Name = "txtValorExtra";
            this.txtValorExtra.Size = new System.Drawing.Size(190, 30);
            this.txtValorExtra.TabIndex = 5;
            this.txtValorExtra.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValorExtra_KeyPress);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar.Location = new System.Drawing.Point(1116, 540);
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(140, 62);
            this.btnLimpiar.TabIndex = 4;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnCrearfactura
            // 
            this.btnCrearfactura.BackColor = System.Drawing.Color.SteelBlue;
            this.btnCrearfactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrearfactura.ForeColor = System.Drawing.Color.White;
            this.btnCrearfactura.Location = new System.Drawing.Point(1116, 471);
            this.btnCrearfactura.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCrearfactura.Name = "btnCrearfactura";
            this.btnCrearfactura.Size = new System.Drawing.Size(140, 62);
            this.btnCrearfactura.TabIndex = 3;
            this.btnCrearfactura.Text = "Crear";
            this.btnCrearfactura.UseVisualStyleBackColor = false;
            this.btnCrearfactura.Click += new System.EventHandler(this.btnCrearfactura_Click);
            // 
            // btnSeleccionarDireccion
            // 
            this.btnSeleccionarDireccion.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSeleccionarDireccion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionarDireccion.ForeColor = System.Drawing.Color.White;
            this.btnSeleccionarDireccion.Location = new System.Drawing.Point(1116, 15);
            this.btnSeleccionarDireccion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSeleccionarDireccion.Name = "btnSeleccionarDireccion";
            this.btnSeleccionarDireccion.Size = new System.Drawing.Size(140, 62);
            this.btnSeleccionarDireccion.TabIndex = 2;
            this.btnSeleccionarDireccion.Text = "Buscar Direccion";
            this.btnSeleccionarDireccion.UseVisualStyleBackColor = false;
            this.btnSeleccionarDireccion.Click += new System.EventHandler(this.btnSeleccionarDireccion_Click);
            // 
            // txtDireccion
            // 
            this.txtDireccion.BackColor = System.Drawing.Color.White;
            this.txtDireccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDireccion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDireccion.Location = new System.Drawing.Point(248, 30);
            this.txtDireccion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(218, 30);
            this.txtDireccion.TabIndex = 1;
            this.txtDireccion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDireccion_KeyPress);
            // 
            // FacturacionU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(1297, 638);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FacturacionU";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FacturacionU";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFactUnica)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvFactUnica;
        private System.Windows.Forms.Label lblDireccionSeleccionada;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboTipoCuota;
        private System.Windows.Forms.TextBox txtDetalleFactExtra;
        private System.Windows.Forms.TextBox txtValorExtra;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnCrearfactura;
        private System.Windows.Forms.Button btnSeleccionarDireccion;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.Label label5;
    }
}
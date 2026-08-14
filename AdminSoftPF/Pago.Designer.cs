namespace AdminSoftPF
{
    partial class Pago
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTransferencia = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtDireccionCondomino = new System.Windows.Forms.TextBox();
            this.txtNombreProyecto = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelarPago = new System.Windows.Forms.Button();
            this.rtbComentario = new System.Windows.Forms.RichTextBox();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.cboBancoCuenta = new System.Windows.Forms.ComboBox();
            this.cboTarjeta = new System.Windows.Forms.ComboBox();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.txtTarjeta = new System.Windows.Forms.TextBox();
            this.txtEfectivo = new System.Windows.Forms.TextBox();
            this.btnAplicarPago = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.txtTransferencia);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnCancelarPago);
            this.panel1.Controls.Add(this.rtbComentario);
            this.panel1.Controls.Add(this.dtpFecha);
            this.panel1.Controls.Add(this.cboBancoCuenta);
            this.panel1.Controls.Add(this.cboTarjeta);
            this.panel1.Controls.Add(this.txtTotal);
            this.panel1.Controls.Add(this.txtTarjeta);
            this.panel1.Controls.Add(this.txtEfectivo);
            this.panel1.Controls.Add(this.btnAplicarPago);
            this.panel1.Location = new System.Drawing.Point(9, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(376, 446);
            this.panel1.TabIndex = 0;
            // 
            // txtTransferencia
            // 
            this.txtTransferencia.BackColor = System.Drawing.Color.White;
            this.txtTransferencia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTransferencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransferencia.Location = new System.Drawing.Point(128, 206);
            this.txtTransferencia.Margin = new System.Windows.Forms.Padding(2);
            this.txtTransferencia.Name = "txtTransferencia";
            this.txtTransferencia.Size = new System.Drawing.Size(82, 24);
            this.txtTransferencia.TabIndex = 19;
            this.txtTransferencia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTransferencia.TextChanged += new System.EventHandler(this.ActualizarTotal_TextChanged);
            this.txtTransferencia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValidarSoloNumeros);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.txtDireccionCondomino);
            this.panel3.Controls.Add(this.txtNombreProyecto);
            this.panel3.Location = new System.Drawing.Point(2, 61);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(372, 82);
            this.panel3.TabIndex = 35;
            // 
            // txtDireccionCondomino
            // 
            this.txtDireccionCondomino.BackColor = System.Drawing.Color.White;
            this.txtDireccionCondomino.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDireccionCondomino.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDireccionCondomino.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtDireccionCondomino.Location = new System.Drawing.Point(12, 42);
            this.txtDireccionCondomino.Name = "txtDireccionCondomino";
            this.txtDireccionCondomino.Size = new System.Drawing.Size(345, 22);
            this.txtDireccionCondomino.TabIndex = 36;
            this.txtDireccionCondomino.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtNombreProyecto
            // 
            this.txtNombreProyecto.BackColor = System.Drawing.Color.White;
            this.txtNombreProyecto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNombreProyecto.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombreProyecto.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtNombreProyecto.Location = new System.Drawing.Point(12, 14);
            this.txtNombreProyecto.Name = "txtNombreProyecto";
            this.txtNombreProyecto.Size = new System.Drawing.Size(345, 22);
            this.txtNombreProyecto.TabIndex = 35;
            this.txtNombreProyecto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SteelBlue;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(372, 54);
            this.panel2.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(90, 11);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(191, 31);
            this.label7.TabIndex = 0;
            this.label7.Text = "Pago a Cliente";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(34, 319);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 20);
            this.label6.TabIndex = 31;
            this.label6.Text = "Comentario:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(76, 286);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 20);
            this.label5.TabIndex = 30;
            this.label5.Text = "Total:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(68, 239);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 20);
            this.label4.TabIndex = 29;
            this.label4.Text = "Fecha:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 207);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 20);
            this.label3.TabIndex = 28;
            this.label3.Text = "Transferencia:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(64, 179);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 20);
            this.label2.TabIndex = 27;
            this.label2.Text = "Tarjeta:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(58, 149);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 26;
            this.label1.Text = "Efectivo:";
            // 
            // btnCancelarPago
            // 
            this.btnCancelarPago.BackColor = System.Drawing.Color.Goldenrod;
            this.btnCancelarPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelarPago.ForeColor = System.Drawing.Color.White;
            this.btnCancelarPago.Location = new System.Drawing.Point(250, 392);
            this.btnCancelarPago.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancelarPago.Name = "btnCancelarPago";
            this.btnCancelarPago.Size = new System.Drawing.Size(110, 28);
            this.btnCancelarPago.TabIndex = 25;
            this.btnCancelarPago.Text = "Cancelar";
            this.btnCancelarPago.UseVisualStyleBackColor = false;
            this.btnCancelarPago.Click += new System.EventHandler(this.btnCancelarPago_Click);
            // 
            // rtbComentario
            // 
            this.rtbComentario.BackColor = System.Drawing.Color.White;
            this.rtbComentario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbComentario.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbComentario.Location = new System.Drawing.Point(15, 342);
            this.rtbComentario.Margin = new System.Windows.Forms.Padding(2);
            this.rtbComentario.Name = "rtbComentario";
            this.rtbComentario.Size = new System.Drawing.Size(231, 79);
            this.rtbComentario.TabIndex = 24;
            this.rtbComentario.Text = "";
            this.rtbComentario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtbComentario_KeyDown);
            // 
            // dtpFecha
            // 
            this.dtpFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(128, 235);
            this.dtpFecha.Margin = new System.Windows.Forms.Padding(2);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(171, 23);
            this.dtpFecha.TabIndex = 23;
            // 
            // cboBancoCuenta
            // 
            this.cboBancoCuenta.BackColor = System.Drawing.Color.White;
            this.cboBancoCuenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBancoCuenta.FormattingEnabled = true;
            this.cboBancoCuenta.Location = new System.Drawing.Point(214, 207);
            this.cboBancoCuenta.Margin = new System.Windows.Forms.Padding(2);
            this.cboBancoCuenta.Name = "cboBancoCuenta";
            this.cboBancoCuenta.Size = new System.Drawing.Size(147, 24);
            this.cboBancoCuenta.TabIndex = 22;
            this.cboBancoCuenta.TextChanged += new System.EventHandler(this.txtTransferencia_TextChanged);
            // 
            // cboTarjeta
            // 
            this.cboTarjeta.BackColor = System.Drawing.Color.White;
            this.cboTarjeta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTarjeta.FormattingEnabled = true;
            this.cboTarjeta.Items.AddRange(new object[] {
            "VISA",
            "MASTERCARD",
            "AMEX",
            "OTRA"});
            this.cboTarjeta.Location = new System.Drawing.Point(214, 179);
            this.cboTarjeta.Margin = new System.Windows.Forms.Padding(2);
            this.cboTarjeta.Name = "cboTarjeta";
            this.cboTarjeta.Size = new System.Drawing.Size(147, 24);
            this.cboTarjeta.TabIndex = 21;
            this.cboTarjeta.TextChanged += new System.EventHandler(this.txtTarjeta_TextChanged);
            // 
            // txtTotal
            // 
            this.txtTotal.BackColor = System.Drawing.Color.White;
            this.txtTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotal.Enabled = false;
            this.txtTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.HideSelection = false;
            this.txtTotal.Location = new System.Drawing.Point(128, 284);
            this.txtTotal.Margin = new System.Windows.Forms.Padding(2);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(137, 26);
            this.txtTotal.TabIndex = 20;
            this.txtTotal.TabStop = false;
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTarjeta
            // 
            this.txtTarjeta.BackColor = System.Drawing.Color.White;
            this.txtTarjeta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTarjeta.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTarjeta.Location = new System.Drawing.Point(128, 176);
            this.txtTarjeta.Margin = new System.Windows.Forms.Padding(2);
            this.txtTarjeta.Name = "txtTarjeta";
            this.txtTarjeta.Size = new System.Drawing.Size(82, 24);
            this.txtTarjeta.TabIndex = 18;
            this.txtTarjeta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTarjeta.Click += new System.EventHandler(this.txtTarjeta_Click);
            this.txtTarjeta.TextChanged += new System.EventHandler(this.ActualizarTotal_TextChanged);
            this.txtTarjeta.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValidarSoloNumeros);
            // 
            // txtEfectivo
            // 
            this.txtEfectivo.BackColor = System.Drawing.Color.White;
            this.txtEfectivo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEfectivo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEfectivo.Location = new System.Drawing.Point(128, 147);
            this.txtEfectivo.Margin = new System.Windows.Forms.Padding(2);
            this.txtEfectivo.Name = "txtEfectivo";
            this.txtEfectivo.Size = new System.Drawing.Size(82, 24);
            this.txtEfectivo.TabIndex = 17;
            this.txtEfectivo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtEfectivo.TextChanged += new System.EventHandler(this.ActualizarTotal_TextChanged);
            this.txtEfectivo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValidarSoloNumeros);
            // 
            // btnAplicarPago
            // 
            this.btnAplicarPago.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAplicarPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAplicarPago.ForeColor = System.Drawing.Color.White;
            this.btnAplicarPago.Location = new System.Drawing.Point(250, 342);
            this.btnAplicarPago.Margin = new System.Windows.Forms.Padding(2);
            this.btnAplicarPago.Name = "btnAplicarPago";
            this.btnAplicarPago.Size = new System.Drawing.Size(110, 45);
            this.btnAplicarPago.TabIndex = 16;
            this.btnAplicarPago.Text = "Aplicar";
            this.btnAplicarPago.UseVisualStyleBackColor = false;
            this.btnAplicarPago.Click += new System.EventHandler(this.btnAplicarPago_Click);
            // 
            // Pago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(392, 462);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Pago";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pago";
            this.Load += new System.EventHandler(this.Pago_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelarPago;
        private System.Windows.Forms.RichTextBox rtbComentario;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.ComboBox cboBancoCuenta;
        private System.Windows.Forms.ComboBox cboTarjeta;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.TextBox txtTransferencia;
        private System.Windows.Forms.TextBox txtTarjeta;
        private System.Windows.Forms.TextBox txtEfectivo;
        private System.Windows.Forms.Button btnAplicarPago;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtNombreProyecto;
        private System.Windows.Forms.TextBox txtDireccionCondomino;
    }
}
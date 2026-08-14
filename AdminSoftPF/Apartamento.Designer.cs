namespace AdminSoftPF
{
    partial class Apartamento
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDetalle = new System.Windows.Forms.TextBox();
            this.txtApartamento = new System.Windows.Forms.TextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.dgvApartamento = new System.Windows.Forms.DataGridView();
            this.btnEstadoApartamento = new System.Windows.Forms.Button();
            this.btnActualizarApartamento = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApartamento)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 20);
            this.label3.TabIndex = 23;
            this.label3.Text = "Apartamento";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(51, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 22;
            this.label2.Text = "Detalle:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 288);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Apartamento:";
            // 
            // txtDetalle
            // 
            this.txtDetalle.BackColor = System.Drawing.Color.White;
            this.txtDetalle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetalle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetalle.Location = new System.Drawing.Point(116, 332);
            this.txtDetalle.Name = "txtDetalle";
            this.txtDetalle.Size = new System.Drawing.Size(170, 28);
            this.txtDetalle.TabIndex = 20;
            // 
            // txtApartamento
            // 
            this.txtApartamento.BackColor = System.Drawing.Color.White;
            this.txtApartamento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtApartamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApartamento.Location = new System.Drawing.Point(116, 284);
            this.txtApartamento.Name = "txtApartamento";
            this.txtApartamento.Size = new System.Drawing.Size(170, 28);
            this.txtApartamento.TabIndex = 19;
            this.txtApartamento.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtApartamento_KeyPress);
            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.SeaGreen;
            this.btnAgregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.ForeColor = System.Drawing.Color.White;
            this.btnAgregar.Location = new System.Drawing.Point(296, 280);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(112, 41);
            this.btnAgregar.TabIndex = 17;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // dgvApartamento
            // 
            this.dgvApartamento.AllowUserToAddRows = false;
            this.dgvApartamento.AllowUserToDeleteRows = false;
            this.dgvApartamento.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvApartamento.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvApartamento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvApartamento.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvApartamento.Location = new System.Drawing.Point(12, 32);
            this.dgvApartamento.MultiSelect = false;
            this.dgvApartamento.Name = "dgvApartamento";
            this.dgvApartamento.ReadOnly = true;
            this.dgvApartamento.RowHeadersVisible = false;
            this.dgvApartamento.RowHeadersWidth = 51;
            this.dgvApartamento.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvApartamento.Size = new System.Drawing.Size(396, 240);
            this.dgvApartamento.TabIndex = 16;
            this.dgvApartamento.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvApartamento_CellClick);
            // 
            // btnEstadoApartamento
            // 
            this.btnEstadoApartamento.BackColor = System.Drawing.Color.DarkOrange;
            this.btnEstadoApartamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstadoApartamento.ForeColor = System.Drawing.Color.White;
            this.btnEstadoApartamento.Location = new System.Drawing.Point(296, 325);
            this.btnEstadoApartamento.Name = "btnEstadoApartamento";
            this.btnEstadoApartamento.Size = new System.Drawing.Size(112, 41);
            this.btnEstadoApartamento.TabIndex = 35;
            this.btnEstadoApartamento.Text = "Estado";
            this.btnEstadoApartamento.UseVisualStyleBackColor = false;
            this.btnEstadoApartamento.Click += new System.EventHandler(this.btnEstadoApartamento_Click);
            // 
            // btnActualizarApartamento
            // 
            this.btnActualizarApartamento.BackColor = System.Drawing.Color.DarkCyan;
            this.btnActualizarApartamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActualizarApartamento.ForeColor = System.Drawing.Color.White;
            this.btnActualizarApartamento.Location = new System.Drawing.Point(296, 278);
            this.btnActualizarApartamento.Name = "btnActualizarApartamento";
            this.btnActualizarApartamento.Size = new System.Drawing.Size(112, 41);
            this.btnActualizarApartamento.TabIndex = 34;
            this.btnActualizarApartamento.Text = "Actualizar";
            this.btnActualizarApartamento.UseVisualStyleBackColor = false;
            this.btnActualizarApartamento.Click += new System.EventHandler(this.btnActualizarApartamento_Click);
            // 
            // Apartamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 378);
            this.Controls.Add(this.btnEstadoApartamento);
            this.Controls.Add(this.btnActualizarApartamento);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDetalle);
            this.Controls.Add(this.txtApartamento);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.dgvApartamento);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Apartamento";
            this.Text = "Apartamento";
            this.Load += new System.EventHandler(this.Apartamento_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvApartamento)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDetalle;
        private System.Windows.Forms.TextBox txtApartamento;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.DataGridView dgvApartamento;
        private System.Windows.Forms.Button btnEstadoApartamento;
        private System.Windows.Forms.Button btnActualizarApartamento;
    }
}
namespace AdminSoftPF
{
    partial class Edificio
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
            this.txtEdificio = new System.Windows.Forms.TextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.dgvEdificio = new System.Windows.Forms.DataGridView();
            this.btnEstadoEdificio = new System.Windows.Forms.Button();
            this.btnActualizarEdificio = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEdificio)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 20);
            this.label3.TabIndex = 23;
            this.label3.Text = "Edificio";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(41, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 22;
            this.label2.Text = "Detalle:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(40, 288);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Edificio:";
            // 
            // txtDetalle
            // 
            this.txtDetalle.BackColor = System.Drawing.Color.White;
            this.txtDetalle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetalle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetalle.Location = new System.Drawing.Point(106, 332);
            this.txtDetalle.Name = "txtDetalle";
            this.txtDetalle.Size = new System.Drawing.Size(180, 28);
            this.txtDetalle.TabIndex = 20;
            // 
            // txtEdificio
            // 
            this.txtEdificio.BackColor = System.Drawing.Color.White;
            this.txtEdificio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEdificio.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEdificio.Location = new System.Drawing.Point(106, 284);
            this.txtEdificio.Name = "txtEdificio";
            this.txtEdificio.Size = new System.Drawing.Size(180, 28);
            this.txtEdificio.TabIndex = 19;
            this.txtEdificio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtEdificio_KeyPress);
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
            // dgvEdificio
            // 
            this.dgvEdificio.AllowUserToAddRows = false;
            this.dgvEdificio.AllowUserToDeleteRows = false;
            this.dgvEdificio.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEdificio.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvEdificio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvEdificio.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvEdificio.Location = new System.Drawing.Point(12, 32);
            this.dgvEdificio.MultiSelect = false;
            this.dgvEdificio.Name = "dgvEdificio";
            this.dgvEdificio.ReadOnly = true;
            this.dgvEdificio.RowHeadersVisible = false;
            this.dgvEdificio.RowHeadersWidth = 51;
            this.dgvEdificio.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEdificio.Size = new System.Drawing.Size(396, 240);
            this.dgvEdificio.TabIndex = 16;
            this.dgvEdificio.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEdificio_CellClick);
            // 
            // btnEstadoEdificio
            // 
            this.btnEstadoEdificio.BackColor = System.Drawing.Color.DarkOrange;
            this.btnEstadoEdificio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstadoEdificio.ForeColor = System.Drawing.Color.White;
            this.btnEstadoEdificio.Location = new System.Drawing.Point(296, 325);
            this.btnEstadoEdificio.Name = "btnEstadoEdificio";
            this.btnEstadoEdificio.Size = new System.Drawing.Size(112, 41);
            this.btnEstadoEdificio.TabIndex = 35;
            this.btnEstadoEdificio.Text = "Estado";
            this.btnEstadoEdificio.UseVisualStyleBackColor = false;
            this.btnEstadoEdificio.Click += new System.EventHandler(this.btnEstadoEdificio_Click);
            // 
            // btnActualizarEdificio
            // 
            this.btnActualizarEdificio.BackColor = System.Drawing.Color.DarkCyan;
            this.btnActualizarEdificio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActualizarEdificio.ForeColor = System.Drawing.Color.White;
            this.btnActualizarEdificio.Location = new System.Drawing.Point(296, 278);
            this.btnActualizarEdificio.Name = "btnActualizarEdificio";
            this.btnActualizarEdificio.Size = new System.Drawing.Size(112, 41);
            this.btnActualizarEdificio.TabIndex = 34;
            this.btnActualizarEdificio.Text = "Actualizar";
            this.btnActualizarEdificio.UseVisualStyleBackColor = false;
            this.btnActualizarEdificio.Click += new System.EventHandler(this.btnActualizarEdificio_Click);
            // 
            // Edificio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 378);
            this.Controls.Add(this.btnEstadoEdificio);
            this.Controls.Add(this.btnActualizarEdificio);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDetalle);
            this.Controls.Add(this.txtEdificio);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.dgvEdificio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Edificio";
            this.Text = "Edificio";
            this.Load += new System.EventHandler(this.Edificio_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEdificio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDetalle;
        private System.Windows.Forms.TextBox txtEdificio;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.DataGridView dgvEdificio;
        private System.Windows.Forms.Button btnEstadoEdificio;
        private System.Windows.Forms.Button btnActualizarEdificio;
    }
}
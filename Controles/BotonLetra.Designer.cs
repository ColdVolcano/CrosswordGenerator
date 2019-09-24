namespace CrosswordGenerator.Controles
{
    partial class BotonLetra
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BotonLetra));
            this.Boton = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Boton
            // 
            this.Boton.BackColor = System.Drawing.Color.Transparent;
            this.Boton.Location = new System.Drawing.Point(0, 0);
            this.Boton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Boton.Name = "Boton";
            this.Boton.Size = new System.Drawing.Size(25, 25);
            this.Boton.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 9);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 2;
            // 
            // BotonLetra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Boton);
            this.Controls.Add(this.panel1);
            this.Name = "BotonLetra";
            this.Size = new System.Drawing.Size(25, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Boton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}

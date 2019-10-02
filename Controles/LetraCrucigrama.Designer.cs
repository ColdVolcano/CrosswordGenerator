namespace CrosswordGenerator.Controles
{
    partial class LetraCrucigrama
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
            this.Display = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Display
            // 
            this.Display.BackColor = System.Drawing.Color.Transparent;
            this.Display.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Display.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Display.Location = new System.Drawing.Point(10, 10);
            this.Display.Name = "Display";
            this.Display.Size = new System.Drawing.Size(20, 20);
            this.Display.TabIndex = 0;
            this.Display.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BotonLetra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Display);
            this.Name = "BotonLetra";
            this.Size = new System.Drawing.Size(30, 30);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.actualizarLetra);
            this.ParentChanged += new System.EventHandler(this.generarIniciales);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Display;
    }
}

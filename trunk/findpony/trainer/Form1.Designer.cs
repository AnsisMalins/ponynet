namespace trainer
{
	partial class MainForm
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
			this.browseButton = new System.Windows.Forms.Button();
			this.resultTextBox = new System.Windows.Forms.TextBox();
			this.showColorPoints = new System.Windows.Forms.CheckBox();
			this.showChromaMap = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// browseButton
			// 
			this.browseButton.Location = new System.Drawing.Point(0, 0);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(75, 23);
			this.browseButton.TabIndex = 0;
			this.browseButton.Text = "Browse...";
			this.browseButton.UseVisualStyleBackColor = true;
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// resultTextBox
			// 
			this.resultTextBox.Location = new System.Drawing.Point(81, 2);
			this.resultTextBox.Name = "resultTextBox";
			this.resultTextBox.ReadOnly = true;
			this.resultTextBox.Size = new System.Drawing.Size(234, 20);
			this.resultTextBox.TabIndex = 1;
			// 
			// showColorPoints
			// 
			this.showColorPoints.AutoSize = true;
			this.showColorPoints.Location = new System.Drawing.Point(321, 4);
			this.showColorPoints.Name = "showColorPoints";
			this.showColorPoints.Size = new System.Drawing.Size(110, 17);
			this.showColorPoints.TabIndex = 2;
			this.showColorPoints.Text = "Show color points";
			this.showColorPoints.CheckedChanged += new System.EventHandler(this.showColorPoints_CheckedChanged);
			// 
			// showChromaMap
			// 
			this.showChromaMap.AutoSize = true;
			this.showChromaMap.Location = new System.Drawing.Point(437, 4);
			this.showChromaMap.Name = "showChromaMap";
			this.showChromaMap.Size = new System.Drawing.Size(114, 17);
			this.showChromaMap.TabIndex = 3;
			this.showChromaMap.Text = "Show chroma map";
			this.showChromaMap.CheckedChanged += new System.EventHandler(this.showChromaMap_CheckedChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(937, 262);
			this.Controls.Add(this.showChromaMap);
			this.Controls.Add(this.showColorPoints);
			this.Controls.Add(this.resultTextBox);
			this.Controls.Add(this.browseButton);
			this.DoubleBuffered = true;
			this.Name = "MainForm";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.TextBox resultTextBox;
		private System.Windows.Forms.CheckBox showColorPoints;
		private System.Windows.Forms.CheckBox showChromaMap;
	}
}


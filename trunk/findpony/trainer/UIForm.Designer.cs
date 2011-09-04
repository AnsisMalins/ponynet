namespace trainer
{
	partial class UIForm
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
			this.listBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// listBox
			// 
			this.listBox.BackColor = System.Drawing.SystemColors.Window;
			this.listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox.Location = new System.Drawing.Point(0, 0);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(284, 208);
			this.listBox.TabIndex = 0;
			// 
			// UIForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(284, 213);
			this.Controls.Add(this.listBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "UIForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UIForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.ListBox listBox;

	}
}
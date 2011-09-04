using System.Windows.Forms;

namespace trainer
{
	public partial class UIForm : Form
	{
		public UIForm(Form owner)
		{
			Owner = owner;
			InitializeComponent();
		}

		void UIForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing) e.Cancel = true;
		}
	}
}
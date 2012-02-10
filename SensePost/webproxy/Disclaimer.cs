using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Org.Mentalis.Proxy
{
	/// <summary>
	/// Summary description for Disclaimer.
	/// </summary>
	public class Disclaimer : System.Windows.Forms.Form
	{
		private DotNetSkin.SkinControls.SkinButton skinButtonAgree;
		private DotNetSkin.SkinControls.SkinButton skinButtonDisagree;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Disclaimer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Disclaimer));
			this.skinButtonAgree = new DotNetSkin.SkinControls.SkinButton();
			this.skinButtonDisagree = new DotNetSkin.SkinControls.SkinButton();
			this.SuspendLayout();
			// 
			// skinButtonAgree
			// 
			this.skinButtonAgree.BackColor = System.Drawing.Color.Transparent;
			this.skinButtonAgree.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.skinButtonAgree.Location = new System.Drawing.Point(13, 193);
			this.skinButtonAgree.Name = "skinButtonAgree";
			this.skinButtonAgree.Size = new System.Drawing.Size(127, 24);
			this.skinButtonAgree.TabIndex = 5;
			this.skinButtonAgree.Text = "I Agree...";
			// 
			// skinButtonDisagree
			// 
			this.skinButtonDisagree.BackColor = System.Drawing.Color.Transparent;
			this.skinButtonDisagree.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.skinButtonDisagree.Location = new System.Drawing.Point(13, 230);
			this.skinButtonDisagree.Name = "skinButtonDisagree";
			this.skinButtonDisagree.Size = new System.Drawing.Size(127, 24);
			this.skinButtonDisagree.TabIndex = 6;
			this.skinButtonDisagree.Text = "I Disagree";
			// 
			// Disclaimer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(474, 289);
			this.Controls.Add(this.skinButtonDisagree);
			this.Controls.Add(this.skinButtonAgree);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Disclaimer";
			this.Text = "Disclaimer";
			this.ResumeLayout(false);

		}
		#endregion
	}
}

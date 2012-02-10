using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Org.Mentalis.Proxy
{
	/// <summary>
	/// Summary description for showresults.
	/// </summary>
	public class showresults : System.Windows.Forms.Form
	{
		public System.Windows.Forms.RichTextBox rtbResults;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public showresults()
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
			this.rtbResults = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// rtbResults
			// 
			this.rtbResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbResults.Location = new System.Drawing.Point(0, 0);
			this.rtbResults.Name = "rtbResults";
			this.rtbResults.Size = new System.Drawing.Size(792, 541);
			this.rtbResults.TabIndex = 0;
			this.rtbResults.Text = "";
			this.rtbResults.WordWrap = false;
			// 
			// showresults
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 541);
			this.Controls.Add(this.rtbResults);
			this.Name = "showresults";
			this.Text = "Results";
			this.ResumeLayout(false);

		}
		#endregion
	}
}

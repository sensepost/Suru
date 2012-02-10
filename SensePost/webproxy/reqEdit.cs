using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SHDocVw;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using Org.Mentalis.Security.Ssl;
using Org.Mentalis.Security.Certificates;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using MOZILLACONTROLLib;



namespace Org.Mentalis.Proxy
{
	public class reqEdit : System.Windows.Forms.Form
	{
		
		
		
		private System.ComponentModel.Container components = null;

		public reqEdit()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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

	
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(reqEdit));
			// 
			// reqEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 673);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "reqEdit";

		}

		public void SetTextSelect()
		{
			foreach (Control myctl1 in this.Controls)
			{
				foreach (Control myctl2 in myctl1.Controls)
				{
					foreach (Control myctl3 in myctl2.Controls)
					{
						foreach (Control myctl4 in myctl3.Controls)
						{
							foreach (Control myctl5 in myctl4.Controls)
							{
								myctl5.Focus();
							}
							myctl4.Focus();
						}
						myctl3.Focus();
					}
					myctl2.Focus();
				}
				myctl1.Focus();
			}
		}
		
	}
}

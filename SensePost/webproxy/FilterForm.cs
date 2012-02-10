using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Org.Mentalis.Proxy {

	/// <summary>
	/// Summary description for FilterForm.
	/// </summary>
	public class FilterForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RichTextBox txtFilterHosts;
		private System.Windows.Forms.RichTextBox txtFilterLocation;
		private System.Windows.Forms.RichTextBox txtFilterExt;
		private System.Windows.Forms.RichTextBox txtFilterActions;
		private System.Windows.Forms.RichTextBox txtFilterRequestHeader;
		private System.Windows.Forms.RichTextBox txtFilterResponseHeader;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.RichTextBox txtFilterCookies;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RichTextBox txtFilterParameters;
		private DotNetSkin.SkinControls.SkinButtonRed btn_FilterReset;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterHosts;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterLocation;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterExt;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterActions;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterParameters;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterAnyParam;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterCookies;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterIsHTTP;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterIsHTTPS;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterBypass;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterRequest;
		private DotNetSkin.SkinControls.SkinCheckBox chkFilterResponse;
		private DotNetSkin.SkinControls.SkinButtonYellow btn_applyfilter;
		private WebProxy m_wb = null;

		
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FilterForm(WebProxy wb)
		{
			//
			// Required for Windows Form Designer support
			//
			m_wb = wb;
			InitializeComponent();
			loadFilter(txtFilterActions,WebProxy.currentFilter.Actions);
			loadFilter(txtFilterHosts,WebProxy.currentFilter.Hosts);
			loadFilter(txtFilterLocation,WebProxy.currentFilter.Locations);
			loadFilter(txtFilterParameters,WebProxy.currentFilter.Parameters);
			loadFilter(txtFilterExt,WebProxy.currentFilter.Ext);
			loadFilter(txtFilterCookies,WebProxy.currentFilter.Cookies);
			loadFilter(txtFilterRequestHeader,WebProxy.currentFilter.RequestHeader);
			loadFilter(txtFilterResponseHeader,WebProxy.currentFilter.ResponseHeader);

			chkFilterActions.Checked=WebProxy.currentFilter.inActions;
			chkFilterLocation.Checked=WebProxy.currentFilter.inLocations;
			chkFilterParameters.Checked=WebProxy.currentFilter.inParameters;
			chkFilterHosts.Checked=WebProxy.currentFilter.inHost;
			chkFilterExt.Checked=WebProxy.currentFilter.inExt;
			chkFilterCookies.Checked=WebProxy.currentFilter.inCookies;
			chkFilterResponse.Checked=WebProxy.currentFilter.inResponse;
			chkFilterRequest.Checked=WebProxy.currentFilter.inRequests;


			chkFilterBypass.Checked=WebProxy.bypassfiltercompletely;
			chkFilterAnyParam.Checked=WebProxy.anyPostorGet;

			chkFilterIsHTTP.Checked=WebProxy.currentFilter.IsHTTP;
			chkFilterIsHTTPS.Checked=WebProxy.currentFilter.IsHTTPS;
			

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FilterForm));
			this.txtFilterHosts = new System.Windows.Forms.RichTextBox();
			this.txtFilterLocation = new System.Windows.Forms.RichTextBox();
			this.txtFilterActions = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtFilterExt = new System.Windows.Forms.RichTextBox();
			this.txtFilterCookies = new System.Windows.Forms.RichTextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtFilterRequestHeader = new System.Windows.Forms.RichTextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtFilterResponseHeader = new System.Windows.Forms.RichTextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtFilterParameters = new System.Windows.Forms.RichTextBox();
			this.btn_FilterReset = new DotNetSkin.SkinControls.SkinButtonRed();
			this.btn_applyfilter = new DotNetSkin.SkinControls.SkinButtonYellow();
			this.chkFilterBypass = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkFilterHosts = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkFilterLocation = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkFilterExt = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkFilterActions = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkFilterParameters = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkFilterAnyParam = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkFilterCookies = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkFilterIsHTTP = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkFilterIsHTTPS = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkFilterRequest = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkFilterResponse = new DotNetSkin.SkinControls.SkinCheckBox();
			this.SuspendLayout();
			// 
			// txtFilterHosts
			// 
			this.txtFilterHosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFilterHosts.DetectUrls = false;
			this.txtFilterHosts.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.txtFilterHosts.Location = new System.Drawing.Point(16, 24);
			this.txtFilterHosts.Name = "txtFilterHosts";
			this.txtFilterHosts.Size = new System.Drawing.Size(200, 88);
			this.txtFilterHosts.TabIndex = 0;
			this.txtFilterHosts.Text = "";
			this.txtFilterHosts.WordWrap = false;
			// 
			// txtFilterLocation
			// 
			this.txtFilterLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFilterLocation.DetectUrls = false;
			this.txtFilterLocation.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.txtFilterLocation.Location = new System.Drawing.Point(224, 24);
			this.txtFilterLocation.Name = "txtFilterLocation";
			this.txtFilterLocation.Size = new System.Drawing.Size(200, 88);
			this.txtFilterLocation.TabIndex = 2;
			this.txtFilterLocation.Text = "";
			this.txtFilterLocation.WordWrap = false;
			// 
			// txtFilterActions
			// 
			this.txtFilterActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFilterActions.DetectUrls = false;
			this.txtFilterActions.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.txtFilterActions.Location = new System.Drawing.Point(224, 160);
			this.txtFilterActions.Name = "txtFilterActions";
			this.txtFilterActions.Size = new System.Drawing.Size(200, 88);
			this.txtFilterActions.TabIndex = 6;
			this.txtFilterActions.Text = "";
			this.txtFilterActions.WordWrap = false;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.label1.Location = new System.Drawing.Point(104, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 16);
			this.label1.TabIndex = 11;
			this.label1.Text = "Hosts";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.label2.Location = new System.Drawing.Point(304, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 12;
			this.label2.Text = "Location";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.label4.Location = new System.Drawing.Point(304, 144);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 14;
			this.label4.Text = "Actions";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.label5.Location = new System.Drawing.Point(88, 144);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 17;
			this.label5.Text = "Extensions";
			// 
			// txtFilterExt
			// 
			this.txtFilterExt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFilterExt.DetectUrls = false;
			this.txtFilterExt.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.txtFilterExt.Location = new System.Drawing.Point(16, 160);
			this.txtFilterExt.Name = "txtFilterExt";
			this.txtFilterExt.Size = new System.Drawing.Size(200, 88);
			this.txtFilterExt.TabIndex = 4;
			this.txtFilterExt.Text = "";
			this.txtFilterExt.WordWrap = false;
			// 
			// txtFilterCookies
			// 
			this.txtFilterCookies.BackColor = System.Drawing.Color.Cornsilk;
			this.txtFilterCookies.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFilterCookies.DetectUrls = false;
			this.txtFilterCookies.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.txtFilterCookies.Location = new System.Drawing.Point(224, 296);
			this.txtFilterCookies.Name = "txtFilterCookies";
			this.txtFilterCookies.Size = new System.Drawing.Size(200, 88);
			this.txtFilterCookies.TabIndex = 11;
			this.txtFilterCookies.Text = "";
			this.txtFilterCookies.WordWrap = false;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.label6.Location = new System.Drawing.Point(304, 280);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 16);
			this.label6.TabIndex = 20;
			this.label6.Text = "Cookies";
			// 
			// txtFilterRequestHeader
			// 
			this.txtFilterRequestHeader.BackColor = System.Drawing.Color.LightYellow;
			this.txtFilterRequestHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFilterRequestHeader.DetectUrls = false;
			this.txtFilterRequestHeader.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.txtFilterRequestHeader.ForeColor = System.Drawing.Color.Black;
			this.txtFilterRequestHeader.Location = new System.Drawing.Point(16, 432);
			this.txtFilterRequestHeader.Name = "txtFilterRequestHeader";
			this.txtFilterRequestHeader.Size = new System.Drawing.Size(200, 88);
			this.txtFilterRequestHeader.TabIndex = 13;
			this.txtFilterRequestHeader.Text = "";
			this.txtFilterRequestHeader.WordWrap = false;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.label7.Location = new System.Drawing.Point(80, 416);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 16);
			this.label7.TabIndex = 26;
			this.label7.Text = "Full request";
			// 
			// txtFilterResponseHeader
			// 
			this.txtFilterResponseHeader.BackColor = System.Drawing.Color.LightYellow;
			this.txtFilterResponseHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFilterResponseHeader.DetectUrls = false;
			this.txtFilterResponseHeader.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.txtFilterResponseHeader.ForeColor = System.Drawing.Color.Black;
			this.txtFilterResponseHeader.Location = new System.Drawing.Point(224, 432);
			this.txtFilterResponseHeader.Name = "txtFilterResponseHeader";
			this.txtFilterResponseHeader.Size = new System.Drawing.Size(200, 88);
			this.txtFilterResponseHeader.TabIndex = 15;
			this.txtFilterResponseHeader.Text = "";
			this.txtFilterResponseHeader.WordWrap = false;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.label8.Location = new System.Drawing.Point(280, 416);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(88, 16);
			this.label8.TabIndex = 29;
			this.label8.Text = "Response header";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.label3.Location = new System.Drawing.Point(88, 280);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 35;
			this.label3.Text = "Parameters";
			// 
			// txtFilterParameters
			// 
			this.txtFilterParameters.BackColor = System.Drawing.Color.Cornsilk;
			this.txtFilterParameters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFilterParameters.DetectUrls = false;
			this.txtFilterParameters.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.txtFilterParameters.Location = new System.Drawing.Point(16, 296);
			this.txtFilterParameters.Name = "txtFilterParameters";
			this.txtFilterParameters.Size = new System.Drawing.Size(200, 88);
			this.txtFilterParameters.TabIndex = 8;
			this.txtFilterParameters.Text = "";
			this.txtFilterParameters.WordWrap = false;
			// 
			// btn_FilterReset
			// 
			this.btn_FilterReset.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.btn_FilterReset.ForeColor = System.Drawing.Color.Brown;
			this.btn_FilterReset.Location = new System.Drawing.Point(8, 624);
			this.btn_FilterReset.Name = "btn_FilterReset";
			this.btn_FilterReset.Size = new System.Drawing.Size(424, 24);
			this.btn_FilterReset.TabIndex = 21;
			this.btn_FilterReset.Text = "Reset Filter";
			this.btn_FilterReset.Click += new System.EventHandler(this.skinButtonRed1_Click);
			// 
			// btn_applyfilter
			// 
			this.btn_applyfilter.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.btn_applyfilter.ForeColor = System.Drawing.Color.DarkGoldenrod;
			this.btn_applyfilter.Location = new System.Drawing.Point(8, 600);
			this.btn_applyfilter.Name = "btn_applyfilter";
			this.btn_applyfilter.Size = new System.Drawing.Size(424, 24);
			this.btn_applyfilter.TabIndex = 20;
			this.btn_applyfilter.Text = "A P P L Y";
			this.btn_applyfilter.Click += new System.EventHandler(this.btn_applyfilter_Click);
			// 
			// chkFilterBypass
			// 
			this.chkFilterBypass.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterBypass.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterBypass.Location = new System.Drawing.Point(16, 576);
			this.chkFilterBypass.Name = "chkFilterBypass";
			this.chkFilterBypass.Size = new System.Drawing.Size(88, 16);
			this.chkFilterBypass.TabIndex = 19;
			this.chkFilterBypass.Text = "Bypass filter";
			// 
			// chkFilterHosts
			// 
			this.chkFilterHosts.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterHosts.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterHosts.Location = new System.Drawing.Point(16, 112);
			this.chkFilterHosts.Name = "chkFilterHosts";
			this.chkFilterHosts.Size = new System.Drawing.Size(88, 16);
			this.chkFilterHosts.TabIndex = 1;
			this.chkFilterHosts.Text = "Invert";
			// 
			// chkFilterLocation
			// 
			this.chkFilterLocation.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterLocation.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterLocation.Location = new System.Drawing.Point(224, 112);
			this.chkFilterLocation.Name = "chkFilterLocation";
			this.chkFilterLocation.Size = new System.Drawing.Size(88, 16);
			this.chkFilterLocation.TabIndex = 3;
			this.chkFilterLocation.Text = "Invert";
			// 
			// chkFilterExt
			// 
			this.chkFilterExt.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterExt.Checked = true;
			this.chkFilterExt.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFilterExt.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterExt.Location = new System.Drawing.Point(16, 248);
			this.chkFilterExt.Name = "chkFilterExt";
			this.chkFilterExt.Size = new System.Drawing.Size(88, 16);
			this.chkFilterExt.TabIndex = 5;
			this.chkFilterExt.Text = "Invert";
			// 
			// chkFilterActions
			// 
			this.chkFilterActions.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterActions.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterActions.Location = new System.Drawing.Point(224, 248);
			this.chkFilterActions.Name = "chkFilterActions";
			this.chkFilterActions.Size = new System.Drawing.Size(96, 16);
			this.chkFilterActions.TabIndex = 7;
			this.chkFilterActions.Text = "Invert";
			// 
			// chkFilterParameters
			// 
			this.chkFilterParameters.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterParameters.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterParameters.Location = new System.Drawing.Point(16, 384);
			this.chkFilterParameters.Name = "chkFilterParameters";
			this.chkFilterParameters.Size = new System.Drawing.Size(56, 16);
			this.chkFilterParameters.TabIndex = 9;
			this.chkFilterParameters.Text = "Invert";
			// 
			// chkFilterAnyParam
			// 
			this.chkFilterAnyParam.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterAnyParam.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterAnyParam.Location = new System.Drawing.Point(136, 384);
			this.chkFilterAnyParam.Name = "chkFilterAnyParam";
			this.chkFilterAnyParam.Size = new System.Drawing.Size(80, 16);
			this.chkFilterAnyParam.TabIndex = 10;
			this.chkFilterAnyParam.Text = "Any param";
			// 
			// chkFilterCookies
			// 
			this.chkFilterCookies.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterCookies.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterCookies.Location = new System.Drawing.Point(224, 384);
			this.chkFilterCookies.Name = "chkFilterCookies";
			this.chkFilterCookies.Size = new System.Drawing.Size(88, 16);
			this.chkFilterCookies.TabIndex = 12;
			this.chkFilterCookies.Text = "Invert";
			// 
			// chkFilterIsHTTP
			// 
			this.chkFilterIsHTTP.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterIsHTTP.Checked = true;
			this.chkFilterIsHTTP.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFilterIsHTTP.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterIsHTTP.Location = new System.Drawing.Point(16, 544);
			this.chkFilterIsHTTP.Name = "chkFilterIsHTTP";
			this.chkFilterIsHTTP.Size = new System.Drawing.Size(104, 16);
			this.chkFilterIsHTTP.TabIndex = 17;
			this.chkFilterIsHTTP.Text = "Normal HTTP";
			// 
			// chkFilterIsHTTPS
			// 
			this.chkFilterIsHTTPS.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterIsHTTPS.Checked = true;
			this.chkFilterIsHTTPS.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFilterIsHTTPS.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterIsHTTPS.Location = new System.Drawing.Point(16, 560);
			this.chkFilterIsHTTPS.Name = "chkFilterIsHTTPS";
			this.chkFilterIsHTTPS.Size = new System.Drawing.Size(96, 16);
			this.chkFilterIsHTTPS.TabIndex = 18;
			this.chkFilterIsHTTPS.Text = "SSL requests";
			// 
			// chkFilterRequest
			// 
			this.chkFilterRequest.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterRequest.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterRequest.Location = new System.Drawing.Point(16, 520);
			this.chkFilterRequest.Name = "chkFilterRequest";
			this.chkFilterRequest.Size = new System.Drawing.Size(88, 16);
			this.chkFilterRequest.TabIndex = 14;
			this.chkFilterRequest.Text = "Invert";
			// 
			// chkFilterResponse
			// 
			this.chkFilterResponse.BackColor = System.Drawing.Color.Transparent;
			this.chkFilterResponse.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.chkFilterResponse.Location = new System.Drawing.Point(224, 520);
			this.chkFilterResponse.Name = "chkFilterResponse";
			this.chkFilterResponse.Size = new System.Drawing.Size(88, 16);
			this.chkFilterResponse.TabIndex = 16;
			this.chkFilterResponse.Text = "Invert";
			// 
			// FilterForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.DarkGray;
			this.ClientSize = new System.Drawing.Size(440, 653);
			this.Controls.Add(this.chkFilterResponse);
			this.Controls.Add(this.chkFilterRequest);
			this.Controls.Add(this.chkFilterIsHTTPS);
			this.Controls.Add(this.chkFilterIsHTTP);
			this.Controls.Add(this.chkFilterCookies);
			this.Controls.Add(this.chkFilterAnyParam);
			this.Controls.Add(this.chkFilterParameters);
			this.Controls.Add(this.chkFilterActions);
			this.Controls.Add(this.chkFilterExt);
			this.Controls.Add(this.chkFilterLocation);
			this.Controls.Add(this.chkFilterHosts);
			this.Controls.Add(this.chkFilterBypass);
			this.Controls.Add(this.btn_applyfilter);
			this.Controls.Add(this.btn_FilterReset);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtFilterParameters);
			this.Controls.Add(this.txtFilterResponseHeader);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.txtFilterRequestHeader);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.txtFilterCookies);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtFilterExt);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtFilterActions);
			this.Controls.Add(this.txtFilterLocation);
			this.Controls.Add(this.txtFilterHosts);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(448, 680);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(448, 680);
			this.Name = "FilterForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Configure Filter/Finder";
			this.ResumeLayout(false);

		}
		#endregion

		/*private void btnFilterApply_Click(object sender, System.EventArgs e) {
			
			WebProxy.currentFilter.Actions.Clear();
			WebProxy.currentFilter.Hosts.Clear();
			WebProxy.currentFilter.Locations.Clear();
			WebProxy.currentFilter.Parameters.Clear();
			WebProxy.currentFilter.Ext.Clear();
			WebProxy.currentFilter.Cookies.Clear();
			WebProxy.currentFilter.RequestHeader.Clear();
			WebProxy.currentFilter.ResponseHeader.Clear();

			WebProxy.currentFilter.Hosts.AddRange(txtFilterHosts.Lines);
			WebProxy.currentFilter.Locations.AddRange(txtFilterLocation.Lines);
			WebProxy.currentFilter.Parameters.AddRange(txtFilterParameters.Lines);
			WebProxy.currentFilter.Actions.AddRange(txtFilterActions.Lines);
			WebProxy.currentFilter.Ext.AddRange(txtFilterExt.Lines);
			WebProxy.currentFilter.Cookies.AddRange(txtFilterCookies.Lines);
			WebProxy.currentFilter.RequestHeader.AddRange(txtFilterRequestHeader.Lines);
			WebProxy.currentFilter.ResponseHeader.AddRange(txtFilterResponseHeader.Lines);

			WebProxy.currentFilter.inActions=chkFilterActions.Checked;
			WebProxy.currentFilter.inHost=chkFilterHosts.Checked;
			WebProxy.currentFilter.inLocations=chkFilterLocation.Checked;
			WebProxy.currentFilter.inParameters=chkFilterParameters.Checked;
			WebProxy.currentFilter.inExt=chkFilterExt.Checked;
			WebProxy.currentFilter.inCookies=chkFilterCookies.Checked;
			WebProxy.currentFilter.inRequests=chkFilterRequest.Checked;
			WebProxy.currentFilter.inResponse=chkFilterResponse.Checked;

			WebProxy.currentFilter.IsHTTP=chkFilterIsHTTP.Checked;
			WebProxy.currentFilter.IsHTTPS=chkFilterIsHTTPS.Checked;

			WebProxy.bypassfiltercompletely=chkFilterBypass.Checked;
			WebProxy.anyPostorGet=chkFilterAnyParam.Checked;

			WebProxy.button2.PerformClick();
			
		}*/
		private void loadFilter(RichTextBox where, ArrayList which){
			where.Clear();
			foreach (string line in which){
				where.Text+=line+"\r\n";
			}
			string all=where.Text;
			all=all.TrimEnd('\n').TrimEnd('\r');
			where.Clear();
			where.Text=all;

		}

		private void skinButtonRed1_Click(object sender, System.EventArgs e)
		{
			txtFilterActions.Clear();
			chkFilterActions.Checked=false;
			
			txtFilterCookies.Clear();
			chkFilterCookies.Checked=false;

			txtFilterExt.Text="css\r\njpg\r\ngif\r\nico\r\npng\r\njs";
			chkFilterExt.Checked=true;

			txtFilterHosts.Clear();
			chkFilterHosts.Checked=false;

			txtFilterLocation.Clear();
			chkFilterLocation.Checked=false;

			txtFilterParameters.Clear();
			chkFilterParameters.Checked=false;

			chkFilterAnyParam.Checked=false;

			txtFilterRequestHeader.Clear();
			chkFilterRequest.Checked=false;

			txtFilterResponseHeader.Clear();
			chkFilterResponse.Checked=false;

			chkFilterIsHTTP.Checked=true;
			chkFilterIsHTTPS.Checked=true;
		}

		private void btn_applyfilter_Click(object sender, System.EventArgs e)
		{
			WebProxy.currentFilter.Actions.Clear();
			WebProxy.currentFilter.Hosts.Clear();
			WebProxy.currentFilter.Locations.Clear();
			WebProxy.currentFilter.Parameters.Clear();
			WebProxy.currentFilter.Ext.Clear();
			WebProxy.currentFilter.Cookies.Clear();
			WebProxy.currentFilter.RequestHeader.Clear();
			WebProxy.currentFilter.ResponseHeader.Clear();

			WebProxy.currentFilter.Hosts.AddRange(txtFilterHosts.Lines);
			WebProxy.currentFilter.Locations.AddRange(txtFilterLocation.Lines);
			WebProxy.currentFilter.Parameters.AddRange(txtFilterParameters.Lines);
			WebProxy.currentFilter.Actions.AddRange(txtFilterActions.Lines);
			WebProxy.currentFilter.Ext.AddRange(txtFilterExt.Lines);
			WebProxy.currentFilter.Cookies.AddRange(txtFilterCookies.Lines);
			WebProxy.currentFilter.RequestHeader.AddRange(txtFilterRequestHeader.Lines);
			WebProxy.currentFilter.ResponseHeader.AddRange(txtFilterResponseHeader.Lines);

			WebProxy.currentFilter.inActions=chkFilterActions.Checked;
			WebProxy.currentFilter.inHost=chkFilterHosts.Checked;
			WebProxy.currentFilter.inLocations=chkFilterLocation.Checked;
			WebProxy.currentFilter.inParameters=chkFilterParameters.Checked;
			WebProxy.currentFilter.inExt=chkFilterExt.Checked;
			WebProxy.currentFilter.inCookies=chkFilterCookies.Checked;
			WebProxy.currentFilter.inRequests=chkFilterRequest.Checked;
			WebProxy.currentFilter.inResponse=chkFilterResponse.Checked;

			WebProxy.currentFilter.IsHTTP=chkFilterIsHTTP.Checked;
			WebProxy.currentFilter.IsHTTPS=chkFilterIsHTTPS.Checked;

			WebProxy.bypassfiltercompletely=chkFilterBypass.Checked;
			WebProxy.anyPostorGet=chkFilterAnyParam.Checked;
			m_wb.button2.PerformClick();
			//WebProxy.button2.PerformClick();
		}

	}
}

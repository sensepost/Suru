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

namespace Org.Mentalis.Proxy {

	#region structures used throughout
	struct HTTPRequest {
		public bool isSSL;
		public string host;
		public string URL;
		public string header;
		public DateTime DT;
		public string response;
		public int reqnum;
		public bool isHighlighted;
		public int isColour;
	}

	struct Mangled {
		public string varname;
		public string varmd5;
		public string varsha1;
		public string varbase64enc;
		public string varbase64dec;
		public string varval;
		public string varvalmd5;
		public string varvalsha1;
		public string varvalbase64enc;
		public string varvalbase64dec;
		public string type;
	}


	struct detailedRequest{
		public bool isSSL;
		public string action;
		public string URL;
		public string host;
		public string filetype;
		public ArrayList GETparameters;
		public string header;
		public string filename;
		public ArrayList cookie;
		public ArrayList POSTparameters;
		public string port;
		public ArrayList Processed;
		public bool isXML;
		public bool isMultiPart;
	}

	struct detailedRequest_text{
		public string host;
		public string port;
		public string body;
		public bool isSSL;
	}

	struct detailedRequest_IE{
		public object url;
		public object headers;
		public object postdata;
	}

	public struct jobQ{
		public string jobtype;
		public string fullrequest;
		public string fullresponse;
		public long fuzzyindex;
		public string ext;
		public string location;
		public string targethost;
		public string targetport;
		public string header;
		public string filename;
		public bool isSSL;
	}

	public struct niktoRequests {
		public string type;
		public string request;
		public string description;
		public string trigger;
		public bool isSSL;
		public string method;
			
	}
	public struct niktoFP{
		public string URLlocation;
		public string HTTPblob;
		public string filetype;
		public string method;
		public string host;
		public bool isSSL;
	}

	public struct discovered{
		public string protocol;
		public string host;
		public string port;
		public string URL;
		public bool isSSL;
		public string header;
		public int mode;
	}

	public struct TreeTagType{
		public string header;
		public bool isSSL;
		public string port;
	}
	public struct FPdata {
		public string URLlocation;
		public string filetype;
		public string host;
		public string method;
	}
	public struct FilterStruct{
		public ArrayList Hosts;
		public ArrayList Locations;
		public ArrayList Parameters;
		public ArrayList Actions;
		public ArrayList Ext;
		public ArrayList Cookies;
		public ArrayList RequestHeader;
		public ArrayList ResponseHeader;
		public bool inHost;
		public bool inLocations;
		public bool inParameters;
		public bool inActions;
		public bool inExt;
		public bool inCookies;
		public bool inRequests;
		public bool inResponse;
		public bool IsHTTP;
		public bool IsHTTPS;
	}
	

	#endregion
	
	public class WebProxy : System.Windows.Forms.Form {

		#region Windows Form Designer generated code
		// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WebProxy));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.Proxy = new System.Windows.Forms.TabPage();
			this.panel8 = new System.Windows.Forms.Panel();
			this.panel11 = new System.Windows.Forms.Panel();
			this.txtHTTPdetails = new System.Windows.Forms.RichTextBox();
			this.panel10 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.txtTargetHost = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtTargetPort = new System.Windows.Forms.TextBox();
			this.chkTargetIsSSL = new DotNetSkin.SkinControls.SkinCheckBox();
			this.label39 = new System.Windows.Forms.Label();
			this.lblDateTime = new System.Windows.Forms.Label();
			this.panel9 = new System.Windows.Forms.Panel();
			this.btnClear = new DotNetSkin.SkinControls.SkinButtonRed();
			this.btnReplay = new DotNetSkin.SkinControls.SkinButton();
			this.btnSendRawRequest = new DotNetSkin.SkinControls.SkinButton();
			this.chkProxyAutoUpdate = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkImmFuzz = new DotNetSkin.SkinControls.SkinCheckBox();
			this.button2 = new DotNetSkin.SkinControls.SkinButtonYellow();
			this.chkShowHosts = new DotNetSkin.SkinControls.SkinCheckBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel12 = new System.Windows.Forms.Panel();
			this.panel14 = new System.Windows.Forms.Panel();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.panel13 = new System.Windows.Forms.Panel();
			this.splitter3 = new System.Windows.Forms.Splitter();
			this.panel5 = new System.Windows.Forms.Panel();
			this.panel7 = new System.Windows.Forms.Panel();
			this.lstCrowResponse = new System.Windows.Forms.ListBox();
			this.contextMenu2 = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.panel6 = new System.Windows.Forms.Panel();
			this.txtCrowResponse = new System.Windows.Forms.RichTextBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.chkCrowStoreResponse = new DotNetSkin.SkinControls.SkinCheckBox();
			this.prgBarCrow = new System.Windows.Forms.ProgressBar();
			this.label42 = new System.Windows.Forms.Label();
			this.updownGroupTolerance = new System.Windows.Forms.NumericUpDown();
			this.btnCrowGroup = new DotNetSkin.SkinControls.SkinButton();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.radioNotEqual = new DotNetSkin.SkinControls.SkinRadioButton();
			this.radioAll = new DotNetSkin.SkinControls.SkinRadioButton();
			this.btnExport = new DotNetSkin.SkinControls.SkinButton();
			this.updowntwo = new System.Windows.Forms.NumericUpDown();
			this.radioequal = new DotNetSkin.SkinControls.SkinRadioButton();
			this.radiooutside = new DotNetSkin.SkinControls.SkinRadioButton();
			this.radioinside = new DotNetSkin.SkinControls.SkinRadioButton();
			this.btnReCalc = new DotNetSkin.SkinControls.SkinButton();
			this.updownCrowAI = new System.Windows.Forms.NumericUpDown();
			this.chkUseAIAtAll = new DotNetSkin.SkinControls.SkinCheckBox();
			this.txtContentEndWord = new System.Windows.Forms.TextBox();
			this.txtContentStartWord = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.grpInner = new System.Windows.Forms.GroupBox();
			this.comboCrowEncode = new System.Windows.Forms.ComboBox();
			this.btnManualBaseResponse = new DotNetSkin.SkinControls.SkinButton();
			this.cmbCustom = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lblFile1 = new System.Windows.Forms.Label();
			this.btnFileLoad1 = new DotNetSkin.SkinControls.SkinButton();
			this.radioFile1 = new DotNetSkin.SkinControls.SkinRadioButton();
			this.radioNumeric1 = new DotNetSkin.SkinControls.SkinRadioButton();
			this.txtNumericTo1 = new System.Windows.Forms.TextBox();
			this.txtNumericFrom1 = new System.Windows.Forms.TextBox();
			this.btnCrowStart = new DotNetSkin.SkinControls.SkinButtonGreen();
			this.btnCrowPause = new DotNetSkin.SkinControls.SkinButtonYellow();
			this.btnCrowStop = new DotNetSkin.SkinControls.SkinButtonRed();
			this.Misc = new System.Windows.Forms.TabPage();
			this.panel20 = new System.Windows.Forms.Panel();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.panel30 = new System.Windows.Forms.Panel();
			this.panel32 = new System.Windows.Forms.Panel();
			this.groupBox12 = new System.Windows.Forms.GroupBox();
			this.txtToolsBase64Encoded = new System.Windows.Forms.RichTextBox();
			this.splitter8 = new System.Windows.Forms.Splitter();
			this.panel31 = new System.Windows.Forms.Panel();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.txtToolsBase64Decoded = new System.Windows.Forms.RichTextBox();
			this.panel49 = new System.Windows.Forms.Panel();
			this.btnToolsGo = new DotNetSkin.SkinControls.SkinButton();
			this.splitter16 = new System.Windows.Forms.Splitter();
			this.panel29 = new System.Windows.Forms.Panel();
			this.groupBox15 = new System.Windows.Forms.GroupBox();
			this.txtToolsMD5Sum = new System.Windows.Forms.RichTextBox();
			this.groupBox13 = new System.Windows.Forms.GroupBox();
			this.txtToolsHex = new System.Windows.Forms.RichTextBox();
			this.groupBox14 = new System.Windows.Forms.GroupBox();
			this.txtToolsSHA1 = new System.Windows.Forms.RichTextBox();
			this.groupBox16 = new System.Windows.Forms.GroupBox();
			this.txtToolsUuserInput = new System.Windows.Forms.RichTextBox();
			this.splitter7 = new System.Windows.Forms.Splitter();
			this.panel21 = new System.Windows.Forms.Panel();
			this.panel23 = new System.Windows.Forms.Panel();
			this.panel48 = new System.Windows.Forms.Panel();
			this.lstSRIncoming = new System.Windows.Forms.ListBox();
			this.panel24 = new System.Windows.Forms.Panel();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label19 = new System.Windows.Forms.Label();
			this.btnSRDeleteOut = new DotNetSkin.SkinControls.SkinButton();
			this.btnSRAddOut = new DotNetSkin.SkinControls.SkinButton();
			this.label18 = new System.Windows.Forms.Label();
			this.txtSRReplaceIn = new System.Windows.Forms.RichTextBox();
			this.txtSRSearchIn = new System.Windows.Forms.RichTextBox();
			this.splitter6 = new System.Windows.Forms.Splitter();
			this.panel22 = new System.Windows.Forms.Panel();
			this.panel47 = new System.Windows.Forms.Panel();
			this.lstSRlist = new System.Windows.Forms.ListBox();
			this.panel25 = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnSRDelete = new DotNetSkin.SkinControls.SkinButton();
			this.btnSRAdd = new DotNetSkin.SkinControls.SkinButton();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtSRReplace = new System.Windows.Forms.RichTextBox();
			this.txtSRSearch = new System.Windows.Forms.RichTextBox();
			this.splitter5 = new System.Windows.Forms.Splitter();
			this.panel18 = new System.Windows.Forms.Panel();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.panel19 = new System.Windows.Forms.Panel();
			this.txtAutoMangle = new System.Windows.Forms.RichTextBox();
			this.panel50 = new System.Windows.Forms.Panel();
			this.panel28 = new System.Windows.Forms.Panel();
			this.lstMangleUserInput = new System.Windows.Forms.ListBox();
			this.panel27 = new System.Windows.Forms.Panel();
			this.txtMangleUserInput = new System.Windows.Forms.RichTextBox();
			this.btnMangleAddUserInput = new DotNetSkin.SkinControls.SkinButton();
			this.btnMangleDeleteUserInput = new DotNetSkin.SkinControls.SkinButton();
			this.panel26 = new System.Windows.Forms.Panel();
			this.chkAutoMangle = new DotNetSkin.SkinControls.SkinCheckBox();
			this.btnAnaliseMangle = new DotNetSkin.SkinControls.SkinButton();
			this.Recon = new System.Windows.Forms.TabPage();
			this.panel17 = new System.Windows.Forms.Panel();
			this.treeRecon = new System.Windows.Forms.TreeView();
			this.cntReconTree = new System.Windows.Forms.ContextMenu();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItem18 = new System.Windows.Forms.MenuItem();
			this.panel16 = new System.Windows.Forms.Panel();
			this.chkSmartDirScan = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkSmartFileDeep = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkSmartFileShallow = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkDoRecon = new DotNetSkin.SkinControls.SkinCheckBox();
			this.cmbReconTargetHost = new System.Windows.Forms.ComboBox();
			this.ckhReconIndex = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkReconDirMine = new DotNetSkin.SkinControls.SkinCheckBox();
			this.label23 = new System.Windows.Forms.Label();
			this.lblJobQLength = new System.Windows.Forms.Label();
			this.lblCurrentJobQ = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.NUPDOWNBackEnd = new System.Windows.Forms.NumericUpDown();
			this.label17 = new System.Windows.Forms.Label();
			this.trckReconSpeed = new System.Windows.Forms.TrackBar();
			this.panel15 = new System.Windows.Forms.Panel();
			this.chkReconAlwaysExpand = new DotNetSkin.SkinControls.SkinCheckBox();
			this.btnReconTreeExpandAll = new DotNetSkin.SkinControls.SkinButton();
			this.btnReconTreeCollapseAll = new DotNetSkin.SkinControls.SkinButton();
			this.btnTreeReload = new DotNetSkin.SkinControls.SkinButton();
			this.btnClearJobQ = new DotNetSkin.SkinControls.SkinButtonYellow();
			this.btnReconClearTree = new DotNetSkin.SkinControls.SkinButtonRed();
			this.button1 = new DotNetSkin.SkinControls.SkinButton();
			this.Config = new System.Windows.Forms.TabPage();
			this.panel38 = new System.Windows.Forms.Panel();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.panel46 = new System.Windows.Forms.Panel();
			this.txtConfigFuzzContent = new System.Windows.Forms.RichTextBox();
			this.panel45 = new System.Windows.Forms.Panel();
			this.btnBackEndUpdateDirs = new DotNetSkin.SkinControls.SkinButtonYellow();
			this.cmbBackEndUpdate = new System.Windows.Forms.ComboBox();
			this.cmbFuzzFileEdit = new System.Windows.Forms.ComboBox();
			this.txtConfigFuzzFileName = new System.Windows.Forms.TextBox();
			this.btnUpdateQuickNav = new DotNetSkin.SkinControls.SkinButton();
			this.txtFuzzDirLocation = new System.Windows.Forms.Label();
			this.btnFuzzDBLocationFind = new DotNetSkin.SkinControls.SkinButton();
			this.label9 = new System.Windows.Forms.Label();
			this.panel44 = new System.Windows.Forms.Panel();
			this.btnSaveFuzzFile = new DotNetSkin.SkinControls.SkinButtonYellow();
			this.splitter10 = new System.Windows.Forms.Splitter();
			this.panel37 = new System.Windows.Forms.Panel();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.panel43 = new System.Windows.Forms.Panel();
			this.groupBox22 = new System.Windows.Forms.GroupBox();
			this.txtReconSkipSites = new System.Windows.Forms.RichTextBox();
			this.splitter14 = new System.Windows.Forms.Splitter();
			this.panel42 = new System.Windows.Forms.Panel();
			this.groupBox21 = new System.Windows.Forms.GroupBox();
			this.txtWiktoTestFilenames = new System.Windows.Forms.RichTextBox();
			this.splitter13 = new System.Windows.Forms.Splitter();
			this.panel41 = new System.Windows.Forms.Panel();
			this.groupBox20 = new System.Windows.Forms.GroupBox();
			this.txtWiktoTestTypes = new System.Windows.Forms.RichTextBox();
			this.splitter12 = new System.Windows.Forms.Splitter();
			this.panel40 = new System.Windows.Forms.Panel();
			this.groupBox19 = new System.Windows.Forms.GroupBox();
			this.txtWiktoSkipDirs = new System.Windows.Forms.RichTextBox();
			this.splitter11 = new System.Windows.Forms.Splitter();
			this.panel39 = new System.Windows.Forms.Panel();
			this.groupBox18 = new System.Windows.Forms.GroupBox();
			this.txtWiktoTestDirs = new System.Windows.Forms.RichTextBox();
			this.panel33 = new System.Windows.Forms.Panel();
			this.panel36 = new System.Windows.Forms.Panel();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.bntProxyChange = new DotNetSkin.SkinControls.SkinButton();
			this.updownListenPort = new System.Windows.Forms.NumericUpDown();
			this.chkListenEverywhere = new DotNetSkin.SkinControls.SkinCheckBox();
			this.label40 = new System.Windows.Forms.Label();
			this.panel35 = new System.Windows.Forms.Panel();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.chkReplayIE = new DotNetSkin.SkinControls.SkinCheckBox();
			this.chkReplayFireFox = new DotNetSkin.SkinControls.SkinCheckBox();
			this.panel34 = new System.Windows.Forms.Panel();
			this.grpSepChars = new System.Windows.Forms.GroupBox();
			this.label36 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.txtVariableSeparator = new System.Windows.Forms.TextBox();
			this.label31 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.txtBaseURLSeparator = new System.Windows.Forms.TextBox();
			this.txtKeyValueSeparator = new System.Windows.Forms.TextBox();
			this.label33 = new System.Windows.Forms.Label();
			this.txtCookieVariableSeparator = new System.Windows.Forms.TextBox();
			this.txtCookieKeyValueSeparator = new System.Windows.Forms.TextBox();
			this.label34 = new System.Windows.Forms.Label();
			this.groupBox17 = new System.Windows.Forms.GroupBox();
			this.skinButton1 = new DotNetSkin.SkinControls.SkinButton();
			this.updownTimeOut = new System.Windows.Forms.NumericUpDown();
			this.label29 = new System.Windows.Forms.Label();
			this.btnClearALL = new DotNetSkin.SkinControls.SkinButtonRed();
			this.btnSaveData = new DotNetSkin.SkinControls.SkinButton();
			this.btnLoadData = new DotNetSkin.SkinControls.SkinButton();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.mnu_FILTER = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.mnu_HLBrown = new System.Windows.Forms.MenuItem();
			this.mnu_HLRed = new System.Windows.Forms.MenuItem();
			this.mnu_HLOrange = new System.Windows.Forms.MenuItem();
			this.mnu_HLYellow = new System.Windows.Forms.MenuItem();
			this.mnu_HLGreen = new System.Windows.Forms.MenuItem();
			this.mnu_HLAqua = new System.Windows.Forms.MenuItem();
			this.mnu_HLBlue = new System.Windows.Forms.MenuItem();
			this.mnu_HLPurple = new System.Windows.Forms.MenuItem();
			this.mnu_HLGrey = new System.Windows.Forms.MenuItem();
			this.mnu_HLBlack = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mnu_RE = new System.Windows.Forms.MenuItem();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.timer2 = new System.Windows.Forms.Timer(this.components);
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.timer_mangled = new System.Windows.Forms.Timer(this.components);
			this.toolTipSuru = new System.Windows.Forms.ToolTip(this.components);
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.tabControl1.SuspendLayout();
			this.Proxy.SuspendLayout();
			this.panel8.SuspendLayout();
			this.panel11.SuspendLayout();
			this.panel10.SuspendLayout();
			this.panel9.SuspendLayout();
			this.panel12.SuspendLayout();
			this.panel14.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel7.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel4.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.updownGroupTolerance)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.updowntwo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.updownCrowAI)).BeginInit();
			this.grpInner.SuspendLayout();
			this.Misc.SuspendLayout();
			this.panel20.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.panel30.SuspendLayout();
			this.panel32.SuspendLayout();
			this.groupBox12.SuspendLayout();
			this.panel31.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.panel49.SuspendLayout();
			this.panel29.SuspendLayout();
			this.groupBox15.SuspendLayout();
			this.groupBox13.SuspendLayout();
			this.groupBox14.SuspendLayout();
			this.groupBox16.SuspendLayout();
			this.panel21.SuspendLayout();
			this.panel23.SuspendLayout();
			this.panel48.SuspendLayout();
			this.panel24.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.panel22.SuspendLayout();
			this.panel47.SuspendLayout();
			this.panel25.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panel18.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.panel19.SuspendLayout();
			this.panel50.SuspendLayout();
			this.panel28.SuspendLayout();
			this.panel27.SuspendLayout();
			this.panel26.SuspendLayout();
			this.Recon.SuspendLayout();
			this.panel17.SuspendLayout();
			this.panel16.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.NUPDOWNBackEnd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trckReconSpeed)).BeginInit();
			this.panel15.SuspendLayout();
			this.Config.SuspendLayout();
			this.panel38.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.panel46.SuspendLayout();
			this.panel45.SuspendLayout();
			this.panel44.SuspendLayout();
			this.panel37.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.panel43.SuspendLayout();
			this.groupBox22.SuspendLayout();
			this.panel42.SuspendLayout();
			this.groupBox21.SuspendLayout();
			this.panel41.SuspendLayout();
			this.groupBox20.SuspendLayout();
			this.panel40.SuspendLayout();
			this.groupBox19.SuspendLayout();
			this.panel39.SuspendLayout();
			this.groupBox18.SuspendLayout();
			this.panel33.SuspendLayout();
			this.panel36.SuspendLayout();
			this.groupBox10.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.updownListenPort)).BeginInit();
			this.panel35.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.panel34.SuspendLayout();
			this.grpSepChars.SuspendLayout();
			this.groupBox17.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.updownTimeOut)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabControl1.Controls.Add(this.Proxy);
			this.tabControl1.Controls.Add(this.Misc);
			this.tabControl1.Controls.Add(this.Recon);
			this.tabControl1.Controls.Add(this.Config);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(846, 556);
			this.tabControl1.TabIndex = 0;
			this.toolTipSuru.SetToolTip(this.tabControl1, "Files that end in .TXT in the Fuzz DB directory");
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_Click);
			// 
			// Proxy
			// 
			this.Proxy.BackColor = System.Drawing.Color.DarkGray;
			this.Proxy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Proxy.Controls.Add(this.panel8);
			this.Proxy.Controls.Add(this.splitter1);
			this.Proxy.Controls.Add(this.panel12);
			this.Proxy.Controls.Add(this.splitter3);
			this.Proxy.Controls.Add(this.panel5);
			this.Proxy.Controls.Add(this.panel4);
			this.Proxy.ForeColor = System.Drawing.Color.Black;
			this.Proxy.Location = new System.Drawing.Point(4, 24);
			this.Proxy.Name = "Proxy";
			this.Proxy.Size = new System.Drawing.Size(838, 528);
			this.Proxy.TabIndex = 0;
			this.Proxy.Text = "Proxy";
			// 
			// panel8
			// 
			this.panel8.Controls.Add(this.panel11);
			this.panel8.Controls.Add(this.panel10);
			this.panel8.Controls.Add(this.panel9);
			this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel8.Location = new System.Drawing.Point(0, 158);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(836, 166);
			this.panel8.TabIndex = 49;
			// 
			// panel11
			// 
			this.panel11.Controls.Add(this.txtHTTPdetails);
			this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel11.Location = new System.Drawing.Point(0, 0);
			this.panel11.Name = "panel11";
			this.panel11.Size = new System.Drawing.Size(723, 141);
			this.panel11.TabIndex = 45;
			// 
			// txtHTTPdetails
			// 
			this.txtHTTPdetails.BackColor = System.Drawing.Color.Silver;
			this.txtHTTPdetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtHTTPdetails.DetectUrls = false;
			this.txtHTTPdetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtHTTPdetails.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F);
			this.txtHTTPdetails.ForeColor = System.Drawing.Color.DarkSlateGray;
			this.txtHTTPdetails.Location = new System.Drawing.Point(0, 0);
			this.txtHTTPdetails.Name = "txtHTTPdetails";
			this.txtHTTPdetails.Size = new System.Drawing.Size(723, 141);
			this.txtHTTPdetails.TabIndex = 2;
			this.txtHTTPdetails.Text = "";
			this.toolTipSuru.SetToolTip(this.txtHTTPdetails, "current HTTP(s) raw request");
			this.txtHTTPdetails.WordWrap = false;
			this.txtHTTPdetails.Leave += new System.EventHandler(this.txtHTTPdetails_Leave);
			this.txtHTTPdetails.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHTTPdetails_KeyPress);
			this.txtHTTPdetails.MouseEnter += new System.EventHandler(this.txtHTTPdetails_EnterM);
			this.txtHTTPdetails.MouseLeave += new System.EventHandler(this.txtHTTPdetails_LeaveM);
			this.txtHTTPdetails.Enter += new System.EventHandler(this.txtHTTPdetails_Enter);
			// 
			// panel10
			// 
			this.panel10.Controls.Add(this.label3);
			this.panel10.Controls.Add(this.txtTargetHost);
			this.panel10.Controls.Add(this.label4);
			this.panel10.Controls.Add(this.txtTargetPort);
			this.panel10.Controls.Add(this.chkTargetIsSSL);
			this.panel10.Controls.Add(this.label39);
			this.panel10.Controls.Add(this.lblDateTime);
			this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel10.Location = new System.Drawing.Point(0, 141);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(723, 25);
			this.panel10.TabIndex = 44;
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label3.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(3, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(34, 17);
			this.label3.TabIndex = 17;
			this.label3.Text = "Host";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtTargetHost
			// 
			this.txtTargetHost.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtTargetHost.BackColor = System.Drawing.Color.Snow;
			this.txtTargetHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTargetHost.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtTargetHost.ForeColor = System.Drawing.Color.DarkGreen;
			this.txtTargetHost.Location = new System.Drawing.Point(40, 3);
			this.txtTargetHost.Name = "txtTargetHost";
			this.txtTargetHost.Size = new System.Drawing.Size(284, 18);
			this.txtTargetHost.TabIndex = 3;
			this.txtTargetHost.Text = "";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label4.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(328, 3);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(34, 17);
			this.label4.TabIndex = 20;
			this.label4.Text = "Port";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtTargetPort
			// 
			this.txtTargetPort.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtTargetPort.BackColor = System.Drawing.Color.Snow;
			this.txtTargetPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTargetPort.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtTargetPort.ForeColor = System.Drawing.Color.DarkGreen;
			this.txtTargetPort.Location = new System.Drawing.Point(363, 2);
			this.txtTargetPort.Name = "txtTargetPort";
			this.txtTargetPort.Size = new System.Drawing.Size(44, 18);
			this.txtTargetPort.TabIndex = 4;
			this.txtTargetPort.Text = "80";
			this.txtTargetPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// chkTargetIsSSL
			// 
			this.chkTargetIsSSL.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.chkTargetIsSSL.BackColor = System.Drawing.Color.DarkGray;
			this.chkTargetIsSSL.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkTargetIsSSL.ForeColor = System.Drawing.Color.Black;
			this.chkTargetIsSSL.Location = new System.Drawing.Point(410, 3);
			this.chkTargetIsSSL.Name = "chkTargetIsSSL";
			this.chkTargetIsSSL.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkTargetIsSSL.Size = new System.Drawing.Size(44, 17);
			this.chkTargetIsSSL.TabIndex = 18;
			this.chkTargetIsSSL.Text = "SSL";
			// 
			// label39
			// 
			this.label39.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label39.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label39.Location = new System.Drawing.Point(457, 3);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(50, 17);
			this.label39.TabIndex = 42;
			this.label39.Text = "Req Time";
			this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDateTime
			// 
			this.lblDateTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblDateTime.BackColor = System.Drawing.Color.LightGray;
			this.lblDateTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblDateTime.Location = new System.Drawing.Point(512, 3);
			this.lblDateTime.Name = "lblDateTime";
			this.lblDateTime.Size = new System.Drawing.Size(210, 17);
			this.lblDateTime.TabIndex = 5;
			this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel9
			// 
			this.panel9.BackColor = System.Drawing.Color.LightGray;
			this.panel9.Controls.Add(this.btnClear);
			this.panel9.Controls.Add(this.btnReplay);
			this.panel9.Controls.Add(this.btnSendRawRequest);
			this.panel9.Controls.Add(this.chkProxyAutoUpdate);
			this.panel9.Controls.Add(this.chkImmFuzz);
			this.panel9.Controls.Add(this.button2);
			this.panel9.Controls.Add(this.chkShowHosts);
			this.panel9.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel9.Location = new System.Drawing.Point(723, 0);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(113, 166);
			this.panel9.TabIndex = 43;
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClear.BackColor = System.Drawing.Color.LightGray;
			this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnClear.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnClear.ForeColor = System.Drawing.Color.Brown;
			this.btnClear.Location = new System.Drawing.Point(6, 138);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(104, 27);
			this.btnClear.TabIndex = 12;
			this.btnClear.Text = "Clear cache";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnReplay
			// 
			this.btnReplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReplay.BackColor = System.Drawing.Color.LightGray;
			this.btnReplay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnReplay.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnReplay.ForeColor = System.Drawing.Color.Black;
			this.btnReplay.Location = new System.Drawing.Point(6, 112);
			this.btnReplay.Name = "btnReplay";
			this.btnReplay.Size = new System.Drawing.Size(104, 24);
			this.btnReplay.TabIndex = 11;
			this.btnReplay.Text = "Browse Request";
			this.toolTipSuru.SetToolTip(this.btnReplay, "Send the current request using a browser");
			this.btnReplay.Click += new System.EventHandler(this.btnReplay_Click);
			// 
			// btnSendRawRequest
			// 
			this.btnSendRawRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSendRawRequest.BackColor = System.Drawing.Color.LightGray;
			this.btnSendRawRequest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSendRawRequest.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnSendRawRequest.ForeColor = System.Drawing.Color.Black;
			this.btnSendRawRequest.Location = new System.Drawing.Point(6, 90);
			this.btnSendRawRequest.Name = "btnSendRawRequest";
			this.btnSendRawRequest.Size = new System.Drawing.Size(104, 24);
			this.btnSendRawRequest.TabIndex = 10;
			this.btnSendRawRequest.Text = "Send Raw Request";
			this.toolTipSuru.SetToolTip(this.btnSendRawRequest, "Sends the current request with a raw TCP connection");
			this.btnSendRawRequest.Click += new System.EventHandler(this.btnSendRawRequest_Click);
			// 
			// chkProxyAutoUpdate
			// 
			this.chkProxyAutoUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkProxyAutoUpdate.BackColor = System.Drawing.Color.LightGray;
			this.chkProxyAutoUpdate.Checked = true;
			this.chkProxyAutoUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkProxyAutoUpdate.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkProxyAutoUpdate.ForeColor = System.Drawing.Color.Black;
			this.chkProxyAutoUpdate.Location = new System.Drawing.Point(12, 37);
			this.chkProxyAutoUpdate.Name = "chkProxyAutoUpdate";
			this.chkProxyAutoUpdate.Size = new System.Drawing.Size(96, 16);
			this.chkProxyAutoUpdate.TabIndex = 7;
			this.chkProxyAutoUpdate.Text = "AutoUpdate";
			this.toolTipSuru.SetToolTip(this.chkProxyAutoUpdate, "Updates the upper list box (and request reditor) in real time. Uncheck if playing" +
				" with a single request");
			// 
			// chkImmFuzz
			// 
			this.chkImmFuzz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkImmFuzz.BackColor = System.Drawing.Color.LightGray;
			this.chkImmFuzz.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkImmFuzz.ForeColor = System.Drawing.Color.Black;
			this.chkImmFuzz.Location = new System.Drawing.Point(12, 72);
			this.chkImmFuzz.Name = "chkImmFuzz";
			this.chkImmFuzz.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkImmFuzz.Size = new System.Drawing.Size(96, 14);
			this.chkImmFuzz.TabIndex = 9;
			this.chkImmFuzz.Text = "Fuzz autostart";
			this.toolTipSuru.SetToolTip(this.chkImmFuzz, "Start fuzzing right away after submitting the request from the editor");
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.BackColor = System.Drawing.Color.LightGray;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button2.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button2.ForeColor = System.Drawing.Color.DarkGoldenrod;
			this.button2.Location = new System.Drawing.Point(6, 6);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(104, 28);
			this.button2.TabIndex = 6;
			this.button2.Text = "Update URL list";
			this.toolTipSuru.SetToolTip(this.button2, "Filters request and updates the upper listbox accordingly");
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// chkShowHosts
			// 
			this.chkShowHosts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkShowHosts.BackColor = System.Drawing.Color.LightGray;
			this.chkShowHosts.Checked = true;
			this.chkShowHosts.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowHosts.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkShowHosts.ForeColor = System.Drawing.Color.Black;
			this.chkShowHosts.Location = new System.Drawing.Point(12, 53);
			this.chkShowHosts.Name = "chkShowHosts";
			this.chkShowHosts.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkShowHosts.Size = new System.Drawing.Size(96, 14);
			this.chkShowHosts.TabIndex = 8;
			this.chkShowHosts.Text = "Show hosts";
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.Color.DimGray;
			this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 152);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(836, 6);
			this.splitter1.TabIndex = 52;
			this.splitter1.TabStop = false;
			// 
			// panel12
			// 
			this.panel12.Controls.Add(this.panel14);
			this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel12.Location = new System.Drawing.Point(0, 0);
			this.panel12.Name = "panel12";
			this.panel12.Size = new System.Drawing.Size(836, 152);
			this.panel12.TabIndex = 51;
			// 
			// panel14
			// 
			this.panel14.Controls.Add(this.listView1);
			this.panel14.Controls.Add(this.panel13);
			this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel14.Location = new System.Drawing.Point(0, 0);
			this.panel14.Name = "panel14";
			this.panel14.Size = new System.Drawing.Size(836, 152);
			this.panel14.TabIndex = 15;
			// 
			// listView1
			// 
			this.listView1.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listView1.AutoArrange = false;
			this.listView1.BackColor = System.Drawing.Color.GhostWhite;
			this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listView1.CausesValidation = false;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader1});
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listView1.HideSelection = false;
			this.listView1.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.listView1.LabelWrap = false;
			this.listView1.Location = new System.Drawing.Point(0, 0);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(836, 150);
			this.listView1.TabIndex = 15;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listView1_KeyPress);
			this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
			this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TestItem);
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "";
			this.columnHeader1.Width = 830;
			// 
			// panel13
			// 
			this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel13.Location = new System.Drawing.Point(0, 150);
			this.panel13.Name = "panel13";
			this.panel13.Size = new System.Drawing.Size(836, 2);
			this.panel13.TabIndex = 14;
			// 
			// splitter3
			// 
			this.splitter3.BackColor = System.Drawing.Color.DimGray;
			this.splitter3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter3.Location = new System.Drawing.Point(0, 324);
			this.splitter3.Name = "splitter3";
			this.splitter3.Size = new System.Drawing.Size(836, 6);
			this.splitter3.TabIndex = 48;
			this.splitter3.TabStop = false;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.panel7);
			this.panel5.Controls.Add(this.splitter2);
			this.panel5.Controls.Add(this.panel6);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel5.Location = new System.Drawing.Point(0, 330);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(836, 98);
			this.panel5.TabIndex = 47;
			// 
			// panel7
			// 
			this.panel7.Controls.Add(this.lstCrowResponse);
			this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel7.Location = new System.Drawing.Point(433, 0);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(403, 98);
			this.panel7.TabIndex = 33;
			// 
			// lstCrowResponse
			// 
			this.lstCrowResponse.BackColor = System.Drawing.Color.GhostWhite;
			this.lstCrowResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstCrowResponse.ContextMenu = this.contextMenu2;
			this.lstCrowResponse.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstCrowResponse.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F);
			this.lstCrowResponse.Location = new System.Drawing.Point(0, 0);
			this.lstCrowResponse.Name = "lstCrowResponse";
			this.lstCrowResponse.Size = new System.Drawing.Size(403, 93);
			this.lstCrowResponse.TabIndex = 14;
			this.toolTipSuru.SetToolTip(this.lstCrowResponse, "Format is: Fuzzy Logic Index, Parameter value, Index, Content extracted");
			this.lstCrowResponse.DoubleClick += new System.EventHandler(this.lstCrowResponse_SelectedIndexChanged);
			this.lstCrowResponse.SelectedIndexChanged += new System.EventHandler(this.lstCrowResponse_SelectedIndexChanged_1);
			// 
			// contextMenu2
			// 
			this.contextMenu2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem1,
																						 this.menuItem5,
																						 this.menuItem6,
																						 this.menuItem7,
																						 this.menuItem8});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "Only these";
			this.menuItem1.Click += new System.EventHandler(this.contextEqual);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.Text = "All but these";
			this.menuItem5.Click += new System.EventHandler(this.contextOutside);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 2;
			this.menuItem6.Text = "-";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 3;
			this.menuItem7.Text = "Show reply";
			this.menuItem7.Click += new System.EventHandler(this.contextshowContent);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 4;
			this.menuItem8.Text = "Browse reply";
			this.menuItem8.Click += new System.EventHandler(this.contextbrowseContent);
			// 
			// splitter2
			// 
			this.splitter2.BackColor = System.Drawing.Color.DimGray;
			this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter2.Location = new System.Drawing.Point(427, 0);
			this.splitter2.MinExtra = 0;
			this.splitter2.MinSize = 0;
			this.splitter2.Name = "splitter2";
			this.splitter2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitter2.Size = new System.Drawing.Size(6, 98);
			this.splitter2.TabIndex = 32;
			this.splitter2.TabStop = false;
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.txtCrowResponse);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel6.Location = new System.Drawing.Point(0, 0);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(427, 98);
			this.panel6.TabIndex = 31;
			// 
			// txtCrowResponse
			// 
			this.txtCrowResponse.BackColor = System.Drawing.Color.Gainsboro;
			this.txtCrowResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtCrowResponse.DetectUrls = false;
			this.txtCrowResponse.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCrowResponse.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F);
			this.txtCrowResponse.Location = new System.Drawing.Point(0, 0);
			this.txtCrowResponse.Name = "txtCrowResponse";
			this.txtCrowResponse.Size = new System.Drawing.Size(427, 98);
			this.txtCrowResponse.TabIndex = 13;
			this.txtCrowResponse.Text = "";
			this.toolTipSuru.SetToolTip(this.txtCrowResponse, "HTTP(s) response (during Fuzzing)");
			this.txtCrowResponse.WordWrap = false;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.groupBox4);
			this.panel4.Controls.Add(this.grpInner);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new System.Drawing.Point(0, 428);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(836, 98);
			this.panel4.TabIndex = 45;
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.BackColor = System.Drawing.Color.DarkGray;
			this.groupBox4.Controls.Add(this.chkCrowStoreResponse);
			this.groupBox4.Controls.Add(this.prgBarCrow);
			this.groupBox4.Controls.Add(this.label42);
			this.groupBox4.Controls.Add(this.updownGroupTolerance);
			this.groupBox4.Controls.Add(this.btnCrowGroup);
			this.groupBox4.Controls.Add(this.label11);
			this.groupBox4.Controls.Add(this.label12);
			this.groupBox4.Controls.Add(this.radioNotEqual);
			this.groupBox4.Controls.Add(this.radioAll);
			this.groupBox4.Controls.Add(this.btnExport);
			this.groupBox4.Controls.Add(this.updowntwo);
			this.groupBox4.Controls.Add(this.radioequal);
			this.groupBox4.Controls.Add(this.radiooutside);
			this.groupBox4.Controls.Add(this.radioinside);
			this.groupBox4.Controls.Add(this.btnReCalc);
			this.groupBox4.Controls.Add(this.updownCrowAI);
			this.groupBox4.Controls.Add(this.chkUseAIAtAll);
			this.groupBox4.Controls.Add(this.txtContentEndWord);
			this.groupBox4.Controls.Add(this.txtContentStartWord);
			this.groupBox4.Controls.Add(this.label15);
			this.groupBox4.Controls.Add(this.label14);
			this.groupBox4.ForeColor = System.Drawing.Color.Black;
			this.groupBox4.Location = new System.Drawing.Point(426, 0);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.groupBox4.Size = new System.Drawing.Size(401, 98);
			this.groupBox4.TabIndex = 44;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "| Fuzzy Logic Trigger |";
			// 
			// chkCrowStoreResponse
			// 
			this.chkCrowStoreResponse.BackColor = System.Drawing.Color.Transparent;
			this.chkCrowStoreResponse.ForeColor = System.Drawing.Color.Black;
			this.chkCrowStoreResponse.Location = new System.Drawing.Point(344, 36);
			this.chkCrowStoreResponse.Name = "chkCrowStoreResponse";
			this.chkCrowStoreResponse.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkCrowStoreResponse.Size = new System.Drawing.Size(52, 16);
			this.chkCrowStoreResponse.TabIndex = 41;
			this.chkCrowStoreResponse.Text = "!Store";
			this.toolTipSuru.SetToolTip(this.chkCrowStoreResponse, "Do not store the ouput or content extraction - this is useful when brute forcing " +
				"with large collections as it does not take up memory");
			// 
			// prgBarCrow
			// 
			this.prgBarCrow.Location = new System.Drawing.Point(8, 16);
			this.prgBarCrow.Name = "prgBarCrow";
			this.prgBarCrow.Size = new System.Drawing.Size(184, 12);
			this.prgBarCrow.TabIndex = 27;
			// 
			// label42
			// 
			this.label42.BackColor = System.Drawing.Color.DarkGray;
			this.label42.ForeColor = System.Drawing.Color.Black;
			this.label42.Location = new System.Drawing.Point(344, 58);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(50, 16);
			this.label42.TabIndex = 48;
			this.label42.Text = "Tolerance";
			// 
			// updownGroupTolerance
			// 
			this.updownGroupTolerance.BackColor = System.Drawing.Color.SeaShell;
			this.updownGroupTolerance.DecimalPlaces = 3;
			this.updownGroupTolerance.ForeColor = System.Drawing.Color.Black;
			this.updownGroupTolerance.Increment = new System.Decimal(new int[] {
																				   1,
																				   0,
																				   0,
																				   131072});
			this.updownGroupTolerance.Location = new System.Drawing.Point(344, 76);
			this.updownGroupTolerance.Maximum = new System.Decimal(new int[] {
																				 10,
																				 0,
																				 0,
																				 0});
			this.updownGroupTolerance.Name = "updownGroupTolerance";
			this.updownGroupTolerance.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.updownGroupTolerance.Size = new System.Drawing.Size(52, 18);
			this.updownGroupTolerance.TabIndex = 42;
			this.updownGroupTolerance.Value = new System.Decimal(new int[] {
																			   2,
																			   0,
																			   0,
																			   131072});
			// 
			// btnCrowGroup
			// 
			this.btnCrowGroup.BackColor = System.Drawing.Color.DarkGray;
			this.btnCrowGroup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCrowGroup.ForeColor = System.Drawing.Color.Black;
			this.btnCrowGroup.Location = new System.Drawing.Point(256, 70);
			this.btnCrowGroup.Name = "btnCrowGroup";
			this.btnCrowGroup.Size = new System.Drawing.Size(84, 22);
			this.btnCrowGroup.TabIndex = 39;
			this.btnCrowGroup.Text = "Auto Group";
			this.toolTipSuru.SetToolTip(this.btnCrowGroup, "Start Auto Grouper for responses");
			this.btnCrowGroup.Click += new System.EventHandler(this.btnCrowGroup_Click);
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.DarkGray;
			this.label11.ForeColor = System.Drawing.Color.Black;
			this.label11.Location = new System.Drawing.Point(64, 34);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(24, 16);
			this.label11.TabIndex = 37;
			this.label11.Text = "Low";
			// 
			// label12
			// 
			this.label12.BackColor = System.Drawing.Color.DarkGray;
			this.label12.ForeColor = System.Drawing.Color.Black;
			this.label12.Location = new System.Drawing.Point(166, 34);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(28, 16);
			this.label12.TabIndex = 36;
			this.label12.Text = "High";
			// 
			// radioNotEqual
			// 
			this.radioNotEqual.BackColor = System.Drawing.Color.Transparent;
			this.radioNotEqual.ForeColor = System.Drawing.Color.Black;
			this.radioNotEqual.Location = new System.Drawing.Point(206, 30);
			this.radioNotEqual.Name = "radioNotEqual";
			this.radioNotEqual.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.radioNotEqual.Size = new System.Drawing.Size(36, 16);
			this.radioNotEqual.TabIndex = 33;
			this.radioNotEqual.Text = "!=";
			// 
			// radioAll
			// 
			this.radioAll.BackColor = System.Drawing.Color.Transparent;
			this.radioAll.Checked = true;
			this.radioAll.ForeColor = System.Drawing.Color.Cornsilk;
			this.radioAll.Location = new System.Drawing.Point(206, 14);
			this.radioAll.Name = "radioAll";
			this.radioAll.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.radioAll.Size = new System.Drawing.Size(36, 16);
			this.radioAll.TabIndex = 32;
			this.radioAll.TabStop = true;
			this.radioAll.Text = "All";
			// 
			// btnExport
			// 
			this.btnExport.BackColor = System.Drawing.Color.DarkGray;
			this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnExport.ForeColor = System.Drawing.Color.Black;
			this.btnExport.Location = new System.Drawing.Point(256, 46);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(84, 22);
			this.btnExport.TabIndex = 38;
			this.btnExport.Text = "Export";
			this.toolTipSuru.SetToolTip(this.btnExport, "Export results");
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// updowntwo
			// 
			this.updowntwo.BackColor = System.Drawing.Color.SeaShell;
			this.updowntwo.DecimalPlaces = 3;
			this.updowntwo.ForeColor = System.Drawing.Color.Black;
			this.updowntwo.Increment = new System.Decimal(new int[] {
																		5,
																		0,
																		0,
																		131072});
			this.updowntwo.Location = new System.Drawing.Point(108, 32);
			this.updowntwo.Maximum = new System.Decimal(new int[] {
																	  10000,
																	  0,
																	  0,
																	  0});
			this.updowntwo.Name = "updowntwo";
			this.updowntwo.Size = new System.Drawing.Size(56, 18);
			this.updowntwo.TabIndex = 29;
			this.updowntwo.Value = new System.Decimal(new int[] {
																	5,
																	0,
																	0,
																	65536});
			// 
			// radioequal
			// 
			this.radioequal.BackColor = System.Drawing.Color.Transparent;
			this.radioequal.ForeColor = System.Drawing.Color.Black;
			this.radioequal.Location = new System.Drawing.Point(206, 46);
			this.radioequal.Name = "radioequal";
			this.radioequal.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.radioequal.Size = new System.Drawing.Size(36, 16);
			this.radioequal.TabIndex = 34;
			this.radioequal.Text = "=";
			// 
			// radiooutside
			// 
			this.radiooutside.BackColor = System.Drawing.Color.Transparent;
			this.radiooutside.ForeColor = System.Drawing.Color.Black;
			this.radiooutside.Location = new System.Drawing.Point(206, 62);
			this.radiooutside.Name = "radiooutside";
			this.radiooutside.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.radiooutside.Size = new System.Drawing.Size(40, 16);
			this.radiooutside.TabIndex = 35;
			this.radiooutside.Text = "><";
			// 
			// radioinside
			// 
			this.radioinside.BackColor = System.Drawing.Color.Transparent;
			this.radioinside.ForeColor = System.Drawing.Color.Black;
			this.radioinside.Location = new System.Drawing.Point(206, 78);
			this.radioinside.Name = "radioinside";
			this.radioinside.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.radioinside.Size = new System.Drawing.Size(38, 16);
			this.radioinside.TabIndex = 36;
			this.radioinside.Text = "<>";
			// 
			// btnReCalc
			// 
			this.btnReCalc.BackColor = System.Drawing.Color.DarkGray;
			this.btnReCalc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnReCalc.ForeColor = System.Drawing.Color.Black;
			this.btnReCalc.Location = new System.Drawing.Point(260, 20);
			this.btnReCalc.Name = "btnReCalc";
			this.btnReCalc.Size = new System.Drawing.Size(84, 24);
			this.btnReCalc.TabIndex = 37;
			this.btnReCalc.Text = "Recalculate";
			this.toolTipSuru.SetToolTip(this.btnReCalc, "Apply Fuzzy Logic Index control");
			this.btnReCalc.Click += new System.EventHandler(this.btnReCalc_Click_1);
			// 
			// updownCrowAI
			// 
			this.updownCrowAI.BackColor = System.Drawing.Color.SeaShell;
			this.updownCrowAI.DecimalPlaces = 3;
			this.updownCrowAI.ForeColor = System.Drawing.Color.Black;
			this.updownCrowAI.Increment = new System.Decimal(new int[] {
																		   5,
																		   0,
																		   0,
																		   131072});
			this.updownCrowAI.Location = new System.Drawing.Point(8, 32);
			this.updownCrowAI.Maximum = new System.Decimal(new int[] {
																		 10000,
																		 0,
																		 0,
																		 0});
			this.updownCrowAI.Name = "updownCrowAI";
			this.updownCrowAI.Size = new System.Drawing.Size(56, 18);
			this.updownCrowAI.TabIndex = 28;
			// 
			// chkUseAIAtAll
			// 
			this.chkUseAIAtAll.BackColor = System.Drawing.Color.Transparent;
			this.chkUseAIAtAll.ForeColor = System.Drawing.Color.Black;
			this.chkUseAIAtAll.Location = new System.Drawing.Point(344, 20);
			this.chkUseAIAtAll.Name = "chkUseAIAtAll";
			this.chkUseAIAtAll.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkUseAIAtAll.Size = new System.Drawing.Size(48, 16);
			this.chkUseAIAtAll.TabIndex = 40;
			this.chkUseAIAtAll.Text = "! FLT";
			this.toolTipSuru.SetToolTip(this.chkUseAIAtAll, "Do not perform Fuzzy Logic checking - for content extraction only");
			// 
			// txtContentEndWord
			// 
			this.txtContentEndWord.BackColor = System.Drawing.Color.SeaShell;
			this.txtContentEndWord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtContentEndWord.ForeColor = System.Drawing.Color.Black;
			this.txtContentEndWord.Location = new System.Drawing.Point(8, 74);
			this.txtContentEndWord.Name = "txtContentEndWord";
			this.txtContentEndWord.Size = new System.Drawing.Size(158, 18);
			this.txtContentEndWord.TabIndex = 31;
			this.txtContentEndWord.Text = "</title>";
			this.txtContentEndWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.toolTipSuru.SetToolTip(this.txtContentEndWord, "End token for content extraction");
			// 
			// txtContentStartWord
			// 
			this.txtContentStartWord.BackColor = System.Drawing.Color.SeaShell;
			this.txtContentStartWord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtContentStartWord.ForeColor = System.Drawing.Color.Black;
			this.txtContentStartWord.Location = new System.Drawing.Point(8, 54);
			this.txtContentStartWord.Name = "txtContentStartWord";
			this.txtContentStartWord.Size = new System.Drawing.Size(158, 18);
			this.txtContentStartWord.TabIndex = 30;
			this.txtContentStartWord.Text = "<title>";
			this.txtContentStartWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.toolTipSuru.SetToolTip(this.txtContentStartWord, "Start token for content extraction");
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(168, 76);
			this.label15.Name = "label15";
			this.label15.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label15.Size = new System.Drawing.Size(28, 12);
			this.label15.TabIndex = 45;
			this.label15.Text = "Stop";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(168, 56);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(28, 12);
			this.label14.TabIndex = 44;
			this.label14.Text = "Start";
			// 
			// grpInner
			// 
			this.grpInner.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.grpInner.BackColor = System.Drawing.Color.DarkGray;
			this.grpInner.Controls.Add(this.comboCrowEncode);
			this.grpInner.Controls.Add(this.btnManualBaseResponse);
			this.grpInner.Controls.Add(this.cmbCustom);
			this.grpInner.Controls.Add(this.label8);
			this.grpInner.Controls.Add(this.label7);
			this.grpInner.Controls.Add(this.lblFile1);
			this.grpInner.Controls.Add(this.btnFileLoad1);
			this.grpInner.Controls.Add(this.radioFile1);
			this.grpInner.Controls.Add(this.radioNumeric1);
			this.grpInner.Controls.Add(this.txtNumericTo1);
			this.grpInner.Controls.Add(this.txtNumericFrom1);
			this.grpInner.Controls.Add(this.btnCrowStart);
			this.grpInner.Controls.Add(this.btnCrowPause);
			this.grpInner.Controls.Add(this.btnCrowStop);
			this.grpInner.ForeColor = System.Drawing.Color.Black;
			this.grpInner.Location = new System.Drawing.Point(0, 0);
			this.grpInner.Name = "grpInner";
			this.grpInner.Size = new System.Drawing.Size(427, 98);
			this.grpInner.TabIndex = 43;
			this.grpInner.TabStop = false;
			this.grpInner.Text = "| Parameter control |";
			// 
			// comboCrowEncode
			// 
			this.comboCrowEncode.BackColor = System.Drawing.Color.Snow;
			this.comboCrowEncode.Items.AddRange(new object[] {
																 "No encoding",
																 "Hex",
																 "SHA1",
																 "MD5",
																 "B64e",
																 "B64d",
																 "ToUpper",
																 "ToLower"});
			this.comboCrowEncode.Location = new System.Drawing.Point(316, 18);
			this.comboCrowEncode.Name = "comboCrowEncode";
			this.comboCrowEncode.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.comboCrowEncode.Size = new System.Drawing.Size(108, 20);
			this.comboCrowEncode.TabIndex = 18;
			this.comboCrowEncode.Text = "No encoding";
			this.toolTipSuru.SetToolTip(this.comboCrowEncode, "Selects the type of encoding on the parameter - can be SHA1, MD5, Hex, Base64 enc" +
				"ode or Base64 decode");
			// 
			// btnManualBaseResponse
			// 
			this.btnManualBaseResponse.BackColor = System.Drawing.Color.DarkGray;
			this.btnManualBaseResponse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnManualBaseResponse.ForeColor = System.Drawing.Color.SteelBlue;
			this.btnManualBaseResponse.Location = new System.Drawing.Point(218, 70);
			this.btnManualBaseResponse.Name = "btnManualBaseResponse";
			this.btnManualBaseResponse.Size = new System.Drawing.Size(90, 23);
			this.btnManualBaseResponse.TabIndex = 23;
			this.btnManualBaseResponse.Text = "BaseResponse";
			this.toolTipSuru.SetToolTip(this.btnManualBaseResponse, "Manually create a base response");
			this.btnManualBaseResponse.Click += new System.EventHandler(this.btnManualBaseResponse_Click);
			// 
			// cmbCustom
			// 
			this.cmbCustom.BackColor = System.Drawing.Color.Snow;
			this.cmbCustom.Location = new System.Drawing.Point(6, 70);
			this.cmbCustom.Name = "cmbCustom";
			this.cmbCustom.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmbCustom.Size = new System.Drawing.Size(208, 20);
			this.cmbCustom.TabIndex = 22;
			this.cmbCustom.Text = "Fuzz string library quick nav";
			this.cmbCustom.SelectedIndexChanged += new System.EventHandler(this.cmbCustom_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(204, 20);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(21, 16);
			this.label8.TabIndex = 38;
			this.label8.Text = "To";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(168, 20);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(32, 16);
			this.label7.TabIndex = 37;
			this.label7.Text = "From";
			// 
			// lblFile1
			// 
			this.lblFile1.BackColor = System.Drawing.Color.Snow;
			this.lblFile1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblFile1.ForeColor = System.Drawing.Color.Black;
			this.lblFile1.Location = new System.Drawing.Point(122, 40);
			this.lblFile1.Name = "lblFile1";
			this.lblFile1.Size = new System.Drawing.Size(188, 26);
			this.lblFile1.TabIndex = 21;
			this.lblFile1.Text = "None";
			// 
			// btnFileLoad1
			// 
			this.btnFileLoad1.BackColor = System.Drawing.Color.DarkGray;
			this.btnFileLoad1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnFileLoad1.ForeColor = System.Drawing.Color.Black;
			this.btnFileLoad1.Location = new System.Drawing.Point(46, 39);
			this.btnFileLoad1.Name = "btnFileLoad1";
			this.btnFileLoad1.Size = new System.Drawing.Size(78, 27);
			this.btnFileLoad1.TabIndex = 20;
			this.btnFileLoad1.Text = "FileLocate";
			this.toolTipSuru.SetToolTip(this.btnFileLoad1, "Click to read a file that is not in the Fuzz DB directory");
			this.btnFileLoad1.Click += new System.EventHandler(this.btnFileLoad1_Click);
			// 
			// radioFile1
			// 
			this.radioFile1.BackColor = System.Drawing.Color.DarkGray;
			this.radioFile1.ForeColor = System.Drawing.Color.Black;
			this.radioFile1.Location = new System.Drawing.Point(8, 39);
			this.radioFile1.Name = "radioFile1";
			this.radioFile1.Size = new System.Drawing.Size(64, 24);
			this.radioFile1.TabIndex = 19;
			this.radioFile1.Text = "File";
			// 
			// radioNumeric1
			// 
			this.radioNumeric1.BackColor = System.Drawing.Color.DarkGray;
			this.radioNumeric1.Checked = true;
			this.radioNumeric1.ForeColor = System.Drawing.Color.Black;
			this.radioNumeric1.Location = new System.Drawing.Point(8, 16);
			this.radioNumeric1.Name = "radioNumeric1";
			this.radioNumeric1.Size = new System.Drawing.Size(62, 24);
			this.radioNumeric1.TabIndex = 15;
			this.radioNumeric1.TabStop = true;
			this.radioNumeric1.Text = "Numeric";
			// 
			// txtNumericTo1
			// 
			this.txtNumericTo1.BackColor = System.Drawing.Color.Snow;
			this.txtNumericTo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtNumericTo1.ForeColor = System.Drawing.Color.Black;
			this.txtNumericTo1.Location = new System.Drawing.Point(226, 18);
			this.txtNumericTo1.Name = "txtNumericTo1";
			this.txtNumericTo1.Size = new System.Drawing.Size(84, 18);
			this.txtNumericTo1.TabIndex = 17;
			this.txtNumericTo1.Text = "999";
			this.txtNumericTo1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txtNumericFrom1
			// 
			this.txtNumericFrom1.BackColor = System.Drawing.Color.Snow;
			this.txtNumericFrom1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtNumericFrom1.ForeColor = System.Drawing.Color.Black;
			this.txtNumericFrom1.Location = new System.Drawing.Point(72, 18);
			this.txtNumericFrom1.Name = "txtNumericFrom1";
			this.txtNumericFrom1.Size = new System.Drawing.Size(95, 18);
			this.txtNumericFrom1.TabIndex = 16;
			this.txtNumericFrom1.Text = "000";
			this.txtNumericFrom1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// btnCrowStart
			// 
			this.btnCrowStart.BackColor = System.Drawing.Color.DarkGray;
			this.btnCrowStart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCrowStart.ForeColor = System.Drawing.Color.ForestGreen;
			this.btnCrowStart.Location = new System.Drawing.Point(318, 44);
			this.btnCrowStart.Name = "btnCrowStart";
			this.btnCrowStart.Size = new System.Drawing.Size(104, 23);
			this.btnCrowStart.TabIndex = 24;
			this.btnCrowStart.Text = "Start";
			this.toolTipSuru.SetToolTip(this.btnCrowStart, "Start fuzzing - you need to mark the request with FUZZCTRL");
			this.btnCrowStart.Click += new System.EventHandler(this.btnCrowStart_Click);
			// 
			// btnCrowPause
			// 
			this.btnCrowPause.BackColor = System.Drawing.Color.DarkGray;
			this.btnCrowPause.Enabled = false;
			this.btnCrowPause.ForeColor = System.Drawing.Color.DarkGoldenrod;
			this.btnCrowPause.Location = new System.Drawing.Point(312, 72);
			this.btnCrowPause.Name = "btnCrowPause";
			this.btnCrowPause.Size = new System.Drawing.Size(58, 23);
			this.btnCrowPause.TabIndex = 25;
			this.btnCrowPause.Text = "Pause";
			this.btnCrowPause.Click += new System.EventHandler(this.btnCrowPause_Click);
			// 
			// btnCrowStop
			// 
			this.btnCrowStop.BackColor = System.Drawing.Color.DarkGray;
			this.btnCrowStop.Enabled = false;
			this.btnCrowStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCrowStop.ForeColor = System.Drawing.Color.Brown;
			this.btnCrowStop.Location = new System.Drawing.Point(368, 72);
			this.btnCrowStop.Name = "btnCrowStop";
			this.btnCrowStop.Size = new System.Drawing.Size(56, 23);
			this.btnCrowStop.TabIndex = 26;
			this.btnCrowStop.Text = "Stop";
			this.btnCrowStop.Click += new System.EventHandler(this.btnCrowStop_Click);
			// 
			// Misc
			// 
			this.Misc.BackColor = System.Drawing.Color.DarkGray;
			this.Misc.Controls.Add(this.panel20);
			this.Misc.Controls.Add(this.splitter7);
			this.Misc.Controls.Add(this.panel21);
			this.Misc.Controls.Add(this.splitter5);
			this.Misc.Controls.Add(this.panel18);
			this.Misc.Location = new System.Drawing.Point(4, 24);
			this.Misc.Name = "Misc";
			this.Misc.Size = new System.Drawing.Size(838, 528);
			this.Misc.TabIndex = 3;
			this.Misc.Text = "Misc";
			// 
			// panel20
			// 
			this.panel20.Controls.Add(this.groupBox5);
			this.panel20.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel20.Location = new System.Drawing.Point(0, 179);
			this.panel20.Name = "panel20";
			this.panel20.Size = new System.Drawing.Size(838, 189);
			this.panel20.TabIndex = 67;
			// 
			// groupBox5
			// 
			this.groupBox5.BackColor = System.Drawing.Color.Gray;
			this.groupBox5.Controls.Add(this.panel30);
			this.groupBox5.Controls.Add(this.panel49);
			this.groupBox5.Controls.Add(this.splitter16);
			this.groupBox5.Controls.Add(this.panel29);
			this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox5.Location = new System.Drawing.Point(0, 0);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(838, 189);
			this.groupBox5.TabIndex = 60;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "| Tools |";
			this.toolTipSuru.SetToolTip(this.groupBox5, "Click <> button to perform basic crypto/encoding translations");
			// 
			// panel30
			// 
			this.panel30.Controls.Add(this.panel32);
			this.panel30.Controls.Add(this.splitter8);
			this.panel30.Controls.Add(this.panel31);
			this.panel30.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel30.Location = new System.Drawing.Point(435, 14);
			this.panel30.Name = "panel30";
			this.panel30.Size = new System.Drawing.Size(400, 172);
			this.panel30.TabIndex = 58;
			// 
			// panel32
			// 
			this.panel32.Controls.Add(this.groupBox12);
			this.panel32.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel32.Location = new System.Drawing.Point(0, 0);
			this.panel32.Name = "panel32";
			this.panel32.Size = new System.Drawing.Size(400, 74);
			this.panel32.TabIndex = 58;
			// 
			// groupBox12
			// 
			this.groupBox12.Controls.Add(this.txtToolsBase64Encoded);
			this.groupBox12.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox12.Location = new System.Drawing.Point(0, 0);
			this.groupBox12.Name = "groupBox12";
			this.groupBox12.Size = new System.Drawing.Size(400, 74);
			this.groupBox12.TabIndex = 55;
			this.groupBox12.TabStop = false;
			this.groupBox12.Text = "Base64 Encode";
			// 
			// txtToolsBase64Encoded
			// 
			this.txtToolsBase64Encoded.BackColor = System.Drawing.Color.Gainsboro;
			this.txtToolsBase64Encoded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtToolsBase64Encoded.DetectUrls = false;
			this.txtToolsBase64Encoded.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtToolsBase64Encoded.Font = new System.Drawing.Font("Courier New", 8.25F);
			this.txtToolsBase64Encoded.Location = new System.Drawing.Point(3, 14);
			this.txtToolsBase64Encoded.Name = "txtToolsBase64Encoded";
			this.txtToolsBase64Encoded.Size = new System.Drawing.Size(394, 57);
			this.txtToolsBase64Encoded.TabIndex = 16;
			this.txtToolsBase64Encoded.Text = "";
			// 
			// splitter8
			// 
			this.splitter8.BackColor = System.Drawing.Color.DimGray;
			this.splitter8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter8.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter8.Location = new System.Drawing.Point(0, 74);
			this.splitter8.Name = "splitter8";
			this.splitter8.Size = new System.Drawing.Size(400, 6);
			this.splitter8.TabIndex = 57;
			this.splitter8.TabStop = false;
			// 
			// panel31
			// 
			this.panel31.Controls.Add(this.groupBox11);
			this.panel31.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel31.Location = new System.Drawing.Point(0, 80);
			this.panel31.Name = "panel31";
			this.panel31.Size = new System.Drawing.Size(400, 92);
			this.panel31.TabIndex = 56;
			// 
			// groupBox11
			// 
			this.groupBox11.Controls.Add(this.txtToolsBase64Decoded);
			this.groupBox11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox11.Location = new System.Drawing.Point(0, 0);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(400, 92);
			this.groupBox11.TabIndex = 54;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "Base64 Decode";
			// 
			// txtToolsBase64Decoded
			// 
			this.txtToolsBase64Decoded.BackColor = System.Drawing.Color.Gainsboro;
			this.txtToolsBase64Decoded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtToolsBase64Decoded.DetectUrls = false;
			this.txtToolsBase64Decoded.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtToolsBase64Decoded.Font = new System.Drawing.Font("Courier New", 8.25F);
			this.txtToolsBase64Decoded.Location = new System.Drawing.Point(3, 14);
			this.txtToolsBase64Decoded.Name = "txtToolsBase64Decoded";
			this.txtToolsBase64Decoded.Size = new System.Drawing.Size(394, 75);
			this.txtToolsBase64Decoded.TabIndex = 17;
			this.txtToolsBase64Decoded.Text = "";
			// 
			// panel49
			// 
			this.panel49.Controls.Add(this.btnToolsGo);
			this.panel49.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel49.Location = new System.Drawing.Point(412, 14);
			this.panel49.Name = "panel49";
			this.panel49.Size = new System.Drawing.Size(23, 172);
			this.panel49.TabIndex = 60;
			// 
			// btnToolsGo
			// 
			this.btnToolsGo.BackColor = System.Drawing.Color.Gray;
			this.btnToolsGo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnToolsGo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnToolsGo.Location = new System.Drawing.Point(0, 0);
			this.btnToolsGo.Name = "btnToolsGo";
			this.btnToolsGo.Size = new System.Drawing.Size(23, 172);
			this.btnToolsGo.TabIndex = 15;
			this.btnToolsGo.Text = "<>";
			this.btnToolsGo.Click += new System.EventHandler(this.btnToolsGo_Click);
			// 
			// splitter16
			// 
			this.splitter16.BackColor = System.Drawing.Color.DimGray;
			this.splitter16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter16.Location = new System.Drawing.Point(406, 14);
			this.splitter16.Name = "splitter16";
			this.splitter16.Size = new System.Drawing.Size(6, 172);
			this.splitter16.TabIndex = 59;
			this.splitter16.TabStop = false;
			// 
			// panel29
			// 
			this.panel29.Controls.Add(this.groupBox15);
			this.panel29.Controls.Add(this.groupBox13);
			this.panel29.Controls.Add(this.groupBox14);
			this.panel29.Controls.Add(this.groupBox16);
			this.panel29.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel29.Location = new System.Drawing.Point(3, 14);
			this.panel29.Name = "panel29";
			this.panel29.Size = new System.Drawing.Size(403, 172);
			this.panel29.TabIndex = 57;
			// 
			// groupBox15
			// 
			this.groupBox15.Controls.Add(this.txtToolsMD5Sum);
			this.groupBox15.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox15.Location = new System.Drawing.Point(0, 65);
			this.groupBox15.Name = "groupBox15";
			this.groupBox15.Size = new System.Drawing.Size(403, 36);
			this.groupBox15.TabIndex = 59;
			this.groupBox15.TabStop = false;
			this.groupBox15.Text = "MD5";
			// 
			// txtToolsMD5Sum
			// 
			this.txtToolsMD5Sum.BackColor = System.Drawing.Color.Gainsboro;
			this.txtToolsMD5Sum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtToolsMD5Sum.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtToolsMD5Sum.Font = new System.Drawing.Font("Courier New", 8.25F);
			this.txtToolsMD5Sum.Location = new System.Drawing.Point(3, 14);
			this.txtToolsMD5Sum.Name = "txtToolsMD5Sum";
			this.txtToolsMD5Sum.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.txtToolsMD5Sum.Size = new System.Drawing.Size(397, 19);
			this.txtToolsMD5Sum.TabIndex = 12;
			this.txtToolsMD5Sum.Text = "";
			// 
			// groupBox13
			// 
			this.groupBox13.BackColor = System.Drawing.Color.Transparent;
			this.groupBox13.Controls.Add(this.txtToolsHex);
			this.groupBox13.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox13.Location = new System.Drawing.Point(0, 101);
			this.groupBox13.Name = "groupBox13";
			this.groupBox13.Size = new System.Drawing.Size(403, 35);
			this.groupBox13.TabIndex = 57;
			this.groupBox13.TabStop = false;
			this.groupBox13.Text = "HEX";
			// 
			// txtToolsHex
			// 
			this.txtToolsHex.BackColor = System.Drawing.Color.Gainsboro;
			this.txtToolsHex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtToolsHex.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtToolsHex.Font = new System.Drawing.Font("Courier New", 8.25F);
			this.txtToolsHex.Location = new System.Drawing.Point(3, 14);
			this.txtToolsHex.Name = "txtToolsHex";
			this.txtToolsHex.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.txtToolsHex.Size = new System.Drawing.Size(397, 18);
			this.txtToolsHex.TabIndex = 13;
			this.txtToolsHex.Text = "";
			// 
			// groupBox14
			// 
			this.groupBox14.Controls.Add(this.txtToolsSHA1);
			this.groupBox14.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox14.Location = new System.Drawing.Point(0, 136);
			this.groupBox14.Name = "groupBox14";
			this.groupBox14.Size = new System.Drawing.Size(403, 36);
			this.groupBox14.TabIndex = 58;
			this.groupBox14.TabStop = false;
			this.groupBox14.Text = "SHA1";
			// 
			// txtToolsSHA1
			// 
			this.txtToolsSHA1.BackColor = System.Drawing.Color.Gainsboro;
			this.txtToolsSHA1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtToolsSHA1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtToolsSHA1.Font = new System.Drawing.Font("Courier New", 8.25F);
			this.txtToolsSHA1.Location = new System.Drawing.Point(3, 14);
			this.txtToolsSHA1.Name = "txtToolsSHA1";
			this.txtToolsSHA1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.txtToolsSHA1.Size = new System.Drawing.Size(397, 19);
			this.txtToolsSHA1.TabIndex = 14;
			this.txtToolsSHA1.Text = "";
			// 
			// groupBox16
			// 
			this.groupBox16.Controls.Add(this.txtToolsUuserInput);
			this.groupBox16.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox16.Location = new System.Drawing.Point(0, 0);
			this.groupBox16.Name = "groupBox16";
			this.groupBox16.Size = new System.Drawing.Size(403, 172);
			this.groupBox16.TabIndex = 60;
			this.groupBox16.TabStop = false;
			this.groupBox16.Text = "User Input";
			// 
			// txtToolsUuserInput
			// 
			this.txtToolsUuserInput.BackColor = System.Drawing.Color.Snow;
			this.txtToolsUuserInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtToolsUuserInput.DetectUrls = false;
			this.txtToolsUuserInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtToolsUuserInput.Font = new System.Drawing.Font("Courier New", 8.25F);
			this.txtToolsUuserInput.Location = new System.Drawing.Point(3, 14);
			this.txtToolsUuserInput.Name = "txtToolsUuserInput";
			this.txtToolsUuserInput.Size = new System.Drawing.Size(397, 155);
			this.txtToolsUuserInput.TabIndex = 11;
			this.txtToolsUuserInput.Text = "SensePost";
			this.txtToolsUuserInput.TextChanged += new System.EventHandler(this.txtToolsUuserInput_KeyPress);
			// 
			// splitter7
			// 
			this.splitter7.BackColor = System.Drawing.Color.DimGray;
			this.splitter7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter7.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter7.Location = new System.Drawing.Point(0, 173);
			this.splitter7.Name = "splitter7";
			this.splitter7.Size = new System.Drawing.Size(838, 6);
			this.splitter7.TabIndex = 69;
			this.splitter7.TabStop = false;
			// 
			// panel21
			// 
			this.panel21.Controls.Add(this.panel23);
			this.panel21.Controls.Add(this.splitter6);
			this.panel21.Controls.Add(this.panel22);
			this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel21.Location = new System.Drawing.Point(0, 0);
			this.panel21.Name = "panel21";
			this.panel21.Size = new System.Drawing.Size(838, 173);
			this.panel21.TabIndex = 68;
			// 
			// panel23
			// 
			this.panel23.Controls.Add(this.panel48);
			this.panel23.Controls.Add(this.panel24);
			this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel23.Location = new System.Drawing.Point(423, 0);
			this.panel23.Name = "panel23";
			this.panel23.Size = new System.Drawing.Size(415, 173);
			this.panel23.TabIndex = 62;
			// 
			// panel48
			// 
			this.panel48.Controls.Add(this.lstSRIncoming);
			this.panel48.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel48.Location = new System.Drawing.Point(0, 88);
			this.panel48.Name = "panel48";
			this.panel48.Size = new System.Drawing.Size(415, 85);
			this.panel48.TabIndex = 61;
			// 
			// lstSRIncoming
			// 
			this.lstSRIncoming.BackColor = System.Drawing.Color.Snow;
			this.lstSRIncoming.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstSRIncoming.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstSRIncoming.ItemHeight = 12;
			this.lstSRIncoming.Location = new System.Drawing.Point(0, 0);
			this.lstSRIncoming.Name = "lstSRIncoming";
			this.lstSRIncoming.Size = new System.Drawing.Size(415, 74);
			this.lstSRIncoming.TabIndex = 10;
			this.lstSRIncoming.SelectedIndexChanged += new System.EventHandler(this.lstSRIncoming_SelectedIndexChanged);
			// 
			// panel24
			// 
			this.panel24.Controls.Add(this.groupBox3);
			this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel24.Location = new System.Drawing.Point(0, 0);
			this.panel24.Name = "panel24";
			this.panel24.Size = new System.Drawing.Size(415, 88);
			this.panel24.TabIndex = 60;
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.Color.DarkGray;
			this.groupBox3.Controls.Add(this.label19);
			this.groupBox3.Controls.Add(this.btnSRDeleteOut);
			this.groupBox3.Controls.Add(this.btnSRAddOut);
			this.groupBox3.Controls.Add(this.label18);
			this.groupBox3.Controls.Add(this.txtSRReplaceIn);
			this.groupBox3.Controls.Add(this.txtSRSearchIn);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox3.Location = new System.Drawing.Point(0, 0);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(415, 88);
			this.groupBox3.TabIndex = 50;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "| Response Search and Replace |";
			this.toolTipSuru.SetToolTip(this.groupBox3, "Search and replace on incoming responses");
			// 
			// label19
			// 
			this.label19.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label19.BackColor = System.Drawing.Color.DarkGray;
			this.label19.ForeColor = System.Drawing.Color.Black;
			this.label19.Location = new System.Drawing.Point(44, 18);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(44, 16);
			this.label19.TabIndex = 53;
			this.label19.Text = "Search";
			// 
			// btnSRDeleteOut
			// 
			this.btnSRDeleteOut.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnSRDeleteOut.BackColor = System.Drawing.Color.DarkGray;
			this.btnSRDeleteOut.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSRDeleteOut.Location = new System.Drawing.Point(248, 58);
			this.btnSRDeleteOut.Name = "btnSRDeleteOut";
			this.btnSRDeleteOut.Size = new System.Drawing.Size(128, 22);
			this.btnSRDeleteOut.TabIndex = 9;
			this.btnSRDeleteOut.Text = "Delete";
			this.btnSRDeleteOut.Click += new System.EventHandler(this.btnSRDeleteOut_Click);
			// 
			// btnSRAddOut
			// 
			this.btnSRAddOut.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnSRAddOut.BackColor = System.Drawing.Color.DarkGray;
			this.btnSRAddOut.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSRAddOut.Location = new System.Drawing.Point(88, 58);
			this.btnSRAddOut.Name = "btnSRAddOut";
			this.btnSRAddOut.Size = new System.Drawing.Size(128, 22);
			this.btnSRAddOut.TabIndex = 8;
			this.btnSRAddOut.Text = "Add";
			this.btnSRAddOut.Click += new System.EventHandler(this.btnSRAddOut_Click);
			// 
			// label18
			// 
			this.label18.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label18.BackColor = System.Drawing.Color.DarkGray;
			this.label18.ForeColor = System.Drawing.Color.Black;
			this.label18.Location = new System.Drawing.Point(44, 38);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(44, 16);
			this.label18.TabIndex = 54;
			this.label18.Text = "Replace";
			// 
			// txtSRReplaceIn
			// 
			this.txtSRReplaceIn.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtSRReplaceIn.BackColor = System.Drawing.Color.LightGray;
			this.txtSRReplaceIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSRReplaceIn.ForeColor = System.Drawing.Color.Black;
			this.txtSRReplaceIn.Location = new System.Drawing.Point(92, 34);
			this.txtSRReplaceIn.Name = "txtSRReplaceIn";
			this.txtSRReplaceIn.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.txtSRReplaceIn.Size = new System.Drawing.Size(283, 20);
			this.txtSRReplaceIn.TabIndex = 7;
			this.txtSRReplaceIn.Text = "Moogle";
			this.txtSRReplaceIn.WordWrap = false;
			// 
			// txtSRSearchIn
			// 
			this.txtSRSearchIn.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtSRSearchIn.BackColor = System.Drawing.Color.LightGray;
			this.txtSRSearchIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSRSearchIn.ForeColor = System.Drawing.Color.Black;
			this.txtSRSearchIn.Location = new System.Drawing.Point(92, 14);
			this.txtSRSearchIn.Name = "txtSRSearchIn";
			this.txtSRSearchIn.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.txtSRSearchIn.Size = new System.Drawing.Size(283, 20);
			this.txtSRSearchIn.TabIndex = 6;
			this.txtSRSearchIn.Text = "Google";
			this.toolTipSuru.SetToolTip(this.txtSRSearchIn, "For binary use `FF (BackTick) - e.g. `3e will result in \'=\'");
			this.txtSRSearchIn.WordWrap = false;
			// 
			// splitter6
			// 
			this.splitter6.BackColor = System.Drawing.Color.DimGray;
			this.splitter6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter6.Location = new System.Drawing.Point(417, 0);
			this.splitter6.Name = "splitter6";
			this.splitter6.Size = new System.Drawing.Size(6, 173);
			this.splitter6.TabIndex = 61;
			this.splitter6.TabStop = false;
			// 
			// panel22
			// 
			this.panel22.Controls.Add(this.panel47);
			this.panel22.Controls.Add(this.panel25);
			this.panel22.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel22.Location = new System.Drawing.Point(0, 0);
			this.panel22.Name = "panel22";
			this.panel22.Size = new System.Drawing.Size(417, 173);
			this.panel22.TabIndex = 60;
			// 
			// panel47
			// 
			this.panel47.Controls.Add(this.lstSRlist);
			this.panel47.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel47.Location = new System.Drawing.Point(0, 88);
			this.panel47.Name = "panel47";
			this.panel47.Size = new System.Drawing.Size(417, 85);
			this.panel47.TabIndex = 61;
			// 
			// lstSRlist
			// 
			this.lstSRlist.BackColor = System.Drawing.Color.Snow;
			this.lstSRlist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstSRlist.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstSRlist.ItemHeight = 12;
			this.lstSRlist.Location = new System.Drawing.Point(0, 0);
			this.lstSRlist.Name = "lstSRlist";
			this.lstSRlist.Size = new System.Drawing.Size(417, 74);
			this.lstSRlist.TabIndex = 5;
			this.lstSRlist.SelectedIndexChanged += new System.EventHandler(this.lstSRlist_SelectedIndexChanged);
			// 
			// panel25
			// 
			this.panel25.Controls.Add(this.groupBox2);
			this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel25.Location = new System.Drawing.Point(0, 0);
			this.panel25.Name = "panel25";
			this.panel25.Size = new System.Drawing.Size(417, 88);
			this.panel25.TabIndex = 60;
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.Color.DarkGray;
			this.groupBox2.Controls.Add(this.btnSRDelete);
			this.groupBox2.Controls.Add(this.btnSRAdd);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.txtSRReplace);
			this.groupBox2.Controls.Add(this.txtSRSearch);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(417, 88);
			this.groupBox2.TabIndex = 59;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "| Request Search and Replace |";
			this.toolTipSuru.SetToolTip(this.groupBox2, "Search and replace on outgoing requests");
			// 
			// btnSRDelete
			// 
			this.btnSRDelete.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnSRDelete.BackColor = System.Drawing.Color.DarkGray;
			this.btnSRDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSRDelete.Location = new System.Drawing.Point(244, 58);
			this.btnSRDelete.Name = "btnSRDelete";
			this.btnSRDelete.Size = new System.Drawing.Size(132, 22);
			this.btnSRDelete.TabIndex = 4;
			this.btnSRDelete.Text = "Delete";
			this.btnSRDelete.Click += new System.EventHandler(this.btnSRDelete_Click);
			// 
			// btnSRAdd
			// 
			this.btnSRAdd.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnSRAdd.BackColor = System.Drawing.Color.DarkGray;
			this.btnSRAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSRAdd.Location = new System.Drawing.Point(92, 58);
			this.btnSRAdd.Name = "btnSRAdd";
			this.btnSRAdd.Size = new System.Drawing.Size(128, 24);
			this.btnSRAdd.TabIndex = 3;
			this.btnSRAdd.Text = "Add";
			this.btnSRAdd.Click += new System.EventHandler(this.btnSRAdd_Click);
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label6.BackColor = System.Drawing.Color.DarkGray;
			this.label6.ForeColor = System.Drawing.Color.Black;
			this.label6.Location = new System.Drawing.Point(40, 38);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 16);
			this.label6.TabIndex = 47;
			this.label6.Text = "Replace";
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label5.BackColor = System.Drawing.Color.DarkGray;
			this.label5.ForeColor = System.Drawing.Color.Black;
			this.label5.Location = new System.Drawing.Point(40, 18);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(44, 16);
			this.label5.TabIndex = 46;
			this.label5.Text = "Search";
			// 
			// txtSRReplace
			// 
			this.txtSRReplace.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtSRReplace.BackColor = System.Drawing.Color.LightGray;
			this.txtSRReplace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSRReplace.ForeColor = System.Drawing.Color.Black;
			this.txtSRReplace.Location = new System.Drawing.Point(92, 34);
			this.txtSRReplace.Name = "txtSRReplace";
			this.txtSRReplace.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.txtSRReplace.Size = new System.Drawing.Size(284, 20);
			this.txtSRReplace.TabIndex = 2;
			this.txtSRReplace.Text = "PostSense";
			this.txtSRReplace.WordWrap = false;
			// 
			// txtSRSearch
			// 
			this.txtSRSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtSRSearch.BackColor = System.Drawing.Color.LightGray;
			this.txtSRSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSRSearch.ForeColor = System.Drawing.Color.Black;
			this.txtSRSearch.Location = new System.Drawing.Point(92, 14);
			this.txtSRSearch.Name = "txtSRSearch";
			this.txtSRSearch.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.txtSRSearch.Size = new System.Drawing.Size(284, 20);
			this.txtSRSearch.TabIndex = 1;
			this.txtSRSearch.Text = "SensePost";
			this.toolTipSuru.SetToolTip(this.txtSRSearch, "For binary use `FF (BackTick) - e.g. `3e will result in \'=\'");
			this.txtSRSearch.WordWrap = false;
			// 
			// splitter5
			// 
			this.splitter5.BackColor = System.Drawing.Color.DimGray;
			this.splitter5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter5.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter5.Location = new System.Drawing.Point(0, 368);
			this.splitter5.Name = "splitter5";
			this.splitter5.Size = new System.Drawing.Size(838, 6);
			this.splitter5.TabIndex = 66;
			this.splitter5.TabStop = false;
			// 
			// panel18
			// 
			this.panel18.Controls.Add(this.groupBox6);
			this.panel18.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel18.Location = new System.Drawing.Point(0, 374);
			this.panel18.Name = "panel18";
			this.panel18.Size = new System.Drawing.Size(838, 154);
			this.panel18.TabIndex = 65;
			// 
			// groupBox6
			// 
			this.groupBox6.BackColor = System.Drawing.Color.DarkGray;
			this.groupBox6.Controls.Add(this.panel19);
			this.groupBox6.Controls.Add(this.panel50);
			this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox6.Location = new System.Drawing.Point(0, 0);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(838, 154);
			this.groupBox6.TabIndex = 64;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "| Auto Relationship Discovery |";
			this.toolTipSuru.SetToolTip(this.groupBox6, "Determine if there is a relationship between all variables (incl. cookie values) " +
				"and the SHA1, MD5, B64enc and B64dec version of all of the variables submitted");
			// 
			// panel19
			// 
			this.panel19.Controls.Add(this.txtAutoMangle);
			this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel19.Location = new System.Drawing.Point(3, 68);
			this.panel19.Name = "panel19";
			this.panel19.Size = new System.Drawing.Size(832, 83);
			this.panel19.TabIndex = 68;
			// 
			// txtAutoMangle
			// 
			this.txtAutoMangle.BackColor = System.Drawing.Color.Snow;
			this.txtAutoMangle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAutoMangle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtAutoMangle.Location = new System.Drawing.Point(0, 0);
			this.txtAutoMangle.Name = "txtAutoMangle";
			this.txtAutoMangle.Size = new System.Drawing.Size(832, 83);
			this.txtAutoMangle.TabIndex = 24;
			this.txtAutoMangle.Text = "";
			this.txtAutoMangle.WordWrap = false;
			// 
			// panel50
			// 
			this.panel50.Controls.Add(this.panel28);
			this.panel50.Controls.Add(this.panel27);
			this.panel50.Controls.Add(this.panel26);
			this.panel50.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel50.Location = new System.Drawing.Point(3, 14);
			this.panel50.Name = "panel50";
			this.panel50.Size = new System.Drawing.Size(832, 54);
			this.panel50.TabIndex = 72;
			// 
			// panel28
			// 
			this.panel28.Controls.Add(this.lstMangleUserInput);
			this.panel28.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel28.Location = new System.Drawing.Point(225, 0);
			this.panel28.Name = "panel28";
			this.panel28.Size = new System.Drawing.Size(527, 54);
			this.panel28.TabIndex = 71;
			// 
			// lstMangleUserInput
			// 
			this.lstMangleUserInput.BackColor = System.Drawing.Color.Gainsboro;
			this.lstMangleUserInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstMangleUserInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstMangleUserInput.ItemHeight = 12;
			this.lstMangleUserInput.Location = new System.Drawing.Point(0, 0);
			this.lstMangleUserInput.Name = "lstMangleUserInput";
			this.lstMangleUserInput.Size = new System.Drawing.Size(527, 50);
			this.lstMangleUserInput.TabIndex = 21;
			this.lstMangleUserInput.SelectedIndexChanged += new System.EventHandler(this.lstMangleUserInput_SelectedIndexChanged);
			// 
			// panel27
			// 
			this.panel27.Controls.Add(this.txtMangleUserInput);
			this.panel27.Controls.Add(this.btnMangleAddUserInput);
			this.panel27.Controls.Add(this.btnMangleDeleteUserInput);
			this.panel27.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel27.Location = new System.Drawing.Point(0, 0);
			this.panel27.Name = "panel27";
			this.panel27.Size = new System.Drawing.Size(225, 54);
			this.panel27.TabIndex = 70;
			// 
			// txtMangleUserInput
			// 
			this.txtMangleUserInput.BackColor = System.Drawing.Color.Snow;
			this.txtMangleUserInput.Location = new System.Drawing.Point(3, 0);
			this.txtMangleUserInput.Name = "txtMangleUserInput";
			this.txtMangleUserInput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.txtMangleUserInput.Size = new System.Drawing.Size(216, 21);
			this.txtMangleUserInput.TabIndex = 18;
			this.txtMangleUserInput.Text = "SensePost";
			this.toolTipSuru.SetToolTip(this.txtMangleUserInput, "Add additional values to be compared against");
			this.txtMangleUserInput.WordWrap = false;
			// 
			// btnMangleAddUserInput
			// 
			this.btnMangleAddUserInput.BackColor = System.Drawing.Color.DarkGray;
			this.btnMangleAddUserInput.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMangleAddUserInput.Location = new System.Drawing.Point(3, 24);
			this.btnMangleAddUserInput.Name = "btnMangleAddUserInput";
			this.btnMangleAddUserInput.Size = new System.Drawing.Size(100, 24);
			this.btnMangleAddUserInput.TabIndex = 19;
			this.btnMangleAddUserInput.Text = "Add";
			this.btnMangleAddUserInput.Click += new System.EventHandler(this.btnMangleAddUserInput_Click);
			// 
			// btnMangleDeleteUserInput
			// 
			this.btnMangleDeleteUserInput.BackColor = System.Drawing.Color.DarkGray;
			this.btnMangleDeleteUserInput.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMangleDeleteUserInput.Location = new System.Drawing.Point(120, 24);
			this.btnMangleDeleteUserInput.Name = "btnMangleDeleteUserInput";
			this.btnMangleDeleteUserInput.Size = new System.Drawing.Size(100, 24);
			this.btnMangleDeleteUserInput.TabIndex = 20;
			this.btnMangleDeleteUserInput.Text = "Delete";
			this.btnMangleDeleteUserInput.Click += new System.EventHandler(this.btnMangleDeleteUserInput_Click);
			// 
			// panel26
			// 
			this.panel26.Controls.Add(this.chkAutoMangle);
			this.panel26.Controls.Add(this.btnAnaliseMangle);
			this.panel26.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel26.Location = new System.Drawing.Point(752, 0);
			this.panel26.Name = "panel26";
			this.panel26.Size = new System.Drawing.Size(80, 54);
			this.panel26.TabIndex = 69;
			// 
			// chkAutoMangle
			// 
			this.chkAutoMangle.BackColor = System.Drawing.Color.Transparent;
			this.chkAutoMangle.Location = new System.Drawing.Point(7, 3);
			this.chkAutoMangle.Name = "chkAutoMangle";
			this.chkAutoMangle.Size = new System.Drawing.Size(67, 21);
			this.chkAutoMangle.TabIndex = 22;
			this.chkAutoMangle.Text = "Auto run";
			this.toolTipSuru.SetToolTip(this.chkAutoMangle, "Automatically run check every 20sec");
			// 
			// btnAnaliseMangle
			// 
			this.btnAnaliseMangle.BackColor = System.Drawing.Color.DarkGray;
			this.btnAnaliseMangle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAnaliseMangle.Location = new System.Drawing.Point(3, 24);
			this.btnAnaliseMangle.Name = "btnAnaliseMangle";
			this.btnAnaliseMangle.Size = new System.Drawing.Size(75, 24);
			this.btnAnaliseMangle.TabIndex = 23;
			this.btnAnaliseMangle.Text = "Manual Run";
			this.btnAnaliseMangle.Click += new System.EventHandler(this.btnAnaliseMangle_Click);
			// 
			// Recon
			// 
			this.Recon.BackColor = System.Drawing.Color.DarkGray;
			this.Recon.Controls.Add(this.panel17);
			this.Recon.Controls.Add(this.panel16);
			this.Recon.Controls.Add(this.panel15);
			this.Recon.ForeColor = System.Drawing.Color.Black;
			this.Recon.Location = new System.Drawing.Point(4, 24);
			this.Recon.Name = "Recon";
			this.Recon.Size = new System.Drawing.Size(838, 528);
			this.Recon.TabIndex = 1;
			this.Recon.Text = "Recon";
			// 
			// panel17
			// 
			this.panel17.Controls.Add(this.treeRecon);
			this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel17.Location = new System.Drawing.Point(0, 57);
			this.panel17.Name = "panel17";
			this.panel17.Size = new System.Drawing.Size(838, 440);
			this.panel17.TabIndex = 64;
			// 
			// treeRecon
			// 
			this.treeRecon.BackColor = System.Drawing.Color.GhostWhite;
			this.treeRecon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.treeRecon.CheckBoxes = true;
			this.treeRecon.ContextMenu = this.cntReconTree;
			this.treeRecon.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeRecon.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F);
			this.treeRecon.ImageIndex = -1;
			this.treeRecon.Indent = 15;
			this.treeRecon.ItemHeight = 16;
			this.treeRecon.Location = new System.Drawing.Point(0, 0);
			this.treeRecon.Name = "treeRecon";
			this.treeRecon.SelectedImageIndex = -1;
			this.treeRecon.Size = new System.Drawing.Size(838, 440);
			this.treeRecon.TabIndex = 12;
			this.toolTipSuru.SetToolTip(this.treeRecon, "To perform file discovery, check relevant checkbox. Undo by unchecking. Different" +
				" colors for HTTP/HTTPS, files and directories. Also right click on item for cont" +
				"ext menu");
			this.treeRecon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SetMousePosition);
			this.treeRecon.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeRecon_AfterCheck);
			// 
			// cntReconTree
			// 
			this.cntReconTree.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem9,
																						 this.menuItem10,
																						 this.menuItem11,
																						 this.menuItem12,
																						 this.menuItem13,
																						 this.menuItem14,
																						 this.menuItem15,
																						 this.menuItem16,
																						 this.menuItem17,
																						 this.menuItem18});
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 0;
			this.menuItem9.Text = "Scan this directory/site for directories";
			this.menuItem9.Click += new System.EventHandler(this.AddDirectoryForDirectoryscan);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 1;
			this.menuItem10.Text = "Scan this directory for files";
			this.menuItem10.Click += new System.EventHandler(this.AddDirectoryForFilescan);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 2;
			this.menuItem11.Text = "-";
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 3;
			this.menuItem12.Text = "Expand node";
			this.menuItem12.Click += new System.EventHandler(this.expandTreeNode);
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 4;
			this.menuItem13.Text = "Prune node";
			this.menuItem13.Click += new System.EventHandler(this.pruneTreeNode);
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 5;
			this.menuItem14.Text = "Delete node";
			this.menuItem14.Click += new System.EventHandler(this.deleteTreeNode);
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 6;
			this.menuItem15.Text = "-";
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 7;
			this.menuItem16.Text = "Clear queue for this host";
			this.menuItem16.Click += new System.EventHandler(this.clearQonehost);
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 8;
			this.menuItem17.Text = "-";
			// 
			// menuItem18
			// 
			this.menuItem18.Index = 9;
			this.menuItem18.Text = "Navigate here now";
			this.menuItem18.Click += new System.EventHandler(this.TreeNavigate);
			// 
			// panel16
			// 
			this.panel16.Controls.Add(this.chkSmartDirScan);
			this.panel16.Controls.Add(this.chkSmartFileDeep);
			this.panel16.Controls.Add(this.chkSmartFileShallow);
			this.panel16.Controls.Add(this.chkDoRecon);
			this.panel16.Controls.Add(this.cmbReconTargetHost);
			this.panel16.Controls.Add(this.ckhReconIndex);
			this.panel16.Controls.Add(this.chkReconDirMine);
			this.panel16.Controls.Add(this.label23);
			this.panel16.Controls.Add(this.lblJobQLength);
			this.panel16.Controls.Add(this.lblCurrentJobQ);
			this.panel16.Controls.Add(this.label21);
			this.panel16.Controls.Add(this.NUPDOWNBackEnd);
			this.panel16.Controls.Add(this.label17);
			this.panel16.Controls.Add(this.trckReconSpeed);
			this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel16.Location = new System.Drawing.Point(0, 0);
			this.panel16.Name = "panel16";
			this.panel16.Size = new System.Drawing.Size(838, 57);
			this.panel16.TabIndex = 63;
			// 
			// chkSmartDirScan
			// 
			this.chkSmartDirScan.BackColor = System.Drawing.Color.Transparent;
			this.chkSmartDirScan.ForeColor = System.Drawing.Color.Black;
			this.chkSmartDirScan.Location = new System.Drawing.Point(338, 30);
			this.chkSmartDirScan.Name = "chkSmartDirScan";
			this.chkSmartDirScan.Size = new System.Drawing.Size(110, 16);
			this.chkSmartDirScan.TabIndex = 7;
			this.chkSmartDirScan.Text = "Dir Smart Scan";
			this.toolTipSuru.SetToolTip(this.chkSmartDirScan, "With file & dir checking dynamically include directories found in the past");
			// 
			// chkSmartFileDeep
			// 
			this.chkSmartFileDeep.BackColor = System.Drawing.Color.Transparent;
			this.chkSmartFileDeep.ForeColor = System.Drawing.Color.Black;
			this.chkSmartFileDeep.Location = new System.Drawing.Point(332, 8);
			this.chkSmartFileDeep.Name = "chkSmartFileDeep";
			this.chkSmartFileDeep.Size = new System.Drawing.Size(13, 16);
			this.chkSmartFileDeep.TabIndex = 3;
			this.chkSmartFileDeep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTipSuru.SetToolTip(this.chkSmartFileDeep, "Checks for the existence of files with the same name but different extensions acr" +
				"oss all known directories");
			this.chkSmartFileDeep.CheckedChanged += new System.EventHandler(this.chkSmartFileDeep_CheckedChanged);
			// 
			// chkSmartFileShallow
			// 
			this.chkSmartFileShallow.BackColor = System.Drawing.Color.Transparent;
			this.chkSmartFileShallow.ForeColor = System.Drawing.Color.Black;
			this.chkSmartFileShallow.Location = new System.Drawing.Point(346, 8);
			this.chkSmartFileShallow.Name = "chkSmartFileShallow";
			this.chkSmartFileShallow.Size = new System.Drawing.Size(116, 16);
			this.chkSmartFileShallow.TabIndex = 4;
			this.chkSmartFileShallow.Text = "File Smart Scan";
			this.toolTipSuru.SetToolTip(this.chkSmartFileShallow, "Checks for the existence of files with the same name but different extensions in " +
				"the same directory");
			this.chkSmartFileShallow.CheckedChanged += new System.EventHandler(this.chkSmartFileShallow_CheckedChanged);
			// 
			// chkDoRecon
			// 
			this.chkDoRecon.BackColor = System.Drawing.Color.DarkGray;
			this.chkDoRecon.ForeColor = System.Drawing.Color.Black;
			this.chkDoRecon.Location = new System.Drawing.Point(8, 8);
			this.chkDoRecon.Name = "chkDoRecon";
			this.chkDoRecon.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkDoRecon.Size = new System.Drawing.Size(156, 16);
			this.chkDoRecon.TabIndex = 1;
			this.chkDoRecon.Text = "Scan sites as they appear";
			this.toolTipSuru.SetToolTip(this.chkDoRecon, "CAUTION: USE WITH CARE. Perform indexability or directory mining on sites as you " +
				"surf to it");
			this.chkDoRecon.CheckedChanged += new System.EventHandler(this.chkDoRecon_CheckedChanged);
			// 
			// cmbReconTargetHost
			// 
			this.cmbReconTargetHost.BackColor = System.Drawing.Color.AliceBlue;
			this.cmbReconTargetHost.Items.AddRange(new object[] {
																	"----------------------------------------------"});
			this.cmbReconTargetHost.Location = new System.Drawing.Point(3, 31);
			this.cmbReconTargetHost.MaxDropDownItems = 12;
			this.cmbReconTargetHost.Name = "cmbReconTargetHost";
			this.cmbReconTargetHost.Size = new System.Drawing.Size(203, 20);
			this.cmbReconTargetHost.TabIndex = 5;
			this.cmbReconTargetHost.Text = "Specific Recon target";
			// 
			// ckhReconIndex
			// 
			this.ckhReconIndex.BackColor = System.Drawing.Color.Transparent;
			this.ckhReconIndex.Checked = true;
			this.ckhReconIndex.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckhReconIndex.ForeColor = System.Drawing.Color.Black;
			this.ckhReconIndex.Location = new System.Drawing.Point(212, 32);
			this.ckhReconIndex.Name = "ckhReconIndex";
			this.ckhReconIndex.Size = new System.Drawing.Size(120, 16);
			this.ckhReconIndex.TabIndex = 6;
			this.ckhReconIndex.Text = "Check Indexability";
			this.toolTipSuru.SetToolTip(this.ckhReconIndex, "Checks for indexability of directories");
			// 
			// chkReconDirMine
			// 
			this.chkReconDirMine.BackColor = System.Drawing.Color.DarkGray;
			this.chkReconDirMine.Checked = true;
			this.chkReconDirMine.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkReconDirMine.ForeColor = System.Drawing.Color.Black;
			this.chkReconDirMine.Location = new System.Drawing.Point(212, 8);
			this.chkReconDirMine.Name = "chkReconDirMine";
			this.chkReconDirMine.Size = new System.Drawing.Size(102, 16);
			this.chkReconDirMine.TabIndex = 2;
			this.chkReconDirMine.Text = "Directory mining";
			this.toolTipSuru.SetToolTip(this.chkReconDirMine, "Mine for common directories in the current directory");
			// 
			// label23
			// 
			this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label23.BackColor = System.Drawing.Color.DarkGray;
			this.label23.ForeColor = System.Drawing.Color.Black;
			this.label23.Location = new System.Drawing.Point(474, 8);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(83, 12);
			this.label23.TabIndex = 45;
			this.label23.Text = "Number of jobs";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblJobQLength
			// 
			this.lblJobQLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblJobQLength.BackColor = System.Drawing.Color.Snow;
			this.lblJobQLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblJobQLength.Location = new System.Drawing.Point(448, 28);
			this.lblJobQLength.Name = "lblJobQLength";
			this.lblJobQLength.Size = new System.Drawing.Size(83, 18);
			this.lblJobQLength.TabIndex = 8;
			this.lblJobQLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTipSuru.SetToolTip(this.lblJobQLength, "How many jobs in total");
			// 
			// lblCurrentJobQ
			// 
			this.lblCurrentJobQ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCurrentJobQ.BackColor = System.Drawing.Color.Snow;
			this.lblCurrentJobQ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblCurrentJobQ.Location = new System.Drawing.Point(536, 28);
			this.lblCurrentJobQ.Name = "lblCurrentJobQ";
			this.lblCurrentJobQ.Size = new System.Drawing.Size(26, 18);
			this.lblCurrentJobQ.TabIndex = 9;
			this.lblCurrentJobQ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTipSuru.SetToolTip(this.lblCurrentJobQ, "How many jobs waiting in the queue");
			// 
			// label21
			// 
			this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label21.BackColor = System.Drawing.Color.DarkGray;
			this.label21.ForeColor = System.Drawing.Color.Black;
			this.label21.Location = new System.Drawing.Point(572, 8);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(84, 13);
			this.label21.TabIndex = 42;
			this.label21.Text = "AI trigger level";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTipSuru.SetToolTip(this.label21, "\"Difference in response\"-index lower than this will trigger a file/directory foun" +
				"d");
			// 
			// NUPDOWNBackEnd
			// 
			this.NUPDOWNBackEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.NUPDOWNBackEnd.BackColor = System.Drawing.Color.Snow;
			this.NUPDOWNBackEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.NUPDOWNBackEnd.DecimalPlaces = 3;
			this.NUPDOWNBackEnd.Increment = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 65536});
			this.NUPDOWNBackEnd.Location = new System.Drawing.Point(573, 28);
			this.NUPDOWNBackEnd.Name = "NUPDOWNBackEnd";
			this.NUPDOWNBackEnd.Size = new System.Drawing.Size(84, 18);
			this.NUPDOWNBackEnd.TabIndex = 10;
			this.NUPDOWNBackEnd.Value = new System.Decimal(new int[] {
																		 80,
																		 0,
																		 0,
																		 131072});
			// 
			// label17
			// 
			this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label17.BackColor = System.Drawing.Color.DarkGray;
			this.label17.ForeColor = System.Drawing.Color.Black;
			this.label17.Location = new System.Drawing.Point(660, 8);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(168, 11);
			this.label17.TabIndex = 38;
			this.label17.Text = "3s---------2s---------1s--------max";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// trckReconSpeed
			// 
			this.trckReconSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.trckReconSpeed.Location = new System.Drawing.Point(660, 26);
			this.trckReconSpeed.Maximum = 3000;
			this.trckReconSpeed.Name = "trckReconSpeed";
			this.trckReconSpeed.Size = new System.Drawing.Size(168, 42);
			this.trckReconSpeed.TabIndex = 11;
			this.trckReconSpeed.TickFrequency = 5;
			this.trckReconSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
			this.toolTipSuru.SetToolTip(this.trckReconSpeed, "Speed at which to recon");
			this.trckReconSpeed.Value = 2000;
			this.trckReconSpeed.Scroll += new System.EventHandler(this.trckReconSpeed_Scroll);
			// 
			// panel15
			// 
			this.panel15.Controls.Add(this.chkReconAlwaysExpand);
			this.panel15.Controls.Add(this.btnReconTreeExpandAll);
			this.panel15.Controls.Add(this.btnReconTreeCollapseAll);
			this.panel15.Controls.Add(this.btnTreeReload);
			this.panel15.Controls.Add(this.btnClearJobQ);
			this.panel15.Controls.Add(this.btnReconClearTree);
			this.panel15.Controls.Add(this.button1);
			this.panel15.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel15.Location = new System.Drawing.Point(0, 497);
			this.panel15.Name = "panel15";
			this.panel15.Size = new System.Drawing.Size(838, 31);
			this.panel15.TabIndex = 62;
			// 
			// chkReconAlwaysExpand
			// 
			this.chkReconAlwaysExpand.BackColor = System.Drawing.Color.Transparent;
			this.chkReconAlwaysExpand.Checked = true;
			this.chkReconAlwaysExpand.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkReconAlwaysExpand.ForeColor = System.Drawing.Color.Black;
			this.chkReconAlwaysExpand.Location = new System.Drawing.Point(12, 3);
			this.chkReconAlwaysExpand.Name = "chkReconAlwaysExpand";
			this.chkReconAlwaysExpand.Size = new System.Drawing.Size(120, 24);
			this.chkReconAlwaysExpand.TabIndex = 13;
			this.chkReconAlwaysExpand.Text = "Expand discovered";
			this.toolTipSuru.SetToolTip(this.chkReconAlwaysExpand, "Expand the tree around discovered items");
			// 
			// btnReconTreeExpandAll
			// 
			this.btnReconTreeExpandAll.BackColor = System.Drawing.Color.DarkGray;
			this.btnReconTreeExpandAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnReconTreeExpandAll.Location = new System.Drawing.Point(136, 3);
			this.btnReconTreeExpandAll.Name = "btnReconTreeExpandAll";
			this.btnReconTreeExpandAll.Size = new System.Drawing.Size(84, 24);
			this.btnReconTreeExpandAll.TabIndex = 14;
			this.btnReconTreeExpandAll.Text = "Expand all";
			this.toolTipSuru.SetToolTip(this.btnReconTreeExpandAll, "Expands the entire tree");
			this.btnReconTreeExpandAll.Click += new System.EventHandler(this.btnReconTreeExpandAll_Click);
			// 
			// btnReconTreeCollapseAll
			// 
			this.btnReconTreeCollapseAll.BackColor = System.Drawing.Color.DarkGray;
			this.btnReconTreeCollapseAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnReconTreeCollapseAll.Location = new System.Drawing.Point(232, 3);
			this.btnReconTreeCollapseAll.Name = "btnReconTreeCollapseAll";
			this.btnReconTreeCollapseAll.Size = new System.Drawing.Size(84, 24);
			this.btnReconTreeCollapseAll.TabIndex = 15;
			this.btnReconTreeCollapseAll.Text = "Collapse all";
			this.toolTipSuru.SetToolTip(this.btnReconTreeCollapseAll, "Collapse entrie tree");
			this.btnReconTreeCollapseAll.Click += new System.EventHandler(this.btnReconTreeCollapseAll_Click);
			// 
			// btnTreeReload
			// 
			this.btnTreeReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTreeReload.BackColor = System.Drawing.Color.DarkGray;
			this.btnTreeReload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnTreeReload.ForeColor = System.Drawing.Color.Black;
			this.btnTreeReload.Location = new System.Drawing.Point(468, 3);
			this.btnTreeReload.Name = "btnTreeReload";
			this.btnTreeReload.Size = new System.Drawing.Size(124, 24);
			this.btnTreeReload.TabIndex = 16;
			this.btnTreeReload.Text = "Manually Reload tree";
			this.toolTipSuru.SetToolTip(this.btnTreeReload, "Click here if you changed settings and wish to restart the recon process");
			this.btnTreeReload.Click += new System.EventHandler(this.btnTreeReload_Click);
			// 
			// btnClearJobQ
			// 
			this.btnClearJobQ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearJobQ.BackColor = System.Drawing.Color.DarkGray;
			this.btnClearJobQ.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnClearJobQ.ForeColor = System.Drawing.Color.DarkGoldenrod;
			this.btnClearJobQ.Location = new System.Drawing.Point(596, 4);
			this.btnClearJobQ.Name = "btnClearJobQ";
			this.btnClearJobQ.Size = new System.Drawing.Size(116, 24);
			this.btnClearJobQ.TabIndex = 17;
			this.btnClearJobQ.Text = "Clear Job Queue";
			this.toolTipSuru.SetToolTip(this.btnClearJobQ, "Zero the queue - when things go bad...");
			this.btnClearJobQ.Click += new System.EventHandler(this.btnClearJobQ_Click);
			// 
			// btnReconClearTree
			// 
			this.btnReconClearTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReconClearTree.BackColor = System.Drawing.Color.DarkGray;
			this.btnReconClearTree.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnReconClearTree.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnReconClearTree.ForeColor = System.Drawing.Color.Brown;
			this.btnReconClearTree.Location = new System.Drawing.Point(716, 3);
			this.btnReconClearTree.Name = "btnReconClearTree";
			this.btnReconClearTree.Size = new System.Drawing.Size(116, 24);
			this.btnReconClearTree.TabIndex = 18;
			this.btnReconClearTree.Text = "Clear All";
			this.toolTipSuru.SetToolTip(this.btnReconClearTree, "Clear all intel information, including tree");
			this.btnReconClearTree.Click += new System.EventHandler(this.btnReconClearTree_Click);
			// 
			// button1
			// 
			this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.button1.Location = new System.Drawing.Point(832, 24);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(1, 1);
			this.button1.TabIndex = 46;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Config
			// 
			this.Config.AutoScroll = true;
			this.Config.BackColor = System.Drawing.Color.LightSlateGray;
			this.Config.Controls.Add(this.panel38);
			this.Config.Controls.Add(this.splitter10);
			this.Config.Controls.Add(this.panel37);
			this.Config.Controls.Add(this.panel33);
			this.Config.Location = new System.Drawing.Point(4, 24);
			this.Config.Name = "Config";
			this.Config.Size = new System.Drawing.Size(838, 528);
			this.Config.TabIndex = 2;
			this.Config.Text = "Config";
			// 
			// panel38
			// 
			this.panel38.Controls.Add(this.groupBox8);
			this.panel38.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel38.Location = new System.Drawing.Point(0, 198);
			this.panel38.Name = "panel38";
			this.panel38.Size = new System.Drawing.Size(838, 233);
			this.panel38.TabIndex = 82;
			// 
			// groupBox8
			// 
			this.groupBox8.BackColor = System.Drawing.Color.LightGray;
			this.groupBox8.Controls.Add(this.panel46);
			this.groupBox8.Controls.Add(this.panel45);
			this.groupBox8.Controls.Add(this.panel44);
			this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox8.Location = new System.Drawing.Point(0, 0);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(838, 233);
			this.groupBox8.TabIndex = 68;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "| Fuzz DB Management |";
			this.toolTipSuru.SetToolTip(this.groupBox8, "This basically allows you to edit TXT files in the \'Fuzz DB\'");
			// 
			// panel46
			// 
			this.panel46.Controls.Add(this.txtConfigFuzzContent);
			this.panel46.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel46.Location = new System.Drawing.Point(3, 73);
			this.panel46.Name = "panel46";
			this.panel46.Size = new System.Drawing.Size(832, 132);
			this.panel46.TabIndex = 65;
			// 
			// txtConfigFuzzContent
			// 
			this.txtConfigFuzzContent.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtConfigFuzzContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtConfigFuzzContent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtConfigFuzzContent.Location = new System.Drawing.Point(0, 0);
			this.txtConfigFuzzContent.Name = "txtConfigFuzzContent";
			this.txtConfigFuzzContent.Size = new System.Drawing.Size(832, 132);
			this.txtConfigFuzzContent.TabIndex = 14;
			this.txtConfigFuzzContent.Text = "";
			this.txtConfigFuzzContent.WordWrap = false;
			// 
			// panel45
			// 
			this.panel45.BackColor = System.Drawing.Color.Silver;
			this.panel45.Controls.Add(this.btnBackEndUpdateDirs);
			this.panel45.Controls.Add(this.cmbBackEndUpdate);
			this.panel45.Controls.Add(this.cmbFuzzFileEdit);
			this.panel45.Controls.Add(this.txtConfigFuzzFileName);
			this.panel45.Controls.Add(this.btnUpdateQuickNav);
			this.panel45.Controls.Add(this.txtFuzzDirLocation);
			this.panel45.Controls.Add(this.btnFuzzDBLocationFind);
			this.panel45.Controls.Add(this.label9);
			this.panel45.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel45.Location = new System.Drawing.Point(3, 14);
			this.panel45.Name = "panel45";
			this.panel45.Size = new System.Drawing.Size(832, 59);
			this.panel45.TabIndex = 63;
			// 
			// btnBackEndUpdateDirs
			// 
			this.btnBackEndUpdateDirs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBackEndUpdateDirs.BackColor = System.Drawing.Color.Silver;
			this.btnBackEndUpdateDirs.Location = new System.Drawing.Point(684, 4);
			this.btnBackEndUpdateDirs.Name = "btnBackEndUpdateDirs";
			this.btnBackEndUpdateDirs.Size = new System.Drawing.Size(140, 28);
			this.btnBackEndUpdateDirs.TabIndex = 9;
			this.btnBackEndUpdateDirs.Text = "Update from SensePost";
			this.btnBackEndUpdateDirs.Click += new System.EventHandler(this.btnBackEndUpdateDirs_Click);
			// 
			// cmbBackEndUpdate
			// 
			this.cmbBackEndUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbBackEndUpdate.BackColor = System.Drawing.Color.GhostWhite;
			this.cmbBackEndUpdate.Location = new System.Drawing.Point(692, 32);
			this.cmbBackEndUpdate.Name = "cmbBackEndUpdate";
			this.cmbBackEndUpdate.Size = new System.Drawing.Size(128, 18);
			this.cmbBackEndUpdate.TabIndex = 13;
			this.cmbBackEndUpdate.Text = "Click update";
			// 
			// cmbFuzzFileEdit
			// 
			this.cmbFuzzFileEdit.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cmbFuzzFileEdit.BackColor = System.Drawing.Color.AliceBlue;
			this.cmbFuzzFileEdit.Location = new System.Drawing.Point(344, 31);
			this.cmbFuzzFileEdit.Name = "cmbFuzzFileEdit";
			this.cmbFuzzFileEdit.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmbFuzzFileEdit.Size = new System.Drawing.Size(294, 18);
			this.cmbFuzzFileEdit.TabIndex = 12;
			this.cmbFuzzFileEdit.Text = "Select fuzz file";
			this.cmbFuzzFileEdit.SelectedIndexChanged += new System.EventHandler(this.cmbFuzzFileEdit_SelectedIndexChanged);
			// 
			// txtConfigFuzzFileName
			// 
			this.txtConfigFuzzFileName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtConfigFuzzFileName.BackColor = System.Drawing.Color.AliceBlue;
			this.txtConfigFuzzFileName.Location = new System.Drawing.Point(344, 6);
			this.txtConfigFuzzFileName.Name = "txtConfigFuzzFileName";
			this.txtConfigFuzzFileName.Size = new System.Drawing.Size(293, 18);
			this.txtConfigFuzzFileName.TabIndex = 8;
			this.txtConfigFuzzFileName.Text = "New_File_Name";
			// 
			// btnUpdateQuickNav
			// 
			this.btnUpdateQuickNav.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnUpdateQuickNav.BackColor = System.Drawing.Color.Silver;
			this.btnUpdateQuickNav.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnUpdateQuickNav.ForeColor = System.Drawing.Color.SteelBlue;
			this.btnUpdateQuickNav.Location = new System.Drawing.Point(8, 31);
			this.btnUpdateQuickNav.Name = "btnUpdateQuickNav";
			this.btnUpdateQuickNav.Size = new System.Drawing.Size(112, 25);
			this.btnUpdateQuickNav.TabIndex = 10;
			this.btnUpdateQuickNav.Text = "Reload DB";
			this.btnUpdateQuickNav.Click += new System.EventHandler(this.btnUpdateQuickNav_Click_1);
			// 
			// txtFuzzDirLocation
			// 
			this.txtFuzzDirLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtFuzzDirLocation.BackColor = System.Drawing.Color.DarkGray;
			this.txtFuzzDirLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFuzzDirLocation.Location = new System.Drawing.Point(124, 24);
			this.txtFuzzDirLocation.Name = "txtFuzzDirLocation";
			this.txtFuzzDirLocation.Size = new System.Drawing.Size(212, 28);
			this.txtFuzzDirLocation.TabIndex = 11;
			this.txtFuzzDirLocation.Text = "c:\\Program Files\\SensePost\\Suru\\FuzzDB\\";
			this.txtFuzzDirLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnFuzzDBLocationFind
			// 
			this.btnFuzzDBLocationFind.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnFuzzDBLocationFind.BackColor = System.Drawing.Color.Silver;
			this.btnFuzzDBLocationFind.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnFuzzDBLocationFind.ForeColor = System.Drawing.Color.Black;
			this.btnFuzzDBLocationFind.Location = new System.Drawing.Point(8, 6);
			this.btnFuzzDBLocationFind.Name = "btnFuzzDBLocationFind";
			this.btnFuzzDBLocationFind.Size = new System.Drawing.Size(112, 26);
			this.btnFuzzDBLocationFind.TabIndex = 6;
			this.btnFuzzDBLocationFind.Text = "Find Directory";
			this.btnFuzzDBLocationFind.Click += new System.EventHandler(this.btnFuzzDBLocationFind_Click);
			// 
			// label9
			// 
			this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label9.Location = new System.Drawing.Point(124, 9);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(210, 16);
			this.label9.TabIndex = 7;
			this.label9.Text = "Fuzz DB location for quick nav";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel44
			// 
			this.panel44.Controls.Add(this.btnSaveFuzzFile);
			this.panel44.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel44.Location = new System.Drawing.Point(3, 205);
			this.panel44.Name = "panel44";
			this.panel44.Size = new System.Drawing.Size(832, 25);
			this.panel44.TabIndex = 62;
			// 
			// btnSaveFuzzFile
			// 
			this.btnSaveFuzzFile.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnSaveFuzzFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSaveFuzzFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSaveFuzzFile.ForeColor = System.Drawing.Color.Black;
			this.btnSaveFuzzFile.Location = new System.Drawing.Point(0, 0);
			this.btnSaveFuzzFile.Name = "btnSaveFuzzFile";
			this.btnSaveFuzzFile.Size = new System.Drawing.Size(832, 25);
			this.btnSaveFuzzFile.TabIndex = 15;
			this.btnSaveFuzzFile.Text = "Create / Save file";
			this.btnSaveFuzzFile.Click += new System.EventHandler(this.btnSaveFuzzFile_Click);
			// 
			// splitter10
			// 
			this.splitter10.BackColor = System.Drawing.Color.DimGray;
			this.splitter10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter10.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter10.Location = new System.Drawing.Point(0, 192);
			this.splitter10.Name = "splitter10";
			this.splitter10.Size = new System.Drawing.Size(838, 6);
			this.splitter10.TabIndex = 81;
			this.splitter10.TabStop = false;
			// 
			// panel37
			// 
			this.panel37.Controls.Add(this.groupBox7);
			this.panel37.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel37.Location = new System.Drawing.Point(0, 0);
			this.panel37.Name = "panel37";
			this.panel37.Size = new System.Drawing.Size(838, 192);
			this.panel37.TabIndex = 79;
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.panel43);
			this.groupBox7.Controls.Add(this.splitter14);
			this.groupBox7.Controls.Add(this.panel42);
			this.groupBox7.Controls.Add(this.splitter13);
			this.groupBox7.Controls.Add(this.panel41);
			this.groupBox7.Controls.Add(this.splitter12);
			this.groupBox7.Controls.Add(this.panel40);
			this.groupBox7.Controls.Add(this.splitter11);
			this.groupBox7.Controls.Add(this.panel39);
			this.groupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox7.Location = new System.Drawing.Point(0, 0);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(838, 192);
			this.groupBox7.TabIndex = 67;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "| Recon |";
			// 
			// panel43
			// 
			this.panel43.Controls.Add(this.groupBox22);
			this.panel43.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel43.Location = new System.Drawing.Point(693, 14);
			this.panel43.Name = "panel43";
			this.panel43.Size = new System.Drawing.Size(142, 175);
			this.panel43.TabIndex = 78;
			// 
			// groupBox22
			// 
			this.groupBox22.Controls.Add(this.txtReconSkipSites);
			this.groupBox22.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox22.Location = new System.Drawing.Point(0, 0);
			this.groupBox22.Name = "groupBox22";
			this.groupBox22.Size = new System.Drawing.Size(142, 175);
			this.groupBox22.TabIndex = 69;
			this.groupBox22.TabStop = false;
			this.groupBox22.Text = "Skip Sites";
			// 
			// txtReconSkipSites
			// 
			this.txtReconSkipSites.BackColor = System.Drawing.Color.GhostWhite;
			this.txtReconSkipSites.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtReconSkipSites.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtReconSkipSites.Location = new System.Drawing.Point(3, 14);
			this.txtReconSkipSites.Name = "txtReconSkipSites";
			this.txtReconSkipSites.Size = new System.Drawing.Size(136, 158);
			this.txtReconSkipSites.TabIndex = 5;
			this.txtReconSkipSites.Text = "";
			this.toolTipSuru.SetToolTip(this.txtReconSkipSites, "Sites containing these words will be skipped ");
			// 
			// splitter14
			// 
			this.splitter14.BackColor = System.Drawing.Color.DimGray;
			this.splitter14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter14.Location = new System.Drawing.Point(687, 14);
			this.splitter14.Name = "splitter14";
			this.splitter14.Size = new System.Drawing.Size(6, 175);
			this.splitter14.TabIndex = 77;
			this.splitter14.TabStop = false;
			// 
			// panel42
			// 
			this.panel42.Controls.Add(this.groupBox21);
			this.panel42.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel42.Location = new System.Drawing.Point(520, 14);
			this.panel42.Name = "panel42";
			this.panel42.Size = new System.Drawing.Size(167, 175);
			this.panel42.TabIndex = 76;
			// 
			// groupBox21
			// 
			this.groupBox21.Controls.Add(this.txtWiktoTestFilenames);
			this.groupBox21.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox21.Location = new System.Drawing.Point(0, 0);
			this.groupBox21.Name = "groupBox21";
			this.groupBox21.Size = new System.Drawing.Size(167, 175);
			this.groupBox21.TabIndex = 68;
			this.groupBox21.TabStop = false;
			this.groupBox21.Text = "Test File Names";
			// 
			// txtWiktoTestFilenames
			// 
			this.txtWiktoTestFilenames.BackColor = System.Drawing.Color.Ivory;
			this.txtWiktoTestFilenames.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtWiktoTestFilenames.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtWiktoTestFilenames.Location = new System.Drawing.Point(3, 14);
			this.txtWiktoTestFilenames.Name = "txtWiktoTestFilenames";
			this.txtWiktoTestFilenames.Size = new System.Drawing.Size(161, 158);
			this.txtWiktoTestFilenames.TabIndex = 4;
			this.txtWiktoTestFilenames.Text = "";
			this.toolTipSuru.SetToolTip(this.txtWiktoTestFilenames, "File names to test for");
			// 
			// splitter13
			// 
			this.splitter13.BackColor = System.Drawing.Color.DimGray;
			this.splitter13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter13.Location = new System.Drawing.Point(514, 14);
			this.splitter13.Name = "splitter13";
			this.splitter13.Size = new System.Drawing.Size(6, 175);
			this.splitter13.TabIndex = 75;
			this.splitter13.TabStop = false;
			// 
			// panel41
			// 
			this.panel41.Controls.Add(this.groupBox20);
			this.panel41.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel41.Location = new System.Drawing.Point(348, 14);
			this.panel41.Name = "panel41";
			this.panel41.Size = new System.Drawing.Size(166, 175);
			this.panel41.TabIndex = 74;
			// 
			// groupBox20
			// 
			this.groupBox20.Controls.Add(this.txtWiktoTestTypes);
			this.groupBox20.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox20.Location = new System.Drawing.Point(0, 0);
			this.groupBox20.Name = "groupBox20";
			this.groupBox20.Size = new System.Drawing.Size(166, 175);
			this.groupBox20.TabIndex = 67;
			this.groupBox20.TabStop = false;
			this.groupBox20.Text = "Test File Types";
			// 
			// txtWiktoTestTypes
			// 
			this.txtWiktoTestTypes.BackColor = System.Drawing.Color.Ivory;
			this.txtWiktoTestTypes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtWiktoTestTypes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtWiktoTestTypes.Location = new System.Drawing.Point(3, 14);
			this.txtWiktoTestTypes.Name = "txtWiktoTestTypes";
			this.txtWiktoTestTypes.Size = new System.Drawing.Size(160, 158);
			this.txtWiktoTestTypes.TabIndex = 3;
			this.txtWiktoTestTypes.Text = "";
			this.toolTipSuru.SetToolTip(this.txtWiktoTestTypes, "File types to check for");
			// 
			// splitter12
			// 
			this.splitter12.BackColor = System.Drawing.Color.DimGray;
			this.splitter12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter12.Location = new System.Drawing.Point(342, 14);
			this.splitter12.Name = "splitter12";
			this.splitter12.Size = new System.Drawing.Size(6, 175);
			this.splitter12.TabIndex = 73;
			this.splitter12.TabStop = false;
			// 
			// panel40
			// 
			this.panel40.Controls.Add(this.groupBox19);
			this.panel40.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel40.Location = new System.Drawing.Point(176, 14);
			this.panel40.Name = "panel40";
			this.panel40.Size = new System.Drawing.Size(166, 175);
			this.panel40.TabIndex = 72;
			// 
			// groupBox19
			// 
			this.groupBox19.Controls.Add(this.txtWiktoSkipDirs);
			this.groupBox19.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox19.Location = new System.Drawing.Point(0, 0);
			this.groupBox19.Name = "groupBox19";
			this.groupBox19.Size = new System.Drawing.Size(166, 175);
			this.groupBox19.TabIndex = 66;
			this.groupBox19.TabStop = false;
			this.groupBox19.Text = "Skip Directories";
			// 
			// txtWiktoSkipDirs
			// 
			this.txtWiktoSkipDirs.BackColor = System.Drawing.Color.GhostWhite;
			this.txtWiktoSkipDirs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtWiktoSkipDirs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtWiktoSkipDirs.Location = new System.Drawing.Point(3, 14);
			this.txtWiktoSkipDirs.Name = "txtWiktoSkipDirs";
			this.txtWiktoSkipDirs.Size = new System.Drawing.Size(160, 158);
			this.txtWiktoSkipDirs.TabIndex = 2;
			this.txtWiktoSkipDirs.Text = "";
			this.toolTipSuru.SetToolTip(this.txtWiktoSkipDirs, "Skip these directories when performing mining");
			// 
			// splitter11
			// 
			this.splitter11.BackColor = System.Drawing.Color.DimGray;
			this.splitter11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter11.Location = new System.Drawing.Point(170, 14);
			this.splitter11.Name = "splitter11";
			this.splitter11.Size = new System.Drawing.Size(6, 175);
			this.splitter11.TabIndex = 71;
			this.splitter11.TabStop = false;
			// 
			// panel39
			// 
			this.panel39.Controls.Add(this.groupBox18);
			this.panel39.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel39.Location = new System.Drawing.Point(3, 14);
			this.panel39.Name = "panel39";
			this.panel39.Size = new System.Drawing.Size(167, 175);
			this.panel39.TabIndex = 70;
			// 
			// groupBox18
			// 
			this.groupBox18.Controls.Add(this.txtWiktoTestDirs);
			this.groupBox18.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox18.Location = new System.Drawing.Point(0, 0);
			this.groupBox18.Name = "groupBox18";
			this.groupBox18.Size = new System.Drawing.Size(167, 175);
			this.groupBox18.TabIndex = 65;
			this.groupBox18.TabStop = false;
			this.groupBox18.Text = "Test Directories";
			// 
			// txtWiktoTestDirs
			// 
			this.txtWiktoTestDirs.BackColor = System.Drawing.Color.Ivory;
			this.txtWiktoTestDirs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtWiktoTestDirs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtWiktoTestDirs.Location = new System.Drawing.Point(3, 14);
			this.txtWiktoTestDirs.Name = "txtWiktoTestDirs";
			this.txtWiktoTestDirs.Size = new System.Drawing.Size(161, 158);
			this.txtWiktoTestDirs.TabIndex = 1;
			this.txtWiktoTestDirs.Text = "";
			this.toolTipSuru.SetToolTip(this.txtWiktoTestDirs, "Directories used for mining");
			// 
			// panel33
			// 
			this.panel33.Controls.Add(this.panel36);
			this.panel33.Controls.Add(this.panel35);
			this.panel33.Controls.Add(this.panel34);
			this.panel33.Controls.Add(this.groupBox17);
			this.panel33.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel33.Location = new System.Drawing.Point(0, 431);
			this.panel33.Name = "panel33";
			this.panel33.Size = new System.Drawing.Size(838, 97);
			this.panel33.TabIndex = 78;
			// 
			// panel36
			// 
			this.panel36.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.panel36.Controls.Add(this.groupBox10);
			this.panel36.Location = new System.Drawing.Point(684, 0);
			this.panel36.Name = "panel36";
			this.panel36.Size = new System.Drawing.Size(143, 97);
			this.panel36.TabIndex = 75;
			// 
			// groupBox10
			// 
			this.groupBox10.Controls.Add(this.bntProxyChange);
			this.groupBox10.Controls.Add(this.updownListenPort);
			this.groupBox10.Controls.Add(this.chkListenEverywhere);
			this.groupBox10.Controls.Add(this.label40);
			this.groupBox10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox10.Location = new System.Drawing.Point(0, 0);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Size = new System.Drawing.Size(143, 97);
			this.groupBox10.TabIndex = 74;
			this.groupBox10.TabStop = false;
			this.groupBox10.Text = "Proxy settings";
			// 
			// bntProxyChange
			// 
			this.bntProxyChange.BackColor = System.Drawing.Color.LightSlateGray;
			this.bntProxyChange.Location = new System.Drawing.Point(7, 66);
			this.bntProxyChange.Name = "bntProxyChange";
			this.bntProxyChange.Size = new System.Drawing.Size(130, 26);
			this.bntProxyChange.TabIndex = 30;
			this.bntProxyChange.Text = "Reload";
			this.bntProxyChange.Click += new System.EventHandler(this.bntProxyChange_Click);
			// 
			// updownListenPort
			// 
			this.updownListenPort.Location = new System.Drawing.Point(7, 44);
			this.updownListenPort.Maximum = new System.Decimal(new int[] {
																			 65535,
																			 0,
																			 0,
																			 0});
			this.updownListenPort.Minimum = new System.Decimal(new int[] {
																			 1025,
																			 0,
																			 0,
																			 0});
			this.updownListenPort.Name = "updownListenPort";
			this.updownListenPort.Size = new System.Drawing.Size(60, 18);
			this.updownListenPort.TabIndex = 29;
			this.updownListenPort.Value = new System.Decimal(new int[] {
																		   2002,
																		   0,
																		   0,
																		   0});
			// 
			// chkListenEverywhere
			// 
			this.chkListenEverywhere.BackColor = System.Drawing.Color.Transparent;
			this.chkListenEverywhere.Location = new System.Drawing.Point(7, 19);
			this.chkListenEverywhere.Name = "chkListenEverywhere";
			this.chkListenEverywhere.Size = new System.Drawing.Size(130, 23);
			this.chkListenEverywhere.TabIndex = 28;
			this.chkListenEverywhere.Text = "Listen on all interfaces";
			// 
			// label40
			// 
			this.label40.Location = new System.Drawing.Point(73, 48);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(60, 12);
			this.label40.TabIndex = 75;
			this.label40.Text = "Listen Port";
			// 
			// panel35
			// 
			this.panel35.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.panel35.Controls.Add(this.groupBox9);
			this.panel35.Location = new System.Drawing.Point(564, 0);
			this.panel35.Name = "panel35";
			this.panel35.Size = new System.Drawing.Size(120, 97);
			this.panel35.TabIndex = 74;
			// 
			// groupBox9
			// 
			this.groupBox9.Controls.Add(this.chkReplayIE);
			this.groupBox9.Controls.Add(this.chkReplayFireFox);
			this.groupBox9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox9.Location = new System.Drawing.Point(0, 0);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(120, 97);
			this.groupBox9.TabIndex = 73;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Replay Browser";
			this.toolTipSuru.SetToolTip(this.groupBox9, "To use FireFox the Mozilla ActiveX Control needs to be downloaded and installed");
			// 
			// chkReplayIE
			// 
			this.chkReplayIE.BackColor = System.Drawing.Color.Transparent;
			this.chkReplayIE.Checked = true;
			this.chkReplayIE.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkReplayIE.Location = new System.Drawing.Point(12, 56);
			this.chkReplayIE.Name = "chkReplayIE";
			this.chkReplayIE.Size = new System.Drawing.Size(80, 24);
			this.chkReplayIE.TabIndex = 27;
			this.chkReplayIE.Text = "Microsoft IE";
			// 
			// chkReplayFireFox
			// 
			this.chkReplayFireFox.BackColor = System.Drawing.Color.Transparent;
			this.chkReplayFireFox.Location = new System.Drawing.Point(12, 28);
			this.chkReplayFireFox.Name = "chkReplayFireFox";
			this.chkReplayFireFox.Size = new System.Drawing.Size(80, 23);
			this.chkReplayFireFox.TabIndex = 26;
			this.chkReplayFireFox.Text = "FireFox";
			this.chkReplayFireFox.CheckedChanged += new System.EventHandler(this.chkReplayFireFox_CheckedChanged);
			// 
			// panel34
			// 
			this.panel34.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.panel34.Controls.Add(this.grpSepChars);
			this.panel34.Location = new System.Drawing.Point(368, 0);
			this.panel34.Name = "panel34";
			this.panel34.Size = new System.Drawing.Size(194, 97);
			this.panel34.TabIndex = 73;
			// 
			// grpSepChars
			// 
			this.grpSepChars.Controls.Add(this.label36);
			this.grpSepChars.Controls.Add(this.label35);
			this.grpSepChars.Controls.Add(this.txtVariableSeparator);
			this.grpSepChars.Controls.Add(this.label31);
			this.grpSepChars.Controls.Add(this.label30);
			this.grpSepChars.Controls.Add(this.label32);
			this.grpSepChars.Controls.Add(this.txtBaseURLSeparator);
			this.grpSepChars.Controls.Add(this.txtKeyValueSeparator);
			this.grpSepChars.Controls.Add(this.label33);
			this.grpSepChars.Controls.Add(this.txtCookieVariableSeparator);
			this.grpSepChars.Controls.Add(this.txtCookieKeyValueSeparator);
			this.grpSepChars.Controls.Add(this.label34);
			this.grpSepChars.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpSepChars.Location = new System.Drawing.Point(0, 0);
			this.grpSepChars.Name = "grpSepChars";
			this.grpSepChars.Size = new System.Drawing.Size(194, 97);
			this.grpSepChars.TabIndex = 72;
			this.grpSepChars.TabStop = false;
			this.grpSepChars.Text = "Separator Chars";
			this.toolTipSuru.SetToolTip(this.grpSepChars, "Characters that seperate the Base URL from request parameters, variable from each" +
				" others and name, value pairs. Dont mess with this unless you know what you are " +
				"doing");
			// 
			// label36
			// 
			this.label36.ForeColor = System.Drawing.Color.AliceBlue;
			this.label36.Location = new System.Drawing.Point(100, 16);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(60, 12);
			this.label36.TabIndex = 68;
			this.label36.Text = "Cookies";
			// 
			// label35
			// 
			this.label35.ForeColor = System.Drawing.Color.AliceBlue;
			this.label35.Location = new System.Drawing.Point(7, 16);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(60, 12);
			this.label35.TabIndex = 67;
			this.label35.Text = "Parameters";
			// 
			// txtVariableSeparator
			// 
			this.txtVariableSeparator.Location = new System.Drawing.Point(77, 50);
			this.txtVariableSeparator.MaxLength = 1;
			this.txtVariableSeparator.Name = "txtVariableSeparator";
			this.txtVariableSeparator.Size = new System.Drawing.Size(16, 18);
			this.txtVariableSeparator.TabIndex = 22;
			this.txtVariableSeparator.Text = "&";
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(7, 50);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(66, 16);
			this.label31.TabIndex = 74;
			this.label31.Text = "Variable";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(7, 31);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(66, 16);
			this.label30.TabIndex = 73;
			this.label30.Text = "Base URL";
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(7, 69);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(66, 16);
			this.label32.TabIndex = 75;
			this.label32.Text = "Key/Value";
			// 
			// txtBaseURLSeparator
			// 
			this.txtBaseURLSeparator.Location = new System.Drawing.Point(77, 28);
			this.txtBaseURLSeparator.MaxLength = 1;
			this.txtBaseURLSeparator.Name = "txtBaseURLSeparator";
			this.txtBaseURLSeparator.Size = new System.Drawing.Size(16, 18);
			this.txtBaseURLSeparator.TabIndex = 21;
			this.txtBaseURLSeparator.Text = "?";
			// 
			// txtKeyValueSeparator
			// 
			this.txtKeyValueSeparator.Location = new System.Drawing.Point(77, 69);
			this.txtKeyValueSeparator.MaxLength = 1;
			this.txtKeyValueSeparator.Name = "txtKeyValueSeparator";
			this.txtKeyValueSeparator.Size = new System.Drawing.Size(16, 18);
			this.txtKeyValueSeparator.TabIndex = 23;
			this.txtKeyValueSeparator.Text = "=";
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(100, 31);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(67, 16);
			this.label33.TabIndex = 79;
			this.label33.Text = "Variable";
			// 
			// txtCookieVariableSeparator
			// 
			this.txtCookieVariableSeparator.Location = new System.Drawing.Point(170, 28);
			this.txtCookieVariableSeparator.MaxLength = 1;
			this.txtCookieVariableSeparator.Name = "txtCookieVariableSeparator";
			this.txtCookieVariableSeparator.Size = new System.Drawing.Size(16, 18);
			this.txtCookieVariableSeparator.TabIndex = 24;
			this.txtCookieVariableSeparator.Text = ";";
			// 
			// txtCookieKeyValueSeparator
			// 
			this.txtCookieKeyValueSeparator.Location = new System.Drawing.Point(170, 50);
			this.txtCookieKeyValueSeparator.MaxLength = 1;
			this.txtCookieKeyValueSeparator.Name = "txtCookieKeyValueSeparator";
			this.txtCookieKeyValueSeparator.Size = new System.Drawing.Size(16, 18);
			this.txtCookieKeyValueSeparator.TabIndex = 25;
			this.txtCookieKeyValueSeparator.Text = "=";
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(100, 50);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(67, 16);
			this.label34.TabIndex = 80;
			this.label34.Text = "Key/Value";
			// 
			// groupBox17
			// 
			this.groupBox17.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox17.Controls.Add(this.skinButton1);
			this.groupBox17.Controls.Add(this.updownTimeOut);
			this.groupBox17.Controls.Add(this.label29);
			this.groupBox17.Controls.Add(this.btnClearALL);
			this.groupBox17.Controls.Add(this.btnSaveData);
			this.groupBox17.Controls.Add(this.btnLoadData);
			this.groupBox17.Location = new System.Drawing.Point(0, 0);
			this.groupBox17.Name = "groupBox17";
			this.groupBox17.Size = new System.Drawing.Size(364, 97);
			this.groupBox17.TabIndex = 79;
			this.groupBox17.TabStop = false;
			this.groupBox17.Text = "| MISC |";
			// 
			// skinButton1
			// 
			this.skinButton1.BackColor = System.Drawing.Color.LightSlateGray;
			this.skinButton1.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.skinButton1.ForeColor = System.Drawing.Color.Black;
			this.skinButton1.Location = new System.Drawing.Point(192, 60);
			this.skinButton1.Name = "skinButton1";
			this.skinButton1.Size = new System.Drawing.Size(164, 24);
			this.skinButton1.TabIndex = 20;
			this.skinButton1.Text = "Remove those pesky tooltips";
			this.toolTipSuru.SetToolTip(this.skinButton1, "Dont listen to the button...I\'ll be good, I promise!");
			this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
			// 
			// updownTimeOut
			// 
			this.updownTimeOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.updownTimeOut.Increment = new System.Decimal(new int[] {
																			1000,
																			0,
																			0,
																			0});
			this.updownTimeOut.Location = new System.Drawing.Point(204, 20);
			this.updownTimeOut.Maximum = new System.Decimal(new int[] {
																		  100000,
																		  0,
																		  0,
																		  0});
			this.updownTimeOut.Minimum = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  0});
			this.updownTimeOut.Name = "updownTimeOut";
			this.updownTimeOut.Size = new System.Drawing.Size(148, 18);
			this.updownTimeOut.TabIndex = 19;
			this.updownTimeOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.toolTipSuru.SetToolTip(this.updownTimeOut, "Read/Write timeouts on raw TCP connections - including fuzzing");
			this.updownTimeOut.Value = new System.Decimal(new int[] {
																		8500,
																		0,
																		0,
																		0});
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(212, 40);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(132, 11);
			this.label29.TabIndex = 79;
			this.label29.Text = "Read/Write timeout (ms)";
			// 
			// btnClearALL
			// 
			this.btnClearALL.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnClearALL.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnClearALL.ForeColor = System.Drawing.Color.Firebrick;
			this.btnClearALL.Location = new System.Drawing.Point(8, 64);
			this.btnClearALL.Name = "btnClearALL";
			this.btnClearALL.Size = new System.Drawing.Size(64, 28);
			this.btnClearALL.TabIndex = 18;
			this.btnClearALL.Text = "Clear ALL";
			this.toolTipSuru.SetToolTip(this.btnClearALL, "Clears ALL information");
			this.btnClearALL.Click += new System.EventHandler(this.btnClearALL_Click);
			// 
			// btnSaveData
			// 
			this.btnSaveData.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnSaveData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSaveData.ForeColor = System.Drawing.Color.SteelBlue;
			this.btnSaveData.Location = new System.Drawing.Point(8, 16);
			this.btnSaveData.Name = "btnSaveData";
			this.btnSaveData.Size = new System.Drawing.Size(150, 24);
			this.btnSaveData.TabIndex = 16;
			this.btnSaveData.Text = "Save session";
			this.toolTipSuru.SetToolTip(this.btnSaveData, "Saves all request and discovered file and directories");
			this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
			// 
			// btnLoadData
			// 
			this.btnLoadData.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnLoadData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnLoadData.ForeColor = System.Drawing.Color.DarkGreen;
			this.btnLoadData.Location = new System.Drawing.Point(8, 40);
			this.btnLoadData.Name = "btnLoadData";
			this.btnLoadData.Size = new System.Drawing.Size(150, 24);
			this.btnLoadData.TabIndex = 17;
			this.btnLoadData.Text = "Load session";
			this.toolTipSuru.SetToolTip(this.btnLoadData, "Loads a previously saved session - requests will get added to current requests");
			this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.mnu_FILTER,
																						 this.menuItem2,
																						 this.menuItem3,
																						 this.menuItem4,
																						 this.mnu_RE});
			// 
			// mnu_FILTER
			// 
			this.mnu_FILTER.Index = 0;
			this.mnu_FILTER.Text = "[F] Filter";
			this.mnu_FILTER.Click += new System.EventHandler(this.mnu_FILTER_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnu_HLBrown,
																					  this.mnu_HLRed,
																					  this.mnu_HLOrange,
																					  this.mnu_HLYellow,
																					  this.mnu_HLGreen,
																					  this.mnu_HLAqua,
																					  this.mnu_HLBlue,
																					  this.mnu_HLPurple,
																					  this.mnu_HLGrey,
																					  this.mnu_HLBlack});
			this.menuItem3.Text = "[0-9] Highlight";
			// 
			// mnu_HLBrown
			// 
			this.mnu_HLBrown.Index = 0;
			this.mnu_HLBrown.Text = "[1] Brown";
			this.mnu_HLBrown.Click += new System.EventHandler(this.mnu_HLBrown_Click);
			// 
			// mnu_HLRed
			// 
			this.mnu_HLRed.Index = 1;
			this.mnu_HLRed.Text = "[2] Red";
			this.mnu_HLRed.Click += new System.EventHandler(this.mnu_HLRed_Click);
			// 
			// mnu_HLOrange
			// 
			this.mnu_HLOrange.Index = 2;
			this.mnu_HLOrange.Text = "[3] Orange";
			this.mnu_HLOrange.Click += new System.EventHandler(this.mnu_HLOrange_Click);
			// 
			// mnu_HLYellow
			// 
			this.mnu_HLYellow.Index = 3;
			this.mnu_HLYellow.Text = "[4] Yellow";
			this.mnu_HLYellow.Click += new System.EventHandler(this.mnu_HLYellow_Click);
			// 
			// mnu_HLGreen
			// 
			this.mnu_HLGreen.Index = 4;
			this.mnu_HLGreen.Text = "[5] Green";
			this.mnu_HLGreen.Click += new System.EventHandler(this.mnu_HLGreen_Click);
			// 
			// mnu_HLAqua
			// 
			this.mnu_HLAqua.Index = 5;
			this.mnu_HLAqua.Text = "[6] Cyan";
			this.mnu_HLAqua.Click += new System.EventHandler(this.mnu_HLAqua_Click);
			// 
			// mnu_HLBlue
			// 
			this.mnu_HLBlue.Index = 6;
			this.mnu_HLBlue.Text = "[7] Blue";
			this.mnu_HLBlue.Click += new System.EventHandler(this.mnu_HLBlue_Click);
			// 
			// mnu_HLPurple
			// 
			this.mnu_HLPurple.Index = 7;
			this.mnu_HLPurple.Text = "[8] Purple";
			this.mnu_HLPurple.Click += new System.EventHandler(this.mnu_HLPurple_Click);
			// 
			// mnu_HLGrey
			// 
			this.mnu_HLGrey.Index = 8;
			this.mnu_HLGrey.Text = "[9] Grey";
			this.mnu_HLGrey.Click += new System.EventHandler(this.mnu_HLGrey_Click);
			// 
			// mnu_HLBlack
			// 
			this.mnu_HLBlack.Index = 9;
			this.mnu_HLBlack.Text = "[0] None";
			this.mnu_HLBlack.Click += new System.EventHandler(this.mnu_HLBlack_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "-";
			// 
			// mnu_RE
			// 
			this.mnu_RE.Index = 4;
			this.mnu_RE.Text = "[Enter] Request Editor";
			this.mnu_RE.Click += new System.EventHandler(this.mnu_RE_Click);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// timer2
			// 
			this.timer2.Enabled = true;
			this.timer2.Interval = 1000;
			this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
			// 
			// statusBar1
			// 
			this.statusBar1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.statusBar1.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F);
			this.statusBar1.Location = new System.Drawing.Point(0, 0);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(846, 22);
			this.statusBar1.TabIndex = 1;
			this.statusBar1.Text = "Status";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.CheckFileExists = false;
			this.openFileDialog1.RestoreDirectory = true;
			// 
			// timer_mangled
			// 
			this.timer_mangled.Enabled = true;
			this.timer_mangled.Interval = 15000;
			this.timer_mangled.Tick += new System.EventHandler(this.timer_mangled_Tick);
			// 
			// toolTipSuru
			// 
			this.toolTipSuru.AutomaticDelay = 1000;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(846, 578);
			this.panel1.TabIndex = 2;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.tabControl1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(846, 556);
			this.panel3.TabIndex = 3;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.statusBar1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 556);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(846, 22);
			this.panel2.TabIndex = 2;
			// 
			// WebProxy
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 11);
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.Cornsilk;
			this.ClientSize = new System.Drawing.Size(846, 578);
			this.Controls.Add(this.panel1);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F);
			this.ForeColor = System.Drawing.Color.Black;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "WebProxy";
			this.Text = "SensePost Suru Web Proxy version 1.1";
			this.Resize += new System.EventHandler(this.WebProxy_Resize);
			this.tabControl1.ResumeLayout(false);
			this.Proxy.ResumeLayout(false);
			this.panel8.ResumeLayout(false);
			this.panel11.ResumeLayout(false);
			this.panel10.ResumeLayout(false);
			this.panel9.ResumeLayout(false);
			this.panel12.ResumeLayout(false);
			this.panel14.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.updownGroupTolerance)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.updowntwo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.updownCrowAI)).EndInit();
			this.grpInner.ResumeLayout(false);
			this.Misc.ResumeLayout(false);
			this.panel20.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.panel30.ResumeLayout(false);
			this.panel32.ResumeLayout(false);
			this.groupBox12.ResumeLayout(false);
			this.panel31.ResumeLayout(false);
			this.groupBox11.ResumeLayout(false);
			this.panel49.ResumeLayout(false);
			this.panel29.ResumeLayout(false);
			this.groupBox15.ResumeLayout(false);
			this.groupBox13.ResumeLayout(false);
			this.groupBox14.ResumeLayout(false);
			this.groupBox16.ResumeLayout(false);
			this.panel21.ResumeLayout(false);
			this.panel23.ResumeLayout(false);
			this.panel48.ResumeLayout(false);
			this.panel24.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.panel22.ResumeLayout(false);
			this.panel47.ResumeLayout(false);
			this.panel25.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.panel18.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.panel19.ResumeLayout(false);
			this.panel50.ResumeLayout(false);
			this.panel28.ResumeLayout(false);
			this.panel27.ResumeLayout(false);
			this.panel26.ResumeLayout(false);
			this.Recon.ResumeLayout(false);
			this.panel17.ResumeLayout(false);
			this.panel16.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.NUPDOWNBackEnd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trckReconSpeed)).EndInit();
			this.panel15.ResumeLayout(false);
			this.Config.ResumeLayout(false);
			this.panel38.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.panel46.ResumeLayout(false);
			this.panel45.ResumeLayout(false);
			this.panel44.ResumeLayout(false);
			this.panel37.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.panel43.ResumeLayout(false);
			this.groupBox22.ResumeLayout(false);
			this.panel42.ResumeLayout(false);
			this.groupBox21.ResumeLayout(false);
			this.panel41.ResumeLayout(false);
			this.groupBox20.ResumeLayout(false);
			this.panel40.ResumeLayout(false);
			this.groupBox19.ResumeLayout(false);
			this.panel39.ResumeLayout(false);
			this.groupBox18.ResumeLayout(false);
			this.panel33.ResumeLayout(false);
			this.panel36.ResumeLayout(false);
			this.groupBox10.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.updownListenPort)).EndInit();
			this.panel35.ResumeLayout(false);
			this.groupBox9.ResumeLayout(false);
			this.panel34.ResumeLayout(false);
			this.grpSepChars.ResumeLayout(false);
			this.groupBox17.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.updownTimeOut)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public static InternetExplorer ie;
		public AxMOZILLACONTROLLib.AxMozillaBrowser FF;

		public bool AmIUpdating;
		

		#region Widgets
		public System.Windows.Forms.TabControl tabControl1;
		public System.Windows.Forms.TabPage Proxy;
		public System.Windows.Forms.RichTextBox txtHTTPdetails;
		private System.Windows.Forms.TabPage Recon;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Timer timer2;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TextBox txtTargetHost;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtTargetPort;
		private System.Windows.Forms.Label label4;
		private DotNetSkin.SkinControls.SkinCheckBox chkTargetIsSSL;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.StatusBar statusBar1;
		public System.Windows.Forms.RichTextBox txtCrowResponse;
		private System.Windows.Forms.ListBox lstCrowResponse;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.TabPage Config;
		private System.Windows.Forms.Label label9;
		private DotNetSkin.SkinControls.SkinButton btnUpdateQuickNav;
		private System.Windows.Forms.RichTextBox txtWiktoTestDirs;
		private System.Windows.Forms.RichTextBox txtWiktoTestFilenames;
		private System.Windows.Forms.RichTextBox txtWiktoTestTypes;
		private DotNetSkin.SkinControls.SkinButtonYellow btnSaveFuzzFile;
		private System.Windows.Forms.NumericUpDown NUPDOWNBackEnd;
		private System.Windows.Forms.TrackBar trckReconSpeed;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label lblJobQLength;
		private System.Windows.Forms.Label label23;
		private DotNetSkin.SkinControls.SkinCheckBox chkDoRecon;
		private System.Windows.Forms.ComboBox cmbReconTargetHost;
		private DotNetSkin.SkinControls.SkinButton button1;
		private System.Windows.Forms.RichTextBox txtWiktoSkipDirs;
		private DotNetSkin.SkinControls.SkinButtonYellow btnClearJobQ;
		public System.Windows.Forms.TreeView treeRecon;
		private DotNetSkin.SkinControls.SkinCheckBox chkReconAlwaysExpand;
		private DotNetSkin.SkinControls.SkinButtonRed btnReconClearTree;
		private DotNetSkin.SkinControls.SkinButton btnReconTreeExpandAll;
		private DotNetSkin.SkinControls.SkinButton btnReconTreeCollapseAll;
		private DotNetSkin.SkinControls.SkinCheckBox ckhReconIndex;
		private DotNetSkin.SkinControls.SkinCheckBox chkReconDirMine;
		private System.Windows.Forms.ContextMenu cntReconTree;
		public  System.Windows.Forms.ListBox lstSRlist;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private DotNetSkin.SkinControls.SkinButton btnSRAdd;
		private DotNetSkin.SkinControls.SkinButton btnSRDelete;
		private System.Windows.Forms.RichTextBox txtSRSearch;
		private System.Windows.Forms.RichTextBox txtSRReplace;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private DotNetSkin.SkinControls.SkinButton btnSRDeleteOut;
		private DotNetSkin.SkinControls.SkinButton btnSRAddOut;
		private System.Windows.Forms.RichTextBox txtSRReplaceIn;
		private System.Windows.Forms.RichTextBox txtSRSearchIn;
		public System.Windows.Forms.ListBox lstSRIncoming;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.RichTextBox txtToolsUuserInput;
		private System.Windows.Forms.RichTextBox txtToolsMD5Sum;
		private System.Windows.Forms.RichTextBox txtToolsSHA1;
		private System.Windows.Forms.RichTextBox txtToolsBase64Encoded;
		private System.Windows.Forms.RichTextBox txtToolsBase64Decoded;
		private DotNetSkin.SkinControls.SkinButton btnToolsGo;
		private System.Windows.Forms.Timer timer_mangled;
		private System.Windows.Forms.RichTextBox txtAutoMangle;
		private DotNetSkin.SkinControls.SkinButton btnAnaliseMangle;
		private DotNetSkin.SkinControls.SkinCheckBox chkAutoMangle;
		private System.Windows.Forms.GroupBox groupBox6;
		public System.Windows.Forms.ListBox lstMangleUserInput;
		private System.Windows.Forms.RichTextBox txtMangleUserInput;
		private DotNetSkin.SkinControls.SkinButton btnMangleAddUserInput;
		private DotNetSkin.SkinControls.SkinButton btnMangleDeleteUserInput;
		private System.Windows.Forms.TabPage Misc;
		private System.Windows.Forms.RichTextBox txtReconSkipSites;
		private System.Windows.Forms.ComboBox cmbFuzzFileEdit;
		private System.Windows.Forms.TextBox txtConfigFuzzFileName;
		private System.Windows.Forms.RichTextBox txtConfigFuzzContent;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.GroupBox groupBox8;
		private DotNetSkin.SkinControls.SkinButton btnTreeReload;
		private System.Windows.Forms.GroupBox grpSepChars;
		private System.Windows.Forms.TextBox txtVariableSeparator;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.TextBox txtBaseURLSeparator;
		private System.Windows.Forms.TextBox txtKeyValueSeparator;
		private System.Windows.Forms.TextBox txtCookieKeyValueSeparator;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.TextBox txtCookieVariableSeparator;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label lblCurrentJobQ;
		private System.Windows.Forms.ToolTip toolTipSuru;
		private DotNetSkin.SkinControls.SkinButton btnFuzzDBLocationFind;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label txtFuzzDirLocation;
		private System.Windows.Forms.Label lblDateTime;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.GroupBox groupBox9;
		private DotNetSkin.SkinControls.SkinCheckBox chkReplayFireFox;
		private DotNetSkin.SkinControls.SkinCheckBox chkReplayIE;
		private System.Windows.Forms.GroupBox groupBox10;
		public DotNetSkin.SkinControls.SkinCheckBox chkListenEverywhere;
		private System.Windows.Forms.Label label40;
		public System.Windows.Forms.NumericUpDown updownListenPort;
		private DotNetSkin.SkinControls.SkinButton bntProxyChange;
		private System.Windows.Forms.GroupBox grpInner;
		private DotNetSkin.SkinControls.SkinButton btnManualBaseResponse;
		private System.Windows.Forms.ComboBox cmbCustom;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblFile1;
		private DotNetSkin.SkinControls.SkinButton btnFileLoad1;
		private DotNetSkin.SkinControls.SkinRadioButton radioFile1;
		private DotNetSkin.SkinControls.SkinRadioButton radioNumeric1;
		private System.Windows.Forms.TextBox txtNumericTo1;
		private System.Windows.Forms.TextBox txtNumericFrom1;
		private DotNetSkin.SkinControls.SkinButtonGreen btnCrowStart;
		private DotNetSkin.SkinControls.SkinButtonYellow btnCrowPause;
		private DotNetSkin.SkinControls.SkinButtonRed btnCrowStop;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private DotNetSkin.SkinControls.SkinRadioButton radioNotEqual;
		private DotNetSkin.SkinControls.SkinRadioButton radioAll;
		private DotNetSkin.SkinControls.SkinButton btnExport;
		private System.Windows.Forms.NumericUpDown updowntwo;
		private DotNetSkin.SkinControls.SkinRadioButton radioequal;
		private DotNetSkin.SkinControls.SkinRadioButton radiooutside;
		private DotNetSkin.SkinControls.SkinRadioButton radioinside;
		private DotNetSkin.SkinControls.SkinButton btnReCalc;
		private System.Windows.Forms.NumericUpDown updownCrowAI;
		private DotNetSkin.SkinControls.SkinCheckBox chkUseAIAtAll;
		private System.Windows.Forms.TextBox txtContentEndWord;
		private System.Windows.Forms.TextBox txtContentStartWord;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.RichTextBox txtToolsHex;
		
		#endregion

		#region globals
		#region proxy specific
		Proxy prx = new Proxy("");
		public static string ProxyListenPort="2002";
		public static bool ListenOnAll=false;
		//for pesky stuff
		bool insidetxtedit=true;
		bool insidetxteditRQ=true;
		public static bool EXITCLEANERNOW=false;
		#endregion

		#region for req editor

		ComboBox[] cookie_comboBoxes = new ComboBox[200];
		TextBox[] cookie_textBoxes = new TextBox[200];
		CheckBox[] cookie_chk = new CheckBox[200];
		int cookie_amount=0;

		ComboBox[] GET_comboBoxes = new ComboBox[200];
		TextBox[] GET_textBoxes = new TextBox[200];
		CheckBox[] GET_chk = new CheckBox[200];
		int GETparameters_amount=0;
		
		ComboBox[] POST_comboBoxes = new ComboBox[200];
		CheckBox[] POST_chk = new CheckBox[200];
		TextBox[] POST_textBoxes = new TextBox[200];
		int POSTparameters_amount=0;

		TextBox URL_textbox = new TextBox();
		TextBox action_textbox = new TextBox();
		TextBox header_textbox = new TextBox();

		//position
		int locX=0;
		int locY=0;
		#endregion

		#region for autogrouper
		RichTextBox[] MajorGroup = new RichTextBox[1000];
		Button[] GroupButtons = new Button[1000];
		Button[] GroupButtonsRaw = new Button[1000];
		#endregion

		#region ArrayLists
		ArrayList discovered_goods = new ArrayList();
		ArrayList detailed_Requests = new ArrayList();
		ArrayList userMange_detailed_Requests = new ArrayList();
		ArrayList JOBQ = new ArrayList();
		#endregion

		#region used for "cryptoCache"
		public static Hashtable MyCryptoCache = new Hashtable();
		public static int CurrentHTTPSport=0;
		#endregion

		#region for traps - to be used later
		public static bool Trap=true;
		public static byte[] TrapBuffer = new byte[16384];
		#endregion

		#region Crowbar Globals
		

		public struct CrowResponses{
			public string param1;
			public string param2;
			public string response;
			public double compare;
			public int ID;
			public string content;
			public string rawreq;
			public string comment;
		}
		public CrowResponses[] Cresponse = new CrowResponses[1000000];
		ArrayList CrowResponsesA = new ArrayList();
		
	
		static string baseResponse="";
		int rescount=0;
		bool stopit=false;
		bool pauseit=false;
		int howmany=0;
		long file1Pos=0;
		Int32 basenumber1=0;
		bool crow_running=false;

		#endregion 	
		
		#region used with reqeditor and listbox
		int jobstodo=0;
		int displayed_items=0;
		bool is_reqeditor_open=false;
		#endregion

		#region used to keep BlobDB
		Hashtable nikto_FP = new Hashtable();
		Hashtable backend_FP = new Hashtable();
		#endregion

		#region Recon tree Nodes
		//ExtendedTreeNode.Mode mode	= ExtendedTreeNode.Mode.one;
		Hashtable Reconnodes = new Hashtable();
		public enum Mode		{ one=1, two=2, three=3 };
		public enum Protocol	{ HTTP, HTTPS };

		#endregion

		#region globalcounter
		public static int globalcount=0;
		#endregion

		#region structures were we keep the recon data
		public static ArrayList dis_dirs;
		public static ArrayList dis_filenames;
		public static ArrayList dis_exts;

		public static ArrayList kn_dirs;
		public static ArrayList kn_filenames;
		public static ArrayList kn_exts;
		public static ArrayList kn_hosts;

		public static ArrayList Requests;
		public static ArrayList WorkRequests;
		public static ArrayList EditRequests = new ArrayList();
		#endregion

		#region Search and Replace structures
		public static ArrayList SRList = new ArrayList();
		public static ArrayList SRListIncoming = new ArrayList();
		#endregion

		#region Search on raw req
		public TextBox searchRawReq;
		public RichTextBox RawReqreply;
		public Label RawReqCountL;
		#endregion
		
		#region filter
		public static FilterStruct currentFilter = new FilterStruct();
		public static bool bypassfiltercompletely = false;
		public static bool anyPostorGet = false;
		
		#endregion
		#endregion

		reqEdit reqeditor;
		showresults rawsendwindow;
		AutoGroup autogroup;
		private DotNetSkin.SkinControls.SkinButton btnCrowGroup;
		private System.Windows.Forms.NumericUpDown updownGroupTolerance;
		private System.Windows.Forms.Label label42;
		private DotNetSkin.SkinControls.SkinButton btnSaveData;
		private DotNetSkin.SkinControls.SkinButton btnLoadData;
		private DotNetSkin.SkinControls.SkinButtonRed btnClearALL;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Splitter splitter3;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Panel panel9;
		private System.Windows.Forms.Panel panel10;
		private System.Windows.Forms.Panel panel11;
		private System.Windows.Forms.Panel panel12;
		private System.Windows.Forms.Panel panel13;
		private System.Windows.Forms.Panel panel14;
		private System.Windows.Forms.Panel panel15;
		private System.Windows.Forms.Panel panel16;
		private System.Windows.Forms.Panel panel17;
		private System.Windows.Forms.Panel panel18;
		private System.Windows.Forms.Panel panel19;
		private System.Windows.Forms.Splitter splitter5;
		private System.Windows.Forms.Panel panel20;
		private System.Windows.Forms.Panel panel21;
		private System.Windows.Forms.Panel panel22;
		private System.Windows.Forms.Splitter splitter6;
		private System.Windows.Forms.Panel panel23;
		private System.Windows.Forms.Splitter splitter7;
		private System.Windows.Forms.Panel panel26;
		private System.Windows.Forms.Panel panel27;
		private System.Windows.Forms.Panel panel28;
		private System.Windows.Forms.Panel panel29;
		private System.Windows.Forms.Panel panel30;
		private System.Windows.Forms.GroupBox groupBox11;
		private System.Windows.Forms.GroupBox groupBox12;
		private System.Windows.Forms.Panel panel31;
		private System.Windows.Forms.Splitter splitter8;
		private System.Windows.Forms.Panel panel32;
		private System.Windows.Forms.GroupBox groupBox13;
		private System.Windows.Forms.GroupBox groupBox14;
		private System.Windows.Forms.GroupBox groupBox15;
		private System.Windows.Forms.GroupBox groupBox16;
		private System.Windows.Forms.Panel panel33;
		private System.Windows.Forms.Panel panel34;
		private System.Windows.Forms.Panel panel35;
		private System.Windows.Forms.Panel panel36;
		private System.Windows.Forms.GroupBox groupBox17;
		private System.Windows.Forms.Panel panel37;
		private System.Windows.Forms.Splitter splitter10;
		private System.Windows.Forms.Panel panel38;
		private System.Windows.Forms.GroupBox groupBox18;
		private System.Windows.Forms.GroupBox groupBox19;
		private System.Windows.Forms.GroupBox groupBox20;
		private System.Windows.Forms.GroupBox groupBox21;
		private System.Windows.Forms.GroupBox groupBox22;
		private System.Windows.Forms.Panel panel39;
		private System.Windows.Forms.Splitter splitter11;
		private System.Windows.Forms.Panel panel40;
		private System.Windows.Forms.Splitter splitter12;
		private System.Windows.Forms.Panel panel41;
		private System.Windows.Forms.Splitter splitter13;
		private System.Windows.Forms.Panel panel42;
		private System.Windows.Forms.Splitter splitter14;
		private System.Windows.Forms.Panel panel43;
		private System.Windows.Forms.Panel panel44;
		private System.Windows.Forms.Panel panel45;
		private System.Windows.Forms.Panel panel46;
		private System.Windows.Forms.Panel panel24;
		private System.Windows.Forms.Panel panel25;
		private System.Windows.Forms.Panel panel47;
		private System.Windows.Forms.Panel panel48;
		private System.Windows.Forms.Splitter splitter16;
		private System.Windows.Forms.Panel panel49;
		private System.Windows.Forms.NumericUpDown updownTimeOut;
		private System.Windows.Forms.Label label29;
		private DotNetSkin.SkinControls.SkinButtonYellow btnBackEndUpdateDirs;
		private System.Windows.Forms.ComboBox cmbBackEndUpdate;
		private DotNetSkin.SkinControls.SkinCheckBox chkSmartFileShallow;
		private DotNetSkin.SkinControls.SkinCheckBox chkSmartFileDeep;
		private DotNetSkin.SkinControls.SkinCheckBox chkSmartDirScan;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private DotNetSkin.SkinControls.SkinButton btnReplay;
		private DotNetSkin.SkinControls.SkinButton btnSendRawRequest;
		private DotNetSkin.SkinControls.SkinCheckBox chkProxyAutoUpdate;
		private DotNetSkin.SkinControls.SkinCheckBox chkImmFuzz;
		public DotNetSkin.SkinControls.SkinButtonYellow button2;
		private DotNetSkin.SkinControls.SkinCheckBox chkShowHosts;
		private DotNetSkin.SkinControls.SkinButtonRed btnClear;
		private System.Windows.Forms.Panel panel50;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ProgressBar prgBarCrow;
		private DotNetSkin.SkinControls.SkinCheckBox chkCrowStoreResponse;
		private DotNetSkin.SkinControls.SkinButton skinButton1;
		private System.Windows.Forms.ComboBox comboCrowEncode;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnu_FILTER;
		private System.Windows.Forms.MenuItem mnu_RE;
		private System.Windows.Forms.MenuItem mnu_HLBrown;
		private System.Windows.Forms.MenuItem mnu_HLRed;
		private System.Windows.Forms.MenuItem mnu_HLOrange;
		private System.Windows.Forms.MenuItem mnu_HLYellow;
		private System.Windows.Forms.MenuItem mnu_HLGreen;
		private System.Windows.Forms.MenuItem mnu_HLAqua;
		private System.Windows.Forms.MenuItem mnu_HLBlue;
		private System.Windows.Forms.MenuItem mnu_HLPurple;
		private System.Windows.Forms.MenuItem mnu_HLGrey;
		private System.Windows.Forms.MenuItem mnu_HLBlack;
		private System.Windows.Forms.ContextMenu contextMenu2;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private int MouseX;
		private int MouseY;
		

		object isnull=null;

		public WebProxy() {
			
			InitializeComponent();

			#region Startup
			try{

				/*#region setting up Recon tree & crowbar context menus
				contextMenu2.MenuItems.Add("Only these",new System.EventHandler(this.contextEqual));
				contextMenu2.MenuItems.Add("All but these",new System.EventHandler(this.contextOutside));
				contextMenu2.MenuItems.Add("-");
				contextMenu2.MenuItems.Add("Show reply", new System.EventHandler(this.contextshowContent));
				contextMenu2.MenuItems.Add("Browse reply", new System.EventHandler(this.contextbrowseContent));
				//########################### NEW CHANGES
				cntReconTree.MenuItems.Add("Scan this directory/site for directories",new System.EventHandler(this.AddDirectoryForDirectoryscan));
				cntReconTree.MenuItems.Add("Scan this directory for files",new System.EventHandler(this.AddDirectoryForFilescan));
				cntReconTree.MenuItems.Add("-");
				cntReconTree.MenuItems.Add("Expand node",new System.EventHandler(this.expandTreeNode));
				cntReconTree.MenuItems.Add("Prune node",new System.EventHandler(this.pruneTreeNode));
				cntReconTree.MenuItems.Add("Delete node",new System.EventHandler(this.deleteTreeNode));
				cntReconTree.MenuItems.Add("-");
				cntReconTree.MenuItems.Add("Clear queue for this host",new System.EventHandler(this.clearQonehost));
				cntReconTree.MenuItems.Add("-");
				cntReconTree.MenuItems.Add("Navigate here now",new System.EventHandler(this.TreeNavigate));
						
			
			
				#endregion*/
			
				#region getting files for the FuzzDB
				QuickNavUpdate(txtFuzzDirLocation.Text,"*.txt");
				#endregion

				#region init of ArrayLists
				Requests = new ArrayList();
			
				kn_dirs = new ArrayList();
				kn_exts = new ArrayList();
				kn_filenames = new ArrayList();
				kn_hosts=new ArrayList();
				#endregion

				#region populating the config menu
				txtWiktoTestDirs.Text+="data\r\npublic\r\nadmin\r\nbackup\r\nlogin\r\ntest\r\napps\r\nconfig\r\ndata\r\ndatabase\r\ndb\r\ndownloads\r\nupload\r\nuploads\r\nfile\r\nfiles\r\nhome\r\nuser\r\nusers\r\nlogs\r\nlog\r\nmisc\r\nold\r\nprivate\r\nsecure\r\nsite\r\nstats\r\ntemp\r\n_vti_pvt";
				txtWiktoSkipDirs.Text+="images\r\nImages\r\ninclude\r\nlib\r\ncss\r\npics\r\nPics";
				txtReconSkipSites.Text+="google\r\nmsn\r\nsecway\r\ngmail\r\nsensepost\r\nwindowsupdate.com\r\nmicrosoft\r\nadobe\r\n.gov\r\n.mil";
				txtWiktoTestFilenames.Text="admin\r\nbackend\r\nbackup\r\ncmd\r\ndata\r\ndatabase\r\nleft\r\nlog\r\nlogfile\r\nlogfiles\r\nlogin\r\nlogon\r\nlogs\r\nmain\r\nmembers\r\npassword\r\npasswords\r\nroot\r\nsensepost\r\nstats\r\ntest\r\ntrace\r\nupload\r\nuploader\r\nuploads";
				txtWiktoTestTypes.Text="pl\r\nphp\r\nsh\r\npy\r\ntgz\r\ntar\r\ntar.gz\r\nasp\r\naspx\r\ndoc\r\nexe\r\ncmd\r\ncfm\r\nnsf\r\nhtm\r\nhtml\r\ntxt\r\ntmp\r\nold\r\nbak\r\nbackup\r\nlog\r\nzip\r\nsql\r\nxml\r\nasmx\r\nashx\r\naxd\r\nwsdl";
				#endregion

				#region filter init
				WebProxy.currentFilter.Actions=new ArrayList();
				WebProxy.currentFilter.Hosts = new ArrayList();
				WebProxy.currentFilter.Locations = new ArrayList();
				WebProxy.currentFilter.Parameters = new ArrayList();
				WebProxy.currentFilter.Ext=new ArrayList();
				string[] boos = {"css","gif","jpg","ico","png","js"};
				foreach (string boo in boos){
					WebProxy.currentFilter.Ext.Add(boo);
				}
				WebProxy.currentFilter.Cookies = new ArrayList();
				WebProxy.currentFilter.RequestHeader = new ArrayList();
				WebProxy.currentFilter.ResponseHeader = new ArrayList();

				
				WebProxy.currentFilter.inActions=false;
				WebProxy.currentFilter.inParameters=false;
				WebProxy.currentFilter.inHost=false;
				WebProxy.currentFilter.inLocations=false;
				WebProxy.currentFilter.inExt=true;
				WebProxy.currentFilter.inCookies=false;
				WebProxy.currentFilter.inRequests=false;
				WebProxy.currentFilter.inResponse=false;

				WebProxy.currentFilter.IsHTTP=true;
				WebProxy.currentFilter.IsHTTPS=true;
				#endregion

				//this.Text		= string.Format("{0} {1} v{2}", Application.CompanyName, Application.ProductName, Application.ProductVersion);
				BeginLongTaskThread();
			} catch{}
			#endregion
		}

		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		[STAThread]
		static void Main() {

			// Run the disclaimer before we do anything
			Org.Mentalis.Proxy.Disclaimer disclaimer	= new Org.Mentalis.Proxy.Disclaimer();
			if ( disclaimer.ShowDialog() != DialogResult.OK )	
			{
				Application.Exit();
				return;
			}

			try{
				Application.Run(new WebProxy());
			} catch(Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}


		#region -=-> common utils

		private string getExtFromURL(string URL){
			string[] parts = URL.Split('.');
			return parts[parts.Length-1];
			
		}
		private string convertToHex(string query,bool skip){
			string work=query;
			string returner="";
			char[] letters = work.ToCharArray();
			char[] dont_encode_normal ={'=','&','\\','.','%','+','_','-'};
			char[] dont_encode_skip ={'&','\\','.','%','_','-'};
			char[] dont_encode= new char[20];
			if (skip==true){
				dont_encode = dont_encode_skip;
			} else {
				dont_encode = dont_encode_normal;
			}

			foreach (char letter in letters){
				if (Char.IsLetterOrDigit(letter)==false && Array.IndexOf(dont_encode,letter)<0){
					int ord=(int)letter;
					string hexletter=String.Format("%{0:X}",ord);
					returner+=hexletter;
				} else {
					returner+=letter;
				}
			}
			return returner;
		}

		private string convertToHex_for_real(string query){
			
			char[] letters = query.ToCharArray();
			string returner=string.Empty;
			
			foreach (char letter in letters){
					int ord=(int)letter;
					string hexletter=String.Format("%{0:X}",ord);
					returner+=hexletter;
			}
			return returner;
		}

		private string convertFromHex_for_real(string query){
			return "";
		}

		private string convertFromHex(string query){
			string work=query.Replace("%28", "(").Replace("%3D","=").Replace("%22","\"").Replace("%24","$").
				Replace("%3A", ":").Replace("%21", "!").Replace("%3F", "?").Replace("%2D","-").Replace("%3C","<").Replace("%3E",">").
				Replace("%29", ")").Replace("%40", "@").Replace("%26", "&").Replace("%5F","_").Replace("%3B",";").Replace("%2A","*").
				Replace("%7C", "|").Replace("%2B", "+").Replace("%5B", "[").Replace("%7E","~").Replace("%7B","{").Replace("%7D","}").
				Replace("%2C", ",").Replace("%20", " ").Replace("%23", "#").Replace("%2E",".").Replace("%5C","\\").
				Replace("%2F", "/").Replace("%5D", "]").Replace("%27", "'").Replace("%25", "%");

			work=work.Replace("%28", "(").Replace("%3d","=").Replace("%22","\"").Replace("%24","$").
				Replace("%3a", ":").Replace("%21", "!").Replace("%3f", "?").Replace("%2d","-").Replace("%3c","<").Replace("%3e",">").
				Replace("%29", ")").Replace("%40", "@").Replace("%26", "&").Replace("%5f","_").Replace("%3b",";").Replace("%2a","*").
				Replace("%7c", "|").Replace("%2b", "+").Replace("%5b", "[").Replace("%2e",".").Replace("%7b","{").Replace("%7d","}").
				Replace("%2c", ",").Replace("%20", " ").Replace("%23", "#").Replace("%7e","~").Replace("%5c","\\").
				Replace("%2f", "/").Replace("%5d", "]").Replace("%27", "'").Replace("%25", "%");

			return work;

		}

		private void cleanHTTP(){
			
			string temp=clean_partial_header(txtHTTPdetails.Text);
			if (temp.Substring(0,3).CompareTo("GET")==0){
				for (int y=1; y<5; y++){
					temp=temp.TrimEnd('\n');
					temp=temp.TrimEnd('\r');
					temp=temp.TrimStart('\n');
					temp=temp.TrimStart('\r');
					
				}
				txtHTTPdetails.Clear();
				if (temp.IndexOf("Connection:")>0){
					temp=temp.Replace("Connection: keep-alive","Connection: close");
					temp=temp.Replace("Connection: Keep-Alive","Connection: close");
					temp=temp.Replace("Connection: Keep-alive","Connection: close");
				}
				if (temp.IndexOf("Connection: close")<0) {
					temp+="\r\nConnection: close\r\n";
				}
				string[] lines = temp.Replace("\r\n","\n").Split('\n');
				foreach (string line in lines){
					if (line.StartsWith("Keep-Alive")==false && line.StartsWith("Keep-alive")==false){
						txtHTTPdetails.Text+=line.TrimEnd(' ')+"\r\n";
					}
				}
				
			} else {
				//its a POST!!!
				string postheader="";
				string body="";
				bool record=true;
				string[] lines = clean_partial_header(txtHTTPdetails.Text).Replace("\r\n","\n").Split('\n');
				foreach (string line in lines){
					if (line.Length>0 && record){
						postheader+=line+"\r\n";
					} else {
						record =false;
					}
					if (record == false){
						body+=line+"\r\n";
					}
				}
				//work on post header
				temp=postheader;
				for (int y=1; y<5; y++){
					temp=temp.TrimEnd('\n','\r');
				
				}
				txtHTTPdetails.Clear();
				if (temp.IndexOf("Connection:")>0){
					temp=temp.Replace("Connection: keep-alive","Connection: close");
					temp=temp.Replace("Connection: Keep-Alive","Connection: close");
					temp=temp.Replace("Connection: Keep-alive","Connection: close");
				}
				if (temp.IndexOf("Connection: close")<0){
					temp+="\r\nConnection: close";
				}
				string[] plines = temp.Replace("\r\n","\n").Split('\n');
				foreach (string line in plines){
					if (line.StartsWith("Keep-Alive")==false && line.StartsWith("Keep-alive")==false){
						txtHTTPdetails.Text+=line.TrimEnd(' ')+"\r\n";
					}
				}
				//now add the body
				for (int y=1; y<5; y++){
					body=body.TrimEnd('\r');
					body=body.TrimEnd('\n');
					body=body.TrimStart('\r');
					body=body.TrimStart('\n');
				}
				txtHTTPdetails.Text+="\r\n"+body;
			}
			

		}

		private bool SetCookie(string url, string name, string valu)	{
			valu += "; expires = Mon, 16-Jul-2037 00:00:00GMT";
			return InternetSetCookie(url, name, valu);
		}

		//---this is used everwhere..
		public string sendraw (string ipRaw, string portRaw, string payloadRaw, int size, int TimeOut) {
			int retry=1;

			while (retry>0){
				try {
					
					TcpClient tcpclnt= new TcpClient();	
					tcpclnt.ReceiveTimeout=TimeOut;
					tcpclnt.SendTimeout=TimeOut;
					tcpclnt.LingerState.LingerTime=1;
					LingerOption lingerOption = new LingerOption(false, 1);
					tcpclnt.LingerState = lingerOption;
					tcpclnt.NoDelay=true;

					tcpclnt.Connect(ipRaw,Int32.Parse(portRaw)); 
				
					Stream stm = tcpclnt.GetStream();
					ASCIIEncoding asen= new ASCIIEncoding();
					byte[] ba=asen.GetBytes(payloadRaw);
					stm.Write(ba,0,ba.Length);
		
					byte[] bb=new byte[size];

					int k=1;
					string response="";
					while (k >0){
						k=stm.Read(bb,0,size);		
						response+=Encoding.Default.GetString(bb,0,k);
					}

					tcpclnt.Close();
					//this is need else we get CLOSE_WAITS - not nice..but works well
					GC.Collect();
					GC.WaitForPendingFinalizers();

					return response;
				}
       
				catch {
					
					retry--;
					Thread.Sleep(1000);
				}
			}
			
			return "Timeout or retry count exceeded";
		}

		//sendraw SSL
		public string sendraw (string ipRaw, string portRaw, string payloadRaw, int size, int TimeOut, int retry, bool useSSL) {
			
			IPHostEntry IPHost = Dns.Resolve(ipRaw); 
			string []aliases = IPHost.Aliases; 
			IPAddress[] addr = IPHost.AddressList; 
			IPEndPoint iep ;
			SecureProtocol sp;
			sp=SecureProtocol.Ssl3;
			SecureSocket s=null; 
			SecurityOptions options = new SecurityOptions(sp);
			options.Certificate = null;
			options.Protocol=SecureProtocol.Ssl3;
			options.Entity = ConnectionEnd.Client;
			options.CommonName = ipRaw;
			options.VerificationType = CredentialVerification.None;
			options.Flags = SecurityFlags.IgnoreMaxProtocol;
			options.AllowedAlgorithms = SslAlgorithms.ALL;

			while (retry>0){
				try {
				
					s = new SecureSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, options);
					s.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 4000);
					s.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 4000);
			
					iep	= new IPEndPoint(addr[0],Convert.ToInt32(portRaw));
					s.Connect(iep);

					ASCIIEncoding asen= new ASCIIEncoding();
					byte[] ba=asen.GetBytes(payloadRaw);
					s.Send(ba,ba.Length,System.Net.Sockets.SocketFlags.None);
					

					byte[] bb=new byte[size];
					
					int k=1;
					string response="";
					while (k>0){
						k=s.Receive(bb,size,System.Net.Sockets.SocketFlags.None);					
						response+=Encoding.Default.GetString(bb,0,k);
					}
					s.Close();
					GC.Collect();
					GC.WaitForPendingFinalizers();

					return response;

				}
				catch(Exception ex) {
					retry--;
					Thread.Sleep(1000);
				}
			}
			
			return "Timeout or retry count exceeded";
		}

		private string clean_partial_header(string header){
			string returner="";
			string[] badthings = {"Proxy-Connection:","Keep-Alive","Keep-alive","Connection:","connection:","If-Modified-Since:","If-None-Match:","Accept-Encoding:"};
			ArrayList BadThings = new ArrayList();
			BadThings.AddRange(badthings);
			string[] lines = header.Replace("\r\n","\n").Split('\n');
			foreach (string line in lines){
				string[] thingsinline = line.Split(' ');
				ArrayList Thingsinline = new ArrayList();
				Thingsinline.AddRange(thingsinline);
				bool flag=false;
				foreach (string partlines in Thingsinline){
					if (BadThings.Contains(partlines)){
						flag=true;
						break;
					}
				}
				if (!flag){
					returner+=line+"\r\n";
				}
			}
			returner.TrimEnd('\r','\n');
			return returner;
		}

		private string extractFileType (string request){
			
			string[] header=new string[15];
			header=request.Split('\n');
			try{
				string[] types=new string[20];
				types=header[0].Split(txtBaseURLSeparator.Text[0]);
			
				string[] dot=new string[20];
				dot=types[0].Split('.');
				if (dot[dot.Length-1].Length > 15){
					return "default";
				}
				else return dot[dot.Length-1];
			}
			catch {
				return "default";
			}
			
		}

		private string extractLocation (string request){
			
			string[] header=new string[5];
			header=request.Split('\n');
			header[0]=header[0].Replace("..","||");

			string[] directories=new string[20];
			
			directories=header[0].Split('?','!','#','$','^','&','\\','*',',');

			string[] dirparts = new string[2000];
			dirparts=directories[0].Split('/');
			string returner="";

			int endplace=2;
			try{
				if (directories[0].ToCharArray()[directories[0].Length-1].CompareTo('/')==0){
					endplace=3;
				}
			} catch {}


			for (int i=0; i <= dirparts.Length-endplace; i++){
				returner+=dirparts[i]+"/";
			}
			returner=returner.Replace("||","..");
			return returner;
			
		}

		private string ComputeNewContentLength (string header){
			
			string[] partsheader = new string[50000];
			string newheader="";	

			//GET - let's just \r\n everything
			if (header.Substring(0,3).CompareTo("GET") ==0){
				partsheader=header.Replace("\r","").Split('\n');		
				for (int l=0; l<partsheader.Length; l++){
					newheader+=partsheader[l]+"\r\n";
				}
				
				return newheader;
			}
			//ok its a POST
			partsheader=header.Replace("\r","").Split('\n');
			int i=0;
			for (i=0; i<partsheader.Length; i++){
				if (partsheader[i].Length==0){
					break;
				}
			}

			//ok we need to be careful when calculating the content length...
			//its everything that follows...
			string postbody="";
			int newlines=0;
			for (int j=i+1; j<partsheader.Length; j++){
				newlines++;
				postbody+=partsheader[j];
			}


			//string poststring=partsheader[i+1];
			int contentlength=postbody.Length;
			// all the \rs
			if (newlines > 1){
				contentlength+=(2*newlines);
			}

			//reconstruct the header
			for (i=0; i<partsheader.Length; i++){
				if (partsheader[i].IndexOf("Content-Length")>=0 || partsheader[i].IndexOf("Content-length")>=0){
					newheader+="Content-Length: "+contentlength.ToString()+"\r\n";
				}
				else {
					newheader+=partsheader[i]+"\r\n";
				}
			}
			return newheader;
		}

		public static string crop_header(string withheader){
			//lock (WebProxy){
				int startbody=withheader.IndexOf("\r\n\r\n");
				if (startbody==-1) startbody=0;
				string returner=withheader.Substring(startbody);
				return returner;
			//}
		}

		//overload used in MPP
		public static string crop_header(string withheader,bool o){
			//lock (WebProxy){
			int startbody=withheader.IndexOf("\r\n\r\n");
			if (startbody==-1) return "";
			string returner=withheader.Substring(startbody);
			return returner;
			//}
		}
		public double compareBlobs (string blobA, string blobB,bool bypass,bool old){
			if (blobA.Equals("Timeout or retry count exceeded")||blobB.Equals("Timeout or retry count exceeded")){
				return -1;
			}
			string cA=crop_header(blobA);
			string cB=crop_header(blobB);

			if (cA.Equals(cB) || bypass){
				return 1;
			}

			char[] splits={' ','>','\n'};
			string[] wordsblobA = new string[cA.Split(splits).Length];
			string[] wordsblobB = new string[cB.Split(splits).Length];
			
			wordsblobA=cA.Split(splits);
			wordsblobB=cB.Split(splits);
			
			int matchcount=0;
			int blanks=0;
			foreach (string wordA in wordsblobA){
				if (wordA.Length==0){
					blanks++;
				}
			}
			foreach (string wordB in wordsblobB){
				if (wordB.Length==0){
					blanks++;
				}
			}
			foreach (string wordA in wordsblobA){
				foreach (string wordB in wordsblobB){
					if (wordA.CompareTo(wordB)==0 && wordB.Length>0 && wordA.Length>0){
						matchcount++;
						break;
					}	
				}

			}
			double sum=((double)wordsblobA.Length+(double)wordsblobB.Length)-blanks;
			return (Math.Round( (double)(matchcount*2.0)/sum ,5));
		}
		
		 	
		#region used for setting the cookies
		[DllImport("wininet.dll", CharSet=CharSet.Auto, SetLastError=true)]
		public static extern bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData);
		#endregion
		#endregion

		#region ====> PROXY traditional

		#region Main Proxy Thread
		public delegate void DelegateToLongTask();
		public void BeginLongTaskThread() {
			DelegateToLongTask delLongTaskDelegate= new DelegateToLongTask(KickProxy);
			AsyncCallback callBackWhenDone = new AsyncCallback(this.EndLongTaskThread);
			delLongTaskDelegate.BeginInvoke(callBackWhenDone,null);
		}
			
		public void EndLongTaskThread(IAsyncResult arResult) {}
		

		private void KickProxy (){
			try {
				string dir = Environment.CurrentDirectory;
				if (!dir.Substring(dir.Length - 1, 1).Equals(@"\"))
					dir += @"\";
				
				prx.Start();
			} catch {
				//Console.WriteLine("The program ended abnormally!");
			}
		}
		#endregion

		#region click update URLs from proxy
		private void button2_Click(object sender, System.EventArgs e) 
		{
			UpdateListViewControl();
		}

		private void UpdateListViewControl()
		{
			listView1.Items.Clear();
			txtHTTPdetails.Clear();
			EditRequests.Clear();
			detailed_Requests.Clear();

			try
			{
				if (Requests.Count <1){
					return;
				}
			} catch{
				return;
			}
			WorkRequests = new ArrayList();
			lock (Requests){
				foreach (HTTPRequest test in Requests){	
					WorkRequests.Add(test);
				}
			
			}

			//display
			foreach (HTTPRequest item in WorkRequests){
				string display=string.Empty;
				bool isHighlighted = false;
				int isColour = 0;
				detailedRequest work =getHTTPdetails(item.header,item.host,item.isSSL);
				detailed_Requests.Add(work);			
				bool badextflag=true;
				badextflag=ApplyBigFilter(work,item);

				if (badextflag==false){
					display+=item.reqnum.ToString()+" ";

					try{
						txtCrowResponse.Text=item.response;
						char [] tosplit={' ','\r'};
						string[] response_parts=item.response.Split(tosplit);
							display+=response_parts[1];
					} catch{}
			
					if (item.header.IndexOf("SensePost-SuruWP: edited request")>=0){
						display+="=";
					} else {display+=" ";}
				
					string proto="";
					if (item.isSSL){
						display+="# ";
						proto="https://";
					} else {
						display+="+ ";
						proto="http://";
					}
			
					if (work.isXML){
						display+="X";
					} 
					if (work.isMultiPart){
						display+="MP";
					}
				
					if (work.action.Equals("GET")==true){
						display+="G ";
					}
					if (work.action.Equals("POST")==true){
						display+="P ";
					}

					switch (item.isColour)
					{
						case 0:
							isHighlighted = false;
							isColour = 0;
							break;
						case 1:
							isHighlighted = true;
							isColour = 1;
							break;
						case 2:
							isHighlighted = true;
							isColour = 2;
							break;
						case 3:
							isHighlighted = true;
							isColour = 3;
							break;
						case 4:
							isHighlighted = true;
							isColour = 4;
							break;
						case 5:
							isHighlighted = true;
							isColour = 5;
							break;
						case 6:
							isHighlighted = true;
							isColour = 6;
							break;
						case 7:
							isHighlighted = true;
							isColour = 7;
							break;
						case 8:
							isHighlighted = true;
							isColour = 8;
							break;
						case 9:
							isHighlighted = true;
							isColour = 9;
							break;
						default:
							isColour = 0;
							isColour = 0;
							isHighlighted = false;
							break;
					}
				
					display+=work.GETparameters.Count.ToString()+" ";
					display+=work.POSTparameters.Count.ToString()+" ";
					if (chkShowHosts.Checked){
						display+=work.host+" ";
					}
					display+=work.URL;
				
					if (work.URL.Length>0){
						do_Recon(work);
					}else{
						MessageBox.Show("NULL URL click");
					}

					//add the tag to the tree entry
					TreeTagType tag = new TreeTagType();
					tag.isSSL=work.isSSL;
					tag.port=work.port;
					tag.header=work.header;
					AddURLToTreeView(proto+work.host+work.URL,1, Reconnodes, tag);
					EditRequests.Add(item);
				}
				if (display.Length>4){
					listView1.Items.Add(display);
					ListViewItem z = listView1.Items[listView1.Items.Count-1];
					switch (isColour)
					{
						case 0:
							z.Font = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
							z.ForeColor = System.Drawing.Color.Black;
							break;
						case 1:
							z.Font = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
							z.ForeColor = System.Drawing.Color.Brown;
							break;
						case 2:
							z.Font = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
							z.ForeColor = System.Drawing.Color.Red;
							break;
						case 3:
							z.Font = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
							z.ForeColor = System.Drawing.Color.Orange;
							break;
						case 4:
							z.Font = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
							z.ForeColor = System.Drawing.Color.DarkKhaki;
							break;
						case 5:
							z.Font = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
							z.ForeColor = System.Drawing.Color.Green;
							break;
						case 6:
							z.Font = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
							z.ForeColor = System.Drawing.Color.Cyan;
							break;
						case 7:
							z.Font = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
							z.ForeColor = System.Drawing.Color.Blue;
							break;
						case 8:
							z.Font = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
							z.ForeColor = System.Drawing.Color.Purple;
							break;
						case 9:
							z.Font = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
							z.ForeColor = System.Drawing.Color.DarkGray;
							break;
						default:
							z.Font = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
							z.ForeColor = System.Drawing.Color.Black;
							break;
					}
					listView1.Items[listView1.Items.Count-1].Selected = true;
				}
			}			
		}
		#endregion

		#region filter
		private bool ApplyBigFilter(detailedRequest passed, HTTPRequest Hpassed){

			if (bypassfiltercompletely){
				return false;
			}
			//assume its good
			bool matchfilterh = false;
			
			//compare hosts
			if (currentFilter.Hosts.Count>0){
				foreach (string item in currentFilter.Hosts){
					if (passed.host.IndexOf(item)>=0){
						matchfilterh=true;
						break;
					}
				}
			} else {matchfilterh=true;}

			//compare locations
			bool matchfilterl = false;
			if (currentFilter.Locations.Count>0){
				foreach (string item in currentFilter.Locations){
					if (passed.URL.IndexOf(item)>=0){
						matchfilterl=true;
						break;
					}
				} 
			}else {
				matchfilterl=true;
			}

			//compare actions
			bool matchfiltera = false;
			if (currentFilter.Actions.Count>0){
				foreach (string item in currentFilter.Actions){
					if (passed.action.IndexOf(item)>=0){
						matchfiltera=true;
						break;
					}
				}
			} else {
				matchfiltera=true;
			}

			//compare ext
			bool matchfiltere=false;
			if (currentFilter.Ext.Count>0){
				foreach (string item in currentFilter.Ext){
					if (passed.filetype.IndexOf(item)>=0){
						matchfiltere=true;
						//break;
					}
				}
			} else {
				matchfiltere=true;
			}

			//compare cookies
			bool matchfilterc=false;
			if (currentFilter.Cookies.Count>0){
				foreach (string item in currentFilter.Cookies){
					foreach (string Citem in passed.cookie){
						if (Citem.IndexOf(item)>=0){
							matchfilterc=true;
							//break;
						}
					}
				}
			} else {
				matchfilterc=true;
			}


			//compare parameters
			bool matchfilterpg = false;
			bool matchfilterpp = false;
			if (anyPostorGet==false){
			
				foreach (string item in currentFilter.Parameters){
					foreach (string Gitem in passed.GETparameters){
						if (Gitem.IndexOf(item)>=0){
							matchfilterpg=true;
							//break;
						}
					}
					if (matchfilterpg){
						break;
					}
				}
				if (currentFilter.Parameters.Count<=0){
					matchfilterpg=true;
				}
			
				
				// POST
				foreach (string item in currentFilter.Parameters){
					foreach (string Pitem in passed.POSTparameters){
						if (Pitem.IndexOf(item)>=0){
							matchfilterpp=true;
							//break;
						}
					}
				}
				if (currentFilter.Parameters.Count<=0){
					matchfilterpp=true;
				}
			} else {
				if (passed.GETparameters.Count>0 || passed.POSTparameters.Count>0){
					matchfilterpg=true;
					matchfilterpp=true;
				}
			}

			//request
			bool matchfilterReq=false;
			if (currentFilter.RequestHeader.Count>0){
				foreach (string item in currentFilter.RequestHeader){
					if (Hpassed.header.IndexOf(item)>=0){
						matchfilterReq=true;
					}
				}
			} else {
				matchfilterReq=true;
			}

			//response
			bool matchfilterRes=false;
			if (currentFilter.ResponseHeader.Count>0){
				foreach (string item in currentFilter.ResponseHeader){
					if (Hpassed.response.IndexOf(item)>=0){
						matchfilterRes=true;
					}
				}
			} else {
				matchfilterRes=true;
			}

			//isSSL
			bool mfSSL=false;
			if (currentFilter.IsHTTPS && Hpassed.isSSL){
				mfSSL=true;
			}

			//is HTTP
			bool mfHTTP=false;
			if (currentFilter.IsHTTP && !Hpassed.isSSL){
				mfHTTP=true;
			}


			matchfiltere=matchfiltere^currentFilter.inExt;
			matchfiltera=matchfiltera^currentFilter.inActions;
			matchfilterl=matchfilterl^currentFilter.inLocations;
			matchfilterpg=matchfilterpg^currentFilter.inParameters;
			matchfilterpp=matchfilterpp^currentFilter.inParameters;
			matchfilterh=matchfilterh^currentFilter.inHost;
			matchfilterc=matchfilterc^currentFilter.inCookies;
			matchfilterReq=matchfilterReq^currentFilter.inRequests;
			matchfilterRes=matchfilterRes^currentFilter.inResponse;

			if (currentFilter.inParameters){
				return !((mfSSL || mfHTTP) && matchfilterReq && matchfilterRes && matchfilterc && matchfiltere && matchfiltera && matchfilterh && matchfilterl && (matchfilterpg && matchfilterpp));
			} else {
				return !((mfSSL || mfHTTP) && matchfilterReq && matchfilterRes && matchfilterc && matchfiltere && matchfiltera && matchfilterh && matchfilterl && (matchfilterpg || matchfilterpp));
			}
			
		}
		#endregion
		
		#region updating the URL list from the timer
		//this is very much the same as on top...with minor diffs.
		private void timer1_Tick(object sender, System.EventArgs e) {
			lock (this){

				//lets first update the discovery tree...
				foreach (discovered item in discovered_goods){
					//add the tag to the tree entry
					TreeTagType tag = new TreeTagType();
					tag.isSSL=item.isSSL;
					tag.port=item.port;
					tag.header=item.header;
					AddURLToTreeView(item.protocol+item.host+item.URL,item.mode, Reconnodes,tag);
				}

				if (chkProxyAutoUpdate.Checked){
					try{
						if (Requests.Count <1){
							return;
						}
					} catch{
						return;
					}

					WorkRequests = new ArrayList();
					lock (Requests){
						int countr=0;
						for (int g=displayed_items; g<Requests.Count; g++){	
							WorkRequests.Add(Requests[g]);
							countr++;
						}
						displayed_items+=countr;

					}

					//display
					//EditRequests.Clear();
					int actuallyadded=0;
					foreach (HTTPRequest item in WorkRequests){
						
						string display=string.Empty;
						if (item.host.Equals("this is a comment")){
							display="- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -";
							EditRequests.Add(item);
						} else{
						
							detailedRequest work =getHTTPdetails(item.header,item.host,item.isSSL);
							detailed_Requests.Add(work);
					
							bool badextflag=true;
							badextflag=ApplyBigFilter(work,item);
						
							
							if (badextflag==false){
								display+=item.reqnum.ToString()+" ";
								try{
									txtCrowResponse.Text=item.response;

									char [] tosplit={' ','\r'};
									string[] response_parts=item.response.Split(tosplit);
									display+=response_parts[1];
								} catch{}

								
								if (item.header.IndexOf("SensePost-SuruWP: edited request")>=0){
									display+="=";
								} else {display+=" ";}

								string proto="";
								if (item.isSSL){
									display+="# ";
									proto="https://";
								} else {
									display+="+ ";
									proto="http://";
								}
				
								if (work.isXML){
									display+="X";
								} 
								if (work.isMultiPart){
									display+="MP";
								}
								if (work.action.Equals("GET")==true){
									display+="G ";
								}
								if (work.action.Equals("POST")==true){
									display+="P ";
								}
								display+=work.GETparameters.Count.ToString()+" ";
								display+=work.POSTparameters.Count.ToString()+" ";
								if (chkShowHosts.Checked){
									display+=work.host+" ";
								}
								display+=work.URL;

								if (work.URL.Length>0){
									do_Recon(work);
								}else{
									MessageBox.Show("NULL URL auto");
								}
							
								//add the tag to the tree entry
								TreeTagType tag = new TreeTagType();
								tag.isSSL=work.isSSL;
								tag.port=work.port;
							
								if (work.header.IndexOf("Cookie:")<0 && work.cookie.Count>0){
									//we need to restitch the bloody cookies into the header....if they not there already..
									string cookieline="Cookie: ";
									foreach (string itemc in work.cookie){
										cookieline+=itemc+txtCookieVariableSeparator.Text[0];
									}
									cookieline.TrimEnd(';');
									//lets add it right at the top
									string tempheader=cookieline+"\r\n"+work.header;
									work.header=tempheader;
								} 

								tag.header=clean_partial_header(work.header);
								AddURLToTreeView(proto+work.host+work.URL,1, Reconnodes, tag);
								EditRequests.Add(item);
								actuallyadded++;

								//add to widget
							}
						}
						if (display.Length>4){
							listView1.Items.Add(display);
						}
						
				
					}
				
					if (actuallyadded>0){
						//lstURLs.SelectedIndex=lstURLs.Items.Count-1;
						///***** NEW DATA HERE *****///
						///***** NEW DATA HERE *****///
					}
				}
				
				
			}
		}

		#endregion

		#region Buttons on proxy - replay IE, raw TCP replay and Clear
		private void btnReplay_Click(object sender, System.EventArgs e) {
			try{
				if (txtHTTPdetails.Text.Length<=0 || txtTargetHost.Text.Length<=0 || txtTargetPort.Text.Length<=0){
					return;
				}
				showresults formres=null;
				detailedRequest_IE work = new detailedRequest_IE();
				work=getHTTPdetails(txtHTTPdetails.Text,true);
			
				if (chkReplayIE.Checked){
					//IE
					try{
						if (ie.FullName.Length>1){
							ie.Stop();
							ie.Quit();
						
							ie = new InternetExplorer();
						}
					} catch{
						ie = new InternetExplorer();
					}
					ie.Visible=true;
					Thread.Sleep(300);
					ie.Navigate2(ref work.url, ref isnull, ref isnull, ref work.postdata, ref work.headers);
				}
			
				if (chkReplayFireFox.Checked){
					//FF
					try{
						if (FF.FullName.Length>1){
					
							formres.Controls.Remove(FF);
							FF.Stop();
							FF.Quit();

							FF = new AxMOZILLACONTROLLib.AxMozillaBrowser();
							((System.ComponentModel.ISupportInitialize)(this.FF)).BeginInit();
							FF.Location = new System.Drawing.Point(0, 0);
							FF.Size = new System.Drawing.Size(792, 541);
							formres.Controls.Add(FF);
							Thread.Sleep(300);
						}
					} catch{
						formres = new showresults();
						try{
							formres.Text="FireFox Browser results";
							FF = new AxMOZILLACONTROLLib.AxMozillaBrowser();
							((System.ComponentModel.ISupportInitialize)(this.FF)).BeginInit();
							FF.Location = new System.Drawing.Point(0, 0);
							FF.Size = new System.Drawing.Size(792, 541);
							formres.Controls.Remove(formres.rtbResults);
							formres.Controls.Add(FF);
							formres.Show();
						} catch {}
					}
					//hack - Firefox adds its own content length
					try{
						work.headers=work.headers.ToString().Replace("Content-Length","Firefix").Replace("Content-length","Firefix");
						FF.Navigate2(ref work.url, ref isnull, ref isnull, ref work.postdata, ref work.headers);			
					} catch {}
				}
			} catch {}
		}

	
		private void txtHTTPdetails_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
			char pressed=e.KeyChar;
			if (pressed==27){
				btnReplay.PerformClick();
			}
		}

		private void btnClear_Click(object sender, System.EventArgs e) {
			
			globalcount=0;
			txtHTTPdetails.Clear();
			txtCrowResponse.Text="";
			try{
				listView1.Items.Clear();
				WorkRequests.Clear();
				EditRequests.Clear();
				displayed_items=0;
				lock(Requests){
					Requests.Clear();
					detailed_Requests.Clear();
				}
			} catch{}
		}

		#region rawsend but
		private void btnSendRawRequest_Click(object sender, System.EventArgs e) {			
			if (txtHTTPdetails.Text.Length<=0 || txtTargetHost.Text.Length<=0 || txtTargetPort.Text.Length<=0){
				return;
			}
			try{
				rawsendwindow.Dispose();
				rawsendwindow.Show();
				rawsendwindow.Controls.Clear();
			} catch {
				
				rawsendwindow = new showresults();
				rawsendwindow.Show();
				rawsendwindow.Controls.Clear();
			}

			rawsendwindow.StartPosition=System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
			rawsendwindow.AutoScale=true;

			//we need to create a panel for the text
			System.Windows.Forms.Panel mainpanel = new Panel();
			mainpanel.Dock=DockStyle.Fill;
			mainpanel.Size=new System.Drawing.Size(752, 477);

			//and one for the buttons
			System.Windows.Forms.Panel buttonpanel = new Panel();
			buttonpanel.Dock=DockStyle.Top;
			buttonpanel.Size = new System.Drawing.Size(752, 30);
			buttonpanel.BackColor=Color.DarkGray;
						
			//rawsendwindow.Controls.Remove(rawsendwindow.rtbResults);

			//add the panels to the main window
			rawsendwindow.Controls.Add(mainpanel);
			rawsendwindow.Controls.Add(buttonpanel);

			//create the buttons
			System.Windows.Forms.Button butSearch = new DotNetSkin.SkinControls.SkinButton();
			System.Windows.Forms.Button butClear = new DotNetSkin.SkinControls.SkinButtonYellow();
			//search
			butSearch.Location=new System.Drawing.Point(5,5);
			butSearch.Size=new Size(80,21);
			butSearch.Text="Search";
			butSearch.ForeColor=Color.Black;
			butSearch.BackColor=Color.DarkGray;
			butSearch.FlatStyle=FlatStyle.Popup;
			butSearch.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			butSearch.Click += new System.EventHandler(rawReqSearch);
			buttonpanel.Controls.Add(butSearch);
			

			//clear
			butClear.Location=new System.Drawing.Point(345,5);
			butClear.Size=new Size(80,21);
			butClear.Text="Clear";
			butClear.ForeColor=Color.Black;
			butClear.BackColor=Color.DarkGray;
			butClear.FlatStyle=FlatStyle.Popup;
			butClear.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			butClear.Click += new System.EventHandler(rawReqClear);
			buttonpanel.Controls.Add(butClear);
			

			//create the search box
			searchRawReq = new TextBox();
			searchRawReq.Location=new Point(90,5);
			searchRawReq.Size=new Size(250,21);
			//add to the panel
			buttonpanel.Controls.Add(searchRawReq);


			//the RTB
			RawReqreply = new RichTextBox();
			RawReqreply.Location=new Point(0,0);
			RawReqreply.Multiline=true;
			RawReqreply.Size=new Size (580,685);
			RawReqreply.WordWrap=true;
			RawReqreply.DetectUrls=false;
			RawReqreply.BackColor=Color.DimGray;
			RawReqreply.ForeColor=Color.WhiteSmoke;
			RawReqreply.Dock=DockStyle.Fill;
			RawReqreply.Font=new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			
			//finally the count label
			RawReqCountL=new Label();
			RawReqCountL.Location=new Point(430,7);
			buttonpanel.Controls.Add(RawReqCountL);
	
			Cursor.Current=System.Windows.Forms.Cursors.WaitCursor;
			cleanHTTP();
			
			string newheader=ComputeNewContentLength(txtHTTPdetails.Text);
			txtHTTPdetails.Text=newheader;
			newheader+="\r\n";

			string replyz="";

			bool useSSL=chkTargetIsSSL.Checked;

			if (useSSL){
				replyz=sendraw(txtTargetHost.Text,txtTargetPort.Text,newheader,65535,(int)updownTimeOut.Value,2,useSSL);
					                         
			} else {
				replyz=sendraw(txtTargetHost.Text,txtTargetPort.Text,newheader,65535,(int)updownTimeOut.Value);
			}

			//now add the rtfbox to the main panel
			mainpanel.Controls.Add(RawReqreply);
			RawReqreply.Text=replyz;
			rawsendwindow.AcceptButton=butSearch;
		}

		private void rawReqClear(object sender, System.EventArgs e) {
			string all=RawReqreply.Text;
			//RawReqreply.SelectionFont=new Font(this.Font,FontStyle.Regular);
			RawReqreply.Clear();
			RawReqreply.Text=all;

		}
		private void rawReqSearch(object sender, System.EventArgs e) {
			string all=RawReqreply.Text;
			//RawReqreply.SelectionFont=new Font(this.Font,FontStyle.Regular);
			RawReqreply.Clear();
			RawReqreply.Text=all;

			int where=-1;
			int count=0;
			while (where<RawReqreply.Text.Length-searchRawReq.Text.Length){
				where=RawReqreply.Find(searchRawReq.Text,where+1,RichTextBoxFinds.None);
				if (where==-1){
					break;
				}
				count++;
				RawReqreply.SelectionFont=new Font(this.Font,FontStyle.Bold);
				RawReqreply.SelectionColor=Color.Red;
				RawReqreply.Select(where,searchRawReq.Text.Length);
			}
			RawReqCountL.Text=count.ToString()+" found";
			
		}
		#endregion
	
		#endregion

		#region getHTTPdetails - there are 3 overloads
		private detailedRequest getHTTPdetails(string request,string host, bool isSSL){
			detailedRequest result = new detailedRequest();
			result.POSTparameters=new System.Collections.ArrayList();
			result.GETparameters=new System.Collections.ArrayList();
			result.Processed=new ArrayList();
			result.cookie=new System.Collections.ArrayList();
			//assume no XML
			result.isXML=false;
			result.isMultiPart=false;


			try{
				if (request.Length<3){
					return result;
				}

				string[] lines = request.Replace("\r\n","\n").Split('\n');
				string[] parts = lines[0].Split(' ');
				result.isSSL=isSSL;
	
				//action
				result.action=parts[0];

				//URL
				string[] actionpar=parts[1].Split(txtBaseURLSeparator.Text[0]);
				string[] URLparts = actionpar[0].Replace("http://","").Split('/');
				result.URL="/";
				for (int y=1; y<URLparts.Length; y++){
					if (URLparts[y].Length>=0){
						result.URL+=URLparts[y]+"/";
					}
				}
				if (actionpar[0].IndexOf(".")>=0 || actionpar[0].EndsWith("/")==false){
					result.URL=result.URL.TrimEnd('/');
				}
				result.URL=result.URL.Replace("//","/");
				if (result.URL.Equals("")){
					result.URL="/";
				}

				//Filetype
				string[] types=result.URL.Split('/');
				string[] ftypes=types[types.Length-1].Split('.');
				if (ftypes.Length>=2){
					result.filetype=ftypes[ftypes.Length-1];
				} else {
					result.filetype="none";
				}
				//ok here we split the host and the port...
				
				if (host.IndexOf(":")>=0){
					string[] portparts=host.Split(':');
					result.host=portparts[0];
					result.port=portparts[1];
				} else {
					result.host=host;
					if (isSSL){
						result.port="443";
					} else {
						result.port="80";
					}
				}

				//filename
				string temp=ftypes[0];
				string[] dirs=temp.Split('/');
				result.filename=dirs[dirs.Length-1];

				//GET parameters
				if (actionpar.Length>=2){
					string[] parparts=actionpar[1].Split(txtVariableSeparator.Text[0]);
					foreach (string item in parparts){

						if (item.Length>1){
							result.GETparameters.Add(item);
							result.Processed.Add(getMangleSet(item,"GET",txtKeyValueSeparator.Text[0]));
						}
					}
				} 

				//header
				int u=1;
				for (u=1; u<lines.Length; u++){
					if (lines[u].Length<3){
						break;
					}
					//cookie
					if (lines[u].StartsWith("Cookie: ")){
						string moo=lines[u].Replace("Cookie: ","").Replace(txtCookieKeyValueSeparator.Text[0]+" ",txtCookieKeyValueSeparator.Text);
						string[] work=moo.Split(txtCookieVariableSeparator.Text[0]);
						foreach (string item in work){
							if (item.Length>1){
								result.cookie.Add(item.TrimStart(' '));
								result.Processed.Add(getMangleSet(item.TrimStart(' '),"Cookie",txtCookieKeyValueSeparator.Text[0]));
							}
						}
					} else {
						result.header+=lines[u]+"\r\n";
					}
				}

				//POST parameters
				//check if its multi part form...:|
				if (result.header.IndexOf("Content-Type: multipart/form-data")>=0){
					//its multi part - welcome to hell - for now, we'll handle it as a blob
					result.isMultiPart=true;
					string allMulti="";
					for (int y=(u+1); y<lines.Length; y++){
						allMulti+=lines[y]+"\r\n";
					}
					result.POSTparameters.Add(allMulti);
					result.Processed.Add(getMangleSet(allMulti,"POST",txtKeyValueSeparator.Text[0]));
				} else {
					//first check if its XML
					if (lines[u+1].StartsWith("<?xml")){
						//find the end of the XML
						result.isXML=true;
						string allXML="";
						for (int y=(u+1); y<lines.Length; y++){
							//if (lines[y].Length<=0){
							//	break;
							//} else {
							allXML+=lines[y];
							//}
						}
						result.POSTparameters.Add(allXML);
						result.Processed.Add(getMangleSet(allXML,"POST",txtKeyValueSeparator.Text[0]));
					} else {
						//normal POST
						//get the rest of the post..if there is more!
						string restofpost=string.Empty;
						if (u+1<=lines.Length-3){
					
							for (int o=u+1; o<lines.Length; o++){
								restofpost+=lines[o]+"\r\n";
							}
						} else {
							restofpost=lines[u+1];
						}

						string[] postPar = restofpost.Split(txtVariableSeparator.Text[0]);
						foreach (string oitem in postPar){
							string item=oitem.Replace("\0","");
							if (item.Length > 1){
								result.POSTparameters.Add(item);
								result.Processed.Add(getMangleSet(item,"POST",txtKeyValueSeparator.Text[0]));
							}
						}
					}
				}

				return result;
			} catch {return result;}
		}


		private detailedRequest_IE getHTTPdetails(string request,bool isIE){
			
			detailedRequest tempreq = getHTTPdetails(request,txtTargetHost.Text,chkTargetIsSSL.Checked);
			detailedRequest_IE returner = new detailedRequest_IE();

			object header=null;
			object postdata=null;
			object url;
			if (chkTargetIsSSL.Checked){
				if (txtTargetPort.Text.Equals("443")==false){
					url="https://"+tempreq.host+":"+txtTargetPort.Text+tempreq.URL+txtBaseURLSeparator.Text[0];
				} else {
					url="https://"+tempreq.host+tempreq.URL+txtBaseURLSeparator.Text[0];
				}
			}else{
				if (txtTargetPort.Text.Equals("80")==false){
					url="http://"+tempreq.host+":"+txtTargetPort.Text+tempreq.URL+txtBaseURLSeparator.Text[0];
				} else {
					url="http://"+tempreq.host+tempreq.URL+txtBaseURLSeparator.Text[0];
				}
			}
			foreach (string getitem in tempreq.GETparameters){
				url+=getitem+txtVariableSeparator.Text[0];
			}
			url=url.ToString().TrimEnd(txtVariableSeparator.Text[0]);
			url=url.ToString().TrimEnd(txtBaseURLSeparator.Text[0]);

			//if its a post we must recalc the content length!
			if (tempreq.action.Equals("POST")){
				string[] headerlines = tempreq.header.Replace("\r\n","\n").Split('\n');
				foreach (string line in headerlines){
					string[] parts = line.Split(':');
					int plen=0;
					if (parts[0].ToLower().Equals("content-length")){
						//work out the new content length
						if (tempreq.isXML || tempreq.isMultiPart){
							header+="Content-length: "+tempreq.POSTparameters[0].ToString().Length;
						} else{
							string temppoststring="";
							foreach (string postitem in tempreq.POSTparameters){
								temppoststring+=postitem+txtVariableSeparator.Text[0];
							}
							temppoststring.TrimEnd(txtVariableSeparator.Text[0]);
							plen=temppoststring.Length;
							plen--;
							header+="Content-length: "+plen.ToString()+"\r\n";
						}
					}else{
						header+=line+"\r\n";
					}
				}
				
			} else {
				header+=tempreq.header;
			}
			

			if (header.ToString().IndexOf("SensePost-SuruWP: edited request")<0){
				header+="SensePost-SuruWP: edited request\r\n";
			}

			//ok - we need to populate the cookies...doesnt happen by itself! :)
			// THIS CLEARING DOES NOT WORK!!! it sometimes hangs!!
			// it thus needs to be able to timeout!! NASTY!!!!!
				
			Org.Mentalis.Proxy.DeleteCache delete = new DeleteCache();
			//this will exit as soon as EXITCLEANERNOW is set
			delete.clearIE();
			
			foreach (string cookiepair in tempreq.cookie){
				if (cookiepair.IndexOf(txtCookieKeyValueSeparator.Text[0])>=0){
					string[] parts = cookiepair.Split(txtCookieKeyValueSeparator.Text[0]);
					//reconstruct
					string cvalue=string.Empty;
					for (int i=1; i<parts.Length; i++){
						cvalue+=parts[i]+txtCookieKeyValueSeparator.Text[0];
					}
					cvalue=cvalue.Substring(0,cvalue.Length-1);

					SetCookie("http://"+tempreq.host,parts[0],cvalue);
				}
			}
			//add the cookies anyway - this is used in the FireFox browser!...or maybe not...:(
			if (tempreq.cookie.Count>0){
				header+="Cookie: ";
				foreach (string cookiepair in tempreq.cookie){
					header+=cookiepair+txtCookieVariableSeparator.Text[0];
				}
				header=header.ToString().TrimEnd(txtCookieVariableSeparator.Text[0]);
				header+="\r\n";
			}
			header=header.ToString().Replace("\r\n\r\n","\r\n");


			//now the post data..
			if (tempreq.action.Equals("POST")){
				foreach (string postline in tempreq.POSTparameters){
					if (postline.Length>0){
						postdata+=postline+txtVariableSeparator.Text[0];
					}
				}
				try{ //what is postdata is 0 bytes..
					postdata=postdata.ToString().TrimEnd(txtVariableSeparator.Text[0]);
					postdata=ASCIIEncoding.ASCII.GetBytes(postdata.ToString());
				}
				catch {}
				
				
			}
			returner.headers=header;
			returner.postdata=postdata;
			returner.url=url;
			return returner;
		}
		
		private void CleanCookieTimeout(object sender, System.EventArgs e){
			EXITCLEANERNOW=true;
		}
		private void timer_clean_cookie_timeout_Tick(object sender, System.EventArgs e) {
			EXITCLEANERNOW=true;
		}

		private detailedRequest_text getHTTPdetails(string request,bool isIE, bool istext){
			detailedRequest work = getHTTPdetails(request,txtTargetHost.Text,chkTargetIsSSL.Checked);
			detailedRequest_text returner = new detailedRequest_text();
			returner.host=work.host;
			returner.isSSL=work.isSSL;
			returner.port=work.port;
			string bodywork=request;
			bodywork=bodywork.Replace("Host: "+work.host,"##$$##$!");
			bodywork=bodywork.Replace("http://","").Replace("https://","").Replace(work.host,"").Replace("\n","\r\n");
			bodywork=bodywork.Replace("##$$##$!","Host: "+work.host);
			returner.body=bodywork;
			return returner;
		}

		#endregion

		#region --> Request Editor read and write & clicked

		#region Building Req Editor forms - involved
		private void showeditor(string request){

			int n_Tabs = 0;

			detailedRequest work = getHTTPdetails(request,"",false);
			is_reqeditor_open=true;

			try
			{
				reqeditor.Show();
				reqeditor.Controls.Clear();
				reqeditor.Left=locX;
				reqeditor.Top=locY;
			} 
			catch 
			{
				reqeditor = new reqEdit();
				reqeditor.Show();
				reqeditor.Controls.Clear();
				reqeditor.Left=locX;
				reqeditor.Top=locY;
			}

			Mangled tool = new Mangled();

			try
			{
			
				#region panels,action and url and submit button
				reqeditor.Closed+=new System.EventHandler(req_closed_event);
				
				reqeditor.AutoScale=true;
				
				reqeditor.BackColor=Color.DarkGray;
				reqeditor.Text="SURU Webproxy Request Editor";
				reqeditor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(reqeditor_keypress);
				
				reqeditor.Move +=new System.EventHandler(requesteditor_moved);
				Panel top_main_panel = new System.Windows.Forms.Panel();

				//Top panel
				//top_main_panel.AutoScroll = true;
				top_main_panel.Dock = System.Windows.Forms.DockStyle.Top;
				top_main_panel.Size = new System.Drawing.Size(425, 72);
				top_main_panel.Location = new System.Drawing.Point(0, 0);
				top_main_panel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(reqeditor_keypress);
				top_main_panel.TabIndex=n_Tabs; n_Tabs++;
				top_main_panel.TabStop = false;

				//Bottom panel
				Panel bottom_main_panel = new System.Windows.Forms.Panel();
				bottom_main_panel.AutoScroll = true;
				bottom_main_panel.Dock = System.Windows.Forms.DockStyle.Bottom;
				bottom_main_panel.Size = new System.Drawing.Size(425, 100);
				bottom_main_panel.Location = new System.Drawing.Point(0, 305);
				bottom_main_panel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(reqeditor_keypress);
				bottom_main_panel.TabIndex = 10000;
				bottom_main_panel.TabStop = false;

				//central main panel
				Panel ct_main_p = new System.Windows.Forms.Panel();
				ct_main_p.AutoScroll = true;
				ct_main_p.Dock = System.Windows.Forms.DockStyle.Fill;
				ct_main_p.Location = new System.Drawing.Point(0, 72);
				ct_main_p.Size = new System.Drawing.Size(425, 233);
				ct_main_p.TabIndex = n_Tabs; n_Tabs++;
				ct_main_p.TabStop = false;
				ct_main_p.Enter +=new System.EventHandler(reqEntered);
				ct_main_p.Leave+=new System.EventHandler(reqLeft);

				//splitter
				System.Windows.Forms.Splitter spl = new Splitter();
				spl.Dock = System.Windows.Forms.DockStyle.Bottom;
				spl.Name = "splitter1";
				spl.Size = new System.Drawing.Size(417, 3);
				spl.TabIndex = n_Tabs; n_Tabs++;
				spl.TabStop = false;
				spl.BackColor=Color.Gainsboro;
		
				reqeditor.Controls.Add(spl);

				reqeditor.BringToFront();
				ToolTip toolTip1 = new System.Windows.Forms.ToolTip();
		
				toolTip1.AutomaticDelay = 1000;
				toolTip1.AutoPopDelay = 10000;
				toolTip1.InitialDelay = 1000;
				toolTip1.ReshowDelay = 20;

				
				int panelsize=225;
				if (work.cookie.Count>0){panelsize+=50+(26*work.cookie.Count);}
				if (work.POSTparameters.Count>0){panelsize+=50+(26*work.POSTparameters.Count);}
				if (work.GETparameters.Count>0){panelsize+=50+(26*work.GETparameters.Count);}
				if (work.isXML || work.isMultiPart){panelsize+=200;}
				if (panelsize>600)
				{
					reqeditor.Size=new System.Drawing.Size(580,600);
					//	panel1.Size = new System.Drawing.Size(580,600);
				} 
				else 
				{
					reqeditor.Size=new System.Drawing.Size(560,panelsize);
					//	panel1.Size = new System.Drawing.Size(560,panelsize);
				}
			
				//add the panels
				reqeditor.Controls.Add(ct_main_p);
				reqeditor.Controls.Add(bottom_main_panel);
				reqeditor.Controls.Add(top_main_panel);

				int pos=5;
				#region submit button
				System.Windows.Forms.Button but = new DotNetSkin.SkinControls.SkinButton();
				System.Windows.Forms.Button butraw = new DotNetSkin.SkinControls.SkinButtonYellow();
			
				but.Location=new System.Drawing.Point(5,pos);
				but.Size=new Size(260,21);
				but.Text="Browse request";
				but.ForeColor=Color.Black;
				but.BackColor=Color.DarkGray;
				but.FlatStyle=FlatStyle.Popup;
				but.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				but.Click += new System.EventHandler(reqEditClicked);
				but.TabIndex = n_Tabs; n_Tabs++;
				but.TabStop = true;
				top_main_panel.Controls.Add(but);
				reqeditor.AcceptButton=but;

				butraw.Location=new System.Drawing.Point(270,pos);
				butraw.Size=new Size(260,21);
				butraw.Text="Send raw request";
				butraw.ForeColor=Color.Firebrick;
				butraw.BackColor=Color.DarkGray;
				butraw.FlatStyle=FlatStyle.Popup;
				butraw.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				butraw.Click += new System.EventHandler(reqEditClickedRaw);
				butraw.TabIndex = n_Tabs; n_Tabs++;
				butraw.TabStop=true;
				top_main_panel.Controls.Add(butraw);
				
				#endregion
				pos=pos+25;

				action_textbox = new TextBox();
				action_textbox.Location = new Point(5,pos);
				action_textbox.BorderStyle=BorderStyle.FixedSingle;
				action_textbox.Text=work.action;
				action_textbox.Size = new Size(80,21);
				action_textbox.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				action_textbox.TabIndex = n_Tabs; n_Tabs++;
				action_textbox.TabStop=true;
				top_main_panel.Controls.Add(action_textbox);


				URL_textbox = new TextBox();
				URL_textbox.ForeColor=Color.WhiteSmoke;
				URL_textbox.BackColor=Color.FromArgb(100,100,100);
				URL_textbox.Location = new Point(90,pos);
				URL_textbox.Size= new Size(458,21);
				URL_textbox.Text=work.URL;
				URL_textbox.BorderStyle=BorderStyle.FixedSingle;
				URL_textbox.TabIndex=n_Tabs; n_Tabs++;
				URL_textbox.TabStop=true;
				URL_textbox.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				top_main_panel.Controls.Add(URL_textbox);
				int y=0;
				#endregion

				//ok now the fun starts....			

				#region POSTs
				POSTparameters_amount=0;
				if (work.POSTparameters.Count>0)
				{

					y=0;
					//check if its XML
					if (work.isXML==true)
					{
						int n_curtab = 3999;
						//create the panel
						System.Windows.Forms.Panel se_ch_p= new System.Windows.Forms.Panel();
						System.Windows.Forms.Panel se_p_name= new System.Windows.Forms.Panel();
						System.Windows.Forms.Panel se_p= new System.Windows.Forms.Panel();
						se_p.TabIndex = n_curtab; n_curtab--;
						se_p.TabStop=false;
						se_p.Controls.Add(POST_textBoxes[y]);
						se_p.Controls.Add(se_ch_p);

						se_ch_p.Dock = System.Windows.Forms.DockStyle.Left;
						se_ch_p.Location = new System.Drawing.Point(0, 0);
						se_ch_p.Size = new System.Drawing.Size(25, 200);
						se_ch_p.TabIndex = n_Tabs; n_Tabs++;

						POST_chk[y] = new DotNetSkin.SkinControls.SkinCheckBox();
						POST_chk[y].Location = new Point(5,pos+1);
						POST_chk[y].FlatStyle=FlatStyle.Standard;
						POST_chk[y].CheckState=CheckState.Checked;
						POST_chk[y].Text="";
						POST_chk[y].Size = new System.Drawing.Size(20, 20);
						POST_chk[y].TabIndex=n_Tabs; n_Tabs++;
						POST_chk[y].TabStop=true;
						se_ch_p.Controls.Add(POST_chk[y]);

						POSTparameters_amount=-1;
						POST_textBoxes[y] = new TextBox();
						POST_textBoxes[y].Multiline=true;
						POST_textBoxes[y].WordWrap=false;
						POST_textBoxes[y].ScrollBars=ScrollBars.Both;
						POST_textBoxes[y].ForeColor=Color.Black;
						POST_textBoxes[y].Dock=DockStyle.Fill;
						POST_textBoxes[y].BackColor=Color.Khaki;
						POST_textBoxes[y].BorderStyle=BorderStyle.FixedSingle;
						POST_textBoxes[y].TabIndex = n_Tabs; n_Tabs++;
						POST_textBoxes[y].TabStop = true;
						string XMLstuff = (string)work.POSTparameters[0];
						POST_textBoxes[y].Text = XMLstuff.Replace("<","\r\n<").TrimStart('\r','\n'); 
						

						//add to main panel
						
					

						//se_p.Location = new System.Drawing.Point(0, pos);
						se_p.Dock = System.Windows.Forms.DockStyle.Top;
						se_p.Size = new System.Drawing.Size(425, 200);
						//se_p.TabIndex = y+9;

						
						//add panel to the main section
						ct_main_p.Controls.Add(se_p);
	
						pos+=200;

					}
					if (work.isMultiPart==true)
					{
						int n_curtab = 3999;
						//create the panel
						System.Windows.Forms.Panel se_ch_p= new System.Windows.Forms.Panel();
						System.Windows.Forms.Panel se_p_name= new System.Windows.Forms.Panel();
						//add to main panel
						System.Windows.Forms.Panel se_p= new System.Windows.Forms.Panel();
						se_p.TabIndex=n_curtab; n_curtab--;
						se_p.TabStop=false;
						se_p.Controls.Add(POST_textBoxes[y]);
						se_p.Controls.Add(se_ch_p);

						se_ch_p.Dock = System.Windows.Forms.DockStyle.Left;
						se_ch_p.Location = new System.Drawing.Point(0, 0);
						se_ch_p.Size = new System.Drawing.Size(25, 200);
						se_ch_p.TabIndex = n_Tabs; n_Tabs++;
						se_ch_p.TabStop =false;

						POST_chk[y] = new DotNetSkin.SkinControls.SkinCheckBox();
						POST_chk[y].Location = new Point(5,pos+1);
						POST_chk[y].FlatStyle=FlatStyle.Standard;
						POST_chk[y].CheckState=CheckState.Checked;
						POST_chk[y].Text="";
						POST_chk[y].Size = new System.Drawing.Size(20, 20);
						POST_chk[y].TabIndex=n_Tabs; n_Tabs++;
						POST_chk[y].TabStop=true;
						se_ch_p.Controls.Add(POST_chk[y]);
						
						POSTparameters_amount=-2;
						POST_textBoxes[y] = new TextBox();
						POST_textBoxes[y].Multiline=true;
						POST_textBoxes[y].WordWrap=false;
						POST_textBoxes[y].Dock=DockStyle.Fill;
						POST_textBoxes[y].ScrollBars=ScrollBars.Both;
						POST_textBoxes[y].ForeColor=Color.DimGray;
						POST_textBoxes[y].Size = new System.Drawing.Size(510, 200);
						POST_textBoxes[y].BackColor=Color.Gainsboro;
						POST_textBoxes[y].BorderStyle=BorderStyle.FixedSingle;
						string Multistuff = (string)work.POSTparameters[0];
						POST_textBoxes[y].Text = Multistuff; 
						POST_textBoxes[y].TabIndex=n_Tabs; n_Tabs++;
						POST_textBoxes[y].TabStop = true;

						
					
						//se_p.Location = new System.Drawing.Point(0, pos);
						se_p.Dock = System.Windows.Forms.DockStyle.Top;
						se_p.Size = new System.Drawing.Size(425, 200);
						//se_p.TabIndex = y+9;
						
						//add panel to the main section
						ct_main_p.Controls.Add(se_p);

						pos+=200;

					} 
					if (work.isMultiPart==false && work.isXML==false)
					{
						int n_curtab = 3999;
						//int z_tab = 0;
						//z_tab = work.POSTparameters.Count;
						//just normal POST
						foreach(string item in work.POSTparameters)
						{
							#region panel definitions
							System.Windows.Forms.Panel se_ch_p= new System.Windows.Forms.Panel();
							System.Windows.Forms.Panel se_p_name= new System.Windows.Forms.Panel();
							System.Windows.Forms.Splitter se_s_nv = new System.Windows.Forms.Splitter();
							System.Windows.Forms.Panel se_p_value = new System.Windows.Forms.Panel();
							//add to the single entry panel..
							System.Windows.Forms.Panel se_p= new System.Windows.Forms.Panel();
							se_p.Controls.Add(se_p_value);
							se_p.Controls.Add(se_s_nv);
							se_p.Controls.Add(se_p_name);
							se_p.Controls.Add(se_ch_p);
							se_p.TabIndex = n_curtab; n_curtab--;
							se_p.TabStop = false;

							se_ch_p.Dock = System.Windows.Forms.DockStyle.Left;
							se_ch_p.Location = new System.Drawing.Point(0, 0);
							se_ch_p.Size = new System.Drawing.Size(25, 24);
							se_ch_p.TabIndex = n_Tabs; n_Tabs++;

							se_p_name.Dock = System.Windows.Forms.DockStyle.Left;
							se_p_name.Location = new System.Drawing.Point(48, 0);
							se_p_name.Size = new System.Drawing.Size(120, 24);
							se_p_name.TabIndex = n_Tabs; n_Tabs++;
							se_p_name.TabStop=false;

							se_s_nv.Location = new System.Drawing.Point(168, 0);
							se_s_nv.Size = new System.Drawing.Size(3, 24);
							se_s_nv.TabIndex = n_Tabs; n_Tabs++;
							se_s_nv.TabStop = false;

							se_p_value.Dock = System.Windows.Forms.DockStyle.Fill;
							se_p_value.Location = new System.Drawing.Point(171, 0);
							se_p_value.Size = new System.Drawing.Size(253, 24);
							se_p_value.TabIndex = n_Tabs; n_Tabs++;
							se_p_value.TabStop=false;
							#endregion

							POST_chk[y] = new DotNetSkin.SkinControls.SkinCheckBox();
							POST_chk[y].FlatStyle=FlatStyle.Standard;
							POST_chk[y].Location = new Point(5,2);
							POST_chk[y].CheckState=CheckState.Checked;
							POST_chk[y].Text="";
							POST_chk[y].Size = new System.Drawing.Size(20, 20);
							POST_chk[y].TabIndex = n_Tabs; n_Tabs++;
							POST_chk[y].TabStop=true;

							string[] parts = item.Split(txtKeyValueSeparator.Text[0]);
							POST_comboBoxes[y] = new ComboBox();
							POST_comboBoxes[y].Size = new System.Drawing.Size(335, 21);
							POST_comboBoxes[y].BackColor=Color.Cornsilk;
							POST_comboBoxes[y].Dock=DockStyle.Fill;
						
							se_ch_p.Controls.Add(POST_chk[y]);
							
							string itemadd=string.Empty;
							if (parts.Length>0)
							{
								for (int u=1; u<parts.Length; u++)
								{
									tool = new Mangled();
									tool = (Mangled)work.Processed[y+work.GETparameters.Count+work.cookie.Count];
									toolTip1.SetToolTip(POST_comboBoxes[y],"MD5:     "+tool.varvalmd5+"\r\nSHA1:    "+tool.varvalsha1+"\r\nB64dec:"+tool.varvalbase64dec+"\r\nB64enc:"+tool.varvalbase64enc);
									//itemadd += convertFromHex(parts[u])+"=";
									//lets see if we shouldnt encode POSTS
									itemadd += parts[u]+"=";
								}
								//chop off only the LAST =
								itemadd=itemadd.Substring(0,itemadd.Length-1);
							} 
							else 
							{
								itemadd = "";
							}
							POST_comboBoxes[y].Text=itemadd;
							POST_comboBoxes[y].Items.Add(itemadd);
							POST_comboBoxes[y].Items.Add("FUZZCTRL");
							POST_comboBoxes[y].Items.Add("-v--shortcut to FuzzDB--v--");
							POST_comboBoxes[y].SelectedIndex=0;
							//add the fuzzing strings
							foreach (string itemcmb in cmbCustom.Items)
							{
								POST_comboBoxes[y].Items.Add("FZ: "+itemcmb);
							}

							POST_textBoxes[y] = new TextBox();
							POST_textBoxes[y].BorderStyle=BorderStyle.FixedSingle;
							POST_textBoxes[y].Size = new System.Drawing.Size(180, 20);
							POST_textBoxes[y].BackColor=Color.Cornsilk;
							POST_textBoxes[y].Dock=DockStyle.Fill;
							POST_textBoxes[y].Text = parts[0];
						
							POST_textBoxes[y].TabIndex = n_Tabs; n_Tabs++;
							POST_textBoxes[y].TabStop=true;
							se_p_name.Controls.Add(POST_textBoxes[y]);
							POST_comboBoxes[y].TabIndex = n_Tabs; n_Tabs++;
							POST_comboBoxes[y].TabStop = true;
							se_p_value.Controls.Add(POST_comboBoxes[y]);
					
							

							//se_p.Location = new System.Drawing.Point(0, pos);
							se_p.Dock = System.Windows.Forms.DockStyle.Top;
							se_p.Size = new System.Drawing.Size(425, 24);

							//add panel to the main section
							ct_main_p.Controls.Add(se_p);
	
				
							y++;
							POSTparameters_amount++;
						}
					}
					#region label
					System.Windows.Forms.Panel se_l = new System.Windows.Forms.Panel();
					se_l.TabIndex = n_Tabs; n_Tabs++;
					se_l.TabStop = false;
					Label label_post = new Label();
					label_post.Text="POST";
					if (work.isXML)
					{
						label_post.Text+=" (XML)";
					}
					if (work.isMultiPart)
					{
						label_post.Text+=" (Multipart)";
					}
					label_post.Size = new Size(192, 21);
					label_post.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
					label_post.Location= new Point(25,2);
					se_l.Controls.Add(label_post);
					se_l.Dock = System.Windows.Forms.DockStyle.Top;
					se_l.Size = new System.Drawing.Size(425, 24);
						
					//add panel to the main section
					ct_main_p.Controls.Add(se_l);
					#endregion
				}
				#endregion

				#region cookies
				cookie_amount=0;
				if (work.cookie.Count>0)
				{
					int n_curtab = 2999;


				
					y=0;
				
					foreach(string item in work.cookie)
					{
												
						#region panel definitions
						System.Windows.Forms.Panel se_ch_p= new System.Windows.Forms.Panel();
						System.Windows.Forms.Panel se_p_name= new System.Windows.Forms.Panel();
						System.Windows.Forms.Splitter se_s_nv = new System.Windows.Forms.Splitter();
						System.Windows.Forms.Panel se_p_value = new System.Windows.Forms.Panel();
						//add to the single entry panel..
						System.Windows.Forms.Panel se_p= new System.Windows.Forms.Panel();
						
						se_p.Dock = System.Windows.Forms.DockStyle.Top;
						se_p.Size = new System.Drawing.Size(425, 24);
						se_p.Controls.Add(se_p_value);
						se_p.Controls.Add(se_s_nv);
						se_p.Controls.Add(se_p_name);
						se_p.Controls.Add(se_ch_p);
						se_p.TabIndex=n_curtab; n_curtab--;
						se_p.TabStop = false;

						se_ch_p.Dock = System.Windows.Forms.DockStyle.Left;
						se_ch_p.Location = new System.Drawing.Point(0, 0);
						se_ch_p.Size = new System.Drawing.Size(25, 24);
						se_ch_p.TabIndex = n_Tabs; n_Tabs++;
						se_ch_p.TabStop = false;

						se_p_name.Dock = System.Windows.Forms.DockStyle.Left;
						se_p_name.Location = new System.Drawing.Point(48, 0);
						se_p_name.Size = new System.Drawing.Size(120, 24);
						se_p_name.TabIndex = n_Tabs; n_Tabs++;
						se_p_name.TabStop=false;

						se_s_nv.Location = new System.Drawing.Point(168, 0);
						se_s_nv.Size = new System.Drawing.Size(3, 24);
						se_s_nv.TabIndex = n_Tabs; n_Tabs++;
						se_s_nv.TabStop = false;
						
						se_p_value.Dock = System.Windows.Forms.DockStyle.Fill;
						se_p_value.Location = new System.Drawing.Point(171, 0);
						se_p_value.Size = new System.Drawing.Size(253, 24);
						se_p_value.TabIndex = n_Tabs; n_Tabs++;
						se_p_value.TabStop = false;

						
						//	se_p.SuspendLayout();

						ct_main_p.Controls.Add(se_p);
						
						#endregion

						cookie_chk[y] = new DotNetSkin.SkinControls.SkinCheckBox();
						cookie_chk[y].FlatStyle=FlatStyle.Standard;
						cookie_chk[y].Location = new Point(5,2);
						cookie_chk[y].CheckState=CheckState.Checked;
						cookie_chk[y].Text="";
						cookie_chk[y].Size = new System.Drawing.Size(20, 20);
						cookie_chk[y].TabIndex=n_Tabs; n_Tabs++;
						cookie_chk[y].TabStop=true;
						se_ch_p.Controls.Add(cookie_chk[y]);

						string[] parts = item.Split(txtCookieKeyValueSeparator.Text[0]);
						cookie_comboBoxes[y] = new ComboBox();
						cookie_comboBoxes[y].Dock=DockStyle.Fill;
						cookie_comboBoxes[y].Dock=DockStyle.Fill;
						cookie_comboBoxes[y].Size = new System.Drawing.Size(335, 21);
						cookie_comboBoxes[y].TabIndex = 0;
						cookie_comboBoxes[y].BackColor=Color.Tan;
						cookie_comboBoxes[y].Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						
						
						string itemadd=string.Empty;
						if (parts.Length>0)
						{
							for (int u=1; u<parts.Length; u++)
							{
								tool = new Mangled();
								tool = (Mangled)work.Processed[y+work.GETparameters.Count];
								toolTip1.SetToolTip(cookie_comboBoxes[y],"MD5:     "+tool.varvalmd5+"\r\nSHA1:   "+tool.varvalsha1+"\r\nB64dec:"+tool.varvalbase64dec+"\r\nB64enc:"+tool.varvalbase64enc);
								itemadd += convertFromHex(parts[u])+"=";
							}
							itemadd=itemadd.Substring(0,itemadd.Length-1);
						} 
						else 
						{
							itemadd = "";
						}
						
						cookie_comboBoxes[y].Items.Add(itemadd);
						cookie_comboBoxes[y].Items.Add("FUZZCTRL");
						cookie_comboBoxes[y].Items.Add("-v--shortcut to FuzzDB--v--");
						
						//add the fuzzing strings
						foreach (string itemcmb in cmbCustom.Items)
						{
							cookie_comboBoxes[y].Items.Add("FZ: "+itemcmb);
						}
						cookie_comboBoxes[y].Text=itemadd;
						//se_p_value.Controls.Add(cookie_comboBoxes[y]);

						cookie_textBoxes[y] = new TextBox();
						cookie_textBoxes[y].BorderStyle=BorderStyle.FixedSingle;
						cookie_textBoxes[y].BackColor=Color.Tan;
						cookie_textBoxes[y].Dock=DockStyle.Fill;
						cookie_textBoxes[y].Size = new System.Drawing.Size(180, 20);
						cookie_textBoxes[y].TabIndex = 17;
						cookie_textBoxes[y].Text = convertFromHex(parts[0]);
						cookie_textBoxes[y].Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						
						cookie_textBoxes[y].TabIndex = n_Tabs; n_Tabs++;
						cookie_textBoxes[y].TabStop =true;
						se_p_name.Controls.Add(cookie_textBoxes[y]);
						cookie_comboBoxes[y].TabIndex = n_Tabs; n_Tabs++;
						cookie_comboBoxes[y].TabStop = true;
						se_p_value.Controls.Add(cookie_comboBoxes[y]);
						
							
						y++;
						cookie_amount++;
						
					}
					
				
					#region label
					System.Windows.Forms.Panel se_l = new System.Windows.Forms.Panel();
					se_l.TabIndex = n_Tabs; n_Tabs++;
					se_l.TabStop = false;
					Label label_cookie = new Label();
					label_cookie.Text="Cookies";
					label_cookie.Size = new Size(192, 21);
					label_cookie.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
					label_cookie.Location= new Point(25,2);
					label_cookie.TabIndex = n_Tabs; n_Tabs++;

					
					se_l.Controls.Add(label_cookie);
					se_l.Dock = System.Windows.Forms.DockStyle.Top;
					se_l.Size = new System.Drawing.Size(425, 24);
						
					//add panel to the main section
					ct_main_p.Controls.Add(se_l);
					#endregion
					
				}
				#endregion

				#region GETs
				GETparameters_amount=0;
				if (work.GETparameters.Count>0)
				{
					int n_curtab = 1999;
					pos+=30;

					
					//GETs
					y=0;
					pos=0;
					foreach(string item in work.GETparameters)
					{
						//all the panel defs
						
						#region panel definitions
						System.Windows.Forms.Panel se_ch_p= new System.Windows.Forms.Panel();
						System.Windows.Forms.Panel se_p_name= new System.Windows.Forms.Panel();
						System.Windows.Forms.Splitter se_s_nv = new System.Windows.Forms.Splitter();
						System.Windows.Forms.Panel se_p_value = new System.Windows.Forms.Panel();

						//add to the single entry panel..
						System.Windows.Forms.Panel se_p= new System.Windows.Forms.Panel();
						se_p.TabIndex=n_curtab; n_curtab--;
						se_p.TabStop=false;

						se_ch_p.Dock = System.Windows.Forms.DockStyle.Left;
						se_ch_p.Location = new System.Drawing.Point(0, 0);
						se_ch_p.Size = new System.Drawing.Size(25, 24);
						se_ch_p.TabIndex = n_Tabs; n_Tabs++;
						se_ch_p.TabStop=false;

						se_p_name.Dock = System.Windows.Forms.DockStyle.Left;
						se_p_name.Location = new System.Drawing.Point(48, 0);
						se_p_name.Size = new System.Drawing.Size(120, 24);
						se_p_name.TabIndex = n_Tabs; n_Tabs++;
						se_p_name.TabStop=false;

						se_s_nv.Location = new System.Drawing.Point(168, 0);
						se_s_nv.Size = new System.Drawing.Size(3, 24);
						se_s_nv.TabIndex = n_Tabs; n_Tabs++;
						se_s_nv.TabStop = false;

						se_p_value.Dock = System.Windows.Forms.DockStyle.Fill;
						se_p_value.Location = new System.Drawing.Point(171, 0);
						se_p_value.Size = new System.Drawing.Size(253, 24);
						se_p_value.TabIndex = n_Tabs; n_Tabs++;
						se_p_value.TabStop=false;
						#endregion


						string[] parts = item.Split(txtKeyValueSeparator.Text[0]);
						GET_comboBoxes[y] = new ComboBox();
						GET_comboBoxes[y].Size = new System.Drawing.Size(335, 21);
						GET_comboBoxes[y].TabIndex = 36;
						GET_comboBoxes[y].Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						GET_comboBoxes[y].BackColor=Color.LightGray;
						GET_comboBoxes[y].Dock=DockStyle.Fill;

						GET_chk[y] = new DotNetSkin.SkinControls.SkinCheckBox();
						GET_chk[y].Location = new Point(5,2);
						GET_chk[y].FlatStyle=FlatStyle.Standard;
						GET_chk[y].CheckState=CheckState.Checked;
						GET_chk[y].Text="";
						GET_chk[y].Size = new System.Drawing.Size(20, 20);
						GET_chk[y].TabIndex=n_Tabs; n_Tabs++;
						GET_chk[y].TabStop=true;
						se_ch_p.Controls.Add(GET_chk[y]);
					
						string itemadd=string.Empty;
						if (parts.Length>1)
						{
							
							for (int u=1; u<parts.Length; u++)
							{
								itemadd += convertFromHex(parts[u])+"=";
								tool = new Mangled();
								tool = (Mangled)work.Processed[y];
								toolTip1.SetToolTip(GET_comboBoxes[y],"MD5:   "+tool.varvalmd5+"\r\nSHA1:   "+tool.varvalsha1+"\r\nB64dec:"+tool.varvalbase64dec+"\r\nB64enc:"+tool.varvalbase64enc);
							}
							
							itemadd=itemadd.Substring(0,itemadd.Length-1);
						} 
						else 
						{
							itemadd = "";
							//toolTip1.SetToolTip(GET_comboBoxes[y],"");
						}
						GET_comboBoxes[y].Text=itemadd;
						GET_comboBoxes[y].Items.Add(itemadd);
						GET_comboBoxes[y].Items.Add("FUZZCTRL");
						GET_comboBoxes[y].Items.Add("-v--shortcut to FuzzDB--v--");
						//add the fuzzing strings
						foreach (string itemcmb in cmbCustom.Items)
						{
							GET_comboBoxes[y].Items.Add("FZ: "+itemcmb);
						}
					

						GET_textBoxes[y] = new TextBox();
						GET_textBoxes[y].BorderStyle=BorderStyle.FixedSingle;
						GET_textBoxes[y].Size = new System.Drawing.Size(180, 20);
						GET_textBoxes[y].TabIndex = 17;
						GET_textBoxes[y].BackColor=Color.LightGray;
						GET_textBoxes[y].Dock=DockStyle.Fill;
						GET_textBoxes[y].Text = convertFromHex(parts[0]);
						GET_textBoxes[y].Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						
						GET_textBoxes[y].TabIndex=n_Tabs; n_Tabs++;
						GET_textBoxes[y].TabStop=true;
						se_p_name.Controls.Add(GET_textBoxes[y]);
						GET_comboBoxes[y].TabIndex=n_Tabs; n_Tabs++;
						GET_comboBoxes[y].TabStop = true;
						se_p_value.Controls.Add(GET_comboBoxes[y]);
				
						se_p.Controls.Add(se_p_value);
						se_p.Controls.Add(se_s_nv);
						se_p.Controls.Add(se_p_name);
						se_p.Controls.Add(se_ch_p);

						se_p.Dock = System.Windows.Forms.DockStyle.Top;
						se_p.Size = new System.Drawing.Size(425, 24);
						
						
						//add panel to the main section
						ct_main_p.Controls.Add(se_p);
						

				
						y++;
						GETparameters_amount++;
						pos+=24;
				
					}
					#region label
					System.Windows.Forms.Panel se_l = new System.Windows.Forms.Panel();
					se_l.TabIndex = n_Tabs; n_Tabs++;
					se_l.TabStop = false;
					Label label_get = new Label();
					label_get.Text="GET parameters";
					label_get.Size = new Size(192, 21);
					label_get.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
					label_get.Location= new Point(25,2);
					label_get.TabIndex = n_Tabs; n_Tabs++;
					
					se_l.Controls.Add(label_get);
					se_l.Dock = System.Windows.Forms.DockStyle.Top;
					se_l.Size = new System.Drawing.Size(425, 24);
						
					//add panel to the main section
					ct_main_p.Controls.Add(se_l);
					#endregion
				}
				#endregion

				#region headers and button


				header_textbox = new TextBox();
				header_textbox.Multiline=true;
				header_textbox.BorderStyle=BorderStyle.FixedSingle;
				header_textbox.Dock=DockStyle.Fill;
				header_textbox.ScrollBars=ScrollBars.Both;
				header_textbox.Size=new Size(545,100);
				header_textbox.ForeColor=Color.DarkSlateGray;
				header_textbox.Text=work.header;
				header_textbox.WordWrap=false;
				header_textbox.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				header_textbox.TabIndex=n_Tabs; n_Tabs++;
				header_textbox.TabStop = true;
				bottom_main_panel.Controls.Add(header_textbox);

								
				#region label
				System.Windows.Forms.Panel se_h = new System.Windows.Forms.Panel();
				se_h.TabIndex = n_Tabs; n_Tabs++;
				se_h.TabStop=false;
				Label label_hdr = new Label();
				label_hdr.Text="The rest of the header:";
				
				label_hdr.Size = new Size(192, 21);
				label_hdr.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				label_hdr.Location= new Point(25,2);
				label_hdr.TabIndex = n_Tabs; n_Tabs++;
					
				se_h.Controls.Add(label_hdr);
				se_h.Dock = System.Windows.Forms.DockStyle.Top;
				se_h.Size = new System.Drawing.Size(425, 24);
						
				//add panel to the main section
				bottom_main_panel.Controls.Add(se_h);
				#endregion

				#endregion

			} 
			catch {}
			reqeditor.SetTextSelect();
			//this.Focus();
		}
		private void reqEntered(object sender, System.EventArgs e){
			insidetxtedit=chkProxyAutoUpdate.Checked;
			chkProxyAutoUpdate.Checked=false;	
		}
		private void reqLeft(object sender, System.EventArgs e){
			//restore
			chkProxyAutoUpdate.Checked=insidetxtedit;
		}

		#endregion
		
		#region rebuild the HTTP(s) request from the req editor - involved
		private string rebuildHTTPrequest(){
			bool isitfuzzed=false;
			string ret="";
		
			#region Lets see if he wants to fuzz something first
			//GETS
			for (int y=0; y<GETparameters_amount; y++){
				try{
					if (GET_comboBoxes[y].Text.Substring(0,4).Equals("FZ: ")){
						lblFile1.Text=txtFuzzDirLocation.Text+GET_comboBoxes[y].Text.Substring(4,GET_comboBoxes[y].Text.Length-4);
						GET_comboBoxes[y].Text="FUZZCTRL";
						radioFile1.Checked=true;
						chkImmFuzz.Checked=true;
					}
				} catch {
					//if the length of the string < 4
				}
			}
			//POSTS
			for (int y=0; y<POSTparameters_amount; y++){
				try{
					if (POST_comboBoxes[y].Text.Substring(0,4).Equals("FZ: ")){
						lblFile1.Text=txtFuzzDirLocation.Text+POST_comboBoxes[y].Text.Substring(4,POST_comboBoxes[y].Text.Length-4);
						POST_comboBoxes[y].Text="FUZZCTRL";
						radioFile1.Checked=true;
						chkImmFuzz.Checked=true;
					}
				} catch {
					//if the length of the string < 4
				}
			}
			//cookies
			for (int y=0; y<cookie_amount; y++){
				try{
					if (cookie_comboBoxes[y].Text.Substring(0,4).Equals("FZ: ")){
						lblFile1.Text=txtFuzzDirLocation.Text+cookie_comboBoxes[y].Text.Substring(4,cookie_comboBoxes[y].Text.Length-4);
						cookie_comboBoxes[y].Text="FUZZCTRL";
						radioFile1.Checked=true;
						chkImmFuzz.Checked=true;
					}
				} catch {
					//if the length of the string < 4
				}
			}
			#endregion
			
			#region action and URL
			ret+=action_textbox.Text+" ";
			ret+=URL_textbox.Text;
		
			#endregion

			#region GETS
			string GETline="";
			if (GETparameters_amount>0){
				for (int y=0; y<GETparameters_amount; y++){
					
					if (GET_textBoxes[y].Text.IndexOf("FUZZCTRL")>=0 || GET_comboBoxes[y].Text.IndexOf("FUZZCTRL")>=0){
						isitfuzzed=true;
					}
					if (GET_chk[y].Checked){
						bool skip=false;
						if (GET_textBoxes[y].Text.Equals("__VIEWSTATE")){
							skip=true;
						}

						if (GET_textBoxes[y].Text.Length>0 && GET_comboBoxes[y].Text.Length>0){
							GETline+=convertToHex(GET_textBoxes[y].Text,false)+"="+convertToHex(GET_comboBoxes[y].Text,skip)+"&";
							//GETline+=toolTip1.GetToolTip(GET_textBoxes[y])+"="+(toolTip1.GetToolTip(GET_comboBoxes[y]))+"&";
						}
						if (GET_textBoxes[y].Text.Length==0 && GET_comboBoxes[y].Text.Length>0){
							GETline+=convertToHex(GET_comboBoxes[y].Text,skip)+"&";
							//GETline+=toolTip1.GetToolTip(GET_comboBoxes[y])+"&";
						}
						if (GET_textBoxes[y].Text.Length>0 && GET_comboBoxes[y].Text.Length==0){
							GETline+=convertToHex(GET_textBoxes[y].Text,false)+"=&";
							//GETline+=toolTip1.GetToolTip(GET_textBoxes[y])+"=&";
						}
					}
				}
			}
			if (GETline.Length>0){
				ret+=txtBaseURLSeparator.Text[0]+GETline;
			}
			ret=ret.TrimEnd(txtVariableSeparator.Text[0]);
			ret=ret.TrimEnd(' ');
			ret+=" HTTP/1.0 \r\n";
			#endregion

			#region cookies
			//cookies
			if (cookie_amount>0){
				string cookieline="";
				
				for (int y=0; y<cookie_amount; y++){

					if (cookie_textBoxes[y].Text.IndexOf("FUZZCTRL")>=0 || cookie_comboBoxes[y].Text.IndexOf("FUZZCTRL")>=0){
						isitfuzzed=true;
					}
					if (cookie_chk[y].Checked){
						

						if (cookie_textBoxes[y].Text.Length>0 && cookie_comboBoxes[y].Text.Length>0){
							cookieline+=cookie_textBoxes[y].Text+txtCookieKeyValueSeparator.Text[0]+cookie_comboBoxes[y].Text+txtCookieVariableSeparator.Text[0];
						}
						if (cookie_textBoxes[y].Text.Length==0 && cookie_comboBoxes[y].Text.Length>0){
							cookieline+=cookie_comboBoxes[y].Text+txtCookieVariableSeparator.Text[0];
						}
						if (cookie_textBoxes[y].Text.Length>0 && cookie_comboBoxes[y].Text.Length==0){
							cookieline+=cookie_textBoxes[y].Text+txtCookieKeyValueSeparator.Text[0];
						}
					}
				}
				cookieline=cookieline.TrimEnd(txtCookieVariableSeparator.Text[0]);
				if (cookieline.Length>0){
					cookieline="Cookie: "+cookieline;
					cookieline+="\r\n";
					ret+=cookieline;
				}
				
			}
			#endregion

			#region rest of headers
			//rest of the headers
			foreach (string item in header_textbox.Text.Replace("\r\n\r\n","\r\n").Replace("\r\n","\n").Split('\n')){
				if (item.ToLower().IndexOf("content-length")<0 && item.Length>2){ 
					ret+=item+"\r\n";
				}
			}
			#endregion

			#region POSTS

			//check if its a POST
			if (action_textbox.Text.CompareTo("POST")==0){
				string poststring="";
				//check if its XML or MultiPart
				if (POSTparameters_amount<0){
					//assume there's just one long length
					if (POST_textBoxes[0].Text.IndexOf("FUZZCTRL")>=0){
						isitfuzzed=true;
					}
					if (POST_chk[0].Checked && POSTparameters_amount==-1){
						//XML
						poststring=POST_textBoxes[0].Text.Replace("\r\n","\n").Replace("\n","");
					}

					if (POST_chk[0].Checked && POSTparameters_amount==-2){
						//MultiPart
						poststring=POST_textBoxes[0].Text;
					}

					ret+="Content-length: "+poststring.Length;
					ret+="\r\n\r\n";
					ret+=poststring;
					return ret;
				}
				else {
					//first generate the POST string
					for (int y=0; y<POSTparameters_amount; y++){
					
						if (POST_textBoxes[y].Text.IndexOf("FUZZCTRL")>=0 || POST_comboBoxes[y].Text.IndexOf("FUZZCTRL")>=0){
							isitfuzzed=true;
						}
						if (POST_chk[y].Checked){
							bool skip=false;
							if (POST_textBoxes[y].Text.Equals("__VIEWSTATE")){
								skip=true;
							}
							//lets see if we shouldnt encode POSTS
							/*

							if (POST_textBoxes[y].Text.Length>0 && POST_comboBoxes[y].Text.Length>0){
								poststring+=convertToHex(POST_textBoxes[y].Text,false)+txtKeyValueSeparator.Text[0]+convertToHex(POST_comboBoxes[y].Text,skip)+txtVariableSeparator.Text[0];
							}
							if (POST_textBoxes[y].Text.Length==0 && POST_comboBoxes[y].Text.Length>0){
								poststring+=convertToHex(POST_comboBoxes[y].Text,skip)+txtVariableSeparator.Text[0];
							}
							if (POST_textBoxes[y].Text.Length>0 && POST_comboBoxes[y].Text.Length==0){
								poststring+=convertToHex(POST_textBoxes[y].Text,false) + txtKeyValueSeparator.Text[0] + txtVariableSeparator.Text[0];
							}
							*/
							if (POST_textBoxes[y].Text.Length>0 && POST_comboBoxes[y].Text.Length>0){
								poststring+=POST_textBoxes[y].Text+txtKeyValueSeparator.Text[0]+POST_comboBoxes[y].Text+txtVariableSeparator.Text[0];
							}
							if (POST_textBoxes[y].Text.Length==0 && POST_comboBoxes[y].Text.Length>0){
								poststring+=POST_comboBoxes[y].Text+txtVariableSeparator.Text[0];
							}
							if (POST_textBoxes[y].Text.Length>0 && POST_comboBoxes[y].Text.Length==0){
								poststring+=POST_textBoxes[y].Text + txtKeyValueSeparator.Text[0] + txtVariableSeparator.Text[0];
							}
						}
					}
					poststring=poststring.TrimEnd(txtVariableSeparator.Text[0]);
					//work out the length
					ret+="Content-length: "+poststring.Length;
					ret+="\r\n\r\n";
					ret+=poststring;
				}
			}
			#endregion
			return ret;
		}
		#endregion

		#region After req editor request has been clicked
		private void reqEditClicked(object sender, System.EventArgs e) {
			
			string edited=rebuildHTTPrequest();
			txtHTTPdetails.Text="";
			txtHTTPdetails.Text=edited;
			//if (txtHTTPdetails.Text.IndexOf("FUZZCTRL")<0 && Trap==false){
			if (txtHTTPdetails.Text.IndexOf("FUZZCTRL")<0){
				btnReplay_Click(null,null);
			} else {
				//set the buffer..
				
				if (chkImmFuzz.Checked){
					btnCrowStart.PerformClick();
				}
			}
		}

		private void reqEditClickedRaw(object sender, System.EventArgs e) {
			
			string edited=rebuildHTTPrequest();
			txtHTTPdetails.Text="";
			txtHTTPdetails.Text=edited;
			//if (txtHTTPdetails.Text.IndexOf("FUZZCTRL")<0 && Trap==false){
			if (txtHTTPdetails.Text.IndexOf("FUZZCTRL")<0){
				btnSendRawRequest_Click(null,null);
			} else {
				//set the buffer..
				
				if (chkImmFuzz.Checked){
					btnCrowStart.PerformClick();
				}
			}
		}

		private void reqeditor_keypress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
			char pressed=e.KeyChar;
			if (pressed==27){
				reqeditor.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(reqeditor_keypress);
				reqeditor.Dispose();
				is_reqeditor_open=false;
			}
		
		}
		private void requesteditor_moved(object sender, System.EventArgs args){
			locY=reqeditor.Top;
			locX=reqeditor.Left;
		}

		private void req_closed_event(object sender, EventArgs args){
			is_reqeditor_open=false;
			locX=reqeditor.Left;
			locY=reqeditor.Top;
			//restore
			chkProxyAutoUpdate.Checked=insidetxtedit;
		}
		#endregion

		#region As it comes in
		private void button3_Click(object sender, System.EventArgs e) {
			
			showeditor(txtHTTPdetails.Text);
		}
		#endregion
		#endregion
	
		#region --> JOBQ handling and finding of files and directories
		
		#region check if we must add a directory, or if we had it before
		private void do_Recon(detailedRequest work){
			//get for recon
			try{
				bool is_new=get_Recon_Dirs(work);
				
				if (kn_exts.Contains(work.filetype)==false && work.filetype.Length>0){
					kn_exts.Add(work.filetype);
				//	txtReconExts.Text+=work.filetype+"\r\n";
					is_new=true;
				}

				if (kn_filenames.Contains(work.filename)==false&& work.filename.Length>0){
					get_Recon_SmartFile(work);
					kn_filenames.Add(work.filename);
				//	txtReconFilenames.Text+=work.filename+"\r\n";
					is_new=true;
				}

				if (kn_hosts.Contains(work.host)==false&& work.host.Length>0){
					kn_hosts.Add(work.host);
					cmbReconTargetHost.Items.Add(work.host);
					is_new=true;
				}
			
				if (is_new){
				}	
			} catch( Exception ex) {
				//	MessageBox.Show("Not there............"+ex.ToString());
				//	Thread.Sleep(1000);
			}
		}
		#endregion

		#region finding a new DIRECTORY to add to the Q
		private bool get_Recon_Dirs(detailedRequest work){
			bool ret=false;
			work.URL=work.URL.Replace("//","/");
			try{
				
				//check if the dir is in the black list
				ArrayList blacklist=new ArrayList();
				blacklist.AddRange(txtWiktoSkipDirs.Lines);
				foreach (string entry in blacklist){
					if (work.URL.IndexOf("/"+entry)>=0){
						return false;
					}
				}

				//check if the site is in the black list
				blacklist=new ArrayList();
				blacklist.AddRange(txtReconSkipSites.Lines);
				foreach (string entry in blacklist){
					if (work.host.IndexOf(entry)>=0){
						return false;
					}
				}


				string[] dirs=work.URL.Split('/');
				string build="/";

				//i hate comboboxes...i hate hate them
				string selectedhost="";
				try{
					selectedhost=cmbReconTargetHost.SelectedItem.ToString();
				} catch {}
			
			
			
				if (work.host.Equals(selectedhost) || chkDoRecon.Checked){
					int subtract=1;
					if (work.URL.IndexOf(".")<0){
						subtract=0;
					}
					for (int t=0; t<dirs.Length-subtract; t++){
						build+=dirs[t]+"/";
						build=build.Replace("//","/");
						string hostanddir=work.host+":"+build;
						hostanddir=hostanddir.Replace("//","/");
						if (kn_dirs.Contains(hostanddir)==false){
							kn_dirs.Add(hostanddir);
							
							string proto;
							if (work.isSSL){
								//	txtReconDirs.Text+="#";
								proto="https://";
							} else {
								//txtReconDirs.Text+="+";
								proto="http://";
							}
							
							//AddURLToTreeView(proto+work.host+work.URL.TrimEnd('/'), Mode.one, Reconnodes);
							//	txtReconDirs.Text+=hostanddir.Replace("//","/")+"\r\n";
							ret=true;

							//we need to here add jobs to do..

							ArrayList totestfor=new ArrayList();
							totestfor.AddRange(txtWiktoTestDirs.Lines);
							if (chkSmartDirScan.Checked){
								foreach (string item in kn_dirs){
									string[] partsd = item.Split(':');
									string[] dirparts = partsd[1].Split('/');
									foreach (string dirpart in dirparts){
										if (totestfor.Contains(dirpart)==false && dirpart.Length>0){
											totestfor.Add(dirpart);
										}
									}
								}
							}
					
							jobQ singlejob = new jobQ();
							singlejob.ext="-NONE-";
							
							
							singlejob.targethost=work.host;
							singlejob.targetport=work.port;
							singlejob.isSSL=work.isSSL;
							try{
								if (work.header.IndexOf("Cookie:")<0 && work.cookie.Count>0){
									//we need to restitch the bloody cookies into the header....if they not there already..
									string cookieline="Cookie: ";
									foreach (string item in work.cookie){
										cookieline+=item+txtCookieVariableSeparator.Text[0];
									}
									cookieline.TrimEnd(';');
									//lets add it right at the top
									string tempheader=cookieline+"\r\n"+work.header;
									work.header=tempheader;
								} 
							}catch{}
							singlejob.header=clean_partial_header(work.header);

							// add it for indexability check
							if (ckhReconIndex.Checked){
								singlejob.jobtype="_check_index_";
								singlejob.location=build.Replace("//","/");
								//give it priority....:)
								JOBQ.Insert(0,singlejob);
								
								
							}

							foreach (string dir_item in totestfor){

								//add job to recon this dir::
								// //////////////////////////
								if (chkReconDirMine.Checked){
									singlejob.jobtype="_dir_recon_";
									singlejob.location=build.Replace("//","/")+dir_item;
									singlejob.location=singlejob.location.Replace("//","/");
									JOBQ.Add(singlejob);
								}	
							}
						}
					}
				} 
			}catch{}
			
			return ret;
		}

		#region Smart file search
		private void get_Recon_SmartFile(detailedRequest work){
			//i hate comboboxes...i hate hate them
			string selectedhost="";
			try{
				selectedhost=cmbReconTargetHost.SelectedItem.ToString();
			} catch {}
						
			if (work.host.Equals(selectedhost) || chkDoRecon.Checked){

				if (chkSmartFileDeep.Checked){
					//if its in kn_dirs - then it has been OK-ed before to check here....
					foreach (string dir in kn_dirs){
						string[] parts = dir.Split(':');
						//0-host
						//1-dir
						if (parts[0].Equals(work.host)){
							//add job
							jobQ singlejob = new jobQ();
							singlejob.targethost=work.host;
							singlejob.targetport=work.port;
							singlejob.isSSL=work.isSSL;

					
							singlejob.location=parts[1];
							singlejob.jobtype="__file_recon__";
							singlejob.header=clean_partial_header(work.header);
							singlejob.filename=work.filename;

							ArrayList totestfor=new ArrayList();
							totestfor.AddRange(txtWiktoTestTypes.Lines);
							
					
							foreach (string test_ext in totestfor){
								if (test_ext.Equals(work.filetype)==false){
									//else we 'discover' the file the guy surfed to anyhow...:)
									singlejob.ext=test_ext;
									JOBQ.Add(singlejob);
								}
							}
						}
					}
				}
				if (chkSmartFileShallow.Checked){
					
					//check if the dir is in the black list
					ArrayList blacklist=new ArrayList();
					blacklist.AddRange(txtWiktoSkipDirs.Lines);
					foreach (string entry in blacklist){
						if (work.URL.IndexOf("/"+entry)>=0){
							return;
						}
					}

					jobQ singlejob = new jobQ();
					singlejob.targethost=work.host;
					singlejob.targetport=work.port;
					singlejob.isSSL=work.isSSL;
				
					//process location..:(
					string[] dirs=work.URL.Split('/');
					string realloc=string.Empty;
					for (int t=0; t<dirs.Length-1; t++){
						realloc+=dirs[t]+"/";
					}
					realloc=realloc.TrimEnd('/');

					singlejob.location=realloc;
					singlejob.jobtype="__file_recon__";
					singlejob.header=clean_partial_header(work.header);
					singlejob.filename=work.filename;
					
					foreach (string test_ext in txtWiktoTestTypes.Lines){
						if (test_ext.Equals(work.filetype)==false){
							//else we 'discover' the file the guy surfed to anyhow...:)
							singlejob.ext=test_ext;
							JOBQ.Add(singlejob);
						}
					}
				}
			}
		}
		#endregion

		#endregion

		#region if an item on the Recon tree has been checked, we need to add jobs to the Q for FILES
		//this is for adding a file finding job.
		private void treeRecon_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e) {
			
			//lets see
			TreeNode current = e.Node;
			jobQ singlejob = new jobQ();
			string FP=current.FullPath;
			
			//host
			string[] FP_parts=FP.Split('\\');
			singlejob.targethost=FP_parts[0];
				
			string temp="";
			for (int t=1; t<FP_parts.Length; t++){
				temp+=FP_parts[t]+"/";
			}
			temp=temp.TrimEnd('/');
			singlejob.location="/"+temp;
			singlejob.location=singlejob.location.Replace("//","/");

			TreeTagType rectag = new TreeTagType();
			rectag=(TreeTagType)current.Tag;
				
			//isSSL
			singlejob.isSSL = rectag.isSSL;
			singlejob.targetport=rectag.port;

			singlejob.jobtype="__file_recon__";
			singlejob.header=clean_partial_header(rectag.header);
			
			if (current.Checked){
				foreach (string filetype in txtWiktoTestTypes.Lines){
					foreach (string filename in txtWiktoTestFilenames.Lines){
						singlejob.ext=filetype;
						singlejob.filename=filename;
						JOBQ.Add(singlejob);
					}
				}

			} else {
				//remove it from the Q
				lock (JOBQ){
					ArrayList kaas = new ArrayList();
					kaas.AddRange(JOBQ.GetRange(0,JOBQ.Count));

					foreach (jobQ item in kaas){
						if (item.targethost.Equals(singlejob.targethost) &&
							item.location.Equals(singlejob.location) &&
							item.targetport.Equals(singlejob.targetport) &&
							item.isSSL==singlejob.isSSL){

							//remove it
							JOBQ.Remove(item);
						}
					}
				}
			}
		}
		#endregion

		#region Q gets serviced from this timer
		//we do work here...
		private void timer2_Tick(object sender, System.EventArgs e) {

			//eish
			if (jobstodo>=0){
				lblCurrentJobQ.Text=jobstodo.ToString();
			} else {
				lblCurrentJobQ.Text="0";
			}
			lock((object)jobstodo){
				if (jobstodo>6){
					return;
				}
			}
			
			jobQ nowjob = new jobQ();
			lblJobQLength.Text=JOBQ.Count.ToString();
			
			
			if (JOBQ.Count>0){
				lock ((object)jobstodo){
					jobstodo++;
				}
				
				nowjob=(jobQ)JOBQ[0];
				JobThread(nowjob);

				//ok here the job is done so take it off the queue
				JOBQ.Remove(nowjob);

			}
			
		}
		#endregion

		#region jobthreads - doing a job in the Q
		public delegate void DelegateJob(jobQ thejob);
		public void JobThread(jobQ thejob) {
			DelegateJob delJob= new DelegateJob(DoOneJob);
			AsyncCallback callBackWhenDone = new AsyncCallback(this.EndJob);
			delJob.BeginInvoke(thejob,callBackWhenDone,null);
		}
			
		
		public void EndJob(IAsyncResult arResult) {
		}
		#endregion

		#region This is called for every job in the Q
		private void DoOneJob(jobQ thejob){		
			try{
				#region JOBRECON_DIR
				
				if (thejob.jobtype.Equals("_dir_recon_")){
				
					statusBar1.Text="Testing directory "+thejob.location+" on host "+thejob.targethost+"  SSL is "+thejob.isSSL.ToString();
				
					bool istrue=testRequest(thejob.targethost,
						thejob.targetport,
						buildRequest(thejob.location,"","",thejob.header),
						(int)updownTimeOut.Value,
						false,
						thejob.isSSL,
						thejob.header,
						Convert.ToDouble(NUPDOWNBackEnd.Value));
				
					if (istrue == true) {
						

						//we also need to feed this back for recursive testing!
						detailedRequest feedback = new detailedRequest();
						feedback.port=thejob.targetport;
						feedback.host=thejob.targethost;
						feedback.isSSL=thejob.isSSL;
						feedback.header=thejob.header;
						feedback.URL=thejob.location+"/";
						feedback.filename="";
						feedback.filetype="";
						do_Recon(feedback);

						//ok add the treenode
						string proto;
						if (thejob.isSSL){
							proto="https://";
						} else {
							proto="http://";
						}

						discovered newdis = new discovered();
						newdis.host=thejob.targethost;
						newdis.isSSL=thejob.isSSL;
						newdis.port=thejob.targetport;
						newdis.URL=thejob.location;
						newdis.protocol=proto;
						newdis.mode=2;
						newdis.header=thejob.header;

						lock (discovered_goods){
							discovered_goods.Add(newdis);
						}
					}	
					lock (this){
						jobstodo--;
					}
				}
				
				#endregion

				#region JOB_CHECK_INDEX
				//do reconjob
				if (thejob.jobtype.Equals("_check_index_")){
					statusBar1.Text="Checking indexability for "+thejob.location+" on "+thejob.targethost+"  SSL is "+thejob.isSSL.ToString();
					bool isindex=checkIndexability(thejob);
					if (isindex){
						string proto;
						if (thejob.isSSL){
							proto="https://";
						} else {
							proto="http://";
						}
						discovered newdis = new discovered();
						newdis.host=thejob.targethost;
						newdis.isSSL=thejob.isSSL;
						newdis.port=thejob.targetport;
						newdis.URL=thejob.location+"[indexable]";
						newdis.protocol=proto;
						newdis.mode=3;
						newdis.header=thejob.header;

						discovered_goods.Add(newdis);
					}
					lock (this){
						jobstodo--;
					}
				}
				
				#endregion

				#region JOBRECON_FILE
				if (thejob.jobtype.Equals("__file_recon__")){
					string displaytext="Testing file /"+thejob.location+"/"+thejob.filename+"."+thejob.ext+" on host "+thejob.targethost+"  SSL is "+thejob.isSSL.ToString();
					statusBar1.Text=displaytext.Replace("//","/").Replace("///","/");
				
					bool istrue=testRequest(thejob.targethost,
						thejob.targetport,
						buildRequest(thejob.location,thejob.filename,thejob.ext,thejob.header),
						(int)updownTimeOut.Value,
						true,
						thejob.isSSL,
						thejob.header,
						Convert.ToDouble(NUPDOWNBackEnd.Value));
				
					if (istrue == true) {
						//ok add the treenode
						string proto;
						if (thejob.isSSL){
							proto="https://";
						} else {
							proto="http://";
						}

						discovered newdis = new discovered();
						newdis.host=thejob.targethost;
						newdis.isSSL=thejob.isSSL;
						newdis.port=thejob.targetport;
						newdis.URL=thejob.location+"/"+thejob.filename+"."+thejob.ext;
						newdis.protocol=proto;
						newdis.mode=4;
						newdis.header=thejob.header;

						lock (discovered_goods){
							discovered_goods.Add(newdis);
						}
					}
					lock (this){
						jobstodo--;
					}
				}
				#endregion

			
			} catch(Exception ex) {
				MessageBox.Show(ex.ToString());
			}
				
		}
		#endregion

		#endregion

		public bool checkIndexability(jobQ thejob){
			string response="";
			
			if (thejob.isSSL){
				response = sendraw(thejob.targethost,thejob.targetport,"GET "+thejob.location+" HTTP/1.0\r\n"+thejob.header+"\r\n",4096,(int)updownTimeOut.Value,3,true);
			} else {
				response = sendraw(thejob.targethost,thejob.targetport,"GET "+thejob.location+" HTTP/1.0\r\n"+thejob.header+"\r\n",4096,(int)updownTimeOut.Value);
			}
			
			if (response.IndexOf("ndex of")>=0 || response.IndexOf("To Parent Directory")>=0){
				return true;
			} else {
				return false;
			}
			return false;
		}

		#endregion

		#region ==> Recon Tree functions etc

		#region ReconTree Context Implementations
		
		// From recon tree context menu
		private void expandTreeNode(object sender, System.EventArgs e){
			try{
				TreeNode current = treeRecon.GetNodeAt(MouseX, MouseY);
				current.ExpandAll();
				//treeRecon.SelectedNode.ExpandAll();
			} catch{}
		}
		private void pruneTreeNode(object sender, System.EventArgs e){
			try{
				TreeNode current = treeRecon.GetNodeAt(MouseX, MouseY);
				TreeNodeCollection below = current.Nodes;
				//TreeNodeCollection below = treeRecon.SelectedNode.Nodes;
				foreach (TreeNode gone in below){
					gone.Remove();
				}
			}catch{}
		}
		private void deleteTreeNode(object sender, System.EventArgs e){
			try{
				TreeNode current = treeRecon.GetNodeAt(MouseX, MouseY);
				current.Remove();
				//treeRecon.SelectedNode.Remove();
			} catch {}
		}
		private void clearQonehost(object sender, System.EventArgs e){
			try{
				TreeNode current = treeRecon.GetNodeAt(MouseX, MouseY);
				TreeTagType thistag = new TreeTagType();
				//thistag=(TreeTagType)treeRecon.SelectedNode.Tag;
				thistag=(TreeTagType)current.Tag;
				//string[] parts = treeRecon.SelectedNode.FullPath.Split('\\');
				string[] parts = current.FullPath.Split('\\');
				ArrayList tempQ = new ArrayList();
				tempQ.AddRange(JOBQ.GetRange(0,JOBQ.Count));
				lock (JOBQ){
					JOBQ.Clear();
					foreach (jobQ item in tempQ){
						if (item.targethost.Equals(parts[0])==false){
							JOBQ.Add(item);
						}
					}
				}
			} catch{}
		}
	
		private void AddDirectoryForFilescan(object sender, System.EventArgs e){
			try{
				TreeNode current = treeRecon.GetNodeAt(MouseX, MouseY);
				current.Checked = true;
				//treeRecon.SelectedNode.Checked=true;
			}catch{}
		}
		private void AddDirectoryForDirectoryscan(object sender, System.EventArgs e){
			try{
				TreeNode current = treeRecon.GetNodeAt(MouseX, MouseY);
				jobQ singlejob = new jobQ();
				string FP=current.FullPath;
				string templocation=string.Empty;
			
				//host
				string[] FP_parts=FP.Split('\\');
				singlejob.targethost=FP_parts[0];
				
				string temp="";
				for (int t=1; t<FP_parts.Length; t++){
					temp+=FP_parts[t]+"/";
				}
				temp=temp.TrimEnd('/');
				templocation="/"+temp;
				templocation=templocation.Replace("//","/");

				TreeTagType rectag = new TreeTagType();
				rectag=(TreeTagType)current.Tag;
				
				//isSSL
				singlejob.isSSL = rectag.isSSL;
				singlejob.targetport=rectag.port;
				singlejob.ext="-NONE-";

				singlejob.jobtype="_dir_recon_";
				singlejob.header=clean_partial_header(rectag.header);
			
				ArrayList totestfor=new ArrayList();
				totestfor.AddRange(txtWiktoTestDirs.Lines);
				if (chkSmartDirScan.Checked){
					foreach (string item in kn_dirs){
						string[] partsd = item.Split(':');
						string[] dirparts = partsd[1].Split('/');
						foreach (string dirpart in dirparts){
							if (totestfor.Contains(dirpart)==false && dirpart.Length>0){
								totestfor.Add(dirpart);
							}
						}
					}
				}

				foreach (string dir in totestfor){
					singlejob.location=templocation+"/"+dir;
					singlejob.location=singlejob.location.Replace("//","/");
					JOBQ.Add(singlejob);
				
				}
			} catch{}

			 
		}
		// Navigate tree
		private void TreeNavigate(object sender, System.EventArgs e){
			try{
				TreeNode current = treeRecon.GetNodeAt(MouseX, MouseY);
				//string path=treeRecon.SelectedNode.FullPath;
				string path=current.FullPath;
				//TreeTagType here = (TreeTagType)treeRecon.SelectedNode.Tag;
				TreeTagType here = (TreeTagType)current.Tag;
				/*
				
				object nul=null;
				InitBrowser(800,600,20,20);
				object kaas = here.header.Replace("Content-length","Suru").Replace("Content-Length","Suru");
				if (here.isSSL){
					BrowseRequest("https://"+path,ref nul, ref kaas);
				} else {
					BrowseRequest("http://"+path,ref nul, ref kaas);
				}
				*/

				string[] parts = path.Split('\\');
				string build=string.Empty;
				for (int i=1; i<parts.Length; i++){
					build+=parts[i]+"/";
				}
				
				if (parts[parts.Length-1].IndexOf('.')>=0){
					build=build.TrimEnd('/');
				} else {
					build=build.Replace("//","/");
				}
				txtHTTPdetails.Text="GET /"+build+" HTTP/1.0\r\n";
				txtHTTPdetails.Text+=here.header.Replace("Content-length","SuruBlockedContentLength").Replace("Content-Length","SuruBlockedContentLength");;
				txtTargetHost.Text=parts[0];
				txtTargetPort.Text=here.port;
				chkTargetIsSSL.Checked=here.isSSL;
				btnReplay_Click(null,null);


			} catch{}
		}
		#endregion

		#region ReconTree Button manupilation implementations

		private void btnReconClearTree_Click(object sender, System.EventArgs e) {
			//cmbReconTargetHost.Items.Clear();
			JOBQ.Clear();
			treeRecon.Nodes.Clear();
			Reconnodes.Clear();
			discovered_goods.Clear();
			kn_dirs.Clear();
			kn_exts.Clear();
			kn_filenames.Clear();
			jobstodo=0;
		}

		private void chkDoRecon_CheckedChanged(object sender, System.EventArgs e) {
			cmbReconTargetHost.Enabled=!chkDoRecon.Checked;
		}

		private void btnReconTreeExpandAll_Click(object sender, System.EventArgs e) {
			treeRecon.ExpandAll();
		}

		private void btnReconTreeCollapseAll_Click(object sender, System.EventArgs e) {
			treeRecon.CollapseAll();
		}
		private void btnClearJobQ_Click(object sender, System.EventArgs e) {
			JOBQ.Clear();
			jobstodo=0;
		}
		private void btnTreeReload_Click(object sender, System.EventArgs e) {
			UpdateListViewControl();
			//button2_Click(null,null);
		}
		private void trckReconSpeed_Scroll(object sender, System.EventArgs e) {
			timer2.Interval=3001-trckReconSpeed.Value;
		}
		#endregion
		
		#region Updating the Recon tree

		/*public class ExtendedTreeNode : TreeNode {

		
			public enum Protocol { HTTP, HTTPS };

			public Hashtable children;

			public ExtendedTreeNode(string name)	{
				//
				// TODO: Add constructor logic here
				//
				this.Text						= name;
				this.children					= new Hashtable();
			}
		}*/

		public static Color getColourCode(int mode, Protocol p)	{
			Color theColour				= Color.Black;
			// Colour code the last node
			switch ( mode )	{
				case 1 :
					if ( p == Protocol.HTTP )
						theColour		= Color.Black;
					if ( p == Protocol.HTTPS )
						theColour		= Color.SlateBlue;
					break;

				case 2 :
					if ( p == Protocol.HTTP )
						theColour		= Color.Red;
					if ( p == Protocol.HTTPS )
						theColour		= Color.Firebrick;
					break;

				case 3:
					theColour			= Color.Green;
					break;

				case 4 :
					if ( p == Protocol.HTTP )
						theColour		= Color.Magenta;
					if ( p == Protocol.HTTPS )
						theColour		= Color.Maroon;
					break;
			}
			return theColour;
		}

		public void AddURLToTreeView(string url, int mode, Hashtable htNodes, TreeTagType treetag)	{
			Regex r							= new Regex(@"^\s*(?<protocol>https?)://(?<path>.*)", RegexOptions.IgnoreCase);
			Match m							= r.Match(url);
			TreeNodeCollection nodes		= treeRecon.Nodes;
			if ( m.Success )	{
				Protocol p	= (Protocol)Enum.Parse(typeof(Protocol), m.Result("${protocol}"), true);
				char[] separators			= { '\\', '/' };
				ExtendedTreeNode etn		= null;
				bool updateflag=false;
				foreach ( string path in m.Result("${path}").Split(separators) )	{
					if (path.Length>0){
						if ( htNodes.ContainsKey(path) )	{
							etn					= (ExtendedTreeNode)htNodes[path];
						}
						else	{
							etn					= new ExtendedTreeNode(path);
							etn.ForeColor		= getColourCode(mode, p);
							etn.Tag =			(TreeTagType)treetag;
							nodes.Add(etn);
							htNodes.Add(path, etn);
							updateflag=true;
						}
						// Expand the node in mode two & three
						if ( (mode == 2 || mode==3 || mode==4) && chkReconAlwaysExpand.Checked)
							etn.Expand();
						htNodes					= etn.children;
						nodes					= etn.Nodes;
					}
				}		// next node
				etn.ForeColor				= getColourCode(mode, p);
				if (updateflag){
					treeRecon.Refresh();
				}
			}		// if valid url
		}		// end AddURLToTreeView()
		#endregion

		#region smart scanning checkboxes

		private void chkSmartFileDeep_CheckedChanged(object sender, System.EventArgs e) {
			if (chkSmartFileDeep.Checked){
				chkSmartFileShallow.Checked=false;
			}
		}

		private void chkSmartFileShallow_CheckedChanged(object sender, System.EventArgs e) {
			if (chkSmartFileDeep.Checked){
				chkSmartFileDeep.Checked=false;
			}
		}
		#endregion
		#endregion

		#region ==> WIKTO - checking for existence of files and directories

		public bool testRequest (string ipRaw, string portRaw, string requestRaw, int TimeOut, bool fileordir, bool isSSL, string header, double trigger) {
			string response="";
			
			if (isSSL){
				response = sendraw(ipRaw,portRaw,requestRaw,4096,TimeOut,3,true);
			} else {
				response = sendraw(ipRaw,portRaw,requestRaw,4096,TimeOut);
			}
		
			string[] parts = new string[2000];
			parts=requestRaw.Split(' ');
			double result;
			niktoRequests niktoset = new niktoRequests();
				
			//true=file false=dir
			if (fileordir == false){
				niktoset.description="FPtestdir";
				niktoset.request=requestRaw;
				niktoset.trigger="";
				niktoset.type="FPtestdir";
				niktoset.method=parts[0];
				niktoset.isSSL=isSSL;
					
				string[] urlparts = new string[20];
				urlparts=parts[1].Split('/');
				string finalresult="";
				for(int a=1; a<urlparts.Length-2; a++){
					finalresult+="/"+urlparts[a];
				}
				finalresult=finalresult.Replace("//","/");
				result=testniktoFP(ipRaw,portRaw,niktoset,finalresult+"/mooforgetit",response,header);
				//recheck!
				if (result<trigger && result>=0){
					lock(this){
						
						cleanBlob(ipRaw,extractFileType(finalresult+"/mooforgetit"),extractLocation(finalresult+"/mooforgetit"),"directory");
						
					}
					if (isSSL){
						response = sendraw(ipRaw,portRaw,requestRaw,4096,TimeOut,3,true);
					} else {
						response = sendraw(ipRaw,portRaw,requestRaw,4096,TimeOut);
					}
					
					result=testniktoFP(ipRaw,portRaw,niktoset,finalresult+"/mooforgetit",response,header);
						
				}
			} else {
				niktoset.description="FPtestfile";
				niktoset.request=requestRaw;
				niktoset.trigger="";
				niktoset.type="FPtestfile";
				niktoset.method=parts[0];
				niktoset.isSSL=isSSL;
				parts[1]=parts[1].Replace("//","/");
				result=testniktoFP(ipRaw,portRaw,niktoset,parts[1],response,header);
				//recheck!
				if (result<trigger && result >=0){
					lock(this){
						
						cleanBlob(ipRaw,extractFileType(parts[1]),extractLocation(parts[1]),"file");
						
					}
					if (isSSL){
						response = sendraw(ipRaw,portRaw,requestRaw,4096,TimeOut,3,true);
					} else {
						response = sendraw(ipRaw,portRaw,requestRaw,4096,TimeOut);
					}
					result=testniktoFP(ipRaw,portRaw,niktoset,parts[1],response,header);
					
					
				}
			}
			
			if ((result < trigger) && result >= 0.00){
				return true;
			} else {return false;}
			
				
			
		}

		private double testniktoFP (string ipRaw, string portRaw, niktoRequests niktoset, string request, string reply,string header){
			
			string location=extractLocation(request);
			string filetype=extractFileType(request);
			
			string blobFromDB=getBlob(ipRaw,portRaw,niktoset,filetype,location,header);
			if (blobFromDB.Length > 0){
				double result;
				lock (this){
					result=compareBlobs(blobFromDB,reply,false,false);
				}
				return result;
			}else return -1.0;
			
			
		}

		
		#region blob handling, cleaning, generating, getting
		private string getBlob (string ipRaw, string portRaw, niktoRequests niktoset, string filetype, string filelocation,string header){
			lock (this){
				
				FPdata entry = new FPdata();
				entry.filetype=filetype;
				entry.host=ipRaw;
				entry.method=niktoset.method;
				entry.URLlocation=filelocation;
				if (niktoset.type.CompareTo("FPtestfile")==0 || niktoset.type.CompareTo("FPtestdir")==0){
					//check if it exists
					if (backend_FP.Contains(entry)){
						return (string)backend_FP[entry];
					}

				} else {
					if (nikto_FP.Contains(entry)){
						return (string)nikto_FP[entry];
					}
				}
			}
			
			//if we end up here we know we must go get a new one
			return generateBlob(ipRaw,portRaw,niktoset,filetype,filelocation,header);
			
		}

		
		private void cleanBlob(string ipRaw,string filetype, string filelocation,string mode){
			lock(nikto_FP){
				lock(backend_FP){
					FPdata item = new FPdata();
					item.filetype = filetype;
					item.host=ipRaw;
					item.URLlocation=filelocation;

					if (mode.Equals("directory")){
				
						backend_FP.Remove(item);
					}
					if (mode.Equals("file")){
						nikto_FP.Remove(item);
					}
				}
			}
		}

		private string generateBlob (string target, string port, niktoRequests niktoset, string filetype, string filelocation,string header){
			
			niktoRequests FPtest;
			FPtest.method=niktoset.method;
			FPtest.description="FP test item";
			FPtest.type="FP test item";
			FPtest.trigger="";
			FPtest.isSSL=niktoset.isSSL;
			
			string result="";
			
			if (niktoset.type.Equals("FPtestdir")==false){
				FPtest.request=filelocation + "noteverthere." + filetype;
			} else FPtest.request=filelocation + "noteverthere/";

			if (niktoset.isSSL){
				result=stestNiktoRequest(target,port,buildNiktoRequest(FPtest,header),FPtest,6000,true);
			} else {
				result=stestNiktoRequest(target,port,buildNiktoRequest(FPtest,header),FPtest,6000,false);
			}
			
			//actually..this is the only place we really need to lock
			lock (backend_FP){
				lock (nikto_FP){
					FPdata item = new FPdata();
					item.URLlocation=filelocation;
					item.method=FPtest.method;
					item.filetype=filetype;
					item.host=target;

					if (niktoset.type.CompareTo("FPtestfile")==0 || niktoset.type.CompareTo("FPtestdir")==0){
						if (backend_FP.Contains(item)==false){
							backend_FP.Add(item,result);
						}
					}else{
						if (nikto_FP.Contains(item)==false){
							nikto_FP.Add(item,result);
						}
					}
				}
			}

			return result;
			

		}
		#endregion

		#region building the requests
		public string stestNiktoRequest (string ipRaw, string portRaw, string requestRaw, niktoRequests niktoset, int TimeOut, bool isSSL) {
			string response="";
			
			if (isSSL){
				response = sendraw(ipRaw,portRaw,requestRaw,4096,TimeOut,2,true);
			} else {
				response = sendraw(ipRaw,portRaw,requestRaw,4096,TimeOut);
			}
			
			return response;
			
		}
		public string buildNiktoRequest(niktoRequests niktoset,string header) {
			
			string methodGETHEAD=niktoset.method;
			string actualrequest="";
			actualrequest=methodGETHEAD+" "+niktoset.request+" HTTP/1.0\r\n";

			
			actualrequest+=header + "\r\n\r\n";
			return actualrequest;
			
		}

		public string buildRequest(string DirectoryItem, string FileItem, string FileTypeItem,string header) {
			
			string myCombo="";
			string methodGETHEAD="HEAD";

			if (FileTypeItem.Length>0) {
				myCombo=DirectoryItem  + "/" + FileItem + "." + FileTypeItem;
			} 
			else {
				myCombo= DirectoryItem  + "/" + FileItem;
			}

			if ((DirectoryItem.Length == 1) && (FileTypeItem.Length >0)) {
				myCombo=DirectoryItem  + FileItem + "." + FileTypeItem;
			}
			else if ((DirectoryItem.Length == 1) && (FileTypeItem.Length ==0)) {
				myCombo=DirectoryItem + FileItem;

			}

			methodGETHEAD="GET";
			string actualrequest="";
			actualrequest=methodGETHEAD+" "+myCombo+" HTTP/1.0\r\n";

			
			actualrequest+=header + "\r\n";
			return actualrequest;
			
		}
		#endregion

		#endregion

		#region ==> CROWBAR - fuzzing requests
		#region context menu implementation
		private void contextEqual(object sender, System.EventArgs e) {
			try{
				string[] parts = new string[5];
				parts=lstCrowResponse.SelectedItem.ToString().Split(':');
				radioequal.Checked=true;
				updownCrowAI.Value=decimal.Parse(parts[0]);
				btnReCalc_Click_1(null,null);
			}catch{}
		}

		private void contextOutside(object sender, System.EventArgs e) {
			try{
				string[] parts = new string[5];
				parts=lstCrowResponse.SelectedItem.ToString().Split(':');
				radioNotEqual.Checked=true;
				updownCrowAI.Value=decimal.Parse(parts[0]);
				btnReCalc_Click_1(null,null);
			}catch{}
		}

		private void contextshowContent(object sender, System.EventArgs e) {
			try{

				string[] parts = new string[5];
				parts=lstCrowResponse.SelectedItem.ToString().Split(':');
				int ID=Int32.Parse(parts[2]);
				
				foreach (CrowResponses response in CrowResponsesA){
					
					if (response.ID==ID){
						if (response.response.Length>0){
							showresults formres = new showresults();
							formres.Show();
							formres.rtbResults.AppendText(response.response);
						} else {
							MessageBox.Show("No content for this request - did you disable saving of content?");
						}

					}
				}
			}catch{
				MessageBox.Show("No content for this request - did you disable saving of content?");
			}
		}

		private void contextbrowseContent(object sender, System.EventArgs e) {
			try{
				string[] parts = new string[5];
				parts=lstCrowResponse.SelectedItem.ToString().Split(':');
				int ID=Int32.Parse(parts[2]);
				foreach (CrowResponses response in CrowResponsesA){
					if (response.ID==ID){
						if (response.response.Length>0){
							StreamWriter writetemp = new StreamWriter("c:\\crowbar-temp.html",false);
							string[] partswrite = response.response.Replace("\r","").Split('\n');
							int ii=0;
							for (ii=0; partswrite[ii].Length>0; ii++){}
							for (int j=ii+1; j<partswrite.Length; j++){
								writetemp.WriteLine(partswrite[j]);
							}
							writetemp.Close();
							InitBrowser(800,800,0,0);
							object o 	= null;
							BrowseRequest("file:\\\\c:\\crowbar-temp.html",ref o , ref o);
							//Thread.Sleep(1000);
							File.Delete("c:\\crowbar-temp.html");
						} else {
							MessageBox.Show("No content for this request - did you disable saving of content?");
						}
					}
				}
			}catch(Exception ex){
				if (ex.ToString().IndexOf("file")<=0){
					MessageBox.Show("No content for this request - did you disable saving of content?");
				}
			}
		}
		#endregion

		#region threads - Crowbar
		//parent thread 
		public delegate void StartTask();
		public void StartThread() {
			
			StartTask del= new StartTask(StartCrow);
			AsyncCallback callBackWhenDone = new AsyncCallback(this.EndStartThread);
			del.BeginInvoke(callBackWhenDone,null);
			
		}
		public void EndStartThread(IAsyncResult arResult) {
			MethodInvoker mi = new MethodInvoker(this.UpdateUI);
			this.BeginInvoke(mi);
		}

		private void UpdateUI() {
			
		}

		private void btnCrowStart_Click(object sender, System.EventArgs e) {
			
		
			if (txtHTTPdetails.Text.IndexOf("FUZZCTRL")>=0){
				radioAll.Checked=true;
				btnCrowStart.Enabled=false;
				btnCrowStop.Enabled=true;
				btnCrowPause.Enabled=true;

				
				StartThread();
			} else {
				MessageBox.Show("The HTTP(s) request does not contain a fuzzpoint.\r\nPlease select a fuzz point by marking it with 'FUZZCTRL'.");
			}
		}

		//worker thread 
		public delegate void StartTaskWorker(string target, string port,string changer, string comment,CrowResponses crowadd);
		public void StartThreadWorker(string target, string port,string changer, string comment,CrowResponses crowadd) {
			
			StartTaskWorker del= new StartTaskWorker(StartCrowWorker);
			AsyncCallback callBackWhenDoneWorker = new AsyncCallback(this.EndStartThreadWorker);
			del.BeginInvoke(target,port,changer,comment,crowadd,callBackWhenDoneWorker,null);
			
		}
		public void EndStartThreadWorker(IAsyncResult arResult) {
			MethodInvoker mi = new MethodInvoker(this.UpdateUIWorker);
			this.BeginInvoke(mi);
		}

		private void UpdateUIWorker() {
			
		}
		#endregion


		private void btnFileLoad1_Click(object sender, System.EventArgs e) {
			DialogResult opened=openFileDialog1.ShowDialog();
			radioFile1.Checked=true;
			if (opened==DialogResult.OK){
				lblFile1.Text=openFileDialog1.FileName;
			}
			
			
		}

		private void GetBaseResponse() {
			if (txtHTTPdetails.Text.Length<=0 || txtTargetHost.Text.Length<=0 || txtTargetPort.Text.Length<=0){
				return;
			}
			Cursor.Current=System.Windows.Forms.Cursors.WaitCursor;
			cleanHTTP();
		
			//maybe they changed a post...
			string newheader=ComputeNewContentLength(txtHTTPdetails.Text);
			newheader+="\r\n";
			bool useSSL=chkTargetIsSSL.Checked;

			if (useSSL){
				baseResponse=sendraw(txtTargetHost.Text,txtTargetPort.Text,newheader,65535,(int)updownTimeOut.Value,2,useSSL);
					                         
			} else {
				baseResponse=sendraw(txtTargetHost.Text,txtTargetPort.Text,newheader,65535,(int)updownTimeOut.Value);
			}
			
			txtCrowResponse.Text=baseResponse;
			btnCrowStart.Enabled=true;
			
		}
		

		private string findnext (){

			lock (this){
		
				bool twovars=false;
				if (txtHTTPdetails.Text.IndexOf("FUZZCTRL")>=0 && txtHTTPdetails.Text.IndexOf("##2##")>=0){
					twovars=true;
				} else {
					twovars=false;
				}
				string ret="";
				string firstthing="";
				string secondthing="";
			
				string formatstring1="";
				formatstring1="d"+txtNumericFrom1.Text.Length.ToString();
					
				
				
						
				// THE FIRST PARAMETER...
				//lets see if its numbers
				if (radioNumeric1.Checked==true){
					if (basenumber1 < Convert.ToInt32(txtNumericTo1.Text)){
						basenumber1++;
						firstthing=basenumber1.ToString(formatstring1);
					
					} else {
						if (twovars==false){
							return "DONE";
						} else {
							basenumber1=Convert.ToInt32(txtNumericFrom1.Text);
							ret="CYCLE";
							firstthing=basenumber1.ToString(formatstring1);
						}
					}
				}
			
				string nextword1="";
				string nextword2="";
				//lets see if its a file
				if (radioFile1.Checked==true){
					nextword1=wordfromfile(file1Pos,1);
					file1Pos+=(nextword1.Length+2);
					if (nextword1.CompareTo("FILEDONE")==0){
						if (twovars==false){
							return "DONE";
						}else {
							ret="CYCLE";
							firstthing=wordfromfile(0,1);
							file1Pos=firstthing.Length+2;
						}
					} else {
						firstthing=nextword1;
					}
				}

				
				return CrowMangle(firstthing);
				
				
			}
		}

		private string CrowMangle(string passed){
			//mode 0 - md5
			//mode 1 - sha1
			//mode 2 - base 64 encode
			//mode 3 - base 64 decode
			//mode 4 - hex encode
			if (comboCrowEncode.Text.Equals("No encoding")){
				return passed;
			}
			if (comboCrowEncode.Text.Equals("MD5")){
				return GetMangle(passed,0);
			}
			if (comboCrowEncode.Text.Equals("SHA1")){
				return GetMangle(passed,1);
			}
			if (comboCrowEncode.Text.Equals("B64e")){
				return GetMangle(passed,2);
			}
			if (comboCrowEncode.Text.Equals("B64d")){
				return GetMangle(passed,3);
			}
			if (comboCrowEncode.Text.Equals("Hex")){
				return GetMangle(passed,4);
			}
			if (comboCrowEncode.Text.Equals("ToUpper")){
				return passed.ToUpper();
			}
			if (comboCrowEncode.Text.Equals("Hex")){
				return passed.ToLower();
			}
			return passed;
		}

		private string wordfromfile(long position,int mode){
			FileStream fileStream;
			string filename="";
			if (mode==1){
				filename=lblFile1.Text;
			}
			

			try{
				fileStream = new FileStream(filename, FileMode.Open);
			}
			
			catch (Exception ex){
				MessageBox.Show("The file "+filename+" appears to be invalid:\n\n"+ex.ToString());
				stopit=true;
				return "FILEDONE";
			}

			if (position > fileStream.Length){
				fileStream.Close();
				return "FILEDONE";
			}

			try{
				fileStream.Seek(position,SeekOrigin.Begin);
			} catch (Exception ex){
				fileStream.Close();
				return "FILEDONE";
			}
			if (position >= fileStream.Length){
				fileStream.Close();
				return "FILEDONE";
			}
			char onebyte='x';
			string entry="";
			while (onebyte.ToString().CompareTo("\n")!=0 && position < fileStream.Length){
									
				int reader=fileStream.ReadByte();
				if (reader==-1){
					fileStream.Close();
					return entry.Replace("\r\n","");
				}
				onebyte=(char)reader;
				entry=entry+onebyte.ToString();

			}
			fileStream.Close();
			return entry.Replace("\r\n","");
		}

		
		private Int32 getTotalLength (){
			Int32 one=0;
			Int32 two=1;

			
			if (grpInner.Enabled==true){
				if (radioFile1.Checked==true){
					one=getFileItems(lblFile1.Text);
				}
				if (radioNumeric1.Checked==true){
					one=1+Convert.ToInt32(txtNumericTo1.Text)-Convert.ToInt32(txtNumericFrom1.Text);
				}
			}
			return (one*two);


		}


		private Int32 getFileItems(string filename){
			Int32 length=0;
			try{
				StreamReader reader = new StreamReader(filename);
				string readline="";
				while ((readline = reader.ReadLine()) != null){
					if (readline.StartsWith("##")==false){
						length++;
					}
				}
				reader.Close();
				return length;
			}catch{
				return 0;
			}
		}


		
		// -//////////////////// Parent
		private void StartCrow(){
			cleanHTTP();
			CrowResponsesA.Clear();
			
			
			Cursor.Current=System.Windows.Forms.Cursors.WaitCursor;
			if (chkImmFuzz.Checked || baseResponse.Length==0){
				statusBar1.Text="Getting base response...";
				GetBaseResponse();
			}
			lstCrowResponse.Items.Clear();
			txtCrowResponse.Clear();
			btnCrowStop.Enabled=true;
			btnCrowPause.Enabled=true;
			btnCrowStart.Enabled=false;
			//update button
			button2.Enabled=false;
			btnClear.Enabled=false;
			listView1.Enabled = false;
			txtHTTPdetails.Enabled=false;
			bool isAuto=chkProxyAutoUpdate.Checked;
			chkProxyAutoUpdate.Checked=false;
		
			//lets see if the files are OK...
			if (radioFile1.Checked==true && File.Exists(lblFile1.Text)==false && grpInner.Enabled==true){
				MessageBox.Show("The file "+lblFile1.Text+" doesnt exists...!\n");
				return;
			}
			

			//init
			stopit=false;
			pauseit=false;
			file1Pos=0;
		
			howmany=0;
			basenumber1=Convert.ToInt32(txtNumericFrom1.Text)-1;
		
			int i=0;
			rescount=0;
			//btnCrowPause.BackColor=System.Drawing.Color.Black;

			//lets see how many requests..
			Int32 numberofrequests=getTotalLength();
			if (numberofrequests>2000){
				DialogResult turnoff=MessageBox.Show("This seems like a lot of requests...do you want to turn off saving of content? \r\n[Recommended unless you have LOADS of memory]","Warning",MessageBoxButtons.YesNo);
				if (turnoff==DialogResult.Yes){
					chkCrowStoreResponse.Checked=true;
				}
			}

			//lblnumofreqs.Text=numberofrequests.ToString();
			prgBarCrow.Maximum=(int)numberofrequests;
			prgBarCrow.Value=(int)numberofrequests;
			prgBarCrow.Minimum=0;
			
			try{
	
				string theTarget=txtTargetHost.Text;
				string thePort=txtTargetPort.Text;
				string changer = "";
				
				
				while (1==1){
					crow_running=true;
					if (stopit==true){
						break;
					}

					while (pauseit==true){
						statusBar1.Text="Paused....";
						btnCrowPause.BackColor=System.Drawing.Color.DarkGray;
						Thread.Sleep(250);
						statusBar1.Text="Click on Resume to resume";
						btnCrowPause.BackColor=System.Drawing.Color.DarkGray;
						Thread.Sleep(250);
						btnCrowPause.Enabled=true;
					}
					bool iscomment=false;
					string commented=string.Empty;
					changer=findnext();

					string[] val_com = changer.Split('\t');
					if (val_com.Length>1){
						commented=val_com[val_com.Length-1];
					}

					changer=val_com[0];

					if (changer.StartsWith("##")){
						iscomment=true;
					}
					if (changer.CompareTo("DONE")==0){
						break;
					}
				
					//build the request...
					string strTry=txtHTTPdetails.Text;
					string[] parts = new string[2];
					parts=changer.Replace("%$%","`").Split('`');
					if (parts[0].Length>0){
						strTry=strTry.Replace("FUZZCTRL",convertToHex(parts[0],false));
					}
					
					//new item to add
					CrowResponses crowadd = new CrowResponses();
					if (iscomment==false){
						crowadd.param1=parts[0];
					}
					
				

					//compute the new content-length
					strTry=ComputeNewContentLength(strTry);
					strTry+="\r\n";
					
					if (stopit==true){
						break;
					}

					//see if we can fire more threads...
					while (howmany >= 5 && stopit==false){
						Thread.Sleep(200);
					}
					if (howmany < 5 && pauseit==false && stopit==false){
						if (iscomment==false){
							howmany++;
							StartThreadWorker(theTarget,thePort,strTry,commented,crowadd);
						} 
						//to single thread comment above and uncomment below:
						//StartCrowWorker(theTarget,thePort,strTry,i);
					}

					if (iscomment==false){
						i++;
					}

				}
				crow_running=false;
				listView1.Enabled=true;
				btnCrowStart.Enabled=true;
				txtHTTPdetails.Enabled=true;
				btnCrowStop.Enabled=false;
				btnCrowStart.Enabled=true;
				btnCrowPause.Enabled=false;
				//update but
				button2.Enabled=true;
				btnClear.Enabled=true;
				chkProxyAutoUpdate.Checked=isAuto;

				//	btnBase.Enabled=true;
			}
			catch (Exception ex){
				MessageBox.Show(ex.ToString());
			}
			//wait for all the threads
			while (howmany>0){
				Thread.Sleep(200);
			}

			statusBar1.Text="Done..";
		}

		static readonly object locker = new object();
		//-//////////////////// WORKER
		private void StartCrowWorker(string target,string port,string strTry, string comment,CrowResponses crowadd){

			bool useSSL=chkTargetIsSSL.Checked;
			string response;
			try{
			
				//do the actual TCP request...
				if (useSSL){
					response=sendraw(target,port,strTry,65535,(int)updownTimeOut.Value,2,useSSL);
					                         
				} else {
					response=sendraw(target,port,strTry,65535,(int)updownTimeOut.Value);
				}

				if (stopit==true){
					return;
				}
				lock (this){
					if (stopit==true){
						return;
					}
						
					statusBar1.Text="Testing with "+crowadd.param1+":"+crowadd.param2;
					txtCrowResponse.Text=response;
				
					if (chkCrowStoreResponse.Checked==false){
						crowadd.response=response;
						crowadd.rawreq=strTry;
					}
					lock (this){
						crowadd.compare=compareBlobs(response,baseResponse,chkUseAIAtAll.Checked,true);
					}
					
					crowadd.content=getContentFilter(response,true);
					
					crowadd.ID=rescount;
					crowadd.comment=comment.Replace("%$%","");
					
					rescount++;

					//add it to our list
					CrowResponsesA.Add(crowadd);

					//build the stuff that needs to go into the list
					string listentry=crowadd.compare.ToString()+":"+crowadd.param1+":"+crowadd.ID+":"+crowadd.content+":"+crowadd.comment;
					//if (lstCrowResponse.Items.Count>0){
						lstCrowResponse.SelectedIndex=lstCrowResponse.Items.Count-1;
					//}
					if (radioAll.Checked==true){
						
						lstCrowResponse.Items.Add(listentry);
					}else { 
						//equal
						if (radioequal.Checked==true){
							if (crowadd.compare == Convert.ToDouble(updownCrowAI.Value)){
								lstCrowResponse.Items.Add(listentry);
							}
						} else {
							//inside
							if (radioinside.Checked==true){
								if (crowadd.compare >= Convert.ToDouble(updownCrowAI.Value) && crowadd.compare <= Convert.ToDouble(updowntwo.Value)){
									lstCrowResponse.Items.Add(listentry);
								}
							} else {

								//outside
								if (radiooutside.Checked==true){
									if (crowadd.compare <= Convert.ToDouble(updownCrowAI.Value) || crowadd.compare >= Convert.ToDouble(updowntwo.Value)){
										lstCrowResponse.Items.Add(listentry);
									}
								} else {
									//not equal
									if (radioNotEqual.Checked==true){
										if (crowadd.compare != Convert.ToDouble(updownCrowAI.Value)){
											lstCrowResponse.Items.Add(listentry);
										}
									}
								}
							}
						}
					}
				}
				lock (locker){
					if (prgBarCrow.Value>0){
							prgBarCrow.Increment(-1);
						}
					howmany--;
				}
			} catch (Exception ex){
				MessageBox.Show(ex.ToString());
			}
			
		}


		private string getContentFilter(string content){
			if (stopit==true){
				return "";
			}
			if (txtContentEndWord.Text.Length>0 && txtContentStartWord.Text.Length>0){
				string returner="";
				content=content.Replace("\n"," ");
				content=content.Replace("\r"," ");
				
				int i;
				for (i=0; i<content.Length-(txtContentStartWord.Text.Length); i++){
					string first=content.Substring(i,txtContentStartWord.Text.Length);
					if (first.CompareTo(txtContentStartWord.Text)==0){
						//ok we have the first match
						i+=txtContentStartWord.Text.Length;
						break;
					}
				}
				int t;
				for (t=i; t<content.Length-(txtContentEndWord.Text.Length); t++){
					string last=content.Substring(t,txtContentEndWord.Text.Length);
					if (last.CompareTo(txtContentEndWord.Text)==0){
						break;
					}
					returner=returner+content[t];
				}
				return returner;
			} else {return "";}
		}

		private string getContentFilter(string content,bool revised){
			if (stopit==true){
				return "";
			}
			int startpoint=content.IndexOf(txtContentStartWord.Text)+txtContentStartWord.Text.Length;
			string remainder=content.Substring(startpoint);
			int endpoint=remainder.IndexOf(txtContentEndWord.Text);
			if (endpoint>0){
				return remainder.Substring(0,endpoint);
			} else {
				return "No Content To Extract";
			}

		}


		private void btnReCalc_Click_1(object sender, System.EventArgs e) {
		
			lstCrowResponse.Items.Clear();
			foreach (CrowResponses response in CrowResponsesA ){
				string listitem=response.compare+":"+response.param1+":"+response.ID+":"+response.content+":"+response.comment;
				if (radioAll.Checked==true){
					lstCrowResponse.Items.Add(listitem);
				} else {
					//equal
					if (radioequal.Checked==true){
						if (response.compare == Convert.ToDouble(updownCrowAI.Value)){
							lstCrowResponse.Items.Add(listitem);
						}
					}else{
						//inside
						if (radioinside.Checked==true){
							if (response.compare >= Convert.ToDouble(updownCrowAI.Value) && response.compare <= Convert.ToDouble(updowntwo.Value)){
								lstCrowResponse.Items.Add(listitem);
							}
						} else {
							//outside
							if (radiooutside.Checked==true){
								if (response.compare <= Convert.ToDouble(updownCrowAI.Value) || response.compare >= Convert.ToDouble(updowntwo.Value)){
									lstCrowResponse.Items.Add(listitem);
								}
							} else {
								//not equal
								if (radioNotEqual.Checked==true){
									if (response.compare != Convert.ToDouble(updownCrowAI.Value)){
										lstCrowResponse.Items.Add(listitem);
									}
								}
							}
						}
					}
				}
			}
		}

		#region auto group
		private void btnCrowGroup_Click(object sender, System.EventArgs e) {
			if (CrowResponsesA.Count<=0){
				MessageBox.Show("Currently, there is nothing to group - I suggest you first do something with a parameter");
				return;
			}
			if (CrowResponsesA.Count>2000){
				MessageBox.Show("Sjoe - there many to group - this might take a while...\r\nPlease be a patient..");
			}
			try{
				autogroup.Dispose();
				autogroup.Show();
				autogroup.Controls.Clear();
			} catch {
				
				autogroup = new AutoGroup();
				autogroup.Show();
				autogroup.Controls.Clear();
				autogroup.AutoScale=true;
				autogroup.AutoScroll=true;
				autogroup.BackColor=Color.DarkGray;
			}

			//copy all the results so we can work with it
			ArrayList work = new ArrayList();
			work.AddRange(CrowResponsesA);

			//now start with the first response
			int pos=0;
			
			Label heading = new Label();
			heading.Size=new Size(300,20);
			heading.Location=new Point(5,5);

			while (work.Count>0){

				double currentval = ((CrowResponses)work[0]).compare;
				MajorGroup[pos]= new RichTextBox();
				MajorGroup[pos].WordWrap=false;
				MajorGroup[pos].BackColor=Color.Gainsboro;
				MajorGroup[pos].ForeColor=Color.DarkSlateGray;
				MajorGroup[pos].ScrollBars=RichTextBoxScrollBars.Both;
				MajorGroup[pos].Location = new Point(5,20+(pos*175));
				MajorGroup[pos].BorderStyle=BorderStyle.FixedSingle;
				MajorGroup[pos].Size = new Size(500,150);
				MajorGroup[pos].Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				autogroup.Controls.Add(MajorGroup[pos]);

			
				
				
				//check all of them
				ArrayList removethese = new ArrayList();
				//storing the last index of the compared list
				int keepnumber=0;
				bool timeouted=false;
				foreach (CrowResponses item in work){
					if (item.compare <= currentval+(double)updownGroupTolerance.Value && item.compare >= currentval-(double)updownGroupTolerance.Value){
						if (item.compare==-1){
							timeouted=true;
							MajorGroup[pos].BackColor=Color.Tan;
						}
						MajorGroup[pos].Text+=item.compare+":"+item.param1+":"+item.content+":"+item.comment+"\r\n";
						removethese.Add(item);
						//make sure we get the first one
						if (keepnumber == 0){
							keepnumber=item.ID;
						}
					}
				}
				if (removethese.Count>0){
					foreach (CrowResponses rem in removethese){
						work.Remove(rem);
					}
				}
				if (timeouted==false){
					//add the button
					GroupButtons[pos]=new DotNetSkin.SkinControls.SkinButton();
					GroupButtons[pos].BackColor=Color.DarkGray;
					GroupButtons[pos].Location=new Point(5,170+(pos*175));
					GroupButtons[pos].Size=new Size(240,25);
					GroupButtons[pos].Text="^Browse response^";
					GroupButtons[pos].Tag=keepnumber;
					GroupButtons[pos].Click+=new EventHandler(clickGroupShowResponse);
					autogroup.Controls.Add(GroupButtons[pos]);

					//add the button - raw 
					GroupButtonsRaw[pos]=new DotNetSkin.SkinControls.SkinButtonYellow();
					GroupButtonsRaw[pos].BackColor=Color.DarkGray;
					GroupButtonsRaw[pos].Location=new Point(260,170+(pos*175));
					GroupButtonsRaw[pos].Size=new Size(240,25);
					GroupButtonsRaw[pos].Text="^Show Raw Response^";
					GroupButtonsRaw[pos].Tag=keepnumber;
					GroupButtonsRaw[pos].Click+=new EventHandler(clickGroupShowResponseRaw);
					autogroup.Controls.Add(GroupButtonsRaw[pos]);
				}
				pos++;
			}
			heading.Text="Unique response count: ["+pos.ToString()+"]";
			
			int sizeh=230+((pos-1)*175);
			int sizel=520;
			if (sizeh>600){
				sizeh=600;
				//scrollbar
				sizel=527;
			}
			autogroup.Size=new Size(sizel,sizeh);
			autogroup.Controls.Add(heading);
		}

		
		private void clickGroupShowResponse(object sender, System.EventArgs e) {
			Button fakebut = new Button();
			fakebut=(Button)sender;
			try{	
				InitBrowser(800,800,0,0);
				int ID=(int)fakebut.Tag;
				foreach (CrowResponses response in CrowResponsesA){
					if (response.ID==ID){
						StreamWriter writetemp = new StreamWriter("c:\\crowbar-temp-group.html",false);
						string[] partswrite = response.response.Replace("\r","").Split('\n');
						int ii=0;
						for (ii=0; partswrite[ii].Length>0; ii++){}
						for (int j=ii+1; j<partswrite.Length; j++){
							writetemp.WriteLine(partswrite[j]);
						}
						writetemp.Close();
						object o 	= null;
						Thread.Sleep(200);
						BrowseRequest("file:\\\\c:\\crowbar-temp-group.html",ref o , ref o);
						Thread.Sleep(200);
						File.Delete("c:\\crowbar-temp-group.html");
					}
				}
					
			}catch{}
		}

		private void clickGroupShowResponseRaw(object sender, System.EventArgs e) {
			Button fakebut = new Button();
			fakebut=(Button)sender;
			try{	
				int ID=(int)fakebut.Tag;
				foreach (CrowResponses response in CrowResponsesA){
					if (response.ID==ID){
						showresults resultwin = new showresults();
						resultwin.Controls.Remove(resultwin.rtbResults);
						RichTextBox box = new RichTextBox();
						box.Dock=DockStyle.Fill;
						box.Text=response.response;
						box.BackColor=Color.Snow;
						box.ForeColor=Color.Black;
						box.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F);
						
						int height=response.response.Split('\n').Length * 18;
						if (height>600){
							height=600;
						}

						resultwin.Size=new Size(500,height);
						resultwin.Text="Raw result";
						resultwin.Controls.Add(box);
						resultwin.Show();
					}
				}
					
			}catch{}
		}
		#endregion



		private void btnCrowStop_Click(object sender, System.EventArgs e) {
			stopit=true;
			btnCrowStart.Enabled=true;
			btnCrowStop.Enabled=false;
			btnCrowPause.Enabled=false;
		
			btnCrowPause.BackColor=System.Drawing.Color.DarkGray;
			btnCrowPause.ForeColor=Color.Black;
			btnCrowPause.Text="Pause";
			pauseit=false;
			statusBar1.Text="Stopped...";
			howmany=0;
		}

		

		private void btnExport_Click(object sender, System.EventArgs e) {
			if (lstCrowResponse.Items.Count<=0){
				return;
			}
			showresults formres = new showresults();
			formres.Show();
			foreach (string item in lstCrowResponse.Items){
				/*
				string[] parts = new string[5];
				parts=item.Split(':');
				string display = parts[0]+":"+parts[1]+":"+parts[2]+":"+parts[4];
				string remain="";
				for (int y=3; y<parts.Length; y++){
					remain+=parts[y];
				}
				display+=":"+remain;

*/
				formres.rtbResults.BackColor=Color.Snow;
				formres.rtbResults.ForeColor=Color.Black;
				formres.rtbResults.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.75F);
				int height=lstCrowResponse.Items.Count * 18;
				if (height>600){
					height=600;
				}

				formres.Size=new Size(500,height);
				formres.Text="Exported Results";

				formres.rtbResults.AppendText(item+"\r\n");
			}
		}
		

		private void btnCrowPause_Click(object sender, System.EventArgs e) {
			if (btnCrowPause.Text.CompareTo("Pause")==0){
				btnCrowPause.Text="Resume";
				pauseit=true;
				btnCrowPause.BackColor=System.Drawing.Color.DarkGray;

				return;
			}
			if (btnCrowPause.Text.CompareTo("Resume")==0){
				btnCrowPause.Text="Pause";
				btnCrowPause.BackColor=System.Drawing.Color.DarkGray;
				pauseit=false;
				return;
			}
		}

		private void txtHTTPdetailsChange(object sender, System.EventArgs e) {
			
			if (txtHTTPdetails.Text.IndexOf("FUZZCTRL")>=0){
				grpInner.Enabled=true;
			} else {
				grpInner.Enabled=false;
			}
			
		}

		


		#region crow browsers
		public void BrowseRequest(string url, ref object objPostData, ref object objHeaders)	{
			object o 	= null;
			object objUrl	= (object)url;
			try	{
				ie.Navigate2(ref objUrl, ref o, ref o, ref objPostData, ref objHeaders);	
			}
			catch ( Exception ex )	{
				Console.WriteLine(ex.Message);	}
		}	
		public void InitBrowser(decimal wide, decimal high, decimal left, decimal top) {
			try {
				ie					= new InternetExplorer();
				ie.Visible				= true;
				ie.MenuBar				= false;
				ie.Height				= (int)high;
				ie.Width				= (int)wide;
				ie.Left					= (int)left;
				ie.Top					= (int)top;
				
				ie.MenuBar				= true;
				ie.Resizable			= true;
				ie.AddressBar			= true;
				ie.StatusBar			= true;
				ie.TheaterMode		= false;
				
			}
			catch ( Exception ex )	{	
				Console.WriteLine("Captured exception " + ex.Message);
			}
		}

		#endregion

		private void lstCrowResponse_SelectedIndexChanged(object sender, System.EventArgs e) {
			if (crow_running){
				MessageBox.Show("Cant do this while in a run - wait for it to end or click stop");
				return;
			} 
			try{
				string[] parts = new string[5];
				parts=lstCrowResponse.SelectedItem.ToString().Split(':');
				int ID=Int32.Parse(parts[2]);
				foreach (CrowResponses response in CrowResponsesA){
					if (response.ID==ID){
						
						txtCrowResponse.Text=response.response;
						txtHTTPdetails.Text=response.rawreq;
						btnReplay.PerformClick();
						break;
					}
				
				}
			}catch{}
		}

		private void lstCrowResponse_SelectedIndexChanged_1(object sender, System.EventArgs e) {
			lock (this){
				if (crow_running==true){
					return;
				}
			}
			//see if there's an ie open
				string leftdisplay=string.Empty;
				try{
					string[] parts = new string[5];
					parts=lstCrowResponse.SelectedItem.ToString().Split(':');
					int ID=Int32.Parse(parts[2]);
					foreach (CrowResponses response in CrowResponsesA){
						
						if (response.ID==ID){
							leftdisplay=response.response;
							if (ie.FullName.Length>1 && response.response.Length>0){
								StreamWriter writetemp = new StreamWriter("c:\\crowbar-temp.html",false);
								string[] partswrite = response.response.Replace("\r","").Split('\n');
								int ii=0;
								for (ii=0; partswrite[ii].Length>0; ii++){}
								for (int j=ii+1; j<partswrite.Length; j++){
									writetemp.WriteLine(partswrite[j]);
								}
								writetemp.Close();
								object o 	= null;
								Thread.Sleep(200);
								BrowseRequest("file:\\\\c:\\crowbar-temp.html",ref o , ref o);
								Thread.Sleep(200);
								File.Delete("c:\\crowbar-temp.html");
							}
							//display left hand side
							txtCrowResponse.Text=leftdisplay;	
						}
					}
				}catch{
					//display left hand side
					txtCrowResponse.Text=leftdisplay;
				}
		}

		


		private void btnUpdateQuickNav_Click(object sender, System.EventArgs e) {
			QuickNavUpdate(txtFuzzDirLocation.Text,"*.txt");
		}

		private void btnManualBaseResponse_Click(object sender, System.EventArgs e) {
			GetBaseResponse();
		}

		private void cmbCustom_SelectedIndexChanged(object sender, System.EventArgs e) {
			radioFile1.Checked=true;
			lblFile1.Text=txtFuzzDirLocation.Text+cmbCustom.SelectedItem.ToString();
		}
		#endregion

		#region ==> Search and Replace front end
		private void btnSRAdd_Click(object sender, System.EventArgs e) {
			lstSRlist.Items.Add(txtSRSearch.Text+"\t"+txtSRReplace.Text);
			SRList.Add(txtSRSearch.Text+"\t"+txtSRReplace.Text);
		}

		private void btnSRDelete_Click(object sender, System.EventArgs e) {
			try{
				string selitem=lstSRlist.SelectedItem.ToString();
				ArrayList newlist = new ArrayList();
				foreach (string item in SRList){
					if (item.Equals(selitem)==false){
						newlist.Add(item);
					}
				}
				SRList.Clear();
				SRList=newlist;
				lstSRlist.Items.Clear();

				foreach (string listitem in SRList){
					lstSRlist.Items.Add(listitem);
				}
			}catch{}
		}

		private void btnSRDeleteOut_Click(object sender, System.EventArgs e) {
			try{
				string selitem=lstSRIncoming.SelectedItem.ToString();
				ArrayList newlist = new ArrayList();
				foreach (string item in SRListIncoming){
					if (item.Equals(selitem)==false){
						newlist.Add(item);
					}
				}
				SRListIncoming.Clear();
				SRListIncoming=newlist;
				lstSRIncoming.Items.Clear();

				foreach (string listitem in SRListIncoming){
					lstSRIncoming.Items.Add(listitem);
				}
			} catch{}
		}

		private void btnSRAddOut_Click(object sender, System.EventArgs e) {
			lstSRIncoming.Items.Add(txtSRSearchIn.Text+"\t"+txtSRReplaceIn.Text);
			SRListIncoming.Add(txtSRSearchIn.Text+"\t"+txtSRReplaceIn.Text);
		}

		private void lstSRlist_SelectedIndexChanged(object sender, System.EventArgs e) {
			try{
				string thestr=lstSRlist.SelectedItem.ToString();
				string[] parts = thestr.Split('\t');
				txtSRReplace.Text=parts[1];
				txtSRSearch.Text=parts[0];
			} catch{}
		}

		private void lstSRIncoming_SelectedIndexChanged(object sender, System.EventArgs e) {
			try{
				string thestr=lstSRIncoming.SelectedItem.ToString();
				string[] parts = thestr.Split('\t');
				txtSRReplaceIn.Text=parts[1];
				txtSRSearchIn.Text=parts[0];
			} catch{}
		}
		#endregion
		
		#region ==> Fuzz File Editor & update
		private void QuickNavUpdate(string path, string mask){
			cmbCustom.Items.Clear();
			cmbFuzzFileEdit.Items.Clear();
			try{
				System.IO.DirectoryInfo dirinfo = new DirectoryInfo(path);
				System.IO.FileInfo[] filelist = dirinfo.GetFiles(mask);
				foreach (System.IO.FileInfo file in filelist){
					cmbCustom.Items.Add(file.ToString());
					cmbFuzzFileEdit.Items.Add(file.ToString());
				}
			} catch {
				
			}

		}
		
		private void cmbFuzzFileEdit_SelectedIndexChanged(object sender, System.EventArgs e) {
			txtConfigFuzzFileName.Text=cmbFuzzFileEdit.SelectedItem.ToString();
			txtConfigFuzzContent.Text="";
			StreamReader fileopen = new StreamReader(txtFuzzDirLocation.Text+cmbFuzzFileEdit.SelectedItem.ToString());
			try{
				string readline="";
				while ( (readline=fileopen.ReadLine()) != null){
					txtConfigFuzzContent.Text+=readline+"\r\n";
				}
			}catch {
				MessageBox.Show("Cannot open the file: "+txtConfigFuzzFileName.Text);
			}
			fileopen.Close();
		}

		private void btnSaveFuzzFile_Click(object sender, System.EventArgs e) {
			try{
				StreamWriter filewrite = new StreamWriter(txtFuzzDirLocation.Text+txtConfigFuzzFileName.Text,false);
				foreach (string line in txtConfigFuzzContent.Lines){
					filewrite.WriteLine(line);
				}
				filewrite.Close();
				MessageBox.Show("File "+txtFuzzDirLocation.Text+txtConfigFuzzFileName.Text+" saved");
				QuickNavUpdate(txtFuzzDirLocation.Text,"*.txt");
			} catch{}

		
		}
		private void btnUpdateQuickNav_Click_1(object sender, System.EventArgs e) {
			QuickNavUpdate(txtFuzzDirLocation.Text,"*.txt");
		}

		private void btnFuzzDBLocationFind_Click(object sender, System.EventArgs e) {
			DialogResult didheopen = folderBrowserDialog1.ShowDialog();
			if (didheopen != DialogResult.OK){
				return;
			}
			if (folderBrowserDialog1.SelectedPath.Length > 0){
				cmbFuzzFileEdit.Items.Clear();
				cmbFuzzFileEdit.Text="";
				txtConfigFuzzFileName.Clear();

				txtFuzzDirLocation.Text=folderBrowserDialog1.SelectedPath+"\\";
				QuickNavUpdate(txtFuzzDirLocation.Text,"*.txt");
			} 
		}

		#endregion

		#region ==>Mangling, MD5, SHA1 etc

		private Mangled getMangleSet(string pair,string type, char separator){
			Mangled process = new Mangled();

			process.varval="UD";
			process.varvalmd5="UD";
			process.varvalsha1="UD";
			process.varvalbase64enc="UD";
			process.varvalbase64dec="UD";
			process.varname="UD";
			process.varmd5="UD";
			process.varsha1="UD";
			process.varbase64enc="UD";
			process.varbase64dec="UD";
			
			try{
				int sep=pair.IndexOf(separator.ToString());
				string par=pair.Substring(0,sep);
				string val=pair.Substring(sep+1);

				process.varname=par;
				process.varmd5=GetMangle(par,0);
				process.varsha1=GetMangle(par,1);
				process.varbase64enc=GetMangle(par,2);
				process.varbase64dec=GetMangle(par,3);

				if (val.Length>0){
					process.varval=val;
					process.varvalmd5=GetMangle(val,0);
					process.varvalsha1=GetMangle(val,1);
					process.varvalbase64enc=GetMangle(val,2);
					process.varvalbase64dec=GetMangle(val,3);
				}
				process.type=type;
				return (process);
			}catch{
				return process;
			}
		}

		static public string MD5SUM(byte[] FileOrText) { 
			return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(FileOrText)).Replace("-","").ToLower();
		}
		static public string SHA1SUM(byte[] FileOrText) { 
			return BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(FileOrText)).Replace("-","").ToLower();
		}

		private string GetMangle(string str,int mode) {
			//mode 0 - md5
			//mode 1 - sha1
			//mode 2 - base 64 encode
			//mode 3 - base 64 decode
			//mode 4 - hex encode
			string modestr="---"+mode.ToString();
			if (MyCryptoCache.Contains(str+modestr)){
				return (string)MyCryptoCache[str+modestr];
			}

			if (mode==0){
				string temp=MD5SUM(System.Text.Encoding.ASCII.GetBytes(str));
				MyCryptoCache[str+modestr]=temp;
				return temp;
			}
			if (mode==1){
				string temp= SHA1SUM(System.Text.Encoding.ASCII.GetBytes(str));
				MyCryptoCache[str+modestr]=temp;
				return temp;
			}
			
			if (mode==2){
				try {
					byte[] encData_byte = new byte[str.Length];
					encData_byte = System.Text.Encoding.UTF8.GetBytes(str);    
					string encodedData = Convert.ToBase64String(encData_byte);
					MyCryptoCache[str+modestr]=encodedData;
					return encodedData;
				}
				catch(Exception e) {
					MyCryptoCache[str+modestr]="Cannot Base64 encode string";
					return "Cannot Base64 encode string";
				}
			}
			if (mode==3){
				try {
					System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();  
					System.Text.Decoder utf8Decode = encoder.GetDecoder();
    
					byte[] todecode_byte = Convert.FromBase64String(str);
					int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);    
					char[] decoded_char = new char[charCount];
					utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);                   
					string result = new String(decoded_char);
					MyCryptoCache[str+modestr]=result;
					return result;
				}
				catch(Exception e) {
					MyCryptoCache[str+modestr]="Cannot Base64 decode string";
					return "Cannot Base64 decode string";
				}
			}
			if (mode==4){
				return (convertToHex_for_real(str));
				

			}



			return "Unknown hash type";

			
		}

		

		private void btnToolsGo_Click(object sender, System.EventArgs e) {
			txtToolsMD5Sum.Text=GetMangle(txtToolsUuserInput.Text,0);
			txtToolsSHA1.Text=GetMangle(txtToolsUuserInput.Text,1);
			txtToolsBase64Encoded.Text=GetMangle(txtToolsUuserInput.Text,2);
			txtToolsBase64Decoded.Text=GetMangle(txtToolsUuserInput.Text,3);
			txtToolsHex.Text=GetMangle(txtToolsUuserInput.Text,4);
		}

		//mangled run...
		//parent thread 
		public delegate void StartTaskMan();
		public void StartThreadMan() {
			
			StartTaskMan del= new StartTaskMan(AnalyzeMangle);
			AsyncCallback callBackWhenDone = new AsyncCallback(this.EndStartThreadMan);
			del.BeginInvoke(callBackWhenDone,null);
			
		}
		public void EndStartThreadMan(IAsyncResult arResult) {
			MethodInvoker mi = new MethodInvoker(this.UpdateUI);
			this.BeginInvoke(mi);
		}

		
		private void AnalyzeMangle(){
			ArrayList work_detailed_request = new ArrayList();
			
			work_detailed_request.AddRange(detailed_Requests.GetRange(0,detailed_Requests.Count));

			//first add the user defined
			foreach (detailedRequest item in userMange_detailed_Requests){
				work_detailed_request.Add(item);
			}
			txtAutoMangle.Text="Working...\r\n";
			try{
				//each request
				foreach (detailedRequest work in work_detailed_request ){
				
					//each parameter
					foreach (Mangled item_mangled in work.Processed){
					
						//each request
						foreach (detailedRequest work_inner in work_detailed_request){		
							
							//each parameter
							foreach (Mangled item_mangled_inner in work_inner.Processed){

								if (item_mangled.varname.Equals(item_mangled_inner.varname) && work.URL.Equals(work_inner.URL) && item_mangled.type.Equals(item_mangled_inner.type)){
									break;
								}
								if (item_mangled.varval.Length>4 && item_mangled_inner.varval.Length>4 && item_mangled.varvalbase64enc.Length>4 && item_mangled_inner.varvalbase64enc.Length>4 && item_mangled.varvalbase64dec.Length>4 && item_mangled_inner.varvalbase64dec.Length>4 ){
									
									#region one way
									//SHA1 
									if (item_mangled_inner.varval.IndexOf(item_mangled.varvalsha1)>=0){
										txtAutoMangle.Text+="U["+work_inner.URL+"] T["+item_mangled_inner.type+"] Var["+item_mangled_inner.varname+"] Val["+item_mangled_inner.varval+"] H["+work_inner.host+"]\r\n";
										txtAutoMangle.Text+="U["+work.URL+"] T["+item_mangled.type+"] Var["+item_mangled.varname+"] Val["+item_mangled.varval+"]=SHA1=>["+item_mangled.varvalsha1+"] H["+work.host+"]\r\n\r\n";
									}

									//MD5
									if (item_mangled_inner.varval.IndexOf(item_mangled.varvalmd5)>=0){
										txtAutoMangle.Text+="U["+work_inner.URL+"] T["+item_mangled_inner.type+"] Var["+item_mangled_inner.varname+"] Val["+item_mangled_inner.varval+"] H["+work_inner.host+"]\r\n";
										txtAutoMangle.Text+="U["+work.URL+"] T["+item_mangled.type+"] Var["+item_mangled.varname+"] Val["+item_mangled.varval+"]=MD5=>["+item_mangled.varvalmd5+"] H["+work.host+"]\r\n\r\n";

									}

									//B64Decode
									if (item_mangled_inner.varval.IndexOf(item_mangled.varvalbase64dec)>=0){
										txtAutoMangle.Text+="U["+work_inner.URL+"] T["+item_mangled_inner.type+"] Var["+item_mangled_inner.varname+"] Val["+item_mangled_inner.varval+"] H["+work_inner.host+"]\r\n";
										txtAutoMangle.Text+="U["+work.URL+"] T["+item_mangled.type+"] Var["+item_mangled.varname+"] Val["+item_mangled.varval+"]=B64D=>["+item_mangled.varvalbase64dec+"] H["+work.host+"]\r\n\r\n";
									}

									//B64Encode
									if (item_mangled_inner.varval.IndexOf(item_mangled.varvalbase64enc)>=0){
										txtAutoMangle.Text+="U["+work_inner.URL+"] T["+item_mangled_inner.type+"] Var["+item_mangled_inner.varname+"] Val["+item_mangled_inner.varval+"] H["+work_inner.host+"]\r\n";
										txtAutoMangle.Text+="U["+work.URL+"] T["+item_mangled.type+"] Var["+item_mangled.varname+"] Val["+item_mangled.varval+"]=B64E=>["+item_mangled.varvalbase64enc+"] H["+work.host+"]\r\n\r\n";
									}
									#endregion
								
									#region other way
									//SHA1 
									if (item_mangled_inner.varvalsha1.IndexOf(item_mangled.varval)>=0){
										txtAutoMangle.Text+="U["+work_inner.URL+"] T["+item_mangled_inner.type+"] Var["+item_mangled_inner.varname+"] Val["+item_mangled_inner.varval+"]=SHA1=>["+item_mangled_inner.varvalsha1+"] H["+work_inner.host+"]\r\n";
										txtAutoMangle.Text+="U["+work.URL+"] T["+item_mangled.type+"] Var["+item_mangled.varname+"] Val["+item_mangled.varval+"] H["+work.host+"]\r\n\r\n";
									}

									//MD5
									if (item_mangled_inner.varvalmd5.IndexOf(item_mangled.varval)>=0){
										txtAutoMangle.Text+="U["+work_inner.URL+"] T["+item_mangled_inner.type+"] Var["+item_mangled_inner.varname+"] Val["+item_mangled_inner.varval+"]=MD5=>["+item_mangled_inner.varvalmd5+"] H["+work_inner.host+"]\r\n";
										txtAutoMangle.Text+="U["+work.URL+"] T["+item_mangled.type+"] Var["+item_mangled.varname+"] Val["+item_mangled.varval+"] H["+work.host+"]\r\n\r\n";
									}

									//B64Decode
									if (item_mangled_inner.varvalbase64dec.IndexOf(item_mangled.varval)>=0){
										txtAutoMangle.Text+="U["+work_inner.URL+"] T["+item_mangled_inner.type+"] Var["+item_mangled_inner.varname+"] Val["+item_mangled_inner.varval+"]=B64D=>["+item_mangled_inner.varvalbase64dec+"] H["+work_inner.host+"]\r\n";
										txtAutoMangle.Text+="U["+work.URL+"] T["+item_mangled.type+"] Var["+item_mangled.varname+"] Val["+item_mangled.varval+"] H["+work.host+"]\r\n\r\n";
									}

									//B64Encode
									if (item_mangled_inner.varvalbase64enc.IndexOf(item_mangled.varval)>=0){
										txtAutoMangle.Text+="U["+work_inner.URL+"] T["+item_mangled_inner.type+"] Var["+item_mangled_inner.varname+"] Val["+item_mangled_inner.varval+"]=B64E=>["+item_mangled_inner.varvalbase64enc+"] H["+work_inner.host+"]\r\n";
										txtAutoMangle.Text+="U["+work.URL+"] T["+item_mangled.type+"] Var["+item_mangled.varname+"] Val["+item_mangled.varval+"] H["+work.host+"]\r\n\r\n";
									}
									#endregion
								}
							}
						}
					}
				}
			} catch{}
			txtAutoMangle.Text+="Done...";
		}

		
		private void timer_mangled_Tick(object sender, System.EventArgs e) {
			if (chkAutoMangle.Checked){
				StartThreadMan();
			}
		}

		private void btnAnaliseMangle_Click(object sender, System.EventArgs e) {
			StartThreadMan();
		}

		private void btnMangleAddUserInput_Click(object sender, System.EventArgs e) {
			detailedRequest workreq = new detailedRequest();
			Mangled workman = new Mangled();

			workreq.header="";
			workreq.action="UsrDef";
			workreq.cookie=null;
			//this here is a 'hack'..hehehehe - see the delete code
			workreq.filename=txtMangleUserInput.Text;
			workreq.filetype="";
			workreq.GETparameters=null;
			workreq.isSSL=false;
			workreq.port="0";
			workreq.POSTparameters=null;
			workreq.URL="UsrDef";
			workreq.host="UsrDef";
			workreq.Processed=new ArrayList();
		
			workman.type="UsrDef";
			workman.varname="UsrDef";
			workman.varval=txtMangleUserInput.Text;
			workman.varvalmd5=GetMangle(workman.varval,0);
			workman.varvalsha1=GetMangle(workman.varval,1);
			workman.varvalbase64dec=GetMangle(workman.varval,2);
			workman.varvalbase64enc=GetMangle(workman.varval,3);

			workreq.Processed.Add(workman);
			userMange_detailed_Requests.Add(workreq);

			lstMangleUserInput.Items.Add(txtMangleUserInput.Text);
		}

		private void btnMangleDeleteUserInput_Click(object sender, System.EventArgs e) {
			try{
				string selitem=lstMangleUserInput.SelectedItem.ToString();
				ArrayList newlist = new ArrayList();
				foreach (detailedRequest item in userMange_detailed_Requests){
					if (item.filename.Equals(selitem)==false){
						newlist.Add(item);
					}
				}
				userMange_detailed_Requests.Clear();
				userMange_detailed_Requests=newlist;
				lstMangleUserInput.Items.Clear();

				foreach (detailedRequest listitem in userMange_detailed_Requests){
					lstMangleUserInput.Items.Add(listitem.filename);
				}
			} catch{}
		
		}

		private void lstMangleUserInput_SelectedIndexChanged(object sender, System.EventArgs e) {
			try{
				string thestr=lstMangleUserInput.SelectedItem.ToString();
				txtMangleUserInput.Text=thestr;
			} catch{}
		}
		#endregion

		#region debug Qserve button
		private void button1_Click(object sender, System.EventArgs e) {
			jobQ nowjob = new jobQ();
			
			nowjob=(jobQ)JOBQ[0];
			JobThread(nowjob);
			JOBQ.Remove(nowjob);
		}
		private void btnHACK_Click(object sender, System.EventArgs e) {
			jobstodo=0;
		}
		#endregion

		private void bntProxyChange_Click(object sender, System.EventArgs e) {
			prx.Stop();
			Thread.Sleep(1000);
			
			ProxyListenPort=updownListenPort.Value.ToString();
			ListenOnAll=chkListenEverywhere.Checked;

			BeginLongTaskThread();
		}

		#region --> Saving and loading data & config

		#region Saving
		private void btnSaveData_Click(object sender, System.EventArgs e) {
			string filename=string.Empty;
			try{
				DialogResult opened=openFileDialog1.ShowDialog();
				radioFile1.Checked=true;
				if (opened==DialogResult.OK){
					filename=openFileDialog1.FileName;
				} else {
					return;
				}
				#region main
				StreamWriter writer = new StreamWriter(filename);
				writer.WriteLine("SURU LOG:");
				foreach (HTTPRequest test in Requests){	
					writer.WriteLine("--------->>>>>>");
					writer.WriteLine(test.DT);
					writer.WriteLine(test.reqnum.ToString());
					writer.WriteLine(test.host);
					// We want our file to still be backwards compatible with older versions.
					// Hence, I decided to use the isSSL line to include an additional boolean variable
					// indicating whether this was a highlighted line or not...
					writer.WriteLine(test.isSSL.ToString() + "|" + test.isColour.ToString());
					writer.WriteLine(test.URL);
					writer.WriteLine(test.header);
					
					writer.WriteLine("=-=-=-=-=-=-=-");
					writer.WriteLine(test.response);
					writer.WriteLine("\n\n");
				}
				writer.Close();
				#endregion

				#region discovered
				writer = new StreamWriter(filename+".Suru-discovered");
				foreach (discovered item in discovered_goods){
					writer.WriteLine("--------->>>>>>");
					writer.WriteLine(item.host);
					writer.WriteLine(item.isSSL.ToString());
					writer.WriteLine(item.mode.ToString());
					writer.WriteLine(item.port);
					writer.WriteLine(item.protocol);
					writer.WriteLine(item.URL);
					writer.WriteLine(item.header);
				}
				writer.Close();
				#endregion

			} catch {
				MessageBox.Show("Problems writing file:"+filename+"\nCheck your permissions etc\n");
				return;
			}
		}
		#endregion

		#region Load data
		private void btnLoadData_Click(object sender, System.EventArgs e) {
			string filename=string.Empty;
			StreamReader reader;
			string dummy=string.Empty;
			try{
				DialogResult opened=openFileDialog1.ShowDialog();
				radioFile1.Checked=true;
			
				if (opened==DialogResult.OK){
					filename=openFileDialog1.FileName;
				} else {
					return;
				}
				#region log data
				reader = new StreamReader(filename);
				string line=string.Empty;
				bool startrecord=true;
				bool recordheader=false;
				bool recordresponse=false;
			
				dummy=reader.ReadLine();
				dummy=reader.ReadLine();
				HTTPRequest newrequest=new HTTPRequest();

				while (1==1)	{
				
					try{

						string responseline=string.Empty;
						while (recordresponse && responseline.Equals("--------->>>>>>")==false){
							responseline=reader.ReadLine();
							if (responseline.Equals("--------->>>>>>")){
								recordresponse=false;
								recordheader=false;
								startrecord=true;

								newrequest.header=newrequest.header.TrimEnd('\n');
								newrequest.header=newrequest.header.TrimEnd('\r');
								Requests.Add(newrequest);
								break;
							}
							newrequest.response+=responseline+"\n";
							
						}


						string headerline=string.Empty;
						while (recordheader && headerline.Equals("=-=-=-=-=-=-=-")==false){				
							headerline=reader.ReadLine();
							if (headerline.Equals("=-=-=-=-=-=-=-")){
								recordresponse=true;
								recordheader=false;
								break;
							}
							newrequest.header+=headerline+"\n";

					
						}
						if (startrecord){
							newrequest = new HTTPRequest();
							newrequest.DT=Convert.ToDateTime(reader.ReadLine());
							newrequest.reqnum=Convert.ToInt32(reader.ReadLine());
							newrequest.host=reader.ReadLine();
							string sz_line = reader.ReadLine();
							if (sz_line.IndexOf("|") == -1)
							{
								newrequest.isSSL = Convert.ToBoolean(sz_line);
								newrequest.isHighlighted = false;
								newrequest.isColour = 0;
							}
							else
							{
								string[] st_line = sz_line.Split('|');
								newrequest.isSSL = Convert.ToBoolean(st_line[0]);
								newrequest.isColour = Convert.ToInt32(st_line[1]);
							}
							newrequest.URL=reader.ReadLine();
							recordheader=true;
							startrecord=false;
						}
					
					
					} catch {
						reader.Close();
						//add the last one!!
						newrequest.header=newrequest.header.TrimEnd('\n');
						newrequest.header=newrequest.header.TrimEnd('\r');
						Requests.Add(newrequest);
						break;
					}
				}
			} catch {
				MessageBox.Show("Cannot find main file, or file is corrupt - sorry!");
				return;
			}
			#endregion

			#region discovered
			try{

				//read the discovered goods
			
				reader = new StreamReader(filename+".Suru-discovered");
				string deaderline;
				dummy=reader.ReadLine();
				while (1==1){
					discovered disc = new discovered();
					try{
						disc.host=reader.ReadLine();
						if (disc.host==null){
							throw new Exception("moo");
						}
						disc.isSSL=Convert.ToBoolean(reader.ReadLine());
						disc.mode=Convert.ToInt32(reader.ReadLine());
						disc.port=reader.ReadLine();
						disc.protocol=reader.ReadLine();
						disc.URL=reader.ReadLine();
						while ((deaderline=reader.ReadLine()) != null){
							if (deaderline.Equals("--------->>>>>>")){
								break;
							}
							disc.header+=deaderline;
						}
						discovered_goods.Add(disc);
					} catch {
						reader.Close();
						break;
					}
				}
				reader.Close();
			} catch{
				MessageBox.Show("Cant read "+filename+".Suru-discovered\nDiscovered data wont be shown\n");
				return;
			}
			#endregion
			
			Reconnodes.Clear();
			globalcount = Requests.Count;
			UpdateListViewControl();
		}

		#endregion
		
		private void btnClearALL_Click(object sender, System.EventArgs e) {
			DialogResult really = MessageBox.Show("Are you sure you want to clear EVERYTHING?","Confirm",MessageBoxButtons.YesNo);
			if (really==DialogResult.Yes){
				try{
					discovered_goods.Clear();
					WorkRequests.Clear();
					txtHTTPdetails.Clear();
				
				    listView1.Items.Clear();
					WorkRequests.Clear();
					EditRequests.Clear();
					displayed_items=0;
					lock(Requests){
						Requests.Clear();
						detailed_Requests.Clear();
					}
			
					lstCrowResponse.Items.Clear();
					txtCrowResponse.Clear();
					cmbReconTargetHost.Items.Clear();
					treeRecon.Nodes.Clear();
					kn_exts.Clear();
					kn_filenames.Clear(); 
					kn_hosts.Clear();
					kn_dirs.Clear();
					Reconnodes.Clear();
				} catch{}
			}
		}
		#endregion

		#region autosizing
		private void WebProxy_Resize(object sender, System.EventArgs e) {
			try{
				int middle=WebProxy.ActiveForm.Width/2;
				int vier=WebProxy.ActiveForm.Width/4;
				splitter2.SplitPosition=middle;
				grpInner.Left=middle-(grpInner.Size.Width);
				groupBox4.Left=middle;

				splitter6.SplitPosition=middle;
				splitter16.SplitPosition=middle;

				int hmiddle=WebProxy.ActiveForm.Height/2;
				int hdrie=WebProxy.ActiveForm.Height/3;
				int hvier=WebProxy.ActiveForm.Height/4;
				splitter1.SplitPosition=hdrie-50;
				splitter3.SplitPosition=hvier-50;
			}catch{}
		}

		
		#endregion

		#region BackEnd update

		private void btnBackEndUpdateDirs_Click(object sender, System.EventArgs e) {
			
			WebClient myWebClient;
			string download="";
			string update_site="http://www.sensepost.com/research/wikto/DB/BackEnd";

			if (cmbBackEndUpdate.Items.Count<2){
				
				myWebClient = new WebClient();
				statusBar1.Text="Getting update list from SensePost...";
				try{
					byte[] myDataBuffer = myWebClient.DownloadData (update_site+"/back-end-categories.txt");
					download = Encoding.ASCII.GetString(myDataBuffer);
				} catch (Exception ex){
					MessageBox.Show("Cannot get update categories:\n\n"+ex.ToString());
					return;
				}
				cmbBackEndUpdate.Items.Clear();
				cmbBackEndUpdate.Text="Choose one";
				statusBar1.Text="Please choose a category from the combo box and click on Update";

				string[] newcats = download.Split('\n');
				cmbBackEndUpdate.Items.Clear();
				foreach (string cat in newcats){
					cmbBackEndUpdate.Items.Add(cat);
				}
			} else {
				try{
					//if (cmbBackEndUpdate.SelectedValue.ToString().StartsWith("Choose")==false){
						getandupdate(txtWiktoTestDirs,update_site+"/"+cmbBackEndUpdate.SelectedItem.ToString()+"/backend-dirs.txt");
						txtWiktoTestDirs.Text=txtWiktoTestDirs.Text.TrimEnd('\r').TrimEnd('\n');
						getandupdate(txtWiktoTestFilenames,update_site+"/"+cmbBackEndUpdate.SelectedItem.ToString()+"/backend-filenames.txt");
						txtWiktoTestFilenames.Text=txtWiktoTestFilenames.Text.TrimEnd('\r').TrimEnd('\n');
						getandupdate(txtWiktoTestTypes,update_site+"/"+cmbBackEndUpdate.SelectedItem.ToString()+"/backend-extensions.txt");
						txtWiktoTestTypes.Text=txtWiktoTestTypes.Text.TrimEnd('\r').TrimEnd('\n');
					//}
					statusBar1.Text="DB updated..";
				} catch {}
			}
			
		}
		private void getandupdate (System.Windows.Forms.RichTextBox box, string URL){
			string download="";
			WebClient myWebClient = new WebClient();
			statusBar1.Text="Updating the DB...";
			try{
				byte[] myDataBuffer = myWebClient.DownloadData (URL);
				download = Encoding.ASCII.GetString(myDataBuffer);
			} catch (Exception ex){
				MessageBox.Show("Cannot update:\n\n"+ex.ToString());
				return;
			}
			box.Clear();
			string[] newdirs = download.Replace("\r\n","\n").Split('\n');
			foreach (string dir in newdirs){
				if (dir.Length>1){
					box.AppendText(dir.Replace("/","")+"\r\n");
				}
			}
		}
		#endregion

		private void chkReplayFireFox_CheckedChanged(object sender, System.EventArgs e) {
			if (chkReplayFireFox.Checked){
				MessageBox.Show("Remember that you need to install the Mozilla ActiveX Control for replaying via FireFox browser.\r\nAlso remember that the control does not support the population of cookies!");
			}
		}

		private void skinButton1_Click(object sender, System.EventArgs e) {
			toolTipSuru.RemoveAll();
		}

		#region disabling and enabling auto-update when text editing a request

		private void txtHTTPdetails_Enter(object sender, System.EventArgs e) {
			//stationaryinside=true;
			insidetxteditRQ=chkProxyAutoUpdate.Checked;
			chkProxyAutoUpdate.Checked=false;
		}

		private void txtHTTPdetails_Leave(object sender, System.EventArgs e) {
			chkProxyAutoUpdate.Checked=insidetxteditRQ;
			//stationaryinside=false;
		}
		private void txtHTTPdetails_EnterM(object sender, System.EventArgs e) {
			//insidetxtedit=chkProxyAutoUpdate.Checked;
			//chkProxyAutoUpdate.Checked=false;
		}

		private void txtHTTPdetails_LeaveM(object sender, System.EventArgs e) {
		//	if (!stationaryinside){
		//		chkProxyAutoUpdate.Checked=insidetxtedit;
		//	}
		}
		#endregion


		private void txtToolsUuserInput_KeyPress(object sender, System.EventArgs e) {
		btnToolsGo.PerformClick();
		}

		private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string sz_reqnum = "";
			int n_reqnum = 0;
			int n_index = 0;
			int n_count = 0;
			int zz = listView1.Items.Count-1;
			foreach ( ListViewItem z in listView1.SelectedItems)
			{
				sz_reqnum = z.Text.Substring(0, z.Text.IndexOf(" "));
				n_reqnum = Convert.ToInt32(sz_reqnum);
				n_count++;
			}
			if (n_count > 0)
			{
				foreach (HTTPRequest hr in EditRequests)
				{
					if (hr.reqnum == n_reqnum)
					{
						break;
					}
					n_index++;
				}
				txtHTTPdetails.Enabled=true;
				btnSendRawRequest.Enabled=true;
				btnReplay.Enabled=true;
				txtCrowResponse.Enabled=true;
				HTTPRequest test = (HTTPRequest)EditRequests[n_index];
				detailedRequest temp = getHTTPdetails(test.header,test.host,test.isSSL);
				txtCrowResponse.Text=test.response;
				txtTargetHost.Text=temp.host;
				txtTargetPort.Text=temp.port;
				lblDateTime.Text=test.DT.Hour+"h "+test.DT.Minute+"m "+test.DT.Second+"s "+test.DT.Millisecond+"ms";
				if (test.isSSL)
				{	
					chkTargetIsSSL.Checked=true;
				} 
				else 
				{
					chkTargetIsSSL.Checked=false;
				}
				txtHTTPdetails.Text=test.header.Replace("http://"+test.host,"");
				int where=txtHTTPdetails.Text.IndexOf("HTTP");
				int where2=txtHTTPdetails.Text.IndexOf(" ");
				txtHTTPdetails.SelectionFont=new Font(Font,FontStyle.Bold);
				txtHTTPdetails.SelectionColor=Color.Red;
				txtHTTPdetails.Select(where2,where-where2);
				try
				{
					if (is_reqeditor_open)
					{
						if (this.AmIUpdating == false)
							showeditor(txtHTTPdetails.Text);
					}
					this.Focus();
				}
				catch{}
			}
		}

		private void listView1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			char pressed=e.KeyChar;
			//enter
			System.Diagnostics.Debug.WriteLine(System.Convert.ToInt32(pressed).ToString());
			if (pressed==13)
			{
				int zz = 0;
				if (listView1.SelectedItems.Count != 0)
				{
					foreach ( ListViewItem z in listView1.SelectedItems)
					{
						zz = z.Index;
						break;
					}
					HTTPRequest test = (HTTPRequest)EditRequests[zz];
					txtHTTPdetails.Text=test.header;
					showeditor(txtHTTPdetails.Text);
				}
			}
			if (pressed==102)
			{
				FilterForm newfilt = new FilterForm(this);
				newfilt.Show();
			}
			if (pressed == 48)
			{
				mnu_HLBlack.PerformClick();
			}
			if (pressed == 49)
			{
				mnu_HLBrown.PerformClick();
			}
			if (pressed == 50)
			{
				mnu_HLRed.PerformClick();
			}
			if (pressed == 51)
			{
				mnu_HLOrange.PerformClick();
			}
			if (pressed == 52)
			{
				mnu_HLYellow.PerformClick();
			}
			if (pressed == 53)
			{
				mnu_HLGreen.PerformClick();
			}
			if (pressed == 54)
			{
				mnu_HLAqua.PerformClick();
			}
			if (pressed == 55)
			{
				mnu_HLBlue.PerformClick();
			}
			if (pressed == 56)
			{
				mnu_HLPurple.PerformClick();
			}
			if (pressed == 57)
			{
				mnu_HLGrey.PerformClick();
			}
		}

		private void listView1_DoubleClick(object sender, System.EventArgs e)
		{
			if (listView1.SelectedItems.Count > 0)
			{
				int zz = 0;
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					zz = z.Index;
					break;
				}
				HTTPRequest test = (HTTPRequest)EditRequests[zz];
				txtHTTPdetails.Text=test.header;
				showeditor(txtHTTPdetails.Text);
			}
		}

		private void tabControl1_Click(object sender, System.EventArgs e)
		{
			this.AmIUpdating = true;
			if (tabControl1.SelectedIndex == 0)
			{
				UpdateListViewControl();
			}
			this.AmIUpdating = false;
		}

		private void TestItem(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button.ToString() == "Right")
			{
				//contextMenu1.MenuItems.Clear();
				contextMenu1.Show(listView1, new System.Drawing.Point(MousePosition.X - listView1.Left - this.Left - 20, MousePosition.Y - listView1.Top - this.Top - tabControl1.GetTabRect(0).Height - 20));
			}
		}

		private void mnu_FILTER_Click(object sender, System.EventArgs e)
		{
			FilterForm newfilt = new FilterForm(this);
			newfilt.Show();
		}

		private void mnu_RE_Click(object sender, System.EventArgs e)
		{
			int zz = 0;
			if (listView1.SelectedItems.Count != 0)
			{
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					zz = z.Index;
					break;
				}
				HTTPRequest test = (HTTPRequest)EditRequests[zz];
				txtHTTPdetails.Text=test.header;
				showeditor(txtHTTPdetails.Text);
			}
		}

		private void mnu_HLBrown_Click(object sender, System.EventArgs e)
		{
			int n_index = 0;
			int n_myindex = 0;
			if (listView1.SelectedItems.Count != 0)
			{
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					n_myindex = z.Index;
					string sz_reqnum = z.Text.Substring(0, z.Text.IndexOf(" "));
					int n_reqnum = Convert.ToInt32(sz_reqnum);
					HTTPRequest myhr = new HTTPRequest();

					System.Drawing.Font myfont = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
					foreach (HTTPRequest hr in Requests)
					{
						if (hr.reqnum == n_reqnum)
						{
							myhr = hr;
							break;
						}
						n_index++;
					}
					z.Font = myfont;
					z.ForeColor = System.Drawing.Color.Brown;
					myhr.isHighlighted = true;
					myhr.isColour = 1;
					Requests[n_index] = myhr;
				}
				listView1.Refresh();
				listView1.Items[n_myindex].Selected = true;
			}
		}

		private void mnu_HLRed_Click(object sender, System.EventArgs e)
		{
			int n_index = 0;
			int n_myindex = 0;
			if (listView1.SelectedItems.Count != 0)
			{
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					n_myindex = z.Index;
					string sz_reqnum = z.Text.Substring(0, z.Text.IndexOf(" "));
					int n_reqnum = Convert.ToInt32(sz_reqnum);
					HTTPRequest myhr = new HTTPRequest();

					System.Drawing.Font myfont = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
					foreach (HTTPRequest hr in Requests)
					{
						if (hr.reqnum == n_reqnum)
						{
							myhr = hr;
							break;
						}
						n_index++;
					}
					z.Font = myfont;
					z.ForeColor = System.Drawing.Color.Red;
					myhr.isHighlighted = true;
					myhr.isColour = 2;
					Requests[n_index] = myhr;
				}
				listView1.Refresh();
				listView1.Items[n_myindex].Selected = true;
			}
		}

		private void mnu_HLOrange_Click(object sender, System.EventArgs e)
		{
			int n_index = 0;
			int n_myindex = 0;
			if (listView1.SelectedItems.Count != 0)
			{
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					n_myindex = z.Index;
					string sz_reqnum = z.Text.Substring(0, z.Text.IndexOf(" "));
					int n_reqnum = Convert.ToInt32(sz_reqnum);
					HTTPRequest myhr = new HTTPRequest();

					System.Drawing.Font myfont = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
					foreach (HTTPRequest hr in Requests)
					{
						if (hr.reqnum == n_reqnum)
						{
							myhr = hr;
							break;
						}
						n_index++;
					}
					
						z.Font = myfont;
						z.ForeColor = System.Drawing.Color.Orange;
						//z.ForeColor = System.Drawing.Color.Red;
						myhr.isHighlighted = true;
						myhr.isColour = 3;
					Requests[n_index] = myhr;
				}
				listView1.Refresh();
				listView1.Items[n_myindex].Selected = true;
			}
		}

		private void mnu_HLYellow_Click(object sender, System.EventArgs e)
		{
			int n_index = 0;
			int n_myindex = 0;
			if (listView1.SelectedItems.Count != 0)
			{
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					n_myindex = z.Index;
					string sz_reqnum = z.Text.Substring(0, z.Text.IndexOf(" "));
					int n_reqnum = Convert.ToInt32(sz_reqnum);
					HTTPRequest myhr = new HTTPRequest();

					System.Drawing.Font myfont = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
					foreach (HTTPRequest hr in Requests)
					{
						if (hr.reqnum == n_reqnum)
						{
							myhr = hr;
							break;
						}
						n_index++;
					}
						z.Font = myfont;
						z.ForeColor = System.Drawing.Color.DarkKhaki;
						myhr.isHighlighted = true;
						myhr.isColour = 4;
					Requests[n_index] = myhr;
				}
				listView1.Refresh();
				listView1.Items[n_myindex].Selected = true;
			}
		}

		private void mnu_HLGreen_Click(object sender, System.EventArgs e)
		{
			int n_index = 0;
			int n_myindex = 0;
			if (listView1.SelectedItems.Count != 0)
			{
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					n_myindex = z.Index;
					string sz_reqnum = z.Text.Substring(0, z.Text.IndexOf(" "));
					int n_reqnum = Convert.ToInt32(sz_reqnum);
					HTTPRequest myhr = new HTTPRequest();

					System.Drawing.Font myfont = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
					foreach (HTTPRequest hr in Requests)
					{
						if (hr.reqnum == n_reqnum)
						{
							myhr = hr;
							break;
						}
						n_index++;
					}
						z.Font = myfont;
						z.ForeColor = System.Drawing.Color.Green;
						//z.ForeColor = System.Drawing.Color.Red;
						myhr.isHighlighted = true;
						myhr.isColour = 5;
					Requests[n_index] = myhr;
				}
				listView1.Refresh();
				listView1.Items[n_myindex].Selected = true;
			}
		}

		private void mnu_HLAqua_Click(object sender, System.EventArgs e)
		{
			int n_index = 0;
			int n_myindex = 0;
			if (listView1.SelectedItems.Count != 0)
			{
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					n_myindex = z.Index;
					string sz_reqnum = z.Text.Substring(0, z.Text.IndexOf(" "));
					int n_reqnum = Convert.ToInt32(sz_reqnum);
					HTTPRequest myhr = new HTTPRequest();

					System.Drawing.Font myfont = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
					foreach (HTTPRequest hr in Requests)
					{
						if (hr.reqnum == n_reqnum)
						{
							myhr = hr;
							break;
						}
						n_index++;
					}
						z.Font = myfont;
						z.ForeColor = System.Drawing.Color.Cyan;
						myhr.isHighlighted = true;
						myhr.isColour = 6;
					Requests[n_index] = myhr;
				}
				listView1.Refresh();
				listView1.Items[n_myindex].Selected = true;
			}
		}

		private void mnu_HLBlue_Click(object sender, System.EventArgs e)
		{
			int n_index = 0;
			int n_myindex = 0;
			if (listView1.SelectedItems.Count != 0)
			{
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					n_myindex = z.Index;
					string sz_reqnum = z.Text.Substring(0, z.Text.IndexOf(" "));
					int n_reqnum = Convert.ToInt32(sz_reqnum);
					HTTPRequest myhr = new HTTPRequest();

					System.Drawing.Font myfont = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
					foreach (HTTPRequest hr in Requests)
					{
						if (hr.reqnum == n_reqnum)
						{
							myhr = hr;
							break;
						}
						n_index++;
					}
						z.Font = myfont;
						z.ForeColor = System.Drawing.Color.Blue;
						myhr.isHighlighted = true;
						myhr.isColour = 7;
					Requests[n_index] = myhr;
				}
				listView1.Refresh();
				listView1.Items[n_myindex].Selected = true;
			}
		}

		private void mnu_HLPurple_Click(object sender, System.EventArgs e)
		{
			int n_index = 0;
			int n_myindex = 0;
			if (listView1.SelectedItems.Count != 0)
			{
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					n_myindex = z.Index;
					string sz_reqnum = z.Text.Substring(0, z.Text.IndexOf(" "));
					int n_reqnum = Convert.ToInt32(sz_reqnum);
					HTTPRequest myhr = new HTTPRequest();

					System.Drawing.Font myfont = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
					foreach (HTTPRequest hr in Requests)
					{
						if (hr.reqnum == n_reqnum)
						{
							myhr = hr;
							break;
						}
						n_index++;
					}
						z.Font = myfont;
						z.ForeColor = System.Drawing.Color.Purple;
						myhr.isHighlighted = true;
						myhr.isColour = 8;
					Requests[n_index] = myhr;
				}
				listView1.Refresh();
				listView1.Items[n_myindex].Selected = true;
			}
		}

		private void mnu_HLGrey_Click(object sender, System.EventArgs e)
		{
			int n_index = 0;
			int n_myindex = 0;
			if (listView1.SelectedItems.Count != 0)
			{
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					n_myindex = z.Index;
					string sz_reqnum = z.Text.Substring(0, z.Text.IndexOf(" "));
					int n_reqnum = Convert.ToInt32(sz_reqnum);
					HTTPRequest myhr = new HTTPRequest();

					System.Drawing.Font myfont = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
					foreach (HTTPRequest hr in Requests)
					{
						if (hr.reqnum == n_reqnum)
						{
							myhr = hr;
							break;
						}
						n_index++;
					}
						z.Font = myfont;
						z.ForeColor = System.Drawing.Color.DarkGray;
						myhr.isHighlighted = true;
						myhr.isColour = 9;
					Requests[n_index] = myhr;
				}
				listView1.Refresh();
				listView1.Items[n_myindex].Selected = true;
			}
		}

		private void mnu_HLBlack_Click(object sender, System.EventArgs e)
		{
			int n_index = 0;
			int n_myindex = 0;
			if (listView1.SelectedItems.Count != 0)
			{
				foreach ( ListViewItem z in listView1.SelectedItems)
				{
					n_myindex = z.Index;
					string sz_reqnum = z.Text.Substring(0, z.Text.IndexOf(" "));
					int n_reqnum = Convert.ToInt32(sz_reqnum);
					HTTPRequest myhr = new HTTPRequest();

					System.Drawing.Font myfont = new System.Drawing.Font("MS Referense Sans Serif", 7.75F);
					foreach (HTTPRequest hr in Requests)
					{
						if (hr.reqnum == n_reqnum)
						{
							myhr = hr;
							break;
						}
						n_index++;
					}
						z.Font = myfont;
						z.ForeColor = System.Drawing.Color.Black;
						myhr.isHighlighted = false;
						myhr.isColour = 0;
					Requests[n_index] = myhr;
				}
				listView1.Refresh();
				listView1.Items[n_myindex].Selected = true;
			}
		}

		private void SetMousePosition(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				MouseX = e.X;
				MouseY = e.Y;
			}
		}
		  
	}
}


/*
			this.tabControl1.Controls.Add(this.Proxy);
			this.tabControl1.Controls.Add(this.Recon);
			this.tabControl1.Controls.Add(this.Misc);
			this.tabControl1.Controls.Add(this.Config)			*/






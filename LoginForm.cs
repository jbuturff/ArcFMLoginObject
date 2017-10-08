using System;
using System.Windows.Forms;
using System.Diagnostics;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Framework;
using Miner.Interop;
using System.Runtime.InteropServices;
using System.IO;
using Miner.Framework;
using System.Collections.Generic;

namespace ArcFMLoginObject
{
    public enum ConnectionType { NotSet = 0, SDE, PersonalGDB, SQlServer }
	
	[ComVisible(true)]
    [Guid("2E33B960-E9F2-46f2-A50F-D830289DA0C6")]
    [ProgId("ArcFMLoginObject.CustomLogin")]
    public class LoginForm : System.Windows.Forms.Form, IMMLoginObject, IMMChangeDefaultVersion
	{
		#region Designer Generated Private Fields
		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.Label lblDB;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
        private PictureBox pictureBox;
		private System.Windows.Forms.Label lblPassword;
		private System.ComponentModel.Container components = null;
		#endregion

        #region Private Fields
        private MMDefaultLoginObjectClass _defaultLogin;
        private IPropertySet _propSet;
        private bool _initialLogin;
        private bool _loginChanged;
        private ConnectionType connectionType;
        private string server = null;
        private string instance = null;
        private string personalGeoDbPath = null;
        private IWorkspace myworkspace;

	    private Miner.IMMRegistry reg = new Miner.MMRegistry();
        #endregion

		public LoginForm()
		{
            // windows forms generated code
			InitializeComponent();

            _defaultLogin = new MMDefaultLoginObjectClass();
            _propSet = new PropertySetClass();

            ReadCurrentConnectionInformation();

			btnOK.Focus();
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

		
		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblDB = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.btnBrowse);
            this.groupBox.Controls.Add(this.lblDB);
            this.groupBox.Location = new System.Drawing.Point(13, 243);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(287, 131);
            this.groupBox.TabIndex = 10;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Database";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(13, 97);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 24);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblDB
            // 
            this.lblDB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDB.Location = new System.Drawing.Point(13, 21);
            this.lblDB.Name = "lblDB";
            this.lblDB.Size = new System.Drawing.Size(260, 62);
            this.lblDB.TabIndex = 11;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(153, 388);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(147, 20);
            this.txtUser.TabIndex = 3;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(153, 423);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(147, 20);
            this.txtPassword.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(53, 388);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "User name: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(53, 423);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(87, 21);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password: ";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(80, 451);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(200, 451);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            // 
            // pictureBox1
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(53, 14);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(207, 208);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(323, 576);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login to ArcFM";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public string PersonalGeoDbPath
		{
			get { return personalGeoDbPath; }
		}
		public string UserName
		{
			get { return txtUser.Text; }
		}
		public string Password
		{
			get { return txtPassword.Text; }
		}
		public ConnectionType ConnectionType
		{ 
			get { return connectionType; }
		}
		public string Server
		{
			get { return server; }
		}
		public string Instance
		{
			get { return instance; }
		}

		
		#region Button Events
		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			try
			{
				IGxDialog dialog = GetGxDialog();
				IEnumGxObject gxObjects;
				dialog.DoModalOpen(this.Handle.ToInt32(), out gxObjects);
				
				IGxDatabase gxDatabase = gxObjects.Next() as IGxDatabase;
				if ( gxDatabase == null )
					return;

				connectionType = gxDatabase.IsRemoteDatabase ? ConnectionType.SDE : ConnectionType.PersonalGDB;
				_propSet = gxDatabase.WorkspaceName.ConnectionProperties;
				ReadNewConnectionProperties(_propSet);
				PopulateConnectionInfoLabel();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
			}
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
                OpenKey();
                WriteConnectionType();
                WriteConnectionParameters();
                WriteUserName();

                _loginChanged = _defaultLogin.Login(_initialLogin);

                this.Close();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
			}
			
		}
		#endregion

		private void ReadNewConnectionProperties(IPropertySet connectionProperties)
		{
			switch ( ConnectionType )
			{
				case ConnectionType.SDE:
					server = connectionProperties.GetProperty("SERVER").ToString();
					instance = connectionProperties.GetProperty("INSTANCE").ToString();
					personalGeoDbPath = "";
					TogglePassword(true);
					break;
				case ConnectionType.PersonalGDB:
					personalGeoDbPath = connectionProperties.GetProperty("DATABASE").ToString();
					server = "";
					instance = "";
					TogglePassword(false);
					break;
				case ConnectionType.SQlServer:
					break;
				default:
					return;
			}
		}

		private void TogglePassword(bool enabled)
		{
			txtPassword.Enabled = enabled;
			lblPassword.Enabled = enabled;
		}

		#region Registry Reading/Writing Methods
		private void ReadCurrentConnectionInformation()
		{
			try
			{
				OpenKey();
				ReadConnectionType();
				
				if ( ConnectionType == ConnectionType.NotSet )
					return;
			
				SetConnectionParameterFields();
				PopulateConnectionInfoLabel();
				PopulateUserNameTextBox();
			}
			catch (Exception e)
			{
				Trace.WriteLine(e.ToString());
			}
		}

		private void OpenKey()
		{
			reg.OpenKey(mmHKEY.mmHKEY_CURRENT_USER, mmBaseKey.mmArcFM, "DefaultConnection");
		}
		private void ReadConnectionType()
		{
			connectionType = (ConnectionType)Enum.ToObject(typeof(ConnectionType), (int)reg.Read("ConnectionType", 0));
		}
		private void SetConnectionParameterFields()
		{
			switch ( ConnectionType )
			{
				case ConnectionType.SDE:
					ReadConnectionParameters_SDE();
					TogglePassword(true);
					break;
				case ConnectionType.PersonalGDB:
					ReadConnectionParameters_PGDB();
					TogglePassword(false);
					break;
				case ConnectionType.SQlServer:
					ReadConnectionParameters_SQLServer();
					break;
				default:
					return;
			}
		}

		private void ReadConnectionParameters_SDE()
		{
			server = reg.Read("Server", "").ToString();
			instance = reg.Read("Instance", "").ToString();
			personalGeoDbPath = "";
		}
		private void ReadConnectionParameters_PGDB()
		{
			personalGeoDbPath = reg.Read("DatabasePath", "").ToString();
			server = "";
			instance = "";
		}
		private void ReadConnectionParameters_SQLServer()
		{
			//not implemented at this time
		}
		
		private void WriteConnectionType()
		{
			reg.Write("ConnectionType", (int)ConnectionType);
		}

		private void WriteConnectionParameters()
		{
			reg.Write("Server", server);
			reg.Write("Instance", instance);
			reg.Write("DatabasePath", personalGeoDbPath);
		}

		private void WriteUserName()
		{
			reg.Write("User", txtUser.Text);
		}
		#endregion

		#region Update User Interface Methods
		private void PopulateUserNameTextBox()
		{
			txtUser.Text = reg.Read("User", "").ToString();
		}
		private void PopulateConnectionInfoLabel()
		{
			switch ( ConnectionType )
			{
				case ConnectionType.SDE:
					lblDB.Text = "Server: " + server;
					lblDB.Text += "\nInstance: " + instance;
					break;
				case ConnectionType.PersonalGDB:
					lblDB.Text = "Path:\n";
					lblDB.Text += personalGeoDbPath;
					break;
				case ConnectionType.SQlServer:
					break;
				default:
					return;
			}
		}
		#endregion

		private IGxDialog GetGxDialog()
		{
			IGxDialog dialog = new GxDialogClass();
			dialog.ButtonCaption = "OK";
			dialog.AllowMultiSelect = false;
			dialog.ObjectFilter = new MMGxFilterDatabasesClass();
			
			return dialog;
		}

		
		

		private void LoginForm_Load(object sender, System.EventArgs e)
		{
			this.Focus();
		}



        #region IMMChangeDefaultVersion Members

        void IMMChangeDefaultVersion.ChangeDefaultVersion(IVersion pVersion)
        {
            _defaultLogin.ChangeDefaultVersion(pVersion);
        }

        #endregion

        #region IMMLoginObject Members

        string IMMLoginObject.GetFullTableName(string bstrBaseTableName)
        {
            return _defaultLogin.GetFullTableName(bstrBaseTableName);
        }

        bool IMMLoginObject.IsValidLogin
        {
            get
            { return _defaultLogin.IsValidLogin; }
        }

        bool IMMLoginObject.Login(bool vbInitialLogin)
        {
            IExtensionManager pExtMgr;
            IExtension pExt;
            UID pUID;   
            IApplication pApp;
            CacheExt pCacheExt;

            _initialLogin = vbInitialLogin;

 
            _defaultLogin.ShowDialog = false;
            
            //this.ShowDialog(new WindowWrapper(hwnd));
            _propSet = new PropertySetClass();
            //var dic = new Dictionary<string, string>()
            //{
            //    {"SERVER"," "},
            //    {"DATABASE"," "},
            //    {"INSTANCE","SDE:oracle11g:pnm2"},
            //    {"USER","SDE"},
            //    {"PASSWORD","sde"},
            //    {"VERSION","SDE.DEFAULT"}
            //};

            //get the full location of the assembly with DaoTests in it
            string fullPath = System.Reflection.Assembly.GetAssembly(typeof(LoginForm)).Location;

            //get the folder that's in
            string theDirectory = Path.GetDirectoryName(fullPath);
            bool bCacheEnabled;

            bCacheEnabled = false;
            Debug.WriteLine(theDirectory.ToString());

            string[] lines = System.IO.File.ReadAllLines(theDirectory.ToString() + "\\ARCFMLOGINOBJECT.CFG");
            foreach (string line in lines)
            {
                if (line.Substring(1, 1) != "#")
                {
                    // Use a tab to indent each line of the file.
                    string[] sValues = line.Split(',');
                    switch (sValues[0].ToUpper())
                    {
                        case "SERVER":
                            if (sValues.Length > 1)
                            {
                                _propSet.SetProperty("SERVER", sValues[1]);
                            }
                            else
                            {
                                _propSet.SetProperty("SERVER", "SDE");
                            }
                            break;
                        case "INSTANCE":
                            _propSet.SetProperty("INSTANCE", sValues[1]);
                            break;
                        case "DATABASE":
                            _propSet.SetProperty("DATABASE", sValues[1]);
                            break;
                        case "IS_GEODATABASE":
                            _propSet.SetProperty("IS_GEODATABASE", sValues[1]);
                            break;
                        case "AUTHENTICATION_MODE":
                            _propSet.SetProperty("AUTHENTICATION_MODE", sValues[1]);
                            break;
                        case "USER":
                            _propSet.SetProperty("USER", sValues[1]);
                            break;
                        case "PASSWORD":
                            _propSet.SetProperty("PASSWORD", sValues[1]);
                            break;
                        case "CONNPROP_REV":
                            _propSet.SetProperty("CONNPROP-REV", sValues[1]);
                            break;
                        case "VERSION":
                            _propSet.SetProperty("VERSION", sValues[1]);
                            break;
                        case "CACHE":
                            if (sValues[1].ToUpper() == "TRUE")
                            {
                                bCacheEnabled = true;
                            }
                            break;
                    }
                    Console.WriteLine("\t" + line);
                }
            }

            _defaultLogin.SetConnectionProperties(_propSet);
            _loginChanged= _defaultLogin.Login(_initialLogin);
            var isAlright = _defaultLogin.IsValidLogin;
            myworkspace = _defaultLogin.LoginWorkspace;
            pCacheExt = CacheExt.GetExtension();
            if (bCacheEnabled == true)
            {
                pCacheExt.OpenClasses(myworkspace);
            }
            //ESRI.ArcGIS.Geodatabase.IEnumDataset pEDS = myworkspace.get_Datasets(esriDatasetType.esriDTAny);
            return _loginChanged;
        }

        IWorkspace IMMLoginObject.LoginWorkspace
        {
            get { return _defaultLogin.LoginWorkspace; }
        }

        string IMMLoginObject.UserName
        {
            get { return _defaultLogin.UserName; }
        }

        #endregion

        //private bool AttemptLogin(LoginForm login, bool vbInitialLogin)
        //{
        //    if (!Login(login, vbInitialLogin))
        //    {
        //        MessageBox.Show(login, "Unable to login.  The server may be unavaliable, or your user name or password may be invalid", "Login to ArcFM");
        //        Login(vbInitialLogin);
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
        private void InformUserNoArcFM(bool vbInitialLogin)
        {
            if (!vbInitialLogin)
                return;

            if (InformUserNoArcFM() == DialogResult.Cancel)
            {
                //Login(vbInitialLogin);
            }
        }
        private DialogResult InformUserNoArcFM()
        {
            string cancelMessage = "Cancelling login will disable the following:\n";
            cancelMessage += "Page Templates\nStored Displays\nBatch Plotting\nArcFM Favorites";

            return MessageBox.Show(cancelMessage, "ArcFM Login", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private PropertySetClass GetPropertySet(LoginForm login)
        {
            PropertySetClass props = new PropertySetClass();

            switch (login.ConnectionType)
            {
                case ConnectionType.PersonalGDB:
                    props.SetProperty("DATABASE", login.PersonalGeoDbPath);
                    props.SetProperty("USERNAME", login.UserName);
                    break;
                case ConnectionType.SDE:
                    props.SetProperty("USER", login.UserName);
                    props.SetProperty("PASSWORD", login.Password);
                    props.SetProperty("VERSION", "SDE.DEFAULT");
                    props.SetProperty("SERVER", login.Server);
                    props.SetProperty("INSTANCE", login.Instance);
                    break;
                default:
                    throw (new Exception("Invalid DB Type"));
            }

            return props;
        }


        private class WindowWrapper : System.Windows.Forms.IWin32Window
        {
            private IntPtr _hwnd;

            public WindowWrapper(IntPtr handle)
            {
                _hwnd = handle;
            }

            public IntPtr Handle
            {
                get { return _hwnd; }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using Common;
namespace INTUSOFT.Configuration
{
    public delegate void SaveBtnClicked(object sender, EventArgs e);
    public partial class Config_UC : UserControl
    {
        public event SaveBtnClicked saveClicked;
        string configPath = null;
        private string password = null;

        //Multilang:This function initializes the string ID's in this file
        private void initializeLang()
        {
            //label2.Text = Globals.languageResource.GetString("STRING_Config_UC_label2");
            //save_btn.Text = Globals.languageResource.GetString("STRING_Config_UC_save_btn");
        }

        public static FieldInfo[] fieldInfos;
        public Config_UC()
        {
            InitializeComponent();
            initializeLang();
            Setup(" ");
            fieldInfos = ConfigVariables.CurrentSettings.GetType().GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance );
            //if (!IsControlProperty(XmlReadWrite.dic))
            traverseControlProperties(fieldInfos, ConfigVariables.CurrentSettings);
        }

        KeyValuePair<string, object> currentItem = new KeyValuePair<string, object>();
        static FieldInfo[] temp;
        public void TraveseDicNodes(string controlPropertyName, Dictionary<string, object> dicName)
        {
            foreach (KeyValuePair<string, object> item in dicName)
            {
                //if (dicName.Count > 1)
                {
                    if (!item.Key.Replace("_", "").ToLower().Equals(controlPropertyName))
                    {
                        if (item.Value.GetType() == typeof(Dictionary<string, object>))
                            TraveseDicNodes(controlPropertyName, item.Value as Dictionary<string, object>);
                        else
                        {
                        }
                        //Type str = item.Value.GetType();
                    }
                    else
                    {
                        currentItem = item;
                        break;
                    }
                }
            }
        }

        public void traverseControlProperties(PropertyInfo[] pinf ,  object obj)
        {
            for (int i = 0; i < pinf.Length; i++)
            {
                try
                {
                        obj = pinf[i].GetValue(obj);
                        IVLControlProperties ctrlProp = obj as IVLControlProperties;
                        string controlPropertyLabel = ctrlProp.name.Replace(" ", "");
                        controlPropertyLabel = controlPropertyLabel.ToLower();
                        TraveseDicNodes(controlPropertyLabel, XmlReadWrite.dic);
                        //if (controlPropertyLabel == currentItem.Key.ToLower())
                        {
                            ctrlProp.val = currentItem.Value.ToString();
                            pinf[i].SetValue(obj, ctrlProp);
                        }
                }
                catch (Exception ex)
                {
                    
                    throw;
                }
            }
        }

        public void traverseControlProperties(FieldInfo [] f, object obj)
        {
            object obj1 = obj;
                for (int i = 0; i < f.Length; i++)
                {
                    FieldInfo[] temp =   f[i].FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public);
                    obj = f[i].GetValue(obj1);
                        
                    if(temp.Length == 0)
                        {
                            PropertyInfo[] pinf =   f[i].FieldType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        }
                        else   if (temp.Length > 1)
                        {
                            traverseControlProperties(temp ,obj);
                        }
                    }
        }

        public void Setup(String fileName)
        {
            {
                configPath = IVLConfig.fileName;
                File.SetAttributes(configPath, FileAttributes.Normal);
                xmlRW.fileName = configPath;
                // This line has been commented in order to remove read only setting for config file which will disable the writing programmatically which resulted in the defect 0000595 by sriram August 20th 2015
               // File.SetAttributes(configPath, FileAttributes.ReadOnly); 
            }
            xmlRW.ReadXml();
            rootDict = xmlRW.GetRoot();
             //addBranchL(rootDict, null, 1);
            string date = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            if (date.Length == 1)
            {
                date = "0" + DateTime.Now.Day.ToString();
            }
            if (month.Length == 1)
            {
                month = "0" + DateTime.Now.Month.ToString();
            }
            password = "123";
        }

        XmlReadWrite xmlRW = new XmlReadWrite(IVLConfig.fileName);
        public KeyValuePair<String, Object> rootDict;
        public int vIndent = 10;
        int hIndent = 10;
        //private string password = "123";
        //private string password = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString();

        public void addBranchL(KeyValuePair<String, Object> branch, String parentName, int iLevel, Control c)
        {
            XmlLeaf_UC tmp = new XmlLeaf_UC(branch.Key, null, parentName, iLevel, 0, 100000);
            tmp.Left = hIndent + (iLevel - 1) * 30;
            tmp.Top = vIndent;
            c.Controls.Add(tmp);
            vIndent += 30;
            foreach (KeyValuePair<String, Object> pair in branch.Value as Dictionary<string, object>)
            {
                if (!xmlRW.is_branch(pair)) addLeaf(pair, iLevel, parentName == null ? branch.Key : parentName + "." + branch.Key, c);
                else
                    addBranchL(pair, parentName == null ? branch.Key : parentName + "." + branch.Key, iLevel + 1, c);
            }
        }

        public void addBranchL(FieldInfo[] branch, string parentName, int iLevel, Control c ,object obj)
        {
            XmlLeaf_UC tmp = new XmlLeaf_UC(parentName, null, parentName, iLevel, 0, 100000);
            tmp.Left = hIndent + (iLevel - 1) * 30;
            tmp.Top = vIndent;
            c.Controls.Add(tmp);
            vIndent += 30;
            object obj1 = obj;
            for (int i = 0; i < branch.Length; i++)
            {
                if (!branch[i].FieldType.Name.Equals("IVLControlProperties"))
                {
                    temp = branch[i].FieldType.GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance );
                    if (temp.Length == 0)
                    {
                        PropertyInfo[] pinf = branch[i].FieldType.GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance );
                        try
                        {
                            obj = branch[i].GetValue(obj1);
                        }
                        catch (Exception ex)
                        {
                            
                            throw;
                        }
                       
                        string[] strArr = branch[i].Name.Split('.');
                        addBranchL(pinf, strArr[0], iLevel, c, obj);
                    }
                    else if (temp.Length > 0)
                    {
                        addBranchL(temp, branch[i].Name, iLevel + 1, c,obj);
                    }
                }
                else
                {
                     obj = branch[i].GetValue(obj1);
                    IVLControlProperties controlProperties = obj as IVLControlProperties;
                    addLeaf(controlProperties, iLevel, parentName, c);
                }
            }
        }

        public void addBranchL(PropertyInfo[] branch, string parentName, int iLevel, Control c,object obj)
        {
            object obj1 = obj;
            XmlLeaf_UC node = new XmlLeaf_UC(parentName, null, parentName, iLevel+1, 0, 100000);
            node.Left = hIndent + 5 * (iLevel+1);
            node.Top = vIndent;
            c.Controls.Add(node);
            vIndent += 30;
            for (int i = 0; i < branch.Length; i++)
            {
                     obj = branch[i].GetValue(obj1);
                    IVLControlProperties controlProperties = obj as IVLControlProperties;
                    addLeaf(controlProperties, iLevel, parentName, c);
            }
        }

        private void addLeaf(KeyValuePair<String, Object> leaf, int lvl, String parentName,Control c)
        {
            XmlLeaf_UC node = new XmlLeaf_UC(leaf.Key, leaf.Value.ToString(), parentName, lvl, 0, 200000);
            node.Left = hIndent + 5 * lvl;
            node.Top = vIndent;
            c.Controls.Add(node);
            vIndent += 30;
        }

        private void addLeaf(IVLControlProperties ctrl, int lvl, string parentName, Control c)
        {
            XmlLeaf_UC node = new XmlLeaf_UC(ctrl.text, ctrl.val.ToString(), parentName, lvl, ctrl.min, ctrl.max,ctrl);
            node.Left = hIndent + 5 * lvl;
            node.Top = vIndent;
            c.Controls.Add(node);
            vIndent += 30;
        }

        private bool checkPasswd(string passwd)
        {
            password = "123";
            if (passwd == password) return true;
            else return false;
        }

        private void passwd_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (checkPasswd(passwd_tbx.Text.Trim()))
                {
                    password_pnl.Visible = false;
                    panel1.Visible = true;
                    save_btn.Visible = true;
                }
                else
                {
                    //STRING_Config_UC_046
                }
            }
        }

		public static int CAMERA_NVRAM_SIZE = 64;
		public static int CAMERA_VERSION_NUMBER_OFFSET = 23;
		public static int CAMERA_VERSION_NUMBER_LENGTH = 3;

        public void SaveXml()
        {
            save_btn_Click(null, null);
        }

        XmlTextWriter xmlWriter = null;
        public void writeToXml(string filename)
        {
            XmlConfigUtility.Serialize(ConfigVariables.CurrentSettings, filename);
        }

        public void setValue(FieldInfo[] f,string key,string val)
        {
        }

        public static void setValueIterate(PropertyInfo[] p, string key, string val ,object obj)
        {
            object obj1 = obj;
            for (int i = 0; i < p.Length; i++)
			{
			                 obj = p[i].GetValue(obj1);
                            IVLControlProperties ctrlProp = obj as IVLControlProperties;
                            if (ctrlProp.text == key)
                            {
                                ctrlProp.val = val.ToString();
                                break;
                            }
            }
        }
           public static void setValueIterate(FieldInfo[] f,string key,string val, object obj)
            {
                try
                {
                    object obj1 = obj;
                    for (int i = 0; i < f.Length; i++)
                    {
                            temp = f[i].FieldType.GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance  );
                            obj = f[i].GetValue(obj1);
                            
                        if (temp.Length > 0)
                            {
                                setValueIterate(temp, key, val,obj);
                            }
                        else
                        {
                            PropertyInfo [] pinf = f[i].FieldType.GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance  );
                            setValueIterate(pinf, key, val,obj);
                        }
                    }
                }
                catch (Exception ex)
                { }
            }

        private void save_btn_Click(object sender, EventArgs e)
        {
            // This code is added to save the MD5 password to config
            XmlReadWrite.SetValueIterate(XmlReadWrite.StringArrForPassword[1], XmlReadWrite.StringArrForPassword[2].GetMd5Hash());

            XmlConfigUtility.Serialize(ConfigVariables._ivlConfig, IVLConfig.fileName);
         }

        
            //saveClicked(null, null);
			/*// save in camera NVRAM
			//String ver = camera_version_tb.Text.Trim();
			String ver = Globals.settings.TricamVersion;
			if ( ver.Length == 3 ) {
				// get camera
				Camera cam = Camera.getInstance(this.Handle, this.Handle, new EventHandler(dummy));
				// get version number bytes
				System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
				byte[] ver_bytes = encoding.GetBytes(ver);
				// populate version number inside rambuf
				byte[] rambuf = new byte[CAMERA_NVRAM_SIZE];
				for ( int i = CAMERA_VERSION_NUMBER_OFFSET; i < CAMERA_VERSION_NUMBER_LENGTH; ++i ) {
					rambuf[i] = ver_bytes[i];
				}
				// write rambuf to camera nvram
				cam.WriteEEPROM(0, rambuf, rambuf.Length);
			} else {
				//camera_version_tb.Text = "INVALID";
			}*/
		
		void dummy(Object sender, EventArgs e) {
		}
    }
}

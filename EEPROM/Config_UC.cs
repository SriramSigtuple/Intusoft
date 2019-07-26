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
namespace EEPROM
{
    public delegate void SaveBtnClicked(object sender, EventArgs e);
    public partial class Config_UC : UserControl
    {
        public event SaveBtnClicked saveClicked;
        string configPath = null;
        private string password = null;

        //Multilang:This function initializes the string ID's in this file

        public Config_UC()
        {
            InitializeComponent();

        }


        public KeyValuePair<String, Object> rootDict;
        public int vIndent = 10;
        int hIndent = 10;

        public void addBranchL(KeyValuePair<object, Object> branch, String parentName, int iLevel, Control c)
        {
            EEPROM_Props branchKeyEEPROM = branch.Key as EEPROM_Props;
            XmlLeaf_UC tmp = new XmlLeaf_UC(branchKeyEEPROM.text, branchKeyEEPROM.eepromDataType.dataType, branchKeyEEPROM.value, parentName, iLevel);
            tmp.Left = hIndent + (iLevel - 1) * 30;
            tmp.Top = vIndent;
            c.Controls.Add(tmp);
            vIndent += 30;
            foreach (KeyValuePair<object, Object> pair in branch.Value as Dictionary<Object, object>)
            {
                 EEPROM_Props pairKeyEEPROM = null;
              if(pair.Key is EEPROM_Props)

                 pairKeyEEPROM = pair.Key as EEPROM_Props;
              else
                  pairKeyEEPROM = pair.Value as EEPROM_Props;

                if(pairKeyEEPROM.eepromDataType.dataType != DataTypes.Tree)
                {
                    addLeaf(pairKeyEEPROM, iLevel, parentName + "." + branchKeyEEPROM.name, c);
                
                }
                else
                {
                   
                    addBranchL(pair, parentName + "." + branchKeyEEPROM.name, iLevel + 1, c);
                }
            }
        }


        private void addLeaf(EEPROM_Props ctrl, int lvl, string parentName,Control c)
        {
            
            XmlLeaf_UC node = new XmlLeaf_UC(ctrl.text, ctrl.eepromDataType.dataType,ctrl, parentName, lvl);
            node.Left = hIndent + 5 * lvl;
            node.Top = vIndent;
            vIndent += 30;
            c.Controls.Add(node);
        }



        public void SaveXml()
        {
            save_btn_Click(null, null);
        }

        XmlTextWriter xmlWriter = null;
        public void writeToXml(string filename)
        {
             
        }


        public static void setValueIterate(PropertyInfo[] p, string key, string val ,object obj)
        {
            //object obj1 = obj;
            //for (int i = 0; i < p.Length; i++)
            //{
            //                 obj = p[i].GetValue(obj1);
            //                EEPROM_Props ctrlProp = obj as EEPROM_Props;
            //                if (ctrlProp.text == key)
            //                {
            //                    ctrlProp.val = val.ToString();
            //                    break;
            //                }
            //}
        }

        private void save_btn_Click(object sender, EventArgs e)
        {
            {
                if (File.Exists(configPath))
                {
                   // IVLVariables.CurrentSettings = IVLVariables.CurrentSettings;

                   // File.SetAttributes(configPath, FileAttributes.Normal);
                   // //xmlRW.WriteXml();
                   // writeToXml(IVLConfig.fileName);
                   //IVLVariables._ivlConfig = IVLConfig.getInstance();
                }
             }
         }

        //This below function is added by darshan to update the global property.

        //This below function is added by darshan to update the global property.
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

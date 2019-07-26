using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using log4net;
using log4net.Config;
namespace INTUSOFT.Imaging
{
    public partial class EEPROM_UC : UserControl
    {
        private static readonly ILog Exception_Log = LogManager.GetLogger("INTUSOFT.Desktop"); 

        EEPROM _eeprom;
        EEPROM_Version version;
        int vIndent = 10;
        int hIndent = 10;
        public static Dictionary<string,object> eepromFields;
        public EEPROM_UC()
        {
            InitializeComponent();
            eepromFields = new Dictionary<string, object>();

             version =  EEPROM_Version.GetInstance();
            version.ReadEEPROMVersion();
             MessageBox.Show(Encoding.UTF8.GetString(version.EEPROM_Version_Number.value));
            _eeprom = EEPROM.GetInstance();
            //DisplayEEPROM();
           

        }

        public void ReadEEPROM()
        {
            _eeprom.ReadEEPROM();
            DisplayEEPROM();
        }
        public void WriteEEPROM()
        {
            _eeprom.WriteEEPROM();
        }
        public void ReadEEPROM(byte pageNumber)
        {
            //DisplayEEPROM();
            _eeprom.ReadEEPROM(pageNumber);
        }
        public void ResetEEPROM()
        {
            _eeprom.ResetEEPROM();
        }
        public void WriteEEPROM(byte pageNumber,string data)
        {
            _eeprom.WriteEEPROM( pageNumber,data);
        }
        public void DisplayEEPROM()
        {
            //if (_eeprom == null)
            {
                //_eeprom = new EEPROM();
                //_eeprom.ReadEEPROM(version.Bytes_To_Read_Write);
                TabControl tb = new TabControl();
                FieldInfo[] fieldInf = _eeprom.GetType().GetFields();
                for (int i = 0; i < fieldInf.Length; i++)
                {
                    TabPage tbPage = new TabPage();

                    FieldInfo[] tempInf = fieldInf[i].FieldType.GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance );
                    string output = "";
                    //This below foreach has been added by Darshan on 08-09-2015 to solve Defect no 0000631: The spaces are not left for the Labels in settings window.
                    foreach (char letter in fieldInf[i].FieldType.Name)
                    {
                        if (Char.IsUpper(letter) && output.Length > 0)
                            output += " " + letter;
                        else
                            output += letter;
                    }
                    vIndent = 10;

                    tbPage.Text = output;
                    tbPage.AutoScroll = true;
                    addLeaf(tempInf, tbPage.Text, 1, tbPage);
                    tb.Controls.Add(tbPage);
                }
                tb.Multiline = true;
                tb.Dock = DockStyle.Fill;
                this.Controls.Add(tb);
            }
        }

        private void addLeaf(FieldInfo[] fieldinfos,string name,int ilevel,Control c)
        {
            for (int i = 0; i < fieldinfos.Length; i++)
            {
                if (fieldinfos[i].FieldType.Name.Equals("eepromVar") || fieldinfos[i].FieldType.Name.Equals("eeprom_var_byte") || fieldinfos[i].FieldType.Name.Equals("eeprom_var_float") || fieldinfos[i].FieldType.Name.Equals("eeprom_var_int16") || fieldinfos[i].FieldType.Name.Equals("eeprom_var_int32"))
                {
                    try
                    {
                        object obj = fieldinfos[i].GetValue(null);
                        eepromFields.Add(fieldinfos[i].Name, obj);

                        XmlLeaf_UC xmlLeaf = new XmlLeaf_UC(fieldinfos[i].Name, fieldinfos[i].Name, ilevel, obj);
                        xmlLeaf.Left = hIndent + (ilevel - 1) * 30;
                        xmlLeaf.Top = vIndent;
                        c.Controls.Add(xmlLeaf);
                        vIndent += 30;
                    }
                    catch (Exception ex)
                    {
                        var stringBuilder = new StringBuilder();

                        while (ex != null)
                        {
                            stringBuilder.AppendLine(ex.Message);
                            stringBuilder.AppendLine(ex.StackTrace);

                            ex = ex.InnerException;
                        }
                        Exception_Log.Fatal(stringBuilder.ToString());

                        //throw;
                    }
                }
                else
                {
                    FieldInfo[] tempInf = fieldinfos[i].FieldType.GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance );
                    if (tempInf.Length > 1)
                    {
                        addLeaf(tempInf, fieldinfos[i].FieldType.Name, ilevel + 1, c);
                    }
                    
                    
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EEPROM
{
    public partial class XmlLeaf_UC : UserControl
    {
        string parent = null;
        RadioButton trb;
        RadioButton frb;
        string output = "";
        TextBox textBox;
        Label value_lbl;
        NumericUpDown numericUpDown;
        ComboBox comboBox;
        public XmlLeaf_UC(string lbl,DataTypes datatype, object val, string parents, int lvl)
        {
            InitializeComponent();
            EEPROM_Props value = val as EEPROM_Props;
            label1.Text = lbl;
            string valueStr = "";
            this.parent = parents;
            bool b = false;
            float floatupDownVal = 0;
            int upDownVal = 0;
            if (datatype == DataTypes.Tree)
            {
                label1.Dock = DockStyle.Left;
                label1.Font = new Font("Arial", 16 - lvl, FontStyle.Bold);
                this.Controls.Remove(panel1);
                panel2.Dock = DockStyle.Fill;
                switch (lvl)
                {
                    case 1:
                        label1.ForeColor = Color.RoyalBlue;
                        foreach (char letter in label1.Text)
                        {
                            if (Char.IsUpper(letter) && output.Length > 0)
                                output += " " + letter;
                            else
                                output += letter;
                            label1.Text = output;
                        }
                        break;
                    case 2:
                        label1.ForeColor = Color.OrangeRed;
                        foreach (char letter in label1.Text)
                        {
                            if (Char.IsUpper(letter) && output.Length > 0)
                                output += " " + letter;
                            else
                                output += letter;
                            label1.Text = output;
                        }
                        break;
                    case 3:
                        label1.ForeColor = Color.DarkViolet;
                        foreach (char letter in label1.Text)
                        {
                            if (Char.IsUpper(letter) && output.Length > 0)
                                output += " " + letter;
                            else
                                output += letter;
                            label1.Text = output;
                        }
                        break;
                }
                return;
            }
            
                switch (datatype)
                {
                    case DataTypes.SByte:
                        {
                            EEPROM_Data_Types<sbyte> result = value.value as EEPROM_Data_Types<sbyte>;

                            if (!Variables.isReadOnly)
                            {
                            numericUpDown = new NumericUpDown();
                            numericUpDown.Parent = this.panel1;
                            numericUpDown.Size = new Size(100, 20);
                            numericUpDown.Minimum = (decimal)result.Min;
                            numericUpDown.Maximum = (decimal)result.Max;
                            numericUpDown.Value = (decimal)result.Value;
                            numericUpDown.ValueChanged += new System.EventHandler(numericUpDown_ValueChanged);
                            numericUpDown.KeyPress += numericUpDown_KeyPress;
                            label2.Text = "  " + result.Min.ToString() + "  to  " + result.Max.ToString();
                            label2.Dock = DockStyle.Right;
                            label2.Parent = this.panel1;
                            }
                            break;
                        }
                    case DataTypes.Byte:
                        {
                            EEPROM_Data_Types<byte> result = value.value as EEPROM_Data_Types<byte>;
                        
                           {
                               if (result.Min == byte.MinValue && result.Max == (byte)1)
                               {
                                   if (!Variables.isReadOnly)
                                   {
                                       frb = new RadioButton();
                                       frb.Size = new Size(60, 10);
                                       frb.Text = "False";
                                       frb.Dock = DockStyle.Left;
                                       frb.Parent = panel1;
                                       trb = new RadioButton();
                                       trb.Size = new Size(60, 10);
                                       trb.Text = "True";
                                       trb.Dock = DockStyle.Left;
                                       trb.Parent = panel1;
                                       if (result.Value == (byte)1)
                                       {
                                           trb.Checked = true;
                                       }
                                       else
                                       {
                                           frb.Checked = true;
                                       }
                                       trb.CheckedChanged += new System.EventHandler(trb_CheckedChanged);
                                   }
                                   else
                                   {
                                        if (result.Value == (byte)1)
                                            valueStr = "True";
                                        else
                                            valueStr = "False";

                                        PopulateReadOnlyValue(valueStr, "False", "True", "");
                                   }
                               }
                               else if (value.range.Split(',').Length > 1)
                               {
                                   if (!Variables.isReadOnly)
                                   {
                                       comboBox = new ComboBox();
                                       comboBox.Size = new Size(100, 20);
                                       comboBox.Parent = this.panel1;
                                       string[] comboBoxData = value.range.Split(',');
                                       comboBox.DataSource = comboBoxData;
                                       comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
                                   }
                                   else
                                   {
                                       PopulateReadOnlyValue(valueStr, value.range, "", "");
                                   }
                               }

                               else
                               {
                                   if (!Variables.isReadOnly)
                                   {
                                       numericUpDown = new NumericUpDown();
                                       numericUpDown.Parent = this.panel1;
                                       numericUpDown.Size = new Size(100, 20);
                                       numericUpDown.Minimum = (decimal)result.Min;
                                       numericUpDown.Maximum = (decimal)result.Max;
                                       numericUpDown.Value = (decimal)result.Value;
                                       numericUpDown.ValueChanged += new System.EventHandler(numericUpDown_ValueChanged);
                                       numericUpDown.KeyPress += numericUpDown_KeyPress;
                                       label2.Text = "  " + result.Min.ToString() + "  to  " + result.Max.ToString();
                                       label2.Dock = DockStyle.Right;
                                       label2.Parent = this.panel1;
                                   }
                                   else
                                   {
                                       PopulateReadOnlyValue(result.Value.ToString(), result.Min.ToString(), result.Max.ToString(), "");


                                   }
                               }
                           }
                            break;
                        }
                    case DataTypes.Single:
                        {
                            EEPROM_Data_Types<float> result = value.value as EEPROM_Data_Types<float>;
                          if (!Variables.isReadOnly)
                          {
                            numericUpDown = new NumericUpDown();
                            numericUpDown.Parent = this.panel1;
                            numericUpDown.Size = new Size(100, 20);
                            numericUpDown.Minimum = (decimal)result.Min;
                            numericUpDown.Maximum = (decimal)result.Max;
                            numericUpDown.Value = (decimal)result.Value;
                            numericUpDown.DecimalPlaces = 2;
                            numericUpDown.ValueChanged += new System.EventHandler(numericUpDown_ValueChanged);
                            numericUpDown.KeyPress += numericUpDown_KeyPress;
                            label2.Text = "  " + result.Min.ToString() + "  to  " + result.Max.ToString();
                            label2.Dock = DockStyle.Right;
                            label2.Parent = this.panel1;
                          }
                          else
                              PopulateReadOnlyValue(result.Value.ToString(), result.Min.ToString(), result.Max.ToString(), "");

                            break;
                        }
                    case DataTypes.Int16:
                        {
                            EEPROM_Data_Types<Int16> result = value.value as EEPROM_Data_Types<Int16>;
                           if (!Variables.isReadOnly)
                           {numericUpDown = new NumericUpDown();
                            numericUpDown.Parent = this.panel1;
                            numericUpDown.Size = new Size(100, 20);
                            numericUpDown.Minimum = (decimal)result.Min;
                            numericUpDown.Maximum = (decimal)result.Max;
                            numericUpDown.Value = (decimal)result.Value;
                            numericUpDown.ValueChanged += new System.EventHandler(numericUpDown_ValueChanged);
                            numericUpDown.KeyPress += numericUpDown_KeyPress;
                            label2.Text = "  " + result.Min.ToString() + "  to  " + result.Max.ToString();
                            label2.Dock = DockStyle.Right;
                            label2.Parent = this.panel1;
                           }
                           else
                               PopulateReadOnlyValue(result.Value.ToString(), result.Min.ToString(), result.Max.ToString(), "");

                            break;
                        }
                    case DataTypes.UInt16:
                        {
                            EEPROM_Data_Types<UInt16> result = value.value as EEPROM_Data_Types<UInt16>;
                            if (!Variables.isReadOnly)
                            {
                                numericUpDown = new NumericUpDown();
                                numericUpDown.Parent = this.panel1;
                                numericUpDown.Size = new Size(100, 20);
                                numericUpDown.Minimum = (decimal)result.Min;
                                numericUpDown.Maximum = (decimal)result.Max;
                                numericUpDown.Value = (decimal)result.Value;
                                numericUpDown.ValueChanged += new System.EventHandler(numericUpDown_ValueChanged);
                                numericUpDown.KeyPress += numericUpDown_KeyPress;
                                label2.Text = "  " + result.Min.ToString() + "  to  " + result.Max.ToString();
                                label2.Dock = DockStyle.Right;
                                label2.Parent = this.panel1;
                            }
                            else
                                PopulateReadOnlyValue(result.Value.ToString(), result.Min.ToString(), result.Max.ToString(), "");

                            break;
                        }
                    case DataTypes.Int32:
                        {
                            EEPROM_Data_Types<Int32> result = value.value as EEPROM_Data_Types<Int32>;
                            if (!Variables.isReadOnly)
                            {
                                numericUpDown = new NumericUpDown();
                                numericUpDown.Parent = this.panel1;
                                numericUpDown.Size = new Size(100, 20);
                                numericUpDown.Minimum = (decimal)result.Min;
                                numericUpDown.Maximum = (decimal)result.Max;
                                numericUpDown.Value = (decimal)result.Value;
                                numericUpDown.ValueChanged += new System.EventHandler(numericUpDown_ValueChanged);
                                numericUpDown.KeyPress += numericUpDown_KeyPress;
                                label2.Text = "  " + result.Min.ToString() + "  to  " + result.Max.ToString();
                                label2.Dock = DockStyle.Right;
                                label2.Parent = this.panel1;
                            }
                            else
                            {
                                PopulateReadOnlyValue(result.Value.ToString(), result.Min.ToString(), result.Max.ToString(), "");

                            }
                            break;
                        }
                    case DataTypes.ByteArr:
                        {
                            if (!Variables.isReadOnly)
                            {
                                textBox = new TextBox();
                                textBox.Parent = this.panel1;
                                textBox.Dock = DockStyle.Left;
                                textBox.Multiline = true;
                                textBox.Width = 150;
                                textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
                                textBox.TextChanged += textBox_TextChanged;
                                textBox.KeyPress += textBox_KeyPress;
                                textBox.MaxLength = value.eepromDataType.length;
                                textBox.Text = (value.value as string);
                                label2.Text = value.eepromDataType.length.ToString() + " chars";
                                label2.Dock = DockStyle.Right;
                                textBox.Dock = DockStyle.Left;

                                label2.Parent = this.panel1;
                            }
                            else
                            {
                                PopulateReadOnlyValue((value.value as string), "", "", value.eepromDataType.length.ToString() + " chars");
                            }
                            break;
                        }
                }
            }

        private void PopulateReadOnlyValue(string value,string min,string max,string length)
        {

            value_lbl = new Label();
            value_lbl.Parent = this.panel1;
            value_lbl.Dock = DockStyle.Left;
            value_lbl.Text = value;// as string);
            if(string.IsNullOrEmpty(min) && string.IsNullOrEmpty(max))
            label2.Text =length + " chars";
            else
            label2.Text = "  " + min + "  to  " + max ;
            label2.Dock = DockStyle.Right;
            label2.Parent = this.panel1;
        }
        void numericUpDown_TextChanged(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            //XmlReadWrite.SetValue(str, numericUpDown.Value.ToString());
        }

        void textBox_TextChanged(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            if (!label1.Text.Equals("Header Text"))
                XmlReadWrite.SetValue(str, textBox.Text);
            else
            {
                if(!string.IsNullOrEmpty(textBox.Text))
                XmlReadWrite.SetValue(str, textBox.Text);
            }
        }

        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            XmlReadWrite.SetValue(str, comboBox.Text);
        }

        void numericUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            //This condition was added by Darshan on 25-jan-2016 to prevent the numeric updown control from accepting negative symbol.
            if (e.KeyChar.Equals('-'))
                e.Handled = true;
        }

        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
                if (char.IsDigit(e.KeyChar))
                    e.Handled = true;
                else
                    e.Handled = false;
        }



        void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            XmlReadWrite.SetValue(str, numericUpDown.Value);
        }
       

        void trb_CheckedChanged(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            if (trb.Checked)
            {

                XmlReadWrite.SetValue(str, (byte)1);
            }
            else
            {
                XmlReadWrite.SetValue(str,(byte)0);
            }
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                System.Windows.Forms.SendKeys.Send("{TAB}");
                e.SuppressKeyPress = true;
            }
        }





    }
  
}

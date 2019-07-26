using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;

namespace INTUSOFT.Imaging
{
    public partial class XmlLeaf_UC : UserControl
    {
        string parent = null;
        RadioButton trb;
        RadioButton frb;
        string output = "";
        TextBox textBox;
        NumericUpDown numericUpDown, numericUpDownMin, numericUpDownMax;
        ComboBox comboBox;
        public XmlLeaf_UC(string lbl, string parents, int lvl, object obj)
        {
            InitializeComponent();

            label1.Text = lbl;
            this.parent = parents;
            bool b = false;
            float floatupDownVal = 0;
            int upDownVal = 0;
            string val1 = "";
            //if (val == null)
            {
                //textBox.Hide();
                label1.Dock = DockStyle.Left;
                label1.Font = new Font("Arial", 10 - lvl, FontStyle.Bold);


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
                        label1.ForeColor = Color.OrangeRed; ;
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

            }

            if (obj is eeprom_var_byte || obj is eeprom_var_float || obj is eeprom_var_int16 || obj is eeprom_var_int32)
            {

                numericUpDown = new NumericUpDown();
                numericUpDownMin = new NumericUpDown();
                numericUpDownMax = new NumericUpDown();
                TableLayoutPanel tbLayout = new TableLayoutPanel();
                tbLayout.ColumnCount = 8;
                tbLayout.RowCount = 1;
                for (int i = 0; i < tbLayout.ColumnCount; i++)
                {
                    if(i%2 !=0)

                    tbLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                    else
                        tbLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));

                }
                //tbLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
               // numericUpDown.Parent = this.panel1;
                numericUpDown.Size = new Size(70, 20);
                //numericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                //numericUpDownMin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                //numericUpDownMax.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                numericUpDown.Dock = DockStyle.Fill;
                

               // numericUpDownMin.Parent = this.panel1;
                numericUpDownMin.Size = new Size(70, 20);


               // numericUpDownMax.Parent = this.panel1;
                numericUpDownMax.Size = new Size(70, 20);
                numericUpDownMin.Dock = DockStyle.Fill;
                numericUpDownMax.Dock = DockStyle.Fill;
                if (obj is eeprom_var_float )
                    numericUpDown.DecimalPlaces = 2;
                if (obj is eeprom_var_byte)
                {
                    eeprom_var_byte temp = obj as eeprom_var_byte;

                    numericUpDown.Minimum = (decimal)temp.min;
                    numericUpDown.Maximum = (decimal)temp.max;
                    numericUpDownMin.Minimum = byte.MinValue;
                    numericUpDownMin.Maximum = byte.MaxValue;

                    numericUpDownMax.Minimum = byte.MinValue;
                    numericUpDownMax.Maximum = byte.MaxValue;

                    numericUpDownMin.Value = (decimal)temp.min;
                    numericUpDownMax.Value = (decimal)temp.max;
                    numericUpDown.Value = (decimal)temp.value;

                }
                else if (obj is eeprom_var_int16)
                {
                    eeprom_var_int16 temp = obj as eeprom_var_int16;
                    numericUpDown.Minimum = (decimal)temp.min;
                    numericUpDown.Maximum = (decimal)temp.max;
                    numericUpDownMin.Minimum = Int16.MinValue;
                    numericUpDownMin.Maximum = Int16.MaxValue;

                    numericUpDownMax.Minimum = Int16.MinValue;
                    numericUpDownMax.Maximum = Int16.MaxValue;

                    numericUpDownMin.Value = (decimal)temp.min;
                    numericUpDownMax.Value = (decimal)temp.max;
                    numericUpDown.Value = (decimal)temp.value;
                   

                }
                else if (obj is eeprom_var_int32)
                {
                    eeprom_var_int32 temp = obj as eeprom_var_int32;
                    numericUpDown.Minimum = (decimal)temp.min;
                    numericUpDown.Maximum = (decimal)temp.max;
                    numericUpDownMin.Minimum = Int32.MinValue;
                    numericUpDownMin.Maximum = Int32.MaxValue;

                    numericUpDownMax.Minimum = Int32.MinValue;
                    numericUpDownMax.Maximum = Int32.MaxValue;

                    numericUpDownMin.Value = (decimal)temp.min;
                    numericUpDownMax.Value = (decimal)temp.max;
                    numericUpDown.Value = (decimal)temp.value;

                }
                else if (obj is eeprom_var_float)
                {
                    eeprom_var_float temp = obj as eeprom_var_float;
                    numericUpDown.Minimum = (decimal)temp.min;
                    numericUpDown.Maximum = (decimal)temp.max;
                    numericUpDownMin.Minimum = Int32.MinValue;
                    numericUpDownMin.Maximum = Int32.MaxValue;

                    numericUpDownMax.Minimum = Int32.MinValue;
                    numericUpDownMax.Maximum = Int32.MaxValue;

                    numericUpDownMin.Value = (decimal)temp.min;
                    numericUpDownMax.Value = (decimal)temp.max;
                    numericUpDown.Value = (decimal)temp.value;
                  

                }
                //numericUpDown.ValueChanged += new System.EventHandler(numericUpDown_ValueChanged);
                numericUpDown.TextChanged += new System.EventHandler(numericUpDown_TextChanged);

                label2.Text = "  " + numericUpDown.Minimum.ToString() + "  to  " + numericUpDown.Maximum.ToString();
                label2.Dock = DockStyle.Fill;
                tbLayout.Controls.Add(numericUpDown, 1, 0);
                tbLayout.Controls.Add(numericUpDownMin, 3, 0);
                tbLayout.Controls.Add(numericUpDownMax, 5, 0);
                tbLayout.Controls.Add(label2, 7, 0);
                tbLayout.Dock = DockStyle.Fill;
                tbLayout.Parent = this.panel1;
                //this.panel1.Controls.Add(tbLayout);

                //tbLayout.SetColumn(numericUpDown, 0);
                //tbLayout.SetColumn(numericUpDownMin, 1);
                //tbLayout.SetColumn(numericUpDownMax, 2);
                //tbLayout.SetColumn(label2, 3);
               // label2.Dock = DockStyle.Left;

               // label2.Parent = this.panel1;
                //label1.Dock = DockStyle.Left;
                //label1.Font = new Font("Arial", 11 - lvl, FontStyle.Bold);
                //label1.Location 
                //label1.Parent = this.panel1;
                //string paret = parents.Substring(parents.LastIndexOf("."), (int)((parents.Length - 1) - (int)parents.LastIndexOf(".") + 1));
                //paret = paret.Replace(".", string.Empty);

                //XmlReadWrite.setControlValue(c,paret);

            }


                else if (obj is eepromVar)
                {
                        textBox = new TextBox();
                        textBox.Parent = this.panel1;
                        textBox.Multiline = true; //Added to show Disclaimers in two lines by sriram on 23 August 2012
                        //painful workaround for not autoscrolling of text
                        // added for DisclaimerText4AutoAnalysis and AlternativeDisclaimerText also on 27th July 2012..

                        {
                            textBox.Dock = DockStyle.Left;
                        }
                        
                        textBox.Leave += new System.EventHandler(textBox_Leave);
                        textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
                        if (lbl.Equals("Doctor Name"))
                        textBox.KeyPress += textBox_KeyPress;
                        int x = 0;
                        double y = 0;
                        eepromVar temp = obj as eepromVar;
                   
                        textBox.MaxLength = temp.value.Length;
                        label2.Text = "Max  " + textBox.MaxLength.ToString() +" Chars";
                        label2.Dock = DockStyle.Right;

                        label2.Parent = this.panel1;
                        textBox.Text =Encoding.UTF8.GetString(temp.value);;
                        textBox.TextChanged += textBox_TextChanged;
                }
                //else if ("System.Windows.Forms.ComboBox" == controlProps.control)
                //{
                //    comboBox = new ComboBox();
                //    comboBox.Parent = this.panel1;
                //    string[] comboBoxCollection = controlProps.range.Split(',');
                //    comboBox.DataSource = comboBoxCollection;
                //    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                //    comboBox.Text = controlProps.val;
                //    comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;

                //}
            }

        void numericUpDown_TextChanged(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            XmlReadWrite.SetValue(str, numericUpDown.Value.ToString());
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
            // XmlReadWrite.SetValue(str, comboBox.Text);

        }

        void numericUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            //This condition was added by Darshan on 25-jan-2016 to prevent the numeric updown control from accepting negative symbol.
            if (e.KeyChar.Equals('-'))
                e.Handled = true;
        }

        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (textBox.Text.Any(c => char.IsDigit(c)))
            //{
            //    if (char.IsDigit(e.KeyChar))
            //        e.Handled = false;
            //    else if (char.IsLetter(e.KeyChar))
            //        e.Handled = true;

            //}
            //else
            {
                if (char.IsDigit(e.KeyChar))
                    e.Handled = true;
                else
                    e.Handled = false;
            }
        }



        void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            //XmlReadWrite.SetValue(str, numericUpDown.Value.ToString());
        }


        void trb_CheckedChanged(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            if (trb.Checked)
            {
                // XmlReadWrite.SetValue(str, trb.Text.ToLower());
            }
            else
            {
                //XmlReadWrite.SetValue(str, frb.Text.ToLower());
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

        void textBox_Leave(object sender, EventArgs e)
        {

            string str = this.parent + "." + label1.Text;
            if (!label1.Text.Equals("Header Text"))
                XmlReadWrite.SetValue(str, textBox.Text);
            else
            {
                if (!string.IsNullOrEmpty(textBox.Text))
                    XmlReadWrite.SetValue(str, textBox.Text);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

    }

}

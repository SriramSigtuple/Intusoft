using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;

namespace INTUSOFT.Configuration
{
    public partial class XmlLeaf_UC : UserControl
    {
        string parent = null;
        RadioButton trb;
        RadioButton frb;
        string output = "";
        TextBox textBox;
        NumericUpDown numericUpDown;
        ComboBox comboBox;
        public XmlLeaf_UC(string lbl, string val, string parents, int lvl,double min,double max,string additionalText = "")
        {
            
            InitializeComponent();
            
            label1.Text = lbl;
            this.parent = parents;
            bool b = false;
            float floatupDownVal = 0;
            int upDownVal = 0;
            string val1 = "";
            if (val == null)
            {
                //textBox.Hide();
                label1.Dock = DockStyle.Left;
                float fontSize = 12.25f - (float)lvl;
                label1.Font = new Font("Arial", fontSize, FontStyle.Bold);
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
            val1=val.ToLower();
            {
                if (Int32.TryParse(val, out upDownVal))
                {
                    numericUpDown = new NumericUpDown();
                    numericUpDown.Parent = this.panel1;
                    numericUpDown.Size = new Size(70, 20);
                    numericUpDown.Minimum = (decimal)min;
                    numericUpDown.Maximum = (decimal)max;
                    numericUpDown.Value = (decimal)upDownVal;
                    numericUpDown.ValueChanged += new System.EventHandler(numericUpDown_ValueChanged);
                    numericUpDown.KeyPress += numericUpDown_KeyPress;
                    label2.Text = "  "+min.ToString()+"  to  "+max.ToString();
                    label2.Dock = DockStyle.Right;
                    label2.Parent = this.panel1;
                    //label1.Dock = DockStyle.Left;
                    //label1.Font = new Font("Arial", 11 - lvl, FontStyle.Bold);
                    //label1.Location 
                    //label1.Parent = this.panel1;
                    //string paret = parents.Substring(parents.LastIndexOf("."), (int)((parents.Length - 1) - (int)parents.LastIndexOf(".") + 1));
                    //paret = paret.Replace(".", string.Empty);
                    //XmlReadWrite.setControlValue(c,paret);
                }
                else if(float.TryParse(val,out floatupDownVal))
                    {
                        numericUpDown = new NumericUpDown();
                        numericUpDown.Parent = this.panel1;
                        numericUpDown.Size = new Size(70, 20);
                        numericUpDown.Minimum = (decimal)min;
                        numericUpDown.Maximum = (decimal)max;
                        numericUpDown.DecimalPlaces = 2;
                        numericUpDown.Value = (decimal)(floatupDownVal);
                        numericUpDown.ValueChanged += new System.EventHandler(numericUpDown_ValueChanged);
                        numericUpDown.KeyPress+=numericUpDown_KeyPress;
                        label2.Text = "  " + min.ToString() + "  to  " + max.ToString();
                        label2.Dock = DockStyle.Right;
                        label2.Parent = this.panel1;
                     }
                else
                {
                    if (Boolean.TryParse(val1, out b))
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
                        if (val1 == "true")
                        {
                            trb.Checked = true;
                        }
                        else
                        {
                            frb.Checked = true;
                        }
                        trb.CheckedChanged += new System.EventHandler(trb_CheckedChanged);
                    }
                    else if (val1.Contains("show") || val1.Contains("hide"))
                    {
                        frb = new RadioButton();
                        frb.Size = new Size(60, 10);
                        frb.Text = "Hide";
                        frb.Dock = DockStyle.Left;
                        frb.Parent = panel1;
                        trb = new RadioButton();
                        trb.Size = new Size(60, 10);
                        trb.Text = "Show";
                        trb.Dock = DockStyle.Left;
                        trb.Parent = panel1;
                        if (val1 == "show")
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
                        textBox = new TextBox();
                        textBox.Parent = this.panel1;
                        textBox.Multiline = true; //Added to show Disclaimers in two lines by sriram on 23 August 2012
                        //painful workaround for not autoscrolling of text
                        // added for DisclaimerText4AutoAnalysis and AlternativeDisclaimerText also on 27th July 2012..
                        if (lbl.Contains("Installation") || lbl.Contains("DisclaimerText4AutoAnalysis") || lbl.Contains("AlternateDisclaimerText"))
                        {
                            textBox.Dock = DockStyle.Fill;
                        }
                        else
                        {
                            textBox.Dock = DockStyle.Left;
                        }
                       
                        //textBox.Leave += new System.EventHandler(textBox_Leave);
                        textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
                        textBox.KeyPress += textBox_KeyPress;
                        int x = 0;
                        double y = 0;
                        if (int.TryParse(val1.Trim(), out x) || double.TryParse(val1.Trim(), out y))
                        {
                            textBox.Size = new Size(40, 10);
                        }
                        else
                        {
                            if (val1.Trim().Length < 1)
                            {
                                textBox.Size = new Size(100, 10);
                            }
                            else
                            {
                                textBox.Size = new Size(val1.Trim().Length * 15, 10);
                            }
                        }
                        textBox.Text = val;
                        if (!string.IsNullOrEmpty(additionalText))
                        {
                            label2.Text = additionalText;
                            label2.Dock = DockStyle.Right;
                            label2.Parent = this.panel1;
                        }
                    }
                }
            }
        }

        public XmlLeaf_UC(string lbl, string val, string parents, int lvl, double min, double max,IVLControlProperties controlProps)
        {
            InitializeComponent();
            label1.Text = lbl;
            this.parent = parents;
            bool b = false;
            float floatupDownVal = 0;
            int upDownVal = 0;
            string val1 = "";
            if (val == null)
            {
                //textBox.Hide();
                label1.Dock = DockStyle.Left;
                label1.Font = new Font("Arial", 11 - lvl, FontStyle.Bold);
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
                return;
            }
            val1 = val.ToLower();
            Control c = new Control(controlProps.control);
            if ("System.Windows.Forms.NumericUpDown" == controlProps.control)
                {
                    numericUpDown = new NumericUpDown();
                    numericUpDown.Parent = this.panel1;
                    numericUpDown.Size = new Size(70, 20);
                    numericUpDown.Minimum = (decimal)min;
                    numericUpDown.Maximum = (decimal)max;
                    //if (controlProps.type == "float" || controlProps.type == "double")
                    //    numericUpDown.DecimalPlaces = 2;
                    //if (controlProps.name == "ClipValue")
                    //    numericUpDown.DecimalPlaces = 4;
                    if (controlProps.type == "float" || controlProps.type == "double")
                    {
                        //    numericUpDown.DecimalPlaces = 2;
                        //if (controlProps.name == "ClipValue")
                        numericUpDown.DecimalPlaces = controlProps.val.Length - 1;
                    }
                    if (float.TryParse(val, out floatupDownVal))
                    {
                        numericUpDown.Value = (decimal)(floatupDownVal);
                    }
                    else if (Int32.TryParse(val, out upDownVal))
                    {
                        numericUpDown.Value = (decimal)(upDownVal);
                    }
                    numericUpDown.ValueChanged += new System.EventHandler(numericUpDown_ValueChanged);
                    //numericUpDown.TextChanged += new System.EventHandler(numericUpDown_TextChanged);
                    label2.Text = "  " + min.ToString() + "  to  " + max.ToString();
                    label2.Dock = DockStyle.Right;
                    label2.Parent = this.panel1;
                    //label1.Dock = DockStyle.Left;
                    //label1.Font = new Font("Arial", 11 - lvl, FontStyle.Bold);
                    //label1.Location 
                    //label1.Parent = this.panel1;
                    //string paret = parents.Substring(parents.LastIndexOf("."), (int)((parents.Length - 1) - (int)parents.LastIndexOf(".") + 1));
                    //paret = paret.Replace(".", string.Empty);
                    //XmlReadWrite.setControlValue(c,paret);
                }
                else
                {
                    if ( "System.Windows.Forms.RadioButton" == controlProps.control)
                    {
                        if (val1.Contains("show") || val1.Contains("hide"))
                        {
                            frb = new RadioButton();
                            frb.Size = new Size(60, 10);
                            frb.Text = "Hide";
                            frb.Dock = DockStyle.Left;
                            frb.Parent = panel1;
                            trb = new RadioButton();
                            trb.Size = new Size(60, 10);
                            trb.Text = "Show";
                            trb.Dock = DockStyle.Left;
                            trb.Parent = panel1;
                            if (val1 == "show")
                            {
                                trb.Checked = true;
                            }
                            else
                            {
                                frb.Checked = true;
                            }
                        }
                        else
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
                            if (val1 == "true")
                            {
                                trb.Checked = true;
                            }
                            else
                            {
                                frb.Checked = true;
                            }
                        }
                        trb.CheckedChanged += new System.EventHandler(trb_CheckedChanged);
                    }
                    else if("System.Windows.Forms.TextBox" == controlProps.control )
                    {
                        textBox = new TextBox();
                        textBox.Parent = this.panel1;
                        textBox.Multiline = true; //Added to show Disclaimers in two lines by sriram on 23 August 2012
                        //painful workaround for not autoscrolling of text
                        // added for DisclaimerText4AutoAnalysis and AlternativeDisclaimerText also on 27th July 2012..
                        {
                            textBox.Dock = DockStyle.Left;
                        }
                       // textBox.Leave += new System.EventHandler(textBox_Leave);
                        textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
                        if (lbl.Equals("Doctor Name"))
                        textBox.KeyPress += textBox_KeyPress;
                        int x = 0;
                        double y = 0;
                        if (int.TryParse(val1.Trim(), out x) || double.TryParse(val1.Trim(), out y))
                        {
                            textBox.Size = new Size(40, 10);
                        }
                        else
                        {
                            //if (val1.Trim().Length <= 6)
                            {
                                textBox.Size = new Size(350, 10);
                            }
                            //else
                            //{
                            //    textBox.Size = new Size(val1.Trim().Length * 25, 10);
                            //}
                        }
                    if (lbl.Contains("Password"))
                    {
                        textBox.PasswordChar = '*';
                    }
                    textBox.Text = val;
                        textBox.MaxLength = controlProps.length;
                        textBox.TextChanged += textBox_TextChanged;
                        if (!string.IsNullOrEmpty(controlProps.range) && controlProps.range != "0to200000")
                        {
                            textBox.Size = new Size(100, 10);

                            label2.Text = controlProps.range;
                            label2.Dock = DockStyle.Right;
                            label2.Parent = this.panel1;
                        }
                    }
                    else if ("System.Windows.Forms.ComboBox" == controlProps.control)
                    {
                        comboBox = new ComboBox();
                        comboBox.Parent = this.panel1;
                        string[] comboBoxCollection =  controlProps.range.Split(',');
                        comboBox.DataSource = comboBoxCollection;
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        comboBox.Text = controlProps.val;
                        comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
                    }
                }
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
            XmlReadWrite.SetValue(str, numericUpDown.Value.ToString());
        }
       

        void trb_CheckedChanged(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            if (trb.Checked)
            {
                XmlReadWrite.SetValue(str, trb.Text.ToLower());
            }
            else
            {
                XmlReadWrite.SetValue(str, frb.Text.ToLower());
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

        //void textBox_Leave(object sender, EventArgs e)
        //{

        //    string str = this.parent + "." + label1.Text;
        //    if (!label1.Text.Equals("Header Text"))
        //        XmlReadWrite.SetValue(str, textBox.Text);
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(textBox.Text))
        //            XmlReadWrite.SetValue(str, textBox.Text);
        //    }
        //}

    }
  
}

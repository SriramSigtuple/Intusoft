using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ReportUtils;
using System.IO;
using ReportUtils.ReportEnums;
using System.Reflection;
using Newtonsoft.Json;

namespace DesignSurfaceManagerExtension
{
    public enum ValueChanged { Text, LocationX, LocationY, Tag, Size, CellBorderStyle, Location, Width, Height, Font, ForeColor, RowCount, ImageLocation, ColumnCount, BindingType, BorderStyle, Multiline, Picture,ImageCount,XMargin,YMargin,TextAlign, None }

    public partial class CustomPropertyGridHost : UserControl
    {
        public delegate void _InvokeUpdateGridview();
        public event _InvokeUpdateGridview invokeUpdate;
        Font_UC fontUC;
        ValueChanged valueChanged;
        Font font;
        public ReportControlsStructure reportCtrlProp;
        ImageNameSettingsWindow imageNameSettingWindow;
        private DesignSurfaceManagerExt SurfaceManager { get; set; }
        private const string _Name_ = "CustomPropertyGridHost";
        short XLocation = 10;
        short YLocation = 10;
        int rowsCount;
        int colsCount;
        int Width;
        int Height;
        public string controlDisplayText = string.Empty;
        string textBoxValue = "Text Box";
        string editBoxValue = "Edit Box";
        string pictureBoxValue = "Logo";
        string tableLayoutValue = "Table";
        string imageTableValue = "Image Table";
        static string bindingFileName = @"BindingEnums.json";
        IVLFont ivlFont;
        byte Rows = 1;
        object propertyValue;
        Color colorValue;
        TextAlign TextAlignVal;
        object value = null;
        bool isControlsAdded = false;
        byte Columns = 1;
        PropertyGrid prop;
        public static List<string> bindingTypeAdd;
        string logoDefaultValue = @"IconsReportTools\logo.jpg";
        private bool _bSuppressEvents = false;
        public List<ReportControlsStructure> reportCtrlList;
        public PropertyGrid PropertyGrid { get { return prop; } }
        public enum PropertyName { ImageLocation, Width, Height, LocationX, LocationY, Text, Multiline, Font, CellBorderStyle, BorderStyle, ForeColor,ImageName,TextAlign };
        public CustomPropertyGridHost()
        {
            InitializeComponent();
            //GenericGrid = new PropertyGrid();
            //GenericGrid.Dock = DockStyle.Fill;
            //this.Controls.Add(GenericGrid);
            //GenericGrid.BringToFront();
        }

        public CustomPropertyGridHost(DesignSurfaceManagerExt surfaceManager)
        {
            imageNameSettingWindow = new ImageNameSettingsWindow();
            bindingTypeAdd = new List<string>();
            const string _signature_ = _Name_ + @"::ctor()";
            InitializeComponent();
            prop = new PropertyGrid();
            reportCtrlList = new List<ReportControlsStructure>();
            this.Dock = DockStyle.Fill;
            fontUC = new Font_UC();
            if (!File.Exists(bindingFileName))
            {
                foreach (BindingType item in Enum.GetValues(typeof(BindingType)) as BindingType[])
                {
                    bindingTypeAdd.Add("$" + item.ToString());
                }
            }
            else
            {
                Deserialize();
            }
            Binding_cbx.DataSource = bindingTypeAdd;
            LocationX_nud.Value = XLocation;
            LocationY_nud.Value = YLocation;
            NameOfTheImage_tbx.Text = logoDefaultValue;
            panel1.Controls.Add(fontUC);
            ivlFont = new IVLFont();
            //- the surface manager strictly tied with PropertyGridHost
            if (null == surfaceManager)
                throw new ArgumentNullException("surfaceManager", _signature_ + " - Exception: invalid argument (null)!");
            fontUC.Dock = DockStyle.Fill;
            fontUC.fontValueUpdate += fontUC_fontValueUpdate;
            SurfaceManager = surfaceManager;
        }

        void fontUC_fontValueUpdate(ValueChanged valueChanged, object value, Color color)
        {
            this.valueChanged = valueChanged;
            this.value = value;
            if (color.IsKnownColor)
                this.colorValue = color;
            else
            {
                this.colorValue = ColorTranslator.FromHtml("#" + color.Name);
            }
            InitUpdatedValues();
        }

        public static void Serialize()
        {
            string binding = JsonConvert.SerializeObject(bindingTypeAdd);
            File.WriteAllText(bindingFileName, binding);
        }

        void Deserialize()
        {
            string bindingsEnum = File.ReadAllText(bindingFileName);
            bindingTypeAdd = (List<string>)JsonConvert.DeserializeObject(bindingsEnum, typeof (List<string>));
        }

        public void ReloadControlProperties()
        {
            _bSuppressEvents = true;
            try
            {
                //- IDesignerEventService provides a global eventing mechanism for designer events. With this mechanism,
                //- an application is informed when a designer becomes active. The service provides a collection of
                //- designers and a single place where global objects, such as the Properties window, can monitor selection
                //- change events.
                IDesignerEventService des = (IDesignerEventService)SurfaceManager.GetService(typeof(IDesignerEventService));
                if (null == des)
                    return;
                IDesignerHost host = des.ActiveDesigner;
                if (null == SelectedObject)
                    return; //- don't reload at all
                //- get the name of the control selected from the comboBox
                //- and if we are not able to get it then it's better to exit
                string sName = string.Empty;
                if (SelectedObject is Form)
                {
                    sName = ((Form)SelectedObject).Name;
                }
                else if (SelectedObject is System.Windows.Forms.Control)
                {
                    sName = ((Control)SelectedObject).Site.Name;
                }
                if (string.IsNullOrEmpty(sName))
                    return;
                //- prepare the data for reloading the combobox (begin)
                List<object> ctrlsToAdd = new List<object>();
                string pgrdComboBox_Text = string.Empty;
                Property_tbx.Text = sName;
                SetSelctedObjectProperties(SelectedObject);
                try
                {
                    ComponentCollection ctrlsExisting = host.Container.Components;
                    Debug.Assert(0 != ctrlsExisting.Count);
                    foreach (Component comp in ctrlsExisting)
                    {
                        string sItemText = TranslateComponentToName(comp);
                        ctrlsToAdd.Add(sItemText);
                        if (sName == comp.Site.Name)
                            pgrdComboBox_Text = sItemText;
                    }//end_foreach
                }
                catch (Exception)
                {
                    return; //- (rollback)
                }
            }
            finally
            {
                _bSuppressEvents = false;
            }
        }

        public void SetSelctedObjectProperties(object seletedctrl)
        {
            if (seletedctrl is Form)
            {
                Form frm = ((Form)SelectedObject);
                numberOfImages_nud.Enabled = false;
                Property_tbx.Text = ((Form)SelectedObject).Name;
                //Text_tbx.Text = frm.Text;
             
                if (frm.Location.X > 0)
                    LocationX_nud.Value = frm.Location.X;
                if (frm.Location.Y > 0)
                    LocationY_nud.Value = frm.Location.Y;
                Width_nud.Value = frm.Width;
                Height_nud.Value = frm.Height;
                controlDisplayText = "";
            }
            else
            {
                {
                    isControlsAdded = true;
                    if (reportCtrlList.Where(x => x.reportControlProperty.Name == ((Control)SelectedObject).Site.Name).Count() == 0)
                    {
                        reportCtrlProp = new ReportControlsStructure();
                        ivlFont = new IVLFont();
                        ((Control)SelectedObject).Font = new Font(new FontFamily(Common.CommonStaticMethods.GetDescription(CustomFontFamilies.Times_New_Roman)), 9);
                        
                        DisableAllControls();
                    }
                    else
                    {
                        if (reportCtrlProp != null)
                        {
                            if (reportCtrlProp.reportControlProperty.Name != ((Control)SelectedObject).Site.Name)
                                DisableAllControls();
                        }
                        else
                        {
                            DisableAllControls();
                        }
                        reportCtrlProp = reportCtrlList.Where(x => x.reportControlProperty.Name == ((Control)SelectedObject).Site.Name).First();

                        ivlFont = reportCtrlProp.reportControlProperty.Font;
                    }
                    prop.SelectedObject = SelectedObject;

                    reportCtrlProp.reportControlProperty.Name = ((Control)SelectedObject).Site.Name;
                    setPropertyText(((Control)SelectedObject).Site.Name);
                    GetPropertyInfo(reportCtrlProp.reportControlProperty);
                    reportCtrlProp.reportControlProperty.Size = new Size(Width, Height);
                    reportCtrlProp.reportControlProperty.Type = ((Control)SelectedObject).GetType().Name;

                    if (((Control)SelectedObject).GetType() == typeof(PictureBox))
                    {
                        if (reportCtrlList.Where(x => x.reportControlProperty.Name == ((Control)SelectedObject).Site.Name).Count() == 0)
                        {
                            PictureBox ctrl = (Control)SelectedObject as PictureBox;
                            ctrl.ImageLocation = logoDefaultValue;
                            if(File.Exists(logoDefaultValue))
                            reportCtrlProp.reportControlProperty.ImageName = logoDefaultValue;
                            else if (File.Exists(@"ImageResources\"+logoDefaultValue))
                                reportCtrlProp.reportControlProperty.ImageName =  @"ImageResources\" + logoDefaultValue;
                            ctrl.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                    }
                    if(((Control)SelectedObject).GetType()== typeof(Label))
                    {
                        textAlign_Cmbx.Enabled = true;
                    }
                    if (((Control)SelectedObject).GetType() == typeof(ReportUtils.IVL_ImagePanel))
                    {
                        
                        IVL_ImagePanel IVLt = (Control)SelectedObject as IVL_ImagePanel;
                        numberOfImages_nud.Enabled = true;
                        rowsCount = Convert.ToByte(IVLt.RowCount);
                        colsCount = Convert.ToByte(IVLt.ColumnCount);
                    }

                    if (((Control)SelectedObject).GetType() == typeof(TableLayoutPanel))
                    {

                        TableLayoutPanel IVLt = (Control)SelectedObject as TableLayoutPanel;
                        //if (IVLt.RowCount != Rows_nud.Value || IVLt.ColumnCount != Column_nud.Value)
                        //    IVLt.UpdateRowsCols((int)Rows_nud.Value, (int)Column_nud.Value);
                        rowsCount = Convert.ToByte(IVLt.RowCount);
                        colsCount = Convert.ToByte(IVLt.ColumnCount);

                        IVLt.RowStyles.Clear();
                        for (int i = 1; i <= IVLt.RowCount; i++)
                        {
                            IVLt.RowStyles.Add(new RowStyle(SizeType.Percent, (100/IVLt.RowCount)));
                        }
                        IVLt.ColumnCount = IVLt.ColumnCount;
                        IVLt.ColumnStyles.Clear();
                        for (int i = 1; i <= IVLt.ColumnCount; i++)
                        {
                            IVLt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                        }
                    }

                    fontUC.ReloadFontChanges(((Control)SelectedObject), reportCtrlProp.reportControlProperty);
                    AddReportControlPropertyList();
                    EnableControls();
                    showControlProperties();
                }
            }
            isControlsAdded = false;
        }


        void setPropertyText(string ctrlName)
        {
            int data = Convert.ToInt32(Regex.Match(ctrlName, @"\d+").Value);
            if (ctrlName.Contains("Label"))
            {
                controlDisplayText = textBoxValue + data;
            }
            else if (ctrlName.Contains("TextBox"))
            {
                controlDisplayText = editBoxValue + data;

            }
            else if (ctrlName.Contains("PictureBox"))
            {
                controlDisplayText = pictureBoxValue + data;

            }
            else if (ctrlName.Contains("IVL_Image"))
            {
                controlDisplayText = imageTableValue + data;

            }
            else if (ctrlName.Contains("TableLayout"))
            {
                controlDisplayText = tableLayoutValue + data;

            }
            Property_tbx.Text = controlDisplayText;
        }

        private void showControlProperties()
        {
            if (reportCtrlProp.reportControlProperty.Location._X > 0)
                LocationX_nud.Value = reportCtrlProp.reportControlProperty.Location._X;
            if (reportCtrlProp.reportControlProperty.Location._Y > 0)
                LocationY_nud.Value = reportCtrlProp.reportControlProperty.Location._Y;
            Width_nud.Value = reportCtrlProp.reportControlProperty.Size.Width;
            Height_nud.Value = reportCtrlProp.reportControlProperty.Size.Height;
            if (Convert.ToBoolean(reportCtrlProp.reportControlProperty.MultiLine))
            {
                Multilinetrue_radbtn.Checked = reportCtrlProp.reportControlProperty.MultiLine;
            }
            else
            {
                MultilineFalse_radbtn.Checked = true;
            }

            if (Convert.ToBoolean(reportCtrlProp.reportControlProperty.Border))
            {
                Bordertrue_radbtn.Checked = reportCtrlProp.reportControlProperty.Border;
            }
            else
            {
                BorderFalse_radbtn.Checked = true;
            }
            Binding_cbx.Text = "$"+reportCtrlProp.reportControlProperty.Binding.ToString();
            XMarginValue_nud.Value = reportCtrlProp.reportControlProperty.MarginDecrementValue;
            YPaddingValue_nud.Value = reportCtrlProp.reportControlProperty.YMarginDecrementValue;
            //Column_nud.Value = colsCount;
            int i = Text_tbx.SelectionStart;
            Text_tbx.Text = reportCtrlProp.reportControlProperty.Text;
            Text_tbx.SelectionStart = i;
            i = NameOfTheImage_tbx.SelectionStart;
            NameOfTheImage_tbx.Text = reportCtrlProp.reportControlProperty.ImageName;
            NameOfTheImage_tbx.SelectionStart = i;
            textAlign_Cmbx.SelectedItem = reportCtrlProp.reportControlProperty.TextAlign.ToString();
        }

        public void AddReportControlPropertyList()
        {
            foreach (ReportControlsStructure item in reportCtrlList)
            {
                if (item.reportControlProperty.Name == reportCtrlProp.reportControlProperty.Name)
                {
                    return;
                }
            }
            reportCtrlList.Add(reportCtrlProp);
        }

        public void UpdateReportControlPropertyList()
        {
            foreach (ReportControlsStructure item in reportCtrlList)
            {
                if (item.reportControlProperty.Name == reportCtrlProp.reportControlProperty.Name)
                    item.reportControlProperty = reportCtrlProp.reportControlProperty;
            }
        }

        public void DeleteReportControlPropertyItem(string nameOfControl)
        {
            int index = reportCtrlList.FindIndex(x => x.reportControlProperty.Name == nameOfControl);
            if (index >= 0)
                reportCtrlList.RemoveAt(index);
        }

        private string TranslateComponentToName(Component comp)
        {
            string sType = comp.GetType().ToString();
            if (string.IsNullOrEmpty(sType))
                return string.Empty;
            if (string.IsNullOrEmpty(comp.Site.Name))
                return string.Empty;
            sType = sType.Substring(sType.LastIndexOf(".") + 1);
            return String.Format("({0}) {1}", sType, comp.Site.Name);
        }

        public object SelectedObject
        {
            get { return prop.SelectedObject; }
            set
            {
                prop.SelectedObject = value;
                ReloadControlProperties();
            }
        }

        private void Binding_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void InitUpdatedValues()
        {
            switch (valueChanged)
            {
                case ValueChanged.Text:
                    {
                        reportCtrlProp.reportControlProperty.Text = Text_tbx.Text;
                        value = Text_tbx.Text;
                        break;
                    }
                case ValueChanged.LocationX:
                    {
                        reportCtrlProp.reportControlProperty.Location._X = Convert.ToInt16(LocationX_nud.Value);
                        value = new Point((int)(LocationX_nud.Value), (int)(LocationY_nud.Value));
                        valueChanged = ValueChanged.Location;
                        break;
                    }
                case ValueChanged.LocationY:
                    {
                        reportCtrlProp.reportControlProperty.Location._Y = Convert.ToInt16(LocationY_nud.Value);
                        value = new Point((int)(LocationX_nud.Value), (int)(LocationY_nud.Value));
                        valueChanged = ValueChanged.Location;
                        break;
                    }
                case ValueChanged.Width:
                    {
                        reportCtrlProp.reportControlProperty.Size = new Size(Convert.ToInt32(Width_nud.Value), Convert.ToInt32(Height_nud.Value));
                        value = (int)(Width_nud.Value);
                        break;
                    }
                case ValueChanged.Height:
                    {
                        reportCtrlProp.reportControlProperty.Size = new Size(Convert.ToInt32(Width_nud.Value), Convert.ToInt32(Height_nud.Value));
                        value = (int)(Height_nud.Value);
                        break;
                    }
                case ValueChanged.Font:
                    {
                        Font f = (Font)value;
                        ivlFont.FontFamily = f.FontFamily.ToString();
                        ivlFont.FontStyle = f.Style;
                        ivlFont.FontSize = f.Size;
                        reportCtrlProp.reportControlProperty.Font = ivlFont;
                        reportCtrlProp.reportControlProperty.Font.FontColor = colorValue.Name;
                        break;
                    }
                case ValueChanged.ForeColor:
                    {
                        Font f = (Font)value;
                        ivlFont.FontFamily = f.FontFamily.ToString();
                        ivlFont.FontStyle = f.Style;
                        ivlFont.FontSize = f.Size;
                        reportCtrlProp.reportControlProperty.Font = ivlFont;
                        reportCtrlProp.reportControlProperty.Font.FontColor = colorValue.Name;
                        break;
                    }
                case ValueChanged.BorderStyle:
                    {
                        reportCtrlProp.reportControlProperty.Border = ((Bordertrue_radbtn.Checked));
                        value = (bool)(Bordertrue_radbtn.Checked);
                        break;
                    }
                case ValueChanged.Multiline:
                    {
                        reportCtrlProp.reportControlProperty.MultiLine = ((Multilinetrue_radbtn.Checked));
                        value = (bool)(Multilinetrue_radbtn.Checked);
                        break;
                    }
                case ValueChanged.ImageLocation:
                    {
                        reportCtrlProp.reportControlProperty.ImageName = ((NameOfTheImage_tbx.Text));
                        value = (string)(NameOfTheImage_tbx.Text);
                        break;
                    }
                case ValueChanged.BindingType:
                    {
                        string enumValue = Binding_cbx.Text.TrimStart('$');
                        reportCtrlProp.reportControlProperty.Binding = enumValue;
                        if (Binding_cbx.Text != "$" + BindingType.None.ToString())
                        {
                            Text_tbx.Text = Binding_cbx.Text;
                           
                            value = (string)(Binding_cbx.Text);
                        }
                        else
                        {
                            Text_tbx.Text = reportCtrlProp.reportControlProperty.Name;
                            value = (string.Empty);
                        }
                        break;
                    }
                case ValueChanged.ImageCount:
                    {
                        
                          IVL_ImagePanel imagePanel =   SelectedObject as IVL_ImagePanel;
                          imagePanel.Images = (int) numberOfImages_nud.Value;
                          reportCtrlProp.reportControlProperty.NumberOfImages = imagePanel.Images;
                        break;
                    }
                case ValueChanged.XMargin:
                    {
                        //if (reportCtrlProp.reportControlProperty.Binding!=BindingType.None)
                        {
                            reportCtrlProp.reportControlProperty.MarginDecrementValue = Convert.ToInt32(XMarginValue_nud.Value);
                        }
                        break;
                    }
                case ValueChanged.YMargin:
                    {
                        //if (reportCtrlProp.reportControlProperty.Binding!=BindingType.None)
                        {
                            reportCtrlProp.reportControlProperty.YMarginDecrementValue = Convert.ToInt32(YPaddingValue_nud.Value);
                        }
                        break;
                    }
                case ValueChanged.TextAlign:
                    {
                        //if (reportCtrlProp.reportControlProperty.Binding!=BindingType.None)
                        {
                            value = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), textAlign_Cmbx.SelectedItem.ToString());
                            TextAlignVal = (TextAlign)Enum.Parse(typeof(TextAlign), textAlign_Cmbx.SelectedItem.ToString());
                            reportCtrlProp.reportControlProperty.TextAlign = TextAlignVal;// Convert.ToInt32(YPaddingValue_nud.Value);
                        }
                        break;
                    }
            }
            //if (!reportCtrlProp.reportControlProperty.Name.ToLower().Contains("form"))
                UpdateReportControlPropertyList();
            if (!isControlsAdded && valueChanged != ValueChanged.XMargin && valueChanged != ValueChanged.YMargin)
                SetPropertyInfo(value);
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            
            if (SelectedObject is Form)
            { }
            else if (SelectedObject is Control)
            {
                if (!isControlsAdded)
                {
                    if (sender.GetType() == typeof(RichTextBox))
                    {
                        valueChanged = ValueChanged.Text;
                    }
                    else if (sender.GetType() == typeof(TextBox))
                    {
                        if (((Control)sender).Name.Contains("Image"))
                            valueChanged = ValueChanged.ImageLocation;
                    }
                    else if (((Control)sender).Name.Contains("Width"))
                        valueChanged = ValueChanged.Width;
                    else if (((Control)sender).Name.Contains("Height"))
                        valueChanged = ValueChanged.Height;
                    else if (((Control)sender).Name.Contains("LocationX"))
                        valueChanged = ValueChanged.LocationX;
                    else if (((Control)sender).Name.Contains("LocationY"))
                        valueChanged = ValueChanged.LocationY;
                    else if (((Control)sender).Name.Contains("Rows"))
                        valueChanged = ValueChanged.RowCount;
                    else if (((Control)sender).Name.Contains("Column"))
                        valueChanged = ValueChanged.ColumnCount;
                    else if (((Control)sender).Name.Contains("Border"))
                        valueChanged = ValueChanged.BorderStyle;
                    else if (((Control)sender).Name.Contains("Binding"))
                        valueChanged = ValueChanged.BindingType;
                    else if (((Control)sender).Name.Contains("XMargin"))
                        valueChanged = ValueChanged.XMargin;
                    else if (((Control)sender).Name.Contains("YPadding"))
                        valueChanged = ValueChanged.YMargin;
                    else if (((Control)sender).Name.Contains("Multiline"))
                    {
                        if (SelectedObject.GetType() == typeof(TextBox))
                            valueChanged = ValueChanged.Multiline;
                        else
                            MultilineFalse_radbtn.Checked = true;
                    }
                    else if (((Control)sender).Name.Contains("Align"))
                    {
                        valueChanged = ValueChanged.TextAlign;
                    }
                    InitUpdatedValues();
                }
            }
        }

        private void GetPropertyInfo(ReportControlProperties repoConrtolProp)
        {
            PropertyInfo[] pinf = this.prop.SelectedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo item in pinf)
            {
                if (item.Name.Equals(ValueChanged.BorderStyle.ToString()))
                {
                    BorderStyle b = (BorderStyle)item.GetValue(SelectedObject);
                    if (b == BorderStyle.FixedSingle)
                        repoConrtolProp.Border = true;
                    else
                        repoConrtolProp.Border = false;
                }
                else
                    if (item.Name.Equals(ValueChanged.CellBorderStyle.ToString()))
                    {
                        DataGridViewAdvancedCellBorderStyle b = (DataGridViewAdvancedCellBorderStyle)item.GetValue(SelectedObject);
                        if (b == DataGridViewAdvancedCellBorderStyle.Single)
                            repoConrtolProp.Border = true;
                        else
                            repoConrtolProp.Border = false;
                    }
                    else
                        if (item.Name.Equals(ValueChanged.Multiline.ToString()))
                        {
                            repoConrtolProp.MultiLine = (bool)item.GetValue(SelectedObject);
                        }
                        else
                            if (item.Name.Equals(ValueChanged.Location.ToString()))
                            {
                                Point p = (Point)(item.GetValue(SelectedObject));
                                repoConrtolProp.Location._X = (short)p.X;
                                repoConrtolProp.Location._Y = (short)p.Y;
                            }
                            else
                                if (item.Name.Equals(ValueChanged.Text.ToString()))
                                {
                                    repoConrtolProp.Text = (string)(item.GetValue(SelectedObject));
                                }
                                else
                                    if (item.Name.Equals(ValueChanged.Height.ToString()))
                                    {
                                        Height = (int)item.GetValue(SelectedObject);
                                    }
                                    else
                                        if (item.Name.Equals(ValueChanged.Width.ToString()))
                                        {
                                            Width = (int)item.GetValue(SelectedObject);
                                        }
                                        else
                                            if (item.Name.Equals(ValueChanged.ImageCount.ToString()))
                                            {
                                                repoConrtolProp.NumberOfImages = Convert.ToInt32(item.GetValue(SelectedObject));
                                            }
                                                else
                                                    if (item.Name.Equals(ValueChanged.Tag.ToString()))
                                                    {
                                                        //repoConrtolProp.Binding = (BindingType)Enum.Parse(typeof(BindingType), item.GetValue(SelectedObject).ToString());
                                                    }
                                                    else if (item.Name.Equals(ValueChanged.TextAlign.ToString()))
                                                    {
                                                        string textAlignValue = item.GetValue(SelectedObject).ToString();
                                                        if(item.GetType() == typeof(Label))
                                                        repoConrtolProp.TextAlign= (TextAlign)Enum.Parse(typeof(TextAlign), textAlignValue);

                                                        //textAlign.HorizontalTextAlignment = split[1];
                                                        //textAlign.VerticalTextAlignment = split[0];
                                                    }
            }
        }

        public void DisableAllControls()
        {
            foreach (Control item in this.ControlPropertyGrid_tbpnl.Controls)
            {
                //item.Visible = false;
                item.Enabled = false;

            }
            textAlign_Cmbx.Enabled = false;
            ImageSideSetting_btn.Enabled = true;
        }

        private void EnableControls()
        {
            PropertyInfo[] pinf = this.SelectedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo item in pinf)
            {
                foreach (Control ctrl in this.ControlPropertyGrid_tbpnl.Controls)
                {
                   // if (ctrl.Name.Contains(item.Name))
                        ctrl.Enabled = true;
                }
            }
        }

        private void SetPropertyInfo(object value)
        {
            PropertyInfo[] pinf = this.prop.SelectedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo item in pinf)
            {
                object UpdateObjectValue = null;
                string ValueForObjectInString = "";
                if (Enum.IsDefined(typeof(ValueChanged), item.Name) && item.Name.Equals(valueChanged.ToString()))
                    switch (item.Name)
                    {
                        default:
                            {
                                UpdateObjectValue = value;
                                break;
                            }
                        case "BorderStyle":
                            {
                                bool isApplyBorder = Convert.ToBoolean(value);
                                if (isApplyBorder)
                                    UpdateObjectValue = BorderStyle.FixedSingle;
                                else
                                    UpdateObjectValue = BorderStyle.None;
                                break;
                            }

                        case "CellBorderStyle":
                            {
                                bool isApplyBorder = Convert.ToBoolean(value);
                                DataGridViewAdvancedCellBorderStyle borderStyleValue = DataGridViewAdvancedCellBorderStyle.Single;
                                if (isApplyBorder)
                                    borderStyleValue = DataGridViewAdvancedCellBorderStyle.Single;
                                else
                                    borderStyleValue = DataGridViewAdvancedCellBorderStyle.None;
                                ValueForObjectInString = borderStyleValue.ToString();
                                UpdateObjectValue = borderStyleValue;
                                break;
                            }

                        case "ForeColor":
                            {
                                UpdateObjectValue = colorValue;
                                break;
                            }
                        case "TextAlign":
                            {
                                ContentAlignment c = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), textAlign_Cmbx.SelectedItem.ToString());
                                UpdateObjectValue = c;
                                break;
                            }

                    }
                if (UpdateObjectValue != null)
                {
                    if (!string.IsNullOrEmpty(ValueForObjectInString))
                        item.SetValue(SelectedObject, Enum.Parse(UpdateObjectValue.GetType(), ValueForObjectInString));
                    else
                        item.SetValue(SelectedObject, UpdateObjectValue);
                    invokeUpdate();
                }
            }
        }

        private void LocationX_nud_MouseClick(object sender, MouseEventArgs e)
        {
            //InitUpdatedValues();
        }

        private void LocationX_nud_ValueChanged(object sender, EventArgs e)
        {
            if (LocationX_nud.Focus())
            {
                isControlsAdded = false;
                valueChanged = ValueChanged.LocationX;
                InitUpdatedValues();
            }
        }

        private void Width_nud_ValueChanged(object sender, EventArgs e)
        {
            if (Width_nud.Focus())
            {
                isControlsAdded = false;
                valueChanged = ValueChanged.Width;
                InitUpdatedValues();
            }
        }

        private void Text_tbx_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
                MenuItem menuItem = new MenuItem("Cut");
                menuItem.Click += new EventHandler(CutAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Copy");
                menuItem.Click += new EventHandler(CopyAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Paste");
                menuItem.Click += new EventHandler(PasteAction);
                contextMenu.MenuItems.Add(menuItem);
                Text_tbx.ContextMenu = contextMenu;
            }
        }

        void CutAction(object sender, EventArgs e)
        {
            Text_tbx.Cut();
        }

        void CopyAction(object sender, EventArgs e)
        {
            Clipboard.SetText(Text_tbx.SelectedText);
        }

        void PasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                Text_tbx.Text
                    += Clipboard.GetText(TextDataFormat.Text).ToString();
            }
        }

        private void ImagePathBrowse_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                NameOfTheImage_tbx.Text = ofd.FileName;
            else
                return;
        }

        public void UpdateYValue(bool isUp)
        {
            if (SelectedObject.GetType() != typeof(Form))
            {
                if (isUp)
                {
                    object selcet = SelectedObject;
                    int y = Convert.ToInt32(LocationY_nud.Value);
                    LocationY_nud.Value = --y;
                }
                else
                {
                    int y = Convert.ToInt32(LocationY_nud.Value);
                    LocationY_nud.Value = ++y;

                }
            }
        }

        public void UpdateXValue(bool isRight)
        {
            if (SelectedObject.GetType() != typeof(Form))
            {
                if (isRight)
                {
                    object selcet = SelectedObject;
                    int y = Convert.ToInt32(LocationX_nud.Value);
                    LocationX_nud.Value = ++y;
                }
                else
                {
                    int y = Convert.ToInt32(LocationX_nud.Value);
                    LocationX_nud.Value = --y;

                }
            }
        }

        private void numberOfImages_nud_ValueChanged(object sender, EventArgs e)
        {
                valueChanged = ValueChanged.ImageCount;
                InitUpdatedValues();
                invokeUpdate();
        }

        private void Text_tbx_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == (Keys.ControlKey | Keys.X))
            //{
            //    (sender as TextBox).Cut();
            //}
            //else if (e.KeyData == (Keys.ControlKey | Keys.V))
            //{
            //      (sender as TextBox).Paste();
            //}
        }
        private void Binding_cbx_TextChanged(object sender, EventArgs e)
        {
        }
        bool isTextChanged = false;
        
        private void Binding_cbx_Leave(object sender, EventArgs e)
        {
            if (isTextChanged)
            {
                bool isBindingExist = false;
                if (!string.IsNullOrEmpty(Binding_cbx.Text))// && 
                {
                    Binding_cbx.Text.TrimStart('$');
                    Binding_cbx.Text = "$" + Binding_cbx.Text;
                    for (int i = 0; i < Binding_cbx.Items.Count; i++)
                    {
                        if (Binding_cbx.Text == Binding_cbx.Items[i].ToString())
                        {
                            isBindingExist = true;
                            MessageBox.Show("Binding already exists");
                        }
                    }
                    if (!isBindingExist)
                    {
                        Binding_cbx.Text = Binding_cbx.Text.TrimStart('$');
                        bindingTypeAdd.Add("$" + Binding_cbx.Text);
                        OnValueChanged(Binding_cbx, e);
                    }
                    isTextChanged = false;
                }
            }
        }

        private void Binding_cbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            isTextChanged = true;
        }

        private void ImageSideSetting_btn_Click(object sender, EventArgs e)
        {
            imageNameSettingWindow.ImageMedicalName = reportCtrlProp.reportControlProperty.ImageMedicalName;
            if (imageNameSettingWindow.ShowDialog() == DialogResult.OK)
            {
                reportCtrlProp.reportControlProperty.ImageMedicalName = imageNameSettingWindow.ImageMedicalName;
            }

        }

    }

    
}

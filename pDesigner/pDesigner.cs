using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using DesignSurfaceExt;
using DesignSurfaceManagerExtension;
using System.Xml.Linq;
using System.Diagnostics;
using ReportUtils;

namespace pDesigner {
   
    //- [Note FROM MSDN]:
    //- The DesignSurfaceManager.ActiveDesignSurface property should be set
    //- by the designer's Type user interface
    //- whenever a designer becomes the active window!
    //- That is to say:
    //-   the DesignSurfaceManagerExt is an OBSERVER of UI event: SelectedTab/SelectedIndex Changed
    //- usage:
    //       //- SelectedIndexChanged event fires when the TabControls SelectedIndex or SelectedTab value changes.
    //       //- give the focus to the DesigneSurface accordingly to te selected TabPage and sync the propertyGrid
    //       this.tabControl1.SelectedIndexChanged += ( object sender, EventArgs e ) => {
    //            TabControl tabCtrl = sender as TabControl;
    //            DesignSurfaceManagerExtObject.ActiveDesignSurface = (DesignSurfaceExt2) DesignSurfaceManagerExtObject.DesignSurfaces[tabCtrl.SelectedIndex];
    //       };
    //-
    //- p(ico)Designer class
    public partial class pDesigner : UserControl , IpDesigner {
        private const string _Name_ = "pDesigner";
        IComponentChangeService componentChangeService;
        public Control rootComponent = null;

        public delegate void HRulerPixValue(string value);
            public event HRulerPixValue _HRulerPixelValue;

            public delegate void HRulerCmValue(string value);
            public event HRulerCmValue _HRulerCmValue;

            public delegate void VRulerPixValue(string value);
            public event VRulerPixValue _VRulerPixelValue;

            public delegate void VRulerCmValue(string value);
            public event VRulerCmValue _VRulerCmValue;

            //Panel view;
            Control view;


        //- the DesignSurfaceManagerExt instance must be an OBSERVER
        //- of the UI event which change the active DesignSurface
        //- DesignSurfaceManager is exposed as public getter properties as test facility
        public DesignSurfaceManagerExt DesignSurfaceManager { get; private set; }

        #region ctors
        //- usage:
        //-         if (a){
        //-             // do work
        //-         }//end_if
        //-         else{
        //-             // a is not valid
        //-         }//end_else
        public static implicit operator bool ( pDesigner d ) {
            bool isValid = true;
            //- the object 'd' must be correctly initialized
            //isValid &= ( ( null == d.Toolbox ) ? false : true );
            return isValid;
        }


        //- ctor
        public pDesigner() {
            InitializeComponent();

            DesignSurfaceManager = new DesignSurfaceManagerExt();
            DesignSurfaceManager.PropertyGridHost.Parent = this.splitterpDesigner.Panel2;

            //Toolbox = null;
            this.Dock = DockStyle.Fill;
                       
        }
        #endregion

        public void AddToReportControlList(ReportControlsStructure res)
        {
            DesignSurfaceManager.PropertyGridHost.reportCtrlList.Add(res);
        }
        private void tbCtrlpDesigner_SelectedIndexChanged ( object sender, EventArgs e ) {
            //TabControl tabCtrl = sender as TabControl;
            //int index = this.tbCtrlpDesigner.SelectedIndex;
            //if ( index >= 0 )
            //    DesignSurfaceManager.ActiveDesignSurface = ( DesignSurfaceExt2 ) DesignSurfaceManager.DesignSurfaces[index];
            //else {
            //    DesignSurfaceManager.ActiveDesignSurface = null;
            //    DesignSurfaceManager.PropertyGridHost.PropertyGrid.SelectedObject = null;
            //    DesignSurfaceManager.PropertyGridHost.ComboBox.Items.Clear();
           // }
        }

        #region IpDesigner Members

        //- to get and set the real Toolbox which is provided by the user
        public ToolBoxLib.ToolBox Toolbox { get; set; }

        //public TabControl TabControlHostingDesignSurfaces {
        //    get { return this.tbCtrlpDesigner; }
        //}

        public CustomPropertyGridHost PropertyGridHost {
            get { return DesignSurfaceManager.PropertyGridHost; }
        }

        public DesignSurfaceExt2 ActiveDesignSurface {
            get { return DesignSurfaceManager.ActiveDesignSurface as DesignSurfaceExt2; }
        }

        //- Create the DesignSurface and the rootComponent (a .NET Control)
        //- using IDesignSurfaceExt.CreateRootComponent() 
        //- if the alignmentMode doesn't use the GRID, then the gridSize param is ignored
        //- Note:
        //-     the generics param is used to know which type of control to use as RootComponent
        //-     TT is requested to be derived from .NET Control class 
        public DesignSurfaceExt2 AddDesignSurface<TT> (
                                                        int startingFormWidth, int startingFormHeight,
                                                        AlignmentModeEnum alignmentMode, Size gridSize
                                                       ) where TT : Control {
            const string _signature_ = _Name_ + @"::AddDesignSurface<>()";

            if( !this )
                throw new Exception( _signature_ + " - Exception: " + _Name_ + " is not initialized! Please set the Property: IpDesigner::Toolbox before calling any methods!" );
            //HRulerPixel.MouseTrackingOn = true;
            //HRulerCm.MouseTrackingOn = true;
            //VRulerCM.MouseTrackingOn = true;
            //VRulerPixel.MouseTrackingOn = true;

            //if (gridSize.Height > 700)
            //{
            //    VRulerPixel.Height = VRulerCM.Height = gridSize.Height;
                
            //}
            //else
            //{
            //    VRulerPixel.Height = VRulerCM.Height = panel1.Height;
            //}
            
            //- step.0
            //- create a DesignSurface
            DesignSurfaceExt2 surface = DesignSurfaceManager.CreateDesignSurfaceExt2();
            
            this.DesignSurfaceManager.ActiveDesignSurface = surface;


            //this.DesignSurfaceParent_pnl.Size = gridSize;

            //this.splitterpDesigner.Dock = DockStyle.None;
            //this.splitterpDesigner.Size = this.splitterpDesigner.Parent.Size;
           
            //-
            //-
            //- step.1
            //- choose an alignment mode...
            switch( alignmentMode ) {
                case AlignmentModeEnum.SnapLines:
                    surface.UseSnapLines();
                    break;
                case AlignmentModeEnum.Grid:
                    surface.UseGrid( gridSize );
                    break;
                case AlignmentModeEnum.GridWithoutSnapping:
                    surface.UseGridWithoutSnapping( gridSize );
                    break;
                case AlignmentModeEnum.NoGuides:
                    surface.UseNoGuides();
                    break;
                default:
                    surface.UseSnapLines();
                    break;
            }
            //end_switch
            //-
            //-
            //- step.2
            //- enable the UndoEngine
            ((IDesignSurfaceExt) surface).GetUndoEngineExt().Enabled = true;
            //-
            //-
            //- step.3
            //- Select the service IToolboxService
            //- and hook it to our ListBox
            ToolboxServiceImp tbox = ((IDesignSurfaceExt2) surface).GetIToolboxService() as ToolboxServiceImp;
            //- we don't check if Toolbox is null because the very first check: if(!this)...
            if( null != tbox )
                tbox.Toolbox = this.Toolbox;
            //-
            //-
            //- step.4
            //- create the Root compoment, in these cases a Form
            //- cast to .NET Control because the TT object 
            //- has a constraint: to be a ".NET Control"
            rootComponent = surface.CreateRootComponent( typeof( TT ), new Size( startingFormWidth, startingFormHeight ) ) as Control;
            //rootComponent.Location = new Point(32, 32);
            //- rename the Sited component
            //- (because the user may add more then one Form
            //- and every new Form will be called "Form1"
            //- if we don't set its Name)
            rootComponent.Site.Name = this.DesignSurfaceManager.GetValidFormName();
            //rootComponent.Location = new Point(32, 32);
            this.DesignSurfaceParent_pnl.Location = rootComponent.Location;
            //-
            //-
            //- step.5
            //- enable the Drag&Drop on RootComponent
            //((DesignSurfaceExt2) surface).EnableDragandDrop();
            //-
            //-
            //- step.6
            //- IComponentChangeService is marked as Non replaceable service
             componentChangeService = (IComponentChangeService) (surface.GetService( typeof( IComponentChangeService ) ));
            if( null != componentChangeService ) {
                //- the Type "ComponentEventHandler Delegate" Represents the method that will
                //- handle the ComponentAdding, ComponentAdded, ComponentRemoving, and ComponentRemoved
                //- events raised for component-level events
                componentChangeService.ComponentChanged += ( Object sender, ComponentChangedEventArgs e )=>
                {
                    // do nothing
                    DesignSurfaceManager.UpdatePropertyGridHost(surface);
                    
                };
                //System.ComponentModel.Design.DesignerActionUIStateChangeEventHandler
                componentChangeService.ComponentChanging += (Object sender, ComponentChangingEventArgs e) =>
                    {
                        DesignSurfaceManager.UpdatePropertyGridHost( surface );
                    };
                componentChangeService.ComponentAdded += ( Object sender, ComponentEventArgs e )=>
                {
                    DesignSurfaceManager.UpdatePropertyGridHost( surface );
                };
                //componentChangeService.ComponentRemoved += ( Object sender, ComponentEventArgs e )=>
                //{
                //    DesignSurfaceManager.UpdatePropertyGridHost( surface );
                //};
                componentChangeService.ComponentRemoved += (Object sender, ComponentEventArgs e) =>
                {
                    DesignSurfaceManager.UpdatePropertyGridHost(surface,e);
                };
            }
            //DesignSurfaceManager.PropertyGridHost.SelectedObject = rootComponent;
            //-
            //-
            //- step.7
            //- now set the Form::Text Property
            //- (because it will be an empty string
            //- if we don't set it)
             view = surface.GetView();
            
           
            if( null == view )
                return null;
          // view = surface.GetView(ref DesignSurfaceParent_pnl);
           DesignSurfaceParent_pnl.MouseClick += DesignSurfaceParent_pnl_MouseClick;
            DesignSurfaceParent_pnl.MouseWheel+=DesignSurfaceParent_pnl_MouseWheel;
         //   view = viewObject as Control;
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties( view );
            
            //- Sets a PropertyDescriptor to the specific property
            PropertyDescriptor pdS = pdc.Find( "Text", false );
            if( null != pdS )
                pdS.SetValue( rootComponent, rootComponent.Site.Name + " (design mode)" );
            //-
            //-
            //- step.8
            //- display the DesignSurface
            string sTabPageText = rootComponent.Site.Name;
           // TabPage newPage = new TabPage( sTabPageText );
           // newPage.Name = sTabPageText;
            splitterpDesigner.Panel1.SuspendLayout();
            DesignSurfaceParent_pnl.SuspendLayout();
            DesignSurfaceParent_pnl.MouseClick+=DesignSurfaceParent_pnl_MouseClick;
           // newPage.SuspendLayout(); //----------------------------------------------------
            //view.Dock = DockStyle.Left;
            //view.BackColor = Color.Red;
           view.Parent = DesignSurfaceParent_pnl; //- Note this assignment
           //view.BackColor = Color.Red;
            //view.Parent = splitterpDesigner.Panel1;
           // DesignSurfaceParent_pnl.Controls.Add(view);
            splitterpDesigner.Panel1.ResumeLayout();
            DesignSurfaceParent_pnl.ResumeLayout();
            //DesignSurfaceParent_pnl.Visible = true;
            //DesignSurfaceParent_pnl.MouseWheel += DesignSurfaceParent_pnl_MouseWheel;
            //DesignSurfaceParent_pnl.HorizontalScroll.Maximum = 0;
            //DesignSurfaceParent_pnl.AutoScroll = false;
            //DesignSurfaceParent_pnl.VerticalScroll.Visible = false;
            //DesignSurfaceParent_pnl.AutoScroll = true;
            DesignSurfaceParent_pnl.BringToFront();
            DesignSurfaceParent_pnl.Focus();
           // DesignSurfaceParent_pnl.Parent = splitterpDesigner.Panel1;
            //this.tbCtrlpDesigner.TabPages.Add( newPage );
           // newPage.ResumeLayout(); //-----------------------------------------------------
            //splitterpDesigner.Panel1.ResumeLayout();
            //splitterpDesigner.Panel1.Height = gridSize.Height;
            //- select the TabPage created
            //this.tbCtrlpDesigner.SelectedIndex = this.tbCtrlpDesigner.TabPages.Count - 1;
            //-
            //-
            //- step.9
            //- finally return the DesignSurface created to let it be modified again by user
            return surface;
        }

        void DesignSurfaceParent_pnl_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("click");

        }

        void DesignSurfaceParent_pnl_MouseWheel(object sender, MouseEventArgs e)
        {
            Console.WriteLine("scroll");

        }

        public void ChangeYLocationOfControls(bool isUp)
        {
            DesignSurfaceManager.UpdateYLocationPropertyGridHost(isUp);
        }

        public void ChangeXLocationOfControls(bool isRight)
        {
            DesignSurfaceManager.UpdateXLocationPropertyGridHost(isRight);
        }

        //public void ParseComponentChanged()
        ////{
        ////    if (null != componentChangeService)
        ////    {
        ////        //- the Type "ComponentEventHandler Delegate" Represents the method that will
        ////        //- handle the ComponentAdding, ComponentAdded, ComponentRemoving, and ComponentRemoved
        ////        //- events raised for component-level events
        ////        componentChangeService.ComponentChanged += (Object sender, ComponentChangedEventArgs e) =>
        ////        {
        ////            // do nothing
        ////            DesignSurfaceManager.UpdatePropertyGridHost(this.DesignSurfaceManager.ActiveDesignSurface);
        ////        };
        ////        //System.ComponentModel.Design.DesignerActionUIStateChangeEventHandler
        ////        componentChangeService.ComponentChanging += (Object sender, ComponentChangingEventArgs e) =>
        ////            {
        ////                DesignSurfaceManager.UpdatePropertyGridHost(this.DesignSurfaceManager.ActiveDesignSurface);
        ////            };
        ////        componentChangeService.ComponentAdded += (Object sender, ComponentEventArgs e) =>
        ////        {
        ////            DesignSurfaceManager.UpdatePropertyGridHost(this.DesignSurfaceManager.ActiveDesignSurface);
        ////        };
        ////        componentChangeService.ComponentRemoved += (Object sender, ComponentEventArgs e) =>
        ////        {
        ////            DesignSurfaceManager.UpdatePropertyGridHost(this.DesignSurfaceManager.ActiveDesignSurface);
        ////        };
        ////    }
        //}

        void p_MouseUp(object sender, MouseEventArgs e)
        {
            DesignSurfaceManager.UpdatePropertyGridHost(DesignSurfaceManager.ActiveDesignSurface);
        }

        public List<ReportControlsStructure> GetControlProperties()
        {
            return PropertyGridHost.reportCtrlList;
        }

        public void RefreshControlList()
        {
            PropertyGridHost.reportCtrlList = new List<ReportControlsStructure>();
        }

        public void RemoveDesignSurface( DesignSurfaceExt2 surfaceToErase ) {
            try {
                //- remove the TabPage which has the same name of
                //- the RootComponent host by DesignSurface "surfaceToErase"
                //- Note:
                //-     DesignSurfaceManager continues to reference the DesignSurface erased
                //-     that Designsurface continue to exist but it is no more reachable
                //-     this fact is usefull when generate new names for Designsurfaces just created
                //-     avoiding name clashing
                string dsRootComponentName = surfaceToErase.GetIDesignerHost().RootComponent.Site.Name;
               // TabPage tpToRemove = null;
                //foreach ( TabPage tp in this.tbCtrlpDesigner.TabPages ) {
                //    if ( tp.Name == dsRootComponentName ) {
                //        tpToRemove = tp;
                //        break;
                //    }//end_if
                //}//end_foreach
                //if ( null != tpToRemove )
                //    this.tbCtrlpDesigner.TabPages.Remove ( tpToRemove );
                ////- now remove the DesignSurface
                this.DesignSurfaceManager.DeleteDesignSurfaceExt2(surfaceToErase);
                ////- finally the DesignSurfaceManager remove the DesignSurface
                ////- AND set as active DesignSurface the last one
                ////- therefore we set as active the last TabPage
                //this.tbCtrlpDesigner.SelectedIndex = this.tbCtrlpDesigner.TabPages.Count - 1;
            }//end_try
            catch( Exception exx ) {
                Debug.WriteLine( exx.Message );
                if( null != exx.InnerException )
                    Debug.WriteLine( exx.InnerException.Message );
                throw;
            }//end_catch
        }

        public void UndoOnDesignSurface() {
            IDesignSurfaceExt2 isurf = DesignSurfaceManager.ActiveDesignSurface;
            if ( null != isurf )
                isurf.GetUndoEngineExt().Undo();
        }

        public void RedoOnDesignSurface() {
            IDesignSurfaceExt2 isurf = DesignSurfaceManager.ActiveDesignSurface;
            if ( null != isurf )
                isurf.GetUndoEngineExt().Redo();
        }

        public void CutOnDesignSurface() {
            IDesignSurfaceExt isurf = DesignSurfaceManager.ActiveDesignSurface;
            if ( null != isurf )
                isurf.DoAction ( "Cut" );
        }

        public void CopyOnDesignSurface() {
            IDesignSurfaceExt isurf = DesignSurfaceManager.ActiveDesignSurface;
            if ( null != isurf )
                isurf.DoAction ( "Copy" );
        }

        public void PasteOnDesignSurface() {
            IDesignSurfaceExt isurf = DesignSurfaceManager.ActiveDesignSurface;
            if ( null != isurf )
                isurf.DoAction ( "Paste" );
        }

        public void DeleteOnDesignSurface() {
            IDesignSurfaceExt isurf = DesignSurfaceManager.ActiveDesignSurface;
            if ( null != isurf )
                isurf.DoAction ( "Delete" );
        }

        public void SwitchTabOrder() {
            IDesignSurfaceExt isurf = DesignSurfaceManager.ActiveDesignSurface;
            if ( null != isurf )
                isurf.SwitchTabOrder();
        }
        
        #endregion

        private void HRulerPixel_HooverValue(object sender, Lyquidity.UtilityLibrary.Controls.RulerControl.HooverValueEventArgs e)
        {
             //e.Value.ToString("0.0");

            //_HRulerPixelValue(this.HRulerPixel.MouseLocation.ToString());
        }

        private void VRulerCM_HooverValue(object sender, Lyquidity.UtilityLibrary.Controls.RulerControl.HooverValueEventArgs e)
        {
            _VRulerCmValue(e.Value.ToString("0.0"));
        }

        private void HRulerCm_HooverValue(object sender, Lyquidity.UtilityLibrary.Controls.RulerControl.HooverValueEventArgs e)
        {
            _HRulerCmValue(e.Value.ToString("0.0"));
        }

        private void VRulerPixel_HooverValue(object sender, Lyquidity.UtilityLibrary.Controls.RulerControl.HooverValueEventArgs e)
        {
            //_VRulerPixelValue(this.VRulerPixel.MouseLocation.ToString());
        }

        private void DesignSurfaceParent_pnl_Paint(object sender, PaintEventArgs e)
        {
            Console.WriteLine(e.ToString());
            DesignSurfaceParent_pnl.Focus();
        }

        private void DesignSurfaceParent_pnl_Scroll_1(object sender, ScrollEventArgs e)
        {
           // Console.WriteLine("scroll");

        }

        private void splitterpDesigner_Panel1_Scroll(object sender, ScrollEventArgs e)
        {
            Console.WriteLine("scroll");

        }

    }//end_class
}//end_namespace

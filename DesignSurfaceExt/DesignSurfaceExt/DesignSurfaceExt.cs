using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.Drawing;
using ReportUtils;
using System.ComponentModel;
using System.Windows.Forms;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;

namespace DesignSurfaceExt {

//- this class adds to a .NET 
//-     DesignSurface instance
//- the following facilities:
//-     * TabOrder
//-     * UndoEngine
//-     * Cut/Copy/Paste/Delete commands
//-
//- DesignSurfaceExt
//-     |
//-     +--|DesignSurface|
//-     |
//-     +--|TabOrder|
//-     |
//-     +--|UndoEngine|
//-     |
//-     +--|Cut/Copy/Paste/Delete commands|
//-
public class DesignSurfaceExt : DesignSurface, IDesignSurfaceExt {
    private const string _Name_ = "DesignSurfaceExt";

    

    #region IDesignSurfaceExt Members

    public void SwitchTabOrder() {
        if ( false == IsTabOrderMode ) {
            InvokeTabOrder();
        }
        else {
            DisposeTabOrder();
        }
    }

    public void UseSnapLines() {
        IServiceContainer serviceProvider = this.GetService ( typeof ( IServiceContainer ) ) as IServiceContainer;
        DesignerOptionService opsService = serviceProvider.GetService ( typeof ( DesignerOptionService ) ) as DesignerOptionService;
        if ( null != opsService ) {
            serviceProvider.RemoveService ( typeof ( DesignerOptionService ) );
        }
        DesignerOptionService opsService2 = new DesignerOptionServiceExt4SnapLines();
        serviceProvider.AddService ( typeof ( DesignerOptionService ), opsService2 );
    }

    public void UseGrid ( Size gridSize ) {
        IServiceContainer serviceProvider = this.GetService ( typeof ( IServiceContainer ) ) as IServiceContainer;
        DesignerOptionService opsService = serviceProvider.GetService ( typeof ( DesignerOptionService ) ) as DesignerOptionService;
        if ( null != opsService ) {
            serviceProvider.RemoveService ( typeof ( DesignerOptionService ) );
        }
        DesignerOptionService opsService2 = new DesignerOptionServiceExt4Grid ( gridSize );
        serviceProvider.AddService ( typeof ( DesignerOptionService ), opsService2 );
    }

    public void UseGridWithoutSnapping ( Size gridSize ) {
        IServiceContainer serviceProvider = this.GetService ( typeof ( IServiceContainer ) ) as IServiceContainer;
        DesignerOptionService opsService = serviceProvider.GetService ( typeof ( DesignerOptionService ) ) as DesignerOptionService;
        if ( null != opsService ) {
            serviceProvider.RemoveService ( typeof ( DesignerOptionService ) );
        }
        DesignerOptionService opsService2 = new DesignerOptionServiceExt4GridWithoutSnapping ( gridSize );
        serviceProvider.AddService ( typeof ( DesignerOptionService ), opsService2 );
    }

    public void UseNoGuides() {
        IServiceContainer serviceProvider = this.GetService ( typeof ( IServiceContainer ) ) as IServiceContainer;
        DesignerOptionService opsService = serviceProvider.GetService ( typeof ( DesignerOptionService ) ) as DesignerOptionService;
        if ( null != opsService ) {
            serviceProvider.RemoveService ( typeof ( DesignerOptionService ) );
        }
        DesignerOptionService opsService2 = new DesignerOptionServiceExt4NoGuides();
        serviceProvider.AddService ( typeof ( DesignerOptionService ), opsService2 );
    }

    public UndoEngineExt GetUndoEngineExt() {
        return this._undoEngine;
    }

    private IComponent CreateRootComponentCore ( Type controlType, Size controlSize, DesignerLoader loader ) {
        const string _signature_ = _Name_ + @"::CreateRootComponentCore()";
        try {
            //- step.1
            //- get the IDesignerHost
            //- if we are not not able to get it 
            //- then rollback (return without do nothing)
            IDesignerHost host = GetIDesignerHost();
            if ( null == host ) return null;
            //- check if the root component has already been set
            //- if so then rollback (return without do nothing)
            if( null != host.RootComponent ) return null;
            //-
            //-
            //- step.2
            //- create a new root component and initialize it via its designer
            //- if the component has not a designer
            //- then rollback (return without do nothing)
            //- else do the initialization
            if( null != loader ) {
                this.BeginLoad( loader );
                if( this.LoadErrors.Count > 0 )
                    throw new Exception( _signature_ + " - Exception: the BeginLoad(loader) failed!" );
            }
            else {
                this.BeginLoad( controlType );
                if( this.LoadErrors.Count > 0 )
                    throw new Exception( _signature_ + " - Exception: the BeginLoad(Type) failed! Some error during " + controlType.ToString() + " loading" );
            }
            //-
            //-
            //- step.3
            //- try to modify the Size of the object just created
            IDesignerHost ihost = GetIDesignerHost();
            //- Set the backcolor and the Size
            Control ctrl = null;
            if( host.RootComponent is  Form ) {
                ctrl = this.View as Control;
                ctrl.BackColor = Color.LightGray;
                //- set the Size
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties( ctrl );
                //- Sets a PropertyDescriptor to the specific property
                PropertyDescriptor pdS = pdc.Find( "Size", false );
                if (null != pdS)
                {
                    object size = pdS.GetValue(host.RootComponent);
                    pdS.SetValue(ihost.RootComponent, controlSize);
                }
            }
            else if( host.RootComponent is UserControl ) {
                ctrl = this.View as Control;
                ctrl.BackColor = Color.Gray;
                //- set the Size
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties( ctrl );
                //- Sets a PropertyDescriptor to the specific property
                PropertyDescriptor pdS = pdc.Find( "Size", false );
                if( null != pdS )
                    pdS.SetValue( ihost.RootComponent, controlSize );
            }
            else if(  host.RootComponent is  Control ) {
                ctrl = this.View as Control;
                ctrl.BackColor = Color.LightGray;
                //- set the Size
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties( ctrl );
                //- Sets a PropertyDescriptor to the specific property
                PropertyDescriptor pdS = pdc.Find( "Size", false );

                if (null != pdS)
                {
                    object size = pdS.GetValue(host.RootComponent);
                    pdS.SetValue(ihost.RootComponent, controlSize);
                }
            }
            else if(  host.RootComponent is  Component ) {
                ctrl = this.View as Control;
                ctrl.BackColor = Color.White;
                //- don't set the Size
            }
            else {
                //- Undefined Host Type
                ctrl = this.View as Control;
                ctrl.BackColor = Color.Red;
            }

            return ihost.RootComponent;
        }//end_try
        catch( Exception exx ) {
            Debug.WriteLine( exx.Message );
            if( null != exx.InnerException )
                Debug.WriteLine( exx.InnerException.Message );
            
            throw;
        }//end_catch
    }

    public IComponent CreateRootComponent( Type controlType, Size controlSize ) {
        return CreateRootComponentCore( controlType, controlSize, null );
    }

    public IComponent CreateRootComponent( DesignerLoader loader, Size controlSize ) {
        return CreateRootComponentCore( null, controlSize, loader );
    }
    
    public Control CreateControl ( Type controlType, Size controlSize, Point controlLocation ) {
        try {
            //- step.1
            //- get the IDesignerHost
            //- if we are not able to get it 
            //- then rollback (return without do nothing)
            IDesignerHost host = GetIDesignerHost();
            if ( null == host ) return null;
            //- check if the root component has already been set
            //- if not so then rollback (return without do nothing)
            if( null == host.RootComponent ) return null;
            //-
            //-
            //- step.2
            //- create a new component and initialize it via its designer
            //- if the component has not a designer
            //- then rollback (return without do nothing)
            //- else do the initialization
            IComponent newComp = host.CreateComponent ( controlType );
            if ( null == newComp ) return null;
            IDesigner designer = host.GetDesigner ( newComp );
            if ( null == designer ) return null;
            if ( designer is IComponentInitializer )
                ( ( IComponentInitializer ) designer ).InitializeNewComponent ( null );
            //-
            //-
            //- step.3
            //- try to modify the Size/Location of the object just created
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties ( newComp );
            //- Sets a PropertyDescriptor to the specific property.
            PropertyDescriptor pdS = pdc.Find ( "Size", false );
            if ( null != pdS )
                pdS.SetValue ( newComp, controlSize );
            PropertyDescriptor pdL = pdc.Find ( "Location", false );
            if ( null != pdL )
                pdL.SetValue ( newComp, controlLocation );
            //-
            //-
            //- step.4
            //- commit the Creation Operation
            //- adding the control to the DesignSurface's root component
            //- and return the control just created to let further initializations
            ( ( Control ) newComp ).Parent = host.RootComponent as Control;
            return newComp as Control;
        }//end_try
        catch( Exception exx ) {
            Debug.WriteLine( exx.Message );
            if( null != exx.InnerException )
                Debug.WriteLine( exx.InnerException.Message );
            
            throw;
        }//end_catch
    }


    public Control CreateControl(ReportUtils.ReportControlProperties reportControlProperties)
    {
        try
        {
            //- step.1
            //- get the IDesignerHost
            //- if we are not able to get it 
            //- then rollback (return without do nothing)
            IDesignerHost host = GetIDesignerHost();
            if (null == host) return null;
            //- check if the root component has already been set
            //- if not so then rollback (return without do nothing)
            if (null == host.RootComponent) return null;
            //-
            //-
            //- step.2
            //- create a new component and initialize it via its designer
            //- if the component has not a designer
            //- then rollback (return without do nothing)
            //- else do the initialization
            //IComponent newComp = host.CreateComponent(typeof(ReportUtils.Type));
            //IComponent newComp = host.CreateComponent(Type.GetType(reportControlProperties.Type));

            IComponent newComp = host.CreateComponent(GetTypeFromSimpleName(reportControlProperties.Type));

            Type type = GetTypeFromSimpleName(reportControlProperties.Type);

            if (null == newComp) return null;
            IDesigner designer = host.GetDesigner(newComp);
            if (null == designer) return null;
            if (designer is IComponentInitializer)
                ((IComponentInitializer)designer).InitializeNewComponent(null);
            //-
            //-
            //- step.3
            //- try to modify the Size/Location of the object just created
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(newComp);

            //- Sets a PropertyDescriptor to the specific property.
            PropertyDescriptor pdS = pdc.Find("Size", false);
            if (null != pdS)
                pdS.SetValue(newComp, (reportControlProperties.Size));

            PropertyDescriptor pdName = pdc.Find("Name", false);
            if (null != pdName)
                pdName.SetValue(newComp, (reportControlProperties.Name));

            PropertyDescriptor pdL = pdc.Find("Location", false);
            if (null != pdL)
            {
                pdL.SetValue(newComp, new Point(reportControlProperties.Location._X,reportControlProperties.Location._Y));
            }
            PropertyDescriptor pdF = pdc.Find("Font", false);
            if (null != pdF)
            {
                Font font = new Font(reportControlProperties.Font.FontFamily, reportControlProperties.Font.FontSize, reportControlProperties.Font.FontStyle);
                pdF.SetValue(newComp, font);
            }
            PropertyDescriptor pdFc = pdc.Find("ForeColor", false);
            if (null != pdFc)
            {
                Color color = Color.FromName(reportControlProperties.Font.FontColor);
                if (!color.IsKnownColor)
                {
                    color = ColorTranslator.FromHtml("#" + color.Name);
                }
                pdFc.SetValue(newComp, color);
            }

            PropertyDescriptor pdFBorder = pdc.Find("BorderStyle", false);
            if (null != pdFBorder)
            {
                if(reportControlProperties.Border)
                pdFBorder.SetValue(newComp, BorderStyle.FixedSingle);
                else
                    pdFBorder.SetValue(newComp, BorderStyle.None);
            }

            PropertyDescriptor pdText = pdc.Find("Text", false);
            if (null != pdText)
            {
                pdText.SetValue(newComp, reportControlProperties.Text);
            }

            if (type == typeof(PictureBox))
            {
                PropertyDescriptor pdImageLocation = pdc.Find("ImageLocation", false);
                if (null != pdImageLocation)
                {
                    pdImageLocation.SetValue(newComp, reportControlProperties.ImageName);
                }
                pdImageLocation = pdc.Find("SizeMode", false);
                if (null != pdImageLocation)
                {
                    pdImageLocation.SetValue(newComp, PictureBoxSizeMode.Zoom);
                }
            }
            
            PropertyDescriptor pdMultiLine = pdc.Find("Multiline", false);
            if (null != pdMultiLine)
            {
                pdMultiLine.SetValue(newComp, reportControlProperties.MultiLine);
            }
            PropertyDescriptor pdTextAlign = pdc.Find("TextAlign", false);
            if (null != pdTextAlign)
            {
                if (type == typeof(Label))
                {
                    ContentAlignment c = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), reportControlProperties.TextAlign.ToString());
                    pdTextAlign.SetValue(newComp, c);
                }
            }

            PropertyDescriptor pdTag = pdc.Find("Tag", false);
            if (null != pdTag)
            {
                pdTag.SetValue(newComp, reportControlProperties.Binding);
            }

            if (type == typeof(TableLayoutPanel))
            {
                PropertyDescriptor pdTableLayout = pdc.Find("RowCount", false);
                if (null != pdTableLayout)
                {
                    pdTableLayout.SetValue(newComp, reportControlProperties.RowsColumns._Rows);
                }
                pdTableLayout = pdc.Find("ColumnCount", false);
                if (null != pdTableLayout)
                {
                    pdTableLayout.SetValue(newComp, reportControlProperties.RowsColumns._Columns);
                }
                pdTableLayout = pdc.Find("CellBorderStyle", false);
                if (null != pdTableLayout)
                {
                    if(reportControlProperties.Border)
                    pdTableLayout.SetValue(newComp, DataGridViewAdvancedCellBorderStyle.Single);
                    else
                        pdTableLayout.SetValue(newComp, DataGridViewAdvancedCellBorderStyle.None);
                }
            }
            if (type == typeof(IVL_ImagePanel))
            {
                PropertyDescriptor pdTableLayout = pdc.Find("Images", false);
                if (null != pdTableLayout)
                {
                    pdTableLayout.SetValue(newComp, reportControlProperties.NumberOfImages);
                }
                pdTableLayout = pdc.Find("BorderStyle", false);
                if (null != pdTableLayout)
                {
                    pdTableLayout.SetValue(newComp, BorderStyle.FixedSingle);
                }
            }
            //-
            //-
            //- step.4
            //- commit the Creation Operation
            //- adding the control to the DesignSurface's root component
            //- and return the control just created to let further initializations
            ((Control)newComp).Parent = host.RootComponent as Control;
            return newComp as Control;
        }//end_try
        catch (Exception exx)
        {
            Debug.WriteLine(exx.Message);
            if (null != exx.InnerException)
                Debug.WriteLine(exx.InnerException.Message);

            throw;
        }//end_catch
    }


   public static Type GetTypeFromSimpleName(string typeName)
  {
    if (typeName == null)
      throw new ArgumentNullException("typeName");

    bool isArray = false, isNullable = false;

    if (typeName.IndexOf("[]") != -1)
    {
      isArray = true;
      typeName = typeName.Remove(typeName.IndexOf("[]"), 2);
    }

    if (typeName.IndexOf("?") != -1)
    {
      isNullable = true;
      typeName = typeName.Remove(typeName.IndexOf("?"), 1);
    }

    //typeName = typeName.ToLower();
        
    string parsedTypeName = null;
    switch (typeName)
    {
      case "Label": parsedTypeName = "System.Windows.Forms.Label";
        break;
      case "TextBox":parsedTypeName = "System.Windows.Forms.TextBox";
        break;
       
      case "TableLayoutPanel":
        parsedTypeName = "System.Windows.Forms.TableLayoutPanel";
        break;
      case "PictureBox":
        parsedTypeName = "System.Windows.Forms.PictureBox";
        break;
      case "IVL_ImageTable":
        parsedTypeName = "ReportUtils.IVL_ImageTable";
        break;
      case "IVL_ImagePanel":
        parsedTypeName = "ReportUtils.IVL_ImagePanel";
        break;
    }

    if (parsedTypeName != null)
    {
      if (isArray)
        parsedTypeName = parsedTypeName + "[]";

      if (isNullable)
        parsedTypeName = String.Concat("System.Nullable`1[", parsedTypeName, "]");
    }
    else
      parsedTypeName = typeName;
    // Expected to throw an exception in case the type has not been recognized.
    return GetType(parsedTypeName);
  }

    public static Type GetType(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type != null) return type;
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = a.GetType(typeName);
            if (type != null)
                return type;
        }
        return null;
    }


    public IDesignerHost GetIDesignerHost() {
        IDesignerHost temp = (IDesignerHost)(this.GetService(typeof(IDesignerHost)));
        return temp;
    }

    public Control GetView() {
        Type t = this.View.GetType();
        Control ctrl = this.View as Control;
        //Panel pnl = ctrl as Panel;
        if( null == ctrl )
            return null;
        ctrl.Dock = DockStyle.Fill;
    
        return ctrl;
    }
    //public Control GetView(ref Panel parent)
    //{
    //    Type t = this.View.GetType();
    //    Control ctrl = this.View as Control;
    //    //Panel pnl = ctrl as Panel;
    //    if (null == ctrl)
    //        return null;
    //    ctrl.MouseEnter += ctrl_MouseEnter; 
    //        // ctrl.Dock = DockStyle.Fill;
    //    ctrl.Parent = parent;
    //   // parent.BringToFront();
    //    return ctrl;
    //}

    void ctrl_MouseEnter(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }



    public object GetViewObject()
    {
       // Desi ctrl = this.View.GetType(); ;

        //Panel pnl = ctrl as Panel;
        if (null == this.View)
            return null;
       // ctrl.Dock = DockStyle.Fill;
        return this.View;
    }

    #endregion

    #region TabOrder
    
    private TabOrderHooker _tabOrder = null;
    private bool _tabOrderMode = false;
    
    public bool IsTabOrderMode {
        get { return _tabOrderMode;  }
    }

    public TabOrderHooker TabOrder {
        get {
            if ( _tabOrder == null )
                _tabOrder = new TabOrderHooker();
            return _tabOrder;
        }
        set {  _tabOrder = value;  }
    }

    public void InvokeTabOrder() {
        TabOrder.HookTabOrder ( this.GetIDesignerHost() );
        _tabOrderMode = true;
    }

    public void DisposeTabOrder() {
        TabOrder.HookTabOrder ( this.GetIDesignerHost() );
        _tabOrderMode = false;
    }
    #endregion

    #region  UndoEngine

    private UndoEngineExt _undoEngine = null;
    private NameCreationServiceImp _nameCreationService = null;
    private DesignerSerializationServiceImpl _designerSerializationService = null;
    private CodeDomComponentSerializationService _codeDomComponentSerializationService = null;

    #endregion

    #region ctors

    //- ctors
    //- Summary:
    //-     Initializes a new instance of the System.ComponentModel.Design.DesignSurface
    //-     class.
    //-
    //- Exceptions:
    //-   System.ObjectDisposedException:
    //-     The System.ComponentModel.Design.IDesignerHost attached to the System.ComponentModel.Design.DesignSurface
    //-     has been disposed.
    public DesignSurfaceExt() : base() { InitServices(); }
    //-
    //- Summary:
    //-     Initializes a new instance of the System.ComponentModel.Design.DesignSurface
    //-     class.
    //-
    //- Parameters:
    //-   parentProvider:
    //-     The parent service provider, or null if there is no parent used to resolve
    //-     services.
    //-
    //- Exceptions:
    //-   System.ObjectDisposedException:
    //-     The System.ComponentModel.Design.IDesignerHost attached to the System.ComponentModel.Design.DesignSurface
    //-     has been disposed.
    public DesignSurfaceExt( IServiceProvider parentProvider ) : base( parentProvider ) { InitServices(); }
    //-
    //- Summary:
    //-     Initializes a new instance of the System.ComponentModel.Design.DesignSurface
    //-     class.
    //-
    //- Parameters:
    //-   rootComponentType:
    //-     The type of root component to create.
    //-
    //- Exceptions:
    //-   System.ArgumentNullException:
    //-     rootComponent is null.
    //-
    //-   System.ObjectDisposedException:
    //-     The System.ComponentModel.Design.IDesignerHost attached to the System.ComponentModel.Design.DesignSurface
    //-     has been disposed.
    public DesignSurfaceExt( Type rootComponentType ) : base( rootComponentType ) { InitServices(); }
    //-
    //- Summary:
    //-     Initializes a new instance of the System.ComponentModel.Design.DesignSurface
    //-     class.
    //-
    //- Parameters:
    //-   parentProvider:
    //-     The parent service provider, or null if there is no parent used to resolve
    //-     services.
    //-
    //-   rootComponentType:
    //-     The type of root component to create.
    //-
    //- Exceptions:
    //-   System.ArgumentNullException:
    //-     rootComponent is null.
    //-
    //-   System.ObjectDisposedException:
    //-     The System.ComponentModel.Design.IDesignerHost attached to the System.ComponentModel.Design.DesignSurface
    //-     has been disposed.
    public DesignSurfaceExt( IServiceProvider parentProvider, Type rootComponentType ) : base( parentProvider, rootComponentType ) { InitServices(); }


    //- The DesignSurface class provides several design-time services automatically.
    //- The DesignSurface class adds all of its services in its constructor.
    //- Most of these services can be overridden by replacing them in the
    //- protected ServiceContainer property.To replace a service, override the constructor,
    //- call base, and make any changes through the protected ServiceContainer property.
    private void InitServices() {
        //- each DesignSurface has its own default services
        //- We can leave the default services in their present state,
        //- or we can remove them and replace them with our own.
        //- Now add our own services using IServiceContainer
        //-
        //-
        //- Note
        //- before loading the root control in the design surface
        //- we must add an instance of naming service to the service container.
        //- otherwise the root component did not have a name and this caused
        //- troubles when we try to use the UndoEngine
        //-
        //-
        //- 1. NameCreationService
        _nameCreationService = new NameCreationServiceImp();
        if( _nameCreationService != null ) {
            this.ServiceContainer.RemoveService( typeof( INameCreationService ), false );
            this.ServiceContainer.AddService( typeof( INameCreationService ), _nameCreationService );
        }
        //-
        //-
        //- 2. CodeDomComponentSerializationService
        _codeDomComponentSerializationService = new CodeDomComponentSerializationService( this.ServiceContainer );
        if( _codeDomComponentSerializationService != null ) {
            //- the CodeDomComponentSerializationService is ready to be replaced
            this.ServiceContainer.RemoveService( typeof( ComponentSerializationService ), false );
            this.ServiceContainer.AddService( typeof( ComponentSerializationService ), _codeDomComponentSerializationService );
        }
        //-
        //-
        //- 3. IDesignerSerializationService
        _designerSerializationService = new DesignerSerializationServiceImpl( this.ServiceContainer );
        if( _designerSerializationService != null ) {
            //- the IDesignerSerializationService is ready to be replaced
            this.ServiceContainer.RemoveService( typeof( IDesignerSerializationService ), false );
            this.ServiceContainer.AddService( typeof( IDesignerSerializationService ), _designerSerializationService );
        }
        //-
        //-
        //- 4. UndoEngine
        _undoEngine = new UndoEngineExt( this.ServiceContainer );
        //- disable the UndoEngine
        _undoEngine.Enabled = false;
        if( _undoEngine != null ) {
            //- the UndoEngine is ready to be replaced
            this.ServiceContainer.RemoveService( typeof( UndoEngine ), false );
            this.ServiceContainer.AddService( typeof( UndoEngine ), _undoEngine );
        }
        //-
        //-
        //- 5. IMenuCommandService
        this.ServiceContainer.AddService( typeof( IMenuCommandService ), new MenuCommandService( this ) );
    }
    
    #endregion

    //- do some Edit menu command using the MenuCommandServiceImp
    public void DoAction( string command ) {
        if( string.IsNullOrEmpty( command ) ) return;

        IMenuCommandService ims = this.GetService( typeof( IMenuCommandService ) ) as IMenuCommandService;
        if( null == ims ) return;

        try {
            switch( command.ToUpper() ) {
                case "CUT" :
                    ims.GlobalInvoke( StandardCommands.Cut );
                    break;
                case "COPY" :
                    ims.GlobalInvoke( StandardCommands.Copy );
                    break;
                case "PASTE":
                    ims.GlobalInvoke( StandardCommands.Paste );
                    break;
                case "DELETE":
                    ims.GlobalInvoke( StandardCommands.Delete );
                    break;
                default:
                    // do nothing;
                    break;
            }//end_switch
        }//end_try
        catch( Exception exx ) {
            Debug.WriteLine( exx.Message );
            if( null != exx.InnerException )
                Debug.WriteLine( exx.InnerException.Message );
            throw;
        }//end_catch
    }
 }//end_class
}//end_namespace

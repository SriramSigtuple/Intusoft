/*
 Author:	Serban Iulian
 Date:		14 September 2002
 Class:		ToolBox
 e-mail:	iulianserban@hotmail.com
 Version:	1.0 
*/

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using INTUSOFT.ImageListView;
using System.Collections.Generic;

namespace ToolBoxLib
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class ToolBox : System.Windows.Forms.UserControl
	{
		enum States	{Compressed=1,Compressing,Extended,Extending};

		private System.ComponentModel.IContainer components;
        //private System.Windows.Forms.Timer timer1;
		private int ButtonHeight = 18;
		private int XOffset = 22;
		private int YOffset = 20;
		private int RightMargin = 5;
		private int Acceleration = 2;
		private bool Bounce = true;
		private int ScrollButtonX;
		private int ButtonWidth;
		private int ListWidth;
        private int Delay = 2000;
        private bool DoneMove = true;
		private bool Retreat = true;
		private bool RetreatCommand = false;
		private bool ExpandCommand = false;
		private string Separator = ".";
        private int state = (int)States.Extended;
		private ContextMenu CMenu = null;

		public delegate void OnToolBoxClick(int TabIndex,int ItemIndex,string btnName);
		public delegate void OnToolBoxTabChanged(int TabIndex);
		public delegate void OnToolBoxStateChanged(int State);

		private OnToolBoxClick OnTBClick = null;
        private OnToolBoxTabChanged OnTBTabChanged = null;

   
		private OnToolBoxStateChanged OnTBStateChanged = null;

		public void  SetClickDelegate(OnToolBoxClick OnTBxClick)
		{
			OnTBClick = OnTBxClick;
		}
		
		public  void SetTabChangedDelegate(OnToolBoxTabChanged OnTBTabChange)
		{
			OnTBTabChanged = OnTBTabChange;
		}

		public  void SetStateChangedDelegate(OnToolBoxStateChanged OnTBStateChange)
		{
			OnTBStateChanged = OnTBStateChange;
		}

		private void List_Click(object sender, System.EventArgs e)
		{
            ////Point pt = VisibleLV.PointToClient(MousePosition);
            //Point pt = GetTab(0).ImgListView.PointToClient(MousePosition);
            ////ListViewItem itm = VisibleLV.GetItemAt(pt.X,pt.Y);
            //ImageListView itm = (ImageListView)(GetTab(0).ImgListView.get(pt));
            //if(itm!=null && OnTBClick!=null)
            //{
            //    itm.Items[1].Selected = true;
            //    //OnTBClick(SelectedTab, itm.Index);
            //    //OnTBClick(
            //    //OnTBClick(SelectedTab, 0);
            //}
		}

		public ToolBox(Size sz,int Button_Height,ContextMenu Menu)
		{
			
			InitializeComponent();
			this.Size =sz;
			CMenu = Menu;
			ButtonWidth = ClientRectangle.Width - XOffset-2-RightMargin;
			ButtonHeight = (Button_Height<0)?ButtonHeight:Button_Height;
			ListWidth = ButtonWidth+100;
			ScrollButtonX = (ClientRectangle.Width - 19 -RightMargin);
			// TODO: Add any initialization after the InitForm call
		}

        void ImgListView_ItemClick(object sender, ItemClickEventArgs e)
        {
          
            OnTBClick(SelectedTab, e.Item.Index,e.Item.Text);
        }

        //void GetTab(0).ImgListView_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    {
        //        OnTBClick(SelectedTab, e.Item.Index);
        //        //OnTBClick(
        //    }
        //}

        /// <summary> 
		/// Clean up any resources being used.
		/// </summary>
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

        private object selectedItem;

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		

public object SelectedItem
{
  get { return selectedItem; }
  set { selectedItem = value; }
}private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // ToolBox
            // 
            this.Name = "ToolBox";
            this.Size = new System.Drawing.Size(176, 576);
            this.Load += new System.EventHandler(this.ToolBox_Load);
            this.ResumeLayout(false);

		}
		#endregion

        //private ListView VisibleLV = new ListView();


		public void AddTab(string Caption,int ImgIndex,ImageList ImgList)
		{
			ToolBoxTab tab = new ToolBoxTab();

			AddTab(tab);
		}
		public void AddTab(ToolBoxTab Tab)
		{
            Tab.ImgListView.Dock = DockStyle.Fill;
            this.Controls.Add(Tab.ImgListView);
			int nr = arrTabs.Count;
            Tab.ImgListView.ItemClick += ImgListView_ItemClick;

			if(ClientRectangle.Height<=(nr+5)*(ButtonHeight)+YOffset)
			{
				MessageBox.Show("You cannot add more tabs because it will fill up the list space");
				return;
			}


			
            //ListView lv = Tab.ListV ;
           

            //lv.Columns.Add(Tab.Caption,ButtonWidth-20,HorizontalAlignment.Left);
            //lv.FullRowSelect = true;
            //lv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;

            //lv.LabelEdit = true;
            //lv.MultiSelect = false;
            //lv.View = System.Windows.Forms.View.Details;
			








            //if(VisibleLV!=null)


			arrTabs.Add(Tab);

            //SetVisibleLV(lv);
		}
		private ArrayList arrTabs = new ArrayList();
		
		public ToolBoxTab GetTab(int Index)
		{
			if(Index>=arrTabs.Count||Index<0)
			{
				MessageBox.Show("Invalid Tab index at GetTab:"+Index.ToString());
				return null;
			}
			return ((ToolBoxTab)arrTabs[Index]);
		}

		private ArrayList arrMoveList = new ArrayList();
		private ArrayList arrSizeList = new ArrayList();

        public int GetCount()
        {
           return GetTab(0).ItemCount;
        }
        //public void AddItem(string Caption,int ImageIndex,int TabIndex,)
        //{
        //    System.Drawing.Design.ToolboxItem itm = new System.Drawing.Design.ToolboxItem();
        //    itm.Caption = Caption;
        //    itm.ImageIndex = ImageIndex;
        //    AddItem(itm,TabIndex);
        //}

        public List<System.Drawing.Design.ToolboxItem> GetItems()
        {
            return GetTab(0).GetItems();
        }

        public void AddItem(System.Drawing.Design.ToolboxItem Item, int TabIndex,ToolBoxItemDetails details)
		{
			if(TabIndex>arrTabs.Count-1 && TabIndex<0)
			{
				MessageBox.Show("Invalid Index value given to AddItem");
				return;
			}
			GetTab(TabIndex).AddItem(Item);
            //GetTab(TabIndex).ImgIndex = details.ImageIndex;
            //GetTab(TabIndex).ListV.Items.Add(details.Caption, details.ImageIndex);
            //GetTab(TabIndex).ListV.Items[0].Selected = true;
		}

        public void AddItem(System.Drawing.Design.ToolboxItem Item, int TabIndex, ToolBoxItemDetails details, string imagePath)
        {
            if (TabIndex > arrTabs.Count - 1 && TabIndex < 0)
            {
                MessageBox.Show("Invalid Index value given to AddItem");
                return;
            }
            ToolBoxTab tab = GetTab(TabIndex);
            tab.AddItem(Item);
            tab.ImgListView.Items.Add(imagePath, details.Caption);
            tab.ImgListView.Items[0].Selected = true;
            tab.ImgListView.BringToFront();
        }

        public void AddItem(System.Drawing.Design.ToolboxItem Item, int TabIndex, ToolBoxItemDetails details, string imagePath,string toolTip)
        {
            if (TabIndex > arrTabs.Count - 1 && TabIndex < 0)
            {
                MessageBox.Show("Invalid Index value given to AddItem");
                return;
            }
            ToolBoxTab tab = GetTab(TabIndex);
            tab.AddItem(Item);
            tab.ImgListView.Items.Add(imagePath,toolTip, details.Caption);
            tab.ImgListView.Items[0].Selected = true;
            tab.ImgListView.BringToFront();
        }


		private int EndFaze = 20;
		private int StartFaze = 0;
		private void Tab_Click(object sender, System.EventArgs e)
		{
			if(!DoneMove) return;
            Point pt = ((Button)sender).Location;
            //if(pt.Y == VisibleLV.Location.Y - (ButtonHeight+2)) return;
            if (pt.Y == GetTab(0).ImgListView.Location.Y - (ButtonHeight + 2)) return;

            //if(pt.Y<VisibleLV.Location.Y)
                if (pt.Y < GetTab(0).ImgListView.Location.Y)
			{
				int Index = (pt.Y+1-YOffset)/(ButtonHeight+2);
				int bottom = ClientRectangle.Bottom;
				int i = 0;
				for (i=arrTabs.Count-1;i>Index;i--)
				{
				}

                //int OwnButtonIndex = (VisibleLV.Location.Y - (ButtonHeight+2)-YOffset)/(ButtonHeight+2);
                int OwnButtonIndex = (GetTab(0).ImgListView.Location.Y - (ButtonHeight + 2) - YOffset) / (ButtonHeight + 2);

				OwnButtonIndex++;
                //VisibleLV.BringToFront();
                GetTab(0).ImgListView.BringToFront();

                //SizeObject so = new SizeObject(VisibleLV,StartFaze,EndFaze,new Size(ListWidth,0),new Point(XOffset,bottom - (arrTabs.Count-OwnButtonIndex)*(ButtonHeight+2)),false,Acceleration,Bounce);
                SizeObject so = new SizeObject(GetTab(0).ImgListView, StartFaze, EndFaze, new Size(ListWidth, 0), new Point(XOffset, bottom - (arrTabs.Count - OwnButtonIndex) * (ButtonHeight + 2)), false, Acceleration, Bounce);
				
                    arrSizeList.Add(so);

			
                //GetTab(Index).ListV.Show();
                GetTab(Index).ImgListView.Show();
				
				Size sz = new Size(ListWidth,bottom-(ButtonHeight+2)*(arrTabs.Count)-YOffset);
                //VisibleLV.Size = new Size(ListWidth,0);
                GetTab(0).ImgListView.Size = new Size(ListWidth, 0);

                //SizeObject so2 = new SizeObject(VisibleLV,StartFaze,EndFaze,sz,VisibleLV.Location,true,Acceleration,Bounce);
                SizeObject so2 = new SizeObject(GetTab(0).ImgListView, StartFaze, EndFaze, sz, GetTab(0).ImgListView.Location, true, Acceleration, Bounce);

				arrSizeList.Add(so2);
				
                //VisibleLV.SendToBack();
                GetTab(0).ImgListView.SendToBack();
				if(Index+1>=arrTabs.Count)
				{
					ResizeButtons(Index,-1);
				}
				else
				{
					ResizeButtons(Index,Index+1);
				}

                //MoveObject mo2 = new MoveObject((Control)ScrollButton2,StartFaze,EndFaze,new Point(ScrollButtonX,VisibleLV.Location.Y + sz.Height),Acceleration,Bounce);


				if(OnTBTabChanged!=null)
					OnTBTabChanged(Index);
			}
			else
			{
                //int Index = (pt.Y-VisibleLV.Size.Height+2-YOffset)/(ButtonHeight+2);
                int Index = (pt.Y - GetTab(0).ImgListView.Size.Height + 2 - YOffset) / (ButtonHeight + 2);
                if (Index < 0)
                    Index = 0;


				for(int i=0;i<=Index;i++)
				{
				}

                int bottom = ClientRectangle.Bottom;
                //SizeObject so2 = new SizeObject(VisibleLV,StartFaze,EndFaze,new Size(ListWidth,0),VisibleLV.Location,false,Acceleration,Bounce);
                SizeObject so2 = new SizeObject(GetTab(0).ImgListView, StartFaze, EndFaze, new Size(ListWidth, 0), GetTab(0).ImgListView.Location, false, Acceleration, Bounce);
				
                    arrSizeList.Add(so2);

                //GetTab(Index).ListV.Show();
                GetTab(Index).ImgListView.Show();

                //SetVisibleLV(GetTab(Index).ListV);

                //ListView lv = VisibleLV;
                ImageListView lv = GetTab(0).ImgListView;


				ResizeLists(-1);
				if (Index<arrTabs.Count-1)
				{
					ResizeButtons(Index,Index+1);
				}
				else
				{
					ResizeLists(Index);
					ResizeButtons(Index,-1);
				}


                //ScrollButton2.Location = new Point(ScrollButtonX, VisibleLV.Location.Y + VisibleLV.Size.Height);


				Size sz = lv.Size;
				Point pnt = lv.Location;
	
				lv.Size = new Size(ListWidth,0);
                //SizeObject so = new SizeObject(GetTab(Index).ListV,StartFaze,EndFaze,sz,pnt,true,Acceleration,Bounce);
                SizeObject so = new SizeObject(GetTab(Index).ImgListView, StartFaze, EndFaze, sz, pnt, true, Acceleration, Bounce);

				arrSizeList.Add(so);
				
				if(OnTBTabChanged!=null)
				OnTBTabChanged(Index);
			}
		}


		private void ResizeButtons(int i1, int i2)
		{
			for(int i=0;i<arrTabs.Count;i++)
			{

			}
		}

		private void ResizeLists(int i)
		{
			for(int j=0;j<arrTabs.Count;j++)
			{
				Size sz = new Size(ListWidth,ClientRectangle.Height - (ButtonHeight+2)*arrTabs.Count - YOffset);
			}
		}

		private int delay = 0;
		private int sbdelay = 0;
        //private void TimerTick(object sender, System.EventArgs e)
        //{
        //    Point pt =  this.PointToClient(MousePosition);
        //    Rectangle rect = new Rectangle(pt,new Size(1,1));
        //    bool MouseOver = (ClientRectangle.IntersectsWith(rect)&& !panel.ClientRectangle.IntersectsWith(rect));
        //    if (MouseOver) ExpandCommand = true;
			
			
        //if(arrMoveList.Count>0)
        //    {
        //        DoneMove = false;
        //    }
        //    else
        //    {
        //        DoneMove = true;
        //    }
        //    for (int i=0;i<arrMoveList.Count;i++)
        //    {

        //        MoveObject mo =((MoveObject)arrMoveList[i]);
        //        mo.Increment_State();
        //        if(mo.faze==mo.endfaze)
        //        {
        //            arrMoveList.RemoveAt(i);
        //            i--;
        //        }
        //    }

        //    for (int i=0;i<arrSizeList.Count;i++)
        //    {
        //        SizeObject so =((SizeObject)arrSizeList[i]);
        //        so.Increment_State();
        //        if(so.faze==so.endfaze)
        //        {
        //            arrSizeList.RemoveAt(i);
        //            i--;
        //        }
        //    }
        //    ScrollButton1.BringToFront();
        //    ScrollButton2.BringToFront();

        //    if((RetreatCommand)||(!MouseOver&& (delay+=(timer1.Interval)<20?20:timer1.Interval)>Delay&&Retreat))
        //    {
        //        ExpandCommand = false; 
        //        Size sz = this.Size;
        //        if (((sz.Width > XOffset)))
        //        {
        //            sz.Width -= 10;
        //            this.Size = sz;
        //            ChangeState((int)States.Compressing);
        //        }
        //        else
        //        {
        //            sz.Width = XOffset;

        //            this.splitter1.Hide();
        //            Size = sz;
        //            delay = 0;
        //            ChangeState((int)States.Compressed);
        //            RetreatCommand = false;
        //        }
        //    }

        //    if((ExpandCommand)||(MouseOver&& (delay+=20)>10))
        //    {
        //        Size sz = this.Size;
        //        if (sz.Width < ButtonWidth+XOffset+RightMargin)
        //        {
        //            sz.Width += 20;
        //            this.Size = sz;
        //            ChangeState((int)States.Extending);
        //        }
        //        else
        //        {
        //            sz.Width = XOffset + ButtonWidth+2+RightMargin;
        //            this.splitter1.Show();
        //            this.splitter1.Hide();

        //            Size = sz;
        //            delay = 0;
        //            ChangeState((int)States.Extended);
        //            ExpandCommand = false;
        //        }

        //    }

        //    if(SB1Down && (sbdelay+=(timer1.Interval<20)?20:timer1.Interval)>300)
        //    {
        //        ScrollUp();
        //        sbdelay-=20;
        //    }
			
        //    if(SB2Down && (sbdelay+=(timer1.Interval<20)?20:timer1.Interval)>300)
        //    {
        //        ScrollDown();
        //        sbdelay-=20;
        //    }
        //}
		private void Mouse_Down(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		    base.OnMouseDown(e);
            ImageListView lv = ((ImageListView)sender);
            //ListView lv =((ListView)sender);
            //ListViewItem itm = lv.GetItemAt(e.X,e.Y);
            ImageListViewItem itm = (ImageListViewItem)lv.Items.FocusedItem;

			if(itm==null) return;
		
			List_Click(sender,e);
			lv.DoDragDrop(lv.Columns[0].Text + Separator + lv.SelectedItems[0].Text, DragDropEffects.Copy);
		}

		public bool SetSize(Size sz)
		{
		
			bool statewidth = ((state==1)&&(sz.Width!=Width))||(state!=1);
			if (state==1) 
			{
				this.Size = new Size(Width,sz.Height);
			}
			else
			{
				this.Size = sz;
			}

			if((state!=1)&&((sz.Height<=(arrTabs.Count+5)*(ButtonHeight)+YOffset)||(sz.Width<XOffset+50)))
			{
				return false;
			}

			if(statewidth)ButtonWidth = sz.Width - XOffset-2-RightMargin;
			ListWidth = ButtonWidth+100;
			if(statewidth)ScrollButtonX = sz.Width - 19 - RightMargin;
			int bottom = ClientRectangle.Bottom;
            //int Index = (VisibleLV.Location.Y - ButtonHeight+2 +1 - YOffset)/(ButtonHeight+2);
            int Index = (GetTab(0).ImgListView.Location.Y - ButtonHeight + 2 + 1 - YOffset) / (ButtonHeight + 2);


			if(Index<0)
			{
				
				return true;
			}

			if (Index<arrTabs.Count-1)
			{
				ResizeButtons(Index,Index+1);
			}
			else
			{
				ResizeButtons(Index,-1);
			}

			
			int i = 0;
			for (i=arrTabs.Count-1;i>Index;i--)
			{
			}

			for(i=0;i<arrTabs.Count;i++)
			{
			}

			return true;

		}


		private void CloseButton_Click(object sender, System.EventArgs e)
		{
			Hide();
		}


        //public bool RemoveTab(int Index)
        //{
        //    if(Index>=arrTabs.Count||arrTabs.Count==0||Index<0)
        //    {
        //        MessageBox.Show("Index out of range at RemoveTab:"+Index.ToString());
        //        return false;
        //    }

        //    EndAllMovement();

        //    if(Index == arrTabs.Count-1 && Index!=0)
        //    {
        //        this.Controls.Remove(GetTab(Index).button);
        //        this.Controls.Remove(GetTab(Index).ListV);
        //        arrTabs.RemoveAt(Index);
        //        Tab_Click(GetTab(Index-1).button,new EventArgs());
        //    }
        //    else
        //    {	
        //        if(Index == 0 && arrTabs.Count==1)
        //        {
        //                this.Controls.Remove(GetTab(Index).button);
        //                this.Controls.Remove(GetTab(Index).ListV);
        //                arrTabs.RemoveAt(Index);
        //        }
        //        else
        //        {
        //            this.Controls.Remove(GetTab(Index).button);
        //            this.Controls.Remove(GetTab(Index).ListV);
        //            arrTabs.RemoveAt(Index);
        //            //VisibleLV.Hide();
        //            int i =0;
        //            for(i=Index;i<arrTabs.Count;i++)
        //            {
        //                Point pt = GetTab(i).button.Location;
        //                pt.Y = (ButtonHeight+2)*i + YOffset;
        //                GetTab(i).button.Location = pt ;
        //                pt = GetTab(i).ListV.Location;
        //                pt.Y = (ButtonHeight+2)*(i+1) + YOffset;
        //                GetTab(i).ListV.Location = pt ;
        //            }
        //            i--;
        //            SetVisibleLV(GetTab(i).ListV);
        //            VisibleLV.Show();
        //            ResizeButtons(i,-1);
        //            ResizeLists(i);

        //            Size sz = VisibleLV.Size;
        //            VisibleLV.Size = new Size(ListWidth,0);

        //            SizeObject so = new SizeObject(VisibleLV,StartFaze,EndFaze,sz,VisibleLV.Location,true,Acceleration,Bounce);
        //            arrSizeList.Add(so);

        //            ScrollButton1.Location = new Point(ClientRectangle.Width-19,VisibleLV.Location.Y-(ButtonHeight+2));
        //            ScrollButton2.Location = new Point(ClientRectangle.Width-19,VisibleLV.Location.Y+sz.Height);
        //        }
        //    }
        //    return true;
        //}

		public void EndAllMovement()
		{
			for(int i=0;i<arrSizeList.Count;i++)
			{
				((SizeObject)arrSizeList[i]).faze = EndFaze-1;
				((SizeObject)arrSizeList[i]).Increment_State();
				arrSizeList.RemoveAt(i);
				i--;
			}
		
			for(int i=0;i<arrMoveList.Count;i++)
			{
				((MoveObject)arrMoveList[i]).faze = EndFaze-1;
				((MoveObject)arrMoveList[i]).Increment_State();
				arrMoveList.RemoveAt(i);
				i--;
			}

		}

		public int GetTabCount()
		{
			return arrTabs.Count;
		}

		public int SelectedTab
		{
			get
			{
                //return (int)((VisibleLV.Location.Y-(ButtonHeight+2)+1-YOffset)/(ButtonHeight+2));
                return (int)((GetTab(0).ImgListView.Location.Y - (ButtonHeight + 2) + 1 - YOffset) / (ButtonHeight + 2));

			}
			set
			{
				int sel = value;
				if(sel>=arrTabs.Count||sel<0)
				{
					MessageBox.Show("Invalid Index at SelectedTab:"+sel.ToString());
					return;
				}	
			}
		}

		public bool RemoveItem(int TabIndex, int ItemIndex)
		{
			if(TabIndex>=arrTabs.Count||TabIndex<0)
			{
				MessageBox.Show("Invalid TabIndex at RemoveItem:"+TabIndex.ToString());
				return false;
			}
			return true;
		}
        public bool RemoveItem(System.Drawing.Design.ToolboxItem item)
        {
            GetTab(0).Remove(item);
            return true;
        }
        public void SetItem(int itemIndex)
        {
          SelectedItem =  GetTab(0).arrItems[itemIndex];
        }

		private void Splitter_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            //if(e.Button == MouseButtons.Left)
            //{
            //    Size sz = this.Size;

            //        sz.Width = e.X+ClientRectangle.Width;
            //if(sz.Width>XOffset+50)
            //    {
            //        EndAllMovement();
            //        SetSize(sz);
            //        if(e.X>0)
            //        {
            //            Invalidate();
            //            this.Update();
            //        }
            //    }
            //}
		}

		private void Splitter_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int y = e.ClipRectangle.Bottom;
			int top = e.ClipRectangle.Top;

				e.Graphics.DrawLine(Pens.LightGray,new Point(0,top),new Point(0,y));
				e.Graphics.DrawLine(Pens.DarkGray,new Point(1,top),new Point(1,y));
				e.Graphics.DrawLine(Pens.Black,new Point(2,top),new Point(2,y));
				e.Graphics.DrawLine(Pens.Black,new Point(3,top),new Point(3,y));

		}


		private void Menu_Hide_Click(object sender, System.EventArgs e)
		{
			Hide();
		}

		private void Menu_Dock_Left_Click(object sender, System.EventArgs e)
		{
			Dock = DockStyle.Left;
		}

		private void Menu_Dock_Right_Click(object sender, System.EventArgs e)
		{
			Dock = DockStyle.Right;
		}

		public int TabTime
		{
			get
			{
				return EndFaze*20;
			}
			set
			{
				EndFaze = (int)value/20;
			}
		}

		public int DelayBeforeRetreat
		{
			get
			{
				return Delay;
			}
			set
			{
				Delay = value;
			}
		}

		public int TabAcceleration
		{
			get
			{
				return Acceleration;
			}
			set
			{
				if(value>0&&value<11)
				Acceleration = value;
			}
		}

		public bool TabBounce
		{
			get
			{
				return Bounce;
			}
			set
			{
				Bounce = value;
			}
		}

        //public int TimerInterval
        //{
        //    get
        //    {
        //        return timer1.Interval;
        //    }
        //    set
        //    {
        //        timer1.Interval = value;
        //    }
        //}


		public override DockStyle Dock
		{
			get { return DockStyle.None; }

			set
			{
				 return;
			}

		}

		private void AdjustPositions(DockStyle ds)
		{
			return;
/*			Invalidate(true);
			Update();
			if((ds == DockStyle.Left&&XOffset<RightMargin)||(ds == DockStyle.Right&&XOffset>RightMargin))
			{
				int OldXOffset = XOffset;
				XOffset = RightMargin;
				RightMargin = OldXOffset;
			}
			if(ds == DockStyle.Left)
			{
				Picture.Location = new Point(0,0);
				this.splitter1.Dock = DockStyle.Right;
			}
			if(ds == DockStyle.Right)
			{
				Picture.Location = new Point(ClientRectangle.Width-Picture.Width,0);
				this.splitter1.Dock = DockStyle.Left;
			}
			

			ScrollButton1.Location = new Point(ClientRectangle.Width-19 -RightMargin,ScrollButton1.Location.Y);
			ScrollButton2.Location = new Point(ClientRectangle.Width-19 -RightMargin,ScrollButton2.Location.Y);
			Title.Location = new Point(XOffset+2,Title.Location.Y);
			Title.Width = ButtonWidth;

			Pioneza.Location = new Point(ClientRectangle.Width-RightMargin-2*Pioneza.Width-4,Pioneza.Location.Y);
			CloseButton.Location = new Point(ClientRectangle.Width-RightMargin-CloseButton.Width-2,CloseButton.Location.Y);

			for(int i=0;i<arrTabs.Count;i++)
			{
				GetTab(i).button.Location = new Point(XOffset,GetTab(i).button.Location.Y);
				GetTab(i).ListV.Location = new Point(XOffset,GetTab(i).ListV.Location.Y);
			}
*/		}

		private void Menu_About_Click(object sender, System.EventArgs e)
		{
			AboutForm af = new AboutForm();
			af.Show();
		}

		private void ToolBox_Load(object sender, System.EventArgs e)
		{
		
		}

		public void Compress()
		{
			ExpandCommand = false;
			RetreatCommand = true;
		}

		public void Expand()
		{
			ExpandCommand = true;
			RetreatCommand = false;
		}

		public string DragDropSeparatorText
		{
			get
			{
				return Separator;
			}
			set
			{
				Separator = value;
			}
		}

		public int State
		{
			get
			{
				return state;
			}
		}

		private void ChangeState(int CState)
		{
			if(state==CState) return;
			this.state = CState;
			if (OnTBStateChanged!=null)
			OnTBStateChanged(CState);
		}

		private bool SB1Down = false;
		private bool SB2Down = false;

		private void SB1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			SB1Down = true;
            //ScrollUp();
		}

		private void SB1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			SB1Down = false;
			sbdelay = 0;
		}

		private void SB2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			SB2Down = true;
            //ScrollDown();
		}

		private void SB2_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			SB2Down = false;
			sbdelay = 0;
		}

        //public bool ScrollUp()
        //{
        //    if(VisibleLV.Items.Count==0) return false;
        //    ListViewItem itm = VisibleLV.GetItemAt(2,2);
        //    if(itm == null) return false;
        //    if(itm.Index==0) return false;
        //    VisibleLV.EnsureVisible(itm.Index-1);
        //    return true;
        //}

        //public bool ScrollDown()
        //{
        //    if(VisibleLV.Items.Count==0) return false;
        //    ListViewItem itm = VisibleLV.GetItemAt(2,VisibleLV.Height-2);
        //    if(itm == null) return false;
        //    if(itm.Index==VisibleLV.Items.Count-1) return false;
        //    VisibleLV.EnsureVisible(itm.Index+1);
        //    return true;
        //}

		private void Picture_Click(object sender, System.EventArgs e)
		{
			ExpandCommand = true;
		}

	}
}


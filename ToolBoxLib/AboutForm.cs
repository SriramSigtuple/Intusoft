using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ToolBoxLib
{
	/// <summary>
	/// Summary description for AboutForm.
	/// </summary>
	public class AboutForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		private Point start;
		private Point end;
		private bool down = false;
		private MoveObject mo;
		private bool clicked = false;
		public AboutForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			start = Location;
			end = Location;
			end.Y+=500;

			mo = new MoveObject(this,0,100,end,3,false);

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AboutForm));
			this.label1 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(80, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(232, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "ToolBox Control  By Iulian Serban";
			// 
			// linkLabel1
			// 
			this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel1.Location = new System.Drawing.Point(80, 32);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(144, 23);
			this.linkLabel1.TabIndex = 1;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "iulianserban@hotmail.com";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(51, 49);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// timer1
			// 
			this.timer1.Interval = 20;
			this.timer1.Tick += new System.EventHandler(this.Tick);
			// 
			// AboutForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(312, 70);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.pictureBox1,
																		  this.linkLabel1,
																		  this.label1});
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowInTaskbar = false;
			this.Text = "AboutForm";
			this.ResumeLayout(false);

		}
		#endregion

		private void Tick(object sender, System.EventArgs e)
		{
			mo.Increment_State();
			if(mo.faze==mo.endfaze)
			{
				if(down)
				{
					mo = new MoveObject(this,0,100,start,2,false);
					down = false;
				}
				else
				{
					mo = new MoveObject(this,0,100,end,2,false);
					down = true;
				}
			}
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if(!clicked)
			{
				timer1.Enabled = true;
				this.FormBorderStyle = FormBorderStyle.None;
				clicked = true;
				Location = start;
				this.TransparencyKey = System.Drawing.Color.White;
			}
			else
			{
				Hide();
			}	
		}
	}
}

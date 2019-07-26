/*
 Author:	Serban Iulian
 Date:		14 September 2002
 Class:		SizeObject
 e-mail:	iulianserban@hotmail.com
 Version:	1.0 
*/
using System;
using System.Windows.Forms;
using System.Drawing;

namespace ToolBoxLib
{
	/// <summary>
	/// Summary description for SizeObject.
	/// </summary>
	public class SizeObject
	{
		public System.Windows.Forms.Control control ;
		public int faze ;
		public int endfaze;
		public Size endSize ;
		public Size startSize ;
		public Point startPoint;
		public Point endPoint;
		public float increment;
		public float ptincrement;
		private bool up;
		public int Acceleration;
		public bool Bounce;
		private int rest;
		private int ptrest;

		public SizeObject(Control ctrl,int fz,int edfz,Size sz,Point pt,bool updown,int Accel,bool Bnc)
		{
			control = ctrl;
			faze = fz;
			endfaze = edfz;
			endSize = sz;
			endPoint = pt;
			up = updown;
			Acceleration = Accel;
			Bounce = Bnc;

			startPoint = control.Location;
			startSize = ctrl.Size;
			increment = (startSize.Height-endSize.Height)/endfaze;
			rest =	(startSize.Height-endSize.Height)%endfaze;

			ptincrement = (endPoint.Y - startPoint.Y)/endfaze;
			ptrest = (endPoint.Y - startPoint.Y)%endfaze;
		}

		public void Increment_State()
		{
			faze++;
			float inc,ptinc;
			int divide = Acceleration;
			//float BounceCoef = 1/2+1/2*Acceleration;
			//float BounceCoef = 3/4;

			if(faze<endfaze/4) {inc = increment/divide;	ptinc = ptincrement/divide;}
			else
			{
				if(faze<endfaze/2){ inc = increment;ptinc = ptincrement;}
				else
				{
					if((faze<=(int)endfaze*3/4&&Bounce)||(!Bounce && faze<(int)endfaze*3/4)) 
					{
						inc = ((endfaze*increment)/2 - (increment*endfaze)/(4*divide))/(endfaze/4);
						ptinc = ((endfaze*ptincrement)/2 - (ptincrement*endfaze)/(4*divide))/(endfaze/4);
					}
					else
					{inc = increment+rest/(endfaze/4);
					ptinc = ptincrement+ptrest/(endfaze/4);}
				}
			}

			Size sz = control.Size;
			sz.Height -=(int)inc;
			Point pt = control.Location;
			pt.Y +=(int)ptinc;
			if (faze == endfaze)
			{
				if (!up)
				{
					control.Hide();
					sz = startSize;
					pt = startPoint;
				}
				else
				{
					sz = endSize;
					pt = endPoint;
				}
			}
			if(!up)
			{
				control.Size = sz;
				control.Location = pt;
			}
			else
			{
				control.Location = pt;
				control.Size = sz;
			}
		}
	}
}

/*
 Author:	Serban Iulian
 Date:		14 September 2002
 Class:		MoveObject
 e-mail:	iulianserban@hotmail.com
 Version:	1.0 
*/
using System;
using System.Windows.Forms;
using System.Drawing;

namespace ToolBoxLib
{
	/// <summary>
	/// Summary description for MoveObect.
	/// </summary>
	public class MoveObject
	{
		public System.Windows.Forms.Control control ;
		public int faze ;
		public int endfaze;
		public Point endpoint ;
		public float increment;
		public int Acceleration;
		public bool Bounce;
		private int rest;
		public MoveObject(Control ctrl,int fz,int edfz,Point pt,int Accel,bool Bnc)
		{
            control = ctrl;
			faze = fz;
			endfaze = edfz;
			endpoint = pt;
			Acceleration = Accel;
			Bounce = Bnc;
			Point start = ctrl.Location;
			increment = (endpoint.Y - start.Y)/endfaze;
			rest = (endpoint.Y - start.Y)%endfaze;
			
		}

		public void Increment_State()
		{
			float inc;
			int divide = Acceleration;
			//float BounceCoef = 1/2+1/2*Acceleration;
			//float BounceCoef = 3/4;

			if(faze<endfaze/4) {inc = increment/divide;}
			else
			{
				if(faze<endfaze/2){ inc = increment;}
				else
				{
					if((faze<=(int)endfaze*3/4&&Bounce)||(!Bounce && faze<(int)endfaze*3/4)) 
					{
						inc = ((endfaze*increment)/2 - (increment*endfaze)/(4*divide))/(endfaze/4);
					}
					else
						{inc = increment+rest/(endfaze/4);}
				}
			}
			faze++;
			Point pt = control.Location;
			pt.Y +=(int)inc;
			if (faze == endfaze)
			{
				//MessageBox.Show(pt.Y.ToString() + "=====" + endpoint.Y.ToString());
				pt.Y = endpoint.Y;
			}
			control.Location = pt;
		}
	}
}

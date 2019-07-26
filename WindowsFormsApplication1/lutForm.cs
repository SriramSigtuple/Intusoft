using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NLog;
using NLog.Targets;
namespace AssemblySoftware
{
    public partial class lutForm : Form
    {
        public Logger Exception_Log = LogManager.GetLogger("AssemblySoftware.ExceptionLog");// for exception logging

        public lutForm()
        {
            InitializeComponent();
            this.KeyDown += lutForm_KeyDown;
            ShowLut();
        }

        private void ShowLut()
        {
            try
            {
                StreamReader st = new StreamReader(@"lut.csv");
                string readline = "";
                int count = 0;
                List<int> value1 = new List<int>();
                List<int> value2 = new List<int>();
                while ((readline = st.ReadLine()) != null)
                {
                    if (count != 0)
                    {
                        string[] arr = readline.Split(',');
                        value1.Add(Convert.ToInt32(arr[0]));
                        value2.Add(Convert.ToInt32(arr[2]));
                    }
                    count++;
                }
                st.Close();
                st.Dispose();
                Bitmap bm = new Bitmap(255, 255);
                Graphics g = Graphics.FromImage(bm);
                //for (int i = value1.Count - 1; i < 1; i--)
                for (int i = 1; i < value1.Count; i++)
                {
                    g.DrawLine(new Pen(Brushes.Red, 1.0f), new Point(i - 1, bm.Height - value1[i - 1]), new Point(i, bm.Height - value1[i]));
                    PointF[] pointArr = { new PointF(i - 1, bm.Height - value2[i - 1]), new PointF(i, bm.Height - value2[i]) };

                    g.DrawCurve(new Pen(Brushes.Green, 1.0f), pointArr);
                }
                pictureBoxExtended1.Image = bm;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        void lutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

    }
}

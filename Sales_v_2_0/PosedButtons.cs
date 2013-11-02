using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sales_v_2_0
{
    class PosedButtons:Button
    {
        public PosedButtons():base()
        {
        }
        public PosedButtons(int left, int top, string text, Control container)
            : base()
        {
            this.Left = left;
            this.Top = top;
            this.Text = text;
            container.Controls.Add(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sales_v_2_0
{
    static class Pickers
    {
        public static DateTimePicker CreateDatePicker(int left, int top, Control ctr)
        {
            DateTimePicker picker = new DateTimePicker();
            picker.Left = left;
            picker.Top = top;
            ctr.Controls.Add(picker);
            return picker;
        }

        public static DateTimePicker CreateTimePicker(int left, int top, int width, Control ctr)
        {
            DateTimePicker picker = new DateTimePicker();
            picker.Left = left;
            picker.Top = top;
            picker.Width = width;
            picker.Format = DateTimePickerFormat.Time;
            picker.ShowUpDown = true;
            ctr.Controls.Add(picker);
            return picker;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Sales_v_2_0
{
    public partial class Form2 : Form
    {
        //Varibles
        #region
        Label label_local_path;
        TextBox tb_local_path;
        public ListBox lb_local_path;
        Button add_btn;
        Button select_all_btn;
        Button deselect_all_btn;
        Label label_global_path;
        TextBox tb_global_path;
        Button ok_btn;
        Button close_btn;
        Button search_local_btn;
        Button search_global_btn;
        Button delete_local_btn;
        public Form1 mainform;
        #endregion

        public Form2()
        {
            InitializeComponent();
            //mainform = this.Owner as Form1; 
            //Добавление и инициализация элементов управлениявв
            #region
            int padding = 5;
            this.Text = "Пути к базам данных";
            this.BackColor = Color.White;

            label_local_path = new Label();
            label_local_path.Text = "Подключенные базы Local";
            label_local_path.Left = 10;
            label_local_path.Top = 10;
            label_local_path.Width = 300;
            this.Controls.Add(label_local_path);

            tb_local_path = new TextBox();
            tb_local_path.Left = 10;
            tb_local_path.Top = label_local_path.Bottom;
            tb_local_path.Width = 350;
            this.Controls.Add(tb_local_path);

            lb_local_path = new ListBox();
            lb_local_path.Left = 10;
            lb_local_path.Top = tb_local_path.Bottom + padding;
            lb_local_path.Width = tb_local_path.Width;
            lb_local_path.Height = 200;
            lb_local_path.SelectionMode = SelectionMode.MultiExtended;
            OleDbDataReader reader = DataBaseQuerys.SelectQuary("temp.mdb",
                "select path from global_local where name='local' UNION select path from paths", null);
            while (reader.Read())
            {
                lb_local_path.Items.Add(reader[0]);
            }
            reader.Close();
            this.Controls.Add(lb_local_path);


            add_btn = new PosedButtons(10, lb_local_path.Bottom + padding,
                "Добавить", this);

            select_all_btn = new PosedButtons(add_btn.Right + padding, add_btn.Top, "Выделить все", this);
            select_all_btn.Width = 100;

            deselect_all_btn = new PosedButtons(select_all_btn.Right + padding,
                select_all_btn.Top, "Снять выделение", this);
            deselect_all_btn.Width = 120;

            label_global_path = new Label();
            label_global_path.Left = 10;
            label_global_path.Top = add_btn.Bottom + 4 * padding;
            label_global_path.Width = 300;
            label_global_path.Text = "Подключенная база Global";
            this.Controls.Add(label_global_path);

            tb_global_path = new TextBox();
            tb_global_path.Left = 10;
            tb_global_path.Top = label_global_path.Bottom;
            tb_global_path.Width = tb_local_path.Width;
            OleDbDataReader read = DataBaseQuerys.SelectQuary("temp.mdb", "Select path from global_local where name='global'", null);
            while (read.Read())
            {
                tb_global_path.Text = read[0].ToString();
            }
            read.Close();
            this.Controls.Add(tb_global_path);

            ok_btn = new PosedButtons(tb_global_path.Left, tb_global_path.Bottom + 2 * padding, "Ok", this);
            close_btn = new PosedButtons(tb_global_path.Right + padding, ok_btn.Top, "Закрыть", this);
            search_local_btn = new PosedButtons(tb_local_path.Right + padding, tb_local_path.Top, "...", this);
            search_global_btn = new PosedButtons(tb_global_path.Right + padding, tb_global_path.Top, "...", this);
            delete_local_btn = new PosedButtons(lb_local_path.Right + padding, lb_local_path.Top, "Удалить", this);

            this.Width = tb_local_path.Width + search_local_btn.Width + 8 * padding;
            this.Height = label_global_path.Height+tb_local_path.Height+padding+lb_local_path.Height+padding+select_all_btn.Height+
                +padding*4+label_global_path.Height+tb_global_path.Height+padding+ok_btn.Height+padding*13;
            #endregion

            //События Click всех кнопок формы
            #region
            add_btn.Click += new EventHandler(this.add_btn_click);
            close_btn.Click += new EventHandler(this.close_btn_click);
            delete_local_btn.Click += new EventHandler(this.delete_local_btn_click);
            deselect_all_btn.Click += new EventHandler(this.deselect_all_btn_click);
            ok_btn.Click += new EventHandler(this.ok_btn_click);
            search_global_btn.Click += new EventHandler(this.search_global_btn_click);
            search_local_btn.Click += new EventHandler(this.search_local_btn_click);
            select_all_btn.Click += new EventHandler(this.select_all_btn_click);
            #endregion
        }

        public void add_btn_click(object sender, EventArgs e)
        {
            bool existance = false;
            if (tb_local_path.Text == String.Empty)
            {
                MessageBox.Show("Неверный путь");
                return;
            }
            for (int i = 0; i < lb_local_path.Items.Count; i++)
            {
                if (lb_local_path.Items[i].ToString()==tb_local_path.Text)
                {
                    existance = true;
                    tb_local_path.Clear();
                    lb_local_path.SetSelected(i, true);
                    return;
                }
            }
            if (!existance)
            {
                lb_local_path.Items.Add(tb_local_path.Text);
                OleDbParameter[] parameters = { new OleDbParameter { Value = tb_local_path.Text }};
                DataBaseQuerys.InsertQuery("temp.mdb", "insert into paths(path) values(?)",parameters);
                tb_local_path.Clear();
            }
            
        }

        public void close_btn_click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void delete_local_btn_click(object sender, EventArgs e)
        {
            int del = lb_local_path.SelectedIndex;
            if (del!=-1)
            {
                lb_local_path.Items.RemoveAt(del);    
            }
        }

        public void deselect_all_btn_click(object sender, EventArgs e)
        {
        }

        public void ok_btn_click(object sender, EventArgs e)
        {
            mainform = this.Owner as Form1;
            bool existing = false;
            foreach (var item in lb_local_path.SelectedItems)
            {
                if (mainform.local_path.Count>0)
                {
                    foreach (string i in mainform.local_path)
                    {
                        if (i.Contains(item.ToString()))
                        {
                            existing = true;
                        }
                    }
                }
                else
                {
                    mainform.local_path.Add(item.ToString());
                    existing = true;
                }
                if (!existing)
                {
                    mainform.local_path.Add(item.ToString());
                }
            }
            this.Close();
        }

        public void search_global_btn_click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            tb_global_path.Text = ofd.FileName;
        }

        public void search_local_btn_click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            tb_local_path.Text = ofd.FileName;
        }

        public void select_all_btn_click(object sender, EventArgs e)
        {
            for (int i = 0; i < lb_local_path.Items.Count; i++)
            {
                lb_local_path.SetSelected(i, false);
            }
        }
    }
}

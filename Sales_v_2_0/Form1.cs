using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using System.Collections;

namespace Sales_v_2_0
{
    public partial class Form1 : Form
    {
        //variables
        #region
        public OleDbConnection connection;
        public OleDbDataReader reader;
        public ListView goods_list;
        public ListView sales_list;
        public OpenFileDialog ofd;
        public string first_filter_date;
        public string second_filter_date;

        public DateTimePicker first_date;
        public DateTimePicker first_time;
        public DateTimePicker second_date;
        public DateTimePicker second_time;

        public string global_path;
        public List<string> local_path;
        public string main_local_path;
        public Form2 form2;
        #endregion

        public Form1()
        {
            InitializeComponent();
	    //TODO Use english commetns!!
            //Инициализируем элементы управления +++
            #region
            this.Text = "Sales";
            
            //Зададим размеры формы
            //this.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height-50);
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.BackColor = Color.White;

            //Добавляем TabControl в форму
            #region
            TabControl tabs = new TabControl();
            tabs.TabPages.Add("Товары");//первая вкладка
            tabs.TabPages.Add("Продажи");//вторая вкладка
            //Задаем размеры и положение TabControl
            tabs.SetBounds(0, 0,
                (int)(this.Width - this.Width * 0.05),
                (int)(this.Height - this.Height * 0.1));
            tabs.TabPages[0].BackColor = Color.White;
            tabs.TabPages[1].BackColor = Color.White;
            this.Controls.Add(tabs);
            #endregion

            //Вкладка "Товары"

            //Создадим ListView для отображения данных из базы Global
            #region
            goods_list = new ListView();
            goods_list.View = View.Details;
            goods_list.Columns.Add(new ColumnHeader("Code") { Text = "Код" });
            goods_list.Columns.Add(new ColumnHeader("Barcode") { Text = "Штрихкод" });
            goods_list.Columns.Add(new ColumnHeader("Name") { Text = "Наименование" });
            goods_list.Columns.Add(new ColumnHeader("Quantity") { Text = "Количество" });
            goods_list.Columns.Add(new ColumnHeader("MeasurementUnit") { Text = "Ед. изм." });
            goods_list.Columns.Add(new ColumnHeader("Group") { Text = "Группа" });
            goods_list.Columns.Add(new ColumnHeader("department") { Text = "Отдел" });
            goods_list.Columns.Add(new ColumnHeader("НГ") { Text = "НГ" });
            goods_list.Columns.Add(new ColumnHeader("Кр. остаток") { Text = "Кр. остаток" });
            goods_list.Columns.Add(new ColumnHeader("НK") { Text = "НK" });
            goods_list.Columns.Add(new ColumnHeader("Срок годности") { Text = "Срок годности" });
            goods_list.Columns.Add(new ColumnHeader("Служ. 1") { Text = "Служ. 1" });
            goods_list.Columns.Add(new ColumnHeader("Служ. 2") { Text = "Служ. 2" });
            goods_list.Columns.Add(new ColumnHeader("Служ. 3") { Text = "Служ. 3" });

            int goods_list_weigth = 0; //ширина ListView

            for (int i = 0; i < goods_list.Columns.Count; i++)
            {
                switch (i)
                {
                    case 2:
                        goods_list.Columns[i].Width = 100;
                        goods_list_weigth += 100;
                        break;
                    case 3:
                        goods_list.Columns[i].Width = 100;
                        goods_list_weigth += 100;
                        break;
                    case 8:
                        goods_list.Columns[i].Width = 100;
                        goods_list_weigth += 100;
                        break;
                    case 10:
                        goods_list.Columns[i].Width = 100;
                        goods_list_weigth += 100;
                        break;
                    default:
                        goods_list.Columns[i].Width = 70;
                        goods_list_weigth += 70;
                        break;
                }
            }

            goods_list.GridLines = true;
            goods_list.SetBounds(0, 0, goods_list_weigth, tabs.Height);
            tabs.TabPages[0].Controls.Add(goods_list);
            #endregion

            //Кнопка "Refrash"
            Button refrash_btn = new PosedButtons(goods_list_weigth + 10,
                                                  10,
                                                  "Refrash",
                                                  tabs.TabPages[0]);

            //Кнопка Export
            Button goods_export_btn = new PosedButtons(goods_list_weigth + 10,
                refrash_btn.Bottom + 10,
                "Export",
                tabs.TabPages[0]);

                // Вкладка "Товары"

            //Создадим ListView для отображения данных из базы Local
            #region
            sales_list = new ListView();
            sales_list.View = View.Details;
            sales_list.Columns.Add(new ColumnHeader("Code") { Text = "Код" });
            sales_list.Columns.Add(new ColumnHeader("Barcode") { Text = "Штрихкод" });
            sales_list.Columns.Add(new ColumnHeader("Name") { Text = "Наименование" });
            sales_list.Columns.Add(new ColumnHeader("Price") { Text = "Цена" });
            sales_list.Columns.Add(new ColumnHeader("Quantity") { Text = "Количество" });
            sales_list.Columns.Add(new ColumnHeader("Left") { Text = "Остаток" });
            sales_list.Columns.Add(new ColumnHeader("in_all") { Text = "Всего" });
            sales_list.Columns.Add(new ColumnHeader("related") { Text = "Схожие" });

            int sales_list_weigth = 0; //ширина ListView

            for (int i = 0; i < sales_list.Columns.Count; i++)
            {
                switch (i)
                {
                    case 1:
                        sales_list.Columns[i].Width = 100;
                        sales_list_weigth += 100;
                        break;
                    case 2:
                        sales_list.Columns[i].Width = 310;
                        sales_list_weigth += 310;
                        break;
                    default:
                        sales_list.Columns[i].Width = 80;
                        sales_list_weigth += 80;
                        break;
                }
            }

            sales_list.Width = sales_list_weigth;
            sales_list.Height = tabs.Height;
            sales_list.GridLines = true;
            tabs.TabPages[1].Controls.Add(sales_list);
            #endregion

            // Фильтр времени
            #region
            //Создаем 4 DateTimePicker для выбора даты и времени фильтра
            first_date = Pickers.CreateDatePicker(sales_list_weigth + 10, 5, tabs.TabPages[1]);
            first_time = Pickers.CreateTimePicker(first_date.Right + 10, 5, 80, tabs.TabPages[1]);
            second_date = Pickers.CreateDatePicker(sales_list_weigth + 10, first_date.Bottom + 10, tabs.TabPages[1]);
            second_time = Pickers.CreateTimePicker(first_date.Right + 10, first_date.Bottom + 10, 80, tabs.TabPages[1]);
            #endregion

            //Кнопки "База данных" и "Обновить"
            Button database_btn = new PosedButtons(sales_list_weigth + 10, second_date.Bottom + 10, "База данных", tabs.TabPages[1]);
            database_btn.Width = (int)(database_btn.Width * 1.3); 
            Button update_btn = new PosedButtons(database_btn.Right + 10, second_date.Bottom + 10, "Обновить", tabs.TabPages[1]);

            //CheckBox
            #region
            CheckBox similar = new CheckBox();
            similar.Left = sales_list_weigth + 10;
            similar.Top = database_btn.Bottom + 10;
            similar.Text = "Схожие";
            tabs.TabPages[1].Controls.Add(similar);
            #endregion

            //События Click для всех кнопок
            #region
            refrash_btn.Click += new EventHandler(this.refrash_btn_click);
            goods_export_btn.Click += new EventHandler(this.goods_export_btn_click);
            database_btn.Click += new EventHandler(this.database_btn_click);
            update_btn.Click += new EventHandler(this.update_btn_click);
            #endregion
            #endregion
            //База данных
            //Global  +++
            #region
            //Путь к подключенной базе Global
            OleDbDataReader read_global_path = DataBaseQuerys.SelectQuary("temp.mdb", "select path from global_local where name = 'global'", null);
            read_global_path.Read();
            global_path = read_global_path[0].ToString();
            //Вывод данных из подключенной базы Global
            OleDbDataReader read_global = DataBaseQuerys.SelectQuary(global_path,
                "SELECT ItmCode, ItmBarCode, ItmName, ItmPrice, ItmCount, (ItmCount*ItmPrice) as summ, ItmUnit, ItmGroup, ItmDepartm, ItmTaxGrp, ItmCrRest, ItmNullCnt, ItmDateExp, Itm1, Itm2, Itm3 FROM Items", null);
            DataBaseQuerys.ListViewFilling(read_global, goods_list);
            #endregion
            //Local
            #region
            //Записываем путь к основному файлу Local
            local_path = new List<string>();
            OleDbDataReader local_path_rd = DataBaseQuerys.SelectQuary("temp.mdb", 
                "select path from global_local where name='local'", null);
            while (local_path_rd.Read())
            {
                main_local_path = local_path_rd[0].ToString();
                local_path.Add(local_path_rd[0].ToString());
            }
            local_path_rd.Close();

            OleDbDataReader local_check = DataBaseQuerys.SelectQuary("temp.mdb", 
                "SELECT Selected FROM global_local WHERE name='local'", null);
            local_check.Read();
            if (local_check[0].ToString()=="0")
            {
                OleDbDataReader local_copy = DataBaseQuerys.SelectQuary(main_local_path, 
                    "SELECT PrCode, PrBarCode, PrName, PrPrice, PrCount, PrChNum FROM SalesProt", null);
                OleDbParameter[] parametr = new OleDbParameter[local_copy.FieldCount];
                while (local_copy.Read())
                {
                    for (int i = 0; i < local_copy.FieldCount; i++)
                    {
                        parametr[i] = new OleDbParameter { Value = local_copy[i] };
                    }
                    
                    DataBaseQuerys.InsertQuery("temp.mdb",
                        "INSERT INTO SalesProtTemp(PrCode, PrBarCode, PrName, PrPrice, PrCount, PrChNum) VALUES(?,?,?,?,?,?)", parametr);
                }
                local_copy.Close();
                

                OleDbDataReader pay_type_copy = DataBaseQuerys.SelectQuary(main_local_path,
                    "SELECT PTChNum, PTCHDate FROM PayType", null);
                OleDbParameter[] pay_param = new OleDbParameter[pay_type_copy.FieldCount];
                while (pay_type_copy.Read())
                {
                    for (int i = 0; i < pay_type_copy.FieldCount; i++)
                    {
                        pay_param[i] = new OleDbParameter { Value = pay_type_copy[i] };
                    }
                    DataBaseQuerys.InsertQuery("temp.mdb",
                        "INSERT INTO(PTChNum, PTCHDate) VALUES(?,?)", pay_param);
                }
                pay_type_copy.Close();

                DataBaseQuerys.InsertQuery("temp.mdb", "UPDATE global_local SET Selected=1", null);
            }
            local_check.Close();

            //==========================================================
            //Вывод данных
           
            #region
            //Нижняя граница
            DateTime lower_date_limit = new DateTime(first_date.Value.Year,
                first_date.Value.Month,
                first_date.Value.Day,
                first_time.Value.Hour,
                first_time.Value.Minute,
                first_time.Value.Second);
            //Верхняя граница
            DateTime upper_date_limit = new DateTime(second_date.Value.Year,
                second_date.Value.Month,
                second_date.Value.Day,
                second_time.Value.Hour,
                second_time.Value.Minute,
                second_time.Value.Second);
            #endregion

            OleDbParameter[] date_param = { new OleDbParameter{Value=lower_date_limit},
                                            new OleDbParameter{Value=upper_date_limit}};

            OleDbDataReader sales_prot_temp_read = DataBaseQuerys.SelectQuary("temp.mdb",
                "SELECT PrCode, PrBarCode, PrName, PrPrice, PrCount, PrChNum FROM SalesProtTemp WHERE PrChNum IN"+
                "(SELECT PTChNum WHERE PTChDate>=? AND PTChDate<=?)", date_param);
            DataBaseQuerys.ListViewFilling(sales_prot_temp_read, sales_list);
            sales_prot_temp_read.Close();
            #endregion
        }

        public void refrash_btn_click(object sender, EventArgs e)
        {
            OleDbDataReader reader = DataBaseQuerys.SelectQuary(global_path,
                "SELECT ItmCode, ItmBarCode, ItmName, ItmPrice, ItmCount, (ItmCount*ItmPrice) as summ, ItmUnit, ItmGroup, ItmDepartm, ItmTaxGrp, ItmCrRest, ItmNullCnt, ItmDateExp, Itm1, Itm2, Itm3 FROM Items",
                null);
           DataBaseQuerys.ListViewFilling(reader, goods_list);
            reader.Close();
        } //+++

        public void goods_export_btn_click(object sender, EventArgs e) //---
        {
        }

        public void database_btn_click(object sender, EventArgs e) //+++
        {
            form2 = new Form2();
            form2.Owner = this;
            form2.ShowDialog();
        }

        public void update_btn_click(object sender, EventArgs e)
        {
            //Запишем значения указаные в фильтре времени в переменные типа DateTime
            #region
            //Нижняя граница
            DateTime lower_date_limit = new DateTime(first_date.Value.Year,
                first_date.Value.Month,
                first_date.Value.Day,
                first_time.Value.Hour,
                first_time.Value.Minute,
                first_time.Value.Second);
            //Верхняя граница
            DateTime upper_date_limit = new DateTime(second_date.Value.Year,
                second_date.Value.Month,
                second_date.Value.Day,
                second_time.Value.Hour,
                second_time.Value.Minute,
                second_time.Value.Second);
            #endregion

            if (local_path.Count==1)
            {
                MessageBox.Show("Необходимо выбрать базу данных");
                return;
            }
        }
    }
}

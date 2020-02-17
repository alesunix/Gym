using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Arcon.GUI;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\sportzal\BaseSportzal\Database2.mdf;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
            textBox5.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) button5_Click(new object(), new EventArgs()); };//Нажатие кнопки "Поиск" с клавиатуры
        }
        private void button1_Click(object sender, EventArgs e)//Добавить
        { try { if (textBox3.Text != "" & comboBox1.Text != "")
                {
                    con.Open();//открыть соединение
                    SqlCommand cmd = new SqlCommand("INSERT INTO [Table] (name, tel, date, summ, monthsumm, vid, visit, allvisit) VALUES (@name, @tel, @date, @summ, @monthsumm, @vid, @visit, @allvisit)", con);
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@tel", textBox2.Text);
                    cmd.Parameters.AddWithValue("@date", DateTime.Today);
                    cmd.Parameters.AddWithValue("@summ", textBox3.Text);
                    cmd.Parameters.AddWithValue("@monthsumm", textBox3.Text);
                    cmd.Parameters.AddWithValue("@vid", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@visit", 0);
                    cmd.Parameters.AddWithValue("@allvisit", 0);
                    cmd.ExecuteNonQuery();
                    con.Close();//закрыть соединение
                    textBox1.Text = "";//очистка текстовых полей
                    textBox2.Text = "";//
                    disp_data();
                    MessageBox.Show("Запись успешно добавлена");
                }
                else if (textBox3.Text == ""){
                    label8.Visible = true;
                    label8.Text = "Вы не ввели оплату";
                }
                else if (comboBox1.Text == ""){
                    label8.Visible = true;
                    label8.Text = "Вы не выбрали вид тренировок";
                }
                else MessageBox.Show("Ошибка");
            }
            catch (Exception ex){
                MessageBox.Show("error: " + ex);
            }
        }
        public void disp_data()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM [Table]";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }
        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)//Событие окрашивания
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)//Цикл
            {
                DateTime X = Convert.ToDateTime(dataGridView1.Rows[i].Cells[3].Value);//По дате
                DateTime Y = DateTime.Today.AddDays(-40);
                if (X <= Y)// если дата меньше текущей на 40 дней
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;//окраска строк в красный цвет
                }
                int S = Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);//По посещениям
                string G = Convert.ToString(dataGridView1.Rows[i].Cells[8].Value);//По виду тренировок
                if (S >= 12 && G == "Обычный")//если посещений 12 и Вид тренировок обычный
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;//окраска строк в красный цвет
                }
                else if (S >= 20 && G == "Crosfit")//если посещений 20 и Вид тренировок Crosfit
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;//окраска строк в красный цвет
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)//Продлить
        { try { if (textBox4.Text != "" & textBox3.Text != "")
                {
                    con.Open();//открыть соединение
                    SqlCommand cmd = new SqlCommand("UPDATE [Table] SET date = @date, summ = summ+ @summ, monthsumm = @monthsumm, visit = @visit WHERE id = @id", con);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.AddDays(+30));
                    cmd.Parameters.AddWithValue("@id", textBox4.Text);
                    cmd.Parameters.AddWithValue("@summ", textBox3.Text);
                    cmd.Parameters.AddWithValue("@monthsumm", textBox3.Text);
                    cmd.Parameters.AddWithValue("@visit", 0);
                    cmd.ExecuteNonQuery();
                    con.Close();//закрыть соединение
                    disp_data();
                    MessageBox.Show("Обонемент продлен");
                    button3.Enabled = false;
                }
                else if (textBox4.Text == ""){
                    label8.Visible = true;
                    label8.Text = "Введите ID абонемента";
                }
                else MessageBox.Show("Ошибка");
            }
            catch (Exception ex){
                MessageBox.Show("error: " + ex);
            }
        }
        private void button6_Click(object sender, EventArgs e)// Доплатить
        { try { if (textBox4.Text != "" & textBox3.Text != "")
                {
                    con.Open();//открыть соединение
                    SqlCommand cmd = new SqlCommand("UPDATE [Table] SET summ = summ+ @summ, monthsumm = monthsumm+ @monthsumm WHERE id = @id", con);
                    cmd.Parameters.AddWithValue("@id", textBox4.Text);
                    cmd.Parameters.AddWithValue("@summ", textBox3.Text);
                    cmd.Parameters.AddWithValue("@monthsumm", textBox3.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();//закрыть соединение
                    disp_data();
                    MessageBox.Show("Доплата успешно произведена");
                }
                else if (textBox4.Text == ""){
                    label8.Visible = true;
                    label8.Text = "Введите ID абонемента";
                }
                else if (textBox3.Text == ""){
                    label8.Visible = true;
                    label8.Text = "Введите оплату";
                }
            }
            catch (Exception ex){
                MessageBox.Show("error: " + ex);
            }
        }
        private void button2_Click(object sender, EventArgs e)//Посещения
        { try { if (textBox4.Text != "")
                    {
                        con.Open();//открыть соединение
                        SqlCommand cmd = new SqlCommand("UPDATE [Table] SET visit = visit+ @visit, allvisit = allvisit+ @allvisit WHERE id = @id", con);
                        cmd.Parameters.AddWithValue("@id", textBox4.Text);
                        cmd.Parameters.AddWithValue("@visit", 1);
                        cmd.Parameters.AddWithValue("@allvisit", 1);
                        cmd.ExecuteNonQuery();
                        con.Close();//закрыть соединение
                        disp_data();
                        MessageBox.Show("Посещение отмечено");
                        button2.Text = "ПОСЕЩЕНИЕ ОТМЕЧЕНО";
                        button2.Enabled = false;
                        //break; // выход из цикла for (true);
                    }
                    else if (textBox4.Text == ""){
                        label8.Visible = true;
                        label8.Text = "Введите ID абонемента";
                    }
                }
                catch (Exception ex){
                    MessageBox.Show("error: " + ex);
                }
            }
        private void button4_Click(object sender, EventArgs e)//Удалить
        { try { if (textBox4.Text != "" && (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("DELETE FROM [Table] WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", textBox4.Text);
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение
                disp_data();
                MessageBox.Show("Запись успешно Удалена");
                }
                else if (textBox4.Text == ""){
                    label8.Visible = true;
                    label8.Text = "Введите ID абонемента";
                }
            }
            catch (Exception ex){
                MessageBox.Show("error: " + ex);
            }
        }
        private void button5_Click(object sender, EventArgs e)//Поиск
        {   
            con.Open();//открыть соединение
            SqlCommand cmd = new SqlCommand("SELECT * FROM [Table] WHERE name LIKE '%' + @name + '%'", con);
            cmd.Parameters.AddWithValue("@name", textBox5.Text);
            cmd.ExecuteNonQuery();
            con.Close();//закрыть соединение
            disp_data();
            int startRowIndex = dataGridView1.CurrentCell.RowIndex;
            int startColumnIndex = dataGridView1.CurrentCell.ColumnIndex;
            int startR = startColumnIndex + 1;
            int R = startR;
            for (int i = startRowIndex; i < dataGridView1.Rows.Count; i++)
            {
                for (R = startR; R < dataGridView1.Columns.Count; R++){
                    if (dataGridView1[R, i].Value != null
                        && dataGridView1[R, i].Value.ToString().ToUpper().Contains(textBox5.Text.ToUpper()))
                    {
                        dataGridView1.CurrentCell = dataGridView1[R, i];
                        return;
                    }
                }
                startR = 0;
            } for (int i = 0; i <= startRowIndex; i++)
            { int finishJ = dataGridView1.Columns.Count;
                if (i == startRowIndex) finishJ = startColumnIndex + 1;
                for (R = 0; R < finishJ; R++){
                    if (dataGridView1[R, i].Value != null
                        && dataGridView1[R, i].Value.ToString().ToUpper().Contains(textBox5.Text.ToUpper()))
                    {
                        dataGridView1.CurrentCell = dataGridView1[R, i];
                        return;
                    }
                }
            } MessageBox.Show("По вашему запросу ничего не найдено");
              textBox5.ForeColor = Color.Red;
        }
        private void button7_Click(object sender, EventArgs e) // вывести общую сумму
        {     
            double balans = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                double incom;
                double.TryParse((row.Cells[5].Value ?? "0").ToString().Replace(".", ","), out incom);
                balans += incom;
            }
            textBox6.Visible = true;
            textBox6.Text = balans.ToString() + " сом";
            button7.Text = "ОБЩАЯ СУММА";
        }
        private void Form1_Load(object sender, EventArgs e)//Форма программы
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "database2DataSet.Table". При необходимости она может быть перемещена или удалена.
            this.tableTableAdapter.Fill(this.database2DataSet.Table);
            disp_data();
            button1.Enabled = textBox1.Text != "";//Включать кнопку при заполнении Имени
            button3.Enabled = textBox3.Text != "";//Включать кнопку при заполнении оплаты
            textBox6.Visible = false;
            label8.Visible = false;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)//Ф.И.О
        {
            button1.Enabled = textBox1.Text != "";//Включать кнопку при заполнении Имени
        }
        private void textBox3_TextChanged(object sender, EventArgs e)//Оплата
        {
            button3.Enabled = textBox3.Text != "";//Включать кнопку при заполнении оплаты  
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)// ссылка на страничку
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/alesunix");
            linkLabel1.BackColor = Color.Red;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button2.Text = "Отметка посещения";
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            button7.Text = "Вывести общую сумму";
        }
    }
    }



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Otel_Otomasyonu
{
    public partial class MusteriKayitfrm : Form
    {
        public MusteriKayitfrm()
        {
            InitializeComponent();
        }

        private void MusteriKayitfrm_Load(object sender, EventArgs e)
        {
            VeriGetir();
            textBox4.MaxLength = 10;
            textBox3.MaxLength = 11;
            
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

            Regex r = new Regex(@"^[0-9]{10}$");
            if (r.IsMatch(textBox4.Text))
            {
                textBox4.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                textBox4.BackColor = System.Drawing.Color.LightPink;

            }

        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Regex t = new Regex(@"^[0-9]{11}$");
            if (t.IsMatch(textBox3.Text))
            {
                textBox3.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                textBox3.BackColor = System.Drawing.Color.LightPink;

            }
        }
        private void VeriGetir()
        {
            DataRepo.bag.Open();
            SqlCommand komut = new SqlCommand("Select * from il", DataRepo.bag);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox2.Items.Add(dr[1].ToString());
            }
            dr.Close();
            DataRepo.bag.Close();

        }


        void TextTemizle()
        {
            foreach (var item in this.Controls) // Form üzerindeki tüm Controllerin bir döngüyle Değişkene atanması
            {
                if (item is TextBox) // Bu değişkene atanan değerler içerisinde Textbox olanların ayıklanması
                {
                    TextBox t = item as TextBox; // Textbox controlünün .Text gibi özelliklerine erişmek için Textbox türünden türetilen nesnemizin itemden gelen textboxların değerini alması
                    t.Clear();  // itemden gelen textlerin Textbox gibi davranacağı  .Clear() özelliği ile temizlenmesi
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool em= isvalid_email(textBox9.Text);
            Regex r = new Regex(@"^[0-9]{10}$");
            Regex t = new Regex(@"^[0-9]{11}$");
            if (r.IsMatch(textBox4.Text) && t.IsMatch(textBox3.Text) && em==true)
            {
                textBox4.BackColor = System.Drawing.Color.LightGreen;
                textBox3.BackColor = System.Drawing.Color.LightGreen;
                SqlCommand komut = new SqlCommand("insert into Musteri values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)", DataRepo.bag);
                komut.Parameters.AddWithValue("@p1", textBox1.Text);
                komut.Parameters.AddWithValue("@p2", textBox2.Text);
                komut.Parameters.AddWithValue("@p3", textBox3.Text);
                komut.Parameters.AddWithValue("@p4", textBox4.Text);
                komut.Parameters.AddWithValue("@p5", textBox5.Text);
                komut.Parameters.AddWithValue("@p6", textBox6.Text);
                komut.Parameters.AddWithValue("@p7", textBox7.Text);
                komut.Parameters.AddWithValue("@p8", textBox8.Text);
                komut.Parameters.AddWithValue("@p9", textBox9.Text);
                komut.Parameters.AddWithValue("@p10", DataRepo.il);
                komut.Parameters.AddWithValue("@p11", DataRepo.ilce);
                DataRepo.bag.Open();

                komut.ExecuteNonQuery();
                DataRepo.bag.Close();

                MessageBox.Show("Müşteri Bilgileri Kaydedildi");
                TextTemizle();
            }
            else
            {
                textBox4.BackColor= System.Drawing.Color.LightPink;
                textBox3.BackColor= System.Drawing.Color.LightPink;
                MessageBox.Show("Lütfen Bilgilerinizi Doğru Giriniz!");
            }
            

        }
        public bool isvalid_email(string email)
        {
            Regex check = new Regex(@"^\w+[\w-\.]+\@\w{5}\.[a-z]{2,3}$");
            bool valid = false;
            valid = check.IsMatch(email);
            if (valid == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                DataRepo.bag.Open();
                SqlCommand komut = new SqlCommand("Select * from il where iller=@p1",DataRepo.bag);
                komut.Parameters.AddWithValue("@p1",comboBox2.Text);
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    SqlCommand da = new SqlCommand("Select * from ilce where il_no=@p1", DataRepo.bag);
                    da.Parameters.AddWithValue("@p1", Convert.ToInt16(dr[0]));
                    SqlDataReader dr2 = da.ExecuteReader();
                    comboBox3.Items.Clear();
                    while (dr2.Read())
                    {
                        comboBox3.Items.Add(dr2[1]);
                    }
                    dr2.Close();
                    DataRepo.il = Convert.ToInt32(dr[0]);
                }
                dr.Close();
                DataRepo.bag.Close();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text != "")
            {
                DataRepo.bag.Open();
                SqlCommand komut2 = new SqlCommand("Select * from ilce where ilceler=@p1", DataRepo.bag);
                komut2.Parameters.AddWithValue("@p1", comboBox3.Text);
                SqlDataReader dr2 = komut2.ExecuteReader();
                if (dr2.Read())
                {
                    DataRepo.ilce = Convert.ToInt32(dr2[0]);
                }
                dr2.Close();
                DataRepo.bag.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmOdalar frmOdalar = new frmOdalar();
            frmOdalar.ShowDialog();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar !=8)
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar !=11)
            {
                e.Handled = true;
            }
        }

     

    }
}

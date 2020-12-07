using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace HarcosokApplication
{
    public partial class Form1 : Form
    {
        MySqlCommand sql = null;
        MySqlConnection conn = null;
        public Form1()
        {
          
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = "localhost";
            sb.UserID = "root";
            sb.Password = "";
            sb.Database = "cs_harcosok";
            sb.CharacterSet = "utf8";

             conn = new MySqlConnection(sb.ToString());
           

            try
            {
                conn.Open();
                sql = conn.CreateCommand();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
            
            hasznaloComboBox.Items.Clear();
            sql.CommandText = "SELECT `nev` FROM `harcosok` ";
            using (MySqlDataReader dr = sql.ExecuteReader())
            {
                while (dr.Read())
                {
                    hasznaloComboBox.Items.Add(dr.GetString("nev"));
                }
            }

            sql.CommandText = "SELECT `nev` FROM `harcosok`";


            using (MySqlDataReader dr = sql.ExecuteReader())
            {
                while (dr.Read())
                {
                    harcosokListBox.Items.Add(dr.GetString("nev"));
                }
            }


        }
        private void letrehoz(string nev) {
       
            sql.CommandText = "INSERT INTO `harcosok`(`id`, `nev`, `letrehozas`) VALUES (null,'" + nev + "', NOW())";
            if (sql.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Sikeres!");


            }
            else
            {
                MessageBox.Show("Nem sikeres!");
            }

            hasznaloComboBox.Items.Clear();
            sql.CommandText = "SELECT `nev` FROM `harcosok` ";
            using (MySqlDataReader dr = sql.ExecuteReader())
            {
                while (dr.Read())
                {
                    hasznaloComboBox.Items.Add(dr.GetString("nev"));
                }
            }

            sql.CommandText = "SELECT `nev` FROM `harcosok`";

            harcosokListBox.Items.Clear();

            using (MySqlDataReader dr = sql.ExecuteReader())
            {
                while (dr.Read())
                {
                    harcosokListBox.Items.Add(dr.GetString("nev"));
                }
            }
        }
        private void letrehozBtn_Click(object sender, EventArgs e)
        {
            letrehoz(harcosNeveTextBox.Text);

        }
        private void kepessegHozzaadas(string nev, string hasznaloneve)
        {
            sql.CommandText = "INSERT INTO `kepessegek`(`id`, `nev`, `leiras`, `harcos_id`) VALUES (DEFAULT,'" + nev + "','" + leirasTextbox.Text + "',(SELECT `id` FROM `harcosok` WHERE `nev` = '" + hasznaloneve + "'))";

            if (hasznaloComboBox.SelectedIndex == -1 || kepessegNeveTextBox.Text.Length == 0 || leirasTextbox.Text.Length == 0)
            {
                MessageBox.Show("Rossz bemenet");
            }
            else
            {
                if (sql.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Sikeres!");


                }
            }
             

        }
        private void kepessegHozzaad_Click(object sender, EventArgs e)
        {
            if (hasznaloComboBox.SelectedIndex == -1 && kepessegNeveTextBox.Text.Length == 0 && leirasTextbox.Text.Length == 0)
            {
                MessageBox.Show("Rossz bemenet");
            }
            else
            {
                kepessegHozzaadas(kepessegNeveTextBox.Text, hasznaloComboBox.GetItemText(hasznaloComboBox.SelectedItem));
            }

        }

        private void harcosokListBox_MouseDoubleClick_lekerdezes() {

            kepessegekListBox.Items.Clear();
            sql.CommandText = "select kepessegek.nev as knev from kepessegek join  harcosok on harcosok.id = kepessegek.harcos_id where (select harcosok.id from harcosok where harcosok.nev = '" + harcosokListBox.GetItemText(harcosokListBox.SelectedItem) + "')";

            using (MySqlDataReader dr = sql.ExecuteReader())
            {
                while (dr.Read())
                {
                    kepessegekListBox.Items.Add(dr.GetString("knev"));
                }
            }
        }
        private void harcosokListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            harcosokListBox_MouseDoubleClick_lekerdezes();
          
        }
        private void kepessegekListBox_MouseDoubleClick_lekerdezes() {
            sql.CommandText = "select leiras from kepessegek where nev = '" + kepessegekListBox.GetItemText(kepessegekListBox.SelectedItem) + "'";
            using (MySqlDataReader dr = sql.ExecuteReader())
            {
                while (dr.Read())
                {
                    kepessegLeirasTextBox.Text = dr.GetString("leiras");
                }
            }
        }
        private void kepessegekListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            kepessegekListBox_MouseDoubleClick_lekerdezes();
        }
       
        private void modosit_Click(object sender, EventArgs e)
        {
            string regiLeiras = kepessegekListBox.Text;
            // sql.CommandText = "   UPDATE `kepessegek` SET `leiras`= '" + ujLeiras + "' WHERE  `nev` = '" + regiLeiras + "' ";
            using (MySqlDataReader dr = sql.ExecuteReader())
            {
                dr.Read();
           
            }
        }
        private void torles_Click_lekerdezes(string torolKepesseg) {
            sql.CommandText = "DELETE FROM `kepessegek` WHERE nev = '" + torolKepesseg + "' ";
            if (kepessegekListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Nincs kiválasztva elem!");
            }
            else
            {

                kepessegekListBox.Items.RemoveAt(kepessegekListBox.SelectedIndex);
                kepessegLeirasTextBox.Text = "";
                using (MySqlDataReader dr = sql.ExecuteReader())
                {
                    dr.Read();
                }

            }
        }
        private void torles_Click(object sender, EventArgs e)
        {

            torles_Click_lekerdezes(kepessegekListBox.GetItemText(kepessegekListBox.SelectedItem));

        }
    }
}

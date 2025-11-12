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
using System.Data;

namespace SASXIIRPL2_RaendraBastianPutra
{
    public partial class Form1 : Form
    {

        string connectionString = "server=localhost;user id=root;password=;database=db_konversi;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
   
            cmbFrom.Items.Add("USD");
            cmbFrom.Items.Add("JPY");
            cmbFrom.Items.Add("EUR");
            cmbFrom.Items.Add("KRW");
            cmbFrom.Items.Add("SGD");



            cmbFrom.SelectedIndex = 0;
        }

        private void cmbFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCurrencyLabels();
            ConvertCurrency();
        }

        private void UpdateCurrencyLabels()
        {
            lblFromCurrency.Text = GetCurrencyName(cmbFrom.Text);
        }

        private string GetCurrencyName(string code)
        {
            switch (code)
            {
                case "USD": return "United States Dollar";
                case "JPY": return "Japanese Yen";
                case "EUR": return "Euro";
                case "KRW": return "South Korean Won";
                case "SGD": return "Singapore Dollar";


                default: return code;
            }
        }

        private void ConvertCurrency()
        {
            
         

           
            if (double.TryParse(txtAmount.Text, out double amount))
            {
                string from = cmbFrom.Text;

                double rate = GetRateFromDatabase(from);
                double result = amount * rate;

                
                long roundedResult = (long)Math.Round(result);

               
                txtResult.Text = roundedResult.ToString("N0");
            }
            
        }


        private double GetRateFromDatabase(string from)
        {
            double rate = 1;
            string query = "SELECT kurs FROM nilai_tukar WHERE singkatan=@from";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@from", from);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        rate = Convert.ToDouble(result);
                    }
                    else
                    {
                        MessageBox.Show("Nilai tukar tidak ditemukan untuk mata uang ini.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengambil nilai tukar: " + ex.Message);
                }
            }

            return rate;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            ConvertCurrency();

        }
    }

}

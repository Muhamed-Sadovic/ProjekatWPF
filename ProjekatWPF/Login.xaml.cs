using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjekatWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        MainWindow registration = new MainWindow();
        AdminPage adminPage = new AdminPage();
        UserPage userPage = new UserPage();
        public Login()
        {
            InitializeComponent();
            string imagePath = "C:\\Users\\MUKI\\source\\repos\\ProjekatWPF\\ProjekatWPF\\slike\\warehouse1.jpeg";

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            imageBrush.Stretch = Stretch.UniformToFill;

            Background = imageBrush;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (tbEmail.Text.Length == 0)
            {
                errormessage.Text = "Unesite email";
                tbEmail.Focus();
            }
            else if (!Regex.IsMatch(tbEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Unesite email u ispravnom formatu";
                tbEmail.Select(0, tbEmail.Text.Length);
                tbEmail.Focus();
            }
            else if (tbLozinka.Password.Length == 0)
            {
                errormessage.Text = "Unesite lozinku";
                tbLozinka.Focus();
            }
            else
            {
                string email = tbEmail.Text;
                string password = tbLozinka.Password;
                SqlConnection conn = new SqlConnection("Data Source=DESKTOP-LG671OJ;Initial Catalog=Skladiste;Integrated Security=True");
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandText = ("SELECT * FROM Korisnik WHERE Email='" + email + "' AND Lozinka='" + password +"'");
                //cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    string uloga = dataSet.Tables[0].Rows[0]["Uloga"].ToString();
                    if (uloga == "Admin")
                    {
                        adminPage.Show();
                        Close();
                    }
                    else
                    {
                        userPage.Show();
                        Close();
                    }
                }
                else
                {
                    errormessage.Text = $"Unesite vazeci email/lozinku!";
                    return;
                    
                }
                conn.Close();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            registration.Show();  
            Close(); 
        }
    }
}

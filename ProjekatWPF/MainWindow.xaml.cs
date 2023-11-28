using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjekatWPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Korisnik korisnik;
        public Korisnik Korisnik
        {
            get { return korisnik; }
            set
            {
                korisnik = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Korisnik> Korisnici { get; set; }
        public DB DB { get; set; }
        public MainWindow()
        {
            DB = new DB();
            Korisnik = new Korisnik();
            Korisnici = new ObservableCollection<Korisnik>();
            InitializeComponent();
            string imagePath = "C:\\Users\\MUKI\\source\\repos\\ProjekatWPF\\ProjekatWPF\\slike\\warehouse1.jpeg";

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            imageBrush.Stretch = Stretch.UniformToFill;

            Background = imageBrush;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            tbIme.Text = "";
            tbPrezime.Text = "";
            tbEmail.Text = "";
            tbLozinka.Password = "";
            tbPotvrdaLozinke.Password = "";
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (tbIme.Text.Length == 0)
            {
                errormessage.Text = "Unesite ime.";
                tbIme.Focus();
            }
            else if (tbPrezime.Text.Length == 0)
            {
                errormessage.Text = "Unesite prezime.";
                tbPrezime.Focus();
            }
            else if (tbEmail.Text.Length == 0)
            {
                errormessage.Text = "Unesite email.";
                tbEmail.Focus();
            }
            else if (!Regex.IsMatch(tbEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Unesite email u ispravnom formatu.";
                tbEmail.Select(0, tbEmail.Text.Length);
                tbEmail.Focus();
            }
            else
            {
                string password = tbLozinka.Password;
                if (tbLozinka.Password.Length == 0)
                {
                    errormessage.Text = "Unesite lozinku";
                    tbLozinka.Focus();
                }
                else if (tbPotvrdaLozinke.Password.Length == 0)
                {
                    errormessage.Text = "Unesite potvrdu lozinke";
                    tbPotvrdaLozinke.Focus();
                }
                else if (tbLozinka.Password != tbPotvrdaLozinke.Password)
                {
                    errormessage.Text = "Lozinke moraju da se poklapaju";
                    tbPotvrdaLozinke.Focus();
                }
                else
                {
                    if (Korisnici.Any(k => k.Email != tbEmail.Text))
                    {
                        MessageBox.Show("Ovaj korisnik već postoji");
                        return;
                    }
                    else
                    {
                        Korisnik.Lozinka = password;
                        DB.CreateKorisnik(Korisnik);
                        Korisnici.Add(Korisnik);
                        Login login = new Login();
                        login.Show();
                        this.Close();
                    }
                }
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();  
            login.Show();  
            Close();
        }

    }
}

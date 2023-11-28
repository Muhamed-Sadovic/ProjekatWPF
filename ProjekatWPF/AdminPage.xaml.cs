using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjekatWPF
{
    public partial class AdminPage : Window, INotifyPropertyChanged
    {
        private Artikal artikal;
        public Artikal Artikal
        {
            get { return artikal; }
            set
            {
                artikal = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Artikal> Artikli { get; set; }
        public DB DB { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public AdminPage()
        {
            DB = new DB();
            Artikal = new Artikal();
            Artikli = new ObservableCollection<Artikal>();
            LoadData();           
            InitializeComponent();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (imee.Text == string.Empty)
            {
                MessageBox.Show("Unesite naziv artikla za brisanje ili kliknite da artikal u listi koji zelite za uklonite iz skladista");
                return;
            }
            DB.DeleteArtikal(Artikal);
            LoadData();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Artikal a = new Artikal();
            if (kolicinaa.Text == string.Empty || cenaa.Text == string.Empty)
            {
                MessageBox.Show("Unesite sve parametre.");
                return;
            }        
            string ime = imee.Text;
            string kategorija = kategorijaa.Text;
            int kolicina = int.Parse(kolicinaa.Text);
            float cena = float.Parse(cenaa.Text);
            a.Ime = ime;
            a.Kolicina = kolicina;
            a.Cena = cena;
            a.Kategorija = kategorija;
            if (ime.Length == 0 || kategorija.Length == 0 || kolicina == 0 || cena == 0)
            {
                MessageBox.Show("Unesite sve parametre.");
                return;
            }
            else
            {
                DB.CreateArtikal(a);
                LoadData();
            }    
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (imee.Text == string.Empty)
            {
                MessageBox.Show("Kiknite da artikal u listi koji zelite da azurirate");
                return;
            }

            DB.UpdateArtikal(Artikal);
            LoadData();
            imee.Text = kategorijaa.Text = cenaa.Text = kolicinaa.Text = string.Empty;
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            //LoadData();
        }

        private void LoadData()
        {
            var dbArtikli = DB.GetArtikli();
            Artikli.Clear();
            foreach (var art in dbArtikli)
            {
                Artikli.Add(art);
            }
        }
        private void PretraziButton_Click(object sender, RoutedEventArgs e)
        {
            string ime = ImeTextBox.Text;
            string kategorija = KategorijaTextBox.Text;
            int minimalnaKolicina;
            if (!int.TryParse(MinimalnaKolicinaTextBox.Text, out minimalnaKolicina))
            {
                minimalnaKolicina = 0;
            }
            float maksimalnaCena;
            if (!float.TryParse(MaksimalnaCenaTextBox.Text, out maksimalnaCena))
            {
                maksimalnaCena = 0;
            }

            if (string.IsNullOrEmpty(ime) && string.IsNullOrEmpty(kategorija) && minimalnaKolicina == 0 && maksimalnaCena == 0)
            {
                MessageBox.Show("Unesite parametre za pretragu.");
                return;
            }

            List <Artikal> rezultat = DB.PretraziArtikle(ime, minimalnaKolicina, maksimalnaCena, kategorija);

            RezultatListBox.ItemsSource = rezultat;
        }

        private void generisiIzvestaj_Click(object sender, RoutedEventArgs e)
        {
            List<string> sveKategorije = DB.GetSveKategorije();
            List<string> kategorijeSaBrojem = new List<string>();

            foreach (string kategorija in sveKategorije)
            {
                int brojArtikala = DB.GetBrojArtikalaPoKategoriji(kategorija);
                string kategorijaSaBrojem = $"{kategorija} - {brojArtikala}";
                kategorijeSaBrojem.Add(kategorijaSaBrojem);
            }
            kategorijeSaBrojem.Sort((x, y) =>
            {
                int brojArtikalaX = int.Parse(x.Split('-')[1].Trim());
                int brojArtikalaY = int.Parse(y.Split('-')[1].Trim());
                return brojArtikalaY.CompareTo(brojArtikalaX);
            });
            izvestaj.ItemsSource = kategorijeSaBrojem;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }
    }
}

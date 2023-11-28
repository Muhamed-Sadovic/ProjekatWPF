using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Window, INotifyPropertyChanged
    {
        public UserPage()
        {
            DB = new DB();
            Artikal = new Artikal();
            Artikli = new ObservableCollection<Artikal>();
            LoadData();
            InitializeComponent();
        }
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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

            List<Artikal> rezultat = DB.PretraziArtikle(ime, minimalnaKolicina, maksimalnaCena, kategorija);

            RezultatListBox.ItemsSource = rezultat;

        }
    }
}

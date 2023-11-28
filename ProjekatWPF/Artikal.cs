using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatWPF
{
    public class Artikal
    {
        private String ime;
        private int kolicina;
        private float cena;
        private String kategorija;

        public String Ime
        {
            get { return ime; }
            set { ime = value; }
        }
        public int Kolicina
        {
            get { return kolicina; }
            set { kolicina = value; }
        }
        public float Cena
        {
            get { return cena; }
            set { cena = value; }
        }
        public String Kategorija
        {
            get { return kategorija; }
            set { kategorija = value; }
        }
        public Artikal()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatWPF
{
    public class Korisnik
    {
		private String ime;
        private String prezime;
        private String email;
        private String lozinka;
        private String uloga;
        public String Ime
		{
			get { return ime; }
			set { ime = value; }
		}		
		public String Prezime
		{
			get { return prezime; }
			set { prezime = value; }
		}		
		public String Email
		{
			get { return email; }
			set { email = value; }
		}		
		public String Lozinka
		{
			get { return lozinka; }
			set { lozinka = value; }
		}
		public String Uloga
		{
			get { return uloga; }
			set { uloga = value; }
		}

		public Korisnik()
		{

		}
	}
}

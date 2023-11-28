using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProjekatWPF
{
    public class DB
    {
        SqlConnection conn;
        SqlCommand cmd;
        public DB()
        {
            conn = new SqlConnection("Data Source=DESKTOP-LG671OJ;Initial Catalog=Skladiste;Integrated Security=True");
            cmd = conn.CreateCommand();
        }

        public void CreateKorisnik(Korisnik k)
        {
            try
            {
                conn.Open();
                cmd.CommandText = $"INSERT INTO [dbo].[Korisnik] ([Ime], [Prezime], [Email], [Lozinka], [Uloga]) " +
                    $"VALUES('{k.Ime}','{k.Prezime}','{k.Email}','{k.Lozinka}','Korisnik')";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Uspesna registracija. Sada se mozete ulogovati na nasu aplikaciju");

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public List<Artikal> GetArtikli()
        {
            List<Artikal> artikli = new List<Artikal>();
            try
            {
                conn.Open();
                cmd.CommandText = "SELECT * FROM Artikal";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    artikli.Add(new Artikal()
                    {
                        Ime = reader["Ime"].ToString(),
                        Kolicina = int.Parse(reader["Kolicina"].ToString()),
                        Cena = int.Parse(reader["Cena"].ToString()),
                        Kategorija = reader["Kategorija"].ToString(),
                    });
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                if (conn != null)
                {
                    conn.Close();
                }
                MessageBox.Show(ex.Message);
            }
            return artikli;
        }

        public void CreateArtikal(Artikal artikal)
        {
            try
            {
                conn.Open();              
                cmd.CommandText = $"INSERT INTO [dbo].[Artikal] ([Ime], [Kolicina], [Cena], [Kategorija]) " +
                    $"VALUES('{artikal.Ime}','{artikal.Kolicina}','{artikal.Cena}','{artikal.Kategorija}')";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Uspesno dodavanje artikla");
            }
            catch (SqlException ex)
            {
               MessageBox.Show($"Artikal {artikal.Ime} vec postoji u skladistu ");
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
        public void UpdateArtikal(Artikal artikal)
        {
            try
            {
                conn.Open();
                cmd.CommandText = $"UPDATE Artikal SET Kolicina='{artikal.Kolicina}',Cena='{artikal.Cena}' WHERE Kategorija='{artikal.Kategorija}' AND Ime='{artikal.Ime}'";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Uspesno izmenjeni podaci o artiklu");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
        public void DeleteArtikal(Artikal artikal)
        {
            try
            {
                if (MessageBox.Show($"Da li ste sigurni da zelite da uklonite artikal {artikal.Ime}?", "Potvrda", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    conn.Open();
                    cmd.CommandText = $"DELETE FROM Artikal WHERE Ime='{artikal.Ime}'";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Uspesno uklanjanje artikla iz skladista");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public List<Artikal> PretraziArtikle(string ime, int minimalnaKolicina, float maksimalnaCena, string kategorija)
        {
            List<Artikal> rezultat = new List<Artikal>();
            try
            {
                conn.Open();
                StringBuilder query = new StringBuilder("SELECT * FROM Artikal WHERE 1 = 1");

                if (!string.IsNullOrEmpty(ime))
                {
                    query.Append(" AND Ime LIKE @ime");
                    cmd.Parameters.AddWithValue("@ime", "%" + ime + "%");
                }

                if (!string.IsNullOrEmpty(kategorija))
                {
                    query.Append(" AND Kategorija LIKE @kategorija");
                    cmd.Parameters.AddWithValue("@kategorija", "%" + kategorija + "%");
                }

                if (minimalnaKolicina > 0)
                {
                    query.Append(" AND Kolicina <= @minimalnaKolicina");
                    cmd.Parameters.AddWithValue("@minimalnaKolicina", minimalnaKolicina);
                }

                if (maksimalnaCena > 0)
                {
                    query.Append(" AND Cena <= @maksimalnaCena");
                    cmd.Parameters.AddWithValue("@maksimalnaCena", maksimalnaCena);
                }

                cmd.CommandText = query.ToString();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rezultat.Add(new Artikal()
                    {
                        Ime = reader["Ime"].ToString(),
                        Kolicina = int.Parse(reader["Kolicina"].ToString()),
                        Cena = float.Parse(reader["Cena"].ToString()),
                        Kategorija = reader["Kategorija"].ToString(),
                    });
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                if (conn != null)
                {
                    conn.Close();
                }
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cmd.Parameters.Clear();
            }
            return rezultat;
        }

        public List<string> GetSveKategorije()
        {
            List<string> kategorije = new List<string>();
            try
            {
                conn.Open();
                //string query = "SELECT DISTINCT Kategorija FROM Artikal";
                //SqlCommand cmd = new SqlCommand(query, conn);

                cmd.CommandText = "SELECT DISTINCT Kategorija FROM Artikal";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string kategorija = reader["Kategorija"].ToString();
                    kategorije.Add(kategorija);
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                if (conn != null)
                {
                    conn.Close();
                }
                MessageBox.Show(ex.Message);
            }
            return kategorije;
        }
        public int GetBrojArtikalaPoKategoriji(string kategorija)
        {
            int brojArtikala = 0;
            try
            {              
                conn.Open();
                string query = "SELECT COUNT(*) FROM Artikal WHERE Kategorija = @Kategorija";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Kategorija", kategorija);
                    brojArtikala = Convert.ToInt32(cmd.ExecuteScalar());
                }
                conn.Close();              
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return brojArtikala;
        }

    }
}

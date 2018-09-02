using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BonniModel.Interfaces;
using BonnyUI.Model;

namespace BonnyUI.DBConnection
{
    public class DBConnector : IDBConnector
    {
        #region private fields
        private IList<IShop> _shops;
        private IList<ICategory> _categories;
        private IList<IReceipt> _bons;
        private SqlConnection con;
        #endregion

        #region ctor
        public DBConnector()
        {
            ConnectionStringSettings setting = null;
            
            #if (DEBUG)
            setting = ConfigurationManager.ConnectionStrings["SQL2016Dev"];
            #endif

            //Prüfen, ob Config-Datei den Eintrag > SQL2016DEV < enthält
            if (setting == null)
            {
                setting = ConfigurationManager.ConnectionStrings["SQL2016Prod"];
            }
            con = new SqlConnection(setting.ConnectionString);

            _shops = new List<IShop>();

            _bons = new List<IReceipt>();

            _categories = new List<ICategory>();
        }
        #endregion


        public int AddShop(string name)
        {
            int retval = 0;
            string sql = 
                "INSERT INTO Geschaeft(Name) OUTPUT INSERTED.Id VALUES('" + name + "')";
            
            try
            {
                con.Open();
                var cmd = new SqlCommand(sql, con);
                retval = (int)cmd.ExecuteScalar();                
            }
            catch
            {
                throw new Exception("Shop-Hinzufügen fehlgeschlagen");
            }
            con.Close();
            return retval;
        }

        public void ChangeShop(IShop shop)
        {
            string sql = "UPDATE Geschaeft SET Name = '" + shop.Name + "'Where ID = " + shop.ID.ToString();
            try
            {
                con.Open();
                var cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Shop-Update fehlgeschlagen");
            }
            con.Close();
        }

        public int AddCategory(string name)
        {
            int retval = 0;
            string sql = "INSERT INTO Kategorien(Kategorie) OUTPUT INSERTED.Id VALUES('" + name + "')";

            try
            {
                con.Open();
                var cmd = new SqlCommand(sql, con);
                retval = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Hinzfügen der Kategorie fehlgeschlagen", ex);
            }
            con.Close();
            return retval;
        }

        public void ChangeCategory(ICategory category)
        {
            string sql = "UPDATE Kategorien SET Kategorie = '" + category.Name + "'Where ID = " + category.ID.ToString();
            try
            {
                con.Open();
                var cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Kategorie-Update fehlgeschlagen");
            }
            con.Close();
        }

        public IList<IShop> GetAllShops()
        {
            IList<IShop> retval = new List<IShop>();
            try
            {
                con.Open();
                retval = ConnectionOpenendSoGetAllShops();
            }

            catch(Exception ex)
            {
                throw new Exception("Ciao", ex);
            }

            //reader.Close();
            con.Close();

            retval = retval.OrderBy(x => x.Name).ToList();
            _shops = retval;
            
            return retval;
        }

        public IList<ICategory> GetAllCategories()
        {
            IList<ICategory> retval = new List<ICategory>();
            try
            {
                con.Open();
                retval = ConnectionOpenendSoGetAllCategories();
            }

            catch (Exception ex)
            {
                throw new Exception("Ciao", ex);
            }

            //reader.Close();
            con.Close();

            retval = retval.OrderBy(x => x.Name).ToList();
            _categories = retval;

            return retval;
        }



        public IList<IReceipt> GetAllBons()
        {
            IList<IReceipt> retval = new List<IReceipt>();
            string strSQLBon = "SELECT * FROM Bon";
            SqlDataReader reader;
            try
            {
                con.Open();
                var cmd1 = new SqlCommand(strSQLBon, con);
                reader = cmd1.ExecuteReader();

                while (reader.Read())
                {
                    DateTime? payDate = reader["Datum"] as DateTime?;
                    IShop shop = _shops.Where(x => x.ID.Equals((int)reader["idGeschaeft"])).FirstOrDefault();
                    double amount = Convert.ToDouble(reader["Gesamtbetrag"]);
                    string details = reader["Details"] as string;
                    string paymentType = reader["Zahlungstyp"] as string;
                    string user = reader["Benutzer"] as string;
                    int id = (int)reader["ID"];
                    DateTime? settlementDate = reader["Abrechnungsdatum"] as DateTime?;
                    bool settled = (bool)reader["Abgerechnet"];
                    
                    IList<IPayment> zahlungen = new List<IPayment>();

                    IReceipt bon = new Receipt(id, user, payDate, paymentType, details, amount, zahlungen, shop, settled, settlementDate);
                    retval.Add(bon);
                }
                reader.Close();
                foreach(IReceipt bon in retval)
                {
                    bon.Categories = GetAllCategoriesToBon(bon);
                    bon.Payments = GetAllPaymentsToBon(bon);
                }
            }
            
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Laden der Bons aus der Datenbank", ex);
            }
            finally
            {

                con.Close();
            }
            reader.Close();

            return retval;
        }

        public int SaveBon(IReceipt bon)
        {
            string strSQL;
            if (bon.ID == null)
                strSQL = "INSERT INTO Bon(idGeschaeft, Datum, Details, Zahlungstyp, Benutzer, Abgerechnet, Gesamtbetrag)" +
                    " OUTPUT INSERTED.Id VALUES('" +
                    bon.Shop.ID + "', '" + bon.PayDate + "', '" + bon.Details + "', '" + bon.PaymentType + "', '" + bon.User + "', '" + bon.Settled + "', '" +
                    bon.Amount.ToString().Replace(",", ".") + 
                    "')";
            else
                strSQL = "UPDATE Bon SET "
                + "idGeschaeft = '" + bon.Shop.ID.ToString()
                + "', Datum = '" + bon.PayDate
                + "', Details = '" + bon.Details
                + "', Zahlungstyp = '" + bon.PaymentType
                + "', Benutzer = '" + bon.User
                + "', Abgerechnet = '" + bon.Settled
                + "', Gesamtbetrag = '" + bon.Amount.ToString().Replace(",", ".")
                + "' Where ID = " + bon.ID.ToString();

            try
            {
                con.Open();
                var cmd = new SqlCommand(strSQL, con);
                if (bon.ID == null)
                {
                    int id = (int)cmd.ExecuteScalar();
                    bon.ID = id;
                }
                else
                    cmd.ExecuteNonQuery();



                foreach (IPayment zahlung in bon.Payments)
                    this.SaveNewPayment(bon.ID, zahlung);

                this.SaveCategoriesToBon(bon);
            }
            catch
            {
                throw new NotSupportedException("Hallo");
            }
            con.Close();

            if (bon.ID != null)
                return (int)bon.ID;
            else
                return -1;
        }


        public int BalanceBon(IReceipt bon)
        {
            string strSQL;
            if (bon.ID == null)
                strSQL = "INSERT INTO Bon(idGeschaeft, Datum, Details, Zahlungstyp, Benutzer, Abgerechnet, Gesamtbetrag, Abrechnungsdatum)" +
                    " OUTPUT INSERTED.Id VALUES('" +
                    bon.Shop.ID + "', '" + bon.PayDate + "', '" + bon.Details + "', '" + bon.PaymentType + "', '" + bon.User + "', '" + bon.Settled + "', '" + bon.Amount.ToString().Replace(",", ".") +
                    "', '" + DateTime.Now + "')";
            else
                strSQL = "UPDATE Bon SET "
                + "idGeschaeft = '" + bon.Shop.ID.ToString()
                + "', Datum = '" + bon.PayDate
                + "', Details = '" + bon.Details
                + "', Zahlungstyp = '" + bon.PaymentType
                + "', Benutzer = '" + bon.User
                + "', Abgerechnet = '" + bon.Settled
                + "', Gesamtbetrag = '" + bon.Amount.ToString().Replace(",", ".")
                + "', Abrechnungsdatum = '" + DateTime.Today
                + "' Where ID = " + bon.ID.ToString();

            try
            {
                con.Open();
                var cmd = new SqlCommand(strSQL, con);
                if (bon.ID == null)
                {
                    int id = (int)cmd.ExecuteScalar();
                    bon.ID = id;

                }
                else
                    cmd.ExecuteNonQuery();



                foreach (IPayment zahlung in bon.Payments)
                    this.SaveNewPayment(bon.ID, zahlung);
            }
            catch
            {
                throw new NotSupportedException("Hallo");
            }
            con.Close();

            if (bon.ID != null)
                return (int)bon.ID;
            else
                return -1;
        }
        
        private IList<ICategory> GetAllCategoriesToBon(IReceipt bon)
        {
            string sql = "SELECT kat.Kategorie, kat.ID FROM [BonKategorien] zuordnung join[Kategorien] kat on zuordnung.ID_Kategorie = kat.ID WHERE zuordnung.ID_Bon = " + bon.ID.ToString();
            IList<ICategory> retval = new List<ICategory>();
            SqlDataReader reader;
            try
            {   
                var cmd = new SqlCommand(sql, con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var b = reader["Kategorie"];
                    string name = b as string;

                    var id = reader["ID"];
                    int idAlsInt = (int)id;
                    
                    retval.Add(_categories.Where(x => x.ID.Equals(idAlsInt)).FirstOrDefault());
                }
                
                reader.Close();
                
            }
            catch (Exception ex)
            {
                throw new Exception("Fail", ex);
            }
            retval = retval.OrderBy(x => x.Name).ToList();
            return retval;
        }

        private IList<ICategory> GetAllCategoriesToPayment(IPayment payment)
        {
            string sql = "SELECT kat.Kategorie, kat.ID FROM [ZahlungKategorien] zuordnung join[Kategorien] kat on zuordnung.ID_Kategorie = kat.ID WHERE zuordnung.ID_Zahlung = " + payment.ID.ToString();
            IList<ICategory> retval = new List<ICategory>();
            SqlDataReader reader;
            try
            {
                var cmd = new SqlCommand(sql, con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var b = reader["Kategorie"];
                    string name = b as string;

                    var id = reader["ID"];
                    int idAlsInt = (int)id;


                    
                    retval.Add(_categories.Where(x => x.ID.Equals(idAlsInt)).FirstOrDefault());
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Fail", ex);
            }
            retval = retval.OrderBy(x => x.Name).ToList();
            return retval;
        }

        private IList<IPayment> GetAllPaymentsToBon(IReceipt bon)
        {
            
            string strSQL = "SELECT z.* from Zahlungen z join Bon b on b.ID = z.idBon WHERE z.idBon = " + bon.ID.ToString();
            IList<IPayment> retval = new List<IPayment>();
            SqlDataReader reader;
            try
            {
                var cmd = new SqlCommand(strSQL, con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader["ID"];
                    int idAlsInt = (int)id;

                    var c = reader["Zahlungstyp"];
                    string paymentType = c as string;

                    double amount = Convert.ToDouble(reader["Betrag"]);
                    string details = reader["Details"] as string;

                    IPayment payment = new Payment(idAlsInt, amount, details, paymentType);
                    retval.Add(payment);
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Fail", ex);
            }            

            foreach(IPayment payment in retval)
            {
                payment.Categories = GetAllCategoriesToPayment(payment);
            }
            return retval;
        }
        
        private void SaveNewPayment(int? bonId, IPayment payment)
        {
            string strSQL;
            if (payment.ID == null)
                strSQL = "INSERT INTO Zahlungen(idBon, Details, Zahlungstyp, Betrag)" +
                    " OUTPUT INSERTED.Id VALUES('" +
                    bonId + "', '" + payment.Details + "', '" + payment.PaymentType + "', '" + payment.Amount.ToString().Replace(",", ".") + "')";
            else
                strSQL = "UPDATE Zahlungen SET "
                + "idBon = '" + bonId
                + "', Details = '" + payment.Details
                + "', Zahlungstyp = '" + payment.PaymentType
                + "', Betrag = '" + payment.Amount.ToString().Replace(",", ".")
                + "' Where ID = " + payment.ID.ToString();

            try
            {
                var cmd = new SqlCommand(strSQL, con);
                if (payment.ID == null)
                {
                    int id = (int)cmd.ExecuteScalar();
                    payment.ID = id;
                }
                else
                    cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new NotSupportedException("Hallo");
            }

            SaveCategoriesToPayment(payment);
        }

        private void SaveCategoriesToBon(IReceipt bon)
        {
            // Delete:
            string delCat = "DELETE FROM BonKategorien WHERE ID_Bon = " + bon.ID.ToString();
            // SqlDataReader reader;
            try
            {
                SqlCommand cmd = new SqlCommand(delCat, con);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Fehler beim LÖschen der Kategorien zu Bon Nr. " + bon.ID.ToString());
            }

            // Insert:
            string insertSQL;
            foreach (ICategory cat in bon.Categories)
            {
                insertSQL = "INSERT INTO BonKategorien ([ID_Bon] ,[ID_Kategorie]) Values (" + bon.ID.ToString() + ", " + cat.ID.ToString() + ")";

                try
                {
                    var cmd = new SqlCommand(insertSQL, con);

                    cmd.ExecuteNonQuery();                    
                }
                catch
                {
                    throw new Exception("Fehler beim Hinzufügen der Kategorien zu Bon Nr. " + bon.ID.ToString());
                }
            }
        }

        private void SaveCategoriesToPayment(IPayment payment)
        {
            // Delete:
            string delCat = "DELETE FROM ZahlungKategorien WHERE ID_Zahlung = " + payment.ID.ToString();
            // SqlDataReader reader;
            try
            {
                SqlCommand cmd = new SqlCommand(delCat, con);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Fehler beim LÖschen der Kategorien zu Bon Nr. " + payment.ID.ToString());
            }

            // Insert:
            string insertSQL;
            foreach (ICategory cat in payment.Categories)
            {
                insertSQL = "INSERT INTO ZahlungKategorien ([ID_Zahlung] ,[ID_Kategorie]) Values (" + payment.ID.ToString() + ", " + cat.ID.ToString() + ")";

                try
                {
                    var cmd = new SqlCommand(insertSQL, con);

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw new Exception("Fehler beim Hinzufügen der Kategorien zu Bon Nr. " + payment.ID.ToString());
                }
            }
        }



        private IList<ICategory> ConnectionOpenendSoGetAllCategories()
        {
            IList<ICategory> retval = new List<ICategory>();

            string strSQL = "SELECT * FROM Kategorien";

            var cmd = new SqlCommand(strSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    var b = reader["Kategorie"];
                    string name = b as string;

                    var id = reader["ID"];
                    int idAlsInt = (int)id;
                    
                    ICategory geschäft = new Category(idAlsInt, name);

                    retval.Add(geschäft);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Hallo Welt", ex);
            }
            reader.Close();
            return retval;
        }

        private IList<IShop> ConnectionOpenendSoGetAllShops()
        {
            IList<IShop> retval = new List<IShop>();

            string strSQL = "SELECT * FROM Geschaeft";

            var cmd = new SqlCommand(strSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    var b = reader["Name"];
                    string name = b as string;

                    var id = reader["ID"];
                    int idAlsInt = (int)id;



                    IShop geschäft = new Shop(idAlsInt, name);

                    retval.Add(geschäft);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Hallo Welt", ex);
            }
            reader.Close();
            return retval;
        }
    }
}

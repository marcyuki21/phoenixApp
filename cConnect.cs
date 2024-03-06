using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace phoenixApp
{
    class cConnect
    {
        public MySqlConnection mycon;
        public MySqlCommand mycom;
        public MySqlDataReader myread;
        public MySqlDataAdapter myadapt;
        public string connectionFurukawa = "Server=192.168.4.136;Port=3309;Database=furukawa_bak;User ID=root;Password=123;";
        public string connectionFurukawa2 = "Server=192.168.4.136;Port=3309;Database=furukawa;User ID=root;Password=123;";
        public string connectionFormal = "Server=192.168.4.136;Port=3309;Database=formal;User ID=root;Password=123;";
        public string connection137 = "Server=192.168.4.137;Port=3306;Database=hs;User ID=root;Password=pass1234;";
        public DataSet gtable(string date)
        {
            DataSet ds = new DataSet();
            string query = "select * from phoenixchecker where convert(date_process,datetime) between '" + date + " 00:00:00' and '" + date + " 23:59:59' order by convert(date_process,datetime) desc";
            try
            {
                mycon = new MySqlConnection(connectionFurukawa);
                mycon.Open();
                myadapt = new MySqlDataAdapter(query, mycon);
                myadapt.Fill(ds);

                mycon.Close();
                mycon.Dispose();


            }
            catch (Exception)
            {
                mycon.Close();
                mycon.Dispose();
                throw;


            }

            return ds;
        }
        public DataSet gtable2(string serial)
        {
            DataSet ds = new DataSet();
            string query = "select oldbarcode,newbarcode from barcode_to_barcode where newbarcode = '" + serial + "'";
            try
            {
                mycon = new MySqlConnection(connectionFurukawa2);
                mycon.Open();
                myadapt = new MySqlDataAdapter(query, mycon);
                myadapt.Fill(ds);

                mycon.Close();
                mycon.Dispose();


            }
            catch (Exception)
            {
                mycon.Close();
                mycon.Dispose();
                throw;


            }

            return ds;
        }
        public DataSet gthermal(string barcode)
        {
            DataSet ds = new DataSet();
            string query = "select * from thermal_xena where barcode = '" + barcode + "' order by inspection_time desc ";
            try
            {
                mycon = new MySqlConnection(connectionFormal);
                mycon.Open();
                myadapt = new MySqlDataAdapter(query, mycon);
                myadapt.Fill(ds);

                mycon.Close();
                mycon.Dispose();


            }
            catch (Exception)
            {
                mycon.Close();
                mycon.Dispose();
                throw;


            }

            return ds;
        }
        public void iData(string query)
        {
            try
            {
                mycon = new MySqlConnection(connectionFurukawa);
                mycon.Open();
                mycom = new MySqlCommand(query, mycon);
                mycom.ExecuteNonQuery();
                mycon.Close();
                mycon.Dispose();
            }
            catch (Exception)
            {
                mycon.Close();
                mycon.Dispose();

                throw;
            }



        }

        public void uData(string query)
        {
            try
            {
                mycon = new MySqlConnection(connectionFurukawa);
                mycon.Open();
                mycom = new MySqlCommand(query, mycon);
                mycom.ExecuteNonQuery();
                mycon.Close();
                mycon.Dispose();
            }
            catch (Exception)
            {
                mycon.Close();
                mycon.Dispose();

                throw;
            }



        }
        public string gString(string query)
        {
            string ds = string.Empty;
            try
            {
                mycon = new MySqlConnection(connectionFormal);
                mycon.Open();
                mycom = new MySqlCommand(query, mycon);
                myread = mycom.ExecuteReader();
                while (myread.Read())
                {
                    ds = myread.GetValue(0).ToString();
                }
                mycon.Close();
                mycon.Dispose();

            }
            catch (Exception)
            {
                mycon.Close();
                mycon.Dispose();
                throw;
            }

            return ds;



        }
        public string gStringoldB(string query)
        {
            string ds = string.Empty;
            try
            {
                mycon = new MySqlConnection(connectionFurukawa2);
                mycon.Open();
                mycom = new MySqlCommand(query, mycon);
                myread = mycom.ExecuteReader();
                while (myread.Read())
                {
                    ds = myread.GetValue(0).ToString();
                }
                mycon.Close();
                mycon.Dispose();

            }
            catch (Exception)
            {
                mycon.Close();
                mycon.Dispose();
                throw;
            }

            return ds;



        }
        public string gflatness(string query)
        {
            string ds = string.Empty;
            try
            {
                mycon = new MySqlConnection(connection137);
                mycon.Open();
                mycom = new MySqlCommand(query, mycon);
                myread = mycom.ExecuteReader();
                while (myread.Read())
                {
                    ds = myread.GetValue(0).ToString();
                }
                mycon.Close();
                mycon.Dispose();

            }
            catch (Exception)
            {
                mycon.Close();
                mycon.Dispose();
                throw;
            }

            return ds;



        }
        public string gStringFurukawa(string query)
        {
            string ds = string.Empty;
            try
            {
                mycon = new MySqlConnection(connectionFurukawa);
                mycon.Open();
                mycom = new MySqlCommand(query, mycon);
                myread = mycom.ExecuteReader();
                while (myread.Read())
                {
                    ds = myread.GetValue(0).ToString();
                }
                mycon.Close();
                mycon.Dispose();

            }
            catch (Exception)
            {
                mycon.Close();
                mycon.Dispose();
                throw;
            }

            return ds;
        }
        public Boolean dExistflatness(string query)
        {
            Boolean ds = false;
            try
            {
                mycon = new MySqlConnection(connection137);
                mycon.Open();
                mycom = new MySqlCommand(query, mycon);
                myread = mycom.ExecuteReader();
                while (myread.Read())
                {
                    ds = true;
                }
                mycon.Close();
                mycon.Dispose();

            }
            catch (Exception)
            {
                mycon.Close();
                mycon.Dispose();
                throw;
            }

            return ds;



        }
        public Boolean dExiest(string query)
        {
            Boolean ds = false;
            try
            {
                mycon = new MySqlConnection(connectionFurukawa);
                mycon.Open();
                mycom = new MySqlCommand(query, mycon);
                myread = mycom.ExecuteReader();
                while (myread.Read())
                {
                    ds = true;
                }
                mycon.Close();
                mycon.Dispose();

            }
            catch (Exception)
            {
                mycon.Close();
                mycon.Dispose();
                throw;
            }

            return ds;



        }
    }
}

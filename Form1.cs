using Cognex.DataMan.SDK;
using phoenixApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace phoenixApp
{
    public partial class Form1 : Form
    {
        cConnect cn = new cConnect();
        DataManSystem dmsystem;
        private ResultTypes m_ResultTypes;
        bool open = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ping ping = new Ping();
            PingReply pr = ping.Send("192.168.0.10");
            if (pr.Status == IPStatus.Success)
            {
                if (dmsystem != null)
                {
                    dmsystem.Disconnect();
                    pictureBox1.BackgroundImage = Resources.disconnect;
                    open = false;

                }
                else
                {

                    connect();
                    pictureBox1.BackgroundImage = Resources.CONNECT;
                    txtSerial.Enabled = true;
                    //txtUppercase.Enabled = true;


                }
            }
            else
            {
                MessageBox.Show("Cognex is disconnected, Please close the application");
                open = false;
                timer1.Enabled = false;
            }

        }

        public void connect()
        {
            try
            {
                EthSystemConnector myConn = new EthSystemConnector(IPAddress.Parse("192.168.0.10"));
                myConn.UserName = "admin"; myConn.Password = "";
                dmsystem = new DataManSystem(myConn);
                open = true;
                m_ResultTypes = ResultTypes.ReadString;
                dmsystem.ReadStringArrived += _system_readstringarrived;
                dmsystem.Connect();
                pictureBox1.BackgroundImage = Resources.CONNECT;
                dmsystem.SetResultTypes(m_ResultTypes);
            }
            catch (Exception)
            {
                MessageBox.Show("Please Open the Cognex and wait to initialize, Please reopen the application");
                Application.Exit();
                throw;
            }

        }
        private void _system_readstringarrived(object sender, ReadStringArrivedEventArgs args)
        {
            string read = args.ReadString;
            if (read == "")
            {
                MessageBox.Show("COM port cannot be Empty!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    var threadparam = new System.Threading.ThreadStart(delegate { WriteTextSafe(read); });
                    var thread2 = new System.Threading.Thread(threadparam);
                    thread2.Start();
                }
                catch (Exception)
                {

                    throw;
                }

            }


        }
        public void WriteTextSafe(string text)
        {
            if (text == "" || text == "-")
            {


            }
            else
            {
                try
                {
                    if (label1.InvokeRequired)
                    {
                        // Call this same method but append THREAD2 to the text
                        Action safeWrite = delegate { WriteTextSafe($"{text}"); };
                        txtUppercase.Invoke(safeWrite);
                    }
                    else
                        txtUppercase.Text = text;
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (open == true)
            {
                if (dmsystem != null)
                {
                    Ping ping = new Ping();
                    PingReply pr = ping.Send("192.168.0.10");
                    if (pr.Status == IPStatus.Success)
                    {
                        dmsystem.SendCommand("TRIGGER ON\r\n");
                        pictureBox1.BackgroundImage = Resources.CONNECT;
                    }
                    else
                    {
                        MessageBox.Show("Cognex is disconnected, Please close the application");
                        open = false;
                        timer1.Enabled = false;
                    }

                }

            }
            else
            {
                pictureBox1.BackgroundImage = Resources.disconnect;
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        public void loadgrid2()
        {
            DataTable dt = cn.gtable(DateTime.Now.ToString("yyyy-MM-dd")).Tables[0];
            dataGridView2.DataSource = dt;
        }
        public void loadmodel()
        {
            comboBox1.Items.Add("HS1089800");
            comboBox1.Items.Add("HS1089700");
            comboBox1.Items.Add("HS1089600");

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loadmodel();
            loadgrid2();
        }

        public void traceme()
        {
            string querye = "select serialnumber from data2 where serialnumber = '" + txtSerial.Text + "'";
            if (cn.dExistflatness(querye) == true)
            {
                string query = "select judgement from thermal_xena where barcode = '" + txtSerial.Text + "' order by inspection_time desc limit 1";
                lblStatus.Text = cn.gString(query);
                string querflatness = "select judgement from data2 where serialnumber = '" + txtSerial.Text + "' order by data2.index desc limit 1";
                lblFlatness.Text = cn.gflatness(querflatness);
                if (lblFlatness.Text == "OK")
                {
                    groupBox3.BackColor = Color.Green;
                }
                else if (lblFlatness.Text == "")
                {
                    lblFlatness.Text = "SKIP FLATNESS";
                    groupBox3.BackColor = Color.Orange;
                    timer1.Enabled = false;
                    txtUppercase.Enabled = false;
                }
                else
                {
                    groupBox3.BackColor = Color.Red;
                }
                string queryupper = "select ucase from phoenixchecker where serialnumber = '" + txtSerial.Text + "'";
                txtUppercase.Text = cn.gStringFurukawa(queryupper);
                DataTable dt = cn.gthermal(txtSerial.Text).Tables[0];
                dataGridView1.DataSource = dt;
                if (lblStatus.Text.Contains("OK") && lblFlatness.Text == "OK")
                {
                    groupBox2.BackColor = Color.Green;
                    open = true;
                    timer1.Enabled = true;

                    txtUppercase.Enabled = true;
                    txtUppercase.Focus();
                }
                else if (lblStatus.Text.Contains("NG") || lblFlatness.Text == "NG")
                {
                    groupBox2.BackColor = Color.Red;
                    open = false;
                    timer1.Enabled = false;
                    txtUppercase.Enabled = false;
                    if (lblFlatness.Text == "OK")
                    {
                        groupBox3.BackColor = Color.Green;
                    }
                    else if (lblStatus.Text == "OK")
                    {
                        groupBox2.BackColor = Color.Green;
                    }
                    MessageBox.Show(txtSerial.Text + " IS NO GOOD!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSerial.Text = "";
                    groupBox2.BackColor = Color.Transparent;
                    groupBox3.BackColor = Color.Transparent;
                    lblFlatness.Text = "";
                    lblStatus.Text = "";
                }
                else
                {
                    lblStatus.Text = "SKIP THERMAL";
                    groupBox2.BackColor = Color.Orange;
                    open = false;
                    timer1.Enabled = false;
                    txtUppercase.Enabled = false;
                }
            }

            else
            {
                lblFlatness.Text = "SKIP FLATNESS";

                MessageBox.Show(txtSerial.Text + " Skip in flatness process!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSerial.Text = "";
            }


        }
        private void txtSerial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (txtSerial.TextLength == 34)
                {
                    string queryexist = "select * from phoenixchecker where serialnumber = '" + txtSerial.Text + "'";
                    DataTable dt2 = cn.gtable2(txtSerial.Text).Tables[0];
                    dataGridView3.DataSource = dt2;
                    string queryold = "select oldbarcode from barcode_to_barcode where newbarcode = '" + txtSerial.Text + "'";
                    lblOldBarcode.Text = cn.gStringoldB(queryold);
                    string queryold2 = "select oldbarcode from barcode_to_barcode where newbarcode = '" + lblOldBarcode.Text + "'";
                    lbloldbarcode2.Text = cn.gStringoldB(queryold2);

                    if (lblOldBarcode.Text == "" && lbloldbarcode2.Text == "")
                    {
                        if (cn.dExiest(queryexist) == false)
                        {

                            traceme();

                        }
                        else
                        {
                            MessageBox.Show(txtSerial.Text + " Already exist!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtSerial.Text = "";
                        }
                    }
                    else
                    {
                        if (lbloldbarcode2.Text == "")
                        {
                            string queryeOld = "select * from phoenixchecker where serialnumber = '" + lblOldBarcode.Text + "'";
                            if (cn.dExiest(queryeOld) == true)
                            {
                                string queryupdate = "update phoenixchecker set serialnumber = '" + txtSerial.Text + "',Date_process = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where serialnumber = '" + lblOldBarcode.Text + "'";
                                cn.uData(queryupdate);
                                MessageBox.Show(txtSerial.Text + " has been updated", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtSerial.Text = "";
                                lblOldBarcode.Text = "";
                                loadgrid2();
                                timer1.Enabled = false;
                            }
                            else
                            {
                                traceme();
                            }
                        }
                        else
                        {
                            string queryeOld2 = "select * from phoenixchecker where serialnumber = '" + lbloldbarcode2.Text + "'";
                            if (cn.dExiest(queryeOld2) == true)
                            {

                                string queryupdate = "update phoenixchecker set serialnumber = '" + txtSerial.Text + "',Date_process = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where serialnumber = '" + lbloldbarcode2.Text + "'";
                                cn.uData(queryupdate);
                                MessageBox.Show(txtSerial.Text + " has been updated", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtSerial.Text = "";
                                lblOldBarcode.Text = "";
                                loadgrid2();
                                timer1.Enabled = false;
                            }
                            else
                            {
                                traceme();
                            }
                        }
                    }

                }

                else
                {
                    MessageBox.Show(txtSerial.Text + " is a wrong barcode!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }

        private void txtUppercase_TextChanged(object sender, EventArgs e)
        {


        }

        private void txtUppercase_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (txtSerial.TextLength != 34)
                {
                    MessageBox.Show(txtSerial.Text + " is a wrong barcode!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string queryupper = "select ucase from phoenixchecker where ucase = '" + txtUppercase.Text + "'";
                    if (cn.dExiest(queryupper) == true)
                    {
                        MessageBox.Show(txtUppercase.Text + " Duplicate Upper Case, Already Exist!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtUppercase.Text = "";
                        txtSerial.Text = "";
                        txtSerial.Focus();

                    }
                    else if (txtSerial.Text != "" && txtUppercase.Text != "" && comboBox1.Text != "")
                    {
                        string a = txtSerial.Text.Substring(4, 9);

                        if (txtUppercase.TextLength == 14)
                        {
                            if (a == comboBox1.Text)
                            {
                                string query = "insert into phoenixchecker(serialnumber,ucase,rca,date_process,model,flatness)values('" + txtSerial.Text + "','" + txtUppercase.Text + "','" + lblStatus.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + comboBox1.Text + "','" + lblFlatness.Text + "')";
                                cn.iData(query);
                                timer1.Enabled = false;
                                open = false;
                                loadgrid2();
                                txtSerial.Text = "";
                                txtUppercase.Text = "";
                                txtSerial.Focus();
                                txtSerial.Focus();
                                txtUppercase.Enabled = false;
                            }
                            else
                            {
                                MessageBox.Show(txtSerial.Text + " is a different model!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            MessageBox.Show(txtSerial.Text + " is a wrong uppercase!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Empty Fields!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}

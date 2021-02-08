using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Futronic.Devices.FS26;
using System.IO;

namespace EAS
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;
        string conStr;
        public Form1()
        {
            InitializeComponent();
            loginP.BackColor = Color.FromArgb(55, Color.White);
            
            if (isConnected())
            {


                connL.Text = "Connected";
                connL.ForeColor = Color.DarkCyan;


            }
            else {
                connL.Text = "Not Connected";
                connL.ForeColor = Color.DarkRed;

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = conStr;
                conn.Open();
                string q = "SELECT* FROM  logininfo WHERE BINARY  username='" + ut.Text + "'  AND BINARY  password='" + pt.Text + "'";
                MySqlCommand cmd = new MySqlCommand(q, conn);

                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Global.SuperName = (dr["name"].ToString());
                    Global.SuperUser = (dr["username"].ToString());
                    Global.SuperAuth = (dr["auth"].ToString());
                    Form main = new Main();
                    main.Show();
                    ut.Text = "";
                    pt.Text = "";
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Username or Password is Incorrect.");
                }
                conn.Close();

            }
            catch
            {

                MessageBox.Show("there was an issue, check internet connection");

            }

        }

      
        public bool isConnected() {


            conStr = "SERVER=localhost;PORT=3306;DATABASE=EAS;UID=root;PASSWORD=;";

            try
            {

                conn = new MySqlConnection();
                conn.ConnectionString = conStr;
                conn.Open();
               return true;
                conn.Close();
            }
            catch
            {
                return false;
                conn.Close();


            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
 
        private  void button3_Click(object senderr, EventArgs e)
        {


            callAsync();
          

        }

        async void callAsync() {

            await GetFinger();

          


        }

        async Task GetFinger()
        {



             label4.Text = ("LibScanApi Demo");

            var accessor = new DeviceAccessor();

             using (var device = accessor.AccessFingerprintDevice())
            {
                 device.SwitchLedState(false, false);

                device.FingerDetected += (sender, eventArgs) =>
                {
                    label4.Text = ("Finger Detected!");

                    device.SwitchLedState(true, false);

                    // Save fingerprint to temporary folder
                    var fingerprint = device.ReadFingerprint();
                    var tempFile = Path.GetTempFileName();
                    var tmpBmpFile = Path.ChangeExtension(tempFile, "bmp");
                    fingerprint.Save(tmpBmpFile);

                    label4.Text = ("Saved to " + tmpBmpFile);
                };

                device.FingerReleased += (sender, eventArgs) =>
                {
                    label4.Text = ("Finger Released!");

                    device.SwitchLedState(false, true);
                };

                label4.Text = ("FingerprintDevice Opened");

                device.StartFingerDetection();
                device.SwitchLedState(false, true);

                // Console.ReadLine();

                label4.Text = ("Exiting...");

                device.SwitchLedState(false, false);
            }



        }

    }
}

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
using System.Device.Location;
using GoogleMaps.LocationServices;
using System.Threading;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic;

namespace EAS
{
    public partial class Main : Form
    {
        //l1cBWINpeh
        MySqlConnection conn;
        bool fullsc = false;
        string conStr = "SERVER=localhost;PORT=3306;DATABASE=EAS;UID=root;PASSWORD=;";
        string conStrSlave = "Server=master_server,slave_server;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";

        int sleepTime =3000;
        List<string> locationsList = new List<string>();
        List<string> projectsList = new List<string>();

        List<string> reportsList = new List<string>();
        List<string> reportsIDList = new List<string>();

        List<string> delproList = new List<string>();
        List<string> delproListID = new List<string>();
       
        List<string> selProList = new List<string>();
        List<string> selProListID = new List<string>();
        
        List<string> delLocaLit = new List<string>();
        List<string> delLocaLitID = new List<string>();

        public Main()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            carCB.Visible = false;
            carL.Visible = false;
            clearAP();
            addP.BringToFront();
            DateTime today = DateTime.Today;
            dateT.Text = today.ToString("yyyy-MM-dd");
            locationsList.Clear();
            projcb.Items.Clear();
            projcb.SelectedIndex = -1;
            combboxL.Items.Clear();
            combboxL.SelectedIndex=-1;
            combboxL.Text = " -  Select Location";
            projcb.Text = " -  Select Project";
            carsLit.Clear();
            carsLitID.Clear();
            carCB.Items.Clear();
            getProjects();
            superNameTextF.Text = Global.SuperName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearVP();
            viewP.BringToFront();
            reportsList.Clear();
            reportsIDList.Clear();
            repSelectC.Items.Clear();
            repSelectC.SelectedIndex = -1;
            repSelectC.Text = " -  Select Location";
            try { 
            conn = new MySqlConnection();

            conn.ConnectionString = conStr;
            conn.Open();
            DateTime today = DateTime.Today;
            string q = "SELECT * FROM  reports WHERE date ='" + today.ToString("yyyy-MM-dd") + "' AND super_name ='"+Global.SuperName+"' ";
            MySqlCommand cmd = new MySqlCommand(q, conn);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string rec = "Supervisor: " + (dr["super_name"].ToString()) + ",    Smployee: " + (dr["emp_name"].ToString()) + ",      project: " + (dr["project"].ToString() + ",     location: " + (dr["location"].ToString()));
                reportsList.Add(rec);
                reportsIDList.Add(dr["id"].ToString());

            }


            conn.Close();
            for (int i = 0; i < reportsIDList.Count; i++)
            {

                repSelectC.Items.Add(reportsList.ElementAt(i));

            }
        } catch
                {

                    MessageBox.Show("there was an issue, check internet connection");

                }
}

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public void getProjects()
        {
            projcb.Items.Clear();
            projcb.SelectedIndex = -1;
           
            projcb.Text = " -  Select Project";
            projectsList.Clear();
            try { 
            conn = new MySqlConnection();

            conn.ConnectionString = conStr;
            conn.Open();
            string q = "SELECT * FROM  projects ";
            MySqlCommand cmd = new MySqlCommand(q, conn);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                projectsList.Add(dr["project"].ToString());
            }


            conn.Close();
            for (int i = 0; i < projectsList.Count; i++)
            {

                projcb.Items.Add(projectsList.ElementAt(i));

            }
            }
            catch
            {

                MessageBox.Show("there was an issue, check internet connection");

            }

        }
        public void getLocations() {
            combboxL.Items.Clear();
            combboxL.SelectedIndex = -1;
            combboxL.Text = " -  Select Location";

            locationsList.Clear();
            try { 

            conn = new MySqlConnection();

            conn.ConnectionString = conStr;
                conn.Open();
                string q = "SELECT * FROM  locations WHERE project ='"+ projcb.Items[projcb.SelectedIndex] + "' ";
                MySqlCommand cmd = new MySqlCommand(q,conn);

                MySqlDataReader dr = cmd.ExecuteReader();
         
                while (dr.Read()) { 
                    locationsList.Add(dr["location"].ToString());
            }
           
          
                conn.Close();
            for (int i =0; i <locationsList.Count;i++ ) {

               combboxL.Items.Add(locationsList.ElementAt(i));
            
            }
        } catch
                {

                    MessageBox.Show("there was an issue, check internet connection");

                }

}
        private void button1_Click_1(object sender, EventArgs e)
        {


            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
            Thread.Sleep(sleepTime);
            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {
                locT.Text = "Lat:'" + coord.Latitude + "', Long:'" + coord.Longitude + "'";

               

            }
            else
            {
               
                MessageBox.Show("Internet Connection is Too Slow, Try Again!");
                sleepTime += 1000; 
            }
        }

        List<string> carsLit = new List<string>();
        List<string> carsLitID = new List<string>();
        private void button4_Click(object sender, EventArgs e)
        {
            try {
            conn = new MySqlConnection();

            conn.ConnectionString = conStr;
            conn.Open();
            int fb = 12;
            string q = "SELECT * FROM  employees WHERE fp = '"+fb+"' ";
            MySqlCommand cmd = new MySqlCommand(q, conn);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                empFbTB.Text=(dr["name"].ToString());
                positionT.Text = dr["position"].ToString();
                    DWTB.Text=(dr["wage"].ToString());
                    if (dr["daily"].ToString().Equals("true")) {



                        label1.Text = "Daily Pay";


                            }
                    else {
                        label1.Text = "Pay";


                    }
                    if (dr["position"].ToString().ToString().Equals("Driver")) {



                        carCB.Visible = true;
                        carL.Visible = true;
                        getCars();
                    }


                }


            conn.Close();
                ChInTimeTb.Text = DateTime.Now.ToString("hh:mm");

            }
            catch
                {

                    MessageBox.Show("there was an issue, check internet connection");

                }



}
        public void getCars()
        {
            try
            {
                conn = new MySqlConnection();

                conn.ConnectionString = conStr;
                conn.Open();
                string q = "SELECT * FROM  cars ";
                MySqlCommand cmd = new MySqlCommand(q, conn);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    carsLit.Add(dr["car_info"].ToString()+ "  rentaled :  " + dr["rentaled"].ToString() );
                    carsLitID.Add(dr["id"].ToString());


                }


                conn.Close();
                for (int i = 0; i < carsLit.Count; i++)
                {

                    carCB.Items.Add(carsLit.ElementAt(i));

                }
            }
            catch
            {

                MessageBox.Show("there was an issue, check internet connection");

            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are You Sure", "Confirm Exit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                //nothing
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!fullsc) {
                this.Location = new Point(Screen.PrimaryScreen.WorkingArea.X, Screen.PrimaryScreen.WorkingArea.Y);
                this.Size = Screen.PrimaryScreen.WorkingArea.Size;
                fullsc = true;
            }
            else {
                this.Location = new Point(((Screen.PrimaryScreen.WorkingArea.Width/2)-(900/2)), ((Screen.PrimaryScreen.WorkingArea.Height/2)-(550/2)));
                this.Size = new System.Drawing.Size( 900,550);

                fullsc = false;


            }

           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void Main_Load(object sender, EventArgs e)
        {
            
            if (Global.SuperAuth.Equals("1")) { supernameL.Text = Global.SuperName + " - Admin"; Manager.Show(); } else { supernameL.Text = Global.SuperName; Manager.Hide();  }
            addB.PerformClick();
           
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

           
             
           
          


            if (carCB.SelectedIndex > -1&&combboxL.SelectedIndex > -1 && dateT.TextLength > 0 && superNameTextF.TextLength > 0 && empFbTB.Text.Length > 0 && DWTB.Text.Length > 0 && locT.TextLength > 0)
            {
                try
                {
                    MySqlConnection conn;
                    conn = new MySqlConnection();
                    conn.ConnectionString = conStr;
                    conn.Open();
                    string q = "INSERT INTO reports (project,location,date,super_name,emp_name,check_in,d_w,long_latta,position,car) VALUES (@project,@location,@date,@super_name,@emp_name,@check_in,@d_w,@long_latta,@position,@car) ";
                    MySqlCommand cmd = new MySqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@location", combboxL.Items[combboxL.SelectedIndex]);
                    cmd.Parameters.AddWithValue("@project", projcb.Items[projcb.SelectedIndex]);
                    cmd.Parameters.AddWithValue("@date", dateT.Text);
                    cmd.Parameters.AddWithValue("@super_name", superNameTextF.Text);
                    cmd.Parameters.AddWithValue("@emp_name", empFbTB.Text);
                    cmd.Parameters.AddWithValue("@check_in", ChInTimeTb.Text);
                    cmd.Parameters.AddWithValue("@d_w", DWTB.Text);
                    cmd.Parameters.AddWithValue("@long_latta", locT.Text);
                    cmd.Parameters.AddWithValue("@position", positionT.Text);
                    cmd.Parameters.AddWithValue("@car", carsLit.ElementAt(carCB.SelectedIndex));


                    MySqlDataReader dr = cmd.ExecuteReader();
                    conn.Close();
                    MessageBox.Show("Record Added Successfully");

                    addB.PerformClick();
                    

                }
                catch {

                    MessageBox.Show("there was an issue, check internet connection");


                }



            }
            else {

                MessageBox.Show("Please, Dont Leave Anything empty");
            }
        }
        public void clearManager() {

            addlocT.Text = "";
            addproT.Text = "";
            delproC.SelectedIndex = -1;
            delproC.Items.Clear();
            delproC.Text = " -  Select a Project";
            addlocproC.SelectedIndex = -1;
            addlocproC.Items.Clear();
            addlocproC.Text = " -  Select a Project";
            delLoC.SelectedIndex = -1;
            delLoC.Items.Clear();
            delLoC.Text = " -  Select a Location";





        }
        public void getProjectList() {
            try
            {
                conn = new MySqlConnection();

                conn.ConnectionString = conStr;
                conn.Open();
                string q = "SELECT * FROM  projects ";
                MySqlCommand cmd = new MySqlCommand(q, conn);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    delproList.Add(dr["project"].ToString());
                    delproListID.Add(dr["id"].ToString());


                }


                conn.Close();
                for (int i = 0; i < delproList.Count; i++)
                {

                    delproC.Items.Add(delproList.ElementAt(i));
                    addlocproC.Items.Add(selProList.ElementAt(i));

                }
            }
            catch
            {

                MessageBox.Show("there was an issue, check internet connection");

            }
        }
        public void getLocationList() {

            try
            {
                conn = new MySqlConnection();

                conn.ConnectionString = conStr;
                conn.Open();
                string q = "SELECT * FROM  locations ";
                MySqlCommand cmd = new MySqlCommand(q, conn);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    delLocaLit.Add(dr["location"].ToString() +"  -  "+ dr["project"].ToString());
                    delLocaLitID.Add(dr["id"].ToString());


                }


                conn.Close();
                for (int i = 0; i < delLocaLit.Count; i++)
                {

                    delLoC.Items.Add(delLocaLit.ElementAt(i));

                }
            }
            catch
            {

                MessageBox.Show("there was an issue, check internet connection");

            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            clearManager();
            delproList.Clear();
            delproListID.Clear();

            selProList.Clear();
            selProListID.Clear();

            delLocaLit.Clear();
            delLocaLitID.Clear();
            locproman.BringToFront();
            getProjectList();
            getLocationList();




        }

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        public void m_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        public void m_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        public void m_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void locT_TextChanged(object sender, EventArgs e)
        {

        }

        private void projcb_SelectedIndexChanged(object sender, EventArgs e)
        {
            getLocations();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            empFbTB.Text = "";
            DWTB.Text = "";
            ChInTimeTb.Text = "";
        }

        private void repSelectC_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            try { 
            conn = new MySqlConnection();

            conn.ConnectionString = conStr;
            conn.Open();
            int fb = 123;
            string q = "SELECT * FROM  reports WHERE id = '" + reportsIDList.ElementAt(repSelectC.SelectedIndex) + "' ";
            MySqlCommand cmd = new MySqlCommand(q, conn);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                VproT.Text = (dr["project"].ToString());
                VlocT.Text = (dr["location"].ToString());
                VchinL.Text = (dr["check_in"].ToString());
                VempL.Text = (dr["emp_name"].ToString());
                VdpL.Text = (dr["d_w"].ToString());
                VchoutL.Text = (dr["check_out"].ToString());

            }


            conn.Close();
            for (int i = 0; i < locationsList.Count; i++)
            {

                combboxL.Items.Add(locationsList.ElementAt(i));

            }
        } catch
                {

                    MessageBox.Show("there was an issue, check internet connection");

                }


}

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
           VchoutL.Text = DateTime.Now.ToString("hh:mm");

        }
        public void clearVP() {
            VproT.Clear();
            VlocT.Text = "";
            VchinL.Text="";
            VchoutL.Text="";
            VempL.Text = "";
            VdpL.Text = "";
            



        }
        public void clearAP()
        {
            dateT.Clear();
            superNameTextF.Clear();
            locT.Clear();
            empFbTB.Text = "";
            ChInTimeTb.Text = "";
            DWTB.Text = "";




        }
        private void editV_Click(object sender, EventArgs e)
        {
            
            DialogResult dialogResult = MessageBox.Show("Are You Sure You Want to Permanently Delete This Record ", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try { 
                conn = new MySqlConnection();

                conn.ConnectionString = conStr;
                conn.Open();
                int fb = 123;
                string q = "DELETE FROM  reports WHERE id = '" + reportsIDList.ElementAt(repSelectC.SelectedIndex) + "' ";
                MySqlCommand cmd = new MySqlCommand(q, conn);

                MySqlDataReader dr = cmd.ExecuteReader();
                conn.Close();
                manB.PerformClick();
            }  catch
                {

                    MessageBox.Show("there was an issue, check internet connection");

                }

            }
            else if (dialogResult == DialogResult.No)
            {
              
            }
        }

        private void submitV_Click(object sender, EventArgs e)
        {
            if (!VchoutL.Text.Equals(""))
            {
                try
                {
                    conn = new MySqlConnection();

                    conn.ConnectionString = conStr;
                    conn.Open();
                    int fb = 123;
                    string q = "UPDATE reports SET check_out='" + VchoutL.Text + "' WHERE id = '" + reportsIDList.ElementAt(repSelectC.SelectedIndex) + "' ";
                    MySqlCommand cmd = new MySqlCommand(q, conn);

                    MySqlDataReader dr = cmd.ExecuteReader();


                    conn.Close();
                    MessageBox.Show("Record Added Successfully");

                    manB.PerformClick();
                }
                catch {

                    MessageBox.Show("there was an issue, check internet connection");

                }

            }
            else {
                MessageBox.Show("The Check Out Time is Not Set Yet.");


            }
        }

        private void outB_Click(object sender, EventArgs e)
        {
            Global.SuperName = "";
            Global.SuperUser = "";
            Global.SuperAuth = "";

            this.Hide();
            Form1 login = new Form1();
            login.Show();
            
        }

        private void button17_Click(object sender, EventArgs e)
        {
            bool projectExitst = false;

            if (!addproT.Text.Equals("")) {

                DialogResult dialogResult = MessageBox.Show("Are You Sure you want to add the project '"+ addproT .Text+ "'", "Confirm", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        conn = new MySqlConnection();
                        conn.ConnectionString = conStr;
                        conn.Open();
                        string q = "SELECT* FROM projects WHERE project='" + addproT.Text + "'";
                        MySqlCommand cmd = new MySqlCommand(q, conn);

                        MySqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            MessageBox.Show("Project Already Exists.");
                            projectExitst = true;
                        }
                        else
                        {
                            projectExitst = false;



                        }
                        conn.Close();


                        if (!projectExitst)
                        {

                            MySqlConnection conn;
                            conn = new MySqlConnection();
                            conn.ConnectionString = conStr;
                            conn.Open();
                             q = "INSERT INTO projects (project) VALUES (@project) ";
                             cmd = new MySqlCommand(q, conn);
                            cmd.Parameters.AddWithValue("@project", addproT.Text);



                             dr = cmd.ExecuteReader();
                            conn.Close();
                            MessageBox.Show("Project Added Successfully");

                            Manager.PerformClick();





                        }

                    }
                    catch
                    {

                        MessageBox.Show("there was an issue, check internet connection");

                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    //nothing
                }
             
            }
            else {

                MessageBox.Show("Write a Project Name");


            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (delproC.SelectedIndex != -1)
            {
                DialogResult dialogResult = MessageBox.Show("Are You Sure You Want to Permanently Delete This Project ", "Confirm", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        conn = new MySqlConnection();

                        conn.ConnectionString = conStr;
                        conn.Open();
                        
                        string q = "DELETE FROM  projects WHERE id = '" + delproListID.ElementAt(delproC.SelectedIndex) + "' ";
                        MySqlCommand cmd = new MySqlCommand(q, conn);

                        MySqlDataReader dr = cmd.ExecuteReader();
                        conn.Close();
                        MessageBox.Show("Project Deleted Successfully");

                       
                        Manager.PerformClick();

                    }
                    catch
                    {

                        MessageBox.Show("there was an issue, check internet connection");

                    }

                }
                else if (dialogResult == DialogResult.No)
                {

                }
          


        }
            else {
                MessageBox.Show("Please Select a Project");

            }
        }

        private void button18_Click(object sender, EventArgs e)
        {

            if (!addlocT.Text.Equals(""))
            {
                if (addlocproC.SelectedIndex != -1)
                {
                    try { 
                    MySqlConnection conn;
                    conn = new MySqlConnection();
                    conn.ConnectionString = conStr;
                    conn.Open();
                    string q = "INSERT INTO locations (location,project) VALUES (@location,@project) ";
                    MySqlCommand cmd = new MySqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@location", addlocT.Text);
                    cmd.Parameters.AddWithValue("@project", selProList.ElementAt(addlocproC.SelectedIndex));



                    MySqlDataReader dr = cmd.ExecuteReader();
                    conn.Close();
                    MessageBox.Show("Location Added Successfully");

                    Manager.PerformClick();

                }
                    catch
                {

                    MessageBox.Show("there was an issue, check internet connection");

                }


            }
                else
                {
                    MessageBox.Show("Please Select a Location");
                }
            }
            else
            {
                MessageBox.Show("Write a Location Name");
            }


        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (delLoC.SelectedIndex != -1)
            {
                DialogResult dialogResult = MessageBox.Show("Are You Sure You Want to Permanently Delete This Location ", "Confirm", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        conn = new MySqlConnection();

                        conn.ConnectionString = conStr;
                        conn.Open();

                        string q = "DELETE FROM  locations WHERE id = '" + delLocaLitID.ElementAt(delLoC.SelectedIndex) + "' ";
                        MySqlCommand cmd = new MySqlCommand(q, conn);

                        MySqlDataReader dr = cmd.ExecuteReader();
                        conn.Close();
                        MessageBox.Show("Location Deleted Successfully");

                        Manager.PerformClick();

                    }
                    catch
                    {

                        MessageBox.Show("there was an issue, check internet connection");

                    }

                }
                else if (dialogResult == DialogResult.No)
                {

                }



            }
        }

        private void panel16_Paint(object sender, PaintEventArgs e)
        {

        }

        List<string> posList = new List<string>();
        List<string> posListID = new List<string>();
       
        private void staffB_Click(object sender, EventArgs e)
        {
            posList.Clear();
            posListID.Clear();
            PosCB.Items.Clear();
            staffmemdwT.Clear();
            staffmemT.Clear();
            fbt.Text = "";
            getposList();
            staffmanagerP.BringToFront();

            radioD.Checked = true;

      

        }
        public void getposList()
        {

            try
            {
                conn = new MySqlConnection();

                conn.ConnectionString = conStr;
                conn.Open();
                string q = "SELECT * FROM  positions ";
                MySqlCommand cmd = new MySqlCommand(q, conn);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    posList.Add(dr["position"].ToString());
                    posListID.Add(dr["id"].ToString());


                }


                conn.Close();
                for (int i = 0; i < posList.Count; i++)
                {

                    PosCB.Items.Add(posList.ElementAt(i));

                }
            }
            catch
            {

                MessageBox.Show("there was an issue, check internet connection");

            }

        }
        private void addnewstaffmemB_Click(object sender, EventArgs e)
        {
            bool EmpExitst = true;

            if (staffmemdwT.Text.Equals("") || staffmemT.Text.Equals("") || fbtSt.Equals("") || PosCB.SelectedIndex < -1)
            {
                MessageBox.Show("Please, Dont Leave Anything empty");

            }
            else {


                DialogResult dialogResult = MessageBox.Show("Are You Sure you want to add the Employee '" + staffmemT.Text + "'", "Confirm", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    try
                    {
                        conn = new MySqlConnection();
                        conn.ConnectionString = conStr;
                        conn.Open();
                        string q = "SELECT* FROM employees WHERE name='" + staffmemT.Text + "'";
                        MySqlCommand cmd = new MySqlCommand(q, conn);

                        MySqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            MessageBox.Show("Employees Already Exists.");
                            EmpExitst = true;
                        }
                        else
                        {
                            EmpExitst = false;



                        }
                        conn.Close();


                        if (!EmpExitst)
                        {
                           

                            MySqlConnection conn;
                            conn = new MySqlConnection();
                            conn.ConnectionString = conStr;
                            conn.Open();
                             q = "INSERT INTO employees (name,wage,fp,position,daily) VALUES (@name,@daily_wage,@fp,@position,@daily) ";
                            cmd = new MySqlCommand(q, conn);
                            cmd.Parameters.AddWithValue("@name", staffmemT.Text);
                            cmd.Parameters.AddWithValue("@daily_wage", staffmemdwT.Text);
                            cmd.Parameters.AddWithValue("@fp", fbtSt);
                            cmd.Parameters.AddWithValue("@position", PosCB.Items[PosCB.SelectedIndex]);

                            if ((radioD.Checked == true))
                            {

                                cmd.Parameters.AddWithValue("@daily", true);
                            }
                            else {
                                cmd.Parameters.AddWithValue("@daily", false);


                            }

                            dr = cmd.ExecuteReader();
                            conn.Close();
                            MessageBox.Show("Employee Added Successfully");

                            staffB.PerformClick();

                          




                        }

                    }
                    catch
                    {

                        MessageBox.Show("there was an issue, check internet connection");

                    }

                  
                }
                else if (dialogResult == DialogResult.No)
                {
                    //nothing
                }



            }
        }
        public string fbtSt = "";
        private void fbb_Click(object sender, EventArgs e)
        {
            fbt.Text = "Scanned"; 
            fbt.ForeColor = Color.LawnGreen;
            fbtSt = "1246721";
        }

        private void VdpL_Click(object sender, EventArgs e)
        {

        }

        private void fbt_Click(object sender, EventArgs e)
        {

        }
        string ChOutTimeSt = "";
        string CheckOutFP = "";
        string COid = "";
        bool scanned = false;
        string empNameHolder = "";

        private void button15_Click(object sender, EventArgs e)
        {
            ChOutTimeSt = "";
            CheckOutFP = "";
           COid = "";
            CheckOutFP = "12";
          
         
           

            try
            {

                conn = new MySqlConnection();

                conn.ConnectionString = conStr;
                conn.Open();

                

                string q = "SELECT * FROM  employees WHERE fp ='" + CheckOutFP + "' ";
                MySqlCommand cmd = new MySqlCommand(q, conn);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    empNameHolder = (dr["name"].ToString());
                   

                }


                conn.Close();


                if (!empNameHolder.Equals("")) { 

                conn = new MySqlConnection();

                conn.ConnectionString = conStr;
                conn.Open();
               
                DateTime today = DateTime.Today;

                 q = "SELECT * FROM  reports WHERE date ='" + today.ToString("yyyy-MM-dd") + "' AND emp_name ='" + empNameHolder + "' ";
                 cmd = new MySqlCommand(q, conn);

                 dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    EmpNameL.Text = (dr["emp_name"].ToString());
                    ProLocSuper.Text = (dr["super_name"].ToString())+" - "+ (dr["project"].ToString()) + " - "+(dr["location"].ToString());
                    COid = (dr["id"].ToString());
                    FPL.Text = "Scanned";
                    FPL.ForeColor = Color.LightGreen;
                    ChOutTimeSt = DateTime.Now.ToString("hh:mm");
                    TimeChoutT.Text = ChOutTimeSt;
                    scanned = true;

                }


                conn.Close();
                }
                else
                {
                    MessageBox.Show("Record Does Not Exist");


                }
            }
            catch
            {
                FPL.Text = "Not Found Scan Agent";
                FPL.ForeColor = Color.Red;
                scanned = false;



            }
       

    }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (scanned) { 
            try
            {
                conn = new MySqlConnection();

                conn.ConnectionString = conStr;
                conn.Open();
                int fb = 123;
                string q = "UPDATE reports SET check_out='" + ChOutTimeSt + "' WHERE id = '" + COid + "' ";
                MySqlCommand cmd = new MySqlCommand(q, conn);

                MySqlDataReader dr = cmd.ExecuteReader();


                conn.Close();
                MessageBox.Show("Record Updated Successfully");

                ChOutB.PerformClick();
            }
            catch
            {

                MessageBox.Show("there was an issue, check internet connection");

            }
            }
        }

        private void ChOutB_Click(object sender, EventArgs e)
        {
            FPL.Text = "";
                TimeChoutT.Text = "";
            EmpNameL.Text = "";
            ProLocSuper.Text = "";
            CHOUTTIME.BringToFront();
        }
        private void Watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e) // Find GeoLocation of Device  
        {
            try
            {
                if (e.Status == GeoPositionStatus.Ready)
                {
                    // Display the latitude and longitude.  
                    if (watcher.Position.Location.IsUnknown)
                    {
                        latitude = "0";
                        longitute = "0";
                    }
                    else
                    {
                        latitude = watcher.Position.Location.Latitude.ToString();
                        longitute = watcher.Position.Location.Longitude.ToString();
                    }
                }
                else
                {
                    latitude = "0";
                    longitute = "0";
                }
            }
            catch (Exception)
            {
                latitude = "0";
                longitute = "0";
            }
        }
        string latitude;
        string longitute;
        GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
        private void button20_Click_1(object sender, EventArgs e)
        {
         
        watcher = new GeoCoordinateWatcher();
            // Catch the StatusChanged event.  
            watcher.StatusChanged += Watcher_StatusChanged;
            // Start the watcher.  
            watcher.Start();



            MessageBox.Show("long-"+ longitute + "latt"+ latitude);


        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Globalization;

using Astronomy;

namespace stargazer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
                 
            Bodies = new List<CelestialBody>();

            ReadSystemConfig("../cfg/kerbal.xml");

            // Bodies list init
            for (int i = 0; i < Bodies.Count; i++)
            {
                BodyData data = new BodyData();
                Bodies[i].get_data(ref data);

                if (data.name != "Sun")
                    BodiesList.Items.Add(data.name);
            }

            BodiesList.SelectedIndex = 0;            

            for (int i = 1; i <= KCalendar.Days; i++)
            {
                comboDay.Items.Add(i.ToString());
            }

            comboDay.SelectedIndex = 0;

            for (int i = 0; i < KCalendar.Hours; i++)
            {
                comboHour.Items.Add(i.ToString());
            }

            comboHour.SelectedIndex = 0;

            for (int i = 0; i < KCalendar.Mins; i++)
            {
                comboMin.Items.Add(i.ToString());
            }

            comboMin.SelectedIndex = 0;

            for (int i = 0; i < KCalendar.Secs; i++)
            {
                comboSec.Items.Add(i.ToString());
            }

            comboSec.SelectedIndex = 0;

            double t = KCalendar.date_to_sec(33, 353, 3, 42, 20);
            double phi = 10 * RAD;

            Transfer trans = new Transfer();            

            Lambert.get_transfer_date(t, Bodies[6], Bodies[1], phi, ref trans);
        }                
        
        private List<CelestialBody> Bodies;

        private const double RAD = Math.PI / 180.0; 

        //---------------------------------------------------------------
        //
        //---------------------------------------------------------------
        private void ReadSystemConfig(string cfg)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(cfg);

            var nodes = doc.GetElementsByTagName("Body");

            foreach (XmlNode node in nodes)
            {
                // Body data structure
                BodyData data = new BodyData();

                // Read all "Body" nodes
                foreach (XmlNode subnode in node.ChildNodes)
                {
                    // Read body parameters
                    if (subnode.Name == "name")
                    {
                        data.name = subnode.InnerText;
                    }

                    if (subnode.Name == "id")
                    {
                        data.id = int.Parse(subnode.InnerText);
                    }

                    if (subnode.Name == "RefBody")
                    {
                        data.refBody = subnode.InnerText;
                    }

                    if (subnode.Name == "mass")
                    {
                        data.mass = double.Parse(subnode.InnerText, CultureInfo.InvariantCulture);
                    }

                    if (subnode.Name == "radius")
                    {
                        data.radius = double.Parse(subnode.InnerText, CultureInfo.InvariantCulture);
                    }

                    if (subnode.Name == "gravParameter")
                    {
                        data.gravParameter = double.Parse(subnode.InnerText, CultureInfo.InvariantCulture);
                    }

                    if (subnode.Name == "rotationPeriod")
                    {
                        data.rotationPeriod = double.Parse(subnode.InnerText, CultureInfo.InvariantCulture);
                    }

                    if (subnode.Name == "Orbit")
                    {
                        data.orbit = new Orbit();

                        // Read body orbit parameters
                        foreach (XmlNode orb_param in subnode.ChildNodes)
                        {
                            if (orb_param.Name == "semiMajorAxis")
                            {
                                data.orbit.a = double.Parse(orb_param.InnerText, CultureInfo.InvariantCulture);
                            }

                            if (orb_param.Name == "eccentricity")
                            {
                                data.orbit.e = double.Parse(orb_param.InnerText, CultureInfo.InvariantCulture);
                            }

                            if (orb_param.Name == "epoch")
                            {
                                data.orbit.t0 = double.Parse(orb_param.InnerText, CultureInfo.InvariantCulture);
                            }

                            if (orb_param.Name == "meanAnomalyAtEpoch")
                            {
                                data.orbit.M0 = double.Parse(orb_param.InnerText, CultureInfo.InvariantCulture);
                            }

                            if (orb_param.Name == "period")
                            {
                                data.orbit.period = double.Parse(orb_param.InnerText, CultureInfo.InvariantCulture);
                            }

                            if (orb_param.Name == "argPe")
                            {
                                data.orbit.omega = double.Parse(orb_param.InnerText, CultureInfo.InvariantCulture);
                            }

                            if (orb_param.Name == "LAN")
                            {
                                data.orbit.Omega = double.Parse(orb_param.InnerText, CultureInfo.InvariantCulture);
                            }

                            if (orb_param.Name == "inclination")
                            {
                                data.orbit.i = double.Parse(orb_param.InnerText, CultureInfo.InvariantCulture);
                            }
                        }
                    }
                }

                CelestialBody Body = new CelestialBody();
                Body.set_data(data);
                Bodies.Add(Body);
            }

            // Init reference IDs and radiuses
            for (int i = 0; i < Bodies.Count; i++)
            {
                string refBody = Bodies[i].get_ref_body();
                int refId = get_body_index(refBody);

                if (refId != -1)
                {
                    Bodies[i].set_refId(refId);
                    Bodies[i].set_refRadius(Bodies[refId].get_radius());
                }
            }
        }



        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private void itemQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        //------------------------------------------------------------
        //
        //------------------------------------------------------------
        private int get_body_index(string name)
        {
            int idx = 0;
            BodyData data = new BodyData();

            do
            {
                Bodies.ElementAt(idx).get_data(ref data);
                idx++;

            } while ((idx-1 <= Bodies.Count) && (data.name != name));

            if (idx-1 > Bodies.Count)
                return -1;

            return idx - 1;
        }

        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private void buttonEphCalc_Click(object sender, EventArgs e)
        {
            int body_idx = get_body_index(BodiesList.Text.ToString());

            if (body_idx == -1)
            {
                MessageBox.Show("There are no " + BodiesList.Text.ToString() + " in data base");
                return;
            }

            double  t = KCalendar.date_to_sec(int.Parse(textYear.Text.ToString()),
                                            int.Parse(comboDay.Text.ToString()),
                                            int.Parse(comboHour.Text.ToString()),
                                            int.Parse(comboMin.Text.ToString()),
                                            int.Parse(comboSec.Text.ToString()));

            OrbitPos pos = new OrbitPos();
            EclipticPos ecoords = new EclipticPos();
            
            Bodies.ElementAt(body_idx).get_position(t, ref pos);
            Bodies.ElementAt(body_idx).get_ecliptic_coords(pos.theta, ref ecoords);            

            labelTrueAnomaly.Text = Math.Round(pos.theta / RAD, 4).ToString() + " deg";
            labelRadiusVector.Text = Math.Round(pos.r, 0).ToString() + " m";
            labelEccAnomaly.Text = Math.Round(pos.E, 4).ToString() + " rad";
            labelLat.Text = Math.Round(ecoords.beta / RAD, 4).ToString() + " deg";
            labelLon.Text = Math.Round(ecoords.lambda / RAD, 4).ToString() + " deg";
            labelAltitude.Text = Math.Round(pos.refAltitude, 0).ToString() + " m";
                     
            DrawPlanet(panelBodyPos, pos.theta, body_idx);
        }




        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private void BodiesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BodiesList.Text = BodiesList.SelectedItem.ToString();           
        }



        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private void DrawPlanet(Panel panel, double theta, int body_idx)
        {
            int width = panel.Width;
            int height = panel.Height;

            float x0 = width / 2;
            float y0 = height / 2;

            float x = 0;
            float y = 0;

            Graphics graph = panel.CreateGraphics();

            graph.Clear(Color.Black);

            Pen myPen = new Pen(Color.LightGray, 2.0F);           

            // Draw trajectory
            double  scale = 0;
            int Delta = 25;
            float delta = 5.0F;                    

            BodyData data = new BodyData();
                        
            Bodies[body_idx].get_data(ref data);

            int kerbin_idx = get_body_index("Kerbin");
            OrbitPos kerbin_pos = new OrbitPos();

            double  t = KCalendar.date_to_sec(int.Parse(textYear.Text.ToString()),
                                            int.Parse(comboDay.Text.ToString()),
                                            int.Parse(comboHour.Text.ToString()),
                                            int.Parse(comboMin.Text.ToString()),
                                            int.Parse(comboSec.Text.ToString()));

            Bodies[kerbin_idx].get_position(t, ref kerbin_pos);

            EclipticPos kerbin_epos = new EclipticPos();

            Bodies[kerbin_idx].get_ecliptic_coords(kerbin_pos.theta, ref kerbin_epos);            
            
            double  ra = data.orbit.a / (1 - data.orbit.e);
            double r_max = ra;

            float r0 = 0;

            if (width > height)
            {
                scale = (height / 2 - Delta) / r_max;
                r0 = height / 2 - Delta;
            }
            else
            {
                scale = (width / 2 - Delta) / r_max;
                r0 = width / 2 - Delta;
            }

            double  V = 0;
            double  dV = 5.0;           

            Vector3D pos;

            // Draw planet orbit
            while (V <= 360.0)
            {
                pos = Bodies[body_idx].get_cartesian_pos(V * RAD);

                float x1 = x0 + Convert.ToSingle(scale * pos.x);
                float y1 = y0 - Convert.ToSingle(scale * pos.y);

                V += dV;

                pos = Bodies[body_idx].get_cartesian_pos(V * RAD);

                float x2 = x0 + Convert.ToSingle(scale * pos.x);
                float y2 = y0 - Convert.ToSingle(scale * pos.y);

                if (pos.z >= 0) 
                    myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                else
                    myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                graph.DrawLine(myPen, x1, y1, x2, y2);
            }

            // Draw planet position
            pos = Bodies[body_idx].get_cartesian_pos(theta);

            EclipticPos epos = new EclipticPos();

            Bodies[body_idx].get_ecliptic_coords(theta, ref epos);

            double phi = Lambert.get_phase(epos.lambda - kerbin_epos.lambda) / RAD;

            x = x0 + Convert.ToSingle(scale * pos.x);
            y = y0 - Convert.ToSingle(scale * pos.y);

            myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;

            graph.DrawLine(myPen, x0, y0, x, y);

            // Title
            SolidBrush brush = new SolidBrush(Color.LightGray);
            Font font = new Font("Courier New", 10);
            string title = data.name + " position UT: " + 
                           textYear.Text + "y " + 
                           comboDay.Text + "d " + 
                           comboHour.Text + "h " +
                           comboMin.Text + "m " + 
                           comboSec.Text + "s";

            graph.DrawString(title, font, brush, delta, delta);            

            graph.DrawString("Lat. " + Math.Round(epos.beta / RAD, 4).ToString(), font, brush, delta, delta + font.Height);
            graph.DrawString("Lon. " + Math.Round(epos.lambda / RAD, 4).ToString(), font, brush, delta, delta + 2*font.Height);
            graph.DrawString("Kerbin phase: " + Math.Round(phi, 4).ToString(), font, brush, delta, height - delta - font.Height); 
            
            // Draw Ref Body
            myPen.Color = Color.Yellow;

            float sunRadius = 10.0F;

            if (data.refBody == "Sun")
                brush.Color = Color.Yellow;
            else
                brush.Color = Color.Blue;

            graph.FillEllipse(brush, x0 - sunRadius, y0 - sunRadius, 2*sunRadius, 2*sunRadius);

            // Draw Planet
            float radius = 5.0F;

            brush.Color = Color.Green;

            graph.FillEllipse(brush, x - radius, y - radius, 2 * radius, 2 * radius);

            // Draw Kerbin direction
            myPen.Color = Color.Red;
            myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            x = x0 + r0 * Convert.ToSingle(Math.Cos(kerbin_epos.lambda));
            y = y0 - r0 * Convert.ToSingle(Math.Sin(kerbin_epos.lambda));

            graph.DrawLine(myPen, x0, y0, x, y);
        }
    }   
}

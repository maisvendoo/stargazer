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

using EphemeridesCalc;


namespace stargazer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            bodies = new List<TBodyData>();

            ReadSystemConfig("../cfg/kerbal.xml");

            // Bodies list init
            for (int i = 0; i < bodies.Count; i++)
            {
                if (bodies.ElementAt(i).name != "Sun")
                    BodiesList.Items.Add(bodies.ElementAt(i).name);
            }

            BodiesList.SelectedIndex = 0;

            calendar = new CCalendar();

            for (int i = 1; i <= CCalendar.Days; i++)
            {
                comboDay.Items.Add(i.ToString());
            }

            comboDay.SelectedIndex = 0;

            for (int i = 0; i < CCalendar.Hours; i++)
            {
                comboHour.Items.Add(i.ToString());
            }

            comboHour.SelectedIndex = 0;

            for (int i = 0; i < CCalendar.Mins; i++)
            {
                comboMin.Items.Add(i.ToString());
            }

            comboMin.SelectedIndex = 0;

            for (int i = 0; i < CCalendar.Secs; i++)
            {
                comboSec.Items.Add(i.ToString());
            }

            comboSec.SelectedIndex = 0;
        }

        private List<TBodyData> bodies;
        private CCalendar calendar;

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
                TBodyData body = new TBodyData();

                // Read all "Body" nodes
                foreach (XmlNode subnode in node.ChildNodes)
                {
                    // Read body parameters
                    if (subnode.Name == "name")
                    {
                        body.name = subnode.InnerText;
                    }

                    if (subnode.Name == "id")
                    {
                        body.id = int.Parse(subnode.InnerText);
                    }

                    if (subnode.Name == "RefBody")
                    {
                        body.refBody = subnode.InnerText;
                    }

                    if (subnode.Name == "mass")
                    {
                        body.mass = double.Parse(subnode.InnerText);
                    }

                    if (subnode.Name == "radius")
                    {
                        body.radius = double.Parse(subnode.InnerText);
                    }

                    if (subnode.Name == "gravParameter")
                    {
                        body.gravParameter = double.Parse(subnode.InnerText);
                    }

                    if (subnode.Name == "rotationPeriod")
                    {
                        body.rotationPeriod = double.Parse(subnode.InnerText);
                    }

                    if (subnode.Name == "Orbit")
                    {
                        body.orbit = new TOrbitData();

                        // Read body orbit parameters
                        foreach (XmlNode orb_param in subnode.ChildNodes)
                        {
                            if (orb_param.Name == "semiMajorAxis")
                            {
                                body.orbit.a = double.Parse(orb_param.InnerText);
                            }

                            if (orb_param.Name == "eccentricity")
                            {
                                body.orbit.e = double.Parse(orb_param.InnerText);
                            }

                            if (orb_param.Name == "epoch")
                            {
                                body.orbit.t0 = double.Parse(orb_param.InnerText);
                            }

                            if (orb_param.Name == "meanAnomalyAtEpoch")
                            {
                                body.orbit.M0 = double.Parse(orb_param.InnerText);
                            }

                            if (orb_param.Name == "period")
                            {
                                body.orbit.period = double.Parse(orb_param.InnerText);
                            }

                            if (orb_param.Name == "argPe")
                            {
                                body.orbit.omega = double.Parse(orb_param.InnerText);
                            }

                            if (orb_param.Name == "LAN")
                            {
                                body.orbit.Omega = double.Parse(orb_param.InnerText);
                            }

                            if (orb_param.Name == "inclination")
                            {
                                body.orbit.i = double.Parse(orb_param.InnerText);
                            }
                        }
                    }
                }

                bodies.Add(body);
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
        private TBodyData get_body_index(string name)
        {
            int idx = 0;
            
            while ((idx < BodiesList.Items.Count) && (name != bodies.ElementAt(idx).name))
            {
                idx++;
            }

            if (idx > BodiesList.Items.Count)
                idx = -1;

            return bodies.ElementAt(idx);
        }



        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private void buttonEphCalc_Click(object sender, EventArgs e)
        {
            TBodyData body_data = get_body_index(BodiesList.Text.ToString());   
            TBodyData ref_body_data = get_body_index(body_data.refBody);

            body_data.orbit.RefRadius = ref_body_data.radius;

            TBodyState body_state = new TBodyState();
            CPlanet body = new CPlanet();

            double t = calendar.date_to_sec(int.Parse(textYear.Text.ToString()),
                                            int.Parse(comboDay.Text.ToString()),
                                            int.Parse(comboHour.Text.ToString()),
                                            int.Parse(comboMin.Text.ToString()),
                                            int.Parse(comboSec.Text.ToString()));

            body.get_planet_state(body_data.orbit, t, ref body_state);

            labelTrueAnomaly.Text = Math.Round(body_state.theta / CPlanet.RAD, 4).ToString() + " deg";
            labelRadiusVector.Text = Math.Round(body_state.r, 0).ToString() + " m";
            labelAltitude.Text = Math.Round(body_state.h, 0).ToString() + " m";
            labelEccAnomaly.Text = Math.Round(body_state.E, 4).ToString() + " rad";
            labelLat.Text = Math.Round(body_state.beta / CPlanet.RAD, 4).ToString() + " deg";
            labelLon.Text = Math.Round(body_state.lambda / CPlanet.RAD, 4).ToString() + " deg";

            DrawPlanet(panelBodyPos, body_data, body_state);
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
        private void DrawPlanet(Panel panel, TBodyData body_data, TBodyState body_state)
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
            double scale = 0;
            int Delta = 25;
            float delta = 5.0F;

            double ra = body_data.orbit.a / (1 - body_data.orbit.e); 

            if (width > height)
                scale = (height / 2 - Delta) / ra;
            else
                scale = (width / 2 - Delta) / ra;

            double V = 0;
            double dV = 5.0;

            Vector3D pos = new Vector3D();
            CPlanet body = new CPlanet();            

            while (V <= 360.0)
            {
                body.get_coords(body_data.orbit, V*CPlanet.RAD, ref pos);

                float x1 = x0 + Convert.ToSingle(scale * pos.x);
                float y1 = y0 - Convert.ToSingle(scale * pos.y);

                V += dV;

                body.get_coords(body_data.orbit, V*CPlanet.RAD, ref pos);

                float x2 = x0 + Convert.ToSingle(scale * pos.x);
                float y2 = y0 - Convert.ToSingle(scale * pos.y);

                if (pos.z >= 0) 
                    myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                else
                    myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                graph.DrawLine(myPen, x1, y1, x2, y2);
            }

            // Draw planet position
            body.get_coords(body_data.orbit, body_state.theta, ref pos);

            x = x0 + Convert.ToSingle(scale * pos.x);
            y = y0 - Convert.ToSingle(scale * pos.y);

            myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;

            graph.DrawLine(myPen, x0, y0, x, y);

            // Title
            SolidBrush brush = new SolidBrush(Color.LightGray);
            Font font = new Font("Courier New", 10);
            string title = body_data.name + " position UT: " + 
                           textYear.Text + "y " + 
                           comboDay.Text + "d " + 
                           comboHour.Text + "h " +
                           comboMin.Text + "m " + 
                           comboSec.Text + "s";
            graph.DrawString(title, font, brush, delta, delta);
            graph.DrawString("Lat. " + Math.Round(body_state.beta / CPlanet.RAD, 4).ToString(), font, brush, delta, delta + font.Height);
            graph.DrawString("Lon. " + Math.Round(body_state.lambda / CPlanet.RAD, 4).ToString(), font, brush, delta, delta + 2*font.Height); 
            
            // Draw Ref Body
            myPen.Color = Color.Yellow;

            float sunRadius = 10.0F;

            brush.Color = Color.Yellow;

            graph.FillEllipse(brush, x0 - sunRadius, y0 - sunRadius, 2*sunRadius, 2*sunRadius);

            // Draw Planet
            float radius = 5.0F;

            brush.Color = Color.Green;

            graph.FillEllipse(brush, x - radius, y - radius, 2 * radius, 2 * radius);
        }
    }   
}

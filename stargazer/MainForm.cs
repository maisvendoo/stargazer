﻿using System;
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
                {
                    BodiesList.Items.Add(data.name);
                    comboHomanArrivList.Items.Add(data.name);
                    comboHomanDepList.Items.Add(data.name);
                }
            }

            BodiesList.SelectedIndex = 0;
            comboHomanArrivList.SelectedIndex = 0;
            comboHomanDepList.SelectedIndex = 5;

            comboHomanArrivList.SelectedText = "Kerbin";
            comboHomanDepList.SelectedText = "Duna";

            for (int i = 1; i <= KCalendar.Days; i++)
            {
                comboDay.Items.Add(i.ToString());
                comboBeginDay.Items.Add(i.ToString());
                comboEndDay.Items.Add(i.ToString());
            }

            comboDay.SelectedIndex = 0;
            comboBeginDay.SelectedIndex = 0;
            comboEndDay.SelectedIndex = 0;

            for (int i = 0; i < KCalendar.Hours; i++)
            {
                comboHour.Items.Add(i.ToString());
                comboBeginHour.Items.Add(i.ToString());
                comboEndHour.Items.Add(i.ToString());
            }

            comboHour.SelectedIndex = 0;
            comboBeginHour.SelectedIndex = 0;
            comboEndHour.SelectedIndex = 0;

            for (int i = 0; i < KCalendar.Mins; i++)
            {
                comboMin.Items.Add(i.ToString());
                comboBeginMin.Items.Add(i.ToString());
                comboEndMin.Items.Add(i.ToString());
            }

            comboMin.SelectedIndex = 0;
            comboBeginMin.SelectedIndex = 0;
            comboEndMin.SelectedIndex = 0;

            for (int i = 0; i < KCalendar.Secs; i++)
            {
                comboSec.Items.Add(i.ToString());
                comboBeginSec.Items.Add(i.ToString());
                comboEndSec.Items.Add(i.ToString());
            }

            comboSec.SelectedIndex = 0;
            comboBeginSec.SelectedIndex = 0;
            comboEndSec.SelectedIndex = 0;
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

        private void buttonHomanSearch_Click(object sender, EventArgs e)
        {
            double t0 = KCalendar.date_to_sec(int.Parse(textBeginYear.Text.ToString()),
                                            int.Parse(comboBeginDay.Text.ToString()),
                                            int.Parse(comboBeginHour.Text.ToString()),
                                            int.Parse(comboBeginMin.Text.ToString()),
                                            int.Parse(comboBeginSec.Text.ToString()));

            double t1 = KCalendar.date_to_sec(int.Parse(textEndYear.Text.ToString()),
                                            int.Parse(comboEndDay.Text.ToString()),
                                            int.Parse(comboEndHour.Text.ToString()),
                                            int.Parse(comboEndMin.Text.ToString()),
                                            int.Parse(comboEndSec.Text.ToString()));

            int a_idx = get_body_index(comboHomanArrivList.Text.ToString());
            int d_idx = get_body_index(comboHomanDepList.Text.ToString());

            Transfer trans = new Transfer();

            double psi = Convert.ToDouble(textPsi.Text) * RAD;

            bool ready = Lambert.get_transfer_date(t0, t1, Bodies[a_idx], Bodies[d_idx], psi, ref trans);

            if (!ready)
            {
                panelHomanRes.Visible = false;
                return;
            }

            panelHomanRes.Visible = true;

            labelArivDate.Text = trans.arivDate.year.ToString() + "y " +
                                 trans.arivDate.day.ToString() + "d " +
                                 trans.arivDate.hour.ToString() + "h " +
                                 trans.arivDate.min.ToString() + "m " +
                                 trans.arivDate.sec.ToString() + "s";

            labelDepDate.Text = trans.depDate.year.ToString() + "y " +
                                 trans.depDate.day.ToString() + "d " +
                                 trans.depDate.hour.ToString() + "h " +
                                 trans.depDate.min.ToString() + "m " +
                                 trans.depDate.sec.ToString() + "s";
            
            labelSmiMajorAxis.Text = Math.Round(trans.orbit.a, 0).ToString() + " m";
            labelEccentricity.Text = Math.Round(trans.orbit.e, 4).ToString();
            labelInclination.Text = Math.Round(trans.orbit.i, 4).ToString() + " deg";
            labelLAN.Text = Math.Round(trans.orbit.Omega, 4).ToString() + " deg";
            labelArgPe.Text = Math.Round(trans.orbit.omega, 4).ToString() + " deg";

            CelestialBody craft = new CelestialBody();

            BodyData craft_data = new BodyData();

            craft_data.name = "Space craft";
            craft_data.orbit = trans.orbit;

            craft.set_data(craft_data);
            
            DrawTransOrbit(panelTransOrbit, trans, Bodies[a_idx], Bodies[d_idx], craft);
        }


        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        void DrawTransOrbit(Panel panel, Transfer trans, 
                            CelestialBody arivBody, 
                            CelestialBody destBody, 
                            CelestialBody craft)
        {
            Graphics graph = panel.CreateGraphics();

            graph.Clear(Color.Black);

            Pen pen = new Pen(Color.LightGray, 2.0F);

            int width = panel.Width;
            int height = panel.Height;

            float x0 = width / 2;
            float y0 = height / 2;

            float x = 0;
            float y = 0;

            float x1 = 0;
            float y1 = 0;

            float x2 = 0;
            float y2 = 0;
            
            double scale = 0;
            int Delta = 25;
            float delta = 5.0F;

            BodyData ariv_data = new BodyData();
            BodyData dest_data = new BodyData();

            arivBody.get_data(ref ariv_data);
            destBody.get_data(ref dest_data);

            double ariv_ra = ariv_data.orbit.a / (1 - ariv_data.orbit.e);
            double dest_ra = dest_data.orbit.a / (1 - dest_data.orbit.e);

            double r_max = 0;

            if (ariv_ra > dest_ra)
                r_max = ariv_ra;
            else
                r_max = dest_ra;

            if (width > height)
            {
                scale = (height / 2 - Delta) / r_max;                
            }
            else
            {
                scale = (width / 2 - Delta) / r_max;                
            }

            // Arrive Body orbit
            Vector3D pos;
            OrbitPos orbit_pos = new OrbitPos();

            double V0 = 0;
            double V = V0;
            double V1 = 360.0;
            double dV = 5.0;

            while (V <= V1)
            {
                pos = arivBody.get_cartesian_pos(V * RAD);

                x1 = x0 + Convert.ToSingle(scale * pos.x);
                y1 = y0 - Convert.ToSingle(scale * pos.y);

                V += dV;

                pos = arivBody.get_cartesian_pos(V * RAD);

                x2 = x0 + Convert.ToSingle(scale * pos.x);
                y2 = y0 - Convert.ToSingle(scale * pos.y);

                if (pos.z >= 0)
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                else
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                graph.DrawLine(pen, x1, y1, x2, y2);
            }

            // Departure Body orbit
            V0 = 0;
            V = V0;
            V1 = 360.0;
            dV = 5.0;

            while (V <= V1)
            {
                pos = destBody.get_cartesian_pos(V * RAD);

                x1 = x0 + Convert.ToSingle(scale * pos.x);
                y1 = y0 - Convert.ToSingle(scale * pos.y);

                V += dV;

                pos = destBody.get_cartesian_pos(V * RAD);

                x2 = x0 + Convert.ToSingle(scale * pos.x);
                y2 = y0 - Convert.ToSingle(scale * pos.y);

                if (pos.z >= 0)
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                else
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                graph.DrawLine(pen, x1, y1, x2, y2);
            }


            // Transfer orbit
            if (ariv_ra < dest_ra)
            {
                V0 = 0;
                V = V0;
                V1 = Lambert.get_dest_theta(craft, trans.destLambda) / RAD;
                dV = 5.0;                
            }
            else
            {                
                V0 = 180.0;
                V = V0;
                V1 = 360 + Lambert.get_dest_theta(craft, trans.destLambda) / RAD; ; 
                dV = 5.0;                
            }

            pen.Color = Color.Red;
            

            do
            {
                pos = craft.get_cartesian_pos(V * RAD);

                x1 = x0 + Convert.ToSingle(scale * pos.x);
                y1 = y0 - Convert.ToSingle(scale * pos.y);

                V += dV;

                pos = craft.get_cartesian_pos(V * RAD);

                x2 = x0 + Convert.ToSingle(scale * pos.x);
                y2 = y0 - Convert.ToSingle(scale * pos.y);

                if (pos.z >= 0)
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                else
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                graph.DrawLine(pen, x1, y1, x2, y2);                

            } while (V <= V1);

            // Destination body position
            SolidBrush brush = new SolidBrush(Color.Blue);

            // Draw Planet
            float radius = 5.0F;

            destBody.get_position(trans.depTime, ref orbit_pos);
            pos = destBody.get_cartesian_pos(orbit_pos.theta);

            x = x0 + Convert.ToSingle(scale * pos.x);
            y = y0 - Convert.ToSingle(scale * pos.y);

            graph.FillEllipse(brush, x - radius, y - radius, 2 * radius, 2 * radius);

            arivBody.get_position(trans.depTime, ref orbit_pos);
            pos = arivBody.get_cartesian_pos(orbit_pos.theta);

            x = x0 + Convert.ToSingle(scale * pos.x);
            y = y0 - Convert.ToSingle(scale * pos.y);

            graph.FillEllipse(brush, x - radius, y - radius, 2 * radius, 2 * radius);


            brush.Color = Color.Red;

            // Draw Planet
            destBody.get_position(trans.arivTime, ref orbit_pos);
            pos = destBody.get_cartesian_pos(orbit_pos.theta);

            x = x0 + Convert.ToSingle(scale * pos.x);
            y = y0 - Convert.ToSingle(scale * pos.y);

            graph.FillEllipse(brush, x - radius, y - radius, 2 * radius, 2 * radius);

            arivBody.get_position(trans.arivTime, ref orbit_pos);
            pos = arivBody.get_cartesian_pos(orbit_pos.theta);

            x = x0 + Convert.ToSingle(scale * pos.x);
            y = y0 - Convert.ToSingle(scale * pos.y);

            graph.FillEllipse(brush, x - radius, y - radius, 2 * radius, 2 * radius);

            float sunRadius = 10.0F;

            if (ariv_data.refBody == "Sun")
                brush.Color = Color.Yellow;
            else
                brush.Color = Color.Blue;

            graph.FillEllipse(brush, x0 - sunRadius, y0 - sunRadius, 2 * sunRadius, 2 * sunRadius);
        }
    }   
}

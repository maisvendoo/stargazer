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

using EphemeridesCalc;

namespace stargazer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            bodies = new List<TBodyData>();

            ReadSystemConfig("../../cfg/kerbal.xml");

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
        private TBodyData get_body_index()
        {
            int idx = 0;
            
            while ((idx < BodiesList.Items.Count) && (BodiesList.SelectedItem != bodies.ElementAt(idx).name))
            {
                idx++;
            }

            if (idx == BodiesList.Items.Count)
                idx = -1;

            return bodies.ElementAt(idx);
        }



        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private void buttonEphCalc_Click(object sender, EventArgs e)
        {
            TBodyData body_data = get_body_index();            
        }
    }   
}

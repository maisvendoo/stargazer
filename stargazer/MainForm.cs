using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EphemeridesCalc;

namespace stargazer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CPlanet planet = new CPlanet();

            TOrbitData orbit_data = new TOrbitData();
            TPlanetState planet_state = new TPlanetState();

            orbit_data.e = 0.05099999912;
            orbit_data.M0 = 3.14;
            orbit_data.t0 = 0;
            orbit_data.a = 20726155264.0;

            CCalendar cal = new CCalendar();

            planet.get_planet_state(orbit_data, cal.date_to_sec(31, 346, 5, 32, 0), ref planet_state);

            label1.Text = planet_state.theta.ToString();
            label2.Text = planet_state.r.ToString();
        }
    }
}

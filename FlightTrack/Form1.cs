/*********************************************************************
 *  Northeast VA Flight Tracker
 *  Copyright (C) 2018 Daniel A. Hawton (daniel@hawton.com)
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *  
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;

namespace FlightTrack
{
    public partial class Form1 : MetroForm
    {
        private int _panelX = 0;
        private int _panelY = 64;
        private int _panelSizeX = 400;
        private int _panelSizeY = 500;
        
        public Form1()
        {
            InitializeComponent();

            this.MaximizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbUsername.Focus();
        }

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            btnSignIn.Enabled = false;
            ToggleSpinner();
            
            try
            {
                bool chk = await Task.Run(() => API.Login(tbUsername.Text, tbPassword.Text));
                if (chk)
                {
                    MessageBox.Show(@"Success");
                    return;
                }
            }
            catch (WebException webException)
            {
                if (((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.Unauthorized)
                {
                    MetroMessageBox.Show(this, @"Login invalid. Please check your username and password.", @"Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MetroMessageBox.Show(this,
                        @"There was an error attempting to connect to the API.  Please try again later. " +
                        (int) ((HttpWebResponse)webException.Response).StatusCode,
                        @"Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ToggleSpinner();
            btnSignIn.Enabled = true;
            return;
        }

        private void ToggleSpinner()
        {
            if (imgSpinner.Enabled)
            {
                imgSpinner.Enabled = false;
                imgSpinner.Visible = false;
            }
            else
            {
                imgSpinner.Enabled = true;
                imgSpinner.Visible = true;
                imgSpinner.BringToFront();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Chess
{
    public partial class ModePicker : Form
    {
        public ModePicker()
        {
            InitializeComponent();
        }

        private void btn_PvP_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Player Vs Player selected");
            MoveGeneration.playingVsAI = false;
            this.Close();
        }

        private void btn_PvAI_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Player Vs AI selected");
            MoveGeneration.playingVsAI = true;
            this.Close();
        }

        private void ModePicker_Load(object sender, EventArgs e)
        {

        }
    }
}

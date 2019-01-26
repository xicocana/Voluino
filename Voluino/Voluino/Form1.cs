using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voluino
{
    public partial class Form1 : Form
    {

        String msg;
        string[] tokens;
        IEnumerable<CoreAudioDevice> devices = new CoreAudioController().GetPlaybackDevices();

        public Form1()
        {
            InitializeComponent();
            serialPort1.Open();
            timer1.Enabled = true;

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //handling of the msg
            msg = serialPort1.ReadLine();
            tokens = msg.Split('%');
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = tokens[0];

            foreach (CoreAudioDevice d in devices)
            {
                if (d.IsDefaultDevice)
                    d.Volume = Int32.Parse(tokens[0]);
            }

            if (tokens[1].Length == 3)
            {
                if (isDef("Headphones (2- Razer Kraken USB)") == false)
                {
                    ChangeOutput("Headphones (2- Razer Kraken USB)");
                    label4.Text = "Headphones (2- Razer Kraken USB)";
                }
            }
            if (tokens[1].Length == 4)
            {
                if (isDef("Speakers (Realtek High Definition Audio)") == false)
                {
                    ChangeOutput("Speakers (Realtek High Definition Audio)");
                    label4.Text = "Speakers (Realtek High Definition Audio)";
                }
            }
        }

        private void ChangeOutput(string op)
        {
            foreach (CoreAudioDevice d in devices)
            {
                if (d.FullName == op)
                    d.SetAsDefault();
            }
        }

        private bool isDef(string op)
        {
            foreach (CoreAudioDevice d in devices)
            {
                if (d.IsDefaultDevice && d.FullName == op)
                    return true;
            }
            return false;
        }
    }
}
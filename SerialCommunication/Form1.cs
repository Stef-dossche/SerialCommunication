using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        private SerialPort serialPortArduino;

        public Form1()
        {
            InitializeComponent();
            serialPortArduino = new SerialPort();
            serialPortArduino.ReadTimeout = 1000;
            serialPortArduino.WriteTimeout = 1000;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (!serialPortArduino.IsOpen)
            {
                // Connect
                try
                {
                    serialPortArduino.PortName = comboBoxPoort.SelectedItem?.ToString();
                    serialPortArduino.BaudRate = int.Parse(comboBoxBaudrate.SelectedItem?.ToString() ?? "115200");
                    serialPortArduino.DataBits = 8;
                    serialPortArduino.Parity = Parity.None;
                    serialPortArduino.StopBits = StopBits.One;
                    serialPortArduino.Handshake = Handshake.None;
                    serialPortArduino.RtsEnable = true;
                    serialPortArduino.DtrEnable = true;

                    serialPortArduino.Open();

                    // Send ping and wait for pong
                    serialPortArduino.WriteLine("ping");
                    string response = serialPortArduino.ReadLine()?.Trim();

                    if (response != "pong")
                    {
                        serialPortArduino.Close();
                        MessageBox.Show("Arduino antwoord ongeldig. Verwacht 'pong' maar kreeg: " + (response ?? "geen antwoord"),
                            "Ping Test Mislukt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Update UI on success
                    radioButtonVerbonden.Checked = true;
                    buttonConnect.Text = "Disconnect";
                    labelStatus.Text = $"Verbonden met {serialPortArduino.PortName} ({serialPortArduino.BaudRate} baud)";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fout bij verbinden: " + ex.Message, "Verbindingsfout",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (serialPortArduino.IsOpen)
                        serialPortArduino.Close();
                }
            }
            else
            {
                // Disconnect
                try
                {
                    serialPortArduino.Close();
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                    labelStatus.Text = "Verbroken";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fout bij verbrinden: " + ex.Message, "Verbreekingsfout",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    MessageBox.Show("Seriële verbinding is niet geopend.", "Fout",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    checkBoxDigital2.Checked = false;
                    return;
                }

                string command = checkBoxDigital2.Checked ? "set d2 high" : "set d2 low";
                serialPortArduino.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij verzenden van commando: " + ex.Message, "Verzendingsfout",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBoxDigital2.Checked = false;
            }
        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    MessageBox.Show("Seriële verbinding is niet geopend.", "Fout",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    checkBoxDigital3.Checked = false;
                    return;
                }

                string command = checkBoxDigital3.Checked ? "set d3 high" : "set d3 low";
                serialPortArduino.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij verzenden van commando: " + ex.Message, "Verzendingsfout",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBoxDigital3.Checked = false;
            }
        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    MessageBox.Show("Seriële verbinding is niet geopend.", "Fout",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    checkBoxDigital4.Checked = false;
                    return;
                }

                string command = checkBoxDigital4.Checked ? "set d4 high" : "set d4 low";
                serialPortArduino.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij verzenden van commando: " + ex.Message, "Verzendingsfout",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBoxDigital4.Checked = false;
            }
        }
    }
}

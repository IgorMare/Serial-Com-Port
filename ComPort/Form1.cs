using System;
using System.IO.Ports;
using System.Windows.Forms;


namespace ComPort
{
    public partial class Form1 : Form
    {
        //Declaring class-level variable for storing outgoing data
        string dataOut;
        // Global variable String Data
        string sndWith;
        //Global Data IN variable
        string dataIn;

        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Collecting port names and putting them in array of strings
            string[] ports = SerialPort.GetPortNames();
            // Displaying port names in combo box COM PORT
            cBoxComPort.Items.AddRange(ports);

            //Setting initial state of Open Close buttons
            btnOpen.Enabled = true;
            btnClose.Enabled = false;

            //Making shure that DTR port check box is unchecked (this is initial state)
            chBoxDTREnable.Checked = false;
            serialPort1.DtrEnable = false;

            //Making shure that RTS check box is unchecked (initial state)
            chBoxRTSEnable.Checked = false;
            serialPort1.RtsEnable = false;

            //Send button initial condition
            btnSendData.Enabled = false;

            // Initial state of Wrie and WriteLine
            chBoxWriteLine.Checked = false;
            chBoxWrite.Checked = true;
            // Initial state is using Write
            sndWith = "Write";

            // Setting inital state of check Boxes 
            chBoxAlwaysUpdate.Checked = true;
            chBoxAddToOldData.Checked = false;

        }
        // Open button method
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cBoxComPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(cBoxBaudRate.Text);
                serialPort1.DataBits = Convert.ToInt32(cBoxDataBits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxStopBits.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxParityBits.Text);
                serialPort1.Open();
                progressBar1.Value = 100;
                // Preventing clicking Open button while connection is running
                btnOpen.Enabled = false;
                btnClose.Enabled = true;
                // Changing state of COM label
                lblStatusCom.Text = "ON";
                // Changing the color just for fun, and it looks more serios :)
                lblStatusCom.ForeColor = System.Drawing.Color.Green;

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
                lblStatusCom.Text = "OFF";
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Checking the state of serial port1
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                progressBar1.Value = 0;
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
                lblStatusCom.Text = "OFF";
            }
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            //Checking if serial port is open for communication
            if (serialPort1.IsOpen)
            {
                //Assigning value from textbox to class-level variable
                dataOut = tBoxDataOut.Text;
                // Determining weather to use Write or WriteLine
                if (sndWith == "WriteLine")
                {
                    serialPort1.WriteLine(dataOut);
                }
                else if (sndWith == "Write")
                {
                    //Passing dataOut to WriteLine method for output 
                    serialPort1.Write(dataOut);
                }

            }
        }
        // This method is executed when a state of check box DTR ENABLE changes
        private void chBoxDTREnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxDTREnable.Checked)
            {
                //If DTR is enabled then activate DTR for serial port 1
                serialPort1.DtrEnable = true;
                //Pop Up
                MessageBox.Show("DTR Enabled", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //Dectivate DTR for serial port 1
                serialPort1.DtrEnable = false;
            }
        }

        private void chBoxRTSEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxRTSEnable.Checked)
            {
                //Ig RTS chack box is enabled then activate RTS
                serialPort1.RtsEnable = true;
                //Pop Up
                MessageBox.Show("RTS Enabled", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //Deactivate RTS for serial port 1
                serialPort1.RtsEnable = false;
            }
        }

        private void btnClearDataOut_Click(object sender, EventArgs e)
        {
            //Checking if text box data is empty
            if (tBoxDataOut.Text != "")
            {
                //If data is not empty, clear data 
                tBoxDataOut.Text = "";
            }
        }

        private void tBoxDataOut_TextChanged(object sender, EventArgs e)
        {
            // Getting the length of text and assigning it to variable
            int dataOUTLength = tBoxDataOut.TextLength;
            // outputing data from dataOutLength to label and formating it to 2 decimals
            lblDataOutLength.Text = string.Format("{0:00}", dataOUTLength);
            //Preventing enter key to create new line if Using enter is checked
            if (chBoxUsingEnter.Checked)
            {
                //REplaces Enter (new line) with empty string 
                tBoxDataOut.Text = tBoxDataOut.Text.Replace(Environment.NewLine, "");
            }
        }

        private void chBoxUsingButton_CheckedChanged(object sender, EventArgs e)
        {
            //Enabeling Send Data button in dependency on state of Using button check box
            if (chBoxUsingButton.Checked)
            {
                btnSendData.Enabled = true;
            }
            else
            {
                btnSendData.Enabled = false;
            }
        }
        
        private void chBoxWriteLine_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxWriteLine.Checked)
            {
                //Store string so it can be dertermend whato to use WriteLine or Write
                sndWith = "WriteLine";
                chBoxWrite.Checked = false;
                chBoxWriteLine.Checked = true;
            }
        }

        private void chBoxWrite_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxWrite.Checked)
            {
                sndWith = "Write";
                chBoxWrite.Checked = true;
                chBoxWriteLine.Checked = false;
            }
        }

        private void tBoxDataOut_KeyDown(object sender, KeyEventArgs e)
        {
            if (chBoxUsingEnter.Checked)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (serialPort1.IsOpen)
                    {
                        dataOut = tBoxDataOut.Text;
                        serialPort1.Write(dataOut);
                    }
                }
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            dataIn = serialPort1.ReadExisting();
            //Method for displaying data in textbox, data can not be shown directly without this method
            this.Invoke(new EventHandler(ShowData));
        }

        private void ShowData(object sender, EventArgs e)
        {
            // Serial data input length
            int dataInLength = dataIn.Length;
            lblDataInLength.Text = string.Format("{0:00}", dataInLength);
            
            // Making shure that ShowData always dispalys data (Old Data or Update Data)
            if (chBoxAlwaysUpdate.Checked)
            {
                tBoxDataIN.Text = dataIn;
            }
            else if (chBoxAddToOldData.Checked)
            {
                tBoxDataIN.Text += dataIn;
            }
        }

        private void chBoxAlwaysUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAlwaysUpdate.Checked)
            {
                chBoxAlwaysUpdate.Checked = true;
                chBoxAddToOldData.Checked = false;
            }
            else
            {
                chBoxAddToOldData.Checked = true;
            }
        }

        private void chBoxAddToOldData_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAddToOldData.Checked)
            {
                chBoxAlwaysUpdate.Checked = false;
                chBoxAddToOldData.Checked = true;
            }
            else
            {
                chBoxAlwaysUpdate.Checked = false;
            }
        }

        private void btnClearDataIN_Click(object sender, EventArgs e)
        {
            if (btnClearDataIN.Text != "")
            {
                tBoxDataIN.Text = "";
            }
        }
    }
}

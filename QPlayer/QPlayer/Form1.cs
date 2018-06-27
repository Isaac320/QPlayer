using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace QPlayer
{
    public partial class Form1 : Form
    {
        string args = "";

        /// <summary>
        /// Path of selected file
        /// </summary>
        string filename = null;
        Process ps = null;
        public Form1()
        {
            InitializeComponent();
            ps = new Process();

            //Path of Mplayer exe
            ps.StartInfo.FileName = "mplayer ";

            ps.StartInfo.UseShellExecute = false;
            ps.StartInfo.RedirectStandardInput = true;
            args = "-nofs -noquiet -identify -slave ";
            args += "-nomouseinput -sub-fuzziness 1 ";

            //-wid will tell MPlayer to show output inisde our panel
            args += " -vo direct3d, -ao dsound  -wid ";
            int id = (int)panel1.Handle;
            args += id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();
            if(opd.ShowDialog()==DialogResult.OK)
            {
                filename = opd.FileName;
                OpenFile();
            }
        }
        bool SendCommand(string cmd)
        {
            try
            {
                if (ps != null && ps.HasExited == false)
                {
                    ps.StandardInput.Write(cmd + "\n");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        void OpenFile()
        {
            if (filename == null)
                return;
            //Close any current playing media file
            try
            {
                ps.Kill();
            }
            catch
            {
            }
            try
            {
                ps.StartInfo.Arguments = args + " \"" + filename + "\"";
                ps.Start();
              //  SendCommand("set_property volume " + trkVolume.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SendCommand("pause") == false)
            {
                OpenFile();

            }
        }
    }
}

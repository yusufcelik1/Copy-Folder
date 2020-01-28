using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace Dosya2
{
    public partial class Form1 : Form
    {
        BackgroundWorker worker = new BackgroundWorker();
        public Form1()
        { 
            InitializeComponent();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
        }
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
            //Copies each folder to new index
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(System.IO.Path.Combine(target.FullName, fi.Name), true);
            }
            //Copies each Sub-Directory
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        void CopyFile(string source, string des)
        {
            FileStream fsout = new FileStream(des, FileMode.Create);
            FileStream fsIn = new FileStream(source, FileMode.Open);
            byte[] bt = new byte[1048756];
            int readByte;
            while ((readByte=fsIn.Read(bt,  0,  bt.Length))>0)
            {
                fsout.Write(bt, 0, readByte);
                worker.ReportProgress((int)(fsIn.Position*100/fsIn.Length));
            }
            fsIn.Close();
            fsout.Close();
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CopyAll(new DirectoryInfo( txtSource.Text), new DirectoryInfo(txtTarget.Text));
        }
        private void button3_Click(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
            label3.Visible =true;
            label3.Text = "İşlem Başarılı";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtTarget.Text = Path.Combine(fbd.SelectedPath, Path.GetFileName(txtSource.Text));
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(700,450);//The location where the program will be opened is determined here.
            txtSource.Text = @"D:\NewFolder";//From the predetermined location
            txtSource.Enabled = false; //User cannot make changes here
            label4.Text="Kopyalanacağı yeri seçiniz:" ;
            label3.Visible = false;

        }
    }
}

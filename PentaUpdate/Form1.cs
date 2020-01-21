using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PentaUpdate
{
    public partial class Form1 : Form
    {
        public Log log = new Log();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChiusuraProgrammi();
            Task.Run(AvvioAggiornamento);
        }

        private void ChiusuraProgrammi()
        {
            ChiusuraProgramma("FattElett");
            ChiusuraProgramma("ScontrinoPenta");
            ChiusuraProgramma("PentaStart");
            ChiusuraProgramma("Export-Clienti");
        }

        private void AggiornamentoScontrinoPenta()
        {
            if (!ControlloVersione("ScontrinoPenta.exe"))
            {
                labelAgg.Text = "Aggiornamento ScontrinoPenta";
                Log.WriteLog("Avvio aggiornamento ScontrinoPenta");
                ScaricareZip("ScontrinoPenta");
                ScompattareFile("ScontrinoPenta");
                CancellaFileUpdate("ScontrinoPenta");
                Log.WriteLog("Fine aggiornamento ScontrinoPenta");
            }
        }

        private void ChiusuraProgramma(string process)
        {
            Process[] proc = Process.GetProcessesByName(process);

            foreach (var item in proc)
            {
                item.Kill();
            }
        }

        private void CancellaFileUpdate(string filezip)
        {
            if (File.Exists(Application.StartupPath + "\\" + filezip + ".Update.zip"))
            {
                try
                {
                    File.Delete(Application.StartupPath + "\\" + filezip + ".Update.zip");
                }
                catch (Exception ex)
                {
                    Log.WriteLog("Errore nella cancellazione del file " + filezip + ".Update.zip");
                    Log.WriteLog("Errore nella cancellazione del file: " + ex.ToString());
                }
            }
        }

        private void ScompattareFile(string filezip)
        {
            try
            {
                DateTime Inizio = DateTime.Now;
                Log.WriteLog("Avvio scompattamento file: " + filezip);
                FileStream file = new FileStream(Application.StartupPath + "\\" + filezip + ".Update.zip", FileMode.Open);
                ZipArchive zip = new ZipArchive(file);
                ZipArchiveExtensions.ExtractToDirectory(zip, Application.StartupPath,true);
                zip.Dispose();
                file.Dispose();
                Log.WriteLog("Fine scompattamento: " + filezip + " (" + DateTime.Now.Subtract(Inizio).TotalSeconds + " secondi)");
            }
            catch (Exception ex)
            {
                Log.WriteLog("Errore aggiornamento: " + ex.ToString());
            }
        }

        private void ScaricareZip(string filezip)
        {
            using (WebClient Client = new WebClient())
            {
                DateTime Inizio = DateTime.Now;
                Log.WriteLog("Avvio scaricamento: " + filezip);
                string UrlZip = "http://www.pentaelectronic.eu/scarica/" + filezip + "/" + filezip + ".zip";
                Client.DownloadFile(UrlZip, filezip + ".Update.zip");
                Log.WriteLog("Fine scaricamento: " + filezip + " (" + DateTime.Now.Subtract(Inizio).TotalSeconds + " secondi)");
            }
        }

        private bool ControlloVersione(string file)
        {
            string versionefile = FileVersionInfo.GetVersionInfo(Application.StartupPath + "\\" + file).ProductVersion;
            string versioneonline = XDocument.Load("http://www.pentaelectronic.eu/scarica/" + file.Substring(0,file.Length - 4) + "/Update.xml").Root.Element("Versione").Value;
            if (versionefile != versioneonline)
            {
                Log.WriteLog("Trovata nuova versione: " + file);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
        }

        private void AvvioAggiornamento()
        {
            log.InizializzareLog();
            Log.WriteLog("------------------------------------------------------------");
            Log.WriteLog("Avvio servizio update PentaUpdate");
            Log.WriteLog("Controllo nuove versioni in corso...");
            AggiornamentoPentaStart();
            AggiornamentoScontrinoPenta();
            //AggiornamentoFatturazioneElettronica();
            //AggiornamentoExportClienti();
            Process.Start("C:/trilogis/PentaStart.exe");
            Application.Exit();
        }

        private void AggiornamentoPentaStart()
        {
            if (!ControlloVersione("PentaStart.exe"))
            {
                this.Invoke((MethodInvoker)delegate { this.labelAgg.Text = "Aggiornamento PentaStart"; });
                Log.WriteLog("Avvio aggiornamento PentaStart");
                ScaricareZip("PentaStart");
                ScompattareFile("PentaStart");
                CancellaFileUpdate("PentaStart");
                Log.WriteLog("Fine aggiornamento PentaStart");
            }
        }
    }

    public static class ZipArchiveExtensions
    {
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }

            DirectoryInfo di = Directory.CreateDirectory(destinationDirectoryName);
            string destinationDirectoryFullPath = di.FullName;

            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, file.FullName));

                if (!completeFileName.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                {
                    throw new IOException("Trying to extract file outside of destination directory. See this link for more info: https://snyk.io/research/zip-slip-vulnerability");
                }

                if (file.Name == "")
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
        }
    }
}

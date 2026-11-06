using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Updater;

public class Update : Form
{
    public static bool flg1;

    public static bool flg2;

    private Stopwatch sw = new Stopwatch();

    private string url;

    private string path;

    private string filename;

    private bool done;

    private int T;

    private IContainer components;

    private Button button1;

    private ProgressBar progressBar1;

    private Label labelSpeed;

    private Label labelDownloaded;

    private Label Total_Size;

    private Label label1;

    private System.Windows.Forms.Timer timer1;

    private System.Windows.Forms.Timer timeoutTimer;

    private int currentServer = 0;

    private WebClient webClient;

    private readonly string[] exeServers = new string[3] { "http://103.148.67.81/ByteNova.exe.txt", "http://103.148.67.81/ByteNova.exe.txt", "https://novabyte.in/Api/ByteNova.exe.txt" };

    private readonly string[] dllServers = new string[3] { "http://103.148.67.81/ByteNovaDLL.zip", "http://103.148.67.81/ByteNovaDLL.zip", "https://novabyte.in/ByteNovaDLL.zip" };

    private string[] currentServers;

    private DateTime slowStart = DateTime.MinValue;

    private const double MinSpeed = 50.0;

    private const int SlowTime = 20;

    public Update()
    {
        InitializeComponent();
        ApplyByteNovaUpdaterStyle();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
    }

    public static void CleanOrphanConfigs()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string[] files = Directory.GetFiles(baseDirectory, "*.exe.config", SearchOption.TopDirectoryOnly);
        string[] array = files;
        foreach (string text in array)
        {
            string fileName = Path.GetFileName(text);
            string path = fileName.Replace(".config", "");
            if (!File.Exists(Path.Combine(baseDirectory, path)))
            {
                try
                {
                    File.Delete(text);
                    Console.WriteLine("Deleted orphan config: " + fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot delete " + fileName + ": " + ex.Message);
                }
            }
        }
    }
    private void Form1_Load(object sender, EventArgs e)
    {
        path = Application.StartupPath;
        CleanOrphanConfigs();
        Process[] processes = Process.GetProcesses();
        Process[] array = processes;
        Process[] array2 = array;
        foreach (Process process in array2)
        {
            try
            {
                string fileName = process.MainModule.FileVersionInfo.FileName;
                string fileDescription = process.MainModule.FileVersionInfo.FileDescription;
                if (fileDescription == "ByteNova" || fileDescription == "ByteNova")
                {
                    process.Kill();
                    Thread.Sleep(300);
                    File.Delete(fileName);
                }
            }
            catch
            {
            }
        }
        if (File.Exists(Path.Combine(path, "MySql.Data.dll.tmp")))
        {
            File.Delete(Path.Combine(path, "MySql.Data.dll.tmp"));
        }
        int num = 0;
        while (true)
        {
            try
            {
                string fileName2 = "ByteNova.exe.config";
                if (File.Exists(fileName2))
                {
                    File.Delete(fileName2);
                }
                Uri address = new Uri("https://raw.githubusercontent.com/bytenova-dot/masterdata/refs/heads/main/ByteNova.exe.config");
                new WebClient().DownloadFile(address, fileName2);
            }
            catch
            {
                num++;
                if (num < 3)
                {
                    continue;
                }
            }
            break;
        }
        Control.CheckForIllegalCrossThreadCalls = false;
        using (InstalledFontCollection installedFontCollection = new InstalledFontCollection())
        {
            if (!installedFontCollection.Families.Any((FontFamily x) => x.Name.Contains("DS-Digital")))
            {
                try
                {
                    string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "DS-Digital.ttf");
                    if (File.Exists(text))
                    {
                        File.Delete(text);
                    }
                    string text2 = Path.Combine(Path.GetTempPath(), "DS-Digital.ttf");
                    Uri address3 = new Uri("https://raw.githubusercontent.com/bytenova-dot/masterdata/refs/heads/main/DS-DIGIT.TTF");
                    new WebClient().DownloadFile(address3, text2);
                    File.Copy(text2, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "DS-Digital.ttf"));
                    RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts");
                    registryKey.SetValue("DS-Digital", "DS-Digital.ttf");
                    registryKey.Close();
                    Application.Restart();
                }
                catch (Exception)
                {
                }
            }
        }
        if (!File.Exists(Path.Combine(path, "AngleSharp.dll")))
        {
            T = 0;
        }
        else
        {
            T = 5;
        }
        timer1.Enabled = true;
    }

    private bool check_file()
    {
        bool result = false;
        string[] array = new string[41]
        {
            "AngleSharp.dll", "BouncyCastle.Crypto.dll", "Bunifu_UI_v1.52.dll", "Driver.dll", "HVMRun64.dll", "HVMRuntm.dll", "Ionic.Zip.Reduced.dll", "libCaptchaC.dll", "MailKit.dll", "Microsoft.Bcl.AsyncInterfaces.dll",
            "Microsoft.IO.RecyclableMemoryStream.dll", "MimeKit.dll", "MReget.dll", "MsHtmlWeb.dll", "MurmurHash.dll", "MySql.Data.dll", "MySqlDataText.dll", "Newtonsoft.Json.dll", "Patagames.Ocr.dll", "PDFService.dll",
            "QRCodeDecoderLibrary.dll", "QRCoder.dll", "Select.HtmlToPdf.dll", "SeleniumExtras.WaitHelpers.dll", "System.Buffers.dll", "System.Data.SQLite.dll", "System.Data.SQLite.EF6.dll", "System.Drawing.Common.dll", "System.Memory.dll", "System.Net.Http.WinHttpHandler.dll",
            "System.Numerics.Vectors.dll", "System.Runtime.CompilerServices.Unsafe.dll", "System.Text.Encodings.Web.dll", "System.Text.Json.dll", "System.Text.RegularExpressions.dll", "System.Threading.Tasks.Extensions.dll", "System.ValueTuple.dll", "UndetectedChromeDriver.dll", "WebDriver.dll", "WebDriver.Support.dll",
            "zxing.dll"
        };
        string[] array2 = array;
        foreach (string path in array2)
        {
            if (!File.Exists(Path.Combine(this.path, path)))
            {
                result = true;
                break;
            }
        }
        return result;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        if (!done)
        {
            switch (T)
            {
                case 0:
                    T++;
                    done = true;
                    filename = "ByteNovaDLL.zip";
                    url = "https://raw.githubusercontent.com/bytenova-dot/masterdata/refs/heads/main/QRCoder.zip";
                    update();
                    break;
                case 5:
                    T++;
                    done = true;
                    filename = "ByteNova.exe";
                    url = "http://103.181.90.2/ByteNova.exe.txt";
                    update();
                    break;
            }
        }
    }

    private void update()
    {
        currentServer = 0;
        if (filename == "ByteNova.exe")
        {
            currentServers = exeServers;
        }
        else
        {
            currentServers = dllServers;
        }
        DownloadFromServer();
    }

    private void DownloadFromServer()
    {
        try
        {
            if (currentServer < currentServers.Length)
            {
                url = currentServers[currentServer];
                if (webClient != null)
                {
                    webClient.Dispose();
                }
                webClient = new WebClient();
                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.DownloadProgressChanged += webClient_0_DownloadProgressChanged;
                if (filename == "ByteNova.exe")
                {
                    webClient.DownloadDataCompleted += webClient_0_DownloadDataCompleted1;
                }
                else
                {
                    webClient.DownloadDataCompleted += webClient_0_DownloadDataCompleted;
                }
                if (timeoutTimer == null)
                {
                    timeoutTimer = new System.Windows.Forms.Timer();
                    timeoutTimer.Interval = 40000;
                    timeoutTimer.Tick += TimeoutTimer_Tick;
                }
                sw.Restart();
                slowStart = DateTime.MinValue;
                timeoutTimer.Start();
                webClient.DownloadDataAsync(new Uri(url));
            }
        }
        catch
        {
            currentServer++;
            DownloadFromServer();
        }
    }

    private void TimeoutTimer_Tick(object sender, EventArgs e)
    {
        timeoutTimer.Stop();
        if (webClient != null && webClient.IsBusy)
        {
            webClient.CancelAsync();
            currentServer++;
            DownloadFromServer();
        }
    }

    private void webClient_0_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        try
        {
            double num = (double)e.BytesReceived / 1024.0 / sw.Elapsed.TotalSeconds;
            labelSpeed.Text = num.ToString("0.00") + " KB/s";
            labelDownloaded.Text = ((double)e.BytesReceived / 1024.0 / 1024.0).ToString("0.00") + " / MB";
            Total_Size.Text = ((double)e.TotalBytesToReceive / 1024.0 / 1024.0).ToString("0.00") + " / MB";
            progressBar1.Value = e.ProgressPercentage;
            ByteNovaProgressUI(e.ProgressPercentage);
            if (!(sw.Elapsed.TotalSeconds > 3.0))
            {
                return;
            }
            if (num < 50.0)
            {
                if (slowStart == DateTime.MinValue)
                {
                    slowStart = DateTime.Now;
                }
                if ((DateTime.Now - slowStart).TotalSeconds >= 20.0)
                {
                    slowStart = DateTime.MinValue;
                    if (webClient != null && webClient.IsBusy)
                    {
                        timeoutTimer.Stop();
                        webClient.CancelAsync();
                    }
                }
            }
            else
            {
                slowStart = DateTime.MinValue;
            }
        }
        catch
        {
        }
    }

    private void webClient_0_DownloadDataCompleted1(object sender, DownloadDataCompletedEventArgs e)
    {
        timeoutTimer?.Stop();
        if (e.Cancelled || e.Error != null)
        {
            currentServer++;
            if (currentServer < currentServers.Length)
            {
                DownloadFromServer();
            }
            else
            {
                MessageBox.Show("All download servers failed.");
            }
            return;
        }
        string fileName = Path.Combine(path, filename);
        try
        {
            Process[] processesByName = Process.GetProcessesByName("ByteNova");
            foreach (Process process in processesByName)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit(5000);
                }
                catch
                {
                }
            }
            File.WriteAllBytes(fileName, e.Result);
            timer1.Enabled = false;
            Process.Start(fileName);
            try
            {
                Application.Exit();
                Environment.Exit(0);
            }
            catch
            {
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            currentServer++;
            if (currentServer < currentServers.Length)
            {
                DownloadFromServer();
            }
        }
    }

    private void webClient_0_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
    {
        string path = Path.Combine(Path.GetTempPath(), "NexusDllUpdate");
        Directory.CreateDirectory(path);
        string zipPath = Path.Combine(path, "libs_temp.zip");
        try
        {
            if (e.Error == null && !e.Cancelled)
            {
                byte[] result = e.Result;
                FileStream fileStream = new FileStream(zipPath, FileMode.Create);
                fileStream.Write(result, 0, result.Length);
                fileStream.Close();
                if (filename == "ByteNovaDLL.zip")
                {
                    RunPowerShellUpdate(this.path, zipPath);
                    T = 5;
                }
                done = false;
            }
        }
        catch (Exception)
        {
        }
    }

    private static void RunPowerShellUpdate(string currentFolder, string zipPath)
    {
        string path = Path.Combine(Path.GetTempPath(), "NexusDllUpdate");
        Directory.CreateDirectory(path);
        string text = Path.Combine(path, "update_libs.ps1");
        string contents = "\r\nparam(\r\n    [string]$CurrentFolder,\r\n    [string]$ZipPath\r\n)\r\n\r\n$dllFiles = @(\r\n    \"AngleSharp.dll\",\r\n    \"BouncyCastle.Crypto.dll\",\r\n    \"Bunifu_UI_v1.52.dll\",\r\n    \"Driver.dll\",\r\n    \"HVMRun64.dll\",\r\n    \"HVMRuntm.dll\",\r\n    \"Ionic.Zip.Reduced.dll\",\r\n    \"libCaptchaC.dll\",\r\n    \"MailKit.dll\",\r\n    \"Microsoft.Bcl.AsyncInterfaces.dll\",\r\n    \"Microsoft.IO.RecyclableMemoryStream.dll\",\r\n    \"MimeKit.dll\",\r\n    \"MReget.dll\",\r\n    \"MsHtmlWeb.dll\",\r\n    \"MurmurHash.dll\",\r\n    \"MySql.Data.dll\",\r\n    \"MySqlDataText.dll\",\r\n    \"Newtonsoft.Json.dll\",\r\n    \"Patagames.Ocr.dll\",\r\n    \"SeleniumExtras.WaitHelpers.dll\",\r\n    \"System.Buffers.dll\",\r\n    \"System.Data.SQLite.dll\",\r\n    \"System.Data.SQLite.EF6.dll\",\r\n    \"System.Drawing.Common.dll\",\r\n    \"System.Memory.dll\",\r\n    \"System.Net.Http.WinHttpHandler.dll\",\r\n    \"System.Numerics.Vectors.dll\",\r\n    \"System.Runtime.CompilerServices.Unsafe.dll\",\r\n    \"System.Text.Encodings.Web.dll\",\r\n    \"System.Text.Json.dll\",\r\n    \"System.Text.RegularExpressions.dll\",\r\n    \"System.Threading.Tasks.Extensions.dll\",\r\n    \"System.ValueTuple.dll\",\r\n    \"UndetectedChromeDriver.dll\",\r\n    \"WebDriver.dll\",\r\n    \"WebDriver.Support.dll\",\r\n    \"zxing.dll\"\r\n)\r\n\r\n# 1) listed DLLs delete\r\nforeach ($dll in $dllFiles) {\r\n    $path = Join-Path $CurrentFolder $dll\r\n    if (Test-Path $path) {\r\n        Remove-Item $path -Force\r\n    }\r\n}\r\n\r\n# 2) ZIP extract\r\nExpand-Archive -LiteralPath $ZipPath -DestinationPath $CurrentFolder -Force\r\n\r\n# 3) ZIP delete\r\nRemove-Item $ZipPath -Force\r\n";
        File.WriteAllText(text, contents, Encoding.UTF8);
        try
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "powershell.exe";
            processStartInfo.Arguments = "-ExecutionPolicy Bypass -File \"" + text + "\" -CurrentFolder \"" + currentFolder + "\" -ZipPath \"" + zipPath + "\"";
            processStartInfo.Verb = "runas";
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            using Process process = Process.Start(processStartInfo);
            process.WaitForExit();
        }
        finally
        {
            try
            {
                if (File.Exists(text))
                {
                    File.Delete(text);
                }
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }
            }
            catch
            {
            }
        }
    }


    private Panel _bnHeader;
    private Label _bnTitle;
    private Label _bnSubtitle;
    private Label _bnPercent;
    private Label _bnTotalTitle;
    private Label _bnDownloadedTitle;
    private Label _bnSpeedTitle;
    private Button _bnClose;

    private void ApplyByteNovaUpdaterStyle()
    {
        SuspendLayout();

        ClientSize = new Size(390, 176);
        BackColor = Color.FromArgb(3, 14, 31);
        ForeColor = Color.White;
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterScreen;
        DoubleBuffered = true;

        _bnHeader = new Panel();
        _bnHeader.SetBounds(1, 1, 388, 40);
        _bnHeader.BackColor = Color.FromArgb(3, 29, 59);
        _bnHeader.Paint += ByteNovaHeaderPaint;
        _bnHeader.MouseDown += ByteNovaDrag;
        Controls.Add(_bnHeader);
        _bnHeader.BringToFront();

        _bnTitle = new Label();
        _bnTitle.SetBounds(14, 4, 240, 20);
        _bnTitle.Text = "ByteNova Updater";
        _bnTitle.Font = new Font("Segoe UI Black", 10.5F, FontStyle.Bold);
        _bnTitle.ForeColor = Color.White;
        _bnTitle.BackColor = Color.Transparent;
        _bnTitle.MouseDown += ByteNovaDrag;
        _bnHeader.Controls.Add(_bnTitle);

        _bnSubtitle = new Label();
        _bnSubtitle.SetBounds(15, 23, 280, 13);
        _bnSubtitle.Text = "PREMIUM TATKAL SOFTWARE  •  SECURE UPDATE";
        _bnSubtitle.Font = new Font("Segoe UI Semibold", 6.5F, FontStyle.Bold);
        _bnSubtitle.ForeColor = Color.FromArgb(255, 211, 71);
        _bnSubtitle.BackColor = Color.Transparent;
        _bnSubtitle.MouseDown += ByteNovaDrag;
        _bnHeader.Controls.Add(_bnSubtitle);

        _bnClose = new Button();
        _bnClose.SetBounds(351, 8, 25, 23);
        _bnClose.Text = "×";
        _bnClose.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        _bnClose.ForeColor = Color.FromArgb(80, 215, 255);
        _bnClose.BackColor = Color.FromArgb(3, 25, 49);
        _bnClose.FlatStyle = FlatStyle.Flat;
        _bnClose.FlatAppearance.BorderSize = 1;
        _bnClose.FlatAppearance.BorderColor = Color.FromArgb(0, 195, 255);
        _bnClose.Cursor = Cursors.Hand;
        _bnClose.Click += delegate { Close(); };
        _bnHeader.Controls.Add(_bnClose);

        label1.AutoSize = false;
        label1.SetBounds(14, 48, 286, 21);
        label1.Text = "Checking for ByteNova updates...";
        label1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        label1.ForeColor = Color.White;
        label1.BackColor = Color.Transparent;

        _bnPercent = new Label();
        _bnPercent.SetBounds(306, 48, 70, 21);
        _bnPercent.Text = "0%";
        _bnPercent.Font = new Font("Segoe UI Black", 9.5F, FontStyle.Bold);
        _bnPercent.ForeColor = Color.FromArgb(0, 215, 255);
        _bnPercent.BackColor = Color.Transparent;
        _bnPercent.TextAlign = ContentAlignment.MiddleRight;
        Controls.Add(_bnPercent);

        progressBar1.SetBounds(14, 75, 362, 18);
        progressBar1.Style = ProgressBarStyle.Continuous;

        _bnTotalTitle = ByteNovaInfoTitle("TOTAL SIZE", 14);
        _bnDownloadedTitle = ByteNovaInfoTitle("DOWNLOADED", 137);
        _bnSpeedTitle = ByteNovaInfoTitle("DOWNLOAD SPEED", 260);
        Controls.Add(_bnTotalTitle);
        Controls.Add(_bnDownloadedTitle);
        Controls.Add(_bnSpeedTitle);

        ByteNovaInfoValue(Total_Size, 14);
        ByteNovaInfoValue(labelDownloaded, 137);
        ByteNovaInfoValue(labelSpeed, 260);

        button1.Visible = false;

        Paint -= ByteNovaUpdaterPaint;
        Paint += ByteNovaUpdaterPaint;

        ResumeLayout(true);
        PerformLayout();
        Invalidate();
    }

    private Label ByteNovaInfoTitle(string text, int x)
    {
        Label l = new Label();
        l.SetBounds(x, 105, 112, 14);
        l.Text = text;
        l.Font = new Font("Segoe UI Semibold", 6.5F, FontStyle.Bold);
        l.ForeColor = Color.FromArgb(120, 164, 196);
        l.BackColor = Color.Transparent;
        return l;
    }

    private void ByteNovaInfoValue(Label l, int x)
    {
        l.AutoSize = false;
        l.SetBounds(x, 120, 112, 25);
        l.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold);
        l.ForeColor = Color.FromArgb(255, 211, 71);
        l.BackColor = Color.FromArgb(5, 29, 53);
        l.TextAlign = ContentAlignment.MiddleLeft;
        l.Padding = new Padding(7, 0, 3, 0);
    }

    private void ByteNovaHeaderPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = _bnHeader.ClientRectangle;
        using (LinearGradientBrush b = new LinearGradientBrush(
            r, Color.FromArgb(2, 23, 52), Color.FromArgb(5, 53, 91),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(b, r);

        using (Pen p = new Pen(Color.FromArgb(0, 205, 255), 1.5F))
            e.Graphics.DrawLine(p, 0, r.Height - 1, r.Width, r.Height - 1);
    }

    private void ByteNovaUpdaterPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        using (Pen p = new Pen(Color.FromArgb(0, 195, 255), 1F))
            e.Graphics.DrawRectangle(p, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);

        Rectangle card = new Rectangle(8, 98, 373, 55);
        using (LinearGradientBrush bg = new LinearGradientBrush(
            card, Color.FromArgb(4, 22, 43), Color.FromArgb(3, 35, 61),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, card);

        using (Pen p = new Pen(Color.FromArgb(45, 115, 160), 1F))
            e.Graphics.DrawRectangle(p, card);

        using (Pen p = new Pen(Color.FromArgb(255, 211, 71), 1F))
            e.Graphics.DrawLine(p, 14, 157, 376, 157);

        using (Font f = new Font("Segoe UI Semibold", 6.5F, FontStyle.Bold))
        using (Brush b = new SolidBrush(Color.FromArgb(115, 157, 188)))
            e.Graphics.DrawString("Please keep this window open while ByteNova is updating.", f, b, 14, 160);
    }

    private void ByteNovaDrag(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        Capture = false;
        Message m = Message.Create(Handle, 0xA1, new IntPtr(2), IntPtr.Zero);
        WndProc(ref m);
    }

    private void ByteNovaProgressUI(int percentage)
    {
        if (_bnPercent != null)
            _bnPercent.Text = Math.Max(0, Math.Min(100, percentage)).ToString() + "%";

        if (label1 != null)
            label1.Text = percentage >= 100
                ? "Update downloaded successfully"
                : "Downloading latest ByteNova update...";
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.button1 = new System.Windows.Forms.Button();
        this.progressBar1 = new System.Windows.Forms.ProgressBar();
        this.labelSpeed = new System.Windows.Forms.Label();
        this.labelDownloaded = new System.Windows.Forms.Label();
        this.Total_Size = new System.Windows.Forms.Label();
        this.label1 = new System.Windows.Forms.Label();
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.SuspendLayout();
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(96, 301);
        this.button1.Margin = new System.Windows.Forms.Padding(2);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(82, 19);
        this.button1.TabIndex = 1;
        this.button1.Text = "button1";
        this.button1.UseVisualStyleBackColor = true;
        // 
        // progressBar1
        // 
        this.progressBar1.Location = new System.Drawing.Point(2, 4);
        this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new System.Drawing.Size(323, 25);
        this.progressBar1.TabIndex = 173;
        // 
        // labelSpeed
        // 
        this.labelSpeed.AutoSize = true;
        this.labelSpeed.Font = new System.Drawing.Font("DS-Digital", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.labelSpeed.Location = new System.Drawing.Point(242, 42);
        this.labelSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.labelSpeed.Name = "labelSpeed";
        this.labelSpeed.Size = new System.Drawing.Size(17, 18);
        this.labelSpeed.TabIndex = 174;
        this.labelSpeed.Text = "0";
        // 
        // labelDownloaded
        // 
        this.labelDownloaded.AutoSize = true;
        this.labelDownloaded.Font = new System.Drawing.Font("DS-Digital", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.labelDownloaded.Location = new System.Drawing.Point(127, 42);
        this.labelDownloaded.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.labelDownloaded.Name = "labelDownloaded";
        this.labelDownloaded.Size = new System.Drawing.Size(17, 18);
        this.labelDownloaded.TabIndex = 175;
        this.labelDownloaded.Text = "0";
        // 
        // Total_Size
        // 
        this.Total_Size.AutoSize = true;
        this.Total_Size.Font = new System.Drawing.Font("DS-Digital", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Total_Size.Location = new System.Drawing.Point(11, 42);
        this.Total_Size.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.Total_Size.Name = "Total_Size";
        this.Total_Size.Size = new System.Drawing.Size(17, 18);
        this.Total_Size.TabIndex = 176;
        this.Total_Size.Text = "0";
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.BackColor = System.Drawing.Color.Transparent;
        this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label1.ForeColor = System.Drawing.SystemColors.ControlDark;
        this.label1.Location = new System.Drawing.Point(-3, 60);
        this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(190, 26);
        this.label1.TabIndex = 177;
        this.label1.Text = "ByteNova Updating";
        // 
        // timer1
        // 
        this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        // 
        // Update
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
        this.ClientSize = new System.Drawing.Size(334, 94);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.Total_Size);
        this.Controls.Add(this.labelDownloaded);
        this.Controls.Add(this.labelSpeed);
        this.Controls.Add(this.progressBar1);
        this.Controls.Add(this.button1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Margin = new System.Windows.Forms.Padding(2);
        this.Name = "Update";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Updater";
        this.TopMost = true;
        this.Load += new System.EventHandler(this.Form1_Load);
        this.ResumeLayout(false);
        this.PerformLayout();

    }
}

using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImagesMove
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            //textBox1.Text = @"C:\Users\Narender Boyapally\Pictures";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void s_Click(object sender, EventArgs e)
        {
            lblStatus.Text = @"C:\Users\Narender Boyapally\Pictures";
            DirectoryInfo di = new DirectoryInfo(textBox1.Text);

            //File[] files = di.GetFiles("*.jp*", SearchOption.AllDirectories);
            FileInfo[] files = di.GetFiles("*.jp*", SearchOption.AllDirectories).Union(di.GetFiles("*.pn*", SearchOption.AllDirectories)).ToArray();

            foreach (var file in files)
            {
                DateTime dt = GetDateTakenFromImage(file.FullName);

                string outputFolder = textBox2.Text;
                DirectoryInfo oD = new DirectoryInfo(outputFolder);
                if (oD.Exists)
                {
                    string newFolderName = oD.FullName + @"\" + dt.Year;
                    if (!Directory.Exists(newFolderName))
                    {
                        Directory.CreateDirectory(newFolderName);
                    }
                    newFolderName = oD.FullName + @"\" + dt.Year + @"\" + dt.Year + @"_" + dt.Month.ToString("00");
                    if (!Directory.Exists(newFolderName))
                    {
                        Directory.CreateDirectory(newFolderName);
                    }
                    newFolderName = oD.FullName + @"\" + dt.Year + @"\" + dt.Year + @"_" + dt.Month.ToString("00") + @"\" + dt.Year + "_" + dt.Month.ToString("00") + "_" + dt.Day.ToString("00");
                    if (!Directory.Exists(newFolderName))
                    {
                        Directory.CreateDirectory(newFolderName);
                    }
                    string fileName = newFolderName + @"\" + file.Name;
                    try
                    {
                        if (File.Exists(fileName))
                        {
                            string dupFileName = oD.FullName + @"\Duplicates\" + file.Name;
                            if (File.Exists(dupFileName))
                            {
                                dupFileName = oD.FullName + @"\Duplicates\" + file.Name + DateTime.Now.Ticks.ToString();
                            }
                            FileInfo fi = new FileInfo(fileName);
                            fi.MoveTo(dupFileName);
                        }
                        File.Move(file.FullName, fileName);

                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = ex.Message;
                    }
                    lblStatus.Text = "File Moved " + fileName;

                }

            }
            MessageBox.Show("Completed");
        }
        private delegate void UpdateStatusDelegate(string status);
        private void UpdateStatus(string status)
        {
            if (this.lblStatus.InvokeRequired)
            {
                this.Invoke(new UpdateStatusDelegate(this.UpdateStatus), new object[] { status });
                return;
            }

            this.lblStatus.Text = status;
        }
        public static DateTime GetDateTakenFromImage(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = null;
                    try
                    {
                        propItem = myImage.GetPropertyItem(36867);
                    }
                    catch { }
                    if (propItem == null)
                    {
                        propItem = myImage.GetPropertyItem(306);
                    }
                    if (propItem != null)
                    {
                        string dateTaken = Encoding.UTF8.GetString(propItem.Value).Substring(0, 10).Replace(":", "-");
                        DateTime dt1 = DateTime.Parse(dateTaken);
                        return dt1;
                    }
                    else
                        return new FileInfo(path).LastWriteTime;
                }
            }
            catch { }
            finally
            { }
            return DateTime.Now;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TransferMovieFiles("*.mp4");
            TransferMovieFiles("*.m2*");
            TransferMovieFiles("*.mts");
            TransferMovieFiles("*.wm*");
            TransferMovieFiles("*.3gp");
            TransferMovieFiles("*.avi");
            MessageBox.Show("Completed");
        }
        private void TransferMovieFiles(string c)
        {
            lblStatus.Text = "";
            DirectoryInfo di = new DirectoryInfo(textBox1.Text);
            FileInfo[] files = di.GetFiles(c, SearchOption.AllDirectories).Union(di.GetFiles("*.mov*", SearchOption.AllDirectories)).ToArray();
            int count = files.Length;
            int i = 1;
            foreach (var file in files)
            {
                DateTime dt = File.GetCreationTime(file.FullName);
                var fil = ShellFile.FromFilePath(file.FullName);
                //DateTime dt = ((Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperty<System.DateTime?>)(new System.Collections.Generic.Mscorlib_CollectionDebugView<Microsoft.WindowsAPICodePack.Shell.PropertySystem.IShellProperty>(fil.Properties.DefaultPropertyCollection).Items[10])).Value;
                try
                {
                    dt = ((System.DateTime)fil.Properties.DefaultPropertyCollection[10].ValueAsObject).Date;
                }
                catch
                {
                    try
                    {
                        dt = ((System.DateTime)fil.Properties.DefaultPropertyCollection[11].ValueAsObject).Date;
                    }
                    catch
                    {
                        try
                        {
                            dt = ((System.DateTime)fil.Properties.DefaultPropertyCollection[12].ValueAsObject).Date;
                        }
                        catch
                        {
                            try
                            {
                                dt = ((System.DateTime)fil.Properties.DefaultPropertyCollection[13].ValueAsObject).Date;
                            }
                            catch
                            {
                                continue;
                            }
                        }

                    }
                }
                string outputFolder = textBox2.Text;
                DirectoryInfo oD = new DirectoryInfo(outputFolder);
                if (oD.Exists)
                {
                    string newFolderName = oD.FullName + @"\" + dt.Year;
                    if (!Directory.Exists(newFolderName))
                    {
                        Directory.CreateDirectory(newFolderName);
                    }
                    newFolderName = oD.FullName + @"\" + dt.Year + @"\" + dt.Year + @"_" + dt.Month.ToString("00");
                    if (!Directory.Exists(newFolderName))
                    {
                        Directory.CreateDirectory(newFolderName);
                    }
                    newFolderName = oD.FullName + @"\" + dt.Year + @"\" + dt.Year + @"_" + dt.Month.ToString("00") + @"\" + dt.Year + "_" + dt.Month.ToString("00") + "_" + dt.Day.ToString("00");
                    if (!Directory.Exists(newFolderName))
                    {
                        Directory.CreateDirectory(newFolderName);
                    }
                    //----
                    string fileName = newFolderName + @"\" + file.Name;
                    if (File.Exists(fileName))
                    {
                        string dupFileName = oD.FullName + @"\Duplicates\" + file.Name;
                        if (File.Exists(dupFileName))
                        {
                            dupFileName = oD.FullName + @"\Duplicates\" + file.Name + DateTime.Now.Ticks.ToString();
                        }
                        FileInfo fi = new FileInfo(fileName);
                        fi.MoveTo(dupFileName);
                    }
                    File.Move(file.FullName, fileName);
                    lblStatus.Text = i.ToString() + "/" + count + " ,  File Moved " + fileName;
                    i++;
                }

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {

            processDirectory(textBox1.Text, "*.aae*");
            processDirectory(textBox1.Text, "*.Modd*");
        }
        private static void processDirectory(string startLocation, string criteria)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(directory);
                    di.Attributes = FileAttributes.Normal;

                    FileInfo[] files = di.GetFiles(criteria, SearchOption.AllDirectories);
                    foreach (var f in files)
                        File.Delete(f.FullName);

                    files = di.GetFiles("*.moff*", SearchOption.AllDirectories);
                    foreach (var f in files)
                        File.Delete(f.FullName);

                    files = di.GetFiles("*.th*", SearchOption.AllDirectories);
                    foreach (var f in files)
                        File.Delete(f.FullName);

                    files = di.GetFiles("*.Thumbs.db", SearchOption.AllDirectories);
                    foreach (var f in files)
                        File.Delete(f.FullName);

                    processDirectory(directory, criteria);
                    if (Directory.GetFiles(directory).Length == 0 &&
                        Directory.GetDirectories(directory).Length == 0)
                    {

                        try
                        {
                            Directory.Delete(directory, false);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    else if (Directory.GetFiles(directory).Length == 1)
                    {
                        string fileName = Directory.GetFiles(directory)[0];
                        FileInfo fi = new FileInfo(fileName);
                        if (fi.Extension.Contains("db"))
                        {
                            try
                            {
                                File.Delete(fi.FullName);
                                Directory.Delete(directory, false);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                    }
                }
                catch
                { }
            }
            // MessageBox.Show("Completed");
        }

        private void button5_Click(object sender, EventArgs e)
        {

            processDirectory(textBox1.Text, "*deskt*.ini");
            processDirectory(textBox1.Text, "*.pica*");
            processDirectory(textBox1.Text, "*.modd*");
            MessageBox.Show("Completed");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog2.ShowDialog();
            textBox2.Text = folderBrowserDialog2.SelectedPath.ToString();
        }

        private void ARWFiles_Click(object sender, EventArgs e)
        {

            lblStatus.Text = "";
            DirectoryInfo di = new DirectoryInfo(textBox1.Text);
            FileInfo[] files = di.GetFiles("*.arw*", SearchOption.AllDirectories);
            int count = files.Length;
            int i = 1;
            foreach (var file in files)
            {
                DateTime dt = File.GetCreationTime(file.FullName);
                var fil = ShellFile.FromFilePath(file.FullName);
                //DateTime dt = ((Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperty<System.DateTime?>)(new System.Collections.Generic.Mscorlib_CollectionDebugView<Microsoft.WindowsAPICodePack.Shell.PropertySystem.IShellProperty>(fil.Properties.DefaultPropertyCollection).Items[10])).Value;
                dt = ((System.DateTime)fil.Properties.DefaultPropertyCollection[10].ValueAsObject).Date;
                string outputFolder = textBox2.Text;
                DirectoryInfo oD = new DirectoryInfo(outputFolder);
                if (oD.Exists)
                {
                    string newFolderName = oD.FullName + @"\" + dt.Year;
                    if (!Directory.Exists(newFolderName))
                    {
                        Directory.CreateDirectory(newFolderName);
                    }
                    newFolderName = oD.FullName + @"\" + dt.Year + @"\" + dt.Year + @"_" + dt.Month.ToString("00");
                    if (!Directory.Exists(newFolderName))
                    {
                        Directory.CreateDirectory(newFolderName);
                    }
                    newFolderName = oD.FullName + @"\" + dt.Year + @"\" + dt.Year + @"_" + dt.Month.ToString("00") + @"\" + dt.Year + "_" + dt.Month.ToString("00") + "_" + dt.Day.ToString("00") + "_ARW";
                    if (!Directory.Exists(newFolderName))
                    {
                        Directory.CreateDirectory(newFolderName);
                    }
                    //newFolderName = newFolderName + @"\ARW";
                    //if (!Directory.Exists(newFolderName))
                    //{
                    //    Directory.CreateDirectory(newFolderName);
                    //}
                    string fileName = newFolderName + @"\" + file.Name;
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    File.Move(file.FullName, fileName);

                    lblStatus.Text = i.ToString() + "/" + count + " ,  File Moved " + fileName;
                    i++;
                }

            }
            MessageBox.Show("Completed");
        }
    }
}


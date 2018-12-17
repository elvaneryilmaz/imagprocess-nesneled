using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
using System.IO.Ports;

namespace proje1deneme
{
    public partial class Form1 : Form
        
    {
        private FilterInfoCollection VideoCapTureDevices;
        private VideoCaptureDevice Finalvideo;

        public Form1()
        {
            InitializeComponent();
        }

        int R;
        int G;
        int B;

        private void Form1_Load(object sender, EventArgs e)
        {
            VideoCapTureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in VideoCapTureDevices)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
                comboBox2.DataSource = SerialPort.GetPortNames();
            }
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Finalvideo = new VideoCaptureDevice(VideoCapTureDevices[comboBox1.SelectedIndex].MonikerString);
            Finalvideo.NewFrame += new NewFrameEventHandler(Finalvideo_NewFrame);
            Finalvideo.DesiredFrameRate = 20;
            Finalvideo.DesiredFrameSize = new Size(450, 450);
            Finalvideo.Start();
        }

        private void Finalvideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = image;

            if (radioButton4.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(R, G, B));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }
            if (radioButton1.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(191, 0, 0));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }

            if (radioButton3.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(30, 144, 255));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }
            if (radioButton2.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(0, 215, 0));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }
        }

        public void nesnebul(Bitmap image)
        {
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.MinWidth = 5;
            blobCounter.MinHeight = 5;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            BitmapData objectsData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            Grayscale grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            UnmanagedImage grayImage = grayscaleFilter.Apply(new UnmanagedImage(objectsData));
            image.UnlockBits(objectsData);

            blobCounter.ProcessImage(image);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
            Blob[] blobs = blobCounter.GetObjectsInformation();
            pictureBox2.Image = image;

            foreach (Rectangle recs in rects)
            {
                if (rects.Length > 0)
                {
                    Rectangle objectRect = rects[0];
                    //Graphics g = Graphics.FromImage(image);
                    Graphics g = pictureBox1.CreateGraphics();
                    using (Pen pen = new Pen(Color.FromArgb(255, 255, 0), 2))
                    {
                        g.DrawRectangle(pen, objectRect);
                    }
                    int objectX = objectRect.X + (objectRect.Width / 2);
                    int objectY = objectRect.Y + (objectRect.Height / 2);

                    string çerçeve = "";
                    //if(object.X > 0) && (object.Y > 0) && (object.X < 150) && (object.Y < 150))
                    if (objectY < 150)
                    { if (objectX < 150)
                        {
                            çerçeve = "üst-sol";
                            serialPort1.Write("a=1");
                        }
                        else if (objectX < 150 && objectX < 300)
                        {
                            çerçeve = "üst-orta";
                            serialPort1.Write("a=2");
                        }
                        else
                        {
                            çerçeve = "üst-sağ";
                            serialPort1.Write("a=3");
                        }
                    }


                    if (objectY > 150 && objectY < 300)
                    { if (objectX < 150)
                        {
                            çerçeve = "orta-sol";
                            serialPort1.Write("a=4");
                        }
                        else if (objectX > 150 && objectX < 300)
                        {
                            çerçeve = "orta-orta";
                            serialPort1.Write("a=5");
                        }
                        else
                        {
                            çerçeve = "orta-sağ";
                            serialPort1.Write("a=6");
                        }
                    }

                    if (objectY < 450)
                    {
                       if (objectX < 150)
                        {
                            çerçeve = "alt-sağ";
                            serialPort1.Write("a = 7");
                        }
                        else if (objectX > 150 && objectX < 300) 
                        {
                            çerçeve = "alt-orta";
                            serialPort1.Write("a=8");
                        }
                       else
                        {
                            çerçeve = "alt-sol";
                            serialPort1.Write("a=9");
                        }
                    }
       
                    g.DrawString(objectX.ToString() + "X" + objectY.ToString() + çerçeve, new Font("Arial", 12), Brushes.Yellow, new System.Drawing.Point(200,1));
                    g.Dispose();

                    if (checkBox1.Checked)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            richTextBox1.Text = objectRect.Location.ToString() + "\n" + richTextBox1.Text + "\n"; ;
                        });
                    }
                }
            }   
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (Finalvideo.IsRunning)
            {
                Finalvideo.Stop();
            }
            Application.Exit();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = "R değeri: " + trackBar1.Value.ToString();
            R = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label2.Text = "G değeri: " + trackBar2.Value.ToString();
            G = trackBar2.Value;
        }

        private void TrackBar3_Scroll(object sender, EventArgs e)
        {
            label3.Text = "B değeri: " + trackBar3.Value.ToString();
            B = trackBar3.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.BaudRate = 9600;
            serialPort1.PortName = "com8";
            serialPort1.Open();
            if (serialPort1.IsOpen) MessageBox.Show("Port Açıldı");
        }
    }
}
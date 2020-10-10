using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OSM
{
    public partial class Form1 : Form
    {

        private Graphics graphics;
        private Bitmap bitmap;

        private Space space;

        public Form1()
        {
            StartPosition = FormStartPosition.CenterScreen;
            // generate a new Bitmap which 
            bitmap = new Bitmap(1946, 1106);
            graphics = Graphics.FromImage(bitmap);

            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            // set double buffer
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            // ped and ped's move should run in timer_tick
            // before space init
            GenerateDirectory();

            space = new Space().Init(Util.kReport);


            InitializeComponent();

            timer1.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Util.SetChartLength(chart1);
        }


        // generate report directory
        private void GenerateDirectory()
        {
            var path = "./report/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void RemoveAll()
        {
            graphics.Clear(BackColor);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // bitmap draw
            RemoveAll();

            chart1.Series[0].Points.Clear();

            foreach (var floor in space.Floors)
            {

                if (Util.kReport && floor.Cnt != -1)
                    space.AddToReporter(floor.Number + " " + floor.Peds.Count);

                chart1.Series[0].Points.AddXY(floor.Number, floor.Peds.Count); 


                if (floor.Cnt == 0 && floor.Peds.Count > 0)
                {
                    floor.Cnt = 1;
                }

                if (floor.Peds.Count == 0 && floor.Cnt == 1)
                {
                    floor.Cnt -= 2;
                }

                Util.DrawFloor(graphics, floor);
                foreach (var ped in floor.Peds)
                {
                    ped.Move();
                    if (Util.kReport && !ped.Can)
                        space.AddToReporter(ped.ReportLine);
                    Util.DrawPed(graphics, ped);
                }
                // every ped run and do remove job
                foreach (var ped in floor.ToDel)
                {
                    floor.Peds.Remove(ped);

                    if (floor.Number > 0)
                    {
                        space.Floors[floor.Number - 1].Add(ped);
                    }
                }
                floor.ClearRemove();
            }

            if (Util.kId < (Util.kFloorNum - 1) * (Util.kFloorPedNum - Util.kInitPedNum))
            {
                foreach (var floor in space.Floors)
                {
                    if (floor.Number != 0)
                    {
                        if (Util.AddRandomPed(floor, space))
                        {
                            Util.kPedSum++;
                            Util.kId++;
                        }
                    }
                }
            }
            
            if (Util.kReport && Util.kPedSum == 0)
            {
                space.StopReport();
            }

            CreateGraphics().DrawImage(bitmap, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            if (button1.Text.Equals("Start"))
            {
                button1.Text = "Pause";
            }
            else
            {
                button1.Text = "Start";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // change and init for this
            Util.kFloorNum = Util.StringToInt(textBox2.Text, Util.kMaxFloor, Util.kMinFloor);
            Util.kFloorPedNum = Util.StringToInt(textBox4.Text, Util.kMaxFloorPedNum, Util.kInitPedNum);
            // if checked, report else not
            Util.kReport = checkBox1.Checked;
            space = space.Init(Util.kReport);
            // update width of chart
            Util.SetChartLength(chart1);
        }

        // form close -> if space's reporter are running -> stop
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Util.kReport)
                space.StopReport();
            
        }

    }
}

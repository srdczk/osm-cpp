using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OSM
{
    public sealed partial class Form1 : Form
    {
        private float x = 10.0f, y = 10.0f;
        private Graphics _graphics;
        private Bitmap _bitmap;
        private Space _space;
        private int cnt = 0;
        private static int pedNum = 0;
        public Form1()
        {
            // this.MaximizeBox = false;
            // this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            _bitmap = new Bitmap(1946, 1106);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.SmoothingMode = SmoothingMode.HighQuality;
            _graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            Config.maxFloor = 40;
            _space = new Space().Init();
            // 设置双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
        }

        // 清除所有元素
        private void RemoveAll()
        {
            _graphics.Clear(this.BackColor);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            if (button1.Text.Equals("Start")) button1.Text = "Pause";
            else button1.Text = "Start";
        }


        private void PositionUpdate(bool drawPed)
        {
            foreach (var obj in _space.GetMap().Values)
            {
                var floor = (Floor)obj;
                // draw floor's walls
                CustomizeUtil.DrawFloor(_graphics, floor);
                foreach (var ped in floor.GetPeds())
                {
                    
                    // update ped's position
                    ped.UpdateDir();
                    ped.Move();
                    // if need draw every ped
                    if (drawPed)
                    {
                        CustomizeUtil.DrawPed(_graphics, ped);
                    }
                }

                floor.DoUpdate();

                // let the ped's to next floor or out of space
                foreach (var ped in floor.wantRemove)
                {
                    // want to remove ped 
                    floor.RemovePed(ped);
                    if (floor.GetFloor() > 0)
                    {
                        ((Floor)_space.GetMap()[floor.GetFloor() - 1]).AddPed(ped);
                        // add to from up set
                        ((Floor)_space.GetMap()[floor.GetFloor() - 1]).FromUpAdd(ped);
                    }
                }
                floor.wantRemove.Clear();
            }
        }

        private void DrawDefault()
        {
            RemoveAll();

            PositionUpdate(true);

            if (pedNum ++ < 85) _space.RandomPed();

            this.CreateGraphics().DrawImage(_bitmap, 0, 0);

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawDefault();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                pedNum = 0;
                int floor = Int32.Parse(textBox1.Text);
                if (floor > 50) floor = 50;
                else if (floor < 10) floor = 10;
                Config.maxFloor = floor;
                cnt = 0;
                Config.getTarget = 0;
                Config.delCnt = 0;
                _space = new Space().Init();
            }
            catch (Exception exception)
            {
                // 如果有异常

            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}

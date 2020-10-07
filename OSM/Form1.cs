﻿using System;
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

            space = new Space().Init();

            InitializeComponent();
            

            timer1.Enabled = false;
        }

        private void RemoveAll()
        {
            graphics.Clear(BackColor);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // bitmap draw
            RemoveAll();

            foreach (var floor in space.Floors)
            {
                Util.DrawFloor(graphics, floor);
                foreach (var ped in floor.Peds)
                {
                    ped.Move();
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

            if (Util.kId < (Util.kFloorNum - 1) * Util.kFloorPedNum)
            {
                foreach (var floor in space.Floors)
                {
                    if (floor.Number != 0)
                    {
                        if (Util.AddRandomPed(floor, space)) Util.kId++;
                    }
                }
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
            Util.kFloorNum = Util.StringToInt(textBox2.Text);
            space = space.Init();
        }
    }
}
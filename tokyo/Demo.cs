﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Threading;


namespace tokyo
{
    class Demo
    {

        private const int Width = 1024;

        private const int Height = 768;

        private Form form;

        private GraphicBuffer buffer;

        private Font defaultFont;

        private Camera camera;

        Mesh[] Suzanne = new BabylonMeshLoader().Load("Suzanne");

        public static void Main(string[] args)
        {
            new Demo().Run();
        }

        public Demo()
        {
            form = new Form
            {
                Size = new Size(Width, Height),
                StartPosition = FormStartPosition.CenterScreen
            };
            camera = new Camera { Position = new Vector(0, 0, 10), Target = Vector.Zero, Fov = (float)Math.PI / 4, ZNear = 0.1f, ZFar = 1f };
            buffer = new GraphicBuffer(Width, Height);
            defaultFont = new Font(new FontFamily("Microsoft Yahei"), 14);
        }

        public void Run()
        {
            form.Show();
            var stopwatch = new Stopwatch();
            var deltatime = TimeSpan.FromMilliseconds(1000.0 / 60);

            while (!form.IsDisposed)
            {
                stopwatch.Start();
                // 渲染到backbuffer
                Render(deltatime);
                // Render(new Mesh[] { Mesh.Cube() });
                // backbuffer、frontbuffer轮换
                buffer.SwapBuffers();
                // 显示frontbuffer
                Present();
                Application.DoEvents();
                stopwatch.Stop();
                deltatime = stopwatch.Elapsed;

                stopwatch.Reset();
                // break;
            }
        }

        private void Present()
        {
            using (var g = form.CreateGraphics())
            {
                g.DrawImage(buffer.Current, Point.Empty);
            }
        }

        private void Render(TimeSpan dt)
        {
            var g = buffer.BackgroundGraphicDevice;
            buffer.BackgroundGraphicDevice.Clear(Color.Black);
            g.DrawString($"FPS: {1000.0 / dt.Milliseconds}", defaultFont, Brushes.White, 0, 0);
            Suzanne[0].Rotation += new Vector(0, 0.01f, 0);
            g.DrawMeshes(Suzanne, Color.Blue, camera);
        }
    }
}

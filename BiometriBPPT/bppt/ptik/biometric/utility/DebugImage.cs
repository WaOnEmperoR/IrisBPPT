using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using Cairo;
using BiometriBPPT.bppt.ptik.biometric.entity;
using System.IO;

namespace BiometriBPPT.bppt.ptik.biometric.utility
{
    public class DebugImage
    {
        private PGM pgm;
        private bool enabled = false;
        private string logPath = "logBppt";

        public DebugImage(bool status)
        {
            Enabled = status;
            if (!Enabled) return;
            bool IsExists = System.IO.Directory.Exists(logPath);

            if (!IsExists)
                System.IO.Directory.CreateDirectory(logPath);
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public string LogPath
        {
            get { return logPath; }
            set
            {
                if (!Enabled) return;
                logPath = value;
                bool IsExists = System.IO.Directory.Exists(logPath);

                if (!IsExists)
                    System.IO.Directory.CreateDirectory(logPath);
            }
        }

        public void DebugImg_WriteImg(string fileName, MyImage img)
        {
            if (!Enabled) return;
            //assert(fileName && img);
            pgm = new PGM(img);

            pgm.Save(logPath + "\\" + fileName);
        }

        public void DebugImg_WriteIrisTxt(string fileName, int[] HIP, int index, int length)
        {
            if (!Enabled) return;

            FileStream fs = new FileStream(logPath + "\\" + fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = index; i < length; i++)
            {
                sw.WriteLine(i + " " + HIP[i]);
            }
            sw.Dispose();
            sw.Close();
            fs.Close();
        }

        public void DebugImg_ImgToTxt(string fileName, MyImage img)
        {
            if (!Enabled) return;

            FileStream fs = new FileStream(logPath + "\\" + fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int j = 0; j < img.Height; j++)
            {
                for (int i = 0; i < img.Width; i++)
                {
                    sw.WriteLine(((j * img.Width) + i) + "," + img.UPixel[j, i]);
                }
                sw.WriteLine();
            }
            sw.Dispose();
            sw.Close();
            fs.Close();
        }

        public void DebugImg_int2dToTxt(string fileName, int[,] array)
        {
            if (!Enabled) return;

            FileStream fs = new FileStream(logPath + "\\" + fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int j = 0; j < array.GetLength(0); j++)
            {
                for (int i = 0; i < array.GetLength(1); i++)
                {
                    sw.WriteLine(array[j, i]);
                }
                sw.WriteLine();
            }
            sw.Dispose();
            sw.Close();
            fs.Close();
        }

        public void DebugImg_byte2dToTxt(string fileName, byte[,] array)
        {
            if (!Enabled) return;

            FileStream fs = new FileStream(logPath + "\\" + fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int j = 0; j < array.GetLength(0); j++)
            {
                for (int i = 0; i < array.GetLength(1); i++)
                {
                    sw.WriteLine(((j * array.GetLength(1))+i) + "," + array[j, i]);
                }
                sw.WriteLine();
            }
            sw.Dispose();
            sw.Close();
            fs.Close();
        }

        public void DebugImg_byte1dToTxt(string fileName, byte[] array)
        {
            if (!Enabled) return;

            FileStream fs = new FileStream(logPath + "\\" + fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int j = 0; j < array.GetLength(0); j++)
            {
                sw.WriteLine(j + "," + array[j]);
            }
            sw.Dispose();
            sw.Close();
            fs.Close();
        }

        public void DebugImg_float2dToTxt(string fileName, float[,] array)
        {
            if (!Enabled) return;

            FileStream fs = new FileStream(logPath + "\\" + fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int j = 0; j < array.GetLength(0); j++)
            {
                for (int i = 0; i < array.GetLength(1); i++)
                {
                    sw.WriteLine(array[j, i].ToString("0.000000"));
                    //sw.WriteLine(array[j, i]);
                }
                sw.WriteLine();
            }
            sw.Dispose();
            sw.Close();
            fs.Close();
        }

        public void DebugImg_WriteGrid(string fileName, MyImage gridMat)
        {
            if (!Enabled) return;
            //assert(gridMat->type == UCHAR || gridMat->type == FLOAT);

            int i, j;
            MyImage vis = new MyImage(gridMat.Height * Const.BLOCK_HEIGHT, gridMat.Width * Const.BLOCK_WIDTH, gridMat.TypeData);


            for (j = 0; j < vis.Height; j++)
            {
                for (i = 0; i < vis.Width; i++)
                {
                    if (gridMat.TypeData == MyImgType.UCHAR)
                        vis.UPixel[j, i] = gridMat.UPixel[j / Const.BLOCK_HEIGHT, i / Const.BLOCK_WIDTH];
                    else
                        vis.FPixel[j, i] = gridMat.FPixel[j / Const.BLOCK_HEIGHT, i / Const.BLOCK_WIDTH];
                }
            }

            pgm = new PGM(vis);

            pgm.Save(logPath + "\\" + fileName);

        }

        public void DebugImg_WriteOF(string fileName, MyImage OF)
        {
            if (!Enabled) return;
            //assert(OF != NULL && OF->type == FLOAT);

            ImageSurface surface;
            Context cr;
            int imgWidth, imgHeight, i, j;

            imgWidth = OF.Width * Const.BLOCK_WIDTH;
            imgHeight = OF.Height * Const.BLOCK_HEIGHT;

            surface = new ImageSurface(Format.Argb32, imgWidth, imgHeight);
            cr = new Context(surface);

            /* Fill with white background. */
            cr.Rectangle(0, 0, imgWidth, imgHeight); //cairo_rectangle(cr, 0, 0, imgWidth, imgHeight);
            cr.SetSourceRGB(1, 1, 1); //cairo_set_source_rgb(cr, 1, 1, 1);
            cr.Fill(); //cairo_fill(cr);

            /* Create a set of oriented lines. */
            for (j = 0; j < OF.Height; j++)
            {
                for (i = 0; i < OF.Width; i++)
                {

                    int offsetX = i * Const.BLOCK_WIDTH;
                    int offsetY = j * Const.BLOCK_HEIGHT;
                    int coreX = offsetX + Const.BLOCK_WIDTH / 2;
                    int coreY = offsetY + Const.BLOCK_HEIGHT / 2;

                    cr.Save(); //cairo_save(cr);
                    cr.Translate(coreX, coreY); //cairo_translate(cr, coreX, coreY);
                    cr.Rotate(OF.FPixel[j, i]);//cairo_rotate(cr, OF->data.f[j][i]);

                    cr.Rectangle(-Const.BLOCK_WIDTH / 2.0, -Const.BLOCK_HEIGHT / 2.0, Const.BLOCK_WIDTH, Const.BLOCK_HEIGHT); //cairo_rectangle(cr, -BLOCK_WIDTH / 2.0, -BLOCK_HEIGHT / 2.0, BLOCK_WIDTH, BLOCK_HEIGHT);
                    cr.Clip();//cairo_clip(cr);

                    cr.MoveTo(-2 * Const.BLOCK_WIDTH, 0);//cairo_move_to(cr, -2 * BLOCK_WIDTH, 0);
                    cr.LineTo(2 * Const.BLOCK_WIDTH, 0);//cairo_line_to(cr, 2 * BLOCK_WIDTH, 0);

                    cr.SetSourceRGBA(.2, .2, .2, 1); //cairo_set_source_rgba(cr, .2, .2, .2, 1);
                    cr.Stroke();//cairo_stroke(cr);
                    cr.Restore();//cairo_restore(cr);
                }
            }

            /* Create block separator lines. */
            for (j = 0; j < imgHeight; j += Const.BLOCK_HEIGHT)
            {
                cr.MoveTo(0, j);//cairo_move_to(cr, 0, j);
                cr.LineTo(imgWidth, j);//cairo_line_to(cr, imgWidth, j);
            }
            for (i = 0; i < imgWidth; i += Const.BLOCK_WIDTH)
            {
                cr.MoveTo(i, 0);//cairo_move_to(cr, i, 0);
                cr.LineTo(i, imgHeight);//cairo_line_to(cr, i, imgHeight);
            }

            cr.SetSourceRGBA(0, 0, 0, .2); //cairo_set_source_rgba(cr, 0, 0, 0, .2);
            cr.Stroke();//cairo_stroke(cr);

            surface.Flush();
            surface.WriteToPng(logPath + "\\" + fileName); //cairo_surface_write_to_png(surface, fileName);

            //cairo_destroy(cr);
            surface.Dispose(); surface.Destroy();//cairo_surface_destroy(surface);
        }
    }
}

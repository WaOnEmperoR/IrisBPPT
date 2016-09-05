using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace BPPT_Iris_SDK
{

    /// <summary>
    /// PGM Image Class.
    /// </summary>
    public class PGM_Iris
    {
        #region Constants

        public const string MagicString = "P5";
        public const int MaxGray = 255;

        public const int Background = 0;
        public const int Foreground = 255;

        #endregion

        #region Fields

        private int maxGray;
        private Size _size;
        private string _extension;

        #endregion

        #region Properties

        public string FilePath { get; set; }
        public string Comment { get; set; }
        public int[,] Pixels { get; set; }
        public byte[,] PixelsU { get; set; }
        public int MaxValue
        {
            get
            {
                return maxGray;
            }
            set
            {
                maxGray = value;
            }
        }

        public Size Size
        {
            get
            {
                return (_size);
            }
            set
            {
                _size = value;
                Pixels = new int[_size.Height, _size.Width];
                PixelsU = new byte[_size.Height, _size.Width];
            }
        }

        public string Extension
        {
            get
            {
                return (_extension);
            }
            set
            {
                _extension = value;
            }
        }

        #endregion

        #region Constructors

        public PGM_Iris()
        {
            this.FilePath = string.Empty;
            this.Comment = string.Empty;
            this.Size = new Size(0, 0);
        }

        public PGM_Iris(int width, int height)
        {
            this.FilePath = string.Empty;
            this.Comment = string.Empty;
            this.Size = new Size(width, height);
        }

        public PGM_Iris(string path)
        {
            this.FilePath = path;
            this.Read();
        }

        public PGM_Iris(Bitmap bmp)
        {
            ReadBitmap(bmp);
        }

        #endregion

        #region Methods

        public void Read()
        {
            try
            {
                FileStream stream = new FileStream(this.FilePath, FileMode.Open);
                Extension = Path.GetExtension(stream.Name);

                #region Read Image Header

                string magicString = StreamUtils.ReadLine(stream);

                //read comment-lines
                this.Comment = string.Empty;
                string str = string.Empty;
                while (true)
                {
                    str = StreamUtils.ReadLine(stream);
                    if (str.Length > 0)
                    {
                        if (str.StartsWith("#"))
                        {
                            this.Comment += str;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                //parse dimension
                string[] dimensionArray = str.Split(' ');
                int width = Int32.Parse(dimensionArray[0]);
                int height = Int32.Parse(dimensionArray[1]);
                this.Size = new Size(width, height);

                maxGray = short.Parse(StreamUtils.ReadLine(stream));
                MaxValue = maxGray;

                #endregion

                #region Read Image Pixels

                for (int y = 0; y < this.Size.Height; y++)
                {
                    for (int x = 0; x < this.Size.Width; x++)
                    {
                        int gray = stream.ReadByte();
                        this.Pixels[y, x] = (short)gray;
                        PixelsU[y, x] = (byte)gray;
                    }
                }

                #endregion

                stream.Close();
            }
            catch (Exception exception)
            {
                IOException imageReadException = new IOException("Error in reading image [" + this.FilePath + "].", exception);
                throw imageReadException;
            }
        }

        public void Write()
        {
            this.WriteToPath(this.FilePath);
        }

        public void WriteToPath(string FilePath)
        {
            FileStream stream = new FileStream(FilePath, FileMode.Create);

            //write imageheader
            StreamUtils.WriteLine(MagicString, stream);
            if (this.Comment.Length > 0)
            {
                StreamUtils.WriteLine("#" + this.Comment, stream);
            }
            StreamUtils.WriteLine(this.Size.Width + " " + this.Size.Height, stream);
            StreamUtils.WriteLine(MaxGray.ToString(), stream);

            //write imagedata
            for (int y = 0; y < this.Size.Height; y++)
            {
                for (int x = 0; x < this.Size.Width; x++)
                {
                    stream.WriteByte((byte)this.Pixels[y, x]);
                }
            }

            stream.Close();
        }

        public void WriteToPath(string FilePath, int[,] pixels)
        {
            FileStream stream = new FileStream(FilePath, FileMode.Create);

            //write imageheader
            StreamUtils.WriteLine(MagicString, stream);
            if (this.Comment.Length > 0)
            {
                StreamUtils.WriteLine("#" + this.Comment, stream);
            }
            StreamUtils.WriteLine(this.Size.Width + " " + this.Size.Height, stream);
            StreamUtils.WriteLine(MaxGray.ToString(), stream);
            
            //write imagedata
            for (int y = 0; y < this.Size.Height; y++)
            {
                for (int x = 0; x < this.Size.Width; x++)
                {
                    stream.WriteByte((byte)pixels[y, x]);
                }
            }

            stream.Close();
        }

        public byte[] GetBytes()
        {
            //FileStream stream = new FileStream(FilePath, FileMode.Create);
            MemoryStream stream = new MemoryStream();

            //write imageheader
            StreamUtils.WriteLine(MagicString, stream);
            if (this.Comment.Length > 0)
            {
                StreamUtils.WriteLine("#" + this.Comment, stream);
            }
            StreamUtils.WriteLine(this.Size.Width + " " + this.Size.Height, stream);
            StreamUtils.WriteLine(MaxGray.ToString(), stream);

            //write imagedata
            for (int y = 0; y < this.Size.Height; y++)
            {
                for (int x = 0; x < this.Size.Width; x++)
                {
                    stream.WriteByte((byte)this.Pixels[x, y]);
                }
            }
            byte[] b = stream.ToArray();
            stream.Close();
            return b;

        }
        public byte[] GetPixelData()
        {
            //FileStream stream = new FileStream(FilePath, FileMode.Create);
            MemoryStream stream = new MemoryStream();

            //write imageheader
            //StreamUtils.WriteLine(MagicString, stream);
            //if (this.Comment.Length > 0)
            //{
            //    StreamUtils.WriteLine("#" + this.Comment, stream);
            //}
            //StreamUtils.WriteLine(this.Size.Width + " " + this.Size.Height, stream);
            //StreamUtils.WriteLine(MaxGray.ToString(), stream);

            //write imagedata
            for (int y = 0; y < this.Size.Height; y++)
            {
                for (int x = 0; x < this.Size.Width; x++)
                {
                    stream.WriteByte((byte)this.Pixels[y, x]);
                }
            }
            byte[] b = stream.ToArray();
            stream.Close();
            return b;

        }


        public PGM_Iris Clone()
        {
            PGM_Iris newImage = new PGM_Iris(this.Size.Width, this.Size.Height);

            //copy image-data
            for (int y = 0; y < this.Size.Height; y++)
            {
                for (int x = 0; x < this.Size.Width; x++)
                {
                    newImage.Pixels[x, y] = this.Pixels[x, y];
                }
            }

            return (newImage);
        }

        public int GetNeighbor(int x, int y, Direction direction)
        {
            int neighborX = -1, neighborY = -1;

            switch (direction)
            {
                case Direction.NorthWest:
                    neighborX = x - 1;
                    neighborY = y - 1;
                    break;
                case Direction.North:
                    neighborX = x;
                    neighborY = y - 1;
                    break;
                case Direction.NorthEast:
                    neighborX = x + 1;
                    neighborY = y - 1;
                    break;
                case Direction.East:
                    neighborX = x + 1;
                    neighborY = y;
                    break;
                case Direction.SouthEast:
                    neighborX = x + 1;
                    neighborY = y + 1;
                    break;
                case Direction.South:
                    neighborX = x;
                    neighborY = y + 1;
                    break;
                case Direction.SouthWest:
                    neighborX = x - 1;
                    neighborY = y + 1;
                    break;
                case Direction.West:
                    neighborX = x - 1;
                    neighborY = y;
                    break;
            }

            if (neighborX >= 0 && neighborX <= this.Size.Width - 1)
            {
                if (neighborY >= 0 && neighborY <= this.Size.Height - 1)
                {
                    return (this.Pixels[neighborX, neighborY]);
                }
            }

            return (-1);
        }

        public List<int> GetAllNeighbors(int x, int y)
        {
            //find all neighbors
            List<int> allNeighbors = new List<int>();
            allNeighbors.Add(this.Pixels[x, y]);
            allNeighbors.Add(GetNeighbor(x, y, Direction.NorthWest));
            allNeighbors.Add(GetNeighbor(x, y, Direction.North));
            allNeighbors.Add(GetNeighbor(x, y, Direction.NorthEast));
            allNeighbors.Add(GetNeighbor(x, y, Direction.East));
            allNeighbors.Add(GetNeighbor(x, y, Direction.SouthEast));
            allNeighbors.Add(GetNeighbor(x, y, Direction.South));
            allNeighbors.Add(GetNeighbor(x, y, Direction.SouthWest));
            allNeighbors.Add(GetNeighbor(x, y, Direction.West));

            //return valid-neighbors
            List<int> validNeighbors = new List<int>();
            validNeighbors.AddRange(from neighborPixel in allNeighbors
                                    where neighborPixel != -1
                                    select neighborPixel);
            return (validNeighbors);
        }

        #endregion

        #region Static-Methods

        public static PGM_Iris ReadFromBitmap(Bitmap bmp)
        {
            PGM_Iris pgm = new PGM_Iris(bmp.Width, bmp.Height);

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    int rgb = bmp.GetPixel(x, y).ToArgb();
                    int red = rgb & 255; //get 4th byte
                    int green = (rgb & 65280) / 256; //get 3rd byte
                    int blue = (rgb & 16711680) / 65536; //get 2nd byte, 1st byte is alpha
                    int gray = (red + green + blue) / 3; //IBA - inter-band-average
                    pgm.Pixels[y, x] = (short)gray;
                }
            }

            return (pgm);
        }

        private void ReadBitmap(Bitmap bmp)
        {
            this.Size = new Size(bmp.Width, bmp.Height);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    int rgb = bmp.GetPixel(x, y).ToArgb();
                    int red = rgb & 255; //get 4th byte
                    int green = (rgb & 65280) / 256; //get 3rd byte
                    int blue = (rgb & 16711680) / 65536; //get 2nd byte, 1st byte is alpha
                    int gray = (red + green + blue) / 3; //IBA - inter-band-average
                    Pixels[y, x] = (short)gray;
                    PixelsU[y, x] = (byte)gray;
                }
            }
        }

        public static void DrawToGraphics(PGM_Iris pgm, Graphics g)
        {
            for (int x = 0; x < pgm.Size.Width; x++)
            {
                for (int y = 0; y < pgm.Size.Height; y++)
                {
                    int gray = pgm.Pixels[y, x];
                    Pen pen = new Pen(Color.FromArgb(gray, gray, gray), 1);
                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }
        }

        public static Bitmap CreateBitmap(PGM_Iris pgm)
        {
            Bitmap bmp = new Bitmap(pgm.Size.Width, pgm.Size.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);
            DrawToGraphics(pgm, g);
            return (bmp);
        }

        #endregion
    }

    public class StreamUtils
    {
        public const byte LF = 10;
        public const byte CR = 13;
        public const string CRLF = "\r\n";

        public static string ReadLine(FileStream stream)
        {
            string ans = string.Empty;
            int tbyte = stream.ReadByte();

            while (tbyte != LF)
            {
                ans += (char)tbyte;
                tbyte = stream.ReadByte();
            }
            return (ans);
        }

        public static void WriteLine(string line, Stream stream)
        {
            line += CRLF;

            byte[] lineBytes = Encoding.ASCII.GetBytes(line);
            stream.Write(lineBytes, 0, lineBytes.Length);
        }


    }

    /// <summary>
    /// Indicates eight directions of a pixel.
    /// </summary>
    public enum Direction
    {
        NorthWest = 1,
        North = 2,
        NorthEast = 3,
        East = 4,
        SouthEast = 5,
        South = 6,
        SouthWest = 7,
        West = 8
    }

}

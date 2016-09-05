using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPPT_Iris_SDK
{
    public class ImageRepresentation_02
    {
        private int[,] g_pixel;
        private int g_perimeter, g_height, g_width;
        private Border[] g_c = new Border[10000];
        private Border[] g_b = new Border[10000];
        private Border[] g_cc = new Border[10000];
        private Border[] g_bb = new Border[10000];

        public int Perimeter
        {
            get
            {
                return g_perimeter;
            }
            set
            {
                g_perimeter = value;
            }
        }

        public ImageRepresentation_02(int[,] _pixel)
        {
            g_pixel = _pixel;
            g_height = g_pixel.GetLength(0);
            g_width = g_pixel.GetLength(1);
            for (int i = 0; i < 10000; i++)
            {
                this.g_c[i] = new Border();
                this.g_b[i] = new Border();
                this.g_cc[i] = new Border();
                this.g_bb[i] = new Border();
            }
        }

        Border find_next_neighbour(Border _bobject, Border _bground)
        {
            Border next = new Border();
            if ((_bobject.Y > _bground.Y) && (_bobject.X > _bground.X))
            {
                next.setY(_bground.Y);
                next.setX(_bground.X + 1);
            }
            if ((_bobject.Y > _bground.Y) && (_bobject.X == _bground.X))
            {
                next.setY(_bground.Y);
                next.setX(_bground.X + 1);
            }
            if ((_bobject.Y > _bground.Y) && (_bobject.X < _bground.X))
            {
                next.setY(_bground.Y + 1);
                next.setX(_bground.X);
            }
            if ((_bobject.Y == _bground.Y) && (_bobject.X < _bground.X))
            {
                next.setY(_bground.Y + 1);
                next.setX(_bground.X);
            }
            if ((_bobject.Y < _bground.Y) && (_bobject.X < _bground.X))
            {
                next.setY(_bground.Y);
                next.setX(_bobject.X);
            }
            if ((_bobject.Y < _bground.Y) && (_bobject.X == _bground.X))
            {
                next.setY(_bground.Y);
                next.setX(_bground.X - 1);
            }
            if ((_bobject.Y < _bground.Y) && (_bobject.X > _bground.X))
            {
                next.setY(_bground.Y - 1);
                next.setX(_bground.X);
            }
            if ((_bobject.Y == _bground.Y) && (_bobject.X > _bground.X))
            {
                next.setY(_bground.Y - 1);
                next.setX(_bground.X);
            }
            return next;
        }

        private int[,] borderFollowing()
        {
            int[,] temp = new int[g_height, g_width];
            int[,] temp_02 = new int[g_height, g_width];
            for (int j = 0; j < g_height; j++)
            {
                for (int i = 0; i < g_width; i++)
                {
                    temp[j, i] = (g_pixel[j, i] / 255);
                    temp_02[j, i] = 0;
                }
            }
            int cnt = 0;
            // int j = 0;
            for (int flag = 0, j = 0; j < temp.GetLength(0); j++)
            {
                for (int i = 0; i < temp.GetLength(1); i++)
                {
                    if (temp[j, i] == 1)
                    {
                        flag = 1;
                        g_b[cnt].setY(j);
                        g_b[cnt].setX(i);
                        g_c[cnt].setY(j);
                        g_c[cnt].setX(i - 1);

                        cnt++;
                    }
                    else
                    {
                        if (flag == 1)
                        {
                            break;
                        }
                    }
                }
            }
            do
            {
                int k = 0;
                g_bb[k].setY(g_b[(cnt - 1)].Y);
                g_bb[k].setX(g_b[(cnt - 1)].X);
                g_cc[k].setY(g_c[(cnt - 1)].Y);
                g_cc[k].setX(g_c[(cnt - 1)].X);
                Border next;
                do
                {
                    next = find_next_neighbour(g_bb[k], g_cc[k]);
                    g_cc[(k + 1)].setY(next.Y);
                    g_cc[(k + 1)].setX(next.X);
                    g_bb[(k + 1)].setY(g_bb[k].Y);
                    g_bb[(k + 1)].setX(g_bb[k].X);
                    k++;
                } while (temp[next.Y, next.X] != 1);
                g_b[cnt].setY(next.Y);
                g_b[cnt].setX(next.X);
                g_c[cnt].setY(g_cc[(k - 1)].Y);
                g_c[cnt].setX(g_cc[(k - 1)].X);


                cnt++;
            } while ((Math.Abs(g_b[(cnt - 1)].Y - g_b[0].Y) != 0) || (Math.Abs(g_b[(cnt - 1)].X - g_b[0].X) != 0));
            g_perimeter = (cnt - 1);
            for (int j = 0; j < g_height; j++)
            {
                for (int i = 0; i < g_width; i++)
                {
                    temp[j, i] *= 255;
                }
            }

            for (int i = 0; i < g_perimeter; i++)
            {
                temp_02[g_b[i].X, g_b[i].Y] = 255;
            }

            return temp_02;
        }

        private int[,] borderFollowing_02()
        {
            int[,] temp = new int[g_height, g_width];
            int[,] temp_02 = new int[g_height, g_width];
            for (int j = 0; j < g_height; j++)
            {
                for (int i = 0; i < g_width; i++)
                {
                    temp[j, i] = (g_pixel[j, i] / 255);
                    temp_02[j, i] = 0;
                }
            }
            int cnt = 0;
            // int j = 0;
            for (int flag = 0, j = 0; j < temp.GetLength(0); j++)
            {
                for (int i = 0; i < temp.GetLength(1); i++)
                {
                    if (temp[j, i] == 1)
                    {
                        flag = 1;
                        g_b[cnt].setY(j);
                        g_b[cnt].setX(i);
                        g_c[cnt].setY(j);
                        g_c[cnt].setX(i - 1);

                        cnt++;
                    }
                    else
                    {
                        if (flag == 1)
                        {
                            break;
                        }
                    }
                }
            }
            do
            {
                int k = 0;
                g_bb[k].setY(g_b[(cnt - 1)].Y);
                g_bb[k].setX(g_b[(cnt - 1)].X);
                g_cc[k].setY(g_c[(cnt - 1)].Y);
                g_cc[k].setX(g_c[(cnt - 1)].X);
                Border next;
                do
                {
                    next = find_next_neighbour(g_bb[k], g_cc[k]);
                    g_cc[(k + 1)].setY(next.Y);
                    g_cc[(k + 1)].setX(next.X);
                    g_bb[(k + 1)].setY(g_bb[k].Y);
                    g_bb[(k + 1)].setX(g_bb[k].X);
                    k++;
                } while (temp[next.Y, next.X] != 1);
                g_b[cnt].setY(next.Y);
                g_b[cnt].setX(next.X);
                g_c[cnt].setY(g_cc[(k - 1)].Y);
                g_c[cnt].setX(g_cc[(k - 1)].X);


                cnt++;
            } while ((Math.Abs(g_b[(cnt - 1)].Y - g_b[0].Y) != 0) || (Math.Abs(g_b[(cnt - 1)].X - g_b[0].X) != 0));
            g_perimeter = (cnt - 1);
            for (int j = 0; j < g_height; j++)
            {
                for (int i = 0; i < g_width; i++)
                {
                    temp[j, i] *= 255;
                }
            }

            for (int i = 0; i < g_perimeter; i++)
            {
                temp_02[g_b[i].X, g_b[i].Y] = 255;
            }

            return temp_02;
        }

        public int[,] FourierDescriptorRepresentation(int input, String type)
        {
            FourierDescriptor[] s, a, r;
            int k, u, P;
            double real_sum, imaginary_sum;

            int[,] temp = borderFollowing();

            s = new FourierDescriptor[g_perimeter];
            a = new FourierDescriptor[g_perimeter];
            r = new FourierDescriptor[g_perimeter];
            for(k = 0 ; k < g_perimeter ; k++)
            {
                s[k] = new FourierDescriptor();
                a[k] = new FourierDescriptor();
                r[k] = new FourierDescriptor();
            }

        

            for( k = 0 ; k < g_perimeter ; k++)
            {
                s[k].FourierRealPart = g_b[k].X;
                s[k].FourierImaginaryPart = g_b[k].Y;
            }


            int[,] temp2 = new int[g_pixel.GetLength(0), g_pixel.GetLength(1)];
        
            for (int j = 0; j < g_pixel.GetLength(0); j++) {
                for (int i = 0; i < g_pixel.GetLength(1); i++) {
                    temp2[j, i] = 0;
                }
            }

            if(string.Equals(type, "Original", StringComparison.OrdinalIgnoreCase))
            {
                for ( k = 0 ; k < g_perimeter ; k++ )
                    if ( (int)( 0.5 + r[ k ].FourierImaginaryPart ) < g_pixel.GetLength(0) && (int)( 0.5 + r[ k ].FourierRealPart ) < g_pixel.GetLength(1))
                        temp2[ (int)( 0.5 + s[ k ].FourierImaginaryPart ), (int)( 0.5 + s[ k ].FourierRealPart ) ] = 255;
            
                return temp2;   
            }


            //forward
            for(u = 0 ; u < g_perimeter ; u++)
            {
                real_sum = 0;
                imaginary_sum = 0;
                for(k = 0 ; k < g_perimeter ; k++)
                {
                    real_sum += s[ k ].FourierRealPart      * Math.Cos( 2*Math.PI* (double)u*k/g_perimeter ) + s[ k ].FourierImaginaryPart * Math.Sin( 2*Math.PI* (double)u*k/g_perimeter );
                    imaginary_sum +=  s[ k ].FourierImaginaryPart * Math.Cos( 2*Math.PI* (double)u*k/g_perimeter ) - s[ k ].FourierRealPart * Math.Sin( 2*Math.PI* (double)u*k/g_perimeter );
                }
                a[u].FourierRealPart = real_sum;
                a[u].FourierImaginaryPart = imaginary_sum;
            }

            P = g_perimeter;
            int P2 = input;

            for ( k = 0 ; k < g_perimeter ; k++ ) 
            {
                real_sum      = 0 ;
                imaginary_sum = 0 ;
    
                for ( u = 0 ; u < P2 ; u++ ) 
                {    
                    real_sum      += a[ u ].FourierRealPart      * Math.Cos( 2*Math.PI* (float)u*k/g_perimeter ) - a[ u ].FourierImaginaryPart * Math.Sin( 2*Math.PI* (float)u*k/g_perimeter );
                    imaginary_sum += a[ u ].FourierImaginaryPart * Math.Cos( 2*Math.PI* (float)u*k/g_perimeter ) + a[ u ].FourierRealPart      * Math.Sin( 2*Math.PI* (float)u*k/g_perimeter );
                }

                for ( u = g_perimeter-P2 ; u < g_perimeter ; u++ ) 
                {    
                    real_sum      += a[ u ].FourierRealPart      * Math.Cos( 2*Math.PI* (float)u*k/g_perimeter ) - a[ u ].FourierImaginaryPart * Math.Sin( 2*Math.PI* (float)u*k/g_perimeter );
                    imaginary_sum += a[ u ].FourierImaginaryPart * Math.Cos( 2*Math.PI* (float)u*k/g_perimeter ) + a[ u ].FourierRealPart      * Math.Sin( 2*Math.PI* (float)u*k/g_perimeter );
                }

                r[ k ].FourierRealPart      = real_sum / g_perimeter;
                r[ k ].FourierImaginaryPart = imaginary_sum / g_perimeter;
            }

            for ( k = 0 ; k < g_perimeter ; k++ )
                if ( (int)( 0.5 + r[ k ].FourierImaginaryPart ) < g_pixel.GetLength(0) && (int)( 0.5 + r[ k ].FourierRealPart ) < g_pixel.GetLength(1) )
                    temp2[ (int)( 0.5 + r[ k ].FourierImaginaryPart ), (int)( 0.5 + r[ k ].FourierRealPart ) ] = 255;
        
            return temp2;
        }

        public int[,] FourierDescriptorRepresentation_02(int input, String type, string apa)
        {
            FourierDescriptor[] s, a, r;
            int k, u, P;
            double real_sum, imaginary_sum;

            int[,] temp = borderFollowing_02();

            string res_folder = "9999";
            PGM_Iris pi = new PGM_Iris();
            pi.WriteToPath(res_folder + "\\border_following_" + apa +".pgm", temp);

            s = new FourierDescriptor[g_perimeter];
            a = new FourierDescriptor[g_perimeter];
            r = new FourierDescriptor[g_perimeter];
            for (k = 0; k < g_perimeter; k++)
            {
                s[k] = new FourierDescriptor();
                a[k] = new FourierDescriptor();
                r[k] = new FourierDescriptor();
            }
            

            for (k = 0; k < g_perimeter; k++)
            {
                s[k].FourierRealPart = g_b[k].X;
                s[k].FourierImaginaryPart = g_b[k].Y;
            }


            int[,] temp2 = new int[g_pixel.GetLength(0), g_pixel.GetLength(1)];

            for (int j = 0; j < g_pixel.GetLength(0); j++)
            {
                for (int i = 0; i < g_pixel.GetLength(1); i++)
                {
                    temp2[j, i] = 0;
                }
            }

            if (string.Equals(type, "Original", StringComparison.OrdinalIgnoreCase))
            {
                for (k = 0; k < g_perimeter; k++)
                    if ((int)(0.5 + r[k].FourierImaginaryPart) < g_pixel.GetLength(0) && (int)(0.5 + r[k].FourierRealPart) < g_pixel.GetLength(1))
                        temp2[(int)(0.5 + s[k].FourierImaginaryPart), (int)(0.5 + s[k].FourierRealPart)] = 255;

                return temp2;
            }


            //forward
            for (u = 0; u < g_perimeter; u++)
            {
                real_sum = 0;
                imaginary_sum = 0;
                for (k = 0; k < g_perimeter; k++)
                {
                    real_sum += s[k].FourierRealPart * Math.Cos(2 * Math.PI * (double)u * k / g_perimeter) - s[k].FourierImaginaryPart * Math.Sin(2 * Math.PI * (double)u * k / g_perimeter);
                    imaginary_sum += s[k].FourierImaginaryPart * Math.Cos(2 * Math.PI * (double)u * k / g_perimeter) + s[k].FourierRealPart * Math.Sin(2 * Math.PI * (double)u * k / g_perimeter);
                }
                a[u].FourierRealPart = real_sum;
                a[u].FourierImaginaryPart = imaginary_sum;
            }

            P = g_perimeter;
            int P2 = input;

            for (k = 0; k < g_perimeter; k++)
            {
                real_sum = 0;
                imaginary_sum = 0;

                for (u = 0; u < P2; u++)
                {
                    real_sum += a[u].FourierRealPart * Math.Cos(2 * Math.PI * (float)u * k / g_perimeter) + a[u].FourierImaginaryPart * Math.Sin(2 * Math.PI * (float)u * k / g_perimeter);
                    imaginary_sum += a[u].FourierImaginaryPart * Math.Cos(2 * Math.PI * (float)u * k / g_perimeter) - a[u].FourierRealPart * Math.Sin(2 * Math.PI * (float)u * k / g_perimeter);
                }

                for (u = g_perimeter - P2; u < g_perimeter; u++)
                {
                    real_sum += a[u].FourierRealPart * Math.Cos(2 * Math.PI * (float)u * k / g_perimeter) + a[u].FourierImaginaryPart * Math.Sin(2 * Math.PI * (float)u * k / g_perimeter);
                    imaginary_sum += a[u].FourierImaginaryPart * Math.Cos(2 * Math.PI * (float)u * k / g_perimeter) - a[u].FourierRealPart * Math.Sin(2 * Math.PI * (float)u * k / g_perimeter);
                }

                r[k].FourierRealPart = real_sum / g_perimeter;
                r[k].FourierImaginaryPart = imaginary_sum / g_perimeter;
            }

            for (k = 0; k < g_perimeter; k++)
                if ((int)(0.5 + r[k].FourierImaginaryPart) < g_pixel.GetLength(0) && (int)(0.5 + r[k].FourierRealPart) < g_pixel.GetLength(1))
                    temp2[(int)(0.5 + r[k].FourierImaginaryPart), (int)(0.5 + r[k].FourierRealPart)] = 255;

            return temp2;
        }
    }
}

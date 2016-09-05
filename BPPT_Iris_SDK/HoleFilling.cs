using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class HoleFilling
    {
        public HoleFilling()
        {

        }

        public int[,] runHoleFilling(int[,] pixel)
	    {

		    int height = pixel.GetLength(0);
		    int width = pixel.GetLength(1);
		    int done = 0;
		    int[,] pix1 = new int[height,width];
		    int[,] pix2 = new int[height,width];
		    int[,] pix3 = new int[height,width];

		    //int i,j,k,l,m,n;

		    for(int j = 0 ; j < height ; j++)
			    for(int i = 0 ; i < width ; i++)
			    {
				    pix1[j,i] = pixel[j,i];
				    pix2[j,i] = 255;
				    pix3[j,i] = 255;
			    }

		    pix2[0,0] = 0;
		    pix2[1,1] = 0;
            
            while(done == 0)
            {
         	    done = 1;
         	    for ( int m = 0 ; m < height; m++ ) /* make duplicate image to compare */
          	    {
            	    for ( int n = 0 ; n < width; n++ )
            	    {
              		    pix3[m,n] = pix2[m,n];
            	    }
          	    }
          	    for ( int m = 1 ; m < height-1; m++ ) /* start to dilate -> union */
          	    {
            	    for ( int n = 1 ; n < width-1; n++ )
            	    {
              		    if(pix2[m,n] == 0)
              		    {
                  
                  		    if(pix1[m,n-1] == 0)
                  		        pix2[m,n-1] = 0;
                  		    else
                  		        pix2[m,n-1] = 255;
            		
                  		    if(pix1[m,n+1] == 0)
                  		        pix2[m,n+1] = 0;
                  		    else
                  		        pix2[m,n+1] = 255;
                  		
                  		    if(pix1[m-1,n] == 0)
                  		        pix2[m-1,n] = 0;
                  		    else
                  		        pix2[m-1,n] = 255;
                  		
                  		    if(pix1[m-1,n-1] == 0)
                  		        pix2[m-1,n-1] = 0;
                  		    else
                  		        pix2[m-1,n-1] = 255;
                  		
                  		    if(pix1[m-1,n+1] == 0)
                  		        pix2[m-1,n+1] = 0;
                  		    else
                  		        pix2[m-1,n+1] = 255;
                  		
                  		    if(pix1[m+1,n] == 0)
                  		        pix2[m+1,n] = 0;
                  		    else
                  		        pix2[m+1,n] = 255;
                  		
                  		    if(pix1[m+1,n-1] == 0)
                  		        pix2[m+1,n-1] = 0;
                  		    else
                  		        pix2[m+1,n-1] = 255;
                  		
                  		    if(pix1[m+1,n+1] == 0)
                  		        pix2[m+1,n+1] = 0;
                  		    else
                  		        pix2[m+1,n+1] = 255;
                  	    }
            	    }
          	   }

          	   for ( int m = 0 ; m < height; m++ ) /* make duplicate image to compare */
          	   {
            	    for ( int n = 0 ; n < width; n++ )
            	    {
              		    if(done == 1)
              		    {
              			    if(pix2[m,n] != pix3[m,n])
              			    {
              				    done = 0;
              			    }
              		    }
            	    }
          	   }
            }

          for ( int m = 0 ; m < height; m++ ) /* make duplicate image to compare */
          {
                for ( int n = 0 ; n < width; n++ )
            	{
                    if(pix2[m,n] == 0)
              	        pix2[m,n] = 255;
              		else
              	        pix2[m,n] = 0;
            	    }
          	    }
    	        return pix2;
	      }
    }
}

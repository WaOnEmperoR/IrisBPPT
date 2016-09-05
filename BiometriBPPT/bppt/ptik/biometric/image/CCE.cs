using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiometriBPPT.bppt.ptik.biometric.image
{
    public class CCE
    {
        private static int OBJECT_INTENSITY = 255;
        private static int L_BASE = 5;
        private int distance;
        private int area;
        private int pixelsum;

        // function labelset and labelling are based on Inoue's book Jissen gazou shori, Ohmsha, pp.93-94
        public void LabelSet(int[,] pixels, int ys, int xs, int label)
        {
            int i,j,cnt,im,ip,jm,jp; 

	        int height = pixels.GetLength(0);
	        int width = pixels.GetLength(1);
	
            pixels[ys, xs] = label;
            for(;;) {
                cnt = 0;
                for(j = 0; j < height; j++)
                    for(i = 0; i < width; i++)
				        if(pixels[j, i] == label) {
					        im=j-1;  ip=j+1;   jm=i-1; jp=i+1;
					        if (im < 0) 
						        im = 0; 
					        if (ip > height - 1) 
						        ip = height - 1;
					        if (jm < 0) 
						        jm = 0; 
					        if(jp > width - 1)  
						        jp = width - 1;
					
					        if (pixels[j, jp] == OBJECT_INTENSITY) {     // tengok kanan
					            pixels[j, jp] =  label; cnt++;  
					        }
					
					        if (pixels[im, jp] == OBJECT_INTENSITY) {    // tengok kanan atas
					            pixels[im, jp] =  label; cnt++;  
					        }
					
					        if (pixels[im, i] == OBJECT_INTENSITY) {    // tengok atas
						        pixels[im, i] =  label; cnt++;
					        }
					
					        if (pixels[im, jm] == OBJECT_INTENSITY) {    // tengok kiri atas   
						        pixels[im, jm] =  label; cnt++;
					        }
					
					        if (pixels[j, jm] == OBJECT_INTENSITY) {  // tengok kiri
						        pixels[j, jm] =  label; cnt++;
					        }
					
					        if (pixels[ip, jm] == OBJECT_INTENSITY) {     // tengok kiri bawah
						        pixels[ip, jm] =  label; cnt++;
					        }
					
					        if (pixels[ip, i] == OBJECT_INTENSITY) {       // tengok bawah
						        pixels[ip, i] =  label; cnt++;
					        }
					
					        if (pixels[ip, jp] == OBJECT_INTENSITY) {        // tengok kanan bawah
						        pixels[ip, jp] =  label; cnt++;
					        }
				        }
            if(cnt == 0) break;
          }

	        distance = 0;
	        area = 0;
	        pixelsum = 0;
	
	        int centerX = 0;
            int centerY = 0;
	        int sumX = 0;
	        int sumY = 0;
	        int countX = 0;
	        int countY = 0;
	
	        for (i = 0; i < height; i++)
		        for (j = 0; j < width; j++) 
			        if (pixels[i, j] == label) {
				        pixelsum += pixels[i, j];
				        sumX += i;
				        countX++;
				        sumY += j;
				        countY++;
				        area ++;
			        }
	        centerX = sumX / countX;
	        centerY = sumY / countY;
	        distance = (int) Math.Round(Math.Sqrt(Math.Pow(centerX - width/2, 2) + Math.Pow(centerY - height/2, 2)));
        }

        public void CreateImage(int[,] pixels, int label, int isPupil)
        {
            int  i,j;
            int black=0, white=255;
  
            int height = pixels.GetLength(0);
            int width = pixels.GetLength(1);

            int[,] temp = new int[height, width];

            for(j = 0; j < height; j++) {
                for(i = 0; i < width; i++) {
                    if(pixels[j, i] == label) {
				        if (isPupil == 0) {
					        temp[j, i] = 0;
				        }
				        else if (isPupil == 1) {
					        temp[j, i] = 255;
				        }
			        }
                    else {
      	                if (isPupil == 0) {
			                temp[j, i] = 255;
			            }
				        else if (isPupil == 1) {
				            temp[j, i] = 0;
			            }
			        }
                }
	        }
	
	        for (j = 0; j < height; j++) {
		        for (i = 0; i < width; i++) {
			        pixels[j, i] = temp[j, i];
		        }
	        }
        }

        public void Labelling(int[,] pixels, int isPupil)
        {
            int i,j,k,label;
	        int height = pixels.GetLength(0);
            int width = pixels.GetLength(1);
          
            int[,] labelPixels = new int[height, width];
	
	        int[] areahistogram = new int[200];
	        int[] distance_array = new int[200];
	
	        for (i = 0; i < 200; i++)
		        areahistogram[i] = 0;

            for(j = 0; j < height; j++)
                for(i = 0; i < width; i++)
                    labelPixels[j, i] = 255 - pixels[j, i]; // object is changed to become a a pixel with HIGH intensity 
 
	        label = L_BASE;
  
            for(j = 0; j < height; j++)
                for(i = 0; i < width; i++) {
                    if(labelPixels[j, i] == OBJECT_INTENSITY) {
				        if(label >= OBJECT_INTENSITY) {
                            return;
				        }
				        LabelSet(labelPixels, j, i, label);
				
				        areahistogram[label] = area;
				        distance_array[label] = distance;
				        label++;
              }
            }
    
	        int[] array_area = new int[label-L_BASE];
            int[] array_area_n = new int[label-L_BASE];
            int[] array_distance = new int[label - L_BASE]; 
            int[] array_distance_n = new int[label - L_BASE];
	        
            int maxarea = 0;
	        int maxarea_n = 0;

	        for (k = 0; k < label-L_BASE; k++) {			
		        if (areahistogram[k+L_BASE] > maxarea) {			
			        maxarea = areahistogram[k+L_BASE];
			        maxarea_n = k+L_BASE;
		        }
	        }
	
	        if (isPupil == 0) {
		        for (k = 0; k < label-L_BASE; k++) {
			        array_area[k] = areahistogram[k+L_BASE];
			        array_area_n[k] = k + L_BASE;
			        array_distance[k] = distance_array[k+L_BASE];
			        array_distance_n[k] = k + L_BASE;
		        }

		        BubblesortDsc(array_area, array_area_n, label-L_BASE);
	
		        BubblesortAsc(array_distance, array_distance_n, label-L_BASE);

		        int area_threshold = 1000;
		        for (k = 0; k < label-L_BASE; k++) {
			        if (array_area_n[k] == array_distance_n[k] && array_area[k] > area_threshold) {
				        maxarea_n = array_area_n[k];
			        }
		        }
	        }

	        CreateImage(labelPixels, maxarea_n, isPupil);

            for (j = 0; j < height; j++) {
  	            for (i = 0; i < width; i++) {
  		            pixels[j, i] = labelPixels[j, i];
  	            }
            }
        }

        public int find_next_maxarea_n(int[] array, int current_max, int length) 
        {
	        int i, j;
	        int new_max = 0;
	        int new_max_n = 0;
	        for (i = 0; i < length; i++) {
		        if (array[i] > new_max && array[i] < current_max) {
			        new_max_n = i + L_BASE;
			        new_max = array[i];
		        }  
	        }
	        return new_max_n;
        }

        public int find_next_min_distance_n(int[] array, int current_min, int length)
        {
	        int i;
	        int new_min = array[L_BASE];
	        int new_min_n = L_BASE;
	        for (i = 1; i < length; i++) {
		        if (array[i] < new_min && array[i] > current_min) {
			        new_min_n = i + L_BASE;
			        new_min = array[i];
		        }
	        }
	        return new_min_n;
        }

        public void BubblesortAsc(int[] list, int[] list_n, int n)
        {
	        int i,j;
            int tmp = 0;
	        for(i = 0; i < (n-1); i++)
		        for(j = 0; j < (n-(i+1)); j++)
			        if (list[j] > list[j+1]) {
                        tmp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = tmp;

                        tmp = list_n[j];
                        list_n[j] = list_n[j + 1];
                        list_n[j + 1] = tmp;		       
	                }
        }

        public void BubblesortDsc(int[] list, int[] list_n, int n)
        {
	        int i,j;
            int tmp;
		    for(i = 0; i < (n-1); i++)
			    for(j = 0; j < (n-(i+1)); j++)
		             if (list[j] < list[j+1]) {
                         tmp = list[j];
                         list[j] = list[j + 1];
                         list[j + 1] = tmp;

                         tmp = list_n[j];
                         list_n[j] = list_n[j + 1];
                         list_n[j + 1] = tmp;
		             }
        }
    }
}

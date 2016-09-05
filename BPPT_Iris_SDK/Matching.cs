using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class Matching
    {
        private IrisCode irisCode;
        private IrisCode matchIrisCode;

        public Matching(IrisCode irisCode, IrisCode matchIrisCode)
        {
            this.irisCode = irisCode;
            this.matchIrisCode = matchIrisCode;
        }

        public Matching(IrisCode irisCode)
        {
            this.irisCode = irisCode;
        }

        public float PerformMatching()
        {
            int i;
            IrisCode tmp_irisCode = new IrisCode();
            tmp_irisCode.size = matchIrisCode.size;
            tmp_irisCode.bit = matchIrisCode.bit;
            tmp_irisCode.mask = matchIrisCode.mask;

            float HD = 0;
            float HDl = 0;
            float HDr = 0;
            float max_HD = 1;
            //int maxHD_id = 0;

            /*
			for (j = 0; j < 40; j++) {
				for (i = 0; i < 30; i++) {//120; i++) {
					tmp_irisCode = shift_left((j * 30)+i);
					HD = find_HD_match(irisCode, tmp_irisCode);
					//Console.WriteLine("HD = " + HD);
					if (HD < max_HD) {
						max_HD = HD;
						maxHD_id = i;
					}
				}
		
				for (i = 0; i < 30; i++) {//120; i++) {
					tmp_irisCode = shift_right((j * 30) + i);
					HD = find_HD_match(irisCode, tmp_irisCode);

					if (HD < max_HD) {
						max_HD = HD;
						maxHD_id = i;
					}
				}
			}
			*/
            for (i = 0; i < 10; i++)
            {//120; i++) {
                tmp_irisCode = shift_left(i);
                HDl = find_HD_match(irisCode, tmp_irisCode);
                //Console.WriteLine(HD);
                tmp_irisCode = shift_right(i);
                HDr = find_HD_match(irisCode, tmp_irisCode);

                if (HDr < HDl)
                {
                    HD = HDr;
                }
                else
                {
                    HD = HDl;
                }
                if (HD < max_HD)
                {
                    max_HD = HD;
                    //maxHD_id = i;
                }
            }

            return max_HD;
        }

        public string MatchingOneToMany(string irisImageName)
        {
            string[] filePath = System.IO.Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory());
            float tmp = 0;
            string imageName = "";
            char[] sep = { '\\' };

            for (int i = 0; i < filePath.Length; i++)
            {
                string[] folderName = filePath[i].Split(sep);

                if (!irisImageName.Equals(folderName[folderName.Length - 1]))
                {
                    GaborFilter gaborFilter = new GaborFilter(filePath[i]);
                    this.matchIrisCode = gaborFilter.PerformGaborFilter();
                    float hd = this.PerformMatching();

                    if (hd < tmp || tmp == 0)
                    {
                        tmp = hd;
                        imageName = folderName[folderName.Length - 1];
                    }
                }
            }

            string result = imageName + "_" + tmp.ToString();

            return result;
        }

        public IrisCode shift_left(int degree)
        {
            IrisCode irisCodeResult = new IrisCode();
            int length = matchIrisCode.size;

            irisCodeResult.size = length;

            int j, c, d;

            byte[] temp = new byte[length];
            byte[] temp_mask = new byte[length];

            for (j = 0; j < length; j++)
            {
                c = j + degree;
                if (c >= length)
                    d = c - length;
                else
                    d = c;
                temp[j] = matchIrisCode.bit[d];
                temp_mask[j] = matchIrisCode.mask[d];
            }

            irisCodeResult.bit = temp;
            irisCodeResult.mask = temp_mask;

            return irisCodeResult;
        }

        public IrisCode shift_right(int degree)
        {
            IrisCode irisCodeResult = new IrisCode();
            int length = matchIrisCode.size;

            irisCodeResult.size = length;

            int j, c, d;

            byte[] temp = new byte[length];
            byte[] temp_mask = new byte[length];

            for (j = length - 1; j >= 0; j--)
            {
                c = j - degree;
                if (c < 0)
                    d = c + length;
                else
                    d = c;
                temp[j] = matchIrisCode.bit[d];
                temp_mask[j] = matchIrisCode.mask[d];
            }

            irisCodeResult.bit = temp;
            irisCodeResult.mask = temp_mask;

            return irisCodeResult;
        }

        public float find_HD_match(IrisCode irisCode, IrisCode matchIrisCode)
        {
            int j;

            int length = matchIrisCode.size;
            if (irisCode.size < length)
            {
                length = irisCode.size;
            }
            int diff_c = 0;
            int usable_bit = 0;
            float HD = 0;
            for (j = 0; j < length; j++)
            {
                if (irisCode.mask[j] == 0 && matchIrisCode.mask[j] == 0)
                {
                    if (matchIrisCode.bit[j] != irisCode.bit[j])
                        diff_c++;
                    usable_bit++;
                }
            }
            HD = (float)diff_c / usable_bit;

            return HD;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Project_fat_file
{
    class Mini_Fat
    {
        static int[] fat = new int[1024];

        static public void PrepareFat()
        {
            for (int i = 0; i < 1024; i++)
            {
                if (i==0 || i==4)
                {
                    fat[i] = -1;
                }
                else if (i == 1 || i == 2 || i==3)
                {
                    fat[i] = i+1;
                }
                else
                {
                    fat[i] = 0;
                }
                
            }
        }

        
        static public void WriteFat()
        {
            byte[] b = new byte[4096];

            //the next line is converting the fat array to array of byte of size 4096
            System.Buffer.BlockCopy(fat, 0, b, 0, b.Length);

            for (int i = 0; i < 4; i++)
            {
                byte[] c = new byte[1024];

                //the next for loop takes the next 1024 byte from b and puts them in c
                for (int j = i * 1024, k = 0; k < 1024; j++, k++) 
                {
                    c[k] = b[j];
                }

                VertualDisk.writeCluster(i + 1, c);

            }

        }


        static public void ReadFat()          
        {
            byte[] b = new byte[4096];
            for (int i = 0; i < 4; i++)
            {
                byte[] c = VertualDisk.readCluster(i + 1);

                //the next for loop takes the next 1024 byte from b and puts them in c
                for (int j = i * 1024, k = 0; k < 1024; j++, k++)
                {
                    b[j] = c[k] ; 
                }


                //the next line is converting the array of byte of size 4096 to the fat array 
            }
            System.Buffer.BlockCopy(b, 0, fat, 0, b.Length);
        }

        public static void print()
        {
            for(int i=0;i<fat.Length;i++)
            {
                Console.WriteLine($"fat[{i}] = {fat[i]}");
            }
        }

        static public int GetEmptyCluster()
        {
            for (int i = 5; i < 1024; i++)
            {
                if (fat[i]==0)
                {
                    return i;
                }
            }
            return -1;
        }

        static public void SetClusterStatus(int ci,int status)
        {
            fat[ci] = status;
        }

        static public int GetClusterStatus(int ci)
        {
            return fat[ci];
        }

        static public int getAvailableClusters()
        {
            int s = 0;

            for (int i = 0; i < fat.Length; i++)
            {
                if (fat[i]==0)
                {
                    s++;
                }
            }
            return s;
        }

        static public int getFreeSpace()
        {
            //return (1024 * 1024) - (int)VertualDisk.Disk.Length;

            // or

          return (1024 * getAvailableClusters());
        }

    }
}

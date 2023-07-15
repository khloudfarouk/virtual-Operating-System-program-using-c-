using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Project_fat_file
{
    class DirectoryEntry
    {
        public  char[] dir_name = new char[11];
        public  byte dir_attr;
        public  byte[] dir_empty = new byte[12];
        public  int [] dir_FirstCluster= new int[1], dir_FileSize = new int[1];

        public DirectoryEntry(string name="",byte attr= 0x10,int first=0,int size=0)
        {

            dir_name = name.ToCharArray();
            dir_attr = attr;
            for (int i = 0; i < 12; i++)
            {
                dir_empty[i] = 0x0;
            }
            dir_FirstCluster[0] = first;
            dir_FileSize[0] = size;

        }
    }
}
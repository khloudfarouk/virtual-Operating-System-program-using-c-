using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using Microsoft.Azure.Amqp.Framing;

namespace OS_Project_fat_file
{
    class VertualDisk
    {
        static public FileStream Disk;

        static public void  initialize(string name)
        {
            if (!File.Exists(name)) 
            {
                //Disk = File.Open(name, FileMode.Create, FileAccess.ReadWrite);
                Disk = new FileStream(name,FileMode.Create,FileAccess.ReadWrite);  
                byte[] b = new byte[1024];
                writeCluster(0, b);
                 Mini_Fat.PrepareFat(); 
                 Mini_Fat.WriteFat();    

            }
            else
            {
                Disk = new FileStream(name, FileMode.Open);
                Mini_Fat.ReadFat();   

            }

        }

        static public void writeCluster(int ci,byte [] b)
        {
            Disk.Seek(ci * 1024, SeekOrigin.Begin);
            Disk.Write(b);
            Disk.Flush();

        }
        
        static public byte[] readCluster(int ci)
        {
          
            Disk.Seek(ci * 1024,  SeekOrigin.Begin);
            byte[] b = new byte[1024];
            Disk.Read(b);
            return b;
        }
        
    }
}

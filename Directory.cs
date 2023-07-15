using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OS_Project_fat_file
{
    class Directory : DirectoryEntry
    {
        public List<DirectoryEntry> Dir_Files;
        public Directory parent;

        public Directory(string name = "", byte attr = 0x10, int first = 0, int size = 0, Directory d = null) : base(name, attr, first, size)
        {
            parent = d;
        }

        public void WriteDirectory()
        {
            DirectoryEntry old = getDirectoryEntry();
            byte[] b = new byte[Dir_Files.Count * 32];

            byte[] dirName = new byte[11];
            byte[] dirFirstCluster = new byte[4];
            byte[] dirFileSize = new byte[4];
            int c = 0;
            for (int i = 0; i < Dir_Files.Count; i++)
            {
                //System.Buffer.BlockCopy(Dir_Files[i].dir_name, 0, dirName, 0, Dir_Files[i].dir_name.Length);
                System.Buffer.BlockCopy(Dir_Files[i].dir_FirstCluster, 0, dirFirstCluster, 0, dirFirstCluster.Length);
                System.Buffer.BlockCopy(Dir_Files[i].dir_FileSize, 0, dirFileSize, 0, dirFileSize.Length);

                for (int j = c, k = 0; k < Dir_Files[i].dir_name.Length; j++, k++)
                {
                    b[j] = (byte)Dir_Files[i].dir_name[k];
                }
                //if (Dir_Files[i].dir_name.Length < 11)
                //{
                //    for (int r = Dir_Files[i].dir_name.Length; r < 11; r++)
                //    {
                //        b[r] = (byte)'\0';
                //    }
                //}

                b[c + 11] = Dir_Files[i].dir_attr;

                for (int j = c + 12, k = 0; k < 12; j++, k++)
                {
                    b[j] = Dir_Files[i].dir_empty[k];
                }

                for (int j = c + 24, k = 0; k < 4; j++, k++)
                {
                    b[j] = dirFirstCluster[k];
                }

                for (int j = c + 28, k = 0; k < 4; j++, k++)
                {
                    b[j] = dirFileSize[k];
                }

                c += 32;
            }


            List<byte[]> ArrayList = new List<byte[]>();

            for (int i = 0; i < Math.Ceiling(((double)(Dir_Files.Count * 32)) / 1024); i++)
            {
                ArrayList.Add(new byte[1024]);
            }

            for (int i = 0; i < ArrayList.Count; i++)
            {
                for (int j = 0, k = i * 1024; j < 1024; k++, j++)
                {
                    if (k < b.Length)
                        ArrayList[i][j] = b[k];
                    else
                        break;
                }
            }
            ///////////////////////////////////////////////////////////////////////////
            int clusterindex;

            if (dir_FirstCluster[0] == 0)
            {
                clusterindex = Mini_Fat.GetEmptyCluster();
                dir_FirstCluster[0] = clusterindex;
            }
            else
            {
                emptyMyCluster();

                clusterindex = Mini_Fat.GetEmptyCluster();
                dir_FirstCluster[0] = clusterindex;
            }

            int lastClaster = -1;
            ///////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < ArrayList.Count; i++)
            {
                if (clusterindex != -1)
                {
                    VertualDisk.writeCluster(clusterindex, ArrayList[i]);
                    Mini_Fat.SetClusterStatus(clusterindex, -1);
                    if (lastClaster != -1)
                    {
                        Mini_Fat.SetClusterStatus(lastClaster, clusterindex);
                    }
                    lastClaster = clusterindex;
                    clusterindex = Mini_Fat.GetEmptyCluster();
                }
            }

            ///////////////////////////////////////////////////////////////////////////

            if (parent != null)
            {
                parent.updateContent(old, getDirectoryEntry());                    //???????????????????????????????????????????????????????????????????????????????????????????????????

                parent.WriteDirectory();
            }

            Mini_Fat.WriteFat();

        }

        public void readDirectory()
        {
            if (dir_FirstCluster[0] != 0)            //not empty
            {
                Dir_Files = new List<DirectoryEntry>();
                int clusterIndex = dir_FirstCluster[0];         //current
                int next = Mini_Fat.GetClusterStatus(clusterIndex);         //fat[clusterIndex]
                if (clusterIndex == 5 && next == 0)                   
                {
                    return;
                }

                byte[] dirName = new byte[11];
                byte[] dirFirstCluster = new byte[4];
                byte[] dirFileSize = new byte[4];

                
                do
                {
                    byte[] b = VertualDisk.readCluster(clusterIndex);


                    int c = 0;

                    for (int i = 0; i < (b.Length) / 32; i++)             //  تقريبا فيه مشكلة لو الكلاستر دا اخر واحد ف الفايل ؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟
                    {

                        if (b[c] != 0)
                        {
                            DirectoryEntry item = new DirectoryEntry();
                            for (int j = c, k = 0; k < 11; j++, k++)
                            {
                                dirName[k] = b[j];
                            }

                            item.dir_attr = b[c + 11];

                            for (int j = c + 12, k = 0; k < 12; j++, k++)
                            {
                                item.dir_empty[k] = b[j];
                            }

                            for (int j = c + 24, k = 0; k < 4; j++, k++)
                            {
                                dirFirstCluster[k] = b[j];
                            }

                            for (int j = c + 28, k = 0; k < 4; j++, k++)
                            {
                                dirFileSize[k] = b[j];
                            }
                            item.dir_name = new char[11];
                            for (int n = 0; n < dirName.Length; n++)
                            {
                                item.dir_name[n] = (char)dirName[n];
                            }
                            //System.Buffer.BlockCopy(dirName, 0, item.dir_name, 0, dirName.Length);
                            System.Buffer.BlockCopy(dirFirstCluster, 0, item.dir_FirstCluster, 0, dirFirstCluster.Length);
                            System.Buffer.BlockCopy(dirFileSize, 0, item.dir_FileSize, 0, dirFileSize.Length);

                            Dir_Files.Add(item);

                            c += 32;
                        }
                        else
                        {
                            break;
                        }
                    }


                    clusterIndex = next;
                    if (clusterIndex != -1)
                    {
                        next = Mini_Fat.GetClusterStatus(clusterIndex);
                    }

                } while (clusterIndex != -1);
            }



            //List<byte> ls = new List<byte>();               //1024*count/32 bytes                 <<<<<<<<<<<<<<<<<<<<<<<<<<<<----------------------------
            //do
            //{
            //    ls.AddRange(VertualDisk.readCluster(clusterIndex));                //     <<<<<<<<<<<<<<<<<<<<<<-----------------------------
            //    clusterIndex = next;
            //    if (clusterIndex != -1) //not last
            //        next = Mini_Fat.GetClusterStatus(clusterIndex);
            //}
            //while (next != -1);
            //for (int i = 0; i < ls.Count; i++)
            //{
            //    byte[] b = new byte[32];
            //    for (int k = i * 32, m = 0; m < b.Length && k < ls.Count; m++, k++)
            //    {
            //        b[m] = ls[k];
            //    }
            //    if (base[0] == 0)
            //        break;
            //    Dir_Files.Add(Converter.BytesToDirectory_Entery(b));
            //}


        }

        public DirectoryEntry getDirectoryEntry()
        {
            DirectoryEntry opject = new DirectoryEntry();


            opject.dir_name = this.dir_name;


            opject.dir_attr = this.dir_attr;

            opject.dir_empty = this.dir_empty;


            opject.dir_FirstCluster = this.dir_FirstCluster;

            opject.dir_FileSize = this.dir_FileSize;

            return opject;
        }

        public int searchDirectory(string name)              
        {
            readDirectory();
            for (int i = 0; i < Dir_Files.Count; i++)
            {
                string n = new string(Dir_Files[i].dir_name);            
                if (n.Contains(name))
                    return i;
            }
            return -1;
        }

        public void removeEntry(DirectoryEntry d)         //؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟
        {
            readDirectory();
            string n = new string(d.dir_name);
            int index = searchDirectory(n);               //؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟
            Dir_Files.RemoveAt(index);
            WriteDirectory();
        }

        public void AddEntry(DirectoryEntry d)
        {
            Dir_Files.Add(d);
            WriteDirectory();
        }

        public void emptyMyCluster()       // ???????????????????????????????????????????????????????????????????????????????????????
        {
            if (this.dir_FirstCluster[0] != 0)
            {
                int cluster = this.dir_FirstCluster[0];
                int next = Mini_Fat.GetClusterStatus(cluster);
                if (cluster == 5 && next == 0)                    // ?????????????????????????????????????????????????????????
                {
                    return;
                }

                do
                {
                    Mini_Fat.SetClusterStatus(cluster, 0);
                    cluster = next;
                    if (cluster != -1)
                    {
                        next = Mini_Fat.GetClusterStatus(cluster);
                    }

                } while (cluster != -1);


            }
        }

        public void deleteDireactory()
        {
            if (Program.current == this)
            {
                //can't delet current
                return;
            }
            emptyMyCluster();
            if (parent != null)
            {
                this.parent.removeEntry(getDirectoryEntry());
            }

            Mini_Fat.WriteFat();          
        }

        public int getmysizedisk()
        {
            int size = 0;
            if (this.dir_FirstCluster[0] != 0)
            {
                int cluster = this.dir_FirstCluster[0];
                int next = Mini_Fat.GetClusterStatus(cluster);
                do
                {
                    size++;
                    cluster = next;
                    if (cluster != -1)
                    {
                        next = Mini_Fat.GetClusterStatus(cluster);
                    }

                } while (cluster != -1);
            }
            return size;
        }

        public bool canAddEntry(DirectoryEntry d)
        {
            bool can = false;
            int neededsize = (Dir_Files.Count + 1) * 32;
            int neededcluster = neededsize / 1024;
            int rem = neededsize % 1024;

            if (rem > 0)
            {
                neededcluster++;
                neededcluster += d.dir_FileSize[0] / 1024;
            }

            int rem1 = d.dir_FileSize[0] % 1024;
            if (rem1 > 0)
            {
                neededcluster++;
            }

            if (getmysizedisk() + Mini_Fat.getAvailableClusters() >= neededcluster)
            {
                can = true;
            }
            return can;
        }

        public void updateContent(DirectoryEntry Old, DirectoryEntry New)        // كدا صح؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟
        {
            string s = new string(Old.dir_name);
            int x = searchDirectory(s);
            if (x != -1)
            {
                removeEntry(Old);
            }
            AddEntry(New);
        }


    }
}

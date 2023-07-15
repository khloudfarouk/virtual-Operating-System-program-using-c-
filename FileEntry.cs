using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Project_fat_file
{
    class FileEntry : DirectoryEntry
    {
        public string content ;
        public Directory parent;

        public FileEntry(string name = "", byte attr = 0x0, int first = 0, int size = 0, Directory d = null,string Content="") : base(name, attr, first, size)
        {
            if (d!=null)
            {
                parent = d;
            }
           
            this.content = Content;
            this.dir_FileSize[0] = Content.Length;

        }

        public DirectoryEntry getfileEntry()
        {
            string s = new string(this.dir_name);
            DirectoryEntry opject = new DirectoryEntry(s,this.dir_attr,this.dir_FirstCluster[0],this.dir_FileSize[0]);

            if (opject.dir_attr==0x10)
            {
                opject.dir_FileSize[0] = 0;
            }
            else
            {
                opject.dir_FileSize[0] = this.dir_FileSize[0];
            }

            //for (int i = 0; i < 11; i++)
            //{
            //    opject.dir_name[i] = dir_name[i];
            //}

            //opject.dir_attr = dir_attr;

            //for (int i = 0; i < 12; i++)
            //{
            //    opject.dir_empty[i] = dir_empty[i];
            //}

            //opject.dir_FirstCluster[0] = dir_FirstCluster[0];

            //opject.dir_FileSize[0] = dir_FileSize[0];

            return opject;
        }

        public void emptyMyCluster()     
        {
            if (this.dir_FirstCluster[0] != 0)
            {
                int cluster = this.dir_FirstCluster[0];
                int next = Mini_Fat.GetClusterStatus(cluster);
                if (cluster==5 && next==0)                    // ?????????????????????????????????????????????????????????
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

        public void deleteFile()
        {
            //if (Program.current == this)
            //{
            //    //w
            //    return;
            //}
            emptyMyCluster();
            if (parent != null)
            {
                this.parent.removeEntry(getfileEntry());
            }

            Mini_Fat.WriteFat();
        }

        public void writeFileContent()
        {
            byte[] b = Encoding.ASCII.GetBytes(content);
            DirectoryEntry old = getfileEntry();
            List<byte[]> List_ofArraysof_1024 = new List<byte[]>();
            int Frist_Cluster = this.dir_FirstCluster[0];
            int last_cluster = -1, cluster_index;
            int y = 0;
            byte[] arr1024 = new byte[1024];
            //split b to list of arrays each array of size 1024            
            for (int i = 0; i < b.Length; i++)
            {

                if (y % 1024 == 0 && y != 0)
                {
                    y = 0;
                    List_ofArraysof_1024.Add(arr1024);
                }
                arr1024[y] = b[i];
                if (i + 1 == b.Length)
                {
                    List_ofArraysof_1024.Add(arr1024);
                    break;
                }
                y++;
            }
            if (this.dir_FirstCluster[0] == 0)
            {
                cluster_index = Mini_Fat.GetEmptyCluster();
                this.dir_FirstCluster[0] = cluster_index;
            }
            else
            {
                //empty all its cluster
                emptyMyCluster();
                cluster_index = Mini_Fat.GetEmptyCluster();
                this.dir_FirstCluster[0] = cluster_index;
            }
            for (int i = 0; i < List_ofArraysof_1024.Count; i++)
            {
                VertualDisk.writeCluster(cluster_index, List_ofArraysof_1024[i]);
                Mini_Fat.SetClusterStatus(cluster_index, -1);
                if (last_cluster != -1)
                {
                    Mini_Fat.SetClusterStatus(last_cluster, cluster_index);
                }
                last_cluster = cluster_index;
                cluster_index = Mini_Fat.GetEmptyCluster();
            }
            DirectoryEntry new1 = getfileEntry();
            if (this.parent != null)
            {
                this.parent.updateContent(old, new1);       //    دي بتاخد القديم و الجديد صح update_info  مش ال                              
                this.parent.WriteDirectory();
            }
            Mini_Fat.WriteFat();

        }


        //    System.Buffer.BlockCopy(content, 0, b, 0, content[1].Length);

        //    //حوليها لبايت حرف حرف؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟


        //    for (int i = 0; i < Math.Ceiling(((double)(b.Length)) / 1024); i++)
        //    {
        //        byte[] c = new byte[1024];

        //        for (int j = 0, k = i * 1024; j < 1024; j++, k++)
        //        {
        //            c[j] = b[k];
        //        }
        //        int index = Mini_Fat.GetEmptyCluster();
        //        VertualDisk.writeCluster(index, c);

        //        Mini_Fat.SetClusterStatus(index, -1);
        //    }

        //    int clusterindex;

        //    if (dir_FirstCluster[0] == 0)
        //    {
        //        clusterindex = Mini_Fat.GetEmptyCluster();
        //        dir_FirstCluster[0] = clusterindex;
        //    }
        //    else
        //    {
        //        emptyMyCluster();

        //        clusterindex = Mini_Fat.GetEmptyCluster();
        //        dir_FirstCluster[0] = clusterindex;
        //    }

        //int lastClaster = -1;
        /////////////////////////////////////////////////////////////////////////////
        //for (int i = 0; i < ArrayList.Count; i++)
        //{
        //    if (clusterindex != -1)
        //    {
        //        VertualDisk.writeCluster(clusterindex, ArrayList[i]);
        //        Mini_Fat.SetClusterStatus(clusterindex, -1);
        //        if (lastClaster != -1)
        //        {
        //            Mini_Fat.SetClusterStatus(lastClaster, clusterindex);
        //        }
        //        lastClaster = clusterindex;
        //        clusterindex = Mini_Fat.GetEmptyCluster();
        //    }
        //}

        /////////////////////////////////////////////////////////////////////////////

        //if (parent != null)
        //{
        //    parent.updateContent(old, getDirectoryEntry());                    //???????????????????????????????????????????????????????????????????????????????????????????????????

        //    parent.WriteDirectory();
        //}

        // Mini_Fat.WriteFat();





        // }

        public void readFileContent()   
        {
            if (dir_FirstCluster[0] != 0)
            {
                content = string.Empty;
                int cluster_index = dir_FirstCluster[0];
                int next = Mini_Fat.GetClusterStatus(cluster_index);
                if (cluster_index == 5 && next == 0)
                    return;
                List<byte> c = new List<byte>();
                do
                {
                    c.AddRange(VertualDisk.readCluster(cluster_index));
                    cluster_index = next;
                    if (cluster_index != -1)
                        next = Mini_Fat.GetClusterStatus(cluster_index);

                }
                while (cluster_index != -1);


                this.content += Encoding.ASCII.GetString(c.ToArray());
            }
        }


        //    byte[] b = new byte[content[1].Length];
        //    int clusterindex = dir_FirstCluster[0];
        //    int next = Mini_Fat.GetClusterStatus(clusterindex);


        //    for (int i = 0; i < Math.Ceiling(((double)(b.Length)) / 1024); i++)
        //    {

        //        byte[] c = VertualDisk.readCluster(clusterindex);

        //        for (int j = 0, k = i * 1024; j < 1024; j++, k++)
        //        {
        //            b[k] = c[j];
        //        }
        //        //if (clusterindex == -1) 
        //        //{
        //        //    break;
        //        //}
        //        clusterindex =next;
        //        next = Mini_Fat.GetClusterStatus(clusterindex);

        //    }

        //    System.Buffer.BlockCopy(b, 0, content, 0, b.Length);



    
        
        //}

        public void printContent()
        {
            readFileContent();
            Console.WriteLine($"{this.content}\n\n");
        }
    }
}

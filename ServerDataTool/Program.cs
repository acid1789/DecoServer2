using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ServerDataTool
{
    static class Program
    {
        public static Dictionary<ushort, NPCNameID> s_npcNameIDs;
        public static Dictionary<int, IntStrID> s_jobs;
        public static Dictionary<int, IntStrID> s_staticText;
        public static Dictionary<int, IntStrID> s_npcIcons;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitData();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        static void InitData()
        {
            string dataDir = "Data/";
            if (!Directory.Exists(dataDir))
            {
                dataDir = "../../Data/";
                if (!Directory.Exists(dataDir))
                {
                    throw new Exception("Cant find Data folder");
                }
            }

            InitNPCs(dataDir);
            InitJobs(dataDir);
            InitStaticText(dataDir);
            InitNPCIcons(dataDir);
        }

        static void InitNPCs(string dataDir)
        {
            string filename = dataDir + "NPCs.txt";
            s_npcNameIDs = new Dictionary<ushort, NPCNameID>();

            try
            {
                FileStream fs = File.OpenRead(filename);
                StreamReader sr = new StreamReader(fs);

                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] pieces = line.Split(',');
                    ushort id = Convert.ToUInt16(pieces[0]);
                    NPCNameID name = new NPCNameID(pieces[1], id);
                    s_npcNameIDs[id] = name;

                    line = sr.ReadLine();
                }                    
                    
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to read " + filename + "\n" + ex.ToString());
            }
        }

        static void InitJobs(string dataDir)
        {
            string filename = dataDir + "Jobs.txt";
            s_jobs = new Dictionary<int, IntStrID>();

            try
            {
                FileStream fs = File.OpenRead(filename);
                StreamReader sr = new StreamReader(fs);

                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] pieces = line.Split(',');
                    int id = Convert.ToInt32(pieces[0]);
                    s_jobs[id] = new IntStrID(pieces[1], id);

                    line = sr.ReadLine();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to read " + filename + "\n" + ex.ToString());
            }
        }

        static void InitNPCIcons(string dataDir)
        {
            string filename = dataDir + "NPCIcons.txt";
            s_npcIcons = new Dictionary<int, IntStrID>();

            try
            {
                FileStream fs = File.OpenRead(filename);
                StreamReader sr = new StreamReader(fs);

                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] pieces = line.Split(',');
                    int id = Convert.ToInt32(pieces[0]);
                    s_npcIcons[id] = new IntStrID(pieces[1], id);

                    line = sr.ReadLine();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to read " + filename + "\n" + ex.ToString());
            }
        }


        static void InitStaticText(string dataDir)
        {
            string filename = dataDir + "StaticDialog.txt";
            s_staticText = new Dictionary<int, IntStrID>();

            try
            {
                FileStream fs = File.OpenRead(filename);
                StreamReader sr = new StreamReader(fs);

                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] pieces = line.Split(':');
                    int id = Convert.ToInt32(pieces[0]);
                    s_staticText[id] = new IntStrID(pieces[1], id);

                    line = sr.ReadLine();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to read " + filename + "\n" + ex.ToString());
            }
        }

    }

    public class NPCNameID
    {
        public string NPCName;
        public ushort ID;
        
        public NPCNameID(string name, ushort id)
        {
            NPCName = name;
            ID = id;
        }

        public override string ToString()
        {
            return NPCName + ": " + ID.ToString();
        }
    }

    public class IntStrID
    {
        public string Str;
        public int ID;

        public IntStrID(string str, int id)
        {
            Str = str;
            ID = id;
        }

        public override string ToString()
        {
            return Str;
        }
    }
}

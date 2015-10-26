using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace ServerDataTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Database.Init("server=127.0.0.1;uid=DecoServer;pwd=dspass;database=deco;");
            FixMaps();
        }

        void FixMaps()
        {
            ushort[] maps = Database.FetchMaps();

            bool[] mapsSet = new bool[79];
            foreach( ushort map in maps )
                mapsSet[map] = true;

            for (int i = 2; i < mapsSet.Length; i++)
            {
                if( i == 31 || i == 32 || i == 54 || i == 55 || i == 64 || i == 65 )
                    continue;


                if (!mapsSet[i])
                {
                    Debug.WriteLine("Adding Missing map: " + i);
                    Database.AddMap((ushort)i);
                }
            }
        }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            AccountsDialog dlg = new AccountsDialog();
            dlg.ShowDialog();
        }

        private void btnNPCs_Click(object sender, EventArgs e)
        {
            NPCDialog dlg = new NPCDialog();
            dlg.ShowDialog();
        }

        private void btnQuests_Click(object sender, EventArgs e)
        {
            QuestDialog dlg = new QuestDialog();
            dlg.ShowDialog();
        }

        private void btnItems_Click(object sender, EventArgs e)
        {
            ItemsDialog dlg = new ItemsDialog();
            dlg.ShowDialog();
        }
    }
}

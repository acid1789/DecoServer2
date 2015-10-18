using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerDataTool
{
    static class Program
    {
        public static Dictionary<ushort, NPCNameID> s_npcNameIDs;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitNPCMap();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        static void InitNPCMap()
        {
            Dictionary<ushort, string> npcIDMap = new Dictionary<ushort, string>();

            npcIDMap[5000] = "Supreme Heart";
            npcIDMap[5001] = "Detruit Blodger";
            npcIDMap[5002] = "Flora Aramis";
            npcIDMap[5003] = "Arebeth Wallen";
            npcIDMap[5004] = "Auhigen Routs";
            npcIDMap[5005] = "An Old man";
            npcIDMap[5006] = "Louise Grace";
            npcIDMap[5007] = "Luel";
            npcIDMap[5008] = "Tiris Taemple";
            npcIDMap[5009] = "Nanaa Paras";
            npcIDMap[5010] = "Bart";
            npcIDMap[5011] = "Reishana Cube";
            npcIDMap[5012] = "Lainmint";
            npcIDMap[5013] = "Luimi";
            npcIDMap[5014] = "Ciel";
            npcIDMap[5015] = "magic researcher";
            npcIDMap[5016] = "Saiki Rugart";
            npcIDMap[5017] = "Nad Luas";
            npcIDMap[5018] = "Elpid Sheiat";
            npcIDMap[5019] = "Nanna Millenas";
            npcIDMap[5020] = "Keeper of Eingarden";
            npcIDMap[5021] = "Gatt Shake";
            npcIDMap[5022] = "Arahs Shake";
            npcIDMap[5023] = "Ka Lubleton";
            npcIDMap[5024] = "Gabriel Heart";
            npcIDMap[5025] = "Kurio Kahn";
            npcIDMap[5026] = "Illena";
            npcIDMap[5027] = "Illena";
            npcIDMap[5028] = "Lainmint";
            npcIDMap[5029] = "Supreme Heart(S)";
            npcIDMap[5030] = "Nad Luas(S)";
            npcIDMap[5031] = "Instructor";
            npcIDMap[5032] = "Instructor";
            npcIDMap[5033] = "Instructor";
            npcIDMap[5034] = "Instructor";
            npcIDMap[5035] = "Detruit Blodger";
            npcIDMap[5036] = "Auhigen Routs";
            npcIDMap[5037] = "Louise Grace";
            npcIDMap[5038] = "Nanaa Paras";
            npcIDMap[5039] = "Leggi";
            npcIDMap[5040] = "Milton";
            npcIDMap[5041] = "Agnon";
            npcIDMap[5042] = "Leaman";
            npcIDMap[5043] = "Brant";
            npcIDMap[5044] = "Geren";
            npcIDMap[5045] = "Koro";
            npcIDMap[5046] = "Pisaro";
            npcIDMap[5047] = "Gaybel";
            npcIDMap[5048] = "Amelli";
            npcIDMap[5049] = "Larke";
            npcIDMap[5050] = "Boha";
            npcIDMap[5051] = "An officer, Sore";
            npcIDMap[5052] = "Wilson";
            npcIDMap[5053] = "Roren Maltonis";
            npcIDMap[5054] = "Photeras";
            npcIDMap[5055] = "Jimur";
            npcIDMap[5056] = "Xloth";
            npcIDMap[5057] = "Codilia";
            npcIDMap[5058] = "Chapina";
            npcIDMap[5059] = "Snowman";
            npcIDMap[5061] = "Groondalpe";
            npcIDMap[5062] = "Raida";
            npcIDMap[5063] = "Wooslan";
            npcIDMap[5064] = "Buiske";
            npcIDMap[5065] = "Onylis";
            npcIDMap[5066] = "Millena patrol";
            npcIDMap[5067] = "Rain Prominas";
            npcIDMap[5068] = "Flower Fairy";
            npcIDMap[5069] = "Merchant on beach";
            npcIDMap[5070] = "Pet manager";
            npcIDMap[5071] = "Clerk of café";
            npcIDMap[5072] = "Clerk of café";
            npcIDMap[5073] = "Clerk of café";
            npcIDMap[5074] = "Kabayan";
            npcIDMap[5075] = "Elbonseutom";
            npcIDMap[5076] = "Eljaseura";
            npcIDMap[5077] = "Cateon Anos ";
            npcIDMap[5078] = "Sibanon Aran";
            npcIDMap[5079] = "Asron Ker";
            npcIDMap[5080] = "Aran Mogos";
            npcIDMap[5081] = "Kelen Talk";
            npcIDMap[5082] = "Flower Fairy";
            npcIDMap[5083] = "Event NPC M";
            npcIDMap[5084] = "Event NPC R";
            npcIDMap[5085] = "Arcbot";
            npcIDMap[5086] = "Arcbot";
            npcIDMap[5087] = "Mrs. Corn";
            npcIDMap[5090] = "Mission messenger";
            npcIDMap[5091] = "Mission messenger";
            npcIDMap[5092] = "Mission messenger";
            npcIDMap[5093] = "Mission messenger";
            npcIDMap[5094] = "Mission messenger";
            npcIDMap[5095] = "Mission messenger";
            npcIDMap[5096] = "Mission messenger M";
            npcIDMap[5097] = "Mission messenger R";
            npcIDMap[5098] = "Mission messenger M";
            npcIDMap[5099] = "Mission messenger R";
            npcIDMap[5101] = "Detruit Blodger";
            npcIDMap[5102] = "Flora Aramis";
            npcIDMap[5103] = "Arebeth Wallen";
            npcIDMap[5104] = "Auhigen Routs";
            npcIDMap[5201] = "Louise Grace";
            npcIDMap[5202] = "Luel";
            npcIDMap[5203] = "Tiris Taemple";
            npcIDMap[5204] = "Nanaa Paras";
            npcIDMap[5205] = "TentroBium";
            npcIDMap[5206] = "Lide";
            npcIDMap[5207] = "Rantana";
            npcIDMap[5208] = "Lodante";
            npcIDMap[5209] = "Ceigen";
            npcIDMap[5210] = "Logan";
            npcIDMap[5211] = "Mosien";
            npcIDMap[5212] = "Garan";
            npcIDMap[5213] = "Vesi";
            npcIDMap[5214] = "Coodalran";
            npcIDMap[5215] = "Dalin";
            npcIDMap[5216] = "Savie";
            npcIDMap[5217] = "Esien";
            npcIDMap[5218] = "Narien";
            npcIDMap[5300] = "Anate Rain Tower";
            npcIDMap[5301] = "A thousand year old tree";
            npcIDMap[5302] = "A fountain of fairy";
            npcIDMap[5303] = "Original stone of Shikrine";
            npcIDMap[5304] = "Grave of Miate Paras";
            npcIDMap[5305] = "Camelmo";
            npcIDMap[7079] = "Rain Suit (Ruin)";
            npcIDMap[7080] = "Rain Suit (Seal)";
            npcIDMap[7081] = "Millena Suit (Ruin)";
            npcIDMap[7082] = "Millena Suit (Seal)";
            npcIDMap[7083] = "Magician Garb (Ruin)";
            npcIDMap[7084] = "Shaman Garb (Ruin)";
            npcIDMap[7085] = "Priest Garb (Ruin)";
            npcIDMap[7086] = "Light Armor (Ruin)";
            npcIDMap[7087] = "Middle Armor (Ruin)";
            npcIDMap[7088] = "Heavy Armor (Ruin)";
            npcIDMap[7089] = "Magician Garb (Seal)";
            npcIDMap[7090] = "Shaman Garb (Seal)";
            npcIDMap[7091] = "Priest Garb (Seal)";
            npcIDMap[7092] = "Light Armor (Seal)";
            npcIDMap[7093] = "Middle Armor (Seal)";
            npcIDMap[7094] = "Heavy Armor (Seal)";
            npcIDMap[7095] = "Danaria";
            npcIDMap[7096] = "Terobe";
            npcIDMap[7097] = "Timbernoth";
            npcIDMap[7098] = "Flame R";
            npcIDMap[7099] = "Arado";
            npcIDMap[7100] = "Baros";
            npcIDMap[7101] = "Magnoth";
            npcIDMap[7102] = "Flame M";
            npcIDMap[6168] = "Camelmo";

            s_npcNameIDs = new Dictionary<ushort, NPCNameID>();
            foreach (KeyValuePair<ushort, string> kvp in npcIDMap)
            {
                s_npcNameIDs[kvp.Key] = new NPCNameID(kvp.Value, kvp.Key);
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
}

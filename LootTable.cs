using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoServer2
{
    public class LootTable
    {
        class LTEntry
        {
            public double Chance;
            public uint Template;

            public LTEntry(double chance, uint temp)
            {
                Chance = chance;
                Template = temp;
            }
        }

        List<LTEntry> _entries;

        public LootTable()
        {
            _entries = new List<LTEntry>();
        }

        public void AddEntry(double chance, uint template)
        {
            double lastChance = 0;
            if( _entries.Count > 0 )
                lastChance = _entries[_entries.Count - 1].Chance;
            _entries.Add(new LTEntry(lastChance + chance, template));
        }

        public uint? Generate()
        {
            double roll = Program.Server.Rand.NextDouble();

            foreach (LTEntry e in _entries)
            {
                if (roll < e.Chance)
                {
                    return e.Template;
                }
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoServer2.Quests
{
    public class QuestReward
    {
        ushort[] _items;
        uint _gold;
        uint _exp;

        private QuestReward(uint gold, uint exp)
        {
            _gold = gold;
            _exp = exp;
        }

        public class Builder
        {
            List<ushort> _items;
            QuestReward _qr;

            public Builder(uint gold, uint exp)
            {
                _qr = new QuestReward(gold, exp);
                _items = new List<ushort>();
            }

            public void AddItem(ushort item)
            {
                _items.Add(item);
            }

            public void AddGold(uint gold)
            {
                _qr._gold += gold;
            }

            public void AddExp(uint exp)
            {
                _qr._exp += exp;
            }

            public QuestReward Build()
            {
                _qr._items = _items.ToArray();
                return _qr;
            }
        }
    }
}

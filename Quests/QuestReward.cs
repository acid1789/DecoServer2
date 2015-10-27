using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuggleServerCore;
using DecoServer2.CharacterThings;

namespace DecoServer2.Quests
{
    public class QuestReward
    {
        uint[] _items;
        uint _gold;
        uint _exp;
        uint _fame;

        private QuestReward(uint gold, uint exp, uint fame)
        {
            _gold = gold;
            _exp = exp;
            _fame = fame;
        }

        public void Award(Connection client, uint questID)
        {
            Program.Server.TaskProcessor.AddTask(new Task(Task.TaskType.GiveGoldExpFame, client, new GiveGoldExpFameArgs(_gold, _exp, _fame, GiveGoldExpFameArgs.TheReason.Quest, questID)));

            foreach (uint id in _items)
            {
                Program.Server.TaskProcessor.AddTask(new Task(Task.TaskType.GiveItem, client, new GiveItemArgs(id, GiveItemArgs.TheReason.Quest, questID)));
            }
        }

        public class Builder
        {
            List<uint> _items;
            QuestReward _qr;

            public Builder(uint gold, uint exp, uint fame)
            {
                _qr = new QuestReward(gold, exp, fame);
                _items = new List<uint>();
            }

            public void AddItem(uint item)
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

            public void AddFame(uint fame)
            {
                _qr._fame += fame;
            }

            public QuestReward Build()
            {
                _qr._items = _items.ToArray();
                return _qr;
            }
        }
    }
}

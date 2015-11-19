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
        public enum RewardType
        {
            Gold,
            Exp,
            Fame,
            Item,
            Teleport,
            Skill
        }

        RewardType _type;
        uint _context;

        public QuestReward(RewardType type, uint context)
        {
            _type = type;
            _context = context;
        }

        public void Award(Connection client, uint questID)
        {
            switch (_type)
            {
                case RewardType.Gold:
                    Program.Server.TaskProcessor.AddTask(new Task(Task.TaskType.GiveGoldExpFame, client, new GiveGoldExpFameArgs(_context, 0, 0, GiveGoldExpFameArgs.TheReason.Quest, questID)));
                    break;
                case RewardType.Exp:
                    Program.Server.TaskProcessor.AddTask(new Task(Task.TaskType.GiveGoldExpFame, client, new GiveGoldExpFameArgs(0, _context, 0, GiveGoldExpFameArgs.TheReason.Quest, questID)));
                    break;
                case RewardType.Fame:
                    Program.Server.TaskProcessor.AddTask(new Task(Task.TaskType.GiveGoldExpFame, client, new GiveGoldExpFameArgs(0, 0, _context, GiveGoldExpFameArgs.TheReason.Quest, questID)));
                    break;
                case RewardType.Item:
                    Program.Server.TaskProcessor.AddTask(new Task(Task.TaskType.GiveItem, client, new GiveItemArgs(_context, GiveItemArgs.TheReason.Quest, questID)));
                    break;
                case RewardType.Teleport:
                    throw new NotImplementedException();
                case RewardType.Skill:
                    throw new NotImplementedException();
            }
        }
    }
}

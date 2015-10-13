using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoServer2.Quests
{
    public class QuestStep
    {
        public enum CompletionType
        {
            KillMonster,
            CollectItem,
            GoToLocation,
            TalkToNPC,
            ReachLevel
        }

        QuestReward[] _prewards;
        QuestReward[] _rewards;
        QuestLine[] _lines;

        uint _quest;
        byte _step;
        CompletionType _completionType;
        uint _completionCount;
        ushort _completionID;

        private QuestStep(uint quest, byte step, CompletionType type, uint completionCount, ushort completionID)
        {
            _quest = quest;
            _step = step;
            _completionType = type;
            _completionCount = completionCount;
            _completionID = completionID;
        }        

        #region Accessors
        public uint QuestID
        {
            get { return _quest; }
        }

        public byte StepNumber
        {
            get { return _step; }
        }
        #endregion

        public class Builder
        {
            List<QuestReward> _prewards;
            List<QuestReward> _rewards;
            Dictionary<int, QuestLine> _lines;
            byte _lastLine;
            QuestStep _step;

            public Builder(uint quest, byte step, CompletionType type, uint completionCount, ushort completionID = 0)
            {
                _step = new QuestStep(quest, step, type, completionCount, completionID);
                _lastLine = 0;
                _lines = new Dictionary<int, QuestLine>();
                _prewards = new List<QuestReward>();
                _rewards = new List<QuestReward>();
            }

            public void AddLine(QuestLine line)
            {
                _lines[line.LineNumber] = line;
                if (line.LineNumber > _lastLine)
                    _lastLine = line.LineNumber;
            }

            public void AddReward(QuestReward reward)
            {
                _rewards.Add(reward);
            }

            public void AddPreward(QuestReward reward)
            {
                _prewards.Add(reward);
            }

            public QuestStep Build()
            {
                _step._lines = new QuestLine[_lastLine + 1];
                for (int i = 0; i < _step._lines.Length; i++)
                {
                    _step._lines[i] = _lines[i];
                }
                
                _step._rewards = _rewards.ToArray();
                _step._prewards = _prewards.ToArray();
                return _step;
            }

        }
    }


    
}

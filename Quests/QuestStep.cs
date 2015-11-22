using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JuggleServerCore;

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
            ReachLevel,
            WearItem,
        }
        
        QuestReward[] _rewards;
        QuestLine[] _lines;

        uint _quest;
        uint _ownerNPC;
        byte _step;
        CompletionType _completionType;
        uint _completionCount;
        uint _completionID;

        private QuestStep(uint quest, byte step, CompletionType type, uint completionCount, uint completionID, uint owner)
        {
            _quest = quest;
            _step = step;
            _completionType = type;
            _completionCount = completionCount;
            _completionID = completionID;
            _ownerNPC = owner;
        }

        void FinishStep(Connection client)
        {
            // Award rewards
            foreach (QuestReward qr in _rewards)
            {
                qr.Award(client, _quest);
            }

            // Is the whole quest done?
            Quest q = Program.Server.GetQuest(_quest);
            q.NextStep(client, this);
        }

        public QuestLine GetLine(byte index)
        {
            return _lines[index];
        }

        public bool IsNPCRelevent(uint npcID)
        {
            if( _ownerNPC == npcID )
                return true;
            
            if( _completionType == CompletionType.TalkToNPC && _completionID == npcID )
                return true;

            return false;                            
        }

        public void Activate(Connection client)
        {
            switch (_completionType)
            {
                default:
                case CompletionType.KillMonster:
                case CompletionType.CollectItem:
                case CompletionType.GoToLocation:
                case CompletionType.ReachLevel:
                    throw new NotImplementedException();
                case CompletionType.WearItem:
                    if( !CheckItemEquipCompletion(client) )
                        client.OnItemEquipped += Client_OnItemEquipped;
                    break;
                case CompletionType.TalkToNPC:
                    client.OnQuestDialogFinished += Client_OnQuestDialogFinished;
                    break;
            }
        }

        bool CheckItemEquipCompletion(Connection client)
        {
            if (client.Character.HasItemEquipped(_completionID))
            {
                FinishStep(client);
                return true;
            }
            return false;
        }

        private void Client_OnItemEquipped(object sender, EventArgs e)
        {
            Connection client = (Connection)sender;
            if (CheckItemEquipCompletion(client))
            {
                // Done and moved on, remove this handler
                client.OnItemEquipped -= Client_OnItemEquipped;
            }
        }

        private void Client_OnQuestDialogFinished(object sender, QuestDialogFinishedArgs e)
        {
            Connection client = (Connection)sender;
            if (e.NPC == _completionID)
            {
                // We are all done talking to this npc. Remove the handler
                client.OnQuestDialogFinished -= Client_OnQuestDialogFinished;

                // Finish
                FinishStep(client);
            }
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

        public int LineCount
        {
            get { return _lines.Length; }
        }
        #endregion

        public class Builder
        {
            List<QuestReward> _rewards;
            Dictionary<int, QuestLine> _lines;
            byte _lastLine;
            QuestStep _step;

            public Builder(uint quest, byte step, uint owner, CompletionType type, uint completionCount, uint completionID = 0)
            {
                _step = new QuestStep(quest, step, type, completionCount, completionID, owner);
                _lastLine = 0;
                _lines = new Dictionary<int, QuestLine>();
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

            public QuestStep Build()
            {
                if (_lines.Count > 0)
                {
                    _step._lines = new QuestLine[_lastLine + 1];
                    for (int i = 0; i < _step._lines.Length; i++)
                    {
                        _step._lines[i] = _lines[i];
                    }
                }
                
                _step._rewards = _rewards.ToArray();
                return _step;
            }

        }
    }


    public class QuestDialogFinishedArgs : EventArgs
    {
        public uint NPC;

        public QuestDialogFinishedArgs(uint npc)
        {
            NPC = npc;
        }
    }    
}

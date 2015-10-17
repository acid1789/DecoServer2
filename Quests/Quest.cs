using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecoServer2.CharacterThings;
using JuggleServerCore;

namespace DecoServer2.Quests
{
    public class Quest
    {
        QuestStep[] _steps;
        QuestRequirement[] _requirements;
        uint _questID;

        private Quest(uint questID)
        {
            _questID = questID;
        }

        public void AddRequirement(QuestRequirement qr)
        {
            if (_requirements != null)
            {
                List<QuestRequirement> nqr = new List<QuestRequirement>(_requirements);
                nqr.Add(qr);
                _requirements = nqr.ToArray();
            }
            else
            {
                _requirements = new QuestRequirement[1];
                _requirements[0] = qr;
            }
        }

        public bool PlayerMeetsRequirements(CharacterInfo player)
        {
            if (_requirements != null)
            {
                foreach (QuestRequirement qr in _requirements)
                {
                    if (!qr.PlayerMeetsRequirement(player))
                        return false;
                }
            }

            return true;
        }

        public QuestStep GetStep(byte index)
        {
            return _steps[index];
        }

        public void NextStep(Connection client, QuestStep step)
        {
            byte stepIndex = step.StepNumber;
            stepIndex++;
            if (stepIndex < _steps.Length)
            {
                // Valid next step, activate it 
                client.Character.SetActiveQuestStep(_questID, stepIndex);
                _steps[stepIndex].Activate(client);               
            }
            else
            {
                // Done with this quest!
                client.Character.CompleteQuest(_questID);
            }
        }

        #region Accessors
        public uint QuestID
        {
            get { return _questID; }
        }
        #endregion

        public class Builder
        {
            Dictionary<byte, QuestStep> _steps;
            byte _lastStep;
            Quest _q;

            public Builder(uint questID)
            {
                _q = new Quest(questID);
                _steps = new Dictionary<byte, QuestStep>();
                _lastStep = 0;
            }

            public void AddStep(QuestStep step)
            {
                _steps[step.StepNumber] = step;
                if( step.StepNumber > _lastStep )
                    _lastStep = step.StepNumber;
            }

            public Quest Build()
            {
                _q._steps = new QuestStep[_lastStep + 1];
                for (int i = 0; i < _q._steps.Length; i++)
                {
                    _q._steps[i] = _steps[(byte)i];
                }
                return _q;
            }
        }
    }
}

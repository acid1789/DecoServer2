using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoServer2.Quests
{
    public class Quest
    {
        QuestStep[] _steps;
        uint _questID;

        private Quest(uint questID)
        {
            _questID = questID;
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

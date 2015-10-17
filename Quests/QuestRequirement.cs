using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecoServer2.CharacterThings;

namespace DecoServer2.Quests
{
    public class QuestRequirement
    {
        enum Type
        {
            Level,
            Race,
            Gender,
            Job,
            Fame,
            Item
        }
        
        Type _type;
        uint _param;

        public static QuestRequirement LoadFromDB(object[] row)
        {
            // 0: quest_id    int(10) unsigned
            // 1: type        tinyint(3) unsigned
            // 2: param   int(10) unsigned

            QuestRequirement qr = new QuestRequirement();
            qr._type = (Type)((byte)row[1]);
            qr._param = (uint)row[2];
            return qr;
        }

        public bool PlayerMeetsRequirement(CharacterInfo player)
        {
            switch (_type)
            {
                case Type.Level:
                    if( player.Level < _param )
                        return false;
                    break;
                case Type.Race:
                    if( _param != 0 && !player.Millena )
                        return false;
                    break;
                case Type.Gender:
                    if( _param != 0 && !player.Male )
                        return false;
                    break;
                case Type.Job:
                    if( player.Job != (byte)(_param) )
                        return false;
                    break;
                case Type.Fame:
                    if( player.Fame < _param )
                        return false;
                    break;
                case Type.Item:
                    if( !player.HasItem(_param) )
                        return false;
                    break;
            }
            return true;
        }
    }
}

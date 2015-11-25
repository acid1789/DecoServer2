using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JuggleServerCore;

namespace DecoServer2
{
    public class Location
    {
        uint _id;
        uint _x;
        uint _y;
        uint _radius;
        ushort _map;

        public static Location ReadFromDB(object[] row)
        {
            // 0: location_id int(10) unsigned
            // 1: name    varchar(30)
            // 2: x   int(10) unsigned
            // 3: y   int(10) unsigned
            // 4: radius  int(10) unsigned
            // 5: map smallint(5) unsigned

            Location loc = new Location();
            loc._id = (uint)row[0];
            loc._x = (uint)row[2];
            loc._y = (uint)row[3];
            loc._radius = (uint)row[4];
            loc._map = (ushort)row[5];

            return loc;
        }

        #region Accessors
        public uint ID
        {
            get { return _id; }
        }

        public ushort Map
        {
            get { return _map; }
        }

        public uint CellIndex
        {
            get { return Utils.EncodeCellIndex(_map, _x, _y); }
        }

        public uint Radius
        {
            get { return _radius; }
        }
        #endregion
    }
}

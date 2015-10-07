using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuggleServerCore
{
    public class Connection
    {
        public enum ConnStatus
        {
            New,
            Connecting,
            Connected,
            Disconnected,
            Closed
        }

        ConnStatus _conStatus;

        public Connection()
        {
            _conStatus = ConnStatus.New;
        }

        public void Update()
        {
        }



        #region Accessors
        public ConnStatus Status
        {
            get { return _conStatus; }
        }
        #endregion
    }
}

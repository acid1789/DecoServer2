using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DecoServer2
{
    public class AIThread
    {
        Thread _thread;

        Mutex _spawnerLock;
        Dictionary<uint, MonsterSpawner> _spawners;

        public AIThread()
        {
            _spawnerLock = new Mutex();
            _spawners = new Dictionary<uint, MonsterSpawner>();

            _thread = new Thread(new ThreadStart(AIThreadProc));
            _thread.Name = "AI Thread";
            _thread.Start();
        }

        public void AddSpawner(MonsterSpawner ms)
        {
            _spawnerLock.WaitOne();
            _spawners[ms.ID] = ms;
            _spawnerLock.ReleaseMutex();
        }

        void AIThreadProc()
        {
            DateTime frameTime = DateTime.Now;
            while (true)
            {
                double deltaSeconds = (DateTime.Now - frameTime).TotalSeconds;
                frameTime = DateTime.Now;

                // Check spawners
                _spawnerLock.WaitOne();
                MonsterSpawner[] spawners = _spawners.Values.ToArray();
                _spawnerLock.ReleaseMutex();
                foreach (MonsterSpawner ms in spawners)
                {
                    ms.Update(deltaSeconds);
                }

                // Thread
                Thread.Sleep(1);
            }
        }
    }
}

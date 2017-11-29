using System;

namespace NancySelfHosting.DB
{
    public class SingletonClientsInMemory
    {
        private static volatile ClientsInMemory instance;
        private static Object syncRootObject = new Object();

        public static ClientsInMemory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRootObject)
                    {
                        if (instance == null)
                        {
                            instance = new ClientsInMemory();
                        }
                    }
                }

                return instance;
            }
        }
    }
}

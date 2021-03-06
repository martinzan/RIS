﻿using DatabaseEntities;
using IDatabaseExecutor;

namespace DatabaseExecutor
{
    public class DBDataExecutor : IDBDatabaseExecutor
    {
        private readonly risTabulky aRisContext;

        public risTabulky risContext
        {
            get { return aRisContext; }
        }

        public DBDataExecutor()
        {
            aRisContext = new risTabulky();
        }
    }
}

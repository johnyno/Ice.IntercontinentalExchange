using System;
using System.Collections.Generic;
using System.Text;
using IntercontinentalExchange.Domain.Contracts;

namespace IntercontinentalExchange.Domain.Bases
{
    public abstract class HandlerBase
    {
        public IAppLogger Logger { get; }

        protected HandlerBase(IAppLogger logger)
        {
            Logger = logger;
        }
    }
}

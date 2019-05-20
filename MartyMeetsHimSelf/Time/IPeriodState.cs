using MartyMeetsHimSelf.BackToDomainInTheFuture;
using System;
using System.Collections.Generic;

namespace MartyMeetsHimSelf.Time
{
    public interface IPeriodState
    {
        IPeriodState Next(DateTime date, List<MartyVersion> martys);
    }
}

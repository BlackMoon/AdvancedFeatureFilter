using System;
using Library.Rules;

namespace Library.Strategy
{
    public interface IStrategy1<TFilter1>
    {
        Rule1Filters<TFilter1>? FindRule(TFilter1 val1);
    }

    public interface IStrategy2<TFilter1, TFilter2>
    {
        Rule2Filters<TFilter1, TFilter2>? FindRule(TFilter1 val1, TFilter2 val2);
    }

    public interface IStrategy3<TFilter1, TFilter2, TFilter3>
    {
        Rule3Filters<TFilter1, TFilter2, TFilter3>? FindRule(TFilter1 val1, TFilter2 val2, TFilter3 val3);
    }

    public interface IStrategy4<TFilter1, TFilter2, TFilter3, TFilter4>
    {
        Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4>? FindRule(TFilter1 val1, TFilter2 val2, TFilter3 val3, TFilter4 val4);
    }
}


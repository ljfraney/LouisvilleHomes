using System;

namespace TheVilleSkill.Utilities
{
    public interface ITimeService
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}
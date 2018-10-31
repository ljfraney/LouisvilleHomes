using System;

namespace TheVilleSkill.Utilities
{
    public class TimeService : ITimeService
    {
        public DateTime Today
        {
            get
            {
                return DateTime.Today;
            }
        }

        public DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}

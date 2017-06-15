﻿namespace BLL.Threats.External.Serious.Red
{
    public abstract class SeriousRedExternalThreat : SeriousExternalThreat
    {
        protected SeriousRedExternalThreat(int shields, int health, int speed)
            : base(ThreatDifficulty.Red, shields, health, speed)
        {
        }
    }
}

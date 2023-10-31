using System;
using System.Linq;
using BLL.Players;

namespace BLL.ShipComponents
{
    public class VisualConfirmationComponent : ICharlieComponent
    {
        public int NumberOfConfirmationsThisTurn { get; private set; }
        public int[] BestConfirmationPerPhase { get; } = new int[3];
        public int TotalVisualConfirmationPoints => BestConfirmationPerPhase.Sum();
        private int currentPhase = 0;

        public void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
        {
            NumberOfConfirmationsThisTurn += isAdvancedUsage ? 3 : 1;
            BestConfirmationPerPhase[currentPhase] = Math.Max(BestConfirmationPerPhase[currentPhase], GetVisualConfirmationPoints(NumberOfConfirmationsThisTurn));
        }

        public bool CanPerformCAction(Player performingPlayer)
        {
            return true;
        }

        public void PerformEndOfTurn()
        {
            NumberOfConfirmationsThisTurn = 0;
        }

        public void PerformEndOfPhase()
        {
            currentPhase++;
        }

        private static int GetVisualConfirmationPoints(int numberOfPlayers)
        {
            if (numberOfPlayers <= 3)
                return numberOfPlayers;
            return (numberOfPlayers - 3) * 2 + 3;
        }
    }
}

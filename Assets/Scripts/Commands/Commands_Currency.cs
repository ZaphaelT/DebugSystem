using UnityEngine;

namespace Commands
{
    public partial class CommandsManager : MonoBehaviour
    {
        [Command("SpawnNpc", "Spawn npc on commnad")]
        public void SpawnNPCCommand(int amount)
        {
            for (int i = 0; i < amount; i++)
                AgentSpawner.Instance.SpawnAgent();
        }

        [Command("AddMoney", "Add money on command")]
        public void AddMoneyCommand(int amount)
        {
            AddMoney.Instance.AddIncome(amount);
        }
    }
}

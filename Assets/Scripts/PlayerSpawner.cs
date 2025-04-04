using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] GameObject humanPlayerPrefab;
    [SerializeField] GameObject goblinPlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Debug.Log(SwitchCharacter.chosenPlayer);
            if (SwitchCharacter.chosenPlayer == PlayerType.Human)
            {
                var networkPlayer = Runner.Spawn(humanPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
                Runner.SetPlayerObject(player, networkPlayer);
            }
            else
            {
                var networkPlayer = Runner.Spawn(goblinPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
                Runner.SetPlayerObject(player, networkPlayer);
            }
        }
    }
}

public enum PlayerType
{
    Human,
    Goblin
}

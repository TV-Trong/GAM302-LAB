using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    [SerializeField] GameObject humanObject;
    [SerializeField] GameObject goblinObject;
    bool toggle = true;
    public static PlayerType chosenPlayer;

    private void Start()
    {
        humanObject.SetActive(toggle);
        goblinObject.SetActive(!toggle);
        chosenPlayer = toggle ? PlayerType.Human : PlayerType.Goblin;
    }

    public void OnSwitchButton()
    {
        toggle = !toggle;
        humanObject.SetActive(toggle);
        goblinObject.SetActive(!toggle);
        chosenPlayer = toggle ? PlayerType.Human : PlayerType.Goblin;
    }

    public void LoadCharacter(PlayerType _playerType)
    {
        chosenPlayer = _playerType;
        toggle = chosenPlayer == PlayerType.Human ? true : false;
    }
}

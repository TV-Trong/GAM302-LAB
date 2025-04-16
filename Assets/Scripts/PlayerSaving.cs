using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;

public class PlayerSaving : MonoBehaviour
{

    [SerializeField] private SwitchCharacter switchCharacter;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TextMeshProUGUI playerNamePlaceholder;
    [SerializeField] private MainMenu mainMenu;

    [SerializeField] private string savePath = "Assets/SaveFolder/Save.json";

    public int characterIndex = 0;

    private void Update()
    {
        if (SwitchCharacter.chosenPlayer == PlayerType.Human)
            characterIndex = 0;
        else characterIndex = 1;
        Debug.Log(characterIndex);
    }

    public void SaveData()
    {
        try
        {
            if (!Directory.Exists("Assets/SaveFolder"))
            {
                Directory.CreateDirectory("Assets/SaveFolder");
            }

            if (!File.Exists(savePath))
            {
                File.WriteAllText(savePath, "");
            }

            PlayerData data = new PlayerData();
            data.playerName = (String.IsNullOrWhiteSpace(playerNameInputField.text)) ? playerNamePlaceholder.text : playerNameInputField.text;
            data.character = characterIndex;
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(savePath, json);

            Debug.Log("Save Success");
        }
        catch
        {
            Debug.Log("Save Failed");
        }
    }

    public void LoadData()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                playerNameInputField.text = data.playerName;
                characterIndex = data.character;
                PlayerType type = characterIndex == 0 ? PlayerType.Human : PlayerType.Goblin;
                switchCharacter.LoadCharacter(type);
            }
        }
        catch
        {
            Debug.Log("Load Failed");
        }
    }

    public void QuickPlay()
    {
        if (String.IsNullOrEmpty(PlayerPrefs.GetString("LastRoom")))
            return;

        LoadData();

        mainMenu.StartGame(Fusion.GameMode.Shared, PlayerPrefs.GetString("LastRoom"), "PlayScene");
    }
}

[Serializable]
public class PlayerData
{
    public string playerName;
    public int character;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NameManager : MonoBehaviour
{
    public static NameManager nameInstance;
    
    [SerializeField] private InputField playerNameInput;
    [SerializeField] private Text bestScoreText;

    public string playerName = "";
    public string bestName;
    public int bestScore;
    private void Awake()
    {
        if (nameInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        nameInstance = this;
        DontDestroyOnLoad(gameObject);
        LoadName();
       
    }
    private void Start()
    {
        if (bestName != "")
        {
            bestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        }
    }
    public void SetBestScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            bestName = playerName;
            SaveName();
            MainManager.Instance.bestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        }
        Debug.Log("Score: " + score + "  Player: " + playerName);
    }

    public void StartNew()
    {
        if (playerNameInput.text != "")
        {
            playerName = playerNameInput.text;
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogWarning("Please enter a name!");
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int bestScore;
    }

    public void SaveName()
    {
        SaveData data = new SaveData();
        data.playerName = bestName;
        data.bestScore = bestScore;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadName()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestName = data.playerName;
            bestScore = data.bestScore;
        }
    }
}

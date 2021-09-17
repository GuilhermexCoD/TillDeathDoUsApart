using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInputMenu : MonoBehaviour
{

    private string levelName = "Dungeon";

    private LevelData data;

    [SerializeField]
    private InputFieldWithLabel seedInputField;
    private string seed;


    public void SetSeed(string seed)
    {
        this.seed = seed;
        ScenaryManager.current?.SetSeed(seed);
    }

    public void LoadDungeon()
    {
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
    }

    // Start is called before the first frame update
    void Awake()
    {
        data = ScriptableObject.CreateInstance<LevelData>();
        seedInputField.onValueChanged += OnSeedChanged;
    }

    private void OnSeedChanged(object sender, TextArgs e)
    {
        SetSeed(e.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

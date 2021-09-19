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

    [SerializeField]
    private DropdownWithLabel difficultyDropdown;

    // Start is called before the first frame update
    void Awake()
    {
        data = ScriptableObject.CreateInstance<LevelData>();

        seedInputField.onValueChanged += OnSeedChanged;

        difficultyDropdown.onValueChanged += OnDifficultyChanged;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    #region Difficulty

    private void SetDifficulty(EDifficulty difficulty)
    {
        data.difficulty = difficulty;
        ScenaryManager.current?.SetLevelData(data);
    }

    private void OnDifficultyChanged(object sender, EnumArgs e)
    {
        SetDifficulty((EDifficulty)e.value);
    }

    #endregion

    #region Seed

    private void SetSeed(string seed)
    {
        this.seed = seed;
        ScenaryManager.current?.SetSeed(seed);
    }

    private void OnSeedChanged(object sender, TextArgs e)
    {
        SetSeed(e.value);
    }

    #endregion

    public void LoadDungeon()
    {
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
    }
}

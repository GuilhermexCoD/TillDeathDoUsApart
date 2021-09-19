using System;
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

    [SerializeField]
    private DropdownWithLabel difficultyDropdown;

    [SerializeField]
    private InputFieldWithLabel maxRoomQuantityInputField;

    [SerializeField]
    private InputFieldWithLabel maxEnemyQuantityInputField;

    [SerializeField]
    private ToggleWithLabel useBspToggle;

    // Start is called before the first frame update
    void Awake()
    {
        data = ScriptableObject.CreateInstance<LevelData>();

        seedInputField.onValueChanged += OnSeedChanged;

        difficultyDropdown.onValueChanged += OnDifficultyChanged;

        maxRoomQuantityInputField.onValueChanged += OnMaxRoomChanged;

        maxEnemyQuantityInputField.onValueChanged += OnEnemyQuantityChanged;

        useBspToggle.onValueChanged += OnUseBspChanged;
    }

    #region Binary Space Partitioning

    private void SetUseBsp(bool value)
    {
        data.useBSP = value;
    }

    private void OnUseBspChanged(object sender, BoolArgs e)
    {
        SetUseBsp(e.value);
    }

    #endregion

    #region Enemy

    private void SetMaxEnemyQuantity(string value)
    {
        var success = int.TryParse(value, out int maxEnemyQuantity);

        if (success)
            data.maxQuantityOfEnemies = maxEnemyQuantity;
        else
            data.maxQuantityOfEnemies = 0;
    }

    private void OnEnemyQuantityChanged(object sender, TextArgs e)
    {
        SetMaxEnemyQuantity(e.value);
    }

    #endregion

    #region MaxRoomQuantity

    private void SetMaxRoomQuantity(string value)
    {
        var success = int.TryParse(value, out int maxRoomQuantity);

        if (success)
            data.maxQuantityOfRooms = maxRoomQuantity;
        else
            data.maxQuantityOfRooms = 0;
    }

    private void OnMaxRoomChanged(object sender, TextArgs e)
    {
        SetMaxRoomQuantity(e.value);
    }

    #endregion

    #region Difficulty

    private void SetDifficulty(EDifficulty difficulty)
    {
        data.difficulty = difficulty;
    }

    private void OnDifficultyChanged(object sender, EnumArgs e)
    {
        SetDifficulty((EDifficulty)e.value);
    }

    #endregion

    #region Seed

    private void SetSeed(string seed)
    {
        ScenaryManager.current?.SetSeed(seed);
    }

    private void OnSeedChanged(object sender, TextArgs e)
    {
        SetSeed(e.value);
    }

    #endregion

    public void LoadDungeon()
    {
        ScenaryManager.current?.SetLevelData(data);
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
    }
}

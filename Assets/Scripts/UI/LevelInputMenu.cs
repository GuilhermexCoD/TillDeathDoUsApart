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

    [SerializeField]
    private FloatInputsWithLabel roomSizesInputField;

    [SerializeField]
    private FloatInputsWithLabel mapSizeInputField;

    // Start is called before the first frame update
    void Awake()
    {
        data = ScriptableObject.CreateInstance<LevelData>();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        seedInputField.onValueChanged += OnSeedChanged;
        OnSeedChanged(seedInputField, new TextArgs() { value = seedInputField.GetValue() });

        difficultyDropdown.onValueChanged += OnDifficultyChanged;
        OnDifficultyChanged(difficultyDropdown, new EnumArgs() { value = difficultyDropdown.GetValue()});

        maxRoomQuantityInputField.onValueChanged += OnMaxRoomChanged;
        OnMaxRoomChanged(maxRoomQuantityInputField, new TextArgs() { value = maxRoomQuantityInputField.GetValue() });

        maxEnemyQuantityInputField.onValueChanged += OnEnemyQuantityChanged;
        OnEnemyQuantityChanged(maxEnemyQuantityInputField, new TextArgs() { value = maxEnemyQuantityInputField.GetValue() });

        useBspToggle.onValueChanged += OnUseBspChanged;
        OnUseBspChanged(useBspToggle, new BoolArgs() { value = useBspToggle.GetValue() });

        roomSizesInputField.onValueChanged += OnRoomSizeChanged;
        OnRoomSizeChanged(roomSizesInputField, new FloatArrayArgs() { values = roomSizesInputField.GetValues().ToArray() });

        mapSizeInputField.onValueChanged += OnMapSizeChanged;
        OnMapSizeChanged(mapSizeInputField, new FloatArrayArgs() { values = mapSizeInputField.GetValues().ToArray() });
    }

    #region Map Size

    private void OnMapSizeChanged(object sender, FloatArrayArgs e)
    {
        int width = (int)e.values[0];
        int height = (int)e.values[1];

        data.size = new Vector2Int(width, height);
    }

    #endregion

    #region Room Size

    private void OnRoomSizeChanged(object sender, FloatArrayArgs e)
    {
        int width = (int)e.values[0];
        int height = (int)e.values[1];

        data.roomMaxSize = new Vector2Int(width, height);
    }

    #endregion

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

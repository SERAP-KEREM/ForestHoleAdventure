using SerapKeremGameTools._Game._SaveLoadSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadManager : MonoBehaviour
{
    public SaveManager saveManager;
    public LoadManager loadManager;

    // UI Elements
    public TMP_InputField inputStringField;
    public TMP_InputField inputIntField;
    public TMP_InputField inputFloatField;
    public Toggle inputBoolField;

    public TextMeshProUGUI stringText;
    public TextMeshProUGUI intText;
    public TextMeshProUGUI floatText;
    public TextMeshProUGUI boolText;

    public Button saveButton;
    public Button loadButton;
    public Button clearButton;

    private string testStringKey = "TestStringKey";
    private string testIntKey = "TestIntKey";
    private string testFloatKey = "TestFloatKey";
    private string testBoolKey = "TestBoolKey";

    private void Awake()
    {
        // Button click listeners
        saveButton.onClick.AddListener(SaveData);
        loadButton.onClick.AddListener(LoadData);
        clearButton.onClick.AddListener(ClearData);

        // Initialize SaveManager and LoadManager
        saveManager = new SaveManager();
        loadManager = new LoadManager();
    }

    // Save the data from input fields
    private void SaveData()
    {
        // Save string data
        saveManager.SaveData_String(testStringKey, inputStringField.text);

        // Save int data (make sure to parse it safely)
        int intValue;
        if (int.TryParse(inputIntField.text, out intValue))
        {
            saveManager.SaveData_Int(testIntKey, intValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for int.");
        }

        // Save float data (parse safely)
        float floatValue;
        if (float.TryParse(inputFloatField.text, out floatValue))
        {
            saveManager.SaveData_Float(testFloatKey, floatValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for float.");
        }

        // Save bool data
        saveManager.SaveData_Bool(testBoolKey, inputBoolField.isOn);
    }

    // Load the data and display it in text fields
    private void LoadData()
    {
        string loadedString = loadManager.LoadData_String(testStringKey);
        stringText.text = "Loaded String: " + loadedString;

        int loadedInt = loadManager.LoadData_Int(testIntKey);
        intText.text = "Loaded Int: " + loadedInt.ToString();

        float loadedFloat = loadManager.LoadData_Float(testFloatKey);
        floatText.text = "Loaded Float: " + loadedFloat.ToString("F2"); // Float formatlama

        bool loadedBool = loadManager.LoadData_Bool(testBoolKey);
        boolText.text = "Loaded Bool: " + loadedBool.ToString();
    }

    // Clear all saved data
    private void ClearData()
    {
        saveManager.ClearData(testStringKey);
        saveManager.ClearData(testIntKey);
        saveManager.ClearData(testFloatKey);
        saveManager.ClearData(testBoolKey);

        // Clear UI texts after clearing the data
        stringText.text = "Loaded String: ";
        intText.text = "Loaded Int: ";
        floatText.text = "Loaded Float: ";
        boolText.text = "Loaded Bool: ";
    }
}

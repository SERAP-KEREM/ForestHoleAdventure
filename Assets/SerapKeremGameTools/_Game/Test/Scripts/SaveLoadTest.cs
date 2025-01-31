using SerapKeremGameTools._Game._SaveLoadSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadManager : MonoBehaviour
{

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

        
    }

    // Save the data from input fields
    private void SaveData()
    {
        // Save string data
        SaveManager.SaveData(testStringKey, inputStringField.text);

        // Save int data (make sure to parse it safely)
        int intValue;
        if (int.TryParse(inputIntField.text, out intValue))
        {
            SaveManager.SaveData(testIntKey, intValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for int.");
        }

        // Save float data (parse safely)
        float floatValue;
        if (float.TryParse(inputFloatField.text, out floatValue))
        {
            SaveManager.SaveData(testFloatKey, floatValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for float.");
        }

        // Save bool data
        SaveManager.SaveData(testBoolKey, inputBoolField.isOn);
    }

    // Load the data and display it in text fields
    private void LoadData()
    {
        string loadedString =LoadManager.LoadData(testStringKey,"");
        stringText.text = "Loaded String: " + loadedString;

        int loadedInt = LoadManager.LoadData(testIntKey,0);
        intText.text = "Loaded Int: " + loadedInt.ToString();

        float loadedFloat = LoadManager.LoadData(testFloatKey,0.0f);
        floatText.text = "Loaded Float: " + loadedFloat.ToString("F2"); // Float formatlama

        bool loadedBool = LoadManager.LoadData(testBoolKey,true);
        boolText.text = "Loaded Bool: " + loadedBool.ToString();
    }

    // Clear all saved data
    private void ClearData()
    {
        SaveManager.ClearData(testStringKey);
        SaveManager.ClearData(testIntKey);
        SaveManager.ClearData(testFloatKey);
        SaveManager.ClearData(testBoolKey);

        // Clear UI texts after clearing the data
        stringText.text = "Loaded String: ";
        intText.text = "Loaded Int: ";
        floatText.text = "Loaded Float: ";
        boolText.text = "Loaded Bool: ";
    }
}

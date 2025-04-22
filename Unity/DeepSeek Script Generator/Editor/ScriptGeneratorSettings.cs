using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace ScriptGenerator {
// Specifies the file path where the ScriptableSingleton asset is stored within the project folder.
// FilePathAttribute ensures the settings are saved as an asset at "UserSettings/DeepSeekScriptGeneratorSettings.asset".
[FilePath("UserSettings/DeepSeekScriptGeneratorSettings.asset", FilePathAttribute.Location.ProjectFolder)]
public sealed class ScriptGeneratorSettings : ScriptableSingleton<ScriptGeneratorSettings> {

    // URL for the DeepSeek API endpoint. Default value is provided for convenience.
    // Why: Allows the user to connect to DeepSeek's chat completions API without needing to manually enter it initially.
    public string Url = "https://api.deepseek.com/v1/chat/completions";
    
    // API key for authentication with the DeepSeek API
    public string apiKey;

    // Model to be used for the API requests
    public string model = "gpt-3.5-turbo";

    // Default path where generated scripts will be stored
    public string path = "Assets/Scripts/";

    // Flag to determine if a timeout should be used for API requests
    public bool useTimeout = true;

    // Timeout duration in seconds for API requests
    public int timeout = 45;

    // Saves the settings when called
    public void Save() => Save(true);

    // Ensures settings are saved when the scriptable object is disabled
    void OnDisable() => Save();
   
    }

sealed class ScriptGeneratorSettingsProvider : SettingsProvider {
    // Constructor that sets the settings path in Unity's Project Settings UI
    private ScriptGeneratorSettingsProvider() : base("Project/DeepSeek Script Generator", SettingsScope.Project) { }
    // Stores the logo image for the settings UI
    private Texture2D _logo;

    // Override of the OnGUI method to define the custom settings UI.    
    public override void OnGUI(string search) {
            // Load the logo from the specified path
            //_logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/DeepSeek Script Generator/Editor/Logo.png");
            //GUILayout.Label(_logo, GUILayout.Width(780), GUILayout.Height(165));
            var settings = ScriptGeneratorSettings.instance;
        using var rsa = new RSACryptoServiceProvider();

        // Get the user's Unity Cloud account email
        var userEmail = CloudProjectSettings.userName;
        if (string.IsNullOrEmpty(userEmail)) {
            EditorGUILayout.HelpBox("You must be signed in to Unity to use this feature.", MessageType.Error);
            return;
        }

        // Load settings values
        var url = settings.Url;
        var key = string.IsNullOrEmpty(settings.apiKey) ? "" : Encryption.Decrypt(settings.apiKey, userEmail);
        var model = settings.model;
        var useTimeout = settings.useTimeout;
        var timeout = settings.timeout;
        var path = settings.path;

        // Display instructions for obtaining an API key   
        EditorGUILayout.HelpBox("Get an API key from the DeepSeek website:\n" + //
                            "    1. Sign up or log in to your DeepSeek account using the button below.\n" + // 
                            "    2. Navigate to the 'View API Keys' section in your account dashboard.\n" + //
                            "    3. Click 'Create new secret key' and copy the generated key.", MessageType.Info);

        // Button to open DeepSeek's website
        if (GUILayout.Button("DeepSeek Website", GUILayout.ExpandHeight(false))) {
            Application.OpenURL("https://platform.deepseek.com/");
        }

        EditorGUILayout.Space(20);

      
        EditorGUI.BeginChangeCheck();
      
        // Input fields for API settings
        url = EditorGUILayout.TextField("URL", url);
        key = EditorGUILayout.PasswordField("API Key", key);

        // Warning about storing the API key in a local file
        EditorGUILayout.HelpBox("The API key is stored in the following file: " + //
                                "UserSettings/DeepSeekScriptGeneratorSettings.asset. \n" + //
                                "When sharing your project with others, be sure to exclude the 'UserSettings' " + //
                                "directory to prevent unauthorized use of your API key.", MessageType.Warning);

        EditorGUILayout.Space(20);

        // Input field for selecting model and button to view available models
        EditorGUILayout.BeginHorizontal();
        model = EditorGUILayout.TextField("Model", model);
        if (GUILayout.Button("DeepSeek Models")) {
            Application.OpenURL("https://api-docs.deepseek.com/zh-cn/quick_start/pricing");
        }

        EditorGUILayout.EndHorizontal();

        // Input field for script output path with validation
        path = EditorGUILayout.TextField("Scripts output path", path);
        if (!path.EndsWith("/")) path += "/";
        if (!path.StartsWith("Assets/")) path = "Assets/" + path;

        EditorGUILayout.Space(20);

        // Toggle for enabling/disabling timeout
        useTimeout = EditorGUILayout.Toggle("Timeout", useTimeout);

        if (useTimeout) {
            EditorGUI.indentLevel++;
            timeout = EditorGUILayout.IntField("Duration (seconds)", timeout);
            if (timeout < 1) timeout = 1;
            EditorGUI.indentLevel--;
        }

        // Save settings if any changes were made
        if (EditorGUI.EndChangeCheck()) {
            settings.apiKey = Encryption.Encrypt(key, userEmail);
            settings.model = model;
            settings.useTimeout = useTimeout;
            settings.timeout = timeout;
            settings.path = path;
            settings.Url = url;
            settings.Save();
        }
    }

    // Registers this settings provider in Unity's Project Settings UI
    [SettingsProvider]
    public static SettingsProvider CreateCustomSettingsProvider() => new ScriptGeneratorSettingsProvider();
}
} // namespace AICommand
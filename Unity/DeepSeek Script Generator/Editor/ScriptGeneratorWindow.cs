using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace ScriptGenerator {
public sealed class ScriptGeneratorWindow : EditorWindow {
    // Constant error message displayed when the API key is not set.
    private const string ApiKeyErrorText = "API Key hasn't been set. Please check the project settings " +
                                           "(Edit > Project Settings > DeepSeek Script Generator > API Key).";
    // Private fields to store user input and settings for the script generation process.
    private string _prompt = "";
    private string _className = "";
    private bool _forgetHistory;
    private bool _deletePrevious;

    // Property to check if the API key is valid (not null or empty).
    private bool IsApiKeyOk => !string.IsNullOrEmpty(ScriptGeneratorSettings.instance.apiKey);

    // Wraps the user’s prompt with standard instructions for the DeepSeek API to generate a Unity script.
    private static string WrapPrompt(string input) =>
        "Write a Unity script.\n" + " - It is a C# MonoBehaviour.\n " + // Ensures the script is a Unity MonoBehaviour.
        " - Include all the necessary imports.\n" + // Requests required using statements.
        " - Add a RequireComponent attribute to any components used.\n" + // Ensures dependencies are enforced.
        " - Generate tooltips for all properties.\n" + // Adds tooltips for better usability in Unity Inspector.
        " - All properties should have default values.\n" + // Ensures properties are initialized.
        " - I only need the script body. Don’t add any explanation.\n" + // Requests only the code, no comments.
        " - Do not start and end the result with ```csharp and ```.\n" + // Avoids markdown formatting.
        "The task is described as follows:\n" + input; // Appends the user’s specific prompt.

    // Initiates the script generation process by submitting the prompt to the DeepSeek API.
    void RunGenerator(bool forgetHistory = false) {
        // Builds the full prompt, optionally instructing the API to forget prior context.
        var prompt = forgetHistory ? "Forget everything I said before.\n" : string.Empty;
        prompt += WrapPrompt(_prompt); // Combines standard instructions with user input.
        var code = DeepSeekAPI.Submit(prompt); // Sends the prompt to the API and gets the generated code.
        CreateScriptAsset(code); // Creates a script asset from the returned code.
    }

    // Creates a new script asset in the Unity project using the generated code.
    void CreateScriptAsset(string code) {
        // Uses reflection to access a non-public static method in Unity’s ProjectWindowUtil for creating script assets.
        var flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
        var method = typeof(ProjectWindowUtil).GetMethod("CreateScriptAssetWithContent", flags);
        if (method == null) {
            Debug.LogError("Failed to find CreateScriptAssetWithContent method.");
            return;
        }

        // Extract the class name from the code. It is the last word before the colon.
        var firstColonIndex = code.IndexOf(':');
        if(firstColonIndex == -1) return;
        var everythingBeforeColon = code.Substring(0, firstColonIndex).TrimEnd(' ');
        var lastSpaceIndex = everythingBeforeColon.LastIndexOf(' ');
        if (lastSpaceIndex == -1) return;
        // Takes the last non-empty word as the class name.
        var theName = everythingBeforeColon.Substring(lastSpaceIndex + 1);
        _className = theName;
        // Checks for existing scripts with the same name and appends a number if necessary to avoid conflicts.
        var i = 1;
        while (Resources.FindObjectsOfTypeAll<MonoScript>().Any(s => s.name == _className)) {
            _className = theName + i++; // e.g., "MyScript" becomes "MyScript1", "MyScript2", etc.
        }
        Debug.Log($"[DeepSeek Script Generator] Created class {_className}");
        // Saves the final class name to EditorPrefs for persistence across sessions.
        EditorPrefs.SetString("AiEngineerClassName", _className);

        var settings = ScriptGeneratorSettings.instance;
        if (!AssetDatabase.IsValidFolder(settings.path)) {
            // Remove "Assets/" from the path.
            var folder = settings.path.Replace("Assets/", "").TrimEnd('/');
            AssetDatabase.CreateFolder("Assets", folder);
        }

        // Constructs the full file path and invokes the script creation method.
        var path = $"{settings.path}/{_className}.cs";
        Debug.Log(path);
        Debug.Log(code);

        // Creates the script asset at the specified path.
        method?.Invoke(null, new object[] { path, code });
    }

    // Defines the GUI layout and behavior for the editor window.
    void OnGUI() {
        if (IsApiKeyOk) {
            // Read the last prompt from the EditorPrefs.
            if (string.IsNullOrEmpty(_prompt)) {
                _prompt = EditorPrefs.GetString("AiEngineerPrompt", "");
            }

            // Displays a resizable text area for the user to enter the script prompt.
            _prompt = EditorGUILayout.TextArea(_prompt, GUILayout.ExpandHeight(true));

            // Toggle to forget previous context, useful for starting fresh.
            _forgetHistory = EditorGUILayout.Toggle("Forget prior commands", _forgetHistory);

            // Checks if the previously generated script is attached to the selected GameObject and offers to overwrite it.
            var className = EditorPrefs.GetString("AiEngineerClassName", "");
            _deletePrevious =
                !(string.IsNullOrEmpty(className) || !Selection.activeGameObject ||
                  !Selection.activeGameObject.GetComponent(className)) &&
                EditorGUILayout.Toggle($"Overwrite {className}", _deletePrevious);

            // Button to trigger script generation and optionally add it to the selected GameObject.
            if (GUILayout.Button("Generate and Add")) {
                // Saves the current prompt.
                EditorPrefs.SetString("AiEngineerPrompt", _prompt);

                // Deletes the previous script and component if the overwrite option is selected.
                if (_deletePrevious) {
                    var gameObject = Selection.activeGameObject;
                    if (gameObject != null) {
                        var component = gameObject.GetComponent(className);
                        if (component != null) {
                            // Removes the component from the GameObject.
                            DestroyImmediate(component);
                        }
                    }

                    var settings = ScriptGeneratorSettings.instance;
                    var file = AssetDatabase.LoadAssetAtPath<MonoScript>($"{settings.path}{className}.cs");
                    if (file != null) {
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(file)); // Deletes the old script file.
                    }
                }

                // Generates the new script.
                RunGenerator(_forgetHistory);
            }
        } else {
            // Displays an error message if the API key is missing.
            EditorGUILayout.HelpBox(ApiKeyErrorText, MessageType.Error);
        }
    }

    // Registers an event handler when the window is enabled to handle assembly reloads.
    void OnEnable() => AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;

    // Unregisters the event handler when the window is disabled to prevent memory leaks.
    void OnDisable() => AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;

    // Handles post-assembly reload to automatically add the generated script to the selected GameObject.
    void OnAfterAssemblyReload() {
        // Finds the generated script by its class name
        var script = Resources.FindObjectsOfTypeAll<MonoScript>().FirstOrDefault(s => s.name == _className);
        if (script != null) {
            var gameObject = ScriptGeneratorButton.SelectedGameObject;
            if (gameObject == null) {
                gameObject = Selection.activeGameObject;
            }

            if (gameObject != null) {
                gameObject.AddComponent(script.GetClass()); // Attaches the script as a component.
            } else {
                Debug.LogError("[DeepSeek Script Generator] Target GameObject not found.");
            }
        } else {
            Debug.LogError("[DeepSeek Script Generator] Script not found.");
        }

        Close();
    }
}
}
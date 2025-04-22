using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptGenerator {
[InitializeOnLoad]
public class ScriptGeneratorButton : Editor {
    private const double UpdateInterval = 0.1f;

    public static GameObject SelectedGameObject;
    private static double _lastUpdate;
    private static Button _button;

    static ScriptGeneratorButton() {
        // Subscribes the EditorUpdate method to Unity's EditorApplication.update event, called every Editor frame
        EditorApplication.update += EditorUpdate;

        // Subscribes an anonymous lambda function to the selectionChanged event, triggered when Editor selection changes
        // 'Selection.selectionChanged': Delegate callback triggered when currently active/selected item has changed.
        Selection.selectionChanged += () => {
            // Checks if _button (a presumed UI element) is null; exits early if it is, preventing null reference errors
            if (_button == null) return;
            
            // Sets _button's visibility: true if a GameObject is selected, false if not
            _button.visible = Selection.activeGameObject != null;
        };
    }

    // Method runs at intervals while Unity's Editor is running
    private static void EditorUpdate() {
        // Prevents running this method too frequently (only runs if 0.1 seconds have passed)
        if (EditorApplication.timeSinceStartup - _lastUpdate < UpdateInterval) return;
        // Updates the last update time
        _lastUpdate = EditorApplication.timeSinceStartup;

         // If the button has already been created, exit early to avoid duplicates
        if (_button != null) return;

        // Finds all open Editor windows in Unity
        var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var window in windows) {
            // Looks for the Inspector window specifically
            if (window.GetType().Name != "InspectorWindow") continue;

            // Searches for the "Add Component" button inside the Inspector
            var buttons = window.rootVisualElement.Q(className: "unity-inspector-add-component-button");
            
            // If the button isn't found, move to the next window
            if (buttons == null) continue;

            // Creates a new container for our custom button
            var container = new VisualElement {
                style = { flexDirection = FlexDirection.Row, justifyContent = Justify.Center, marginTop = -8 }
            };

            // Adds the container to the Inspector window next to the "Add Component" button
            buttons.parent.Add(container);

            // Creates a new button with the label "Generate Component"
            var button = new Button(ClickEvent) {
                text = "Generate Component", style = { width = 230, height = 25, marginLeft = 2, marginRight = 2 }
            };

            // Adds the button to the container
            container.Add(button);
            // Stores the button reference in the static variable
            _button = button;
            // Shows or hides the button based on whether a GameObject is selected
            _button.visible = Selection.activeGameObject != null;
            // Removes the EditorUpdate method from the update loop since the button is now created
            EditorApplication.update -= EditorUpdate;
        }
    }

    // Method executed when the button is clicked
    private static void ClickEvent() {
        // Stores the currently selected GameObject
        var selection = Selection.activeGameObject;
        SelectedGameObject = selection;

        // Opens a custom Editor window called "ScriptGeneratorWindow"
        var window = EditorWindow.GetWindow<ScriptGeneratorWindow>(true, "Generate Component");
        // Displays the window
        window.Show();

        // Get the main Unity Editor window's position and size
        var mainWindow = EditorGUIUtility.GetMainWindowPosition();

        // Calculate center position
        float centerX = mainWindow.x + (mainWindow.width - 500) / 2; // 500 is the estimated width of the new window
        float centerY = mainWindow.y + (mainWindow.height - 120) / 2; // 120 is the window height

        // Set the position of the window to be centered
        window.position = new Rect(centerX, centerY, 500, 120);
    }
}
}
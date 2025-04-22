using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

namespace ScriptGenerator {
static class DeepSeekAPI {
    
// Disables warning CS0649: "Field is never assigned to, and will always have its default value null".
// This is used because the structs are populated via JSON deserialization, not direct assignment.
#pragma warning disable 0649
    // Struct representing the request payload sent to the DeepSeek API.
    [Serializable]
    public struct Request {
        public string model;
        public RequestMessage[] messages;
    }

    // Struct representing a single message in the request
    [Serializable]
    public struct RequestMessage {
        // Role of the message sender (e.g., "user").
        public string role;
        // The content of the message (e.g., the user’s prompt).
        public string content;
    }

    // Struct representing the response payload received from the DeepSeek API.
    [Serializable]
    public struct Response {
        public string id;
        public ResponseChoice[] choices;
    }

    [Serializable]
    public struct ResponseChoice {
        public int index;
        public ResponseMessage message;
    }

    [Serializable]
    public struct ResponseMessage {
        public string role;
        public string content;
    }
#pragma warning restore 0649

    // Submits a prompt to the DeepSeek API and returns the generated response content.
    public static string Submit(string prompt) {
        // Retrieves the singleton instance of settings for API configuration.
        var settings = ScriptGeneratorSettings.instance;
        // Creates a request object with the specified model and a single user message containing the prompt.
        var request = new Request {
            model = settings.model, messages = new[] { new RequestMessage() { role = "user", content = prompt } }
        };
        
        // Serializes the request struct to JSON format for the API request.
        var requestJson = JsonUtility.ToJson(request);

// Configures the UnityWebRequest based on Unity version for compatibility.
#if UNITY_2022_2_OR_NEWER
        // Uses the newer, simpler Post method signature in Unity 2022.2+.
        using var post = UnityWebRequest.Post(settings.Url, requestJson, "application/json");
#else
        // Uses the older, manual configuration for Unity versions before 2022.2.
        using var post = new UnityWebRequest(settings.Url, "POST");
        post.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(requestJson));
        post.downloadHandler = new DownloadHandlerBuffer();
        post.SetRequestHeader("Content-Type", "application/json");
#endif

        post.timeout = settings.timeout;

        var userEmail = CloudProjectSettings.userName;
        if (string.IsNullOrEmpty(userEmail)) {
            var m = "[DeepSeek Script Generator] You must be signed in to Unity to use this feature.";
            throw new Exception(m);
        }

        var key = Encryption.Decrypt(settings.apiKey, userEmail);
        post.SetRequestHeader("Authorization", "Bearer " + key);

        var webRequest = post.SendWebRequest();

        var time = Time.realtimeSinceStartup;
        var cancel = false;
        var progress = 0f;

        while (!webRequest.isDone && !cancel) {
            cancel = EditorUtility.DisplayCancelableProgressBar("DeepSeek Script Generator", "Generating script...",
                                                                progress += 0.01f);
            System.Threading.Thread.Sleep(100);

            var timeout = settings.useTimeout ? settings.timeout + 1f : float.PositiveInfinity;
            if (Time.realtimeSinceStartup - time > timeout) {
                EditorUtility.ClearProgressBar();
                throw new TimeoutException("[DeepSeek Script Generator] Request timed out");
            }
        }

        EditorUtility.ClearProgressBar();

        var responseJson = post.downloadHandler.text;
        if (!string.IsNullOrEmpty(post.error)) {
            throw new Exception($"[DeepSeek Script Generator] {post.error}");
        }

        if (string.IsNullOrEmpty(responseJson)) {
            throw new Exception("[DeepSeek Script Generator] No response received");
        }

        var data = JsonUtility.FromJson<Response>(responseJson);
        if (data.choices == null || data.choices.Length == 0) {
            throw new Exception("[DeepSeek Script Generator] No choices received");
        }

        return data.choices[0].message.content;
    }
}
}
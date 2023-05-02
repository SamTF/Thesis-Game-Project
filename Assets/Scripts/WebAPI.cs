using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// https://weeklyhow.com/unity-restful-api/
// https://stackoverflow.com/questions/65991562/how-to-upload-an-image-to-server-by-using-unitywebrequest
// https://forum.unity.com/threads/upload-picture-to-rest-api-in-unity.515796/

/// <summary>
/// Class to interact with my Web API to fetch and post data to the leaderboard!
/// </summary>
public class WebAPI : MonoBehaviour
{
    [SerializeField]
    private Texture2D fetchedTexture = null;
    [SerializeField]
    private Texture2D imageToPost = null;

    private const string URL = "http://localhost:5173/api/";

    private void Start() {
        StartCoroutine(GetRequest("pb"));
        StartCoroutine(GetImage("get-image"));
        StartCoroutine(PostRequest("pb", new { username = "harry", score = 19}, imageToPost));
    }

    
    /// <summary>
    /// Fetchs JSON data from an HTTP GET Request
    /// </summary>
    /// <param name="uri">API Route</param>
    private IEnumerator GetRequest(string uri) {
        using (UnityWebRequest request = UnityWebRequest.Get(URL + uri)) {
            yield return request.SendWebRequest();

            // Error handling
            if (request.result == UnityWebRequest.Result.ConnectionError) {
                Debug.LogError(request.error);
                yield return null;
            }

            // Successful request
            string response = request.downloadHandler.text;
            Debug.Log($"[WEB API] >>> GET request response: \n{response}");

            // Parse the JSON response
            SimpleJSON.JSONNode items = SimpleJSON.JSON.Parse(response);

            // Debug
            Debug.Log(items.Count);
            Debug.Log(items);
        }
    }

    /// <summary>
    /// Gets an image from an HTTP Request and saves it to a Texture2D object
    /// </summary>
    /// <param name="uri">API Route</param>
    private IEnumerator GetImage(string uri) {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(URL + uri)) {
            yield return request.SendWebRequest();

            // Error handling
            if (request.result == UnityWebRequest.Result.ConnectionError) {
                Debug.LogError(request.error);
                yield return null;
            }

            // Successful request
            string response = request.downloadHandler.text;
            Debug.Log($"[WEB API] >>> GET request response: \n{response}");

            // Read the texture from the response
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            texture.filterMode = FilterMode.Point;

            // Set it to the global variable for debugging
            fetchedTexture = texture;
        }
    }


    /// <summary>
    /// Sends a POST request using Form Data
    /// </summary>
    /// <param name="uri">API Route</param>
    /// <param name="body">Anonymous object whose fields and values will be added to the Form Data</param>
    private IEnumerator PostRequest(string uri, System.Object body, Texture2D imageToUpload = null) {
        // create a web form
        WWWForm form = new WWWForm();

        // Loops thru all the properties in the anonymous object and gets their value
        foreach (System.Reflection.PropertyInfo prop in body.GetType().GetProperties())
        {
            var value = prop.GetValue(body, null);
            form.AddField(prop.Name, value.ToString());

            Debug.Log($"{prop.Name} : {value} : {value.GetType()}");
        }

        // Uploading an image with the Form Data by converting it to a byte array
        if (imageToUpload) {
            byte[] image = imageToPost.EncodeToPNG();
            form.AddBinaryData("character", image);
        }

        form.AddField("super_secret", "sawmill");

        // send the post request
        UnityWebRequest request = UnityWebRequest.Post(URL + uri, form);

        yield return request.SendWebRequest();

        // error handling
        if (request.result != UnityWebRequest.Result.Success) {
            Debug.LogError(request.error);
            yield return null;
        }

        // successful request
        Debug.Log("POST REQUEST SENT!");
        Debug.Log(request.downloadHandler.text);
        request.Dispose();
    }


    // https://forum.unity.com/threads/posting-json-through-unitywebrequest.476254/
    IEnumerator Post(string url, string bodyJsonString)
    {
        Debug.Log(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UCamera = UnityEngine.Camera;

/// <summary>
/// Single entry-point script to build and run the main menu at runtime (camera + background + Entrer button + Quit optional).
/// Attach to an empty GameObject in your menu scene. Requires a sprite named LaunchMenuBackground in Resources.
/// </summary>
[DisallowMultipleComponent]
public class MainMenuUI : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string gameplaySceneName = "JeuCollecte";

    [Header("Background")]
    [SerializeField] private string backgroundResourceName = "LaunchMenuBackground";
    [SerializeField] private Color fallbackBackgroundColor = new Color(0.05f, 0.05f, 0.07f, 1f);

    [Header("Button")]
    [SerializeField] private string buttonLabel = "Entrer";
    [SerializeField] private Vector2 buttonSize = new Vector2(320f, 96f);
    [SerializeField] private Vector2 buttonPosition = new Vector2(-300f, 0f); // place over the banner
    [SerializeField] private Color buttonColor = new Color(0.18f, 0.43f, 0.78f, 0.92f);
    [SerializeField] private bool showQuitButton = true;

    private UCamera menuCamera;

    private void Awake()
    {
        EnsureCamera();
        var canvas = BuildCanvas();
        var bg = BuildBackground(canvas);
        BuildButton(bg.transform, buttonLabel, buttonSize, buttonPosition, buttonColor, OnStartClicked, "StartButton");
        if (showQuitButton)
        {
            BuildButton(bg.transform, "Quitter", new Vector2(240f, 72f), buttonPosition + new Vector2(0f, -120f), new Color(0.3f, 0.1f, 0.1f, 0.9f), OnQuitClicked, "QuitButton");
        }
    }

    private void EnsureCamera()
    {
        menuCamera = UCamera.main;
        if (menuCamera == null)
        {
            var camGO = new GameObject("Main Camera");
            menuCamera = camGO.AddComponent<UCamera>();
            camGO.tag = "MainCamera";
            camGO.transform.position = new Vector3(0f, 0f, -10f);
            camGO.transform.rotation = Quaternion.identity;
            camGO.AddComponent<AudioListener>();
        }
    }

    private Canvas BuildCanvas()
    {
        var canvasGO = new GameObject("MenuCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = menuCamera;
        canvas.planeDistance = 1f;

        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;
        return canvas;
    }

    private Image BuildBackground(Canvas canvas)
    {
        var bgGO = new GameObject("Background", typeof(Image));
        bgGO.transform.SetParent(canvas.transform, false);
        var img = bgGO.GetComponent<Image>();

        var sprite = Resources.Load<Sprite>(backgroundResourceName);
        if (sprite != null)
        {
            img.sprite = sprite;
            img.preserveAspect = true;
            img.color = Color.white;
        }
        else
        {
            img.color = fallbackBackgroundColor;
            Debug.LogWarning($"MainMenuUI: Sprite '{backgroundResourceName}' not found in Resources.");
        }

        var rect = img.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return img;
    }

    private void BuildButton(Transform parent, string label, Vector2 size, Vector2 pos, Color color, UnityEngine.Events.UnityAction onClick, string name)
    {
        var btnGO = new GameObject(name, typeof(Button), typeof(Image));
        btnGO.transform.SetParent(parent, false);

        var img = btnGO.GetComponent<Image>();
        img.color = color;

        var rect = btnGO.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        rect.anchoredPosition = pos;
        rect.anchorMin = new Vector2(0.25f, 0.5f);
        rect.anchorMax = new Vector2(0.25f, 0.5f);

        var btn = btnGO.GetComponent<Button>();
        btn.onClick.AddListener(onClick);

        var txtGO = new GameObject("Text", typeof(Text));
        txtGO.transform.SetParent(btnGO.transform, false);
        var txt = txtGO.GetComponent<Text>();
        txt.text = label;
        txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        txt.fontSize = 28;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;

        var txtRect = txtGO.GetComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.offsetMin = Vector2.zero;
        txtRect.offsetMax = Vector2.zero;
    }

    private void OnStartClicked()
    {
        if (!string.IsNullOrEmpty(gameplaySceneName))
        {
            SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("MainMenuUI: gameplaySceneName not set.");
        }
    }

    private void OnQuitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UCamera = UnityEngine.Camera;

/// <summary>
/// Programmatically builds a simple launch menu: background image, camera, and a centered button.
/// Attach this script to an empty GameObject in your launch scene.
/// </summary>
[DisallowMultipleComponent]
public class LaunchSceneBuilder : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private string backgroundSpriteName = "LaunchMenuBackground"; // Resources/<name>
    [SerializeField] private string buttonLabel = "Entrer";
    [SerializeField] private string gameplaySceneName = "JeuCollecte";

    [Header("Layout")]
    [SerializeField] private Vector2 buttonSize = new Vector2(320f, 96f);
    [SerializeField] private Vector2 buttonOffset = new Vector2(-300f, 0f); // position relative to center (banner side)

    private UCamera menuCamera;

    private void Awake()
    {
        EnsureCamera();
        EnsureEventSystem();
        var canvas = BuildCanvas();
        var background = BuildBackground(canvas);
        BuildButton(background.transform, buttonLabel, buttonSize, buttonOffset);
    }

    private void EnsureEventSystem()
    {
        // Required for UI clicks to be detected
        if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() != null)
            return;

        var go = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem));

#if ENABLE_INPUT_SYSTEM
        go.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
        go.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
#endif
    }

    private void EnsureCamera()
    {
        menuCamera = UCamera.main;
        if (menuCamera == null)
        {
            GameObject camGO = new GameObject("Main Camera");
            menuCamera = camGO.AddComponent<UCamera>();
            camGO.tag = "MainCamera";
            camGO.transform.position = new Vector3(0f, 0f, -10f);
            camGO.transform.rotation = Quaternion.identity;
            camGO.AddComponent<AudioListener>();
        }
    }

    private Canvas BuildCanvas()
    {
        GameObject canvasGO = new GameObject("MenuCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
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
        GameObject bgGO = new GameObject("Background", typeof(Image));
        bgGO.transform.SetParent(canvas.transform, false);
        var img = bgGO.GetComponent<Image>();
        Sprite sprite = Resources.Load<Sprite>(backgroundSpriteName);
        if (sprite != null)
        {
            img.sprite = sprite;
            img.preserveAspect = true;
            img.color = Color.white;
        }
        else
        {
            img.color = new Color(0.05f, 0.05f, 0.07f, 1f); // fallback dark tint
            Debug.LogWarning($"LaunchSceneBuilder: Sprite '{backgroundSpriteName}' not found in Resources.");
        }

        var rect = img.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return img;
    }

    private void BuildButton(Transform parent, string label, Vector2 size, Vector2 offset)
    {
        GameObject btnGO = new GameObject("StartButton", typeof(Button), typeof(Image));
        btnGO.transform.SetParent(parent, false);

        var img = btnGO.GetComponent<Image>();
        img.color = new Color(0.18f, 0.43f, 0.78f, 0.92f);

        var rect = btnGO.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        rect.anchoredPosition = offset;
        rect.anchorMin = new Vector2(0.25f, 0.5f);
        rect.anchorMax = new Vector2(0.25f, 0.5f);

        var btn = btnGO.GetComponent<Button>();
        btn.onClick.AddListener(OnStartClicked);

        GameObject txtGO = new GameObject("Text", typeof(Text));
        txtGO.transform.SetParent(btnGO.transform, false);
        var txt = txtGO.GetComponent<Text>();
        txt.text = label;
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf")
              ?? Font.CreateDynamicFontFromOSFont("Arial", 28);
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
            Debug.LogError("LaunchSceneBuilder: gameplaySceneName not set.");
        }
    }
}

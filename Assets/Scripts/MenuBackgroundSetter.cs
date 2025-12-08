using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Assigns a background image to a UI Image from Resources at startup.
/// Drop this on a GameObject in the launch menu scene and assign a target Image.
/// Place your background sprite at Resources/LaunchMenuBackground (Sprite).
/// </summary>
[DisallowMultipleComponent]
public class MenuBackgroundSetter : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private string resourceSpriteName = "LaunchMenuBackground";

    private void Awake()
    {
        if (targetImage == null)
        {
            Debug.LogWarning("MenuBackgroundSetter: targetImage is not assigned.");
            return;
        }

        Sprite sprite = Resources.Load<Sprite>(resourceSpriteName);
        if (sprite == null)
        {
            Debug.LogWarning($"MenuBackgroundSetter: Sprite '{resourceSpriteName}' not found in Resources.");
            return;
        }

        targetImage.sprite = sprite;
        targetImage.preserveAspect = true;
        targetImage.color = Color.white;
    }
}

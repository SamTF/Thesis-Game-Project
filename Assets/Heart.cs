using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Heart Icon UI element that represents the Player's Health on-screen.
/// </summary>
public class Heart : MonoBehaviour
{
    private enum ImageType {
        Sprite, UIImage
    }

    [Header("UI Heart Icon")]
    [SerializeField]
    private HeartFill fill = HeartFill.Full;
    [SerializeField]
    private ImageType imageType = ImageType.UIImage;

    private HeartType _status;

    // Components
    private SpriteRenderer spriteRenderer = null;
    private Image uiImage = null;

    /// <summary>
    /// The status of this heart as a HeartType.
    /// </summary>
    /// <value>HeartType Object including fill value and sprite image.</value>
    public HeartType status {
        get { return _status; }
        set {
            _status = value;
            SetSprite(value.sprite);
            fill = value.value;
        }
    }

    // Awakeee
    private void Awake() {
        if (imageType == ImageType.Sprite) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        else if (imageType == ImageType.UIImage) {
            uiImage = gameObject.GetComponent<Image>();
        }
    }

    /// <summary>
    /// Changes the sprite of this object to a new one.
    /// </summary>
    /// <param name="heartSprite">New sprite.</param>
    private void SetSprite(Sprite heartSprite) {
        if (imageType == ImageType.Sprite) {
            spriteRenderer.sprite = heartSprite;
        }
        else if (imageType == ImageType.UIImage) {
            uiImage.overrideSprite = heartSprite;
        }
    }
}

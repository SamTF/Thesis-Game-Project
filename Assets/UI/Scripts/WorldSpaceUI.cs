using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This is a Helper class that allows a UI Builder element to follow a Transform in World Space!
/// </summary>
public class WorldSpaceUI : MonoBehaviour
{
    [Header("World Space UI Builder Tool")]
    [SerializeField][Tooltip("The Transform to follow")]
    private Transform target = null;
    [SerializeField][Tooltip("The ID of the UI Container to move in World Space")]
    private string containerID = "MainContainer";
    [SerializeField]
    private Anchor anchorX = Anchor.Center;
    [SerializeField]
    private Anchor anchorY = Anchor.Center;

    private VisualElement mainContainer = null;
    private Camera mainCamera = null;

    private enum Anchor {
        Start, Center, End
    }


    private void Start() {
        // Components
        mainCamera = Camera.main;

        // Get UI Elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        mainContainer = root.Q<VisualElement>(containerID);

        // Set inital position
        SetPosition();
    }

    private void SetPosition() {
        Vector2 newPosition = RuntimePanelUtils.CameraTransformWorldToPanel(
            mainContainer.panel,
            target.position,
            mainCamera
        );

        newPosition = SetAnchor(newPosition);

        mainContainer.transform.position = newPosition;
    }

    /// <summary>
    /// Offset the Transform position according to the desired anchor point for the UI container.
    /// </summary>
    /// <param name="position">Target position in screen space</param>
    /// <returns>Offset position</returns>
    private Vector2 SetAnchor(Vector2 position) {
        // X-Offset
        switch (anchorX)
        {
            case Anchor.Start:
                break;
            case Anchor.Center:
                position.x = position.x - ( mainContainer.layout.width / 2);
                break;
            case Anchor.End:
                position.x = position.x - mainContainer.layout.width;
                break;
            default:
                break;
        }

        // Y-Offset
        switch (anchorY)
        {
            case Anchor.Start:
                break;
            case Anchor.Center:
                position.y = position.y - ( mainContainer.layout.height / 2);
                break;
            case Anchor.End:
                position.y = position.y - mainContainer.layout.height;
                break;
            default:
                break;
        }

        // Return adjusted position
        return position;
    }



    // Update UI Position
    private void LateUpdate() {
        SetPosition();
    }
}

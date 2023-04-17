using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heart Icon UI element that represents the Player's Health on-screen. Unity UI Builder Edition!
/// </summary>
public class HeartUI : MyImage
{
    // Vars
    private HeartFill fill = HeartFill.Full;
    private HeartType _status;

    /// <summary>
    /// Create a Heart UI Icon object.
    /// </summary>
    /// <param name="heartType">The Heart Type to set it as. (Default = Full)</param>
    public HeartUI(HeartType? heartType) {
        if (heartType != null)
            status = heartType;
        else
            status = HeartsUI.Full;
    }


    /////// Public Getters & Setters

    /// <summary>
    /// The status of this heart as a HeartType. Setting it a value will change its sprite.
    /// </summary>
    /// <value>HeartType Object including fill value and sprite image.</value>
    public HeartType status {
        get { return _status; }
        set {
            _status = value;
            this.sprite = value.sprite;
            fill = value.value;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface that all player input controller should implement
/// </summary>
public interface IPlayerInputCtrl
{
    /// <summary>
    /// Vertical axis input. [-1, 1]
    /// </summary>
    float VerticalInput { get; }

    /// <summary>
    /// Horizontal axis input. [-1, 1]
    /// </summary>
    float HorizontalInput { get; }

    /// <summary>
    /// Return true when player hold shot button
    /// </summary>
    bool ShotButton { get; }

    /// <summary>
    /// Return true when player release shot button 
    /// </summary>
    bool ShotButtonUp { get; }

    /// <summary>
    /// Return true when player press shot button
    /// </summary>
    bool ShotButtonDown { get; }

    /// <summary>
    /// Return true when player hold change state button
    /// </summary>
    bool ChangeStateButton { get; }

    /// <summary>
    /// Return true when player release change state button
    /// </summary>
    bool ChangeStateButtonUp { get; }

    /// <summary>
    /// Return true when player press change state button
    /// </summary>
    bool ChangeStateButtonDown { get; }

    /// <summary>
    /// Return true when player hold max blance button
    /// </summary>
    bool MaxBlanceButton { get; }

    /// <summary>
    /// Return true when player release max blance button
    /// </summary>
    bool MaxBlanceButtonUp { get; }

    /// <summary>
    /// Return true when player press max blance button
    /// </summary>
    bool MaxBlanceButtonDown { get; }

    /// <summary>
    /// Called each frame
    /// </summary>
    void Update ();
}
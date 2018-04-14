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
    float VerticalInput ();

    /// <summary>
    /// Horizontal axis input. [-1, 1]
    /// </summary>
    float HorizontalInput ();

    /// <summary>
    /// Return true when player hold shot button
    /// </summary>
    bool ShotButton ();

    /// <summary>
    /// Return true when player release shot button 
    /// </summary>
    bool ShotButtonUp ();

    /// <summary>
    /// Return true when player press shot button
    /// </summary>
    bool ShotButtonDown ();

    /// <summary>
    /// Return true when player hold change state button
    /// </summary>
    bool ChangeStateButton ();

    /// <summary>
    /// Return true when player release change state button
    /// </summary>
    bool ChangeStateButtonUp ();

    /// <summary>
    /// Return true when player press change state button
    /// </summary>
    bool ChangeStateButtonDown ();

    /// <summary>
    /// Return true when player hold max blance button
    /// </summary>
    bool MaxBlanceButton ();

    /// <summary>
    /// Return true when player release max blance button
    /// </summary>
    bool MaxBlanceButtonUp ();

    /// <summary>
    /// Return true when player press max blance button
    /// </summary>
    bool MaxBlanceButtonDown ();

    /// <summary>
    /// Called each frame
    /// </summary>
    void Update ();
}
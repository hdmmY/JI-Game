using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (CircleCollider2D))]
public class DevSetPlayerSprite : MonoBehaviour
{
	private PlayerProperty _player;

	private MaterialPropertyBlock _matPropertyBlock;

	private static readonly Color PlayerBlackColor = new Color (0, 0, 0, 0);

	private static readonly Color PlayerWhiteColor = new Color (1, 1, 1, 1);

	private static readonly Color PlayerErrorStateColor =
		new Color (170f / 225f, 92f / 225f, 55f / 225f);

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	private void Start ()
	{
		_player = GetComponentInParent<PlayerProperty> ();

		_matPropertyBlock = new MaterialPropertyBlock ();
		_player.m_spriteReference.GetPropertyBlock (_matPropertyBlock);
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	private void Update ()
	{
		if (_matPropertyBlock == null || _player == null)
		{
			Start ();
		}

		switch (_player.m_playerState)
		{
			case JIState.Black:
				_matPropertyBlock.SetColor ("_Color", PlayerBlackColor);
				break;
			case JIState.White:
				_matPropertyBlock.SetColor ("_Color", PlayerWhiteColor);
				break;
			default:
				_matPropertyBlock.SetColor ("_Color", PlayerErrorStateColor);
				break;
		}

		_player.m_spriteReference.SetPropertyBlock (_matPropertyBlock);
	}
}
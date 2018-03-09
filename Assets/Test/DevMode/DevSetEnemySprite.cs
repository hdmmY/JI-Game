using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (EnemyProperty))]
public class DevSetEnemySprite : MonoBehaviour
{
	public bool m_takingDamage;

	private EnemyProperty _enemy;

	private MaterialPropertyBlock _matPropertyBlock;

	private static readonly Color EnemyBlackColor =
		new Color (75f / 255f, 78f / 255f, 42f / 255f);

	private static readonly Color EnemyWhiteColor =
		new Color (252f / 255f, 250f / 255f, 242f / 255f);

	private static readonly Color EnemyErrorColor =
		new Color (170f / 225f, 92f / 225f, 55f / 225f);

	private static readonly Color EnemyTakeDamageColor =
		new Color (208f / 225f, 16f / 225f, 76f / 225f);

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	private void Start ()
	{
		_enemy = GetComponent<EnemyProperty> ();

		_matPropertyBlock = new MaterialPropertyBlock ();
		_enemy.m_enemySprite.GetPropertyBlock (_matPropertyBlock);
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	private void Update ()
	{
		if (_matPropertyBlock == null || _enemy == null)
		{
			Start ();
		}

		if (m_takingDamage)
		{
			_matPropertyBlock.SetColor ("_Color", EnemyTakeDamageColor);
		}
		else
		{
			switch (_enemy.m_enemyState)
			{
				case JIState.Black:
					_matPropertyBlock.SetColor ("_Color", EnemyBlackColor);
					break;
				case JIState.White:
					_matPropertyBlock.SetColor ("_Color", EnemyWhiteColor);
					break;
				default:
					_matPropertyBlock.SetColor ("_Color", EnemyErrorColor);
					break;
			}
		}

		_enemy.m_enemySprite.SetPropertyBlock (_matPropertyBlock);
	}
}
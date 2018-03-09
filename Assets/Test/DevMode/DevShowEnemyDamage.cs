using UnityEngine;

[RequireComponent (typeof (CircleCollider2D))]
public class DevShowEnemyDamage : MonoBehaviour
{
	public GameObject m_damageCirclePrefab;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	private void Awake ()
	{
		if (transform.Find ("DamagePointCricle") == null)
		{
			var circleCol = GetComponent<CircleCollider2D> ();

			var damageChild = Instantiate (m_damageCirclePrefab,
				transform.position + (Vector3) circleCol.offset + Vector3.back,
				Quaternion.identity, transform);

			damageChild.name = "DamagePointCircle";
			damageChild.transform.localScale = circleCol.radius * 2 * Vector3.one;
		}
	}
}
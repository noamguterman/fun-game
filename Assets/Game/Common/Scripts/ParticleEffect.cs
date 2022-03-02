using System.Collections;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
	private ParticleSystem parSys;
    [SerializeField]
    private float deactiveTime = 0;

    private void OnEnable()
    {
		if (parSys == null)
		{
			parSys = GetComponent<ParticleSystem>();
		}
		StartCoroutine(Exploring());
	}

	private IEnumerator Exploring()
	{
        parSys.Play();
		yield return new WaitForSeconds(parSys.main.startLifetimeMultiplier);
        yield return new WaitForSeconds(deactiveTime);

        base.gameObject.SetActive(false);
	}
}

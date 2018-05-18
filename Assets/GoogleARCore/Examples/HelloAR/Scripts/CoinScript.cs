using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(DelayedKinematic());
	}

	private IEnumerator DelayedKinematic()
	{
		bool isMoving = true;
		while (isMoving)
		{
			Vector3 posBefore = gameObject.transform.position;
			yield return new WaitForSecondsRealtime(2);
			Vector3 posAfter = gameObject.transform.position;
			
			if (Vector3.Distance(posBefore, posAfter) < 0.01f)
			{
				isMoving = false;
			}
		}
		
		Destroy(gameObject.GetComponent<Rigidbody>());
	}
}

using UnityEngine;
using System.Collections;

public class CoinDestroyer : MonoBehaviour {

	private float			timer;
	// Use this for initialization
	void Start () {
		timer = 0;
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > 10)
			Destroy (this.gameObject);
	}
}

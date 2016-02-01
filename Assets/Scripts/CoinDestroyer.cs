using UnityEngine;
using System.Collections;

public class CoinDestroyer : MonoBehaviour {

	private GameObject		end;
    private GameObject      gameManager;	
    private double          value;
	
	void Start () {
        gameManager = GameObject.Find("GameManager");
		end = GameObject.Find ("Currency");
	}

	void Update () {
		this.gameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.MoveTowards (this.gameObject.GetComponent<RectTransform>().anchoredPosition3D,
		                                                                                        end.gameObject.GetComponent<RectTransform>().anchoredPosition3D,
							                                                                    Time.deltaTime * 100);
		this.gameObject.GetComponent<RectTransform> ().localScale = this.gameObject.GetComponent<RectTransform> ().localScale * 0.995f;
		if (this.gameObject.GetComponent<RectTransform> ().anchoredPosition3D == end.gameObject.GetComponent<RectTransform> ().anchoredPosition3D) {
            gameManager.GetComponent<GameManager>().gold += value;
			Destroy (this.gameObject);
		}
	}

	public void SetValue(double newvalue){
		value = newvalue;
	}
}
	
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void LoadGame(){
		SceneManager.LoadScene(1);
	}
}

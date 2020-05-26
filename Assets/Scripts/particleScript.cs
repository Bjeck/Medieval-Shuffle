using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class particleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ParticleSystem ps = GetComponent<ParticleSystem> ();
		ps.GetComponent<Renderer>().sortingLayerName = "Particles";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

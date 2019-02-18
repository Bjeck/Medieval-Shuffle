using UnityEngine;
using System.Collections.Generic;
using System;

public class DelegateTesting : MonoBehaviour {



	public delegate void meth (string s);





	string STRING;

	// Use this for initialization
	void Start () {

		meth m = new meth(FunctionInCard);
		meth m2 = new meth(OtherFunctionInCard);

		//IN CARD
		Action<string> onPlayed;

		//

	//	m("hello");
	//	print(STRING);
	//	m2("no");
	//	print(STRING);


		//IN INITIALIZATION
		//card.onPlayed
		onPlayed = s => { string ss = s+" that is "+s; FunctionInCard(ss); print(ss); };
		onPlayed("");
		//print(STRING);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void FunctionInCard(string s){
		STRING = s;
	}

	public void OtherFunctionInCard(string s){
		STRING += " "+s+" "+s;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseControl : MonoBehaviour {

	public GameManager gm;

	public GameObject tooltipObj;
	public Text tooltipText;
	public Image tooltipImg;
	public Sprite defSprite;

	public bool isShowingToolTip = false;
	public GameObject curTip;

	public float tooltipOffset = 50f;

	public Text log;
	public Scrollbar scroll;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void DisplayCardToolTip(Card c){
//		print("display");
		tooltipText.text = (c.nam + "\n\n\nAuthority: "+c.fear+"\nWealth: "+c.wealth+"\nStatus: "+c.Status()+"\nUnruliness: "+c.unruliness) + "" +
			"\n\n"+c.functionLong+"\n\n"+c.desc;
		if(c == gm.playur.playerCard){
			if(c.winAbove){
				tooltipText.text += "\n\nWIN CONDITION:\n"+c.winCondition+" "+c.WinVarAsString()+" or above.";
			}
			else {
				tooltipText.text += "\n\nWIN CONDITION:\nMaximum "+c.winCondition+" "+c.WinVarAsString();
			}
		}
		curTip = c.gameObject;
		tooltipImg.sprite = c.iconImg.sprite;
		isShowingToolTip = true;
		StartCoroutine(mouseHover());
	}



	public void DisplaySlotToolTip(Slot c){
		//		print("display");
		string temp =  (c.slotName+"\n\nProsperity: "+c.energy+" \n\nChange in Wealth: "+c.changeMoney+"\nChange in Fear: "+c.changeAuthority+"\nChange in Defenses: "+c.changeDefense);

		if(c.isBroken){
			temp += "\n\n<color=#ff0000ff>Broken.</color> Wait "+(c.turnsToBeBroken+1)+" days to use this Slot again.";
		}

		if(c.weatherActive){
			temp += "\n\nRain present. Cards here will have no effect this turn. Prosperity +2 next turn.";
		}
			
		if(c.OnNext != null){
			temp += "\n\nStatus effect present.\n"+c.onNextDesc;
		}

		if(c.cardOnMe != null){
			temp += "\n\nCard here: "+c.cardOnMe.nam;
		}
		tooltipText.text = temp;
		curTip = c.gameObject;
		tooltipImg.sprite = c.icon.sprite;
		isShowingToolTip = true;
		StartCoroutine(mouseHover());
	}

	public void DisplayRawText(string s){
		tooltipText.text = "\n\n\n"+s;
		isShowingToolTip = true;

	}
	public void DisplayRawTextCompanion(GameObject g){
		curTip = g;
		StartCoroutine(mouseHover());
	}


	public void RemoveToolTip(){
//		print("remove");
		tooltipText.text = "";
		tooltipImg.sprite = defSprite;
		curTip = null;
		isShowingToolTip = false;
	}



	public IEnumerator mouseHover(){
		bool isGood = false;
		while(isShowingToolTip){
			isGood = false;
			
			PointerEventData pointer = new PointerEventData(EventSystem.current);
			pointer.position = Input.mousePosition;

			List<RaycastResult> ress = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointer,ress);
			foreach(RaycastResult r in ress){
				if(r.gameObject == curTip){
					isGood = true;
				}
			}

			if(Input.GetMouseButton(0)){
				isGood = false;
			}

			if(!isGood){
				RemoveToolTip();
			}

			yield return new WaitForEndOfFrame();
		}


	}



	public void AddMessageToLog(string s){
		log.text += "\n"+s;
		StartCoroutine(waitToScroll());
	}

	IEnumerator waitToScroll(){
		yield return new WaitForSeconds(0.1f);
		scroll.value = 0;
	}


}

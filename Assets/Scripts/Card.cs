using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum WinVariable {Wealth, Fear, Status, Prosperity, Defenses, DeadKing}

public class Card : MonoBehaviour {

	public GameManager gm;
	public Vector3 PosInBar;
	public LayerMask layer;
	public Image thisImg;
	public Image iconImg;
	public Image youPip;
	public Text wealthText, fearText, unruliText, statusText;
	public bool isInHand = false;
	public bool isPlaced = false;
	public bool isDead = false;
	public Animator anim;
	public GameObject cardBack;

	public string id;
	public string nam;
	public string desc;
	public string function, functionLong;

	public int fear = 0;
	public int wealth = 0;
	public int unruliness = 0;

	public Action<Slot, Card> onPlayed; 
	public Action<Slot, Card, int> onPlayedValueChange; 
	public Action<Card, int, int, int> onNextTurn;

	bool isLerping = false;
	Vector3 endOfLerp;

	public WinVariable winvar;
	public int winCondition;
	public bool won = false;
	public bool winAbove = true;

	Slot slotForDrag;

	// Use this for initialization
	void Start () {
		PosInBar = transform.position;
		SetCardText();
		iconImg.sprite = gm.cm.GetSprite(this);

	}


	public void StartDrag(){
		gm.sound.PlayAudio(gm.sound.cardPickup);
	}


	public void OnDrag(){
		if(isLerping){
			return;
		}
		if(isPlaced || !isInHand){
			return;
		}
		transform.position = Input.mousePosition;

		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = Input.mousePosition;

		List<RaycastResult> ress = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointer,ress);

		///keep slot, if slot no longer there, change color and remove

		foreach(RaycastResult r in ress){
			if(r.gameObject.CompareTag("Slot")){
				Slot s = r.gameObject.GetComponent<Slot>();
				if(s == slotForDrag){
					return;	
				}
				if(s.cardOnMe == null && !r.gameObject.GetComponent<Slot>().isBroken){
					if(s.cardPlacedHereLast != this){
						if(!(s.changeMoney < 0 && wealth <= 0)){
							if(s.frame.color != s.cardColor){
								if(slotForDrag != null && !slotForDrag.isBroken){
									slotForDrag.ChangeColor(slotForDrag.defColor);
								}
								slotForDrag = s;
								s.ChangeColor(s.cardColor);
								return;
							}
						}
					}
				}
			}

		}
		if(slotForDrag != null && !slotForDrag.isBroken){
			slotForDrag.ChangeColor(slotForDrag.defColor);
			slotForDrag = null;
		}

	}

	public void SolvePosition(){
		if(isPlaced || !isInHand){
			return;
		}
		//print("SOLVING");

		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = Input.mousePosition;

		List<RaycastResult> ress = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointer,ress);

		//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y),Vector2.zero,5);
		//print(ress.Count);
		foreach(RaycastResult r in ress){
			if(gm.inDuel){
				if(r.gameObject.CompareTag("DuelSpot")){
					if(gm.cm.CheckDuelPossibility(gm.playur.playerCard,this)){
						print("ready to duel! "+gm.cm.CheckDuelPossibility(gm.playur.playerCard,this));
						gm.ChooseTargetForDuel(this);
						return;
					}
				}
			}
			else{
				if(r.gameObject.CompareTag("Slot")){
					Slot s = r.gameObject.GetComponent<Slot>();
					if(s.cardOnMe == null && !r.gameObject.GetComponent<Slot>().isBroken){
						if(s.cardPlacedHereLast != this){
							if(!(s.changeMoney < 0 && wealth <= 0)){
								PlaceCardOnSlot(r.gameObject.GetComponent<Slot>());
								return;
							}
							else{
								gm.AddMessageToLog(nam+" has no money. Cannot visit the "+s.slotName+".");
							}
						}
						else {
							gm.AddMessageToLog("Cannot place the same card twice.");
						}
					}
				}
			}
		}
		CardLerp(this,PosInBar);
		gm.sound.PlayAudioPitched(gm.sound.cardPickup,0.95f);
		//if(Physics.Raycast(ray,,layer)){
		//	print("HIT");
		//}

	}

	public void PlaceCardOnSlot(Slot s){
		//transform.position = s.gameObject.transform.position;
		transform.SetParent(s.cardPos);
		transform.SetAsFirstSibling();
		CardLerp(this,s.cardPos.position);
		isPlaced = true;
		isInHand = false;
		s.cardOnMe = this;
		gm.sound.PlayAudio(gm.sound.cardPlace);
		s.ChangeColor(s.cardColor);
		if(anim.GetBool("flipped")){
			CardFlip(false);
		}
		print("placed Card "+nam+" on Slot "+s.slotName);
		Do(s);
	}


	public void Do(Slot so){
	//	print("RARH, I'm DOING SOMETHING");

		if(onPlayed != null){
			onPlayed(so, this); //Executing the delegate function.
		}
		if(onPlayedValueChange != null){
			onPlayedValueChange(so, this, 0);
		}
		if(onNextTurn != null){
			print("GAVE "+so.slotName+" next Turn thing");
			so.SetNext(onNextTurn, this);
		}
		//Destroy(gameObject);
	}





	public int Status(){
		return wealth+fear;
	}

	public void DisplayToolTip(){
		if(!anim.GetBool("flipped")){
			Camera.main.gameObject.GetComponent<MouseControl>().DisplayCardToolTip(this);
		}
	}

	public void RemoveToolTip(){
		Camera.main.gameObject.GetComponent<MouseControl>().RemoveToolTip();
	}

	public void SetCardText(){
		fearText.text = ""+fear;
		wealthText.text = ""+wealth;
		unruliText.text = ""+unruliness;
		statusText.text = ""+Status();
	}

	public void ShuffleAnimation(){
		anim.SetTrigger("Shuffle");
	}

	public void PlayShuffleSound(){
		gm.sound.PlayAudio(gm.sound.grumbleshake);
	}

	public void CardLerp(Card c, Vector3 endPos){
		endOfLerp = endPos;
		StartCoroutine(lerp(c));
	}

	IEnumerator lerp(Card c){
		isLerping = true;
		Vector3 orgPos = c.transform.position;
		while(c.transform.position != endOfLerp){
			c.transform.position = Vector3.Lerp(c.transform.position,endOfLerp,Time.deltaTime*10);
			if(Vector3.Distance(c.transform.position,endOfLerp) <= 0.9f){
				c.transform.position = endOfLerp;
			}
			yield return new WaitForEndOfFrame();
		}
		isLerping = false;
	}

	public void CardFlip(bool f){
		anim.SetBool("flipped",f);
		gm.sound.PlayAudio(gm.sound.card_flip);
	}

	public void CardBackOn(){
		cardBack.SetActive(true);
	}

	public void CardBackOff(){
		cardBack.SetActive(false);
	}

	public string WinVarAsString(){
		switch(winvar){
		case WinVariable.Defenses:
			return "Defenses";
		case WinVariable.Fear:
			return "Fear";
		case WinVariable.Prosperity:
			return "Prosperity";
		case WinVariable.Status:
			return "Status";
		case WinVariable.Wealth:
			return "Wealth";
		case WinVariable.DeadKing:
			return "Dead King";
		default:
			return "NOTHING";
		}
	}

}

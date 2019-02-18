using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Slot : MonoBehaviour {

	public GameManager gm;
	public Card cardOnMe;
	public Card cardPlacedHereLast;
	public Text desctext;
	public Image icon;
	public Text wText;
	public Text fText;
	public Text dText;
	public string slotName;
	public Image onNextImg;
	public Image weatherImg;
	public bool weatherActive = false;
	public Transform cardPos;

	public int prosperity = 0;

	public int changeFear = 0;
	public int changeWealth = 0;
	public int changeReadiness = 0;
	public bool prosperityOppositeWealth = true;

	public bool isBroken = false;
	public int turnsToBeBroken = 2;
	int curTurnsBroken = 0;


	public Action<Card, int, int, int> OnNext;
	public Action<Card, int, int, int> OnNextNext;
	public bool readyNext = false; Card refForNextChecK;
	public string onNextDesc, onNextNextDesc;

	public Image frame;
	public Color defColor;
	public Color cardColor;
	public Color brokenColor;

	// Use this for initialization
	void Start () {
	
		desctext.text = ("Prosperity: "+prosperity);
		fText.text = ""+changeFear;
		wText.text = ""+changeWealth;
		dText.text = ""+changeReadiness;
		ChangeColor(defColor);
	}

	public void ProcessOutComeForCard(){
		if(isBroken){
			if(curTurnsBroken <= 0){
				UnBreak();
			}
			else{
				curTurnsBroken--;
				return;
			}
		}

		if(cardOnMe != null && !weatherActive){

//			print("processing");
			cardOnMe.fear += changeFear;
			cardOnMe.wealth += changeWealth;
			if(prosperityOppositeWealth){
				prosperity -= changeWealth;
			}
			else {
				prosperity += changeWealth;
			}
			gm.ChangeReadiness(changeReadiness);

			print(slotName+" PROCESSED "+cardOnMe.nam+". Fear "+cardOnMe.fear+". Wealth "+cardOnMe.wealth+". Prosperity is "+prosperity);

			if(OnNext != null && readyNext && cardOnMe != refForNextChecK){
				print(slotName+" executed on next "+readyNext+" "+OnNext);
				OnNext(cardOnMe,0,0,0);

				if(OnNextNext != null){
					OnNext = OnNextNext;
					onNextDesc = onNextNextDesc;
					onNextNextDesc = null;
					OnNextNext = null;
					readyNext = true;
				}
				else{
					print("resetting onnext");
					readyNext = false;
					OnNext = null;
					onNextDesc = null;
					onNextImg.gameObject.SetActive(false);
				}
			}


			UpdateText();

			refForNextChecK = null;

			if(prosperity <= 0){
				Break();
			}

			cardOnMe.SetCardText();

		}
		if(weatherActive){
			DeActivateWeather();
		}


	}

	public void UpdateText(){
		fText.text = ""+changeFear;
		wText.text = ""+changeWealth;
		dText.text = ""+changeReadiness;
		desctext.text = ("Prosperity: "+prosperity);
	}


	public void Break(){
		curTurnsBroken = turnsToBeBroken;
		isBroken = true;
		icon.color = Color.black;
		fText.gameObject.SetActive(false);
		wText.gameObject.SetActive(false);
		dText.gameObject.SetActive(false);
		gm.sound.PlayAudio(gm.sound.broken);
		ChangeColor(brokenColor);
	}

	public void UnBreak(){
		isBroken = false;
		icon.color = Color.white;
		prosperity = 1;
		fText.gameObject.SetActive(true);
		wText.gameObject.SetActive(true);
		dText.gameObject.SetActive(true);
		ChangeColor(defColor);
		UpdateText();
	}


	public void ActivateWeather(){
		weatherActive = true;
		weatherImg.gameObject.SetActive(true);
		gm.sound.PlayRains();
	}

	public void DeActivateWeather(){
		weatherActive = false;
		weatherImg.gameObject.SetActive(false);
		prosperity += 2;
		UpdateText();
	}


	public void SetNext(Action<Card, int, int, int> a, Card c){
		if(OnNext == null){
			OnNext = a;
			onNextDesc = c.functionLong;
		}
		else{
			OnNextNext = a;
			onNextNextDesc = c.functionLong;
		}
		readyNext = true;
		refForNextChecK = c;
		onNextImg.gameObject.SetActive(true);
		print("next turn set "+OnNext+" "+OnNextNext);
	}

	public void ChangeColor(Color c){
		frame.color = c;
	}





}

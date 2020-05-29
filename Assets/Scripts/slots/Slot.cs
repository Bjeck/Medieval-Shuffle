using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class Slot : MonoBehaviour {

    [HideInInspector]
	public GameManager gm;
	public Card cardOnMe;
	public Card cardPlacedHereLast;
    public Text nameText;
	public Text desctext;
	public Image icon;
	public Text wText;
	public Text fText;
	public Text dText;
    public Text moneyText;
	public string slotName;
	public Image onNextImg;
	public Image weatherImg;
	public bool weatherActive = false;
	public Transform cardPos;

	public int energy = 2;

	public int changeAuthority = 0;
	public int changeMoney = 0;
	public int changeDefense = 0;

    public bool moneyGenerator = false;
    public int moneyToGenerate = 0;
    public int availableMoney = 0;

	public bool isBroken = false;
	public int turnsToBeBroken = 1;
	int curTurnsBroken = 0;

    public bool lockCardOnSlot = false;

    public SlotAddition slotAddition = null;

	public Action<Card, int, int, int> OnNext;
	public Action<Card, int, int, int> OnNextNext;
	public bool readyNext = false; Card refForNextChecK;
	public string onNextDesc, onNextNextDesc;

    public List<Action> possibleRandomEvents = new List<Action>();

	public Image frame;
	public Color defColor;
	public Color cardColor;
	public Color brokenColor;

    private void Awake()
    {
        gm = GameObject.Find("Manager").GetComponent<GameManager>(); //AAAAAAAA
    }

    // Use this for initialization
    void Start () {

        energy = 2;
        nameText.text = slotName;
        UpdateText();

        moneyText.transform.parent.gameObject.SetActive(moneyGenerator == false ? false : true);
        dText.transform.parent.gameObject.SetActive(changeDefense == 0 ? false : true);
        wText.transform.parent.gameObject.SetActive(changeMoney == 0 ? false : true);
        fText.transform.parent.gameObject.SetActive(changeAuthority == 0 ? false : true);

        ChangeColor(defColor);

        if (slotAddition != null)
        {
            slotAddition.Setup();
        }
    }


	public virtual void ProcessOutComeForCard(){

        if(slotAddition != null)
        {
            slotAddition.PreProcessOutcome();
        }

        if (isBroken)
        {
			curTurnsBroken--;
            if (curTurnsBroken <= 0)
            {
                UnBreak();
            }
            return;
		}

		if(cardOnMe != null && !weatherActive)
        {

			cardOnMe.auth += changeAuthority;
			cardOnMe.wealth += changeMoney;

            //IF 
            if(availableMoney > 0)
            {
                Debug.Log("change treasury " + slotName + " " + availableMoney + " " + gm.treasury);
                gm.ChangeTreasury(availableMoney);
                availableMoney = 0;
            }

            gm.ChangeDefenses(Mathf.Min(changeDefense,cardOnMe.auth));

			print(slotName+" PROCESSED "+cardOnMe.nam+". Fear "+cardOnMe.auth+". Wealth "+cardOnMe.wealth+". Energy is "+energy);

            cardOnMe.OnTurn?.Invoke(this, cardOnMe);

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


            if(possibleRandomEvents.Count > 0)
            {
                int random = UnityEngine.Random.Range(0, possibleRandomEvents.Count);
                if(random < possibleRandomEvents.Count)
                {
                    possibleRandomEvents[random].Invoke();
                }
            }


			refForNextChecK = null;

            energy--;

            if (energy <= 0){
				Break();
			}

			cardOnMe.SetCardText();

		}


        if (slotAddition != null)
        {
            slotAddition.PostProcessOutcome();
        }

        

        if (weatherActive)
        {
            DeActivateWeather();
        }

        UpdateText();
    }

	public void UpdateText(){
		fText.text = ""+changeAuthority;
		wText.text = ""+changeMoney;
		dText.text = ""+changeDefense;
        moneyText.text = "" + availableMoney;

        desctext.text = ("Energy: "+energy);
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
		energy = 2;
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
		energy = 2;
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

    public void AddAvailableMoney(int money)
    {
        availableMoney += money;
        moneyText.transform.parent.gameObject.SetActive(true);
        UpdateText();
    }





}

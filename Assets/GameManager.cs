using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum State { Start, Choose, Outcome, Deal, Over, Status}
public enum TargetReason { Bribe, Threaten, Steal }

public class GameManager : MonoBehaviour {

	public bool inDuel = false;
	public Player playur;
	public GameObject cardBar;
	public GameObject statusObj;
	public Transform deckPos;
	public Transform allcardsPos;
	public MouseControl ms;
	public CardManager cm;
	public GameOver go;
	public Sound sound;
	public List<string> playerStartCards = new List<string>();
	public List<Card> allCards = new List<Card>();
	public List<Card> deadCards = new List<Card>();

	Dictionary<GameObject,bool> cardPoss = new Dictionary<GameObject, bool>();
	public List<GameObject> poss = new List<GameObject>();
	public List<Slot> slots = new List<Slot>();

	State state;
	public State startState;
	public TargetReason targetReason;

	public int readiness = 0;
	public int readinessCondition = 50;
	public int prosperity = 0;
	public int amtOfDays = 14;
	public int daysLeft;
	public bool playingForever = false;
	public Text dayText;
	public Text readyText;
	public Text prospText;
	public Button theButton;
	public GameObject overScreen;

	public bool firstTurn = true;
	public bool firstRevolt = true;
	public bool firstWeather = true;

	// Use this for initialization
	void Start () {
		daysLeft = amtOfDays;
		TickDay(0);

		foreach(GameObject g in poss){
			cardPoss.Add(g,false);
		}

		print("running setup");
		SetupPlayerDeck();
		ChangeProsperity(GlobalProsperity());
		ChangeReadiness(0);

		SwitchState(startState);
	
	}
	
	// Update is called once per frame
	void Update () {

	}


	public void PressedButton(){
		SwitchState(State.Outcome);
	}

	public void SwitchState(State s){
		state = s;

		switch(state){
		case State.Start:
			playur.BeginIntroduction();
			break;
		case State.Deal:
			Deal();
			break;
		case State.Choose:
			break;
		case State.Outcome:
			Outcome();
			break;
		case State.Over:
			EndGame();
			break;
		case State.Status:
			StatusState();
			break;
		}
	}



	public void Deal(){
	//Put Cards in Deck
		StartCoroutine(DealCoroutine());
	}

	IEnumerator DealCoroutine(){

		sound.PlayAudio(sound.shuffle);
		foreach(Card c in playur.currentDeck){
			c.isInHand = false;
			c.isPlaced = false;
			c.transform.SetParent(cardBar.transform);
			c.CardLerp(c,deckPos.position);
			c.CardFlip(true);
		}

		yield return new WaitForSeconds(0.5f);
		playur.currentDeck.Shuffle();

		//Deal
		List<GameObject> gs = new List<GameObject>(cardPoss.Keys);
		foreach(GameObject g in gs){
			cardPoss[g] = false;
		}


		foreach(Card c in playur.currentDeck){
			foreach(GameObject g in cardPoss.Keys){
				if(cardPoss[g] != true){
					c.transform.SetParent(cardBar.transform);
					//c.transform.position = g.transform.position;
					c.CardLerp(c,g.transform.position);
					c.CardFlip(false);
					sound.PlayAudio(sound.cardDeal);
					cardPoss[g] = true;
					c.isInHand = true;
					c.transform.SetAsLastSibling();
					yield return new WaitForSeconds(0.05f);
					break;
				}
			}
		}

		yield return new WaitForSeconds(0.4f);
		//REVOLT CHECK

		foreach(Card c in playur.currentDeck){
			c.PosInBar = c.transform.position;
		}

		if(!firstTurn){
			foreach(Card c in playur.currentDeck){
				if(c != playur.playerCard){
					if(Utilities.ShouldRevolt(c)){
						c.PlaceCardOnSlot(slots[ChooseSlot()]);

						yield return new WaitForSeconds(0.1f);
					}
				}
			}
		}

		theButton.interactable = true;
		SwitchState(State.Choose);
	}


	public void Outcome(){
		StartCoroutine(OutcomeCoroutine());
	}


	IEnumerator OutcomeCoroutine(){
		theButton.interactable = false;
		foreach(Slot s in slots){
			s.ProcessOutComeForCard();
			if(s.cardOnMe != null){
				sound.PlayAudio(sound.slotOutcome);
				yield return new WaitForSeconds(0.5f);
			}
		}

		//yield return new WaitForSeconds(0.1f);
		foreach(Card c in playur.currentDeck){
			if(!c.isPlaced && c.isInHand){
				c.unruliness++;
				c.ShuffleAnimation();

				if(firstRevolt){
					AddMessageToLog("Those who stay at home grow unruly.");
					firstRevolt = false;
				}
			}
			c.SetCardText();

		}

		yield return new WaitForSeconds(1.4f);

		cm.CheckForNewCards();
		ChangeProsperity(GlobalProsperity());

		for (int i = 0; i < allCards.Count; i++) {
			if(allCards[i].Status() <= 0 && !allCards[i].isDead){
				sound.PlayAudioPitched(sound.bell,0.7f);
				KillCard(allCards[i]);
				yield return new WaitForSeconds(0.6f);
			}
		}

		//RESET
		foreach(Slot s in slots){
			s.cardPlacedHereLast = s.cardOnMe;
			s.cardOnMe = null;
			if(!s.isBroken){
				s.ChangeColor(s.defColor);
			}
			int r = Random.Range(0,20);
			if(r < 2){
				s.ActivateWeather();
				if(firstWeather){
					AddMessageToLog("The "+s.slotName + " sees the first rain of the season.");
					firstWeather = false;
				}
			}
		}
		yield return new WaitForSeconds(0.5f);

		cm.hasDuelledthisTurn = false;

		if(daysLeft == 0 && !playingForever){
			go.gameEndedIn14Days = true;
			SwitchState(State.Over);
			yield return new WaitForEndOfFrame();
		}
		else if(playur.playerCard.Status() <= 0){
				go.gameEndedIn14Days = false;
				SwitchState(State.Over);
				yield return new WaitForEndOfFrame();
		}
		else{
			TickDay(-1);
			firstTurn = false;
			SwitchState(State.Deal);
			yield return new WaitForEndOfFrame();
		}

	}


	//END OUTCOME

	public void ChangeReadiness(int ch){
		readiness += ch;
		readyText.text = "Defenses: "+readiness+"/"+readinessCondition;
	}

	public void ChangeProsperity(int ch){
		prosperity = ch;
		prospText.text = "Prosperity: "+prosperity;
	}

	public int GlobalProsperity(){
		int sum = 0;
		foreach(Slot s in slots){
			if(s.slotName != "Distant Lands"){
				sum += s.prosperity;
			}
		}
		return sum;
	}



	public void TickDay(int i){
		daysLeft += i;
		dayText.text = daysLeft+" Days Left.";
	}


	public void StatusState(){
		statusObj.SetActive(true);

		//print(allCards.Count);
		foreach(Card c in allCards){
			float mappedHeight = c.Status() * Screen.height / 20;
			c.transform.SetParent(statusObj.transform);
		//	print(RectTransformUtility.WorldToScreenPoint(Camera.main,c.transform.position));
		//	c.GetComponent<RectTransform>(). = new Vector2(0.0f,0.0f);
		}

	}


	public void DeactivateStatusState(){
		foreach(Card c in allCards){
			c.transform.SetParent(cardBar.transform);
			c.transform.position = c.PosInBar;
		}
		statusObj.SetActive(false);
	}



	public void StartBribe(){
		StartTargetAction(TargetReason.Bribe);
	}
	public void StartThreaten(){
		StartTargetAction(TargetReason.Threaten);
	}
	public void StartSteal(){
		StartTargetAction(TargetReason.Steal);
	}

	public void StartTargetAction(TargetReason r){
		if(cm.hasDuelledthisTurn){
			AddMessageToLog("Can only perform an action once per day");
			return;
		}
		if(inDuel != true){
			if(state != State.Choose){
				return;
			}
			if(!playur.playerCard.isInHand){
				AddMessageToLog("Cannot do that when you are away.");
				return;
			}
			theButton.interactable = false;
			if(r == TargetReason.Bribe){
				if(playur.playerCard.wealth <= 0){
					AddMessageToLog("Cannot bribe without money.");
					return;
				}
				else{
					targetReason = TargetReason.Bribe;
					inDuel = true;
				}
			}
			else if(r == TargetReason.Threaten){
				targetReason = TargetReason.Threaten;
				inDuel = true;
			}
			else if(r == TargetReason.Steal){
				targetReason = TargetReason.Steal;
				inDuel = true;
			}

			cm.duelPanel.SetActive(true);
			playur.playerCard.transform.SetParent(cm.duelPanel.transform); playur.playerCard.transform.SetAsLastSibling();
			playur.playerCard.transform.position = cm.card1.transform.position;
		}
		else{
			cm.CancelDuel();
		}
	}

	public void ChooseTargetForDuel(Card c){
		print("CHOOSING TARGET");
		c.transform.SetParent(cm.duelPanel.transform); c.transform.SetAsLastSibling();
		c.transform.position = cm.card2.transform.position;

		if(targetReason == TargetReason.Bribe){
			cm.Bribe(playur.playerCard,c);
		}
		else if(targetReason == TargetReason.Threaten){
			cm.Threaten(playur.playerCard,c);
		}
		else if(targetReason == TargetReason.Steal){
			cm.Steal(playur.playerCard,c);
		}
	}



	public void EndGame(){
		go.StartGameOver();

	}



	public void KillCard(Card c){
		
		AddMessageToLog(c.nam+" Dies in shame!");
		c.anim.SetTrigger("Dead");
		if(playur.currentDeck.Exists(x=>x == c)){
			RemoveCardFromPlayer(c);
		}
		if(c.id == "king"){
			go.isKingDead = true;
		}
		//RemoveCardFromAll(c);
		c.isDead = true;
		StartCoroutine(waitToDie(c));
		if(!deadCards.Exists(x=>x==c)){
			deadCards.Add(c);

		}
		//Destroy(c.gameObject);

	}

	IEnumerator waitToDie(Card c){
		print("WAITING TO DIE");
		yield return new WaitForSeconds(1f);
		c.transform.position = allcardsPos.transform.position;
		c.CardLerp(c,allcardsPos.transform.position);

	}


	public void SetupPlayerDeck(){
		List<Card> temps = new List<Card>();
		temps.Add(allCards.Find(x=>x.id == "king"));
		temps.Add(allCards.Find(x=>x.id == "baby"));
		temps.Add(allCards.Find(x=>x.id == "bastard"));

		//FIGURING OUT THE PLAYER CARD
		foreach(Card c in temps){
			allCards.Remove(c);
		}
		int r = Random.Range(0,allCards.Count);
		playur.playerCard = allCards[r];			//CHANGE THIS WHENEVER I DO THE START RIGHT.
		playur.playerCard.youPip.gameObject.SetActive(true);

		foreach(Card c in temps){
			allCards.Add(c);
		}

		foreach(Card c in allCards){
			if(playerStartCards.Exists(x=>x == c.id)){
				AddCardToPlayer(c);
			}
		}

		if(!playur.currentDeck.Exists(x=>x==playur.playerCard)){
			AddCardToPlayer(playur.playerCard);
		}

	}



	public void AddCardToPlayer(Card c){ 
		playur.currentDeck.Add(c);
	}

	public void RemoveCardFromPlayer(Card c){
		playur.currentDeck.Remove(c);
		//c.transform.position = allcardsPos.position;
	}


	public void AddCardToAll(Card c){
		allCards.Add(c);
		c.gm = this;
		c.transform.position = allcardsPos.position;
	}

	public void RemoveCardFromAll(Card c){
		allCards.Remove(c);
	}

	public int ChooseSlot(){
		int r = Random.Range(0,slots.Count);
		if(slots[r].cardOnMe == null && !slots[r].isBroken){
			return r;
		}
		else{
			return ChooseSlot();
		}
	}

	public void AddMessageToLog(string s){
		ms.AddMessageToLog(s);
	}

	public void CheckThisOut(){
		if(Input.GetKey(KeyCode.P)){
			if(Input.GetKey(KeyCode.A)){
				foreach(Card c in allCards){
					c.thisImg.sprite = cm.GetSprite(c);
				}
			}
		}
	}



}

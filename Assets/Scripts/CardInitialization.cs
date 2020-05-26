using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardInitialization : MonoBehaviour {

	public GameObject CardPrefab;
	public GameManager gm;


	// Use this for initialization
	void Start () {
		GameObject cardObj;

		cardObj = (GameObject)Instantiate(CardPrefab);
		Card newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Gutsy Jester";
		newCard.desc = "A laugh's all you get.";
		newCard.id = "jester";
		newCard.fear = 1;
		newCard.wealth = 1;
		newCard.unruliness = 3;
		newCard.winvar = WinVariable.Wealth;
		newCard.winCondition = 15;
		newCard.onPlayed = (sl, c) => {FunctionToExecute(sl, c);};
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		cardObj = (GameObject)Instantiate(CardPrefab);
		newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Petty Prince";
		newCard.desc = "He acts like he doesn't care about anyone, but no one can tell if he means it.";
		newCard.id = "prince";
		newCard.function = "Defenses -2";
		newCard.functionLong = "Decreases The Kingdom's Defenses with 2";
		newCard.fear = 2;
		newCard.wealth = 6;
		newCard.unruliness = 0;
		newCard.winvar = WinVariable.Prosperity;
		newCard.winCondition = 3;
		newCard.winAbove = false;
		newCard.onPlayed = (sl, c) => {FunctionToExecute(sl, c); ChangeDefenses(sl,c,-2);};
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		cardObj = (GameObject)Instantiate(CardPrefab);
		newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Bored King";
		newCard.desc = "He's not sorry.";
		newCard.id = "king";
		newCard.function = "Prosperity -2";
		newCard.functionLong = "Decreases Prosperity with 2";
		newCard.fear = 4;
		newCard.wealth = 10;
		newCard.unruliness = 5;
		newCard.onPlayed = (sl, c) => {FunctionToExecute(sl, c); ChangeProsperity(sl,c,-2);}; 	//ALWAYS DECREASES PROSPERITY BY 2
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		cardObj = (GameObject)Instantiate(CardPrefab);
		newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Clumsy Queen";
		newCard.desc = "She just drops money everywhere she goes!";
		newCard.id = "queen";
		newCard.function = "Next Card: +2 Wealth";
		newCard.functionLong = "Leaves Money. The Next Card on this Slot gets +2 Wealth";
		newCard.fear = 2;
		newCard.wealth = 5;
		newCard.unruliness = 0;
		newCard.winvar = WinVariable.Prosperity;
		newCard.winCondition = 25;
		newCard.winAbove = true;
		newCard.onPlayed = (sl, c) => {FunctionToExecute(sl, c); };
		newCard.onNextTurn = (sl, i, i2, i3) => {ChangeValuesToNextCard(sl, i, 2, i3);};
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		cardObj = (GameObject)Instantiate(CardPrefab);
		newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Lone Knight";
		newCard.desc = "He doesn't talk much. When asked, his name is 'Phillip'.";
		newCard.id = "knight";
		newCard.function = "Next Card: -2 Wealth";
		newCard.functionLong = "Drinks. The Next Card on this Slot gets -2 Wealth";
		newCard.fear = 4;
		newCard.wealth = 1;
		newCard.unruliness = 0;
		newCard.winvar = WinVariable.Status;
		newCard.winCondition = 1;
		newCard.winAbove = false;
		newCard.onPlayed = (sl, c) => {FunctionToExecute(sl, c);};
		newCard.onNextTurn = (sl, i, i2, i3) => {ChangeValuesToNextCard(sl, i, -2, i3);};
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		cardObj = (GameObject)Instantiate(CardPrefab);
		newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Careful Princess";
		newCard.desc = "She awaits the right time to strike.";
		newCard.id = "princess";
		newCard.function = "Prosperity -1. Fear +2";
		newCard.functionLong = "Lays plans. Decreases Prosperity with 1, but gains 2 fear.";
		newCard.fear = 2;
		newCard.wealth = 5;
		newCard.unruliness = 0;
		newCard.winvar = WinVariable.DeadKing;
		newCard.winCondition = 1;
		newCard.winAbove = true;
		newCard.onPlayed = (sl, c) => {FunctionToExecute(sl, c);};
		newCard.onPlayedValueChange = (sl, c, i) => {ChangeProsperity(sl, c, -1); ChangeFear(sl,c,2);}; 
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		cardObj = (GameObject)Instantiate(CardPrefab);
		newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Mellow Executioner";
		newCard.desc = "He has never said whether he enjoys his job or not. Yet he doesn't get paid much for it.";
		newCard.id = "executioner";
		newCard.function = "Next Card: -2 Fear";
		newCard.functionLong = "The Next Card on this Slot gets -2 Fear";
		newCard.fear = 8;
		newCard.wealth = 3;
		newCard.unruliness = 1;
		newCard.winvar = WinVariable.Prosperity;
		newCard.winCondition = 25;
		newCard.winAbove = true;
		newCard.onPlayed = (sl, c) => {FunctionToExecute(sl, c);};
		newCard.onNextTurn = (sl, i, i2, i3) => {ChangeValuesToNextCard(sl, -2, i2, i3);};
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		cardObj = (GameObject)Instantiate(CardPrefab);
		newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Chubby Thief";
		newCard.desc = "She had a rough childhood. Now she steals from kings.";
		newCard.id = "thief";
		newCard.function = "Prosperity -2. Wealth +2";
		newCard.functionLong = "Steals. Decreases Prosperity with 2, but gains 2 wealth.";
		newCard.fear = 6;
		newCard.wealth = 4;
		newCard.unruliness = 3;
		newCard.winvar = WinVariable.Fear;
		newCard.winCondition = 20;
		newCard.winAbove = true;
		newCard.onPlayed = (sl, c) => {FunctionToExecute(sl, c);};
		newCard.onPlayedValueChange = (sl, c, i) => {ChangeProsperity(sl, c, -2); ChangeWealth(sl,c,2);}; 
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		cardObj = (GameObject)Instantiate(CardPrefab);
		newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Court Magician";
		newCard.desc = "He might tell the truth. He might not.";
		newCard.id = "magician";
		newCard.function = "Changes prosperity randomly";
		newCard.functionLong = "Has a random effect on Prosperity between -2/+2";
		newCard.fear = 5;
		newCard.wealth = 1;
		newCard.unruliness = 0;
		newCard.winvar = WinVariable.Status;
		newCard.winCondition = 22;
		newCard.winAbove = true;
		newCard.onPlayed = (sl, c) => {ChangeProsperityRandom(sl, c);}; 		//CHANGES PROSPERITY TO SOMETHING RANDOM.
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		cardObj = (GameObject)Instantiate(CardPrefab);
		newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Unlucky Baby";
		newCard.desc = "No one told him it wasn't feeding time.";
		newCard.id = "baby";
		newCard.function = "Prosperity +2";
		newCard.functionLong = "Improves Prosperity with 2";
		newCard.fear = 0;
		newCard.wealth = 3;
		newCard.unruliness = -4;
		newCard.onPlayedValueChange = (sl, c, i) => {ChangeProsperity(sl, c, 2);}; 		//Improves prosperity with 2
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		cardObj = (GameObject)Instantiate(CardPrefab);
		newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Bastard Child";
		newCard.desc = "She doesn't understand why no one likes her.";
		newCard.id = "bastard";
		newCard.function = "Prosperity -1";
		newCard.functionLong = "Decreases Prosperity with 1";
		newCard.fear = 3;
		newCard.wealth = 0;
		newCard.unruliness = -1;
		newCard.onPlayedValueChange = (sl, c, i) => {ChangeProsperity(sl, c, -1);}; 		//decreases prosperity with 1
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		cardObj = (GameObject)Instantiate(CardPrefab);
		newCard = cardObj.GetComponent<Card>();
		newCard.nam = "The Confused Stranger";
		newCard.desc = "She doesn't know the language, yet everyone wants to talk to her.";
		newCard.id = "stranger";
		newCard.function = "Wealth -2, Fear +2";
		newCard.functionLong = "Decreases Wealth with 2, Increases Fear with 2";
		newCard.fear = 2;
		newCard.wealth = 4;
		newCard.unruliness = 1;
		newCard.winvar = WinVariable.Defenses;
		newCard.winCondition = 10;
		newCard.winAbove = false;
		newCard.onPlayed = (sl, c) => {FunctionToExecute(sl, c);};
		newCard.onPlayedValueChange = (sl, c, i) => {ChangeWealth(sl,c,-2); ChangeFear(sl,c,2);}; 
		newCard.transform.SetParent(gm.cardBar.transform);
		newCard.GetComponent<RectTransform>().localScale = Vector3.one;
		AdjustVisuals(newCard);
		gm.AddCardToAll(newCard);

		print("all cards initialized");

	}


	public void AdjustVisuals(Card c){
		Text[] texts = c.GetComponentsInChildren<Text>();
		texts[0].text = c.nam;
		texts[1].text = c.function;
	}



	public void ChangeDefenses(Slot so, Card c, int i){
		gm.ChangeReadiness(i);
		print(c.nam+" Changed defenses from "+so.slotName+" because I'm "+c.nam);
	}

	public void ChangeProsperity(Slot so, Card c, int i){
	//	so.prosperity += (i);
	//	print(c.nam+" Changed prosperity of "+so.slotName+" from "+ (so.prosperity-i) + " to "+so.prosperity);
	}

	public void ChangeProsperityRandom(Slot so, Card c){
	//	so.prosperity += Random.Range(-2,2);
	//	print("Changed "+so.slotName+"'s prosperity! Now "+so.prosperity);
	}

	public void ChangeWealth(Slot s, Card c, int i){
		print("Changed wealth "+c.nam +" "+i);
		c.wealth += i;
	}

	public void ChangeFear(Slot s, Card c, int i){
		print("Changed fear "+c.nam +" "+i);
		c.fear += i;
	}

	public void ChangeValuesToNextCard(Card c, int f, int w, int u){
		print("CARD CHANGE "+f+" "+w+" "+u);
		c.fear += f;
		c.wealth += w;
		c.unruliness += u;
	}


	public void FunctionToExecute(Slot so, Card c){
	//	print("I SMITE THEE "+so.slotName);
	}

}

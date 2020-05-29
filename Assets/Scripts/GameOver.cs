using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public GameManager gm;
	public GameObject overScreen;

	public Text text;
	public Image cardPos;
	public Image cardDead;
	public Image cover;
	public Button nextButton;
	public Button otherButton;

	public bool isKingDead = false;
	public bool gameEndedIn14Days = false;

	List<Card> cardsAtEnd = new List<Card>();

	int count = 0;

	bool showedFinalScreen = false;

	public void StartGameOver(){
		cardsAtEnd.Clear();
		nextButton.interactable = false;
		nextButton.GetComponentInChildren<Text>().text = "Let us see the damage.";

		if(!gm.playur.playerCard.isDead){
			text.text = "GAME OVER\n\nGood job! You ruled a kingdom for 7 days! Who'd have thought you could do it?";
		}
		else{
			text.text = "YOU DIED\n\nYour power got to your head, you thought yourself invincible. Well, here you are.";
		}
		if(TestWinCondition()){
			text.text += "\n\nYOU WON! You managed to fulfill your dreams. Now what?";
		}
		else{
			text.text += "\n\nYou didn't manage to fulfill your dreams.";
		}

		if(gm.readiness > gm.readinessCondition){
			text.text +="\n\nYou amassed enough defenses to withstand an invasion.";
		}
		else{
			text.text += "\n\nYou did not amass enough defenses to withstand an invasion.";
		}

		overScreen.SetActive(true);
		cardsAtEnd.AddRange(gm.playur.currentDeck);			//REMOVE PLAYER CARD UP HERE AND DO THE SPECIAL THING WITH IT
		cardsAtEnd.AddRange(gm.deadCards);
		cardsAtEnd.Remove(gm.playur.playerCard);


		cardsAtEnd.Shuffle();
		cardsAtEnd.Insert(0,gm.playur.playerCard);

		//cover.gameObject.SetActive(true);
		count = cardsAtEnd.Count-1;
		nextButton.interactable = false;
		otherButton.gameObject.SetActive(false);
		StartCoroutine(GameOverAnim());
	}

	IEnumerator GameOverAnim(){
		float offset = 0;
		foreach(Card c in cardsAtEnd){
			c.transform.SetParent(cardPos.transform);
			c.CardLerp(c,cardPos.transform.position+new Vector3(0,offset,0));
			c.CardFlip(true);

			offset += 30;
			yield return new WaitForSeconds(0.05f);
		//	c.gameObject.SetActive(false);
		}
		yield return new WaitForSeconds(1.4f);

		nextButton.interactable = true;
	}



	public void LoadNextScreen(){
	//	cover.gameObject.SetActive(false);
		cardDead.gameObject.SetActive(false);

		if(count < 0){
			if(!showedFinalScreen){
				FinalLoad();
				return;
			}
			else{
				Restart();
				return;
			}
		}
		cardsAtEnd[count].transform.SetAsLastSibling();
		cardsAtEnd[count].CardFlip(false);

		text.text = PluckCardText(cardsAtEnd[count]);
		nextButton.GetComponentInChildren<Text>().text = "Very Good.";


		count--;
	}


	public void FinalLoad(){

		if(gm.playur.playerCard.isDead){
			if(isKingDead){
				text.text = "With both the King and you dead, the Kingdom spirals into despair, unsure how to elect the next ruler.";
				nextButton.GetComponentInChildren<Text>().text = "'I can fix this!' [Restart]";
			}
			else{
				text.text = "Since you are dead, the King has no choice but to elect another ruler. Let us hope they fare better than you did.";
				nextButton.GetComponentInChildren<Text>().text = "Whose turn is it? [Restart]";
			}
		}
		else{
			if(isKingDead){
				text.text = "With the King dead, no one is to stop you from ruling forever.\nWhat say you?";
				otherButton.gameObject.SetActive(true);
				otherButton.GetComponentInChildren<Text>().text = "Keep your rule! [Keep playing]"; 
				nextButton.GetComponentInChildren<Text>().text = "Let someone else try! [Restart]";

			}
			else{
				text.text = "The King approaches you, reminding you that your time is up.\nWhat say you?";
				nextButton.GetComponentInChildren<Text>().text = "Whose turn is it? [Restart]";
				otherButton.GetComponentInChildren<Text>().text = "I retire from this charade! [Quit Game]"; 
			}
		}
		showedFinalScreen = true;


	}

	public bool TestWinCondition(){
		Card c = gm.playur.playerCard;

		int nr = 0;
		switch(c.winvar){
		case WinVariable.DeadKing:
			if(gm.allCards.Find(x=>x.id=="king").isDead){
				nr = 1;
			}
			break;
		case WinVariable.Defenses:
			nr = gm.readiness;
			break;
		case WinVariable.Fear:
			nr = c.auth;
			break;
		case WinVariable.Wealth:
			nr = c.wealth;
			break;
		case WinVariable.Status:
			nr = c.Status();
			break;
		case WinVariable.Prosperity:
			nr = gm.GlobalProsperity();
			break;
		}

		if(c.winAbove){
			if(nr >= c.winCondition){
				return true;
			}
		}
		else{
			if(nr <= c.winCondition){
				return true;
			}
		}

		return false;

	


	}


	public string PluckCardText(Card c){
		string sToSend = "";




		switch(c.id){
		case "king":
			sToSend = "The King\n\n";
			if(gm.readiness > gm.readinessCondition){
				sToSend += "Pleased with your amounted defenses, he rests back in his castle for the time being, soon to select the next ruler.";
			}
			else if(gm.readiness < 10){
				sToSend += "With the low defenses readied for The Kingdom, the King is under a lot of pressure from the populace, and finds himself fleeing to the Castle in order to prepare for the next week";
			}

			else if(gm.GlobalProsperity() > 20){
				sToSend += "He sees the splendor and prosperity of The Kingdom that you have created and begins to become worried that others might think him a poor ruler.";
			}
			else if(gm.GlobalProsperity() < 5){
				sToSend += "He almost considers never giving another peasant power again, after the absolute ruin you have left behind you. But he's still bored.";
			}
			else {
				sToSend += "Your rule did little to alleviate his boredom. He hopes the next one will fare better.";
			}

			break;


		case "queen":
			sToSend = "The Queen\n\n";
			if(IsCardInList("thief") && IsCardInList("executioner")){
				sToSend += "Having released the thief from prison and thusly led the Executioner into the world, has decided to stay at home, and not cause further trouble, for herself, or others.\n";
			}
			else if(c.wealth < 1){
				sToSend += "Without any money to 'drop', she rushes to the Kingdom's treasury to pick up some more. Again. And again. The King is forced to quarantine her.\n";
			}
			else if(IsCardInList("bastard") && c.auth < 2){
				sToSend += "The shame of the existence of the Bastard Child breaks her and she hides in her chambers.";
			}
			else{
				sToSend += "Keeps dropping money.\n";
			}
			break;

		case "jester":
			sToSend = "The Jester\n\n";
			if(c.auth > 8){
				sToSend += "Becomes a hired assassin feared far and wide. A true agent of the Kingdom.";
			}
			else if(c.wealth > 8){
				sToSend += "Retires with his wealth, and spends the rest of his days drinking fine wine in a manor on a hill";
			}
			else{
				sToSend += "Continues being a Jester. Maybe it's best for him.";
			}
			break;

		case "knight":
			sToSend = "The Knight\n\n";
			if(c.Status() > 15){
				sToSend += "Because of his high Status, he begins demanding higher and higher pay, until the King can no longer afford him. Thus, The Knight is sent away from the Kingdom";
			}
			else if(c.Status() < 2){
				sToSend += "In shame, The Knight hides from the public view, never to be seen again.";
			}
			else{
				sToSend += "Still looking for that special someone.";
			}
			break;

		case "princess":
			sToSend = "The Princess\n\n";
			if(c.wealth > 10){
				sToSend += "With her great wealth, she hires an assassin to kill the King and Queen, to at last become the rightful ruler.";
			}
			else if(IsCardInList("prince") && c.auth > 5){
				sToSend += "She trains as a witcher and seeks to uncover the mysteries of the forest. Something must have turned the prince into a frog.";
			}
			else{
				sToSend += "She waits still. The time is not yet right.";
			}
			break;

		case "prince":			//COME BACK
			sToSend = "The Prince\n\n";
			if(c.wealth > 10){
				sToSend += "Spends all his money doing anything but improving the defenses. It seems he almost wants the country to be invaded.";
			}
			else if(c.auth > 10){
				sToSend += "Strikes a terror into anyone in The Kingdom. Some hopes it'll even help keep the invaders at bay. Yet he has not seemed to care much for helping.";
			}
			else{
				sToSend += "Tends to roam around in his chambers without divulging much to anyone around him. The Kingdom grows worried about his health.";
			}
			break;

		case "magician":
			sToSend = "The... Magician?\n\n";
			if(c.Status() > 12){
				sToSend += "People begin believing him too much and it goes to his head. He becomes the leader of a new cult, devoted his magic";
			}
			else if(c.Status() < 2){
				sToSend += "Shocked that no one believes him, he kills himself from the Tower in the University";
			}
			else {
				sToSend += "No one can tell if they should believe him or not.";
			}
			break;
		case "executioner":
			sToSend = "The Executioner\n\n";
			if(!IsCardInList("thief")){
				sToSend += "With the Thief dead, he drifts aimlessly around the countryside.";
			}
			else if(c.Status() > 15 && IsCardInList("thief")){
				sToSend += "He becomes a terror in the Kingdom, toturing innocents for more information on The Thief's whereabouts.";
			}
			else{
				sToSend += "He contines his hunt on The Thief. Forever.";
			}
			break;
		case "thief":
			sToSend += "The Thief\n\n";
			if(c.wealth > 10){
				sToSend += "With her amassed wealth, she purchases a villa on the edge of the Kingdom and retires her criminal career";
			}
			else if(c.auth > 8 && c.wealth < 3){
				sToSend += "Angry at her low wealth, she plans the greatest heist in the Kingdom's history.";
			}
			else if(IsCardInList("executioner")){
				sToSend += "She flees from The Executioner to the Distant Lands, never to be seen again.";
			}
			else {
				sToSend += "She continues enjoying her criminal career, out of fear from the law and anything else.";
			}
			break;
		case "stranger":
			sToSend += "The Stranger\n\n";
			if(c.wealth > 10){
				sToSend += "She travels back home, glad to escape The Kingdom. She had just wanted to visit";
			}
			else if(c.Status() > 15){
				sToSend += "With her high status, everyone swoons around her, yet she is trying to explain them all that she wants to go home.";
			}
			else {
				sToSend += "She stays in the Kingdom. Yet no one knows what she wants.";
			}

			break;
		case "baby":
			sToSend += "The Child\n\n";
			sToSend += "Is just a Baby. He blinks at you with huge, cute eyes.";
			break;
		case "bastard":
			sToSend += "The Bastard\n\n";
			sToSend += "Is just a Baby. She blinks at you with huge, cute eyes.";
			break;
		}


		if(c.isDead){
			sToSend = c.nam+"\n\nIs dead. What could have happened had they been alive?";
			cardDead.transform.position = c.transform.position;
			cardDead.gameObject.SetActive(true);
		}


		if(c == gm.playur.playerCard){
			sToSend = "You\n\n";
			if(c.Status() > 14){
				sToSend += "Did well for yourself. You are now a respected and feared member of The Kingdom.";
			}
			else if(c.Status() < 3){
				sToSend += "Are ridiculed by most of The Kingdom. You cannot be in public for long without hearing laughter behind your back.";
			}
			else{
				sToSend += "You leave the throne with neither admirers and rivals. People are, at least, impressed with your ability to stay alive.";
			}

			if(gm.readiness > gm.readinessCondition && gm.allCards.Find(x=>x.id == "king").isDead){
				sToSend += "\n\nThe Kingdom is ready for the oncoming invasion. It's bound to come any day now. The King would applaud you, had he been alive";
			}
			else if(gm.readiness > gm.readinessCondition && !gm.allCards.Find(x=>x.id == "king").isDead){
				sToSend += "\n\nThe Kingdom is ready for the oncoming invasion. It's bound to come any day now. The King applauds you for your foresight, but remains skeptical";
			}
			else{
				sToSend += "\n\nThe Kingdom might not be ready for the oncoming invasion. We can always hope it never happens.";
			}
		}

		return sToSend;
		
	}


	public bool IsCardInList(string s){
		return (gm.playur.currentDeck.Exists(x=>x.id==s) && gm.playur.playerCard.id != s);
	}

	public void KeepPlayingWithoutKing(){
		overScreen.SetActive(false);
		gm.playingForever = true;
		gm.SwitchState(State.Deal);
	}

	public void Restart(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void QuitGame(){
		Application.Quit();
	}

}

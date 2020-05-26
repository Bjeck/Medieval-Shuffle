using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public GameManager gm;
	public List<Card> currentDeck = new List<Card>();
	public Card playerCard;

	public GameObject eventScreen;
	public Text eventText;
	public GameObject eventCardPos;

	int count = 0;


	public GameObject tutObj;

	public bool doTutorial = true;
	public Button otherButton;

	// Use this for initialization
	void Start () {
		tutObj.SetActive(false);
	}

	public void BeginIntroduction(){
		eventScreen.SetActive(true);

		eventText.text = "<size=40>The King has grown tired of ruling!</size>\n\n\nHe has decided that a new person, chosen at random, \nwill rule for 7 days.\n\nYou have been chosen. \nHow much trouble do you think you can stir up in 7 days?";

	}

	public void NextScreen(){

		if(count == 0){
			eventText.text = "But doom might be upon The Kingdom.\n\nRumours say The Distant Lands are preparing an invasion. The Kingdom has to improve its defenses before it could withstand such an attack. If it ever comes.";
		}
		else if(count == 1){
			playerCard.transform.SetParent(eventCardPos.transform);
			playerCard.CardLerp(playerCard,eventCardPos.transform.position);
			eventText.text = "You are "+playerCard.nam+".\n\n\n\n";
			eventText.text += DiscoverGoal();
			if(playerCard.winAbove){
				eventText.text += "\n\nPERSONAL WIN CONDITION:\n"+playerCard.winCondition+" "+playerCard.WinVarAsString()+" or above.";
			}
			else {
				eventText.text += "\n\nPERSONAL WIN CONDITION:\nMaximum "+playerCard.winCondition+" "+playerCard.WinVarAsString();
			}
			otherButton.gameObject.SetActive(true);
		}

		else if(count == 2){
			EndIntroduction();
		}

		count++;
	}



	public void EndIntroduction(){

		eventScreen.SetActive(false);


		gm.SwitchState(State.Deal);

			StartCoroutine(WaitWithTutorial());
	}



	public string DiscoverGoal(){

		switch(playerCard.id){
		case "queen":
			return "The King's loyal wife and partner. Officially.\n\nSecretly, you want to share the wealth of the rich with the populace, causing you to leave money wherever you go.";
		case "jester":
			return "The Royal Jester. No one counts you as much, but you have ambitious plans.\n\nIn order to achieve your dreams, you have to first amass significant wealth";
		case "knight":
			return "The Favored Knight of The Kingdom, its steel and arms.\n\nYou are growing tired of the warfare and really just want to find a suitable partner and retire.";
		case "princess":
			return "You were lost in The Kingdom before you were found by an ambitious courtier, whom you swiftly abandoned.\n\nIn truth, you know The Kingdom is rightfully yours and an assassinated King would not be amiss.";
		case "magician":
			return "You were once a Professor at the University, but you have since seen the light, and know that you are a powerful Magician.\n\nProblem is, no one else believes you. You must acquire most Status so those peasants see what power you possess.";
		case "thief":
			return "You have just escaped from Prison. The nomination came as much as a surprise to you as everyone else.\n\nBut you do not intend to squander it. Time to show the world that you should not be messed with.";
		case "executioner":
			return "The Scythe of The Kingdom, you have long been feared throughout the land.\n\nNow, when it is time to rule, you want to show a gentler side to yourself. The countryside should prosper.";
		case "stranger":
			return "You were brought here by a Lone Knight, and is only barely able to speak the language, so this situation is quite strange.\n\nHowever, you come from the Distant Lands, and thus have no intention of building up the defenses.";
		case "prince":
			return "The Kingdom has long abandoned you, starting from a young age, and later when you were turned into a frog in the Forest\n\nNow that you're back, you vow revenge, wanting nothing more than The Kingdom to burn to the ground.";
		}

		return "";
	}


	IEnumerator WaitWithTutorial(){
		gm.sound.StopAudio(gm.sound.menuMusic);
		yield return new WaitForSeconds(0.6f);
		gm.sound.PlayMusic();
		yield return new WaitForSeconds(1f);

		if(doTutorial){
			StartTutorial();
		}
	}

	public void StartTutorial(){
		tutObj.SetActive(true);
	}


	public void SetDoTutorial(bool f){
		doTutorial = f;
		print("SET "+f);
	}




}

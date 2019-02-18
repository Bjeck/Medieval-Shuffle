using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CardManager : MonoBehaviour {

	public GameManager gm;

	Slot castle, castlewing, prison, lands, farm, village, forest, university;

	public bool baby, executioner, thief, magician, knight, prince, princess, bastard, stranger = false;

	public GameObject duelPanel, card1, card2;

	public List<Sprite> cardImgList = new List<Sprite>();
	public List<string> cardImgNames = new List<string>();
	public Dictionary<string, Sprite> cardImgs = new Dictionary<string,Sprite>();

	public bool hasDuelledthisTurn = false;
	// public Card duelist1, duelist2;

	// Use this for initialization
	void Start () {
		castle = gm.slots.Find(x=>x.slotName == "Castle");
		castlewing = gm.slots.Find(x=>x.slotName == "Castle Wing");
		prison = gm.slots.Find(x=>x.slotName == "Prison");
		lands = gm.slots.Find(x=>x.slotName == "Distant Lands");
		farm = gm.slots.Find(x=>x.slotName == "Farm");
		village = gm.slots.Find(x=>x.slotName == "Village");
		forest = gm.slots.Find(x=>x.slotName == "Forest");
		university = gm.slots.Find(x=>x.slotName == "University");

		if(gm.playur.currentDeck.Exists(x=>x.id == "thief")){ thief = true; }
		if(gm.playur.currentDeck.Exists(x=>x.id == "magician")){ magician = true; }
		if(gm.playur.currentDeck.Exists(x=>x.id == "knight")){ knight = true; }
		if(gm.playur.currentDeck.Exists(x=>x.id == "executioner")){ executioner = true; }
		if(gm.playur.currentDeck.Exists(x=>x.id == "prince")){ prince = true; }
		if(gm.playur.currentDeck.Exists(x=>x.id == "princess")){ princess = true; }
		if(gm.playur.currentDeck.Exists(x=>x.id == "stranger")){ stranger = true; }


		for (int i = 0; i < cardImgList.Count; i++) {
			cardImgs.Add(cardImgNames[i],cardImgList[i]);
		}
			
	}




	public void CheckForNewCards(){
//		print("checking "+prince);

		if(!baby){
			if(castle.cardOnMe != null && castlewing.cardOnMe != null){
				if(castle.cardOnMe.id == "king" || castle.cardOnMe.id == "queen"){
					if(castlewing.cardOnMe.id == "king" || castlewing.cardOnMe.id == "queen"){
						gm.AddCardToPlayer(gm.allCards.Find(x=>x.id == "baby"));
						gm.AddMessageToLog("A baby has been born in the kingdom! Rejoice!");
						gm.sound.PlayAudio(gm.sound.bell);
						baby = true;
					}
				}
			}
		}

		if(!bastard){
			if(castle.cardOnMe != null && castlewing.cardOnMe != null){
				if(castle.cardOnMe.id == "knight" || castle.cardOnMe.id == "queen"){
					if(castlewing.cardOnMe.id == "knight" || castlewing.cardOnMe.id == "queen"){
						gm.AddCardToPlayer(gm.allCards.Find(x=>x.id == "bastard"));
						gm.AddMessageToLog("The Queen has given birth to a child. But the kingdom suspects the King is not the father.");
						bastard = true;
						gm.sound.PlayAudio(gm.sound.bell);

					}
				}
				else if(castle.cardOnMe.id == "stranger" || castle.cardOnMe.id == "king"){
					if(castlewing.cardOnMe.id == "stranger" || castlewing.cardOnMe.id == "king"){
						gm.AddCardToPlayer(gm.allCards.Find(x=>x.id == "bastard"));
						gm.AddMessageToLog("The Stranger has given birth to a child. But the King is the father.");
						bastard = true;
						gm.sound.PlayAudio(gm.sound.bell);

					}
				}
			}
		}

		if(!thief){
			if(prison.cardOnMe != null){
				if(prison.cardOnMe.id == "queen"){
					gm.AddCardToPlayer(gm.allCards.Find(x=>x.id == "thief"));
					gm.AddMessageToLog("A Thief has escaped from prison.");
					thief = true;
					gm.sound.PlayAudio(gm.sound.bell);

				}
			}
		}

		if(!knight){
			if(village.cardOnMe != null){
				if(village.cardOnMe.id == "king"){
					gm.AddCardToPlayer(gm.allCards.Find(x=>x.id == "knight"));
					gm.AddMessageToLog("A boy followed the King home. So he knighted him.");
					knight = true;
					gm.sound.PlayAudio(gm.sound.bell);
				}
			}
		}

		if(!stranger){
			if(lands.cardOnMe != null){
				if(lands.cardOnMe.id == "knight" || lands.cardOnMe.id == "king" || lands.cardOnMe.id == "prince"){
					gm.AddCardToPlayer(gm.allCards.Find(x=>x.id == "stranger"));
					gm.AddMessageToLog("A Stranger was brought home from the Distant Lands.");
					stranger = true;
					gm.sound.PlayAudio(gm.sound.bell);
				}
			}
		}

		if(!executioner){
			if(prison.cardOnMe != null){
				if(prison.cardOnMe.id == "thief"){
					gm.AddCardToPlayer(gm.allCards.Find(x=>x.id == "executioner"));
					gm.AddMessageToLog("The thief's bold move agitated the Executioner. He's now on the hunt.");
					executioner = true;
					gm.sound.PlayAudio(gm.sound.bell);
				}
			}
		}

		if(!princess){
			if(farm.cardOnMe != null){
				if(farm.cardOnMe.id == "jester"){
					gm.AddCardToPlayer(gm.allCards.Find(x=>x.id == "princess"));
					gm.AddMessageToLog("The Jester found the long lost Princess on the farm.");
					princess = true;
					gm.sound.PlayAudio(gm.sound.bell);
				}
			}
		}

		if(!prince){
			if(forest.cardOnMe != null){
				if(forest.cardOnMe.id == "princess"){
					gm.AddCardToPlayer(gm.allCards.Find(x=>x.id == "prince"));
					gm.AddMessageToLog("A prince followed the princess home. She claims she kissed a frog, but no one believes her.");
					prince = true;
					gm.sound.PlayAudio(gm.sound.bell);
				}
			}
		}

		if(!magician){
			if(university.cardOnMe != null){
				//if(university.cardOnMe.id == "princess"){
				gm.AddCardToPlayer(gm.allCards.Find(x=>x.id == "magician"));
				gm.AddMessageToLog("A University Professor has declared himself the Court Magician.");
				magician = true;
				gm.sound.PlayAudio(gm.sound.bell);
				//}
			}
		}


	}




	public bool CheckDuelPossibility(Card briber, Card bribee){
		if(gm.targetReason == TargetReason.Bribe){
			print(briber.wealth+" "+bribee.wealth);
			if((briber.wealth-bribee.wealth) < 0){
				gm.AddMessageToLog("Need at least the target's amount of money to bribe.");
				return false;
			}
		}
		else if(gm.targetReason == TargetReason.Threaten){
			if(briber.fear <= bribee.fear){
				gm.AddMessageToLog("Can only threaten if you have more Fear than target.");
				return false;
			}
		}
		else if(gm.targetReason == TargetReason.Steal){
			if(briber.Status() <= bribee.Status()){
				gm.AddMessageToLog("Can only steal from those with less Status.");
				return false;
			}
			else if(bribee.wealth < 0){
				gm.AddMessageToLog("Target has to have something to steal.");
				return false;
			}
		}
		return true;
	}


	public void Bribe(Card briber, Card bribee){
		
		if(briber.wealth < bribee.wealth){
			gm.AddMessageToLog("Need at least the target's amount of money to bribe.");
			return;
		}
		else{
			print(briber.id +" BRIBING "+bribee.id);
			briber.wealth -= bribee.wealth;
			bribee.wealth += bribee.wealth;
			bribee.unruliness = 0;
			gm.sound.PlayAudio(gm.sound.duelBribe);
			gm.AddMessageToLog(briber.nam + " bribed "+bribee.nam + " with "+briber.wealth+".");
			StartCoroutine(DuelHold(briber,bribee));
		}
	}

	public void Threaten(Card briber, Card bribee){
		if(briber.fear < bribee.fear){
			gm.AddMessageToLog("Can only threaten if you have more Fear than target.");
			return;
		}
		else{
			print(briber.id +" THREATENING "+bribee.id);
			briber.fear += bribee.fear;
			bribee.fear -= bribee.fear;
			bribee.unruliness += 3;
			gm.sound.PlayAudio(gm.sound.duelThreaten);
			gm.AddMessageToLog(briber.nam + " threatened "+bribee.nam + ".");
			StartCoroutine(DuelHold(briber,bribee));
		}
	}

	public void Steal(Card briber, Card bribee){
		if(briber.Status() < bribee.Status()){
			gm.AddMessageToLog("Can only steal from those with less Status.");
			return;
		}
		else if(bribee.wealth < 0){
			gm.AddMessageToLog("Target has to have something to steal.");
			return;
		}
		else{
			print(briber.id +" STEALING FROM "+bribee.id);
			briber.wealth += bribee.wealth;
			bribee.wealth -= bribee.wealth;
			gm.sound.PlayAudio(gm.sound.duelSteal);
			gm.AddMessageToLog(briber.nam + " stole from "+bribee.nam + "!");
			StartCoroutine(DuelHold(briber,bribee));
		}
	}

	public void CancelDuel(){
		gm.inDuel = false;
		gm.playur.playerCard.transform.SetParent(gm.cardBar.transform); gm.playur.playerCard.transform.SetAsLastSibling();
		gm.playur.playerCard.gameObject.transform.position = gm.playur.playerCard.PosInBar;
		duelPanel.SetActive(false);
		gm.theButton.interactable = true;
	}

	public void ConcludeDuel(Card briber, Card bribee){
		gm.inDuel = false;
		briber.transform.SetParent(gm.cardBar.transform); briber.transform.SetAsLastSibling();
		bribee.transform.SetParent(gm.cardBar.transform); bribee.transform.SetAsLastSibling();
		briber.gameObject.transform.position = briber.PosInBar;
		bribee.gameObject.transform.position = bribee.PosInBar;
		duelPanel.SetActive(false);
		gm.theButton.interactable = true;
	}




	IEnumerator DuelHold(Card c, Card cc){
		hasDuelledthisTurn = true;
		cc.ShuffleAnimation();
		yield return new WaitForSeconds(0.4f);
		c.SetCardText();
		cc.SetCardText();
		yield return new WaitForSeconds(1.1f);
		ConcludeDuel(c, cc);
	}




	public Sprite GetSprite(Card c){
		return cardImgs[c.id];
	}

}

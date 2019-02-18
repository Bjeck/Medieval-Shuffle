using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class Utilities {
	

	public static float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}

	public static void Shuffle(){
		
	}

	private static System.Random rng = new System.Random();

	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}


	public static bool ShouldRevolt(Card c){
		int r = UnityEngine.Random.Range(0,11);
		if(c.unruliness > r){
			return true;
		}
		else{
			return false;
		}
	}






}
//HOW TO MAP X * NEW MAX / OLD MAX
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSameField : MonoBehaviour {

//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.Log ("hallo");
		print("Detected collision between " + gameObject.name + " and " + collisionInfo.collider.name);
		print("There are " + collisionInfo.contacts.Length + " point(s) of contacts");
		print("Their relative velocity is " + collisionInfo.relativeVelocity);
	}
}

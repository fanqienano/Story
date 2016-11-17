using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class SessionView : MonoBehaviour
{

	public GameObject session;
	public GameObject listView;

	private List<SessionInfo> sessionList;

	// Use this for initialization
	void Start ()
	{
		session.SetActive (false);
		sessionList = SessionInfo.getSessionList ();
		foreach (SessionInfo si in sessionList) {
			GameObject newSession = (GameObject)Instantiate (session);
			newSession.name = si.id.ToString();
			newSession.GetComponent<Button> ().onClick.AddListener(delegate() {
				this.clickButton (newSession); 
			});
			Text[] textList = newSession.GetComponentsInChildren<Text> ();
			Image[] imgList = newSession.GetComponentsInChildren<Image> ();
			foreach (Text t in textList) {
				if (t.tag == "SessionName") {
					t.text = si.name;
				} else if (t.tag == "SessionDesc") {
					t.text = si.desc;
				}
			}
			foreach (Image img in imgList) {
				if (img.tag == "SessionAvatar") {
					img.sprite = (Sprite)Resources.Load (si.avatar.Split ('.') [0], new Sprite ().GetType ());
				} else if (img.tag == "SessionActive"){
					if (si.isActive == 1) {
						img.gameObject.SetActive (true);
					} else {
						img.gameObject.SetActive (false);
					}
				}
			}
			if (si.isShow == 1) {
				newSession.SetActive (true);
			}
			newSession.transform.SetParent (listView.transform);
		}
	}

	private void clickButton(GameObject newSession){
		int sessionId = int.Parse (newSession.name);
		foreach(SessionInfo si in this.sessionList){
			if (si.id == sessionId) {
				Args.sessionId = sessionId;
				Application.LoadLevel("Dialog");
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

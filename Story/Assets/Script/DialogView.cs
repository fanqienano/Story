using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogView : MonoBehaviour
{

	public GameObject dialogItemText;
	public GameObject dialogItemImage_1;
	public GameObject dialogItemImage_2;
	public GameObject dialogItemVideo_1;
	public GameObject dialogItemVideo_2;
	public GameObject dialogItemVoice;
	public GameObject dialogItemSend;
	public GameObject dialogItemWait;
	public GameObject dialogItemInput;
	public GameObject bigImage;

	public GameObject listView;

	private AudioSource audioSource;

	private List<DialogInfo> dialogList;

	// Use this for initialization
	void Start ()
	{
		initObject ();
		loadHistory ();
		InvokeRepeating ("UpdateDialog", 0, 0.1f);
	}

	private void initObject ()
	{
		dialogItemText.SetActive (false);
		dialogItemImage_1.SetActive (false);
		dialogItemImage_2.SetActive (false);
		dialogItemVideo_1.SetActive (false);
		dialogItemVideo_2.SetActive (false);
		dialogItemVoice.SetActive (false);
		dialogItemSend.SetActive (false);
		dialogItemWait.SetActive (false);
		dialogItemInput.SetActive (false);
		bigImage.SetActive (false);
		bigImage.GetComponent<Button> ().onClick.AddListener (delegate() {
			this.clickBigImage ();
		});
	}

	private void loadHistory ()
	{
		List<DialogHistoryInfo> historyList = DialogHistoryInfo.getDialogHistoryList ();
		foreach (DialogHistoryInfo di in historyList) {
			if (di.type == "text") {
				showText (di);
			} else if (di.type == "voice") {
				showVoice (di);
			} else if (di.type == "video") {
				showVideo (di);
			} else if (di.type == "image") {
				showImage (di);
			} else if (di.type == "send") {
				showSend (di);
			}
		}
	}

	private void UpdateDialog ()
	{
	}

	private void showVideo(DialogInfo dialogInfo){
	}

	private void showVoice(DialogInfo dialogInfo){
		GameObject newDialog = (GameObject)Instantiate (dialogItemVoice);
		newDialog.GetComponentInChildren<Image> ().sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
		newDialog.name = dialogInfo.image.Split ('.') [0];
		newDialog.GetComponent<Button> ().onClick.AddListener (delegate() {
			this.clickVoice (newDialog);
		});
	}

	private void clickVoice (GameObject newDialog)
	{
		if (audioSource.isPlaying){
			audioSource.Stop();
		}
		audioSource.clip = (AudioClip)Resources.Load(newDialog.name, typeof(AudioClip));//调用Resources方法加载AudioClip资源
		audioSource.Play();
	}

	private void showSend(DialogInfo dialogInfo){
		GameObject newDialog = (GameObject)Instantiate (dialogItemSend);
		newDialog.GetComponentInChildren<Image> ().sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog.GetComponentInChildren<Text> ().text = dialogInfo.text;
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
	}

	private void showText (DialogInfo dialogInfo)
	{
		GameObject newDialog = (GameObject)Instantiate (dialogItemText);
		newDialog.GetComponentInChildren<Image> ().sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog.GetComponentInChildren<Text> ().text = dialogInfo.text;
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
	}

	private void showImage (DialogInfo dialogInfo)
	{
		GameObject newDialog_1 = (GameObject)Instantiate (dialogItemImage_1);
		GameObject newDialog_2 = (GameObject)Instantiate (dialogItemImage_2);
		newDialog_1.GetComponentInChildren<Image> ().sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog_2.GetComponentInChildren<Image> ().sprite = (Sprite)Resources.Load (dialogInfo.image.Split ('.') [0], new Sprite ().GetType ());
		newDialog_1.SetActive (true);
		newDialog_1.transform.SetParent (this.listView.transform);
		newDialog_2.SetActive (true);
		newDialog_2.transform.SetParent (this.listView.transform);
		newDialog_2.name = dialogInfo.image.Split ('.') [0];
		newDialog_2.GetComponentInChildren<Image> ().GetComponent<Button> ().onClick.AddListener (delegate() {
			this.clickImage (newDialog_2);
		});
	}

	private void clickImage (GameObject newDialog_2)
	{
		bigImage.GetComponent<Image> ().sprite = (Sprite)Resources.Load (newDialog_2.name, new Sprite ().GetType ());
		bigImage.SetActive (true);
	}

	private void clickBigImage ()
	{
		bigImage.SetActive (false);
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}

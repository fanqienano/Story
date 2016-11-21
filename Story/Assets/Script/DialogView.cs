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
	public GameObject dialogItemTextMore;
	public GameObject bigImage;

	public GameObject listView;

	public Button buttonLeft;
	public Button buttonRight;

	private AudioSource audioSource;

	private List<DialogInfo> dialogList;

	private SessionInfo sessionInfo;

	private DialogInfo dialogInfo;

	private bool waiting = false;

	// Use this for initialization
	void Start ()
	{
		initObject ();
		loadArchive ();
		loadHistory ();
		dialogInfo = DialogInfo.getDialogInfo (this.sessionInfo);
		InvokeRepeating ("UpdateDialog", 0, 1f);
	}

	private void UpdateDialog ()
	{
		long timeNow = Utils.dataTimeToLong (DateTime.Now);
		if (dialogInfo.activeTime == 0){
			dialogInfo.activeTime = timeNow + dialogInfo.delay;
		}
//		Debug.Log (dialogInfo.activeTime.ToString() + "|" + timeNow.ToString());
//		Debug.Log (dialogInfo.continueKey.ToString() + "|" + dialogInfo.continueValue.ToString());
		if (dialogInfo.activeTime <= timeNow && !this.waiting) {
			int value = getContinueValue (dialogInfo.continueKey);
			if (dialogInfo.continueValue == value) {
				this.showView (dialogInfo);
				dialogInfo = DialogInfo.getDialogInfo (this.sessionInfo.dialogScript, dialogInfo.id + 1);
			}
		}
	}

	private void initObject ()
	{
		dialogItemTextMore.SetActive (false);
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

	private void loadArchive(){
		int sessionId = Args.SessionId;
		sessionInfo = SessionInfo.getSessionInfo (sessionId);
		Debug.Log (sessionInfo.dialogScript);
		Debug.Log (sessionInfo.dialogId);
	}

	private int getContinueValue(int key){
		sessionInfo = SessionInfo.getSessionInfo (key);
		return sessionInfo.continueValue;
	}

	private void loadHistory ()
	{
		List<DialogHistoryInfo> historyList = DialogHistoryInfo.getDialogHistoryList (sessionInfo.dialogScript);
		foreach (DialogHistoryInfo di in historyList) {
			this.showHistoryView (di);
		}
	}

	private void showView(DialogInfo di){
		if (di.type == "text") {
			showText (di);
		} else if (di.type == "voice") {
			showVoice (di);
		} else if (di.type == "video") {
			showVideo (di);
		} else if (di.type == "image") {
			showImage (di);
		} else if (di.type == "option") {
			showOption (di);
		} else if (di.type == "input") {
			showInput (di);
		}
	}

	private void showHistoryView(DialogInfo di){
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

	private void showOption(DialogInfo dialogInfo){
//		this.waiting = true;
		GameObject newDialog = (GameObject)Instantiate (dialogItemWait);
		newDialog.GetComponentsInChildren<Image> ()[1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
		buttonLeft.GetComponentInChildren<Text> ().text = this.dialogInfo.option_1_text;
		buttonRight.GetComponentInChildren<Text> ().text = this.dialogInfo.option_2_text;
	}

	private void showInput(DialogInfo dialogInfo){
		GameObject newDialog = (GameObject)Instantiate (dialogItemInput);
		newDialog.GetComponentsInChildren<Image> ()[1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
	}

	private void showVideo(DialogInfo dialogInfo){
		GameObject newDialog_1 = (GameObject)Instantiate (dialogItemImage_1);
		GameObject newDialog_2 = (GameObject)Instantiate (dialogItemImage_2);
		newDialog_1.GetComponentsInChildren<Image> ()[1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog_2.GetComponentInChildren<Image> ().sprite = (Sprite)Resources.Load (dialogInfo.image.Split ('.') [0], new Sprite ().GetType ());
		newDialog_1.SetActive (true);
		newDialog_1.transform.SetParent (this.listView.transform);
		newDialog_2.SetActive (true);
		newDialog_2.transform.SetParent (this.listView.transform);
		newDialog_2.name = dialogInfo.video.Split ('.') [0];
		newDialog_2.GetComponentInChildren<Image> ().GetComponent<Button> ().onClick.AddListener (delegate() {
			this.clickVideo (newDialog_2);
		});
	}

	private void showVoice(DialogInfo dialogInfo){
		GameObject newDialog = (GameObject)Instantiate (dialogItemVoice);
		newDialog.GetComponentsInChildren<Image> ()[1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
		newDialog.name = dialogInfo.voice.Split ('.') [0];
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

	private void clickVideo(GameObject newDialog_2){
	}

	private void showSend(DialogInfo dialogInfo){
		GameObject newDialog = (GameObject)Instantiate (dialogItemSend);
		newDialog.GetComponentsInChildren<Image> ()[1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		this.autoShowText(newDialog.GetComponentInChildren<Text> (), dialogInfo.text);
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
	}

	private void showText (DialogInfo dialogInfo)
	{
		GameObject newDialog = (GameObject)Instantiate (dialogItemText);
		newDialog.GetComponentsInChildren<Image> ()[1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		this.autoShowText (newDialog.GetComponentInChildren<Text> (), dialogInfo.text);
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);

//		Debug.Log (newDialog.GetComponentInChildren<Text> ().transform.position.y);
//		Debug.Log (newDialog.GetComponentInChildren<Text> ().rectTransform.rect.height);
	}

	private void showImage (DialogInfo dialogInfo)
	{
		GameObject newDialog_1 = (GameObject)Instantiate (dialogItemImage_1);
		GameObject newDialog_2 = (GameObject)Instantiate (dialogItemImage_2);
		newDialog_1.GetComponentsInChildren<Image> ()[1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
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

	private void autoShowText(Text text, string content){
		text.text = "";
		float height = text.preferredHeight;
		text.text = dialogInfo.text + dialogInfo.text + dialogInfo.text;
		int lineCount = (int)(text.preferredHeight / height);
		float posY = 0 - (height / 2f) - 10;
		text.transform.position = new Vector3 (text.transform.position.x, posY);
		if (lineCount >= 3){
			int needMoreCount = Math.Ceiling((float)lineCount / 3f);
			for (int i = 0; i < needMoreCount; i++){
				GameObject newDialog = (GameObject)Instantiate (dialogItemTextMore);
				newDialog.SetActive (true);
				newDialog.transform.SetParent (this.listView.transform);
			}
		}
	}

}

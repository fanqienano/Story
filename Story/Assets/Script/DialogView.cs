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

	private DialogInfo optionDialogInfo;

	private bool waiting = false;

	private int itemCount = 0;

	private int baseItemCount = 10;

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
		if (dialogInfo.activeTime == 0) {
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

	private void loadArchive ()
	{
		int sessionId = Args.SessionId;
		sessionInfo = SessionInfo.getSessionInfo (sessionId);
		Debug.Log (sessionInfo.dialogScript);
		Debug.Log (sessionInfo.dialogId);
	}

	private int getContinueValue (int key)
	{
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

	private void showView (DialogInfo di)
	{
//		listView.transform.position = new Vector3(listView.transform.position.x, 99999);
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
		itemCount = listView.transform.childCount - baseItemCount;
//		listView.transform.position = new Vector3(listView.transform.position.x, 99999);
	}

	private void showHistoryView (DialogInfo di)
	{
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

	private void showOption (DialogInfo dialogInfo)
	{
		this.waiting = true;
		this.optionDialogInfo = this.dialogInfo;
		GameObject newDialog = (GameObject)Instantiate (dialogItemWait);
		newDialog.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
		buttonLeft.GetComponentInChildren<Text> ().text = this.dialogInfo.option_1_text;
		buttonLeft.GetComponent<Button> ().onClick.AddListener (delegate() {
			this.clickButton (newDialog, this.optionDialogInfo.option_1_script, this.optionDialogInfo.option_1_id, this.optionDialogInfo.option_1_text);
		});
		buttonRight.GetComponentInChildren<Text> ().text = this.dialogInfo.option_2_text;
		buttonRight.GetComponent<Button> ().onClick.AddListener (delegate() {
			this.clickButton (newDialog, this.optionDialogInfo.option_2_script, this.optionDialogInfo.option_2_id, this.optionDialogInfo.option_2_text);
		});
	}

	private void showInput (DialogInfo dialogInfo)
	{
		GameObject newDialog = (GameObject)Instantiate (dialogItemInput);
		newDialog.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
	}

	private void showVideo (DialogInfo dialogInfo)
	{
		GameObject newDialog_1 = (GameObject)Instantiate (dialogItemImage_1);
		GameObject newDialog_2 = (GameObject)Instantiate (dialogItemImage_2);
		newDialog_1.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
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

	private void showVoice (DialogInfo dialogInfo)
	{
		GameObject newDialog = (GameObject)Instantiate (dialogItemVoice);
		newDialog.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
		newDialog.name = dialogInfo.voice.Split ('.') [0];
		newDialog.GetComponent<Button> ().onClick.AddListener (delegate() {
			this.clickVoice (newDialog);
		});
	}

	private void clickVoice (GameObject newDialog)
	{
		if (audioSource.isPlaying) {
			audioSource.Stop ();
		}
		audioSource.clip = (AudioClip)Resources.Load (newDialog.name, typeof(AudioClip));//调用Resources方法加载AudioClip资源
		audioSource.Play ();
	}

	private void clickVideo (GameObject newDialog_2)
	{
	}

	private void showSend (DialogInfo dialogInfo)
	{
		GameObject newDialog = (GameObject)Instantiate (dialogItemSend);
		newDialog.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
		this.autoShowText (newDialog, dialogInfo.text);
	}

	private void showText (DialogInfo dialogInfo)
	{
		GameObject newDialog = (GameObject)Instantiate (dialogItemText);
		newDialog.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog.SetActive (true);
		newDialog.transform.SetParent (this.listView.transform);
		this.autoShowText (newDialog, dialogInfo.text);

//		Debug.Log (newDialog.GetComponentInChildren<Text> ().transform.position.y);
//		Debug.Log (newDialog.GetComponentInChildren<Text> ().rectTransform.rect.height);
	}

	private void showImage (DialogInfo dialogInfo)
	{
		GameObject newDialog_1 = (GameObject)Instantiate (dialogItemImage_1);
		GameObject newDialog_2 = (GameObject)Instantiate (dialogItemImage_2);
		newDialog_1.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
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

	private void clickButton(GameObject newDialog, string script, int id, string content){
//		text.text = content;
		this.autoSendText(newDialog, content);
		this.waiting = false;
		buttonLeft.GetComponent<Button> ().onClick.RemoveAllListeners ();
		buttonRight.GetComponent<Button> ().onClick.RemoveAllListeners ();
	}

	// Update is called once per frame
	void Update ()
	{
	
	}

	private void autoShowText (GameObject gameObj, string content)
	{
		Image img = gameObj.GetComponentsInChildren<Image> () [3];
		Text text = gameObj.GetComponentInChildren<Text> ();
		text.text = "阿";
		float height = text.preferredHeight;
		float width = text.preferredWidth;
		text.text = content;
		int lineCount = (int)(text.preferredHeight / height);
		if (lineCount > 1) {
			width = width * 7;
		} else {
			width = text.preferredWidth;
		}
		float posY = 0 - (height / 2f) - 10;
		img.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width + 20f, text.preferredHeight + 20f);
		if (lineCount >= 3) {
			lineCount = lineCount - 2;
			int needMoreCount = (int)Math.Ceiling ((float)lineCount / 3f);
//			if (needMoreCount == 0) {
//				needMoreCount = 1;
//			}
			for (int i = 0; i < needMoreCount; i++) {
				GameObject newDialog = (GameObject)Instantiate (dialogItemTextMore);
				newDialog.SetActive (true);
				newDialog.transform.SetParent (this.listView.transform);
			}
		}
	}


	private void autoSendText (GameObject gameObj, string content)
	{
		Image img = gameObj.GetComponentsInChildren<Image> () [3];
		Text text = gameObj.GetComponentInChildren<Text> ();
		text.text = "阿";
		float height = text.preferredHeight;
		float width = text.preferredWidth;
		text.text = content;
		int lineCount = (int)(text.preferredHeight / height);
		if (lineCount > 1) {
			width = width * 7;
		} else {
			text.alignment = TextAnchor.UpperLeft;
			width = text.preferredWidth;
		}
		float posY = 0 - (height / 2f) - 10;
		img.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width + 20f, text.preferredHeight + 20f);
		if (lineCount >= 3) {
			lineCount = lineCount - 2;
			int needMoreCount = (int)Math.Ceiling ((float)lineCount / 3f);
			//			if (needMoreCount == 0) {
			//				needMoreCount = 1;
			//			}
			for (int i = 0; i < needMoreCount; i++) {
				GameObject newDialog = (GameObject)Instantiate (dialogItemTextMore);
				newDialog.SetActive (true);
				newDialog.transform.SetParent (this.listView.transform);
			}
		}
	}

}

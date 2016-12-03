using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;

public class DialogView : MonoBehaviour
{

	public List<GameObject> dialogItemText = new List<GameObject> ();
	public List<GameObject> dialogItemImage = new List<GameObject> ();
	public List<GameObject> dialogItemVideo = new List<GameObject> ();
	public List<GameObject> dialogItemVoice = new List<GameObject> ();
	public List<GameObject> dialogItemSend = new List<GameObject> ();
	public List<GameObject> dialogItemWait = new List<GameObject> ();
	public List<GameObject> dialogItemInput = new List<GameObject> ();
	public GameObject dialogItemMore;
	public GameObject bigImage;

	public Scrollbar scrollBar;

	public bool isScroll = false;

	public GameObject listView;

	public Button buttonLeft;
	public Button buttonRight;

	private AudioSource audioSource;

	private List<DialogInfo> dialogList;

	private SessionInfo sessionInfo;

	private DialogInfo dialogInfo;

	private DialogInfo optionDialogInfo;

	private bool waiting = false;

	private float listViewSize = 0;

	private int baseItemCount = 10;

	private int baseItemSize = 10; //标准item行数；

	// Use this for initialization
	void Start ()
	{
		initObject ();
		loadArchive ();
		loadHistory ();
		dialogInfo = DialogInfo.getDialogInfo (this.sessionInfo);
		InvokeRepeating ("UpdateDialog", 0, 0.1f);
		InvokeRepeating ("AutoScroll", 0, 0.05f);
	}

	private void UpdateDialog ()
	{
		long timeNow = Utils.dataTimeToLong (DateTime.Now);
		if (dialogInfo.activeTime == 0) {
			dialogInfo.activeTime = timeNow + dialogInfo.delay;
		}
//		Debug.Log (dialogInfo.activeTime.ToString() + "|" + timeNow.ToString());
//		Debug.Log (dialogInfo.continueKey.ToString() + "|" + dialogInfo.continueValue.ToString());
		if (!this.waiting) {
			this.isScroll = true;
			if (dialogInfo.activeTime <= timeNow) {
				int value = getContinueValue (dialogInfo.continueKey);
				if (dialogInfo.continueValue == value) {
					this.showView (dialogInfo);
					dialogInfo = DialogInfo.getDialogInfo (this.sessionInfo.dialogScript, dialogInfo.id + 1);
				}
			}
		}
	}

	private void AutoScroll(){
		if (this.isScroll) {
			scrollBar.value = scrollBar.value - 0.1f;
		}
		if (scrollBar.value == 0) {
			this.isScroll = false;
		}
	}

	private void initObject ()
	{
		dialogItemMore.SetActive (false);
		initGameObj (dialogItemText);
		initGameObj (dialogItemImage);
		initGameObj (dialogItemVideo);
		initGameObj (dialogItemVoice);
		initGameObj (dialogItemSend);
		initGameObj (dialogItemWait);
		initGameObj (dialogItemInput);
		bigImage.SetActive (false);
		bigImage.GetComponent<Button> ().onClick.AddListener (delegate() {
			this.clickBigImage ();
		});
	}

	private void initGameObj(List<GameObject> objList){
		int size = this.baseItemSize - objList.Count;
		for (int i = 0; i < size; i++) {
			GameObject c = (GameObject)Instantiate (dialogItemMore);
			objList.Add (c);
		}
		foreach (GameObject i in objList){
			i.SetActive (false);
		}
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
//		Debug.Log(di.GetType().GetProperties()[0].Name);
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
		List<GameObject> newGameObjList = this.CopyGameObject (dialogItemWait);
		GameObject newDialog = newGameObjList[0];
		newDialog.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		this.Show (newGameObjList);
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
		List<GameObject> newGameObjList = this.CopyGameObject (dialogItemInput);
		GameObject newDialog = newGameObjList[0];
		newDialog.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		this.Show (newGameObjList);
	}

	private void showVideo (DialogInfo dialogInfo)
	{
		List<GameObject> newGameObjList = this.CopyGameObject (dialogItemImage);
		GameObject newDialog_1 = newGameObjList[0];
		GameObject newDialog_2 = newGameObjList[1];
		newDialog_1.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog_2.GetComponentInChildren<Image> ().sprite = (Sprite)Resources.Load (dialogInfo.image.Split ('.') [0], new Sprite ().GetType ());
		this.Show (newGameObjList);
		newDialog_2.name = dialogInfo.video.Split ('.') [0];
		newDialog_2.GetComponentInChildren<Image> ().GetComponent<Button> ().onClick.AddListener (delegate() {
			this.clickVideo (newDialog_2);
		});
	}

	private void showVoice (DialogInfo dialogInfo)
	{
		List<GameObject> newGameObjList = this.CopyGameObject (dialogItemVoice);
		GameObject newDialog = newGameObjList[0];
		newDialog.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		this.Show (newGameObjList);
		newDialog.name = dialogInfo.voice.Split ('.') [0];
		newDialog.GetComponent<Button> ().onClick.AddListener (delegate() {
			this.clickVoice (newDialog);
		});
	}

	private void showSend (DialogInfo dialogInfo)
	{
		List<GameObject> newGameObjList = this.CopyGameObject (dialogItemSend);
		GameObject newDialog = newGameObjList[0];
		newDialog.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		this.Show (newGameObjList);
		this.autoShowText (newDialog, dialogInfo.text);
	}

	private void showText (DialogInfo dialogInfo)
	{
		List<GameObject> newGameObjList = this.CopyGameObject (dialogItemText);
		GameObject newDialog = newGameObjList[0];
		newDialog.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		this.Show (newGameObjList);
		this.autoShowText (newDialog, dialogInfo.text);
	}

	private void showImage (DialogInfo dialogInfo)
	{
		List<GameObject> newGameObjList = this.CopyGameObject (dialogItemImage);
		GameObject newDialog_1 = newGameObjList[0];
		GameObject newDialog_2 = newGameObjList[1];
		newDialog_1.GetComponentsInChildren<Image> () [1].sprite = (Sprite)Resources.Load (dialogInfo.avatar.Split ('.') [0], new Sprite ().GetType ());
		newDialog_2.GetComponentInChildren<Image> ().sprite = (Sprite)Resources.Load (dialogInfo.image.Split ('.') [0], new Sprite ().GetType ());
		this.Show (newGameObjList);
		newDialog_2.name = dialogInfo.image.Split ('.') [0];
		newDialog_2.GetComponentInChildren<Image> ().GetComponent<Button> ().onClick.AddListener (delegate() {
			this.clickImage (newDialog_2);
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
		img.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width + 20f, text.preferredHeight + 20f);
		if (lineCount > 1) {
			int needMoreCount = lineCount - 1;
			needMoreCount = needMoreCount * 2;
//			lineCount = lineCount - 2;
//			int needMoreCount = (int)Math.Ceiling ((float)lineCount / 3f);
//			if (needMoreCount == 0) {
//				needMoreCount = 1;
//			}
			for (int i = 0; i < needMoreCount; i++) {
				GameObject newDialog = (GameObject)Instantiate (dialogItemMore);
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
			text.alignment = TextAnchor.UpperRight;
			width = text.preferredWidth;
		}
		img.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width + 20f, text.preferredHeight + 20f);
		if (lineCount > 1) {
			int needMoreCount = lineCount - 1;
			needMoreCount = needMoreCount * 2;
//			lineCount = lineCount - 2;
//			int needMoreCount = (int)Math.Ceiling ((float)lineCount / 3f);
			//			if (needMoreCount == 0) {
			//				needMoreCount = 1;
			//			}
			for (int i = 0; i < needMoreCount; i++) {
				GameObject newDialog = (GameObject)Instantiate (dialogItemMore);
				newDialog.SetActive (true);
				newDialog.transform.SetParent (this.listView.transform);
			}
		}
	}

	private void SetActive(List<GameObject> gameObjList, bool status){
		foreach (GameObject i in gameObjList){
			i.SetActive (status);
		}
	}

	private void SetParent(List<GameObject> gameObjList, Transform transform){
		foreach (GameObject i in gameObjList){
			i.transform.SetParent (transform);
		}
	}

	private List<GameObject> CopyGameObject(List<GameObject> gameObjList){
		List<GameObject> copyList = new List<GameObject> ();
		foreach (GameObject i in gameObjList){
			GameObject c = (GameObject)Instantiate (i);
			copyList.Add (c);
		}
		return copyList;
	}

	private void Show(List<GameObject> gameObjList){
		SetActive (gameObjList, true);
		SetParent (gameObjList, this.listView.transform);
	}
}

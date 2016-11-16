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

	private List<DialogInfo> dialogList;

	// Use this for initialization
	void Start ()
	{
		initObject ();
//		bigImage.GetComponent<Image> ().sprite = (Sprite)Resources.Load ("59649598_p0", new Sprite ().GetType());
//		bigImage.GetComponent<Image> ().type = Image.Type.Tiled;
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

	private void UpdateDialog ()
	{
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
		newDialog_2.GetComponentInChildren<Image> ().GetComponent<Button> ();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using MAINVIEW;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EventController : MonoBehaviour {

	[SerializeField]
	UIController uiContorller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void requestPDFData(string _bookId)
	{
		StartCoroutine(requestPDF("http://160.16.196.104:10427/Book/getdocument", _bookId));
	}

	IEnumerator requestPDF (string _url, string _bookId) {

		WWWForm form = new WWWForm ();
		form.AddField ("book_id", _bookId);
		WWW www = new WWW (_url, form);
	
		while (!www.isDone) { // ダウンロードの進捗を表示
			print (Mathf.CeilToInt (www.progress * 100));
			uiContorller.onUpdateProgress (www.progress);
			yield return null;
		}

		if (!string.IsNullOrEmpty(www.error)) { // ダウンロードでエラーが発生した
			print(www.error);
		}  else { // ダウンロードが正常に完了した
			string path = Path.Combine (Application.persistentDataPath, "Book.pdf");
			File.WriteAllBytes(path, www.bytes);
			openPlugin(path);
		}

		uiContorller.onPickUI ();
	}

	public static void openPlugin(string path)
	{
		Debug.Log(path);
		 if (!Application.isEditor)
	        {
				//block to open the file and share it----------START
	            //AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
	           //AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
	           // intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));
	            //AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
	            //AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + path);
	            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
	            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");
	            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "text");
				//intentObject.Call<AndroidJavaObject>("setType", "Application/pdf");
	            //AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	            //AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
				//currentActivity.Call("startActivity", intentObject);
				Application.OpenURL(path);
       		}
	}

		
}

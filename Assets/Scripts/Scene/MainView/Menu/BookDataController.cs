using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MAINVIEW;

namespace BOOK {

	[Serializable]
	public class RequestItem
	{
		public string status;
		public string error;
		//public string param;
		public BookItem param;
	}

	[Serializable]
	public class BookItem
	{
		public int book_id;
		public string name;
	}

	public class BookDataController : MonoBehaviour {

		[SerializeField]
		EventController eventController;

		[SerializeField]
		UIController uiController;

		private bool bReqeust = false;
		private RequestItem cItem;

		public void requestData(string _bookId)
		{
			if (bReqeust == true)
				return;
			
			bReqeust = true;
			
			int bookId = 0;

			int.TryParse (_bookId, out bookId);

			string requstId = bookId.ToString ();
		
			StartCoroutine (RequestPost ("http://160.16.196.104:10427/Book/searchbook", requstId));
		}

		private void dataConvert(string _json)
		{
			cItem = JsonUtility.FromJson<RequestItem>(_json);
			eventController.requestPDFData (cItem.param.book_id.ToString ());
		}

		IEnumerator RequestPost(string _url, string _bookId)
		{
			WWWForm form = new WWWForm ();
			form.AddField ("book_id", _bookId);
			WWW www = new WWW (_url, form);
			yield return www;

			if (www.error != null) {
				Debug.Log (www.error);
				Debug.Log ("BookDataConteroller->Error "+www.error);
			} else {
				dataConvert (www.text);
			}

			bReqeust = false;
		}
	}

}

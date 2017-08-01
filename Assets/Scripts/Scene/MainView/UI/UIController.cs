using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MAINVIEW
{
	public enum UI_TYPE
	{
		NORMAL,
		DOWNLOAD,
		PICK
	}
	
	public class UIController : MonoBehaviour {

		[SerializeField]
		GameObject normalUIObj;

		[SerializeField]
		GameObject pickUIObj;

		[SerializeField]
		GameObject progressObj;

		[SerializeField]
		Image progressImage;

		UI_TYPE uiType = UI_TYPE.NORMAL;

		// Use this for initialization
		void Start () {
			updateUI ();
		}

		// Update is called once per frame
		void Update () {

		}

		private void updateUI()
		{
			normalUIObj.SetActive (uiType == UI_TYPE.NORMAL);
			progressObj.SetActive (uiType == UI_TYPE.DOWNLOAD);
			pickUIObj.SetActive (uiType == UI_TYPE.PICK);
		}

		public void onNomralUI()
		{
			uiType = UI_TYPE.NORMAL;
			updateUI ();
		}

		public void onDonwloadUI()
		{
			uiType = UI_TYPE.DOWNLOAD;
			progressImage.fillAmount = 0;
			updateUI ();
		}

		public void onPickUI()
		{
			uiType = UI_TYPE.PICK;
			updateUI ();
		}

		public void onUpdateProgress(float _progress)
		{
			progressImage.fillAmount = _progress;
		}
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MAINVIEW
{
	public class MenuController : MonoBehaviour {

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public void onClickAuth()
		{
			SceneManager.LoadScene ("AuthView", LoadSceneMode.Single);
		}

		public void onClickSetting()
		{
			SceneManager.LoadScene ("SettingView", LoadSceneMode.Single);
		}
	}

}

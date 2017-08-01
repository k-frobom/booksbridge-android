using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SETTINGVIEW
{
	public class MenuController : MonoBehaviour {

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public void onCloseClick()
		{
			SceneManager.LoadScene ("Main", LoadSceneMode.Single);
		}
	}

}


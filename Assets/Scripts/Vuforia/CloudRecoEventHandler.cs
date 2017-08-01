using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using BOOK;
using MAINVIEW;
using UnityEngine.UI;

public class CloudRecoEventHandler : MonoBehaviour, ICloudRecoEventHandler{

	[SerializeField]
	BookDataController bookDataController;

	[SerializeField]
	UIController uiController;

	private ObjectTracker mObjectTracker;
	private CloudRecoBehaviour mCloudRecoBehaviour;
	private GameObject mParentOfImageTargetTemplate;

	public ImageTargetBehaviour ImageTargetTemplate;
	public ScanLine scanLine;

	/// <summary>
	/// called when TargetFinder has been initialized successfully
	/// </summary>
	public void OnInitialized()
	{
		// get a reference to the Object Tracker, remember it
		mObjectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
	}

	// <summary>
	/// visualize initialization errors
	/// </summary>
	public void OnInitError(TargetFinder.InitState initError)
	{
		switch (initError)
		{
		case TargetFinder.InitState.INIT_ERROR_NO_NETWORK_CONNECTION:
			Debug.Log ("NetWorkError");
			break;
		case TargetFinder.InitState.INIT_ERROR_SERVICE_NOT_AVAILABLE:
			Debug.Log ("ServerError");
			break;
		}
	}

	// <summary>
	/// visualize update errors
	/// </summary>
	public void OnUpdateError(TargetFinder.UpdateState updateError)
	{
		switch (updateError)
		{
		case TargetFinder.UpdateState.UPDATE_ERROR_AUTHORIZATION_FAILED:
			Debug.Log("Authorization Error The cloud recognition service access keys are incorrect or have expired.");
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_NO_NETWORK_CONNECTION:
			Debug.Log("Network Unavailable Please check your internet connection and try again.");
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_PROJECT_SUSPENDED:
			Debug.Log("Authorization Error The cloud recognition service has been suspended.");
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_REQUEST_TIMEOUT:
			Debug.Log("Request Timeout The network request has timed out, please check your internet connection and try again.");
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_SERVICE_NOT_AVAILABLE:
			Debug.Log("Service Unavailable The service is unavailable, please try again later.");
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_TIMESTAMP_OUT_OF_RANGE:
			Debug.Log("Clock Sync Error Please update the date and time and try again.");
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_UPDATE_SDK:
			Debug.Log("Unsupported Version The application is using an unsupported version of Vuforia.");
			break;
		}
	}

	// <summary>
	/// when we start scanning, unregister Trackable from the ImageTargetTemplate, then delete all trackables
	/// </summary>
	public void OnStateChanged(bool scanning)
	{
		if (scanning)
		{
			// clear all known trackables
			mObjectTracker.TargetFinder.ClearTrackables(false);

			mCloudRecoBehaviour.CloudRecoEnabled = true;

			uiController.onNomralUI ();
		}

		ShowScanLine(scanning);
	}

	/// <summary>
	/// Handles new search results
	/// </summary>
	/// <param name="targetSearchResult"></param>
	public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
	{
		// This code demonstrates how to reuse an ImageTargetBehaviour for new search results and modifying it according to the metadata
		// Depending on your application, it can make more sense to duplicate the ImageTargetBehaviour using Instantiate(), 
		// or to create a new ImageTargetBehaviour for each new result

		// Vuforia will return a new object with the right script automatically if you use
		// TargetFinder.EnableTracking(TargetSearchResult result, string gameObjectName)
	
		// Check if the metadata isn't null
		if (targetSearchResult.MetaData == null)
		{
			return;
		}

		// Enable the new result with the same ImageTargetBehaviour:
		ImageTargetBehaviour imageTargetBehaviour = mObjectTracker.TargetFinder.EnableTracking(targetSearchResult, mParentOfImageTargetTemplate) as ImageTargetBehaviour;

		if (imageTargetBehaviour != null)
		{
			Debug.Log ("OnNewSearchResult");
			
			// Stop the target finder
			mCloudRecoBehaviour.CloudRecoEnabled = false;

			// Stop showing the scan-line
			ShowScanLine(false);

			bookDataController.requestData (targetSearchResult.TargetName);

			uiController.onDonwloadUI ();

			// Calls the TargetCreated Method of the SceneManager object to start loading
			// the BookData from the JSON
			//mContentManager.TargetCreated(targetSearchResult.MetaData);
			//mContentManager.AnimationsManager.SetInitialAnimationFlags();
		}
	}

	void Start()
	{
		// Look up the gameobject containing the ImageTargetTemplate:
		mParentOfImageTargetTemplate = ImageTargetTemplate.gameObject;

		// Register this event handler at the cloud reco behaviour
		CloudRecoBehaviour cloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
		if (cloudRecoBehaviour)
		{
			cloudRecoBehaviour.RegisterEventHandler(this);
		}

		// Remember cloudRecoBehaviour for later
		mCloudRecoBehaviour = cloudRecoBehaviour;
	}

	void Update()
	{
		if (mCloudRecoBehaviour.CloudRecoInitialized)
		{
		}
	}

	private void ShowScanLine(bool show)
	{
		// Toggle scanline rendering
		if (scanLine != null)
		{
			Renderer scanLineRenderer = scanLine.GetComponent<Renderer>();
			if (show)
			{
				// Enable scan line rendering
				if (!scanLineRenderer.enabled)
					scanLineRenderer.enabled = true;

				scanLine.ResetAnimation();
			}
			else
			{
				// Disable scanline rendering
				if (scanLineRenderer.enabled)
					scanLineRenderer.enabled = false;
			}
		}
	}
}

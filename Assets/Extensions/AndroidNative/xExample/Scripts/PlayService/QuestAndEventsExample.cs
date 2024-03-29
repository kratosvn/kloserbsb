﻿using UnityEngine;
using System.Collections;

public class QuestAndEventsExample : MonoBehaviour {
	


	public GameObject avatar;
	private Texture defaulttexture;
	public Texture2D pieIcon;
	
	public DefaultPreviewButton connectButton;
	public SA_Label playerLabel;
	
	public DefaultPreviewButton[] ConnectionDependedntButtons;



	//example, replase with your ID
	private const string EVENT_ID = "CgkIipfs2qcGEAIQDQ";

	//example, replase with your ID
	private const string QUEST_ID = "CgkIipfs2qcGEAIQDw";


	


	void Start() {
		
		playerLabel.text = "Player Diconnected";
		defaulttexture = avatar.GetComponent<Renderer>().material.mainTexture;
		
		//listen for GooglePlayConnection events
		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
		GooglePlayConnection.instance.addEventListener(GooglePlayConnection.CONNECTION_RESULT_RECEIVED, OnConnectionResult);
		
		
		//listen for events, we will use action in this example
		GooglePlayEvents.instance.OnEventsLoaded += OnEventsLoaded;

		GooglePlayQuests.instance.OnQuestsAccepted += OnQuestsAccepted;
		GooglePlayQuests.instance.OnQuestsCompleted += OnQuestsCompleted;
		GooglePlayQuests.instance.OnQuestsLoaded += OnQuestsLoaded;
		
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			//checking if player already connected
			OnPlayerConnected ();
		} 
		
	}
	
	private void ConncetButtonPress() {
		Debug.Log("GooglePlayManager State  -> " + GooglePlayConnection.state.ToString());
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			SA_StatusBar.text = "Disconnecting from Play Service...";
			GooglePlayConnection.instance.disconnect ();
		} else {
			SA_StatusBar.text = "Connecting to Play Service...";
			GooglePlayConnection.instance.connect ();
		}
	}
	

	
	

	
	void FixedUpdate() {
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			if(GooglePlayManager.instance.player.icon != null) {
				avatar.GetComponent<Renderer>().material.mainTexture = GooglePlayManager.instance.player.icon;
			}
		}  else {
			avatar.GetComponent<Renderer>().material.mainTexture = defaulttexture;
		}
		
		
		string title = "Connect";
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			title = "Disconnect";
			
			foreach(DefaultPreviewButton btn in ConnectionDependedntButtons) {
				btn.EnabledButton();
			}
			
			
		} else {
			foreach(DefaultPreviewButton btn in ConnectionDependedntButtons) {
				btn.DisabledButton();
				
			}
			if(GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED || GooglePlayConnection.state == GPConnectionState.STATE_UNCONFIGURED) {
				
				title = "Connect";
			} else {
				title = "Connecting..";
			}
		}
		
		connectButton.text = title;
	}


	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------

	public void LoadEvents() {
		GooglePlayEvents.instance.LoadEvents();
	}

	public void IncrementEvent() {
		GooglePlayEvents.instance.SumbitEvent(EVENT_ID);
	}
	


	public void ShowAllQuests() {
		GooglePlayQuests.instance.ShowQuests();
	}

	public void ShowAcceptedQuests() {
		GooglePlayQuests.instance.ShowQuests(GP_QuestsSelect.SELECT_ACCEPTED);
	}

	public void ShowCompletedQuests() {
		GooglePlayQuests.instance.ShowQuests(GP_QuestsSelect.SELECT_COMPLETED);
	}

	public void ShowOpenQuests() {
		GooglePlayQuests.instance.ShowQuests(GP_QuestsSelect.SELECT_OPEN);
	}

	public void AcceptQuest() {
		GooglePlayQuests.instance.AcceptQuest(QUEST_ID);
	}

	public void LoadQuests() {
		GooglePlayQuests.instance.LoadQuests(GP_QuestSortOrder.SORT_ORDER_ENDING_SOON_FIRST);
	}





	//--------------------------------------
	// EVENTS
	//--------------------------------------

	private void OnEventsLoaded (GooglePlayResult result) {
		AndroidNative.showMessage ("Events Loaded", "Total Events: " + GooglePlayEvents.instance.Events.Count);
		SA_StatusBar.text = "OnEventsLoaded:  " + result.response.ToString();
	}

	private void OnQuestsAccepted (GP_QuestResult result) {
		AndroidNative.showMessage ("On Quests Accepted", "Quests Accepted, ID: " + result.questId);

		SA_StatusBar.text = "OnQuestsAccepted:  " + result.response.ToString();
	}

	private void OnQuestsCompleted (GP_QuestResult result) {
		AndroidNative.showMessage ("On Quests Completed", "Quests Completed, Reward: " + result.reward);

		SA_StatusBar.text = "OnQuestsCompleted:  " + result.response.ToString();
	}

	private void OnQuestsLoaded (GooglePlayResult result) {
		AndroidNative.showMessage ("Quests Loaded", "Total Quests: " + GooglePlayQuests.instance.GetQuests().Count);

		SA_StatusBar.text = "OnQuestsLoaded:  " + result.response.ToString();
	}
	
	private void OnPlayerDisconnected() {
		SA_StatusBar.text = "Player Diconnected";
		playerLabel.text = "Player Diconnected";
	}
	
	private void OnPlayerConnected() {
		SA_StatusBar.text = "Player Connected";
		playerLabel.text = GooglePlayManager.instance.player.name;
	}
	
	private void OnConnectionResult(CEvent e) {
		
		GooglePlayConnectionResult result = e.data as GooglePlayConnectionResult;
		SA_StatusBar.text = "ConnectionResul:  " + result.code.ToString();
		Debug.Log(result.code.ToString());
	}
	

	
	void OnDestroy() {
		if(!GooglePlayConnection.IsDestroyed) {
			GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
			GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
			
		}
		
	}

}

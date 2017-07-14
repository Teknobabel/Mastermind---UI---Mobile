using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IApp {

	string Name { get; set;}
	Sprite Icon { get; set; }
	Sprite Icon_Pressed { get; set; }
	List<GameObject> MenuBank { get;}
	bool WantsSystemNavBar { get; }
	AppIcon AppIconInstance { set;}

	void InitializeApp ();

	void EnterApp ();

	void UpdateApp ();

	void ExitApp ();

	void HoldApp ();

	void AppReturn ();

	void PushMenu (IMenu menu);

	void PopMenu ();

	void SetAlerts ();
}

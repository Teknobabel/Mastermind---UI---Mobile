using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenu  {

	IApp ParentApp { get;}

	void Initialize (IApp parentApp);

	void OnEnter (bool animate);

	void OnExit (bool animate);

	void OnHold ();

	void OnReturn ();
}

using UnityEngine;
using System.Collections;

public interface IUIObserver {

	void OnNotify (IUISubject subject, UIEvent thisUIEvent);
}

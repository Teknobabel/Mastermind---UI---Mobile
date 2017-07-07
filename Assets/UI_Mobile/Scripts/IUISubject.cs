using UnityEngine;
using System.Collections;

public interface IUISubject{

	void AddObserver (IUIObserver observer);

	void RemoveObserver (IUIObserver observer);

	void Notify (IUISubject subject, UIEvent thisUIEvent);
}

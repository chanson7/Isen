using UnityEngine;
using UnityEngine.Events;

public class ScriptableEventListener : MonoBehaviour
{
    public ScriptableEvent scriptableEvent;
    public UnityEvent response;

    private void OnEnable()
    {
        scriptableEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        scriptableEvent.UnregisterListener(this);
    }

    public void OnEventRaised() { response.Invoke(); }

}

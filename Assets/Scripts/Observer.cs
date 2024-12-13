using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer
{
    public delegate void ListenerFunction(Actor actor,System.Enum eventType,object param);

    private List<ListenerFunction> listeners = new List<ListenerFunction>();
    public void Notify(Actor actor, System.Enum eventType, object e)
    {
        for(int i=0;i<listeners.Count;i++) 
        {
            listeners[i].Invoke(actor, eventType,e);
        }
    }

    public void addListener(ListenerFunction listener)
    {
        listeners.Add(listener);
    }

    public void removeListener(ListenerFunction listener)
    {
        listeners.Remove(listener);
    }



}

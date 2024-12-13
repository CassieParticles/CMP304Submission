using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer
{
    public delegate void ListenerFunction(object direction);

    private List<ListenerFunction> listeners = new List<ListenerFunction>();
    public void Notify(object e)
    {
        for(int i=0;i<listeners.Count;i++) 
        {
            listeners[i].Invoke(e);
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

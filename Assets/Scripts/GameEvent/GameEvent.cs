using System;
using System.Collections.Generic;
using UnityEngine;





[CreateAssetMenu(menuName = "Events/Game Event")]
public class GameEvent : ScriptableObject
{
    [SerializeField] [TextArea] private string description;
    public List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise(Component _sender,object _data)
    {
           foreach (var t in listeners)
           {
               t.OnEventRised(_sender, _data);
           }

    }

    public void RegisterListener(GameEventListener _gameEventListener)
    {
      if(!listeners.Contains(_gameEventListener))
          {
              listeners.Add(_gameEventListener);
          }
       
    }

    public void UnregisterListener(GameEventListener _gameEventListener)
    {
         if (listeners.Contains(_gameEventListener))
          {
              listeners.Remove(_gameEventListener);
          }  

     
    }







}

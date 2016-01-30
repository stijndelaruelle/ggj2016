using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sjabloon;

public class Vehicle : MonoBehaviour, InteractableObject
{
    [SerializeField]
    protected CharacterController2D m_CharacterController;

    [SerializeField]
    protected float m_Speed;

    [SerializeField]
    protected List<Player> m_RequiredPlayers;
    protected List<Player> m_Passengers;
    protected bool m_IsDriving = false;

    protected virtual void Start()
    {
        GameManager.Instance.StartDayEvent += OnStartDay;
        m_Passengers = new List<Player>();
    }

    protected virtual void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.StartDayEvent -= OnStartDay;
    }

    public bool CanInteract(Player player)
    {
        //We can interact as long as we're not driving
        return (!m_IsDriving);
    }

    public bool IsInteracting(Player player)
    {
        return false;
    }

    public void Interact(Player player)
    {
        if (m_IsDriving)
            return;

        if (m_Passengers.Contains(player))
        {
            m_Passengers.Remove(player);
            player.UpdateVehicle(null);
        }
        else
        {
            m_Passengers.Add(player);
            player.UpdateVehicle(this);

            //Check if the vehicle can drive
            if (CanDrive())
            {
                m_IsDriving = true;
            }
        }
    }

    protected virtual void Update()
    {
        if (m_IsDriving == false)
            return;

        m_CharacterController.Move(m_Speed, 0.0f);
    }

    protected virtual bool CanDrive()
    {
        //Are all the required passengers on board?
        foreach (Player requiredPlayed in m_RequiredPlayers)
        {
            if (!m_Passengers.Contains(requiredPlayed))
                return false;
        }

        return true;
    }

    protected virtual void OnStartDay()
    {
        m_Passengers.Clear();
        m_IsDriving = false;
    }
}

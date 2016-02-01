using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sjabloon;

public class Vehicle : MonoBehaviour, InteractableObject
{
    public enum VehicleType
    {
        None = 0,
        Car = 1,
        Bus = 2
    }

    [SerializeField]
    private VehicleType m_CurrentVehicleType;
    public VehicleType CurrentVehicleType
    {
        get { return m_CurrentVehicleType; }
    }

    [SerializeField]
    protected CharacterController2D m_CharacterController;

    [SerializeField]
    protected float m_Speed;

    [SerializeField]
    private List<Transform> m_WayPoints;
    private int m_CurrentWayPoint = 0;

    [SerializeField]
    protected List<Player> m_RequiredPlayers;
    protected List<Player> m_Passengers;
    protected bool m_IsDriving = false;

    private Vector3 m_OriginalPosition;

    protected virtual void Start()
    {
        GameManager.Instance.StartDayEvent += OnStartDay;
        m_Passengers = new List<Player>();

        m_OriginalPosition = transform.position.Copy();
    }

    protected virtual void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.StartDayEvent -= OnStartDay;
    }

    public bool IsUnlocked()
    {
        return true;
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

    public virtual void Interact(Player player)
    {
        if (m_IsDriving)
            return;

        if (!m_Passengers.Contains(player))
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

    public void CancelInteraction(Player player)
    {
        if (m_Passengers.Contains(player))
        {
            m_Passengers.Remove(player);
            player.UpdateVehicle(null);
        }
    }

    protected virtual void Update()
    {
        if (m_IsDriving == false || m_CurrentWayPoint >= m_WayPoints.Count)
            return;

        Vector2 direction = m_WayPoints[m_CurrentWayPoint].position - transform.position;
        float sqrMagnitude = direction.sqrMagnitude;

        direction.Normalize();

        m_CharacterController.Move(direction.x * m_Speed, direction.y * m_Speed);

        if (sqrMagnitude < 0.1f)
        {
            ++m_CurrentWayPoint;
            m_IsDriving = false;
        }
    }

    protected virtual bool CanDrive()
    {
        //Are all the required passengers on board?
        foreach (Player requiredPlayer in m_RequiredPlayers)
        {
            if (!m_Passengers.Contains(requiredPlayer) && requiredPlayer.IsOnScreen)
                return false;
        }

        return true;
    }

    protected virtual void OnStartDay()
    {
        m_Passengers.Clear();
        m_IsDriving = false;
        m_CurrentWayPoint = 0;

        transform.position = m_OriginalPosition;
    }
}

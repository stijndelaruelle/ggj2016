using UnityEngine;
using System.Collections;

public class CoffeeMachine : TaskObject
{
    [SerializeField]
    private Icon m_Icon;
    public Icon Icon
    {
        get { return m_Icon; }
        set { m_Icon = value; }
    }

    [SerializeField]
    private float m_TimeToMakeCoffee;
    private bool m_HasCoffee = false;
    private Coroutine m_RoutineHandle;

    private void Start()
    {
        GameManager.Instance.StartDayEvent += OnDayStart;
        m_Icon.Initialize();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.StartDayEvent -= OnDayStart;
    }

    public override void Interact(Player player)
    {
        if (IsInteracting(player))
            return;

        if (player == null)
        {
            base.Interact(player);
            return;
        }

        if (m_RoutineHandle != null)
            return;

        if (m_HasCoffee)
        {
            base.Interact(player);
            m_HasCoffee = false;
            return;
        }

        m_RoutineHandle = StartCoroutine(CreateCoffeeRoutine());
    }

    private IEnumerator CreateCoffeeRoutine()
    {
        float timer = m_TimeToMakeCoffee;

        m_Icon.Show();

        while (timer > 0.0f)
        {
            // UPDATE VISUALS
            float progress = (m_TimeToMakeCoffee - timer) / m_TimeToMakeCoffee;
            m_Icon.UpdateProgress(progress);

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Hide icon
        m_Icon.Win();
        m_HasCoffee = true;
        m_RoutineHandle = null;
    }

    private void OnDayStart()
    {
        if (m_RoutineHandle != null)
            StopCoroutine(m_RoutineHandle);

        m_RoutineHandle = null;
        m_HasCoffee = false;
    }
}

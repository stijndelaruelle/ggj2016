﻿using UnityEngine;
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
    private SpriteRenderer m_SpriteRenderer;

    [SerializeField]
    private Sprite m_EmptySprite;

    [SerializeField]
    private Sprite m_FullSprite;

    [SerializeField]
    private float m_TimeToMakeCoffee;

    [SerializeField]
    private float m_SpeedBoost;
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
            SetCoffee(false);
            return;
        }

        m_RoutineHandle = StartCoroutine(CreateCoffeeRoutine());
    }

    protected override void EndInteraction(bool finished)
    {
        if (finished)
            m_CurrentPlayer.ModifySpeed(m_SpeedBoost);
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
        SetCoffee(true);
        m_RoutineHandle = null;
    }

    private void SetCoffee(bool value)
    {
        m_HasCoffee = value;

        if (value)
            m_SpriteRenderer.sprite = m_FullSprite;
        else
            m_SpriteRenderer.sprite = m_EmptySprite;
    }

    private void OnDayStart()
    {
        if (m_RoutineHandle != null)
            StopCoroutine(m_RoutineHandle);

        m_RoutineHandle = null;
        SetCoffee(false);
    }
}
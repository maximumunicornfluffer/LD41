using UnityEngine;

public class InputScreen
{
    #region Properties 

	public static float DOUBLECLIC_DURATION = 0.2f;

    public bool IsDown
    {
        get { return m_isDown; }
    }

    public bool OnDown
    {
        get { return m_isDown == true && m_lastIsDown == false; }
    }

    public bool OnUp
    {
        get { return m_isDown == false && m_lastIsDown == true; }
    }

    public bool OnDoubleClick
    {
		get { return m_unpressedTimer < DOUBLECLIC_DURATION && m_isDown == true && m_lastIsDown == false; }
    }

    public float PressedTimer
    {
        get { return m_pressedTimer; }
    }

    public float UnPressedTimer
    {
        get { return m_unpressedTimer; }
    }

	public float LastClicTimer
	{
		get { return m_lastClicTimer; }
	}

    public Vector2 PressedPosition
    {
        get { return m_pressedPosition; }
    }

    public Vector2 Position
    {
        get { return m_position; }
    }

    public bool IsUsed
    {
        get { return m_isDown == true || m_lastIsDown == true; }
    }

    #endregion

    public void Update(bool isDown, Vector2 position)
    {
        if (m_resetting == true)
            return;

        m_lastIsDown = m_isDown;
        m_isDown = isDown;

        m_position = position;

        if (isDown == true)
		{
            m_pressedTimer += Time.deltaTime;
		}
        else
		{
            m_unpressedTimer += Time.deltaTime;
		}

        if (OnDown == true)
        {
            m_pressedTimer = 0;
			m_pressedPosition = position;
			m_lastClicTimer = m_unpressedTimer;
        }

        if (OnUp == true)
        {
            m_unpressedTimer = 0;
        }
    }

    public void StopReset()
    {
        m_resetting = false;
    }
    public void Reset()
    {
        m_isDown = false;
        m_lastIsDown = false;

        m_pressedTimer = 0.0f;
        m_unpressedTimer = float.MaxValue;

        m_resetting = true;
    }

    #region Fields

    private bool m_isDown;
    private bool m_lastIsDown;

    private Vector2 m_position;
    private Vector2 m_pressedPosition;
    private float m_pressedTimer;

    private bool m_resetting;
	private float m_unpressedTimer = float.MaxValue;
	private float m_lastClicTimer = float.MaxValue;

    #endregion
}
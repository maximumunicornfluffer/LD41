using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputsManager
{
    #region Singleton

    public static InputsManager Instance
    {
        get
        {
            if (s_instance != null)
                return s_instance;

            s_instance = new InputsManager();

            return s_instance;
        }
    }

    private static InputsManager s_instance;

    #endregion

    #region Const 

    private const KeyCode SecondTouchEmulationKey = KeyCode.LeftControl;

    #endregion

    #region Properties

    public InputScreen InputScreen0
    {
        get { return m_inputScreen0; }
    }

    public InputScreen InputScreen1
    {
        get { return m_inputScreen1; }
    }

    #endregion

    #region Initialize

    private InputsManager()
    {
    }

    #endregion

    #region Update

    public void Update()
    {

        
#if ANDROID
            UpdateTouchInput();
#else
            UpdateMouseInput();
#endif
    }

    private void UpdateMouseInput()
    {
        //if (IsOnGUI((int)Input.mousePosition.x, (int)Input.mousePosition.y) == true)
        //    return;

        if (Input.GetMouseButtonDown(0) == true)
        {
            m_inputScreen0.StopReset();
            m_inputScreen1.StopReset();
        }

        if (Input.GetKeyDown(SecondTouchEmulationKey) == true)
        {
            m_inputScreen1.StopReset();
        }

        if (Input.GetKey(SecondTouchEmulationKey) == true)
        {
            m_inputScreen0.Update(Input.GetMouseButton(0), m_inputScreen0.Position);
            m_inputScreen1.Update(Input.GetMouseButton(0), Input.mousePosition);
        }
        else
        {
            if (Input.GetKeyUp(SecondTouchEmulationKey) == true)
                m_inputScreen0.Update(Input.GetMouseButton(0), m_inputScreen0.Position);
            else
                m_inputScreen0.Update(Input.GetMouseButton(0), Input.mousePosition);

            m_inputScreen1.Update(false, Input.mousePosition);
        }
    }

    private void UpdateTouchInput()
    {
		int touchCount = GetRealTouchCount();
		if (touchCount == 1)
        {
            m_fingerIndex0 = Input.touches[0].fingerId;
        }

        UpdateInputScreen(m_inputScreen0, m_fingerIndex0, 1);
		if (touchCount == 2)
		{
			UpdateInputScreen(m_inputScreen1, 1 - m_fingerIndex0, 2);

		}else
		{
			m_inputScreen1.Update(false,Vector2.zero);
		}
    }
	private int GetRealTouchCount()
	{
//		Touch touch1 = GetTouchByFinger(0);
//		Touch touch2 = GetTouchByFinger(1);
		int touchCount = 0;
		for(int i =0;i< Input.touches.Length;i++)
		{
			Touch touch = GetTouchByFinger(i);
			if(touch.phase == TouchPhase.Began ||
			   touch.phase == TouchPhase.Moved ||
			   touch.phase == TouchPhase.Stationary)
			   {
				touchCount++;
			}
		}
		return touchCount;
	}
	
	private void UpdateInputScreen(InputScreen inputScreen, int fingerIndex, int touchCount)
    {
        bool touchDown = false;
        Vector2 touchPosition = inputScreen.Position;

        if (Input.touches.Length >= touchCount)
        {
            Touch touch = GetTouchByFinger(fingerIndex);
            touchPosition = touch.position;

            touchDown = (touch.phase == TouchPhase.Began ||
                         touch.phase == TouchPhase.Moved ||
                         touch.phase == TouchPhase.Stationary);

            if (touch.phase == TouchPhase.Began)
                inputScreen.StopReset();
        }

        inputScreen.Update(touchDown, touchPosition);
    }

    private Touch GetTouchByFinger(int fingerId)
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == fingerId)
                return touch;
        }

        throw new Exception("Can't find fingerId " + fingerId);
    }

    public int GetInputCount()
    {
        int count = 0;

        for (int i = 0; i < Input.touches.Length; i++)
        {
            Touch touch = Input.touches[i];
            if (touch.phase == TouchPhase.Moved ||
                touch.phase == TouchPhase.Stationary)
                count++;
        }

        return count;
    }

    private bool IsOnGUI(int x , int y)
    {
        if (EventSystem.current == null)
            return false;

        var eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(x, y);

        tempRaycastResults.Clear();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, tempRaycastResults);

        foreach (var item in tempRaycastResults)
        {
            if (item.gameObject.layer != 8)
                return true;
        }

        return false;
      //  return GUIUtility.hotControl != 0;
    }
    private static List<RaycastResult> tempRaycastResults = new List<RaycastResult>();

    #endregion

    #region Fields

    private readonly InputScreen m_inputScreen0 = new InputScreen();
    private readonly InputScreen m_inputScreen1 = new InputScreen();

    private int m_fingerIndex0;

    #endregion
}
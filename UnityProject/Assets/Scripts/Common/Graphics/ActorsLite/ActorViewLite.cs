using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorViewLite : MonoBehaviour
{
    public Entity ParentActor;

    public List<ActorActionViewLite> Actions = new List<ActorActionViewLite>();
    public List<ActorStateViewLite> States = new List<ActorStateViewLite>();

    public int CurrentStateID
    {
        get { return CurrentState.ID; }
    }


    [HideInInspector]
    public float StateTimer;

    public ActorActionViewLite CurrentAction
    {
        get
        {
            return m_currentAction;
        }
    }

    [HideInInspector]
    public ActorStateViewLite CurrentState
    {
        get
        {
            return m_currentState;
        }
    }

    private void Awake()
    {
        int index = 0;
        foreach (var state in States)
        {
            state.ID = index;
            index++;
        }

        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        SetState(0);

        m_currentFrame = UnityEngine.Random.Range(0, m_currentState.Animation.Sprites.Count);
        m_spriteRenderer.sprite = m_currentState.Animation.Sprites[m_currentFrame];
    }

    public void Update()
    {
        StateTimer += Time.deltaTime;

        if (m_currentAction != null)
        {
            m_currentAction.CurrentTimer += Time.deltaTime;

            if (m_currentAction.CurrentTimer >= (1.0f / m_currentAction.AnimationSpeed) * (float)m_currentAction.Animation.Sprites.Count)
            {
                string nextAction = m_currentAction.NextAction;

                m_currentAction.CurrentTimer = 0;
                m_currentAction = null;

                EndAction();

                Debug.Log("endaction");

                if (string.IsNullOrEmpty(nextAction) == false)
                    PlayAction(nextAction);
            }
        }

        UpdateView();

    }

    void UpdateView()
    {
        if (m_currentAction != null)
        {
            //float timePerFrame = m_currentAction.AnimationSpeed / (float)m_currentAction.Animation.Sprites.Count;

            //int frame = (int)(((m_currentAction.CurrentTimer) / (timePerFrame)));

            int frame = (int)(m_currentAction.CurrentTimer * m_currentAction.AnimationSpeed);

            if (frame >= m_currentAction.Animation.Sprites.Count)
                frame = m_currentAction.Animation.Sprites.Count - 1;

            if (frame != m_currentFrame)
            {
                m_currentFrame = frame;
                m_spriteRenderer.sprite = m_currentAction.Animation.Sprites[m_currentFrame];
            }

        }
        else if (m_currentState != null)
        {
            int frame = (int)(((StateTimer * m_currentState.AnimationSpeed)) % m_currentState.Animation.Sprites.Count);

            if (frame != m_currentFrame)
            {
                m_currentFrame = frame;

                if (m_currentState.Animation.Sprites.Count > m_currentFrame && m_currentFrame >= 0)
                {
                    m_spriteRenderer.sprite = m_currentState.Animation.Sprites[m_currentFrame];
                }
            }
        }
    }

    void EndAction()
    {
        m_currentAction = null;
    }



    public bool HasAction()
    {
        return m_currentAction != null;
    }

    internal void SetState(int stateID)
    {
        StateTimer = 0;

        m_currentState = States[stateID];

    }

    internal void SetState(string name)
    {
        ActorStateViewLite state = GetState(name);

        if (state == null)
        {
            Debug.LogError("can't find state " + name);
        }
        else
        {
            SetState(state.ID);
        }

    }

    ActorStateViewLite GetState(string name)
    {
        foreach (var item in States)
        {
            if (item.Name == name)
                return item;
        }

        return null;
    }



    internal void PlayAction(string actionName)
    {
        ActorActionViewLite action = GetAction(actionName);
        if (action == null)
        {
            Debug.LogError("can't find action " + actionName);
            return;
        }

        m_currentAction = action;
        m_currentAction.CurrentTimer = 0;
    }



    ActorActionViewLite GetAction(int index)
    {
        if (index < 0 || index >= Actions.Count)
            return null;

        return Actions[index];
    }

    ActorActionViewLite GetAction(string actionName)
    {
        foreach (var action in Actions)
        {
            if (action.Name == actionName)
                return action;
        }
        return null;
    }

    internal void SetParent(Entity parent)
    {
        ParentActor = parent;
    }



    int m_currentFrame;
    SpriteRenderer m_spriteRenderer;
    ActorActionViewLite m_currentAction;
    ActorStateViewLite m_currentState;
}

using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallback
{

    public FirstController sceneController;
    public List<CCFlyAction> Fly;
    public int DiskNumber = 0; //该round剩余飞碟数

    private List<SSAction> used = new List<SSAction>(); //正在使用的动作
    private List<SSAction> free = new List<SSAction>(); //还未被激活的动作

    SSAction GetSSAction()
    {
        SSAction action = null;
        if (free.Count > 0)
        {
            action = free[0];
            free.Remove(free[0]);
        }
        else
        {
            action = Instantiate<CCFlyAction>(Fly[0]);
        }
        used.Add(action);
        return action;
    }

    public void FreeSSAction(SSAction action)
    {
        SSAction tmp = null;
        foreach (SSAction i in used)
        {
            if (action.GetInstanceID() == i.GetInstanceID())
            {
                tmp = i;
            }
        }
        if (tmp != null)
        {
            tmp.reset();
            free.Add(tmp);
            used.Remove(tmp);
        }
    }

    protected new void Start()
    {
        sceneController = (FirstController)SSDirector.getInstance().currentSceneController;
        sceneController.actionManager = this;
        Fly.Add(CCFlyAction.GetSSAction());

    }

    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0,
        string strParam = null,
        UnityEngine.Object objectParam = null)
    {
        if (source is CCFlyAction)
        {
            DiskNumber--;
            DiskFactory df = Singleton<DiskFactory>.Instance;
            df.FreeDisk(source.gameobject);
            FreeSSAction(source);
        }
    }

    public void StartThrow(Queue<GameObject> diskQueue)
    {
        foreach (GameObject tmp in diskQueue)
        {
            RunAction(tmp, GetSSAction(), (ISSActionCallback)this);
        }
    }
}
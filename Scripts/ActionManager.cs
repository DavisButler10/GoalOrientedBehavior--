using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    Goal[] mGoals;
    Action[] mActions;
    Action mChangeOverTime;
    const float TICK_LENGTH = 4.0f;
    public GameObject BarLeft;
    public GameObject BarMiddle;
    public GameObject BarRight;
    Transform bar1;
    Transform bar2;
    Transform bar3;
    public Animator anim;

    void Start()
    {
        bar1 = BarLeft.transform.Find("BarL");
        bar2 = BarMiddle.transform.Find("BarM");
        bar3 = BarRight.transform.Find("BarR");
        

        mGoals = new Goal[3];
        mGoals[0] = new Goal("Groove", 2);
        mGoals[1] = new Goal("Funk", 2);
        mGoals[2] = new Goal("Soul", 2);

        mActions = new Action[6];
        mActions[0] = new Action("dance1");
        mActions[0].targetGoals.Add(new Goal("Groove", -1f));
        mActions[0].targetGoals.Add(new Goal("Funk", -1f));
        mActions[0].targetGoals.Add(new Goal("Soul", +1f));

        mActions[1] = new Action("dance2");
        mActions[1].targetGoals.Add(new Goal("Groove", +1f));
        mActions[1].targetGoals.Add(new Goal("Funk", 0f));
        mActions[1].targetGoals.Add(new Goal("Soul", -1f));

        mActions[2] = new Action("dance3");
        mActions[2].targetGoals.Add(new Goal("Groove", -1f));
        mActions[2].targetGoals.Add(new Goal("Funk", +1f));
        mActions[2].targetGoals.Add(new Goal("Soul", +1f));

        mActions[3] = new Action("dance4");
        mActions[3].targetGoals.Add(new Goal("Groove", 2f));
        mActions[3].targetGoals.Add(new Goal("Funk", -1f));
        mActions[3].targetGoals.Add(new Goal("Soul", -1f));

        mActions[4] = new Action("dance5");
        mActions[4].targetGoals.Add(new Goal("Groove", +3f));
        mActions[4].targetGoals.Add(new Goal("Funk", -2f));
        mActions[4].targetGoals.Add(new Goal("Soul", -1f));

        mActions[5] = new Action("dance6");
        mActions[5].targetGoals.Add(new Goal("Groove", -2f));
        mActions[5].targetGoals.Add(new Goal("Funk", +1f));
        mActions[5].targetGoals.Add(new Goal("Goal3", +1f));

        mChangeOverTime = new Action("tick");
        mChangeOverTime.targetGoals.Add(new Goal("Groove", +1f));
        mChangeOverTime.targetGoals.Add(new Goal("Funk", +1f));
        mChangeOverTime.targetGoals.Add(new Goal("Soul", -1f));

        InvokeRepeating("Tick", 0f, TICK_LENGTH);
    }

    void Tick()
    {
        foreach (Goal goal in mGoals)
        {
            goal.value += mChangeOverTime.getGoalChange(goal);
            goal.value = Mathf.Max(goal.value, 0);
            SetBar(goal);
            anim.SetBool("dance1", false);
            anim.SetBool("dance2", false);
            anim.SetBool("dance3", false);
            anim.SetBool("dance4", false);
            anim.SetBool("dance5", false);
            anim.SetBool("dance6", false);
        }
        Debug.Log("Tick.");
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetBool("dance1", false);
            anim.SetBool("dance2", false);
            anim.SetBool("dance3", false);
            anim.SetBool("dance4", false);
            anim.SetBool("dance5", false);
            anim.SetBool("dance6", false);
            Action bestThingToDo = chooseAction(mActions, mGoals);
            Debug.Log("I think I will " + bestThingToDo.name);
            
            performAction(bestThingToDo);

            foreach (Goal goal in mGoals)
            {
                goal.value += bestThingToDo.getGoalChange(goal);
                goal.value = Mathf.Max(goal.value, 0);
                Debug.Log(goal.name + ": " + goal.value);
                SetBar(goal);
            }
        }
    }

    void SetBar(Goal goal)
    {
        if (goal.value > 10)
        {
            goal.value = 10;
        }
        switch (goal.name)
        {
            case "Groove":
                bar1.localScale = new Vector3(goal.value / 10, 1f);
                break;
            case "Funk":
                bar2.localScale = new Vector3(goal.value / 10, 1f);
                break;
            case "Soul":
                bar3.localScale = new Vector3(goal.value / 10, 1f);
                break;
        }
    }


    void performAction(Action action)
    {
        anim.SetBool("dance1", false);
        anim.SetBool("dance2", false);
        anim.SetBool("dance3", false);
        anim.SetBool("dance4", false);
        anim.SetBool("dance5", false);
        anim.SetBool("dance6", false);
        switch (action.name)
        {
            case "dance1":
                anim.SetBool("dance1", true);
                break;
            case "dance2":
                anim.SetBool("dance2", true);
                break;
            case "dance3":
                anim.SetBool("dance3", true);
                break;
            case "dance4":
                anim.SetBool("dance4", true);
                break;
            case "dance5":
                anim.SetBool("dance5", true);
                break;
            case "dance6":
                anim.SetBool("dance6", true);
                break;
        }
        
    }

    Action chooseAction(Action[] actions, Goal[] goals)
    {
        Action bestAction = null;
        float bestValue = float.PositiveInfinity;

        foreach (Action action in actions)
        {
            float thisValue = discontentment(action, goals);
            
            if (thisValue < bestValue)
            {
                bestValue = thisValue;
                bestAction = action;
            }
        }

        return bestAction;
    }

    float discontentment(Action action, Goal[] goals)
    {
        float discontentment = 0f;

        foreach (Goal goal in goals)
        {
            float newValue = goal.value + action.getGoalChange(goal);
            newValue = Mathf.Max(newValue, 0);

            discontentment += goal.getDiscontentment(newValue);
        }

        return discontentment;
    }
}

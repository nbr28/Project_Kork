using Assets.Scripts.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnEditor : MonoBehaviour
{
    public ClickManager clickManager;
    private YarnLine selection;

    private void Awake()
    {
        resetYarnSelection();
    }

    public void enableYarnSelection()
    {
        clickManager.setClickMode(1);
    }

    public void disableYarnSelection()
    {
        resetYarnSelection();
        clickManager.setClickMode(0);
    }

    public void setYarnSelection(int from, int to, int id)
    {
        selection.Snippet_Id_From = from;
        selection.Snippet_Id_To = to;
        selection.Yarn_Id = id;
    }

    public void resetYarnSelection()
    {
        setYarnSelection(-1, -1, -1);
    }

    public YarnLine getYarnSelection()
    {
        return selection;
    }

    public int getFrom()
    {
        return selection.Snippet_Id_From;
    }

    public int getTo()
    {
        return selection.Snippet_Id_To;
    }

    public int getID()
    {
        return selection.Yarn_Id;
    }

    public void setFrom(int from)
    {
        selection.Snippet_Id_From = from;
    }

    public void setTo(int to)
    {
        selection.Snippet_Id_To = to;
    }

    public void setID(int id)
    {
        selection.Yarn_Id = id;
    }
}

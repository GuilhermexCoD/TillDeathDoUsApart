using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class CustomizationMenu : MonoBehaviour
{
   public PlayerVisualManager visualManager;

    public List<BodyData> heads;
    public List<BodyData> torsos;
    public List<BodyData> hands;
    public List<BodyData> feet;
    public int currentHead;
    public int currentTorso;
    public int currentHand; 
    public int currentFoot;
    private int incr=0;

    public void carregarCena(string nome){
         SceneManager.LoadScene(nome);
    }

    public void SetId(int id)
    {      
        int currentOption;
        List<BodyData> tmp = heads;
        EBodyPartType type = (EBodyPartType)id;
        switch(type){
            case EBodyPartType.Head:
                tmp = heads;
                currentHead = (int)Util.ModLoop((float)this.currentHead+incr, (float)tmp.Count);
                currentOption = currentHead;
                break;
            case EBodyPartType.Torso:
                tmp = torsos;
                currentTorso = (int)Util.ModLoop((float)this.currentTorso+incr, (float)tmp.Count);
                currentOption = currentTorso;
                break;
            case EBodyPartType.Hand:
                tmp = hands;
                currentHand = (int)Util.ModLoop((float)this.currentHand+incr, (float)tmp.Count);
                currentOption = currentHand;
                break;
            case EBodyPartType.Foot: 
                tmp = feet;
                currentFoot = (int)Util.ModLoop((float)this.currentFoot+incr, (float)tmp.Count);
                currentOption = currentFoot;
                break;   
            default:
                currentOption = 0;
                break;
        }
        visualManager.SetPart(type, tmp[currentOption]);
        visualManager.UpdatePart(type);
        return;
    }

    public void setIncr(int x){
        this.incr=x;
    }

    public void setSelector(int x){
        GameObject.Find("ColorPicker").GetComponent<ColorPicker>().option = x;
    }
}

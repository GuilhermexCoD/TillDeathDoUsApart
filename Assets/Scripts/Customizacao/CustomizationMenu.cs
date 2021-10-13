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
    private int incr = 0;

    private ResourceManager<EBodyPartType, BodyData> resourceManager;

    private void Awake()
    {
        resourceManager = new ResourceManager<EBodyPartType, BodyData>("Customization");
    }

    public void CarregarCena(string nome)
    {
        SceneManager.LoadScene(nome);
    }

    public void SetId(int id)
    {
        int currentOption;
        EBodyPartType type = (EBodyPartType)id;
        List<BodyData> tmp = resourceManager.GetAssets((int)type);

        switch (type)
        {
            case EBodyPartType.Head:
                currentHead = (int)Util.ModLoop((float)this.currentHead + incr, (float)tmp.Count);
                currentOption = currentHead;
                break;
            case EBodyPartType.Torso:
                currentTorso = (int)Util.ModLoop((float)this.currentTorso + incr, (float)tmp.Count);
                currentOption = currentTorso;
                break;
            case EBodyPartType.Hand:
                currentHand = (int)Util.ModLoop((float)this.currentHand + incr, (float)tmp.Count);
                currentOption = currentHand;
                break;
            case EBodyPartType.Foot:
                currentFoot = (int)Util.ModLoop((float)this.currentFoot + incr, (float)tmp.Count);
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

    public void SetIncr(int x)
    {
        this.incr = x;
    }

    public void SetSelector(int x)
    {
        GameObject.Find("ColorPicker").GetComponent<ColorPicker>().option = x;
    }
}

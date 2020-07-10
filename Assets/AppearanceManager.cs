using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AppearanceManager : MonoBehaviour
{
    public GameObject RudderSys;
    public GameObject EngineBay;
    public GameObject ControlSurfaces;
    public GameObject LandingGear;
    public GameObject CanardWing;
    public GameObject CockpitAssemply;
    public GameObject MainFuselage;


    public void clearAll()
    {
        RudderSys.SetActive(false);
        EngineBay.SetActive(false);
        ControlSurfaces.SetActive(false);
        LandingGear.SetActive(false);
        CanardWing.SetActive(false);
        CockpitAssemply.SetActive(false);
        MainFuselage.SetActive(false);
    }
    public void ShowAll()
    {
        RudderSys.SetActive(true);
        EngineBay.SetActive(true);
        ControlSurfaces.SetActive(true);
        LandingGear.SetActive(true);
        CanardWing.SetActive(true);
        CockpitAssemply.SetActive(true);
        MainFuselage.SetActive(true);
    }
    public void Cockpit()
    {
        if (CockpitAssemply.gameObject.activeSelf)
        {
            CockpitAssemply.SetActive(false);
        }
        else
        {
            CockpitAssemply.SetActive(true);
        }
    }


    public void Rudder()
    {
        if (RudderSys.gameObject.activeSelf)
        {
            RudderSys.SetActive(false);
        }
        else
        {
            RudderSys.SetActive(true);
        }
    }
    public void Engine()
    {
        if (EngineBay.gameObject.activeSelf)
        {
            EngineBay.SetActive(false);
        }
        else
        {
            EngineBay.SetActive(true);
        }
    }
    public void ControlSurface()
    {
        if (ControlSurfaces.gameObject.activeSelf)
        {
            ControlSurfaces.SetActive(false);
        }
        else
        {
            ControlSurfaces.SetActive(true);
        }
    }
    public void LandingGears()
    {
        if (LandingGear.gameObject.activeSelf)
        {
            LandingGear.SetActive(false);
        }
        else
        {
            LandingGear.SetActive(true);
        }
    }
    public void Canard()
    {
        if (CanardWing.gameObject.activeSelf)
        {
            CanardWing.SetActive(false);
        }
        else
        {
            CanardWing.SetActive(true);
        }
    }
    public void MainFuselageBody()
    {
        if (MainFuselage.gameObject.activeSelf)
        {
            MainFuselage.SetActive(false);
        }
        else
        {
            MainFuselage.SetActive(true);
        }
    }


}

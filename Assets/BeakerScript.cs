﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BeakerScript : MonoBehaviour //to-do: mix colors??
{
    [Header("")]
    [Range(0, 1)]
    public float waterPercent=0f;
    public Transform Liquid;
    public float fullValue=0.85f;
    public float emptyValue=0.03f;
    public float fillRate=0.001f;
    public float drainRate=0.001f;
    [Header("")]
    public Transform[] EmitPoints;
    private Transform EmitPoint;
    public GameObject particle;
    public int pourThreshold = 45;
    [Header("")]
    public float MixRate= 0.004f;
    public Renderer liquidRenderer;
    private Color CurrentLiquidColor;
    private string CurrentLiquidName;
    public float threshold = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waterPercent < 0)
        {
            waterPercent = 0;
        }
        else if(waterPercent >1)
        {
            waterPercent = 1;
        }
        if(waterPercent == 0)
        {
            Color c = liquidRenderer.material.color;
            liquidRenderer.material.color = new Color(c.r,c.g,c.b,0);
        }
        else
        {
            Color c = liquidRenderer.material.color;
            liquidRenderer.material.color = new Color(c.r, c.g, c.b, 1);
        }
        //find the lowest emit point
        foreach (var item in EmitPoints)
        {
            if (EmitPoint != null)
            {
                if (item.transform.position.y < EmitPoint.transform.position.y)
                {
                    EmitPoint = item;
                }
            }
            else
            {
                EmitPoint = EmitPoints[0];
            }
        }
        //calculate the water level
        float val = ((fullValue - emptyValue) * waterPercent) + emptyValue;
        //apply it
        Liquid.localScale = new Vector3(Liquid.localScale.x, Liquid.localScale.y, val);

        //spilling
        if (CaculatePourAngles() <= pourThreshold)
        {
            if (waterPercent > 0 && waterPercent <= 1)
            {
                GameObject liquidParticle = Instantiate(particle, EmitPoint.position, EmitPoint.rotation);
                waterPercent = waterPercent - fillRate;

                //color stuff
                //liquidParticle.GetComponent<ParticleSystemRenderer>().trailMaterial.color = liquidRenderer.material.color;
                liquidParticle.GetComponent<ParticleSystemRenderer>().material.color = liquidRenderer.material.color;

                CurrentLiquidColor = liquidRenderer.materials[0].color;
                CurrentLiquidName = "MixedLiquid";
                foreach (var item in flaskScript.ListOfLiquids)
                {
                        print(CurrentLiquidColor);
                        print(item.color);
                    if (ColorsAreClose(item.color, CurrentLiquidColor))
                    {
                        CurrentLiquidName = item.name;
                    }
                }
                liquidParticle.name = CurrentLiquidName;
            }
        }
        else
        {

        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (waterPercent <= 1 && waterPercent >= 0 )
        {
            waterPercent = waterPercent + fillRate;
                if (waterPercent <= 0.01)
                {
                    liquidRenderer.material.color = other.GetComponent<ParticleSystemRenderer>().material.color;
                }
                else
                {
                    Color currentCol = liquidRenderer.material.color;
                    Color ParticleCol = other.GetComponent<ParticleSystemRenderer>().material.color;
                    liquidRenderer.material.color = Color.Lerp(currentCol, ParticleCol, MixRate);
                }
        }
    }

    float CaculatePourAngles()
    {
        return transform.forward.y * Mathf.Rad2Deg;
    }

    bool ColorsAreClose(Color a, Color z)
    {
        var rDist = Mathf.Abs(a.r - z.r);
        var gDist = Mathf.Abs(a.g - z.g);
        var bDist = Mathf.Abs(a.b - z.b);

        if (rDist + gDist + bDist > threshold)
            return false;

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSettings settings;

    public ColorGenerator(ColorSettings settings)
    {
        this.settings = settings;
    }

    public void UpdateElaviation(MinMax elivationMinmax)
    {
        settings.mat.SetVector("_elivationMinMax", new Vector4(elivationMinmax.Min, elivationMinmax.Max));
    }
}

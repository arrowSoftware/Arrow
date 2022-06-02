using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

public class CharacterStat
{
    public float BaseValue;
    private float lastBaseValue = float.MinValue;

    private bool isDirty = true;
    private float _value;
    private readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public float Value {
        get
        {
            if (isDirty || lastBaseValue != BaseValue)
            {
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }

    public CharacterStat(float baseValue)
    {
        BaseValue = baseValue;
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
    }

    public bool RemoveModifier(StatModifier mod)
    {
        if (statModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    public bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;

        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];

            if (mod.Type == StatModifierType.Flat)
            {
                finalValue += mod.Value;
            }
            /*
             *  every time we encounter a PercentAdd modifier, we start adding it together with all
             *  modifiers of the same type, until we encounter a modifier of a different type or we 
             *  reach the end of the list. At that point, we grab the sum of all the PercentAdd 
             *  modifiers and multiply it with finalValue, just like we do with PercentMult modifiers.
             */
            else if (mod.Type == StatModifierType.PercentAdd)
            {
                sumPercentAdd += mod.Value;

                // If we're at the end of the list OR the next modifer isn't of this type
                if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModifierType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (mod.Type == StatModifierType.PercentMult)
            {
                finalValue *= 1 + mod.Value;
            }
        }

        // Rounding gets around float calculation errors (like getting 12.0001f, instead of 12f)
        // 4 digits is usually precise enough, but feel free to change this to fit your needs
        return (float)Math.Round(finalValue, 4);
    }

    private int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
            return -1;
        else if (a.Order > b.Order)
            return 1;

        return 0; // if (a.Order == b.Order)
    }
}

using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaboratorioDeSoftware.Tools;

public static class EnumHelper
{
    public static SelectList ToSelectListWithDescriptions<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>();
        var items = values.Select(e => new SelectListItem
        {
            Value = e.ToString(),
            Text = GetDescription(e),
            Selected = false
        }).ToList();

        return new SelectList(items, "Value", "Text");
    }

    private static string GetDescription<T>(T value) where T : Enum
    {
        FieldInfo field = value.GetType().GetField(value.ToString());
        DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    public static void ImageTransparent(this Image image, float alpha)
    {
        Color tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }
    public static (List<T> Added, List<T> Removed, List<T> Modified) GetDifferences<T>
        (
            this List<T> originalList,
            List<T> modifiedList,
            IEqualityComparer<T> compare = null
        )
    {
        compare ??= EqualityComparer<T>.Default;

        var addedItems = modifiedList?.Except(originalList, compare).ToList();
        var removedItems = originalList?.Except(modifiedList, compare).ToList();
        var modifiedItems = originalList
            ?.Where(originalItem => modifiedList.Any(modifiedItem => compare.Equals(originalItem, modifiedItem) && !compare.Equals(originalItem, modifiedItem)))
            .ToList();
        return (addedItems, removedItems, modifiedItems);
    }
}

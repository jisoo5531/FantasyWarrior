using System;
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
    public static (List<T> Added, List<T> Removed, List<T> Modified) GetDifferences<T>(
        List<T> originalList,
        List<T> newList,
        Func<T, T, bool> areEqual,
        Func<T, T, bool> isModified)
    {
        // 추가된 항목 찾기
        var added = newList
            .Where(newItem => !originalList.Any(originalItem => areEqual(originalItem, newItem)))
            .ToList();

        // 삭제된 항목 찾기
        var removed = originalList
            .Where(originalItem => !newList.Any(newItem => areEqual(originalItem, newItem)))
            .ToList();

        // 수정된 항목 찾기
        var modified = newList
            .Where(newItem => originalList.Any(originalItem => areEqual(originalItem, newItem) && isModified(originalItem, newItem)))
            .ToList();

        return (added, removed, modified);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    /// <summary>
    /// 이미지 투명도 조절
    /// </summary>
    /// <param name="image"></param>
    /// <param name="alpha"></param>
    public static void ImageTransparent(this Image image, float alpha)
    {
        Color tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }
    /// <summary>
    /// 해당 게임오브젝트의 자식들 지우기
    /// </summary>
    /// <param name="content"></param>
    public static void ContentClear(this GameObject content)
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            MonoBehaviour.Destroy(content.transform.GetChild(i).gameObject);
        }
    }
    public static (List<T> Added, List<T> Removed, List<T> Modified) GetDifferences<T>(
        List<T> originalList,
        List<T> newList,
        Func<T, T, bool> areEqual,
        Func<T, T, bool> isModified = null)
    {
        // 추가된 항목 찾기
        var added = newList
            .Where(newItem => !originalList.Any(originalItem => areEqual(originalItem, newItem)))
            .ToList();

        // 삭제된 항목 찾기
        var removed = originalList
            .Where(originalItem => !newList.Any(newItem => areEqual(originalItem, newItem)))
            .ToList();

        if (isModified == null)
        {
            return (added, removed, null);
        }

        // 수정된 항목 찾기
        var modified = newList
            .Where(newItem => originalList.Any(originalItem => areEqual(originalItem, newItem) && isModified(originalItem, newItem)))
            .ToList();


        return (added, removed, modified);
    }
}

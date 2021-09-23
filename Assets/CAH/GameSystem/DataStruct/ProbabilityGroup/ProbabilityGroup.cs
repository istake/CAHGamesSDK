
using System;
using System.Collections.Generic;

public class ProbabilityGroup<T>
{
    public class ProbabilityItem<T>
    {
        /// <summary>
        /// 아이템 개수중 뽑힐 확률.
        /// </summary>
        public float probability;
        public T item;
    }
    List<ProbabilityItem<T>> items = new List<ProbabilityGroup<T>.ProbabilityItem<T>>();

    public void AddItem(float percentage, T value)
    {

        float sum = 0;
        for (int i = 0; i < items.Count; i++)
        {
            sum += items[i].probability; 
        }

        if (sum + percentage > 100)
        {
            throw new Exception("확률은 총합 100을 넘길 수 없습니다..");
        }
        else
        {
            this.items.Add(new ProbabilityItem<T>()
            {
                item = value,
                probability = percentage
            });
        }
    }
    public ProbabilityItem<T> PickRandom()
    {
        float p = UnityEngine.Random.Range(0, 100);
        float sum = 0;
        for (int i = 0; i < items.Count; i++)
        {
            sum += this.items[i].probability;   
            if (p <= sum)
            {
                return items[i];
            }
        }

        return null;
    }
}
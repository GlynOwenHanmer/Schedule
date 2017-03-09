using System;

namespace GOH.Schedule
{
    public class Schedule<T> : ISchedule<T>
    {
        private Item<T>[] items;

        public Schedule(params Item<T>[] items) { this.Items = items; }

        public Item<T>[] Items
        {
            get
            {
                return this.items;
            }
            private set
            {
                this.items = value;
            }
        }

        public float Length
        {
            get
            {
                float length = 0.0f;
                foreach (Item<T> item in this.items)
                {
                    length += item.Length;
                }
                return length;
            }
        }

        public Item<T> at(float time)
        {
            if (!HasItems) throw new InvalidOperationException(no_items_string);
            time = time % Length;
            float periodStart = 0f;
            foreach (Item<T> item in items)
            {
                float periodEnd = periodStart + item.Length;
                if (periodStart <= time && time < periodEnd)
                {
                    return item;
                }
                periodStart = periodEnd;
            }
            throw new InvalidOperationException(string.Format("Method should not reach here. Other exceptions should be thrown earlier to handle state of current Schedule.\ntime: {0}\nSchedule state: {1}", time, this));
        }

        public Item<T> nextAt(float time)
        {
            if (!HasItems) throw new InvalidOperationException(no_items_string);
            time = time % Length;
            float periodStart = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                float periodEnd = periodStart + items[i].Length;
                if (periodStart <= time && time < periodEnd)
                {
                    return items[++i % items.Length];
                }
                periodStart = periodEnd;
            }
            throw new InvalidOperationException(string.Format("Method should not reach here. Other exceptions should be thrown earlier to handle state of current Schedule.\ntime: {0}\nSchedule state: {1}", time, this));
        }

        public bool HasItems { get { return this.items.Length > 0; } }

        public const string no_items_string = "Schedule has no items.";
    }
}
namespace GOH.Schedule
{
    internal interface ISchedule<T>
    {
        Item<T>[] Items { get; }
        Item<T> at(float time);
        Item<T> nextAt(float time);
        bool HasItems { get; }
        float Length { get; }
    }
}
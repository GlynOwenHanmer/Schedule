using System;

namespace GOH.Schedule
{
    public class Item<K>
    {
        private K content;
        private float length;

        protected Item() { }

        public Item(float length, K content)
        {
            this.content = content;
            this.Length = length;
        }

        public float Length
        {
            get { return this.length; }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException(negative_length_error);
                }
                this.length = value;
            }
        }
        public K Content
        {
            get { return this.content;}
            set
            {
                this.content = value;
            }
        }

        public override string ToString()
        {
            return string.Format("Length: {0} Content: {1}", Length, Content.ToString());
        }

        static public string negative_length_error { get { return "Negative Schedule Item length given."; } }
    }
}


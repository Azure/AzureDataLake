namespace AzureDataLake.Store
{
    public class FSPath
    {
        string s;

        public FSPath(string s)
        {
            if (s == null)
            {
                throw new System.ArgumentNullException(nameof(s));
            }

            if (s.Length == 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(s));

            }
            this.s=s;
        }

        public override string ToString()
        {
            return this.s;
        }

        public bool IsRooted
        {
            get { return this.s[0] == '/'; }
        }


        public static FSPath Root
        {
            get { return FSPath.root; }
        }

        private static readonly FSPath root = new FSPath("/");

        public static FSPath Combine(FSPath left, FSPath right)
        {
            if (right.IsRooted)
            {
                throw new System.ArgumentException(nameof(right));
            }


            if (left.EndsWithSeparator)
            {
                var p = new FSPath(left.ToString() + right.ToString());
                return p;

            }
            else
            {
                var p = new FSPath(left.ToString() + "/" + right.ToString());
                return p;
            }

        }

        public FSPath Append(FSPath p)
        {
            return FSPath.Combine(this, p);
        }

        public FSPath Append(string p)
        {
            return FSPath.Combine(this, new FSPath(p));
        }

        public bool EndsWithSeparator
        {
            get { return this.s[this.s.Length - 1] == '/'; }
        }

    }
}
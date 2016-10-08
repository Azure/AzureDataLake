namespace AzureDataLake.Store
{
    public class FsPath
    {
        string s;

        public FsPath(string s)
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


        public static FsPath Root
        {
            get { return FsPath.root; }
        }

        private static readonly FsPath root = new FsPath("/");

        public static FsPath Combine(FsPath left, FsPath right)
        {
            if (right.IsRooted)
            {
                throw new System.ArgumentException(nameof(right));
            }


            if (left.EndsWithSeparator)
            {
                var p = new FsPath(left.ToString() + right.ToString());
                return p;

            }
            else
            {
                var p = new FsPath(left.ToString() + "/" + right.ToString());
                return p;
            }

        }

        public FsPath Append(FsPath p)
        {
            return FsPath.Combine(this, p);
        }

        public FsPath Append(string p)
        {
            return FsPath.Combine(this, new FsPath(p));
        }

        public bool EndsWithSeparator
        {
            get { return this.s[this.s.Length - 1] == '/'; }
        }

    }
}
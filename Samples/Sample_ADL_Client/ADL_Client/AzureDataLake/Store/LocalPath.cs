namespace AzureDataLake.Store
{
    public class LocalPath
    {
        string s;

        public LocalPath(string s)
        {
            if (s == null)
            {
                throw new System.ArgumentNullException(nameof(s));
            }

            if (s.Length == 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(s));

            }
            this.s = s;
        }

        public override string ToString()
        {
            return this.s;
        }
    }
}
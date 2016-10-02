namespace AzureDataLake.Store
{
    public struct FsPermission
    {
        public readonly int Integer;

        public FsPermission(int value)
        {
            if (value > 7)
            {
                throw new System.ArgumentOutOfRangeException(nameof(value));
            }

            if (value < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(value));
            }

            this.Integer = value;
        }

        public FsPermission(string value)
        {
            if (value.Length != 3)
            {
                throw new System.ArgumentOutOfRangeException(nameof(value));
            }

            bool r = (value[0] == 'r' || value[0] == 'R');
            bool w = (value[1] == 'w' || value[1] == 'W');
            bool x = (value[2] == 'x' || value[2] == 'X');

            this.Integer = bools_to_int(r, w, x);
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", nameof(FsPermission), this.ToRwxString());
        }

        public string ToRwxString()
        {
            char r = this.Read ? 'r' : '-';
            char w = this.Write? 'w' : '-';
            char x = this.Execute? 'x' : '-';
            string s = string.Format("{0}{1}{2}",r,w,x);
            return s;
        }

        public FsPermission(bool read, bool write, bool execute)
        {
            this.Integer = bools_to_int(read, write, execute);
        }

        public static int bools_to_int(bool read, bool write, bool execute)
        {
            return (bool_to_int(read) << 2) | (bool_to_int(write) << 1) | (bool_to_int(execute) << 0);
        }

        private static int bool_to_int(bool v)
        {
            return v ? 1 : 0;
        }

        public bool Read
        {
            get
            {
                return (0x4 & this.Integer) != 0;
            }
        }

        public bool Write
        {
            get
            {
                return (0x2 & this.Integer) != 0;
            }
        }
        public bool Execute
        {
            get
            {
                return (0x1 & this.Integer) != 0;
            }
        }

        public FsPermission Invert()
        {
            var p = new FsPermission(!this.Read,!this.Write,!this.Execute);
            return p;
        }

        public FsPermission AndWith(FsPermission p)
        {
            var np = new FsPermission(this.Read && p.Read, this.Write && p.Write, this.Execute && p.Execute);
            return np;
        }

        public FsPermission OrWith(FsPermission p)
        {
            var np = new FsPermission(this.Read || p.Read, this.Write || p.Write, this.Execute || p.Execute);
            return np;
        }
    }
}
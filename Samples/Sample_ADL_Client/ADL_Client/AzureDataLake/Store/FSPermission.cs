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

            bool r = char_to_bool(value[0], 'r', 'R');
            bool w = char_to_bool(value[1], 'w', 'W');
            bool x = char_to_bool(value[2], 'x', 'X');

            this.Integer = bools_to_int(r, w, x);
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", nameof(FsPermission), this.ToRwxString());
        }

        public string ToRwxString()
        {
            char r = bool_to_char(this.Read,'r');
            char w = bool_to_char(this.Write,'w');
            char x = bool_to_char(this.Execute,'x');
            string s = string.Format("{0}{1}{2}",r,w,x);
            return s;
        }

        private static bool char_to_bool(char input_char, char true_value_1, char true_value_2)
        {
            if ((input_char == true_value_1) || (input_char == true_value_2))
            {
                return true;
            }

            if (input_char == '-')
            {
                return false;
            }

            throw new System.ArgumentOutOfRangeException(nameof(input_char));
        }

        private static char bool_to_char(bool b, char true_char)
        {
            return (b ? true_char : '-');
        }

        public FsPermission(bool read, bool write, bool execute)
        {
            this.Integer = bools_to_int(read, write, execute);
        }

        private static int bools_to_int(bool read, bool write, bool execute)
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
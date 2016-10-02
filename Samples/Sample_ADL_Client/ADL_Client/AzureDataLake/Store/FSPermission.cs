namespace AzureDataLake.Store
{
    public struct FSPermission
    {
        private int _bitValue;

        public int BitValue
        {
            get { return _bitValue; }
            set
            {
                if (value > 7)
                {
                    throw new System.ArgumentOutOfRangeException(nameof(value));
                }

                if (value < 0)
                {
                    throw new System.ArgumentOutOfRangeException(nameof(value));
                }

                _bitValue = value;
            }
        }

        public FSPermission(int i)
        {
            if (i > 7)
            {
                throw new System.ArgumentOutOfRangeException(nameof(i));
            }

            if (i < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(i));
            }

            this._bitValue = i;
        }

        public FSPermission(string s)
        {
            if (s.Length != 3)
            {
                throw new System.ArgumentOutOfRangeException(nameof(s));
            }

            this._bitValue = 0;
            this.Read = (s[0] == 'r' || s[0] == 'R');
            this.Write = (s[1] == 'w' || s[1] == 'W');
            this.Execute = (s[2] == 'x' || s[2] == 'X');
        }

        public FSPermission(bool read, bool write, bool execute)
        {
            this._bitValue = 0;
            this.Read = read;
            this.Write = write;
            this.Execute = execute;
        }

        public bool Read
        {
            get
            {
                return (0x4 & this.BitValue) != 0;
            }
            private set
            {
                if (value)
                {
                    this.BitValue |= 0x4;
                }
                else
                {
                    this.BitValue &= ~0x4;
                }
            }
        }

        public bool Write
        {
            get
            {
                return (0x2 & this.BitValue) != 0;
            }
            private set
            {
                if (value)
                {
                    this.BitValue |= 0x2;
                }
                else
                {
                    this.BitValue &= ~0x2;
                }
            }
        }
        public bool Execute
        {
            get
            {
                return (0x1 & this.BitValue) != 0;
            }
            private set
            {
                if (value)
                {
                    this.BitValue |= 0x1;
                }
                else
                {
                    this.BitValue &= ~0x1;
                }
            }
        }

        public FSPermission Invert()
        {
            var p = new FSPermission(!this.Read,!this.Write,!this.Execute);
            return p;
        }

    }
}
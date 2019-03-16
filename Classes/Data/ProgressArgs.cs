namespace if2ktool
{
    public struct ProgressArgs
    {
        public int processed;
        public int count;
        public long timeMs;

        public int currentRowIndex;

        public bool updateProperties;
    }
}

    public struct UserParameters
    {
        public string OSName { get; set; }
        public string OSVersion { get; set; }

        public UserParameters(string osName, string osVersion)
        {
            OSName = osName;
            OSVersion = osVersion;
        }
    }
namespace libMBIN {
    public static class Version {

        public static System.Version GetVersion() {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

    }
}

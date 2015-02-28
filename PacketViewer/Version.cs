namespace PacketViewer
{
    /// <summary>
    /// Helper class/method to retrieve the version of the PacketViewer application
    /// </summary>
    public sealed class Version
    {
        Version() { }

        /// <summary>
        /// Returns the current version of the PacketViewer application
        /// </summary>
        /// <returns>the current version of the PacketViewer application</returns>
        public static System.Version GetVersion
        {
            get
            {
                System.Reflection.Assembly asm
                    = System.Reflection.Assembly.GetAssembly(typeof(PacketViewer.Version));
                return asm.GetName().Version;
            }
        }
    }
}
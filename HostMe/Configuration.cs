namespace HostMe
{
    public class Configuration
    {
        /// <summary>
        /// Directory to host (with web files, i.e. index.html)
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }


        /// <summary>
        /// Default port to serve
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; set; }


        /// <summary>
        /// If the <see cref="Port"/> is already served at the localhost, it will pick next that is not served and use it instead
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do automatic select unused port]; otherwise, <c>false</c>.
        /// </value>
        public bool doAutoSelectUnusedPort { get; set; } = true;


        /// <summary>
        /// If <c>true</c> it will try to open localhost at served port with OS default web broser
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do automatic open default browser]; otherwise, <c>false</c>.
        /// </value>
        public bool doAutoOpenDefaultBrowser { get; set; } = true;


    }
}

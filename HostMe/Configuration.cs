// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="imbVeles" >
//
// The MIT Licence
// 
// Copyright (C) 2017 Goran Grubic (HostMe fork) and TheCodeCleaner (HostMe, original project)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// </copyright>
// <summary>
// ------------------------------------------------------------------------------------------------------------------
// GitHub of TheCodeCleaner (original project author): https://github.com/TheCodeCleaner
// GitHub repo of the original project: https://github.com/TheCodeCleaner/HostMe
// 
// ------ Goran Grubic -- imbVeles project ------
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// http://blog.veles.rs/
// </summary>
// ------------------------------------------------------------------------------------------------------------------
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


        /// <summary>
        /// If true, it will prevent logging output
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do disable logging]; otherwise, <c>false</c>.
        /// </value>
        public bool doDisableLogFiles { get; set; } = false;


    }
}

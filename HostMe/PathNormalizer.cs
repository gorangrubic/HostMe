// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathNormalizer.cs" company="imbVeles" >
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
using System.IO;
using System.Reflection;

namespace HostMe
{
    public static class PathNormalizer
    {
        private static readonly string RootPath;

        static PathNormalizer()
        {
            var codeBase = Assembly.GetExecutingAssembly().GetName().CodeBase;
            RootPath = Path.GetDirectoryName(codeBase).Replace(@"file:\", "");
        }

        public static string NormalizePath(string path)
        {
            if (path == null)
                path = string.Empty;

            return Path.IsPathRooted(path) ? path : Path.Combine(RootPath, path);
        }
    }
}

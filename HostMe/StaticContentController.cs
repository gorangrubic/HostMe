// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticContentController.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using log4net;

namespace HostMe
{
    public class StaticContentController : ApiController
    {
        private readonly ILog _logger = Logger.GetLogger();

        private static readonly Dictionary<string, string> MediaTypesFixes 
            = new Dictionary<string, string> { {".svg", "image/svg+xml" } };

        public static string SiteRootPath { get; set; }

        [EnableCors("*", "*", "*")]
        [Route("{*path}")]
        public HttpResponseMessage GetContent(string path)
        {
            _logger.Info("Got request. path = " + path);

            var absolutePath = GetAbsolutePath(path);

            _logger.Info("Absolute path = " + absolutePath);

            try
            {
                var response = PrepareResponseForPath(absolutePath);

                _logger.InfoFormat("Response for {0} sent!", path);
                return response;
            }
            catch (Exception exception)
            {
                _logger.Warn(absolutePath + " could not be parsed.", exception);
                _logger.Warn("Responding with Bad Request!");
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        private HttpResponseMessage PrepareResponseForPath(string path)
        {
            var extension = Path.GetExtension(path);

            path = HandleNotExistingHtmlFile(path, extension);

            var mediaType = MimeMapping.GetMimeMapping(path);
            if (MediaTypesFixes.ContainsKey(extension))
                mediaType = MediaTypesFixes[extension];

            _logger.Info("Media Type found = " + mediaType);

            var content = File.ReadAllBytes(path);
            _logger.Info("Content read from: " + path);

            var response = new HttpResponseMessage
            {
                Content = new ByteArrayContent(content)
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            return response;
        }

        private string HandleNotExistingHtmlFile(string path, string extension)
        {
            if (File.Exists(path))
                return path;

            if (extension != "" && extension != ".html")
                return path;

            var newPath = Path.Combine(SiteRootPath, "index.html");
            _logger.Info($"Path {path} was not foud, normalized to: {newPath}");
                
            return newPath;
        }

        private static string GetAbsolutePath(string path)
        {
            if (path == null)
                path = "index.html";

            return Path.Combine(SiteRootPath, path);
        }
    }
}

using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageUpload.Utilies
{
    public static class MultipartRequestHelper
    {
        //Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.
        
        public static string GetBoundary(System.Net.Http.Headers.MediaTypeHeaderValue contentType, int lengthLimit)
        {
            string boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq";
            //string boundary = HeaderUtilities.RemoveQuotes().Value;

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool HasFormDataContentDisposition(System.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && string.IsNullOrEmpty(contentDisposition.FileName)
                && string.IsNullOrEmpty(contentDisposition.FileNameStar);
        }

        public static bool HasFileContentDisposition(System.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && (!string.IsNullOrEmpty(contentDisposition.FileName)
                    || !string.IsNullOrEmpty(contentDisposition.FileNameStar));
        }

        internal static object GetBoundary(System.Net.Http.Headers.MediaTypeHeaderValue mediaTypeHeaderValue, object multipartBoundaryLengthLimit)
        {
            throw new NotImplementedException();
        }
    }
}

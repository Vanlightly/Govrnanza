using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Docs
{
    /// <summary>
    /// Corrects some cases of bad formatting of XML comments in the Swagger UI
    /// </summary>
    public class FormatXmlCommentProperties : IOperationFilter
    {
        /// <summary>
        /// Apply the formatting corrections
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Description = Formatted(operation.Description);
            operation.Summary = Formatted(operation.Summary);
        }

        private string Formatted(string text)
        {
            if (text == null) return null;

            // Strip out the whitespace that messes up the markdown in the xml comments,
            // but don't touch the whitespace in <code> blocks. Those get fixed below.
            string resultString = Regex.Replace(text, @"(^[ \t]+)(?![^<]*>|[^>]*<\/)", "", RegexOptions.Multiline);
            resultString = Regex.Replace(resultString, @"<code[^>]*>", "<pre>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
            resultString = Regex.Replace(resultString, @"</code[^>]*>", "</pre>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
            resultString = Regex.Replace(resultString, @"<!--", "", RegexOptions.Multiline);
            resultString = Regex.Replace(resultString, @"-->", "", RegexOptions.Multiline);

            try
            {
                string pattern = @"<pre\b[^>]*>(.*?)</pre>";

                foreach (Match match in Regex.Matches(resultString, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline))
                {
                    var formattedPreBlock = FormatPreBlock(match.Value);
                    resultString = resultString.Replace(match.Value, formattedPreBlock);
                }
                return resultString;
            }
            catch
            {
                // Something went wrong so just return the original resultString
                return resultString;
            }
        }

        private string FormatPreBlock(string preBlock)
        {
            // Split the <pre> block into multiple lines
            var linesArray = preBlock.Split('\n');
            if (linesArray.Length < 2)
            {
                return preBlock;
            }
            else
            {
                // Get the 1st line after the <pre>
                string line = linesArray[1];
                int lineLength = line.Length;
                string formattedLine = line.TrimStart(' ', '\t');
                int paddingLength = lineLength - formattedLine.Length;

                // Remove the padding from all of the lines in the <pre> block
                for (int i = 1; i < linesArray.Length - 1; i++)
                {
                    linesArray[i] = linesArray[i].Substring(paddingLength);
                }

                var formattedPreBlock = string.Join("", linesArray);
                return formattedPreBlock;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    // http://stackoverflow.com/questions/3268622/regex-to-split-line-csv-file
    public class Csv
    {
        private static readonly Regex pattern = new Regex(@"
                            # Parse CVS line. Capture next value in named group: 'val'
                            \s*                      # Ignore leading whitespace.
                            (?:                      # Group of value alternatives.
                              ""                     # Either a double quoted string,
                              (?<val>                # Capture contents between quotes.
                                [^""]*(""""[^""]*)*  # Zero or more non-quotes, allowing 
                              )                      # doubled "" quotes within string.
                              ""\s*                  # Ignore whitespace following quote.
                            |  (?<val>[^,]*)         # Or... zero or more non-commas.
                            )                        # End value alternatives group.
                            (?:,|$)                  # Match end is comma or EOS",
                    RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

        public static string[] ReadValues(string data)
        {
            try
            {
                List<string> dataValues = new List<string>();
                Match matchResult = pattern.Match(data.Trim());
                while (matchResult.Success)
                {
                    dataValues.Add(matchResult.Groups["val"].Value);
                    matchResult = matchResult.NextMatch();
                }
                return dataValues.ToArray();
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
                throw;
            }
        }

    }
}

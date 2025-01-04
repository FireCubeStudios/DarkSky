using System;
using System.Text;

namespace DarkSky.Core.Helpers
{
    /*
	 * This helper class contains code for Facets and RichText
	 * Facets work with UTF-8 encoding and use byte indices.
	 * Inclusive start index and an exclusive end index.
	 * https://docs.bsky.app/docs/advanced-guides/post-richtext
	 */
    public class RichTextHelper
    {
        // Omit Byte Order Mark which is a sequence of special bytes that indicates encoding type
        private static readonly Encoding UTF8Encoder = new UTF8Encoding(false);

        public static int ByteIndexToCharIndex(string text, long byteIndex)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            // Convert the entire string to a UTF-8 byte array
            byte[] utf8Bytes = UTF8Encoder.GetBytes(text);

            if (byteIndex < 0 || byteIndex > utf8Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(byteIndex), "Byte index is out of bounds.");

            int charIndex = 0; // The resulting character index in the UTF-16 string
            int currentByteIndex = 0; // The current byte index in the UTF-8 byte array

            while (currentByteIndex < byteIndex && charIndex < text.Length)
            {
                char currentChar = text[charIndex];

                // Determine if the current character is part of a surrogate pair
                int charByteSize;
                if (char.IsHighSurrogate(currentChar) && charIndex + 1 < text.Length && char.IsLowSurrogate(text[charIndex + 1]))
                {
                    // Combine surrogate pairs into a single Unicode scalar
                    string surrogatePair = new string(new[] { currentChar, text[charIndex + 1] });
                    charByteSize = UTF8Encoder.GetByteCount(surrogatePair);
                    charIndex++; // Skip the low surrogate
                }
                else
                {
                    // Single character
                    charByteSize = UTF8Encoder.GetByteCount(new[] { currentChar });
                }

                // If adding this character's bytes exceeds the target byteIndex, stop here
                if (currentByteIndex + charByteSize > byteIndex)
                    break;

                currentByteIndex += charByteSize;
                charIndex++;
            }

            return charIndex;
        }
    }
}

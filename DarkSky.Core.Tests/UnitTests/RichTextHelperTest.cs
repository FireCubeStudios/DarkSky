using DarkSky.Core.Helpers;

namespace DarkSky.Core.Tests.UnitTests
{
    public class RichTextHelperTest
    {
        [Xunit.Theory]
        [InlineData("test", 1, 1)] // Regular text
        [InlineData("hello world", 6, 6)] // Regular text with spaces
        [InlineData("😀emoji", 4, 2)] // Emoji at the beginning
        [InlineData("text😀", 4, 4)] // Emoji at the end
        [InlineData("\u4F60\u597D", 3, 1)] // Chinese characters (UTF-16 surrogate pair case)
        [InlineData("السلام", 6, 3)] // Arabic text
        [InlineData("a🚀b", 4, 1)] // Mixed text with emoji in the middle
        public void GetIndexVariousTextTests(string text, long byteIndex, int expectedCharIndex)
        {
            Assert.Equal(expectedCharIndex, RichTextHelper.ByteIndexToCharIndex(text, byteIndex));
        }
    }
}

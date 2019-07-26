using propmgr.Core.Entities;
using Xunit;

namespace propmgr.Core.xUnit.Tests
{
    public class PropertyPairTests
    {
        [Theory]
        [InlineData("#")]
        [InlineData("!")]
        public void ValidCommentLineShouldReturnTrue(string key)
        {
            var p = new PropertyPair { Key = key, Value = "I am a comment." };

            Assert.True(p.IsComment());
        }

        [Fact]
        public void NonCommentsShouldReturnFalse()
        {
            var p = new PropertyPair { Key = "Key1", Value = "I am not a comment." };

            Assert.False(p.IsComment());
        }

        [Fact]
        public void EmptyLineShouldReturnTrue()
        {
            var p = new PropertyPair { Key = "", Value = "" };

            Assert.True(p.IsEmptyLine());
        }

        [Theory]
        [InlineData("", "Value")]
        [InlineData("Key", "")]
        [InlineData("Key", "Value")]
        public void NonEmptyLineShouldReturnFalse(string key, string value)
        {
            var p = new PropertyPair { Key = key, Value = value };

            Assert.False(p.IsEmptyLine());
        }
    }
}

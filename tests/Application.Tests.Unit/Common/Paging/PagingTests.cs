namespace CleanMinimalApi.Application.Tests.Unit.Common.Paging;

using Application.Common.Paging;
using Shouldly;
using Xunit;

public class PagingTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(12345)]
    public void Encode_ShouldRoundTrip_Offset(int offset)
    {
        // Act
        var cursor = Paging.Encode(offset);
        var result = Paging.TryDecode(cursor, out var decoded);

        // Assert
        result.ShouldBeTrue();
        decoded.ShouldBe(offset);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TryDecode_ShouldReturn_FirstPage(string cursor)
    {
        // Act
        var result = Paging.TryDecode(cursor, out var offset);

        // Assert
        result.ShouldBeTrue();
        offset.ShouldBe(0);
    }

    [Theory]
    [InlineData("not a cursor")]
    [InlineData("b2Zmc2V0Og==")]
    [InlineData("b2Zmc2V0Oi0x")]
    [InlineData("cGFnZToy")]
    public void TryDecode_ShouldReturn_False(string cursor)
    {
        // Act
        var result = Paging.TryDecode(cursor, out _);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void ToPage_ShouldReturn_NextCursor()
    {
        // Act
        var result = Paging.ToPage([1, 2], 5, 2);

        // Assert
        result.Items.ShouldBe([1, 2]);
        result.TotalCount.ShouldBe(5);
        _ = Paging.TryDecode(result.NextCursor, out var next);
        next.ShouldBe(4);
    }

    [Fact]
    public void ToPage_ShouldReturn_NoCursorOnLastPage()
    {
        // Act
        var result = Paging.ToPage([5], 5, 4);

        // Assert
        result.NextCursor.ShouldBeNull();
    }
}

using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class ResultTests
{
    private static AtelierRexError SomeError =>
        new(ErrorCode.UnknownFormat, "unknown format");

    // ── Result<T> ──────────────────────────────────────────────────────────

    [Fact]
    public void Generic_success_result_reports_IsSuccess_true()
    {
        var result = Result<int>.Success(42);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Generic_success_result_exposes_the_value()
    {
        var result = Result<string>.Success("hello");
        Assert.Equal("hello", result.Value);
    }

    [Fact]
    public void Generic_success_result_throws_InvalidOperationException_when_accessing_Error()
    {
        var result = Result<int>.Success(1);
        Assert.Throws<InvalidOperationException>(() => _ = result.Error);
    }

    [Fact]
    public void Generic_failure_result_reports_IsSuccess_false()
    {
        var result = Result<int>.Failure(SomeError);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Generic_failure_result_exposes_the_error()
    {
        var error = new AtelierRexError(ErrorCode.MalformedHeader, "bad header", "offset 8");
        var result = Result<int>.Failure(error);
        Assert.Equal(ErrorCode.MalformedHeader, result.Error.Code);
        Assert.Equal("bad header", result.Error.Message);
        Assert.Equal("offset 8", result.Error.Context);
    }

    [Fact]
    public void Generic_failure_result_throws_InvalidOperationException_when_accessing_Value()
    {
        var result = Result<int>.Failure(SomeError);
        Assert.Throws<InvalidOperationException>(() => _ = result.Value);
    }

    [Fact]
    public void Generic_success_result_can_hold_a_reference_type_value()
    {
        var list = new List<string> { "a", "b" };
        var result = Result<List<string>>.Success(list);
        Assert.Same(list, result.Value);
    }

    // ── Result (non-generic) ───────────────────────────────────────────────

    [Fact]
    public void Nongeneric_success_result_reports_IsSuccess_true()
    {
        var result = Result.Success();
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Nongeneric_success_result_throws_InvalidOperationException_when_accessing_Error()
    {
        var result = Result.Success();
        Assert.Throws<InvalidOperationException>(() => _ = result.Error);
    }

    [Fact]
    public void Nongeneric_failure_result_reports_IsSuccess_false()
    {
        var result = Result.Failure(SomeError);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Nongeneric_failure_result_exposes_the_error()
    {
        var error = new AtelierRexError(ErrorCode.AccessDenied, "access denied");
        var result = Result.Failure(error);
        Assert.Equal(ErrorCode.AccessDenied, result.Error.Code);
        Assert.Equal("access denied", result.Error.Message);
    }
}

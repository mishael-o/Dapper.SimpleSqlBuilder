namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// Implements the <see cref="IBuilderFormatter"/> interface for the <see cref="SqlBuilder"/> type.
/// </summary>
internal sealed partial class SqlBuilder : IBuilderFormatter
{
    public void AppendControl(ControlType controlType)
    {
        switch (controlType)
        {
            case ControlType.Space:
                AppendSpace();
                break;

            case ControlType.NewLine:
                AppendNewLine();
                break;
        }
    }

    public void AppendFormatted<T>(T value, string? format = null)
        => stringBuilder.Append(sqlFormatter.Format(value, format));

    public void AppendLiteral(string value)
        => stringBuilder.Append(value);
}

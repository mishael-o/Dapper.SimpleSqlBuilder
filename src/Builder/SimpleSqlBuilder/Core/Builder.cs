using System.Data;

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// An abstract class that implements the <see cref="ISqlBuilder"/> and defines the builder type.
/// </summary>
public abstract class Builder : ISqlBuilder
{
    /// <inheritdoc/>
    public abstract string Sql { get; }

    /// <inheritdoc/>
    public abstract object Parameters { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<string> ParameterNames { get; }

    /// <summary>
    /// An add operator for the builder that enables dynamic query concatenation.
    /// </summary>
    /// <param name="builder">The <see cref="Builder"/> instance to perform the operation on.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> when called on a <see langword="null"/> object.</exception>
    public static Builder operator +(Builder builder, FormattableString formattable)
    {
        return builder is null
            ? throw new ArgumentNullException(nameof(builder))
#if NET6_0_OR_GREATER
            : builder.AppendIntact($"{formattable}");
#else
            : builder.AppendIntact(formattable);
#endif
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends a space and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder Append([InterpolatedStringHandlerArgument("")] ref AppendInterpolatedStringHandler handler);

    /// <summary>
    /// Appends a space and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder Append(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref AppendInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder AppendIntact([InterpolatedStringHandlerArgument("")] ref AppendIntactInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder AppendIntact(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref AppendIntactInterpolatedStringHandler handler);

    /// <summary>
    /// Appends a <see cref="Environment.NewLine"/> and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder AppendNewLine([InterpolatedStringHandlerArgument("")] ref AppendNewLineInterpolatedStringHandler handler);

    /// <summary>
    /// Appends a <see cref="Environment.NewLine"/> and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder AppendNewLine(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref AppendNewLineInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends a space and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder Append(FormattableString formattable);

    /// <summary>
    /// Appends a space and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder Append(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder AppendIntact(FormattableString formattable);

    /// <summary>
    /// Appends the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder AppendIntact(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends a <see cref="Environment.NewLine"/> and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder AppendNewLine(FormattableString formattable);

    /// <summary>
    /// Appends a <see cref="Environment.NewLine"/> and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder AppendNewLine(bool condition, FormattableString formattable);

#endif

    /// <summary>
    /// Appends a <see cref="Environment.NewLine"/> to the builder.
    /// </summary>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder AppendNewLine();

    /// <summary>
    /// Adds a parameter to the dynamic <see cref="Parameters">parameters</see> list.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <param name="dbType">The <see cref="DbType"/> of the parameter.</param>
    /// <param name="direction">The in or out <see cref="ParameterDirection"/> of the parameter.</param>
    /// <param name="size">The size of the parameter.</param>
    /// <param name="precision">The precision of the parameter.</param>
    /// <param name="scale">The scale of the parameter.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder AddParameter(string name, object? value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null);

    /// <summary>
    /// Appends an object containing parameters to the dynamic <see cref="Parameters">parameters</see> bag.
    /// <example>
    /// <para>Example 1:</para>
    /// <code>
    /// // Add property A and B to the builder.
    /// AddDynamicParameters(new {A = 1, B = 2});
    /// </code>
    /// <para>Example 2:</para>
    /// <code>
    /// // Create a DynamicParameters object to hold the parameters.
    /// var dynamicParameters = new DynamicParameters();
    /// // Add parameters A and B to the dynamicParameters object.
    /// dynamicParameters.Add("A", 1);
    /// dynamicParameters.Add("B", 2);
    /// // Add the dynamicParameters object to the builder.
    /// AddDynamicParameters(dynamicParameters);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>The <see cref="Builder"/> instance.</returns>
    public abstract Builder AddDynamicParameters(object? parameter);

    /// <summary>
    /// Gets the value of a parameter.
    /// </summary>
    /// <typeparam name="T">The type to cast the value to.</typeparam>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <returns>The value of the parameter. Note <see cref="DBNull.Value">DBNull.Value</see> is not returned, instead the value is returned as <see langword="null"/>.</returns>
    public abstract T GetValue<T>(string parameterName);

    /// <summary>
    /// Resets the builder to its initial state.
    /// <para>This resets the <see cref="Parameters">parameters</see> and the <see cref="Sql">SQL</see> string.</para>
    /// </summary>
    public abstract void Reset();
}

using System.Data;

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// An abstract class that defines the simple builder type or contract.
/// </summary>
public abstract class SimpleBuilderBase : ISqlBuilder
{
    /// <summary>
    /// Gets the generated the Sql.
    /// </summary>
    public abstract string Sql { get; }

    /// <summary>
    /// Gets the <see cref="DynamicParameters"/> list.
    /// </summary>
    public abstract object Parameters { get; }

    /// <summary>
    /// Gets the parameter names.
    /// </summary>
    public abstract IEnumerable<string> ParameterNames { get; }

    /// <summary>
    /// An add operator for the builder that enables dynamic query concatenation.
    /// </summary>
    /// <param name="simpleBuilder">The <see cref="SimpleBuilderBase"/>.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>Returns a <see cref="SimpleBuilder"/>.</returns>
    public static SimpleBuilderBase operator +(SimpleBuilderBase simpleBuilder, FormattableString formattable)
    {
        return simpleBuilder is null
            ? throw new ArgumentNullException(nameof(simpleBuilder))
            : simpleBuilder.AppendIntact(formattable);
    }

    /// <summary>
    /// Appends a space prefix and a <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString">formattable string</see>.</param>
    /// <returns>Returns a <see cref="SimpleBuilderBase"/>.</returns>
    public abstract SimpleBuilderBase Append(FormattableString formattable);

    /// <summary>
    /// Appends a <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>Returns a <see cref="SimpleBuilderBase"/>.</returns>
    public abstract SimpleBuilderBase AppendIntact(FormattableString formattable);

    /// <summary>
    /// Appends an <see cref="Environment.NewLine"/> and a <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>Returns <see cref="SimpleBuilderBase"/>.</returns>
    public abstract SimpleBuilderBase AppendNewLine(FormattableString? formattable = null);

    /// <summary>
    /// Adds a parameter to the <see cref="Parameters">dynamic parameter</see> list.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <param name="dbType">The <see cref="DbType"/> of the parameter.</param>
    /// <param name="direction">The in or out <see cref="ParameterDirection"/> of the parameter.</param>
    /// <param name="size">The size of the parameter.</param>
    /// <param name="precision">The precision of the parameter.</param>
    /// <param name="scale">The scale of the parameter.</param>
    /// <returns>Returns <see cref="SimpleBuilderBase"/>.</returns>
    public abstract SimpleBuilderBase AddParameter(string name, object? value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null);

    /// <summary>
    /// Append a whole object full of parameters to the <see cref="Parameters">dynamic parameter</see> bag.
    /// <para>Example 1:</para>
    /// <para>AddDynamicParameters(new {A = 1, B = 2}) // will add property A and B to the dynamic.</para>
    /// <para>Example 2:</para>
    /// <para>var dynamicParameters = new DynamicParameters(); //creating a <see cref="DynamicParameters"/> object to hold the parameters.</para>
    /// <para>dynamicParameters.Add("A", 1);.</para>
    /// <para>dynamicParameters.Add("B", 2);.</para>
    /// <para>AddDynamicParameters(dynamicParameters) // will add parameters A and B to the <see cref="Parameters">dynamic parameter</see> bag.</para>
    /// </summary>
    /// <param name="param">The parameter.</param>
    /// <returns>Returns <see cref="SimpleBuilderBase"/>.</returns>
    public abstract SimpleBuilderBase AddDynamicParameters(object? param);

    /// <summary>
    /// Get the value of a parameter.
    /// </summary>
    /// <typeparam name="T">The type to cast the value to.</typeparam>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <returns>The value, note <see cref="DBNull.Value"/> is not returned, instead the value is returned as <see langword="null"/>.</returns>
    public abstract T GetValue<T>(string parameterName);
}

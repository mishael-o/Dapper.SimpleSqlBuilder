# Fluent Builder

The [`Fluent Builder`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.ISimpleFluentBuilder) enables you to build dynamic SQL queries using fluent APIs, offering a more intuitive and readable way to construct complex SQL statements.

The `CreateFluent` method on the [`SimpleSqlBuilder`](xref:Dapper.SimpleSqlBuilder.SimpleBuilder.CreateFluent(System.String,System.Nullable{System.Boolean},System.Nullable{System.Boolean})) or [`ISimpleBuilder`](xref:Dapper.SimpleSqlBuilder.DependencyInjection.ISimpleBuilder.CreateFluent(System.String,System.Nullable{System.Boolean},System.Nullable{System.Boolean})) (when using [dependency injection](../../configuration/dependency-injection.md)) creates a new [`fluent builder`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.ISimpleFluentBuilder) instance.

Using fluent APIs, you can build `SELECT`, `INSERT`, `UPDATE`, and `DELETE` queries. The [`Fluent builder`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.ISimpleFluentBuilder) parses the SQL query and extracts parameters from it. These parameters can be accessed via the [`Parameters`](xref:Dapper.SimpleSqlBuilder.Builder.Parameters) property, and the generated SQL query is available through the [`Sql`](xref:Dapper.SimpleSqlBuilder.Builder.Sql) property.

## Fluent Builders

- [Select Builder](select-builder.md)
- [Insert Builder](insert-builder.md)
- [Update Builder](update-builder.md)
- [Delete Builder](delete-builder.md)

## Features

- [Where Filters](where-filters.md)
- [Conditional Methods](conditional-methods.md)
- [Lower Case Clauses](lower-case-clauses.md)

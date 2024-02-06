# Fluent Builder

The [`fluent builder`](../../../api-docs/netcore/Dapper.SimpleSqlBuilder.FluentBuilder.ISimpleFluentBuilder.yml) enables you to build dynamic SQL queries using fluent APIs.

The `CreateFluent` method on the [`SimpleSqlBuilder`](../../../api-docs/netcore/Dapper.SimpleSqlBuilder.SimpleBuilder.yml) or [`ISimpleBuilder`](../../../api-docs/di/Dapper.SimpleSqlBuilder.DependencyInjection.ISimpleBuilder.yml) (when using [dependency injection](../../configuration/dependency-injection.md)) is used to create a new fluent builder instance.

Using fluent APIs you can build `SELECT`, `INSERT`, `UPDATE` and `DELETE` queries. The fluent builder will parse the SQL query and extract the parameters from it. The parameters can be accessed via the [`Parameters`](../../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml#Dapper_SimpleSqlBuilder_Builder_Parameters) property and the generated SQL query can be accessed via the [`Sql`](../../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml#Dapper_SimpleSqlBuilder_Builder_Sql) property.

#### Fluent Builders

- [Select Builder](select-builder.md)
- [Insert Builder](insert-builder.md)
- [Update Builder](update-builder.md)
- [Delete Builder](delete-builder.md)

#### Features

- [Where Filters](where-filters.md)
- [Conditional Methods](conditional-methods.md)
- [Lower Case Clauses](lower-case-clauses.md)

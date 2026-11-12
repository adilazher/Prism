# Prism

Prism is a .NET 10 ORM built from scratch as a learning project, with a focus on two core mechanics: entity discovery and schema migration.

Entity discovery is the process of scanning an assembly at runtime, identifying classes that represent database tables, and building an in-memory model of their structure using reflection and custom attributes. Schema migration takes that model and translates it into DDL SQL to create the corresponding tables in a SQL Server database automatically on first run.

Building these two systems from scratch is the main goal of this project. Everything else (the DML generators, the typed query methods, the connection management) exists to make them useful and testable end to end.

---

## Features

- Attribute-based entity and column mapping
- Automatic table creation on first run (migration)
- Support for primary keys, foreign keys, and nullable columns
- `PrismIgnore` support to exclude properties or entire classes
- Generic typed methods for querying and inserting data
- Pluggable DDL and DML generators (SQL Server implemented)

---

## Getting Started

### 1. Define an entity

Annotate a class with `[PrismTable]` and mark each mapped property with `[PrismColumn]`. Use `[PrismKey]` to designate the primary key.

```csharp
[PrismTable(Name = "Products")]
public class Product
{
    [PrismKey]
    [PrismColumn("Id", "INT")]
    public int Id { get; set; }

    [PrismColumn("Name", "NVARCHAR(200)")]
    public string Name { get; set; }

    [PrismColumn("Price", "DECIMAL(10,2)")]
    public decimal Price { get; set; }

    public Product() { }
}
```

### 2. Create a context and use it

Pass a SQL Server connection string to `PrismContext`. On construction, Prism scans the entry assembly for all annotated entities and creates their tables if they do not already exist. The context is disposable and should be used in a `using` block.

```csharp
const string connectionString = "Server=localhost;Database=MyDb;Trusted_Connection=True;";

using PrismContext context = new PrismContext(connectionString);

// Insert a record
Product product = new Product { Id = 1, Name = "Keyboard", Price = 79.99m };
bool inserted = context.Insert(product);

// Fetch by primary key
Product fetched = context.GetById<Product>(1);
Console.WriteLine($"{fetched.Name} costs {fetched.Price:C}");
```

---

## Attribute Reference

| Attribute | Target | Purpose |
|---|---|---|
| `[PrismTable(Name = "...")]` | Class | Maps the class to a database table |
| `[PrismColumn("name", "SQL_TYPE")]` | Property | Maps the property to a column |
| `[PrismKey]` | Property | Marks the property as the primary key |
| `[PrismForeignKey(ReferencedTableName, ReferencedColumnName)]` | Property | Declares a foreign key constraint |
| `[PrismIgnore]` | Class or Property | Excludes the class or property from all Prism processing |

---

## Examples

The `Prism.Examples` project contains runnable examples covering each feature:

| Folder | What it covers |
|---|---|
| `BasicCrud` | Insert and GetById with a simple entity |
| `ForeignKeys` | Declaring foreign key relationships between two entities |
| `NullableColumns` | Using nullable types to control NOT NULL constraints |
| `IgnoredProperties` | Excluding computed properties and entire classes |
| `MultipleEntities` | One context managing several entity types simultaneously |

To run the examples, update the connection string in `Prism.Examples/Program.cs` and run the project.

---

## Architecture

```
PrismContext
    AttributeBasedEntityDiscovery   -- scans the assembly for PrismTable-annotated types
    EntityMetadataBuilder           -- builds an in-memory schema model from reflection
    SQLServerDDLGenerator           -- generates CREATE TABLE statements
    SQLServerDMLGenerator           -- generates INSERT, UPDATE, DELETE, SELECT statements
    PrismDbConnection               -- manages the SQL Server connection lifecycle
```

Prism uses a strategy pattern for SQL generation, so alternative database engines can be supported by implementing `IDDLGenerator` and `IDMLGenerator`.

---

## Limitations

This is a learning project and is not intended for production use. Current known limitations include:

- SQL Server only (no other database engines implemented)
- Primary keys must be of type `int` and named `Id`
- No lazy loading or relationship navigation
- No parameterized queries (SQL injection risk if used with untrusted input)
- Schema migration drops and recreates tables on every run -- existing data is not preserved

---

## Requirements

- .NET 10.0
- SQL Server (local or remote instance)

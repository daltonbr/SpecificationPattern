# Specification Pattern

## Introduction

What is?
Avoid domain knowledge duplication
Declarative approach

Introduce by Eric Evans Martin Fowler in this white paper
<https://martinfowler.com/apsupp/spec.pdf>

Also in the famous "blue book"
Domain-Drive Design: Tackling Complexity in the Heart of Software by Eric Evans

Main use cases

- In-memory validation
- Retrieving data from the database
- Construction-to-order

## Implementing the specification pattern the naive way

### How LINQ Works

IEnumerable & IQueryable

The main difference between `IEnumerable` and `IQueryable` in C# is that IQueryable queries out-of-memory data stores, while IEnumerable queries in-memory data. Moreover, IQueryable is part of .NET's System.LINQ namespace, while IEnumerable is in System.Collections namespace.

IQueryable contains methods for constructing expression trees. IQueryable inherits IEnumerable, so IQueryable does everything that IEnumerable does. IQueryable extends the IEnumerable with logic for querying data.

### Using Plain C# Expressions (anti-pattern)

```csharp
public static readonly Expression<Func<Movie, bool>> IsSuitableForChildren =
    x => x.MpaaRating <= MpaaRating.PG;
public static readonly Expression<Func<Movie, bool>> HasCDVersion =
    x => x.ReleaseDate <= DateTime.Now.AddMonths(-6);

Expression<Func<Movie, bool>> exp1 = ForKidsOnly ? Movie.IsSuitableForChildren : x => true;
Expression<Func<Movie, bool>> exp2 = OnCD ? Movie.HasCDVersion : x => true;
Expression<Func<Movie, bool>> exp = exp1 && exp2; // Doesn't compile!
```

To combine two C# Expressions we would need to disassemble them and create a new one on top of them.

```csharp
Func<Movie, bool> isSuitableForChildren = Movie.IsSuitableForChildren.Compile();
if (!isSuitableForChildren(movie))
{
    // ...
}
```

This solution lacks encapsulation.

### Generic Specification (anti-pattern)

Provide some helper methods and acts as an thin wrapper, but it doesn't help as much.

Generic Specification doesn't provide a good encapsulation and should be avoided.

We should use *Strongly typed specifications* to encapsulated those business rules for each specification

`KidsMovieSpecification` or `HasCDVersionSpecification`

```csharp
public class GenericSpecification<T>
{
    public Expression<Func<T, bool>> Expression { get; }

    public GenericSpecification(Expression<Func<T, bool>> expression)
    {
        Expression = expression;
    }

    public bool IsSatisfiedBy(T entity)
    {
        return Expression.Compile().Invoke(entity);
    }
}
```

### Returning IQueryable from a Repository (anti-pattern)

```csharp
public IQueryable<Movie> Find()
    {
        ISession session = SessionFactory.OpenSession();
        return session.Query<Movie>();
        // dbContext.Movies // For EntityFramework
    }
```

- Allows for arbitrary expressions, even unsupported ones.
- Can cause runtime exceptions - not everything can be translated to SQL
- Could be fine for small projects - not suitable for medium and large projects

It violates the Liskov Substitution Principle (L in SOLID)
as the IQueryable cannot be fully "trusted" or used.

Some queries could compile but fail at runtime.

**NEVER return IQueryable from your public methods**

Return an `IReadOnlyList` instead.

```csharp
public IReadOnlyList<MovieDto> GetList(Specification<Movie> specification)
```

IEnumerable could also give you troubles when lazy evaluating.
Imagine trying to enumerate a list when the DB connection is already closed.
It would result in an exception.

```csharp
public IEnumerable<Movie> GetList(Specification<Movie> specification)
{
    using (ISession session = SessionFactory.OpenSession())
    {
        return session.Query<Movie>();
    }
}
```

### Summary

- How LINQ works
  - IEnumerable and IQueryable
  - C# lambda can compile to either a delegate or an expression
  - C# expressions can compile to delegates (but not the other way)
- Using plain C# expressions
  - Help get rid of duplication
  - Don't provide proper encapsulation
- Generic Specification
  - A thin wrapper on top of expressions
- Returning IQueryable from a repository - not a good idea

## Refactoring towards better encapsulation

- Better encapsulation
- Easier to work with

Interfaces

**There is no practical need for interfaces**, such as

```csharp
public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();
    bool IsSatisfiedBy(T entity);
}
```

**YAGNI violation**

```csharp
public abstract class ISpecification<T> : ISpecification<T>
{
    ///
}
```

- Avoid ISpecification interface
- Make specifications as specific as possible

Sometimes is hard to avoid not requesting extra parameters, .e.g.

```csharp
public class MovieWithActorSpecification : Specification<Movie>
{
    private readonly string _actorName;

    public MovieWithActorSpecification(string actorName)
    {
        _actorName = actorName;
    }

    public override Expression<Func<Movie, bool>> ToExpression()
    {
        //...
    }
}

"Although programmers can be generous in providing parameters to customize, eventually they can make the parameterized specification too complex to use and difficult to maintain."
*Marting Fowler and Eric Evans*

- Make specifications **immutable**

### Combining them together

```csharp
var forKids = new MovieForKidsSpecification();
var onCD = new AvailableOnCDSpecification();

Specification<Movie> forKidsAndOnCD = forKids.And(onCD);
Specification<Movie> notForKidsAndOnCD = forKids.Not().And(onCD);
```

- **And** **Or** **Not** (to help with combinations of specifications)
- also **Identity** specification - useful for dynamic search queries

### When not to use specification

- Only 1 out of 3 use cases
  - Search queries
  - In-memory validation

- Application is simple enough
  - Already low maintenance cost
  - Benefits may not justify the investment

- Use the specification pattern when:
  - Have at least 2 out of 3 use cases
  - Codebase is complex enough

### Combining specifications with regular filtration

You don't have to represent all your search validation capabilities as specifications.

### Working with Multiple Classes

e.g. `Movie` and `Director` classes

By default all navigation properties are lazy loaded - when the client code first time tries to access them. In NHibernated we can eager load them.

```csharp
    public IReadOnlyList<Movie> GetList(
        Specification<Movie> specification,
        double minimumRating,
        int page = 0,
        int pageSize = 4)
    {
        //TODO: to be implemented;
        return new List<Movie>()
            .AsQueryable()
            .Where(specification.ToExpression())
            .Where(x => x.Rating >= minimumRating)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Fetch(x => x.Director)  // this is eager loading
            .ToList();
```

- You can use specifications for related classes as well
- Eagerly load all related objects when you fetch data from the database
  - By default ORMs use lazy loading for efficiency reasons.
  - We could DTO (Data Transfer Objects) around that.

### Creation of New Objects

"A way to describe what an object might do, without explaining the details of how the object does it, but in such a way that a candidate might be built to fulfill the requirement."
*Martin Fowler and Eric Evans*

### Summary

- Proper encapsulation and efficient database queries
  - Specification should contain the domain knowledge
- General guidelines
  - Don't use ISpecification interface
  - Make specifications as specific as possible
  - Make specifications immutable
- Combining specifications together
  - **And**, **Or**, and **Not**
  - **Identity** specification
- When not to use the Specification pattern
- Combining specifications with regular filtration
- Creation of new objects

## Resource List

https://github.com/vkhorikov/SpecPattern

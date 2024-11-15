public abstract class BaseEntity
{
    public int Id { get; set; }
}

public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class Customer : BaseEntity
{
    public string FullName { get; set; }
}

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
}

public class Specification<T> : ISpecification<T>
{
    private readonly Func<T, bool> _predicate;

    public Specification(Func<T, bool> predicate)
    {
        _predicate = predicate;
    }

    public bool IsSatisfiedBy(T entity)
    {
        return _predicate(entity);
    }
}

public interface IRepository<T> where T : BaseEntity, new()
{
    void Add(T entity);
    void Remove(T entity);
    T GetById(int id);
    IEnumerable<T> GetAll();
    IEnumerable<T> Find(ISpecification<T> specification);
}

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    private readonly List<T> _entities = new List<T>();

    public void Add(T entity) => _entities.Add(entity);

    public void Remove(T entity) => _entities.Remove(entity);

    public T GetById(int id)
    {
        return _entities.FirstOrDefault(e => ((dynamic)e).Id == id);
    }

    public IEnumerable<T> GetAll() => _entities;

    public IEnumerable<T> Find(ISpecification<T> specification)
    {
        return _entities.Where(e => specification.IsSatisfiedBy(e));
    }
}

public class UnitOfWork
{
    public IRepository<Product> Products { get; } = new Repository<Product>();
    public IRepository<Customer> Customers { get; } = new Repository<Customer>();
}

public class Program
{
    public static void Main()
    {
        var unitOfWork = new UnitOfWork();

        // Örnek veriler ekleyelim
        unitOfWork.Products.Add(new Product { Id = 1, Name = "Laptop", Price = 1500 });
        unitOfWork.Products.Add(new Product { Id = 2, Name = "Mouse", Price = 25 });
        unitOfWork.Products.Add(new Product { Id = 3, Name = "Keyboard", Price = 75 });
        unitOfWork.Products.Add(new Product { Id = 4, Name = "Monitor", Price = 300 });

        // 50 ile 2000 arasındaki fiyatlar için bir Specification oluşturuyoruz
        var priceSpecification = new Specification<Product>(p => p.Price >= 50 && p.Price <= 2000);

        // Bu şarta uyan ürünleri listeleyelim
        var filteredProducts = unitOfWork.Products.Find(priceSpecification);

        foreach (var product in filteredProducts)
        {
            Console.WriteLine($"Product Name: {product.Name}, Price: {product.Price}");
        }
    }
}
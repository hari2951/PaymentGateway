Console.WriteLine("Hello, World!");
var list1 = new List<Employee>();

ListIntersection();
Console.WriteLine(list1.Count);



void ListFilteration(List<Employee> list1)
{
    list1.Add(new Employee { Id = 1, Currency = "USD"});
}


void ListIntersection()
{
    var a = new List<int> { 1,2,3,5};
    var b = new List<int> { 1,2,3,4};

    var toAdd = a.Except(b);
    var toRemove = b.Except(a);

    Console.WriteLine("To ADD");
    toAdd.ToList().ForEach(x => Console.WriteLine(x));
    Console.WriteLine("To Remove");
    toRemove.ToList().ForEach(x => Console.WriteLine(x));
}

class Employee
{
    public int Id { get; set; }
    public string Currency { get; set; }
}
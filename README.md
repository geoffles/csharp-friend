# Friend Scope Implementation For C&#35;

This library allows you to implement a `friend` scope for C# class methods.

Example:

```
public class FooContainer
{
  [Friend(typeof(Foo))]
  private void AddFoo(Foo foo)
  {
    //...
  }
}

public class Foo
{
  private class FooContainerFriend : Friend
  {
    public FooContainerFriend(FooContainer container) : base(FooContainer) {}

    public void AddFoo(Foo foo)
    {
      base.Invoke(foo);
    }
  }

  //The point: This method can effectively call the private method FooContainer.AddFoo on container.
  public void Link(FooContainer container)
  {
    new FooContainerFriend(container).AddFoo(this);
  }
}
```

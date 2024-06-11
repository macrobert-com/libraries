using MacRobert.Collections;

namespace Test.MacRobert.Collections;

public class PairingHeapTest
{
    [Fact]
    public void FindMin_Should_Return_MinElement()
    {
        var numbers = new[] { 10, 7, 8, 9, 1, 2, 5, 3, 4, 6 };
        var heap = new PairingHeap<int>(numbers);

        Assert.Equal(1, heap.FindMin());
    }

    [Fact]
    public void FindMin_Should_Throw_When_Empty()
    {
        var numbers = new int[] { };
        var heap = new PairingHeap<int>(numbers);
        var exception = Assert.Throws<InvalidOperationException>(() => heap.FindMin());
        Assert.Equal("Collection is empty", exception.Message);
    }

    [Fact]
    public void Delete_Should_Return_MinElement_And_Remove_It()
    {
        var numbers = new[] { 10, 7, 8, 9, 1, 2, 5, 3, 4, 6 };
        var heap = new PairingHeap<int>(numbers);

        Assert.Equal(10, heap.Count);
        Assert.Equal(1, heap.DeleteMin());
        Assert.Equal(9, heap.Count);
    }

    [Fact]
    public void Delete_Should_Throw_When_Empty()
    {
        var numbers = new int[] { };
        var heap = new PairingHeap<int>(numbers);
        var exception = Assert.Throws<InvalidOperationException>(() => heap.DeleteMin());
        Assert.Equal("Collection is empty", exception.Message);
    }

}
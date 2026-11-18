using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Add items with different priorities (highest priority placed at the back) and dequeue all of them.
    // Expected Result: Items are removed and returned in descending order of priority: "High", "Medium", "Low".
    // Defect(s) Found: Dequeue loop used (index < _queue.Count - 1) instead of (index < _queue.Count), skipping the last item in the queue, and never removed the item with RemoveAt. Expected High, got Medium
    public void TestPriorityQueue_HighestPriority()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Low", 1);
        priorityQueue.Enqueue("Medium", 5);
        priorityQueue.Enqueue("High", 10);

        Assert.AreEqual("High", priorityQueue.Dequeue());
        Assert.AreEqual("Medium", priorityQueue.Dequeue());
        Assert.AreEqual("Low", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Add multiple items where two or more share the same highest priority, mixed with lower priority items.
    // Expected Result: The item with the highest priority that was added first (closest to the front) is removed first (FIFO): "FirstHigh", "SecondHigh", "Low".
    // Defect(s) Found: Dequeue did not remove the dequeued item from _queue (missing _queue.RemoveAt), and used >= instead of > when comparing priorities, breaking FIFO tie-breaking rules. Expected SecondHigh on second dequeue, got FirstHigh again.
    public void TestPriorityQueue_TieBreakerFIFO()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Low", 1);
        priorityQueue.Enqueue("FirstHigh", 5);
        priorityQueue.Enqueue("SecondHigh", 5);

        Assert.AreEqual("FirstHigh", priorityQueue.Dequeue());
        Assert.AreEqual("SecondHigh", priorityQueue.Dequeue());
        Assert.AreEqual("Low", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Try to dequeue from an empty priority queue.
    // Expected Result: InvalidOperationException is thrown with the message "The queue is empty."
    // Defect(s) Found: none
    public void TestPriorityQueue_Empty()
    {
        var priorityQueue = new PriorityQueue();

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail($"Unexpected exception of type {e.GetType()} caught: {e.Message}");
        }
    }

    [TestMethod]
    // Scenario: Enqueue items, dequeue the highest priority item, then enqueue a new higher priority item and dequeue again.
    // Expected Result: Previously dequeued items are actually removed from the queue, and the newly added highest priority item is returned next: "First", "NewTop", "Second".
    // Defect(s) Found: Dequeue skipped the last element in the list due to (_queue.Count - 1) and failed to remove dequeued items from _queue. Expected First, got Second.
    public void TestPriorityQueue_RemovesItemAndMaintainsOrder()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Second", 2);
        priorityQueue.Enqueue("First", 4);

        Assert.AreEqual("First", priorityQueue.Dequeue());

        priorityQueue.Enqueue("NewTop", 6);
        Assert.AreEqual("NewTop", priorityQueue.Dequeue());
        Assert.AreEqual("Second", priorityQueue.Dequeue());
    }
}
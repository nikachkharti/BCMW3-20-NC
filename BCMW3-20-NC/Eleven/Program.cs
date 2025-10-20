namespace Eleven
{
    internal class Program
    {
        static void Main()
        {
            #region ARRAY
            //int[] testArray = new int[3];
            //testArray[0] = 1;
            //testArray[1] = 10;
            //testArray[2] = 22; 
            #endregion

            #region LIST
            //List<int> intList = new List<int>();
            //intList.Add(1);
            //intList.Add(2);
            //intList.Add(3);
            //intList.Add(13);
            //intList.Add(66);
            //intList.Add(71);
            //intList.AddRange(testArray);
            //intList.Insert(0, 11); // რომელ პოზიციაზე ჩამიმატოს ელემენტი
            //intList.InsertRange(0, new List<int>() { 999 });
            //intList.Add(777);
            //intList.Add(999);


            //intList.Remove(999);
            //intList.RemoveAt(intList.Count() - 1);
            //intList.RemoveRange(0, 2);
            //intList.Clear();

            //intList.TrimExcess();

            //var x = intList.Contains(999);
            //var x = intList.IndexOf(999);
            //var x = intList.LastIndexOf(999);

            //intList.Sort();
            //intList.Reverse();
            //intList.ToArray();



            //Console.WriteLine($"Count: {intList.Count}"); //რაოდენობა
            //Console.WriteLine($"Capacity: {intList.Capacity}"); //მოცულობა

            //List<int> intList2 = new() { 1, 2 };
            //var concated = intList.Concat(intList2);


            //for (int i = 0; i < intList.Capacity; i++)
            //{
            //    Console.WriteLine(intList[i]);
            //} 
            #endregion

            #region LINKEDLIST
            //var linked = new LinkedList<string>();
            //linked.AddFirst("Nika");
            //linked.AddLast("Giorgi");
            //linked.AddAfter(linked.First, "Ana");
            //linked.AddBefore(linked.Last, "Tamar");

            //linked.Remove("Nika");
            //linked.Remove(linked.Last);
            //linked.RemoveFirst();
            //linked.RemoveLast();
            //linked.Clear();

            //linked.Find("Nika");
            //linked.FindLast("Nika");
            //linked.Contains("Nika"); 
            #endregion

            #region DICTIONARY
            //Dictionary<string, int> ages = new()
            //{
            //    {"Nika",30 },
            //    {"Ana",25 },
            //    {"Giorgi",28 }
            //};

            //var a = ages.Count;
            //var a = ages.Keys;
            //var a = ages.Values;

            //ages.Add("Tornike", 28);
            //bool added = ages.TryAdd("Tornike", 29);
            //ages["Zaza"] = 55;

            //int anasAge = ages["Ana"];
            //ages.TryGetValue("Ana", out var age);
            //bool keyExists = ages.ContainsKey("Ana");
            //bool valueExists = ages.ContainsValue(22);

            //ages.Remove("Nika");
            //ages.Clear();


            //foreach (var keyValue in ages)
            //{
            //    Console.WriteLine($"{keyValue.Key} - {keyValue.Value}");
            //} 
            #endregion

            #region HASHSET
            //სიმრავლე
            //HashSet<int> intSet = new HashSet<int>();
            //intSet.Add(1);
            //intSet.Add(2);
            //intSet.Add(1);
            //intSet.Add(2);

            //HashSet<int> intSet2 = new HashSet<int>();

            //bool result = intSet2.IsSubsetOf(intSet);
            //bool result = intSet2.IsSupersetOf(intSet); 
            #endregion

            #region STACK
            //LIFO - Last In First Out

            //Stack<int> intStack = new();

            //intStack.Push(10);
            //intStack.Push(11);
            //intStack.Push(88);

            //intStack.Pop();
            //intStack.Pop();
            //intStack.Pop();

            //intStack.Peek(); // უბრალოდ ამოწმებს კოლექციაში არის თუ არა რაიმე ელემენტი დამატებლი 
            #endregion

            #region QUEUE
            ////FIFO - First In First Out
            //Queue<int> intQueue = new Queue<int>();
            //intQueue.Enqueue(11);
            //intQueue.Enqueue(12);
            //intQueue.Enqueue(13);

            //intQueue.Dequeue();
            //intQueue.Dequeue();
            //intQueue.Dequeue();

            //intQueue.Peek(); 
            #endregion

        }
    }
}
